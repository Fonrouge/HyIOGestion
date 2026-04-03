using Domain.Contracts;
using Domain.Entities.Employees.ValueObjects;
using System;

namespace Domain.Entities
{
    public class Employee : EntityBase, ISoftDeletable, IIntegrityCheckable
    {
        // --- PROPIEDADES DE DOMINIO (Rich Domain Model) ---
        public FirstNameVO FirstName { get; private set; }
        public LastNameVO LastName { get; private set; }
        public NationalIdVO NationalId { get; private set; }

        public FileNumberVO FileNumber { get; private set; }
        public EmployeeEmailVO Email { get; private set; }
        public EmployeePhoneNumberVO PhoneNumber { get; private set; }
        public HomeAddressVO HomeAddress { get; private set; }


        // --- CAMPOS TÉCNICOS Y DE ESTADO ---
        public DvhVo DVH { get; private set; }
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
                DVH = null, // Se calculará después de instanciarlo
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
            bool isDeleted)
        {
            var emp = new Employee()
            {
                Id = id,
                FileNumber = FileNumberVO.Create(rawFileNumber?.ToUpper() ?? string.Empty),
                FirstName = FirstNameVO.Create(rawFirstName?.ToUpper() ?? string.Empty),
                LastName = LastNameVO.Create(rawLastName?.ToUpper() ?? string.Empty),
                NationalId = NationalIdVO.Create(rawNationalId?.ToUpper() ?? string.Empty),
                Email = EmployeeEmailVO.Create(rawEmail?.ToUpper() ?? string.Empty),
                PhoneNumber = EmployeePhoneNumberVO.Create(rawPhoneNumber?.ToUpper() ?? string.Empty),
                HomeAddress = HomeAddressVO.Create(rawHomeAddress?.ToUpper() ?? string.Empty),
                DVH = !string.IsNullOrEmpty(dvh) ? DvhVo.Create(dvh) : null,
                Active = active,
                IsDeleted = isDeleted
            };

            return emp;
        }

        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Incluye los datos de identidad y contacto para asegurar que no sean alterados externamente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Usamos cultura invariante para normalizar la representación de Guids y tipos básicos
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                Id.ToString(),
                FileNumber.Value,    
                FirstName.Value,     
                LastName.Value,      
                NationalId.Value,    
                Email.Value,         
                PhoneNumber.Value,   
                HomeAddress.Value,   
                Active ? "1" : "0",
                IsDeleted ? "1" : "0"
            );
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

            DVH = DvhVo.Create(newDvh);
        }

        public override string ToString()
            => $"{LastName.Value}, {FirstName.Value} ({FileNumber.Value})";
    }
}