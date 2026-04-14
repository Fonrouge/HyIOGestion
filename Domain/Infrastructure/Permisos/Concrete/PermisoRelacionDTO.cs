using Domain.Contracts;
using System;


namespace Domain.Entities.Permisos.Concrete
{
    public class PermisoRelacionDTO : IIntegrityCheckable
    {
        public Guid IdPadre { get; set; }
        public Guid IdHijo { get; set; }
        public DvhVo DVH { get; set; }

        public static PermisoRelacionDTO Create
        (
            Guid idPadre,
            Guid idHijo,
            string dvh
        )
        {
            return new PermisoRelacionDTO()
            {
                IdPadre = idPadre,
                IdHijo = idHijo,
                DVH = DvhVo.Create(dvh)
            };
        }

        public static PermisoRelacionDTO Reconstitute
        (
            Guid idPadre,
            Guid idHijo,
            string dvh
        )
        {
            return new PermisoRelacionDTO()
            {
                IdPadre = idPadre,
                IdHijo = idHijo,
                DVH = DvhVo.Create(dvh)
            };
        }

        public void UpdateDVH(string dvh) => DVH = DvhVo.Create(dvh ?? string.Empty);


        /// <summary>
        /// Genera la cadena de serialización para el cálculo del Dígito Verificador Horizontal.
        /// Protege la integridad de los datos filiatorios y de contacto del cliente.
        /// </summary>
        public string GetDvhSerialization()
        {
            // Mantenemos la consistencia con cultura invariante para el ID (Guid)
            var culture = System.Globalization.CultureInfo.InvariantCulture;

            return string.Join("|",
                IdPadre.ToString(),
                IdHijo.ToString()
            );
        }
    }
}
