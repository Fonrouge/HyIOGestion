using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers
{
    public static class CategoryMapper
    {
        public static CategoryDTO ToDto(Category entity)
        {
            if (entity == null) return null;

            return new CategoryDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
            };
        }

        public static Category ToEntity(CategoryDTO dto)
        {
            if (dto == null) return null;

            return new Category()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
            };
        }

        public static IEnumerable<CategoryDTO> ToListDTO(IEnumerable<Category> entities)
        {
            if (entities == null) return Enumerable.Empty<CategoryDTO>();

            // Usamos LINQ para iterar y transformar cada elemento usando el método individual
            return entities.Select(entity => ToDto(entity));
        }

        public static IEnumerable<Category> ToListEntity(IEnumerable<CategoryDTO> dtos)
        {
            if (dtos == null) return Enumerable.Empty<Category>();

            // Usamos LINQ para iterar y transformar cada elemento usando el método individual
            return dtos.Select(dto => ToEntity(dto));
        }
    }
}