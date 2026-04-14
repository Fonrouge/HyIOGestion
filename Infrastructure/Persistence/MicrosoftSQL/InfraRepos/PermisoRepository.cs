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

        public async Task<IEnumerable<PermisoComponente>> GetAllAsync()
        {
            var lista = new List<PermisoComponente>();
            SqlConnection conn = _currentTransaction?.Connection ?? new SqlConnection(_appSettings.SecurityConnection);
            bool isExternalConn = _currentTransaction?.Connection != null;

            try
            {
                // Traemos solo los permisos raíz (o todos los de la tabla según tu lógica de negocio)
                // que coincidan con el estado de borrado lógico.
                string query = @"SELECT Id, Nombre, Permiso, EsFamilia, DVH 
                         FROM [HSecurity].[dbo].[Permiso]";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;
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

                        c.DVH = !string.IsNullOrEmpty(row.Dvh) ? DvhVo.Create(row.Dvh) : null;

                        // Si es familia, cargamos sus hijos recursivamente
                        // Nota: Los hijos suelen cargarse sin importar su IsDeleted o 
                        // podrías filtrar también dentro de FillFamilyChildrenAsync
                        if (row.EsFamilia)
                        {
                            await FillFamilyChildrenAsync(c, conn);
                        }

                        lista.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }

            return lista;
        }

        public async Task<List<PermisoRelacionDTO>> GetAllPermisoPermisoAsync()
        {
            var lista = new List<PermisoRelacionDTO>();
            SqlConnection conn = _currentTransaction?.Connection ?? new SqlConnection(_appSettings.SecurityConnection);
            bool isExternalConn = _currentTransaction?.Connection != null;

            try
            {
                string query = @"SELECT Id_Padre, Id_Hijo, DVH 
                         FROM [HSecurity].[dbo].[Permiso_Permiso]";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;
                    if (!isExternalConn) await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(PermisoRelacionDTO.Reconstitute
                            (
                                (Guid)reader["Id_Padre"],
                                (Guid)reader["Id_Hijo"],
                                reader["DVH"]?.ToString()
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log o manejo de error según tu política
                throw new Exception("Error al obtener las relaciones Permiso_Permiso", ex);
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