using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models.DTO
{
    public class EditCategoryDTO
    {
        public int? ID { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        public int? ParentID { get; set; }

        public EditCategoryDTO()
        {
        }

        public EditCategoryDTO(int? id, string name, int? parentId)
        {
            ID = id;
            Name = name;
            ParentID = parentId;
        }
    }
}
