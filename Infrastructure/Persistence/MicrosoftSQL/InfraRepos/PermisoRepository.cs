using Domain.Entities;
using Domain.Entities.Permisos.Abstracts;
using Domain.Entities.Permisos.Concrete;
using Domain.Repositories;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DAL.Persistence.MicrosoftSQL
{
    public class PermisoRepository : IPermisoRepository
    {
        private SqlTransaction _currentTransaction;
        private readonly IApplicationSettings _appSettings;

        public PermisoRepository(IApplicationSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SetTransaction(object transaction) => _currentTransaction = (SqlTransaction)transaction;

        public async Task<List<PermisoComponente>> GetPermissionsByUserAsync(Guid userId)
        {
            var lista = new List<PermisoComponente>();
            SqlConnection conn = _currentTransaction?.Connection ?? new SqlConnection(_appSettings.SecurityConnection);
            bool isExternalConn = _currentTransaction?.Connection != null;

            try
            {
                // Sincronizado con la imagen: Id, Nombre, Permiso, EsFamilia, DVH
                string query = @"SELECT p.Id, p.Nombre, p.Permiso, p.EsFamilia, p.DVH
                 FROM [HSecurity].[dbo].[Permiso] p 
                 INNER JOIN [HSecurity].[dbo].[Usuario_Permiso] up ON p.Id = up.Id_Permiso 
                 WHERE up.Id_User = @userId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.UniqueIdentifier) { Value = userId });

                    if (!isExternalConn) await conn.OpenAsync();

                    var tempRows = new List<(Guid Id, string Nombre, string Codigo, bool EsFamilia, string Dvh)>();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tempRows.Add((
                                Id: (Guid)reader["Id"],
                                Nombre: reader["Nombre"].ToString(),
                                Codigo: reader["Permiso"].ToString(),
                                EsFamilia: (bool)reader["EsFamilia"],
                                Dvh: reader["DVH"].ToString()
                            ));
                        }
                    }

                    foreach (var row in tempRows)
                    {
                        PermisoComponente c;
                        if (row.EsFamilia)
                        {
                            c = new Familia { Id = row.Id, Nombre = row.Nombre, PermisoCode = row.Codigo };
                        }
                        else
                        {
                            c = new Patente { Id = row.Id, Nombre = row.Nombre, PermisoCode = row.Codigo };
                        }

                        // Asignamos el VO del DVH
                        c.DVH = !string.IsNullOrEmpty(row.Dvh) ? DvhVo.Create(row.Dvh) : null;

                        if (row.EsFamilia)
                        {
                            await FillFamilyChildrenAsync(c, conn);
                        }

                        lista.Add(c);
                    }
                }
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }

            return lista;
        }

        private async Task FillFamilyChildrenAsync(PermisoComponente padre, SqlConnection conn)
        {
            // Sincronizado con la imagen: Relación en Permiso_Permiso
            string query = @"SELECT p.Id, p.Nombre, p.Permiso, p.EsFamilia, p.DVH 
                      FROM [HSecurity].[dbo].[Permiso] p 
                      INNER JOIN [HSecurity].[dbo].[Permiso_Permiso] pp ON p.Id = pp.Id_Hijo 
                      WHERE pp.Id_Padre = @padreId";

            var tempChildren = new List<(Guid Id, string Nombre, string Codigo, bool EsFamilia, string Dvh)>();

            using (var cmd = new SqlCommand(query, conn))
            {
                if (_currentTransaction != null) cmd.Transaction = _currentTransaction;
                cmd.Parameters.Add(new SqlParameter("@padreId", SqlDbType.UniqueIdentifier) { Value = padre.Id });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tempChildren.Add((
                            Id: (Guid)reader["Id"],
                            Nombre: reader["Nombre"].ToString(),
                            Codigo: reader["Permiso"].ToString(),
                            EsFamilia: (bool)reader["EsFamilia"],
                            Dvh: reader["DVH"].ToString()
                        ));
                    }
                }
            }

            foreach (var childRow in tempChildren)
            {
                PermisoComponente hijo;
                if (childRow.EsFamilia)
                {
                    hijo = new Familia { Id = childRow.Id, Nombre = childRow.Nombre, PermisoCode = childRow.Codigo };
                }
                else
                {
                    hijo = new Patente { Id = childRow.Id, Nombre = childRow.Nombre, PermisoCode = childRow.Codigo };
                }

                hijo.DVH = !string.IsNullOrEmpty(childRow.Dvh) ? DvhVo.Create(childRow.Dvh) : null;

                if (childRow.EsFamilia)
                {
                    await FillFamilyChildrenAsync(hijo, conn); // Recursión
                }

                padre.AgregarHijo(hijo);
            }
        }
    }
}