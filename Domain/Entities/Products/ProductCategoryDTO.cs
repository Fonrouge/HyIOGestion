using Domain.Contracts;
using System;


namespace Domain.Entities.Products
{
    public class ProductCategoryDTO : IIntegrityCheckable
    {
        public Guid IdProduct { get; set; }
        public Guid IdCategory { get; set; }
        public DvhVo DVH { get; set; }

        private ProductCategoryDTO() { }

        public static ProductCategoryDTO Create
        (
            Guid idProduct,
            Guid idCategory
        )
        {
            if (idProduct == null) 
                throw new ArgumentNullException("idPadre cannot be null");
            if (idCategory == null) 
                throw new ArgumentNullException("idChild cannot be null");

            return new ProductCategoryDTO()
            {
                IdProduct = idProduct,
                IdCategory = idCategory,
                //DVH = DvhVo.Create(dvh) ----> DVH sólo se puede calcular a partir de una entidad ya creada
            };
        }

        public static ProductCategoryDTO Reconstitute
        (
            Guid idProduct,
            Guid idCategory,
            string dvh
        )
        {
            if (idProduct == Guid.Empty) 
                throw new ArgumentNullException("idPadre cannot be null");
            if (idCategory == Guid.Empty) 
                throw new ArgumentNullException("idChild cannot be null");
            if (dvh == null) 
                throw new ArgumentNullException("dvh cannot be null");

            return new ProductCategoryDTO()
            {
                IdProduct = idProduct,
                IdCategory = idCategory,
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
