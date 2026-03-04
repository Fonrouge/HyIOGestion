using System;

namespace BLL.DTOs
{
    public class EmployeeDTO
    {
        public Guid Id = Guid.NewGuid();
        public string FileNumber     { get; set; } // El N° 'Legajo'
        public string FirstName     { get; set; }
        public string LastName       { get; set; }
        public string NationalId    { get; set; } // DNI o Cédula
        public string Email         { get; set; }
        public string PhoneNumber    { get; set; }
        public string HomeAddress     { get; set; }
        public bool   Active            { get; set; }
        public string DVH               { get; set; } // Dígito Verificador Horizontal

        public override string ToString() => $"{LastName}, {FirstName} ({FileNumber})";
    }
}
