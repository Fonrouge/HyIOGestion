using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class EmployeeDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; } 


        public string FileNumber { get; set; } // N° Legajo
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }

        public bool   IsDeleted { get; set; }

        public bool Active { get; set; } = true; 

        [Browsable(false)]
        public string DVH { get; set; } 

        public override string ToString() => $"{LastName}, {FirstName} Legajo: ({FileNumber})";
    }
}
