using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyManager.Models.DTO
{
    public class EditUserDto
    {
        public int? Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }

        public EditUserDto()
        {
        }

        public EditUserDto(int? id, string email)
        {
            Id = id;
            Email = email;
        }

        public EditUserDto(int? id, string email, string password)
            : this(id, email)
        {
            Password = password;
        }
    }
}
