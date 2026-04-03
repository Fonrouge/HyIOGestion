using Domain.Contracts;
using Domain.Entities.Sales.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Domain.Entities
{
    public class Sale : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public SaleDateVO Date { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid EmployeeId { get; private set; }
        public TotalAmountVO TotalAmount { get; private set; }
        public IEnumerable<SaleDetail> Items { get; private set; }

        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DvhVo DVH { get; private set; }
        public string InvoiceNumber { get; private set; } = "A-0001-000005";

        // Constructor privado para forzar el uso de Factories
        private Sale() { }

        /// <summary>
        /// ÚNICO punto de creación para una NUEVA Venta.
        /// TotalAmount se calcula automáticamente (invariante del agregado).
        /// </summary>
        public static Sale Create
        (
            Guid clientId,
            Guid employeeId,
            IEnumerable<SaleDetail> items,
            string DVH = null
        )
        {
            if (items == null || !items.Any())
                throw new InvalidOperationException("Una venta debe contener al menos un detalle.");

            // 1. En Sale.Create (después de asignar Items)
            var sale = new Sale
            {
                Date = SaleDateVO.Create(DateTime.UtcNow),
                ClientId = clientId,
                EmployeeId = employeeId,
                Items = new List<SaleDetail>(items),
                Active = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                DVH = null
            };


            foreach (var detail in sale.Items)
            {
                detail.AssignToSale(sale.Id);
            }

            sale.CalculateTotal();
            return sale;

        }

        /// <summary>
        /// Reconstruye una Sale EXISTENTE desde la persistencia (DB).
        /// </summary>
        public static Sale Reconstitute
        (
            Guid id,
            DateTime date,
            Guid clientId,
            Guid employeeId,
            decimal totalAmountRaw,
            IEnumerable<SaleDetail> items,
            bool active,
            DateTime createdAt,
            bool isDeleted,
            string dvh = null
        )
        {
            return new Sale
            {
                Id = id,
                Date = SaleDateVO.Create(date),
                ClientId = clientId,
                EmployeeId = employeeId,
                TotalAmount = TotalAmountVO.Create(totalAmountRaw),
                Items = items != null ? new List<SaleDetail>(items) : new List<SaleDetail>(),
                Active = active,
                CreatedAt = createdAt,
                IsDeleted = isDeleted,
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : DvhVo.Create(dvh)
            };
        }

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            var culture = CultureInfo.InvariantCulture;

            var headerData = string.Join("|",
                Id.ToString(),
                Date.Value.ToString("yyyyMMddHHmmss", culture),
                ClientId.ToString(),
                EmployeeId.ToString(),
                InvoiceNumber.ToUpper(culture),
                TotalAmount.Value.ToString("F2", culture),
                Active ? "1" : "0",
                IsDeleted ? "1" : "0"
            );

            return headerData;
        }

        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---
        private void CalculateTotal()
        {
            // Se asume que SaleDetail tiene .Subtotal (decimal)
            var subtotalSum = Items.Sum(detail => detail.Subtotal);
            TotalAmount = TotalAmountVO.Create(subtotalSum);
        }

        public void AddItem(SaleDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));

            if (!Active || IsDeleted)
                throw new InvalidOperationException("No se pueden modificar ventas inactivas o eliminadas.");

            var list = Items.ToList();
            list.Add(detail);
            Items = list;
            CalculateTotal();
        }
        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh ?? string.Empty);

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            Active = false;

            foreach (var detail in Items)
            {
                detail.MarkAsDeleted();
            }
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar una venta que ha sido eliminada.");
            Active = true;
        }

        public void Deactivate() => Active = false;

        public override string ToString() => $"Venta #{Id} - {Date.Value:dd/MM/yyyy} - Cliente: {ClientId} - Total: {TotalAmount.Value:C2}";
    }
}