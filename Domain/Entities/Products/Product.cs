using Domain.BaseContracts;
using Domain.Entities.Products.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public ProdNameVO Name { get; private set; }
        public DescriptionVO Description { get; private set; }
        public PriceVO Price { get; private set; }
        public StockVO Stock { get; private set; }
        public IEnumerable<Category> Categories { get; private set; }

        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }


        // Constructor privado para forzar el uso de Factories
        private Product() { }


        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Product.
        /// El Domain se defiende desde la primera línea (Fail Fast).
        /// </summary>
        public static Product Create
        (
            string rawName,
            string rawDescription,
            string rawPrice,
            string rawStock,
            IEnumerable<Category> categories = null
        )
        {
            return new Product
            {
                //EntityBase asigna automáticamente el ID
                Name = ProdNameVO.Create(rawName.ToUpper()),
                Description = DescriptionVO.Create((rawDescription ?? string.Empty).ToUpper()),
                Price = PriceVO.Create(rawPrice),
                Stock = StockVO.Create(rawStock),
                Categories = categories != null ? new List<Category>(categories) : new List<Category>(),
                Active = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        /// <summary>
        /// Reconstruye un Product EXISTENTE desde la persistencia (DB).
        /// </summary>
        public static Product Reconstitute
        (
            Guid id,
            string rawName,
            string rawDescription,
            string rawPrice,
            string rawStock,
            IEnumerable<Category> categories,
            bool active,
            DateTime createdAt,
            bool isDeleted
        )
        {
            return new Product
            {
                Id = Guid.Parse(id.ToString()),
                Name = ProdNameVO.Create(rawName),
                Description = DescriptionVO.Create((rawDescription ?? string.Empty)),
                Price = PriceVO.Create(rawPrice),
                Stock = StockVO.Create(rawStock),
                Categories = categories != null ? new List<Category>(categories) : new List<Category>(),
                Active = active,
                CreatedAt = createdAt,
                IsDeleted = isDeleted
            };
        }


        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---
        public void MarkAsDeleted()
        {
            if (IsDeleted) return; // Idempotencia
            IsDeleted = true;
            Active = false; // Regla de negocio: un producto borrado lógicamente no puede estar activo
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un producto que ha sido eliminado.");
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public override string ToString()
            => $"{Name.Value} ({Price.Value}) - Stock: {Stock.Value}";
    }
}