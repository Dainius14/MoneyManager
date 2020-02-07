using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models.Domain
{
    public class RefreshToken
    {
        [Column]
        public int? Id { get; set; }

        [Column]
        public int UserId { get; set; }

        [Column]
        public string Token { get; set; } = null!;

        [Column]
        public bool IsValid { get; set; } = true;

        [Column]
        public DateTime IssuedAt { get; set; }
        
        [Column]
        public DateTime ExpiresAt { get; set; }


        public User User { get; set; } = null!;


        public RefreshToken()
        {
        }

        public RefreshToken(int userId, string token, DateTime issuedAt, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }
    }
}
