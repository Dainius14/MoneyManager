using System;

namespace MoneyManager.Models.DTO
{
    public class GetCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public GetCategoryDTO? Parent { get; set; }
        public string CreatedAt { get; set; } = null!;
        public string? UpdatedAt { get; set; }

        public GetCategoryDTO()
        {
        }

        public GetCategoryDTO(int id, string name, GetCategoryDTO? parent, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Name = name;
            Parent = parent;
            CreatedAt = createdAt.ToISOString();
            UpdatedAt = updatedAt?.ToISOString();
        }
    }
}
