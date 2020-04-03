using MoneyManager.Core.Repositories;
using MoneyManager.Models.Domain;
using MoneyManager.Core.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.ViewModels;

namespace MoneyManager.Core.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<User> Create(RegisterUserVm registerVm)
        {
            if (string.IsNullOrWhiteSpace(registerVm.Email))
            {
                throw new Exception("Email is required");
            }
            if (string.IsNullOrWhiteSpace(registerVm.Password))
            {
                throw new Exception("Password is required");
            }

            CreatePasswordHash(registerVm.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = registerVm.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
            };

            int userId = await _uow.UserRepo.InsertAsync(user);
            _uow.Commit();
            user.Id = userId;
            return user;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _uow.UserRepo.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("Cant find user with given email");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash!, user.PasswordSalt!))
            {
                throw new BadUserPasswordException("Wrong password");
            }

            return user;
        }

        public async Task SaveRefreshToken(int userId, string token)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expiresAt = issuedAt.AddDays(7);
            var refreshToken = new RefreshToken(userId, token, issuedAt, expiresAt);
            await _uow.RefreshTokenRepo.InsertAsync(refreshToken);
        }

        public async Task<bool> IsRefreshTokenValid(string refreshToken)
        {
            var refreshTokenEntry = await _uow.RefreshTokenRepo.GetAsync(refreshToken);
            return refreshTokenEntry != null && refreshTokenEntry.IsValid
                && refreshTokenEntry.ExpiresAt >= DateTime.UtcNow;
        }

        public async Task InvalidateRefreshToken(string refreshToken)
        {
            var refreshTokenEntry = await _uow.RefreshTokenRepo.GetAsync(refreshToken);
            refreshTokenEntry.IsValid = false;
            await _uow.RefreshTokenRepo.UpdateAsync(refreshTokenEntry);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _uow.UserRepo.GetAllAsync();
        }

        public async Task<User> GetOne(int id)
        {
            return await _uow.UserRepo.GetAsync(id);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
