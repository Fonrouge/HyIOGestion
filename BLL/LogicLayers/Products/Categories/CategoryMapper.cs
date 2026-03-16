using Domain.Entities;
using System;
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
                DVH = entity.DVH?.Value ?? string.Empty
            };
        }

        public static Category ToEntity(CategoryDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                return Category.Create(dto.Name, dto.Description);
            }

            return Category.Reconstitute(dto.Id, dto.Name, dto.Description, dto.DVH);
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