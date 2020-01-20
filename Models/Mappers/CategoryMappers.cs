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
                Id = dto.Id,
                Name = dto.Name,
                ParentId = dto.ParentId,
            };
        }

        public static Category ToDomainModel(this GetCategoryDTO dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                ParentId = dto.Parent?.Id,
                Parent = dto.Parent?.ToDomainModel(),
            };
        }

        public static GetCategoryDTO ToGetCategoryDTO(this Category domain)
        {
            return new GetCategoryDTO((int)domain.Id!, domain.Name, domain.Parent?.ToGetCategoryDTO(), domain.CreatedAt, domain.UpdatedAt);
        }

        public static EditCategoryDTO ToEditCategoryDTO(this Category domain)
        {
            return new EditCategoryDTO(domain.Id, domain.Name, domain.Parent?.Id);
        }
    }
}
