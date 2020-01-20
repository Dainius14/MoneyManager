using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class EditCategoryDTO
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }

        public EditCategoryDTO()
        {
        }

        public EditCategoryDTO(int? id, string name, int? parentId)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
        }
    }
}
