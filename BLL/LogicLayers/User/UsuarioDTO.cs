using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.DTOs
{
    public class UsuarioDTO : IDto
    {
        [Browsable(false)]
        public Guid Id { get; set; } = Guid.Empty;

        public string Username { get; set; }

        [Browsable(false)] // No queremos mostrar el hash en las grillas
        public string Password { get; set; }

        public string LanguageCode { get; set; }

        [Browsable(false)]
        public string DVH { get; set; }

        
        // Relación con el empleado
        public EmployeeDTO EmployeeDTO { get; set; }

        [Browsable(false)]
        public Guid EmployeeId => EmployeeDTO?.Id ?? Guid.Empty;

        
        // Propiedades de estado

        [Browsable(false)]
        public bool IsDeleted { get; set; }

        // IMPORTANTE: En el DTO, los permisos suelen ser una lista de strings (códigos)
        // para que la UI sepa qué botones habilitar/deshabilitar de forma simple.
        public List<string> Permisos { get; set; } = new List<string>();

        public override string ToString() => Username;
    }
}