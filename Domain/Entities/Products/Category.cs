using Domain.BaseContracts;
using System;

namespace Domain.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DvhVo DVH { get; private set; }

        // Constructor privado para los Factory Methods
        private Category() { }

        /// <summary>
        /// Para crear una categoría totalmente NUEVA
        /// </summary>
        public static Category Create(string name, string description)
        {
            return new Category
            {
                // El Id se genera en EntityBase
                Name = name?.Trim().ToUpper() ?? string.Empty,
                Description = description?.Trim().ToUpper() ?? string.Empty,
                DVH = null
            };
        }

        /// <summary>
        /// Para hidratar una categoría desde la base de datos
        /// </summary>
        public static Category Reconstitute(Guid id, string name, string description, string dvh)
        {
            return new Category
            {
                Id = id,
                Name = name,
                Description = description,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null
            };
        }

        public void UpdateDVH(string dvh)
        {
            DVH = DvhVo.Create(dvh ?? string.Empty);
        }
    }
}