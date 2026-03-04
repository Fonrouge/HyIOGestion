using Domain.BaseContracts;
using Domain.Entities.Employees.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Employee : EntityBase, ISoftDeletable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public FileNumberVO FileNumber { get; private set; }
        public FirstNameVO FirstName { get; private set; }
        public LastNameVO LastName { get; private set; }
        public NationalIdVO NationalId { get; private set; }
        public EmployeeEmailVO Email { get; private set; }
        public EmployeePhoneNumberVO PhoneNumber { get; private set; }
        public HomeAddressVO HomeAddress { get; private set; }

        
        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public string DVH { get; private set; }
        public bool Active { get; private set; }
        public bool IsDeleted { get; private set; }

        
        // Constructor privado para forzar el uso de Factories
        private Employee() { }


        /// <summary>
        /// ÚNICO punto de creación para un NUEVO Employee.
        /// El Domain se defiende desde la primera línea (Fail Fast).
        /// </summary>
        public static Employee Create
        (
            string rawFileNumber,
            string rawFirstName,
            string rawLastName,
            string rawNationalId,
            string rawEmail,
            string rawPhoneNumber,
            string rawHomeAddress
        )
        {
            return new Employee
            {
                //EntityBase asigna automáticamente el ID
                FileNumber = FileNumberVO.Create(rawFileNumber.ToUpper()),
                FirstName = FirstNameVO.Create(rawFirstName.ToUpper()),
                LastName = LastNameVO.Create(rawLastName.ToUpper()),
                NationalId = NationalIdVO.Create(rawNationalId.ToUpper()),
                Email = EmployeeEmailVO.Create(rawEmail.ToUpper()),
                PhoneNumber = EmployeePhoneNumberVO.Create(rawPhoneNumber.ToUpper()),
                HomeAddress = HomeAddressVO.Create(rawHomeAddress.ToUpper()),
                DVH = string.Empty, // Se calculará después de instanciarlo
                Active = true,
                IsDeleted = false
            };
        }

        /// <summary>
        /// Reconstruye un Employee EXISTENTE desde la persistencia (DB).
        /// </summary>
        public static Employee Reconstitute
        (
            Guid id,
            string rawFileNumber,
            string rawFirstName,
            string rawLastName,
            string rawNationalId,
            string rawEmail,
            string rawPhoneNumber,
            string rawHomeAddress,
            string dvh,
            bool active,
            bool isDeleted
        )
        {
            return new Employee
            {
                Id = Guid.Parse(id.ToString().ToUpper()),
                FileNumber = FileNumberVO.Create(rawFileNumber.ToUpper()),
                FirstName = FirstNameVO.Create(rawFirstName.ToUpper()),
                LastName = LastNameVO.Create(rawLastName.ToUpper()),
                NationalId = NationalIdVO.Create(rawNationalId.ToUpper()),
                Email = EmployeeEmailVO.Create(rawEmail.ToUpper()),
                PhoneNumber = EmployeePhoneNumberVO.Create(rawPhoneNumber.ToUpper()),
                HomeAddress = HomeAddressVO.Create(rawHomeAddress.ToUpper()),
                
                DVH = dvh ?? string.Empty,
                Active = active,
                IsDeleted = isDeleted
            };
        }

        // --- COMPORTAMIENTO (Transiciones de Estado Seguras) ---

        public void MarkAsDeleted()
        {
            if (IsDeleted) return; // Idempotencia
            IsDeleted = true;
            Active = false; // Regla de negocio: un empleado borrado lógicamente no puede estar activo
        }

        public void Activate()
        {
            if (IsDeleted)
                throw new InvalidOperationException("No se puede activar un empleado que ha sido eliminado.");

            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        public void UpdateDVH(string newDvh)
        {
            if (string.IsNullOrWhiteSpace(newDvh))
                throw new ArgumentException("El DVH no puede estar vacío.", nameof(newDvh));

            DVH = newDvh;
        }

        public override string ToString()
            => $"{LastName.Value}, {FirstName.Value} ({FileNumber.Value})";
    }
}