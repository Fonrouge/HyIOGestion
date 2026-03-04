using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class EmployeeDTO : IDto
    {
        [Browsable(false)]
        public Guid Id = Guid.NewGuid();
        public string Email { get; set; }
        public string FileNumber { get; set; } // El N° 'Legajo'
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string NationalId { get; set; } // DNI o Cédula
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public bool   Active { get; set; } = true; // Por defecto, un nuevo empleado está activo
        public string DVH { get; set; } // Dígito Verificador Horizontal

        public override string ToString() => $"{LastName}, {FirstName} Legajo: ({FileNumber})";
    }
}
