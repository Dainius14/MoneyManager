using System;

namespace MoneyManager.Models.DTO
{
    public class GetCategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public GetCategoryDTO? Parent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetCategoryDTO()
        {
        }

        public GetCategoryDTO(int id, string name, GetCategoryDTO? parent, DateTime createdAt, DateTime? updatedAt)
        {
            ID = id;
            Name = name;
            Parent = parent;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
