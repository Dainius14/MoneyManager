using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.Mappers
{
    public static class CategoryMappers
    {

        public static Category ToDomainModel(this EditCategoryDTO dto)
        {
            return new Category
            {
                ID = dto.ID,
                Name = dto.Name,
                ParentID = dto.ParentID,
            };
        }

        public static Category ToDomainModel(this GetCategoryDTO dto)
        {
            return new Category
            {
                ID = dto.ID,
                Name = dto.Name,
                ParentID = dto.Parent?.ID,
                Parent = dto.Parent?.ToDomainModel(),
            };
        }

        public static GetCategoryDTO ToGetCategoryDTO(this Category domain)
        {
            return new GetCategoryDTO((int)domain.ID!, domain.Name, domain.Parent?.ToGetCategoryDTO(), domain.CreatedAt, domain.UpdatedAt);
        }

        public static EditCategoryDTO ToEditCategoryDTO(this Category domain)
        {
            return new EditCategoryDTO(domain.ID, domain.Name, domain.Parent?.ID);
        }
    }
}
