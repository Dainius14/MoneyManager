using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyManager.Models.Domain
{
    public class User
    {
        [Column]
        public int? Id { get; set; }

        [Column]
        public string Email { get; set; } = null!;

        public string? Password { get; set; }

        [Column]
        public byte[]? PasswordHash { get; set; }

        [Column]
        public byte[]? PasswordSalt { get; set; }

        [Column]
        public DateTime CreatedAt { get; set; }

        [Column]
        public DateTime? UpdatedAt { get; set; }

        public User()
        {
        }

        public User(int? id, string email)
        {
            Id = id;
            Email = email;
        }

        public User(int? id, string email, string? password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public User(int? id, string email, string password, byte[] passwordHash, byte[] passwordSalt)
            : this(id, email, password)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
