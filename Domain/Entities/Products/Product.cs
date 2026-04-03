using Domain.Contracts;
using Domain.Entities.Products.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Entities
{
    public class Product : EntityBase, ISoftDeletable, IIntegrityCheckable
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
        public DvhVo DVH { get; private set; }


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
            decimal rawPrice,
            decimal rawStock,
            IEnumerable<Category> categories = null,
            string dvh = null
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
                IsDeleted = false,
                DVH = null
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
            decimal rawPrice,
            decimal rawStock,
            IEnumerable<Category> categories,
            bool active,
            DateTime createdAt,
            bool isDeleted,
            string dvh
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
                IsDeleted = isDeleted,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
            };
        }


        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---
        public void MarkAsDeleted()
        {
            if (IsDeleted) return;
            IsDeleted = true;
            Active = false; 
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un producto que ha sido eliminado.");
            Active = true;
        }

        public void Deactivate() => Active = false;

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh);

        public string GetDvhSerialization()
        {
            var culture = CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                Name.Value,
                Description.Value,
                Price.Value.ToString("F2", culture),
                Stock.Value.ToString("F3", culture),
                Active ? "1" : "0",
                CreatedAt.ToString("yyyyMMddHHmmss", culture),
                IsDeleted ? "1" : "0"
            );
        }

        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---

        /// <summary>
        /// Reduce el stock. Al usar decimal, soportamos ventas por peso, volumen o fracción.
        /// </summary>
        public void ReduceStock(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.");

            // Acceso directo al valor decimal del VO (sin parseos lentos)
            decimal currentStock = this.Stock.Value;

            if (currentStock < quantity)
                throw new InvalidOperationException($"Stock insuficiente para {Name.Value}. Disponible: {currentStock}, Solicitado: {quantity}");

            decimal newStockValue = currentStock - quantity;

            // El VO se encarga de validar que el resultado final sea coherente (ej: >= 0)
            this.Stock = StockVO.Create(newStockValue);
        }

        /// <summary>
        /// Incrementa el stock por ingresos de mercadería o devoluciones.
        /// </summary>
        public void AddStock(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a sumar debe ser mayor a cero.");

            decimal currentStock = this.Stock.Value;
            decimal newStockValue = currentStock + quantity;

            this.Stock = StockVO.Create(newStockValue);
        }

        public override string ToString()
            => $"{Name.Value} ({Price.Value}) - Stock: {Stock.Value}";
    }
}