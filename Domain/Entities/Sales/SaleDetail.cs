using Domain.Contracts;
using Domain.Entities.Sales.ValueObjects;
using System;

namespace Domain.Entities
{
    public class SaleDetail : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public QuantityVO Quantity { get; private set; }
        public UnitPriceVO UnitPrice { get; private set; }
        public decimal Subtotal { get; private set; }   // Calculado automáticamente

        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public bool IsDeleted { get; private set; }
        public DvhVo DVH { get; private set; }




        // Constructor privado para forzar el uso de Factories
        private SaleDetail()
        {
            SaleId = Guid.Empty; // Hasta que el agregado raíz (Sale) lo asigne
        }

        /// <summary>
        /// ÚNICO punto de creación para un NUEVO SaleDetail.
        /// NO recibe SaleId porque quien lo asigna es la Venta (regla DDD de agregado).
        /// </summary>
        public static SaleDetail Create
        (
            Guid productId,
            decimal rawQuantity,
            decimal rawUnitPrice
        )
        {
            var detail = new SaleDetail
            {
                ProductId = productId,
                Quantity = QuantityVO.Create(rawQuantity),
                UnitPrice = UnitPriceVO.Create(rawUnitPrice)
            };

            detail.CalculateSubtotal();
            return detail;
        }

        /// <summary>
        /// Reconstruye un SaleDetail EXISTENTE desde la persistencia (DB).
        /// </summary>
        public static SaleDetail Reconstitute
        (
            Guid id,
            Guid saleId,
            Guid productId,
            decimal quantityRaw,
            decimal unitPriceRaw,
            decimal subtotal,
            bool isDeleted,
            string dvh = null
        )
        {
            return new SaleDetail
            {
                Id = id,
                SaleId = saleId,
                ProductId = productId,
                Quantity = QuantityVO.Create(quantityRaw),
                UnitPrice = UnitPriceVO.Create(unitPriceRaw),
                Subtotal = subtotal,
                IsDeleted = isDeleted,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
            };
        }

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Usamos cultura invariante para normalizar decimales y Guids
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                SaleId.ToString(),       // Crucial para evitar "huérfanos" o traslados entre ventas
                ProductId.ToString(),
                Quantity.Value.ToString("F3", culture),  // F3 por si vendes por peso/volumen
                UnitPrice.Value.ToString("F2", culture), // F2 estándar moneda
                Subtotal.ToString("F2", culture),
                IsDeleted ? "1" : "0"
            );
        }


        // --- COMPORTAMIENTO ---
        private void CalculateSubtotal() => Subtotal = Quantity.Value * UnitPrice.Value;

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh ?? string.Empty);

        /// <summary>
        /// Solo el agregado raíz (Sale) puede asignar el SaleId (protección DDD).
        /// </summary>
        internal void AssignToSale(Guid saleId)
        {
            if (saleId == Guid.Empty)
                throw new ArgumentException("El ID de la venta no puede estar vacío.");

            if (SaleId != Guid.Empty && SaleId != saleId)
                throw new InvalidOperationException("Este detalle ya pertenece a otra venta.");

            SaleId = saleId;
        }

        public override string ToString() => $"{Quantity.Value} x {UnitPrice.Value:C2} = {Subtotal:C2} (Producto: {ProductId})";
    }
}