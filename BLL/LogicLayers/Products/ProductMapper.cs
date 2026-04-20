using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.LogicLayers 
{
    public static class ProductMapper
    {
        public static ProductDTO ToDto(Product entity)
        {
            if (entity == null) return null;

            return new ProductDTO()
            {
                Id = entity.Id,
                Name = (string)entity.Name.Value,
                Description = (string)entity.Description.Value,
                Price = entity.Price.Value,
                Stock = entity.Stock.Value, // Sin casteo, mantenemos precisión
                Categories = CategoryMapper.ToListDTO(entity.Categories).ToList(),
                IsActive = entity.Active,
                CreatedAt = entity.CreatedAt,
                IsDeleted = entity.IsDeleted,
                DVH = (string)(entity.DVH?.Value ?? string.Empty) // Mapeamos el Value Object a string
            };
        }

        public static Product ToEntity(ProductDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                return Product.Create
                (
                    dto.Name,
                    dto.Description,
                    dto.Price,
                    dto.Stock,
                    CategoryMapper.ToListEntity(dto.Categories)
                );
            }

            return Product.Reconstitute(
                dto.Id,
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Stock,
                CategoryMapper.ToListEntity(dto.Categories),
                dto.IsActive,
                dto.CreatedAt,
                dto.IsDeleted,
                dto.DVH
            );
        }
        public static IEnumerable<ProductDTO> ToListDTO(IEnumerable<Product> entities) => entities?.Select(ToDto) ?? Enumerable.Empty<ProductDTO>();

        public static IEnumerable<Product> ToListEntity(IEnumerable<ProductDTO> dtos) => dtos?.Select(ToEntity) ?? Enumerable.Empty<Product>();
    }
}