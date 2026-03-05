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
                Price = entity.Price.Value,
                Stock = (int)entity.Stock.Value,
                Categories = CategoryMapper.ToListDTO(entity.Categories).ToList(),
                IsActive = entity.Active,     
                CreatedAt = entity.CreatedAt
            };
        }

        public static Product ToEntity(ProductDTO dto)
        {
            if (dto == null) return null;

            if (dto.Id == Guid.Empty)
            {
                return Product.Create(
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
                false 
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