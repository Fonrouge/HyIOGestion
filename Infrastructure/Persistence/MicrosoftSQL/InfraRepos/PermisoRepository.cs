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

        public void SetTransaction(object transaction)
        {
            _currentTransaction = (SqlTransaction)transaction;
        }

        // Renombrado para cumplir la convención asíncrona (Asegúrate de actualizar la interfaz IPermisoRepository)
        public async Task<List<PermisoComponente>> GetPermissionsByUserAsync(Guid userId)
        {
            var lista = new List<PermisoComponente>();

            // 1. Determinar conexión (propia o de la transacción compartida)
            SqlConnection conn = _currentTransaction?.Connection;
            bool isExternalConn = conn != null;

            if (!isExternalConn)
                conn = new SqlConnection(_appSettings.SecurityConnection);

            try
            {
                string query = @"SELECT p.Id_Permiso, p.Nombre, p.Permiso, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Usuario_Permiso up ON p.Id_Permiso = up.Id_Permiso 
                                 WHERE up.Id_User = @userId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (isExternalConn) cmd.Transaction = _currentTransaction;

                    // Mejor práctica: Tipo de dato explícito
                    cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.UniqueIdentifier) { Value = userId });

                    if (!isExternalConn) await conn.OpenAsync(); // Asíncrono

                    var tempRows = new List<(Guid Id, string Nombre, string Codigo, bool EsFamilia)>();

                    using (var reader = await cmd.ExecuteReaderAsync()) // Asíncrono
                    {
                        while (await reader.ReadAsync()) // Asíncrono
                        {
                            tempRows.Add((
                                Id: (Guid)reader["Id_Permiso"],
                                Nombre: reader["Nombre"].ToString(),
                                Codigo: reader["Permiso"].ToString(),
                                EsFamilia: (bool)reader["EsFamilia"]
                            ));
                        }
                    } // El Reader se cierra aquí, liberando la conexión

                    // 2. Procesamos la lista y disparamos la recursividad del Composite
                    foreach (var row in tempRows)
                    {
                        PermisoComponente c;

                        if (row.EsFamilia)
                        {
                            c = new Familia { Id = row.Id, Nombre = row.Nombre, Permiso = row.Codigo };
                            // Llamada al método recursivo asíncrono
                            await FillFamilyChildrenAsync(c, conn);
                        }
                        else
                        {
                            c = new Patente { Id = row.Id, Nombre = row.Nombre, Permiso = row.Codigo };
                        }

                        lista.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                //    Debugger.Break(); // debug para leer la excepción
                throw;
            }
            finally
            {
                if (!isExternalConn) conn.Dispose();
            }

            return lista;
        }

        private async Task FillFamilyChildrenAsync(PermisoComponente padre, SqlConnection conn)
        {
            string query = @"SELECT p.Id_Permiso, p.Nombre, p.Permiso, p.EsFamilia 
                             FROM Permiso p 
                             INNER JOIN Permiso_Permiso pp ON p.Id_Permiso = pp.Id_Hijo 
                             WHERE pp.Id_Padre = @padreId";

            var tempChildren = new List<(Guid Id, string Nombre, string Codigo, bool EsFamilia)>();

            using (var cmd = new SqlCommand(query, conn))
            {
                if (_currentTransaction != null) cmd.Transaction = _currentTransaction;

                // Mejor práctica: Tipo de dato explícito
                cmd.Parameters.Add(new SqlParameter("@padreId", SqlDbType.UniqueIdentifier) { Value = padre.Id });

                using (var reader = await cmd.ExecuteReaderAsync()) // Asíncrono
                {
                    while (await reader.ReadAsync()) // Asíncrono
                    {
                        tempChildren.Add((
                            Id: (Guid)reader["Id_Permiso"],
                            Nombre: reader["Nombre"].ToString(),
                            Codigo: reader["Permiso"].ToString(),
                            EsFamilia: (bool)reader["EsFamilia"]
                        ));
                    }
                } // El Reader se cierra aquí, liberando la conexión para la recursión
            }

            foreach (var childRow in tempChildren)
            {
                PermisoComponente hijo;

                if (childRow.EsFamilia)
                {
                    hijo = new Familia { Id = childRow.Id, Nombre = childRow.Nombre, Permiso = childRow.Codigo };
                    // RECURSIÓN ASÍNCRONA: Esperamos a que los hijos de esta familia se carguen
                    await FillFamilyChildrenAsync(hijo, conn);
                }
                else
                {
                    hijo = new Patente { Id = childRow.Id, Nombre = childRow.Nombre, Permiso = childRow.Codigo };
                }

                padre.AgregarHijo(hijo);
            }
        }
    }
}