using Domain.Contracts;
using System;

namespace Domain.Entities
{
    public class ProductCategoryRelacionDTO : IIntegrityCheckable
    {
        public Guid IdProduct { get; set; }
        public Guid IdCategory { get; set; }
        public DvhVo DVH { get; set; }

        public static ProductCategoryRelacionDTO Create
        (
            Guid idProduct,
            Guid idCateogry,
            string dvh
        )
        {
            return new ProductCategoryRelacionDTO()
            {
                IdProduct = idProduct,
                IdCategory = idCateogry,
                DVH = DvhVo.Create(dvh)
            };
        }

        public static ProductCategoryRelacionDTO Reconstitute
        (
            Guid idProduct,
            Guid idCateogry,
            string dvh
        )
        {
            return new ProductCategoryRelacionDTO()
            {
                IdProduct = idProduct,
                IdCategory = idCateogry,
                DVH = DvhVo.Create(dvh)
            };
        }

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh ?? string.Empty);

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Mantenemos la consistencia con cultura invariante para el ID (Guid)
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                IdProduct.ToString(),
                IdCategory.ToString()
            );
        }
    }
}
