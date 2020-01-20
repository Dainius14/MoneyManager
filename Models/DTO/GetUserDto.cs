using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyManager.Models.DTO
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;

        public GetUserDto()
        {
        }

        public GetUserDto(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
