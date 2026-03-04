using BLL.LogicLayers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers // Ajusta el namespace según tu proyecto
{
    public static class ProductMapper
    {
        public static ProductDTO ToDto(Product entity)
        {
            if (entity == null) return null;

            return new ProductDTO()
            {
                Id = entity.Id,
                Name = entity.Name.Value,
                Description = entity.Description.Value,

                // El DTO espera tipos numéricos. Extraemos el Value del VO.
                // (Nota: Asumo que casteamos Stock a int si en el VO lo dejaste como decimal)
                Price = entity.Price.Value,
                Stock = (int)entity.Stock.Value,

                Categories = CategoryMapper.ToListDTO(entity.Categories).ToList(),

                IsActive = entity.Active,     // Ojo a la diferencia de nombres (Active vs IsActive)
                CreatedAt = entity.CreatedAt
                // IsDeleted no está en el DTO, así que no se mapea hacia afuera.
            };
        }

        public static Product ToEntity(ProductDTO dto)
        {
            if (dto == null) return null;

            // Si el ID es vacío, es un producto nuevo
            if (dto.Id == Guid.Empty)
            {
                return Product.Create(
                    dto.Name,
                    dto.Description,
                    dto.Price.ToString(), // El Factory del Domain espera un string crudo
                    dto.Stock.ToString(), // El Factory del Domain espera un string crudo
                    CategoryMapper.ToListEntity(dto.Categories)
                );
            }

            // Si ya tiene ID, reconstruimos el objeto existente
            return Product.Reconstitute(
                dto.Id,
                dto.Name,
                dto.Description,
                dto.Price.ToString(),
                dto.Stock.ToString(),
                CategoryMapper.ToListEntity(dto.Categories),
                dto.IsActive,
                dto.CreatedAt,
                false // Al no existir en el DTO, asumimos false, o deberías agregarlo al DTO si viaja desde la UI.
            );
        }

        public static IEnumerable<ProductDTO> ToListDTO(IEnumerable<Product> entities)
        {
            return entities?.Select(ToDto) ?? Enumerable.Empty<ProductDTO>();
        }

        public static IEnumerable<Product> ToListEntity(IEnumerable<ProductDTO> dtos)
        {
            return dtos?.Select(ToEntity) ?? Enumerable.Empty<Product>();
        }
    }
}