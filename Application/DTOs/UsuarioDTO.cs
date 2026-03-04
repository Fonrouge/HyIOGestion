using Domain.Entities.Permisos.Abstracts;
using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class UsuarioDTO
    {
        public Guid Id = Guid.NewGuid();
        public string Username { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string DVH { get; set; }
        public EmployeeDTO EmployeeDTO { get; set; }

        public List<Componente> Permisos { get; set; } = new List<Componente>();

    }
}
