using System;
using System.Diagnostics;

namespace Shared
{
    public class ApplicationSettings : IApplicationSettings
    {

        public string SecurityConnection
        {
            get
            {
                if (Environment.MachineName == "DESKTOP-O5USKIH")
                {
                    // Conexión para la Notebook
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=HSecurity;Integrated Security=True;MultipleActiveResultSets=True";//@"Data Source=DESKTOP-O5USKIH\LOCALDB#59575304;Initial Catalog=HSecurity;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                }
                else
                {
                    // Conexión para la PC de Escritorio
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=HSecurity;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                }
            
            }
        }

        public string EntitiesConnection
        {
            get
            {
                if (Environment.MachineName == "DESKTOP-O5USKIH")
                {
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=HEntities;Integrated Security=True;MultipleActiveResultSets=True";//@"Data Source=DESKTOP-O5USKIH\LOCALDB#59575304;Initial Catalog=HEntities;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                }
                else
                {
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=HEntities;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
                }
            }
        }



        public string UserTableName { get; } = "Users";
        public string EmployeeTableName { get; } = "Employees";
        public string ClientTableName { get; } = "Clients";
        public string SupplierTableName { get; } = "Suppliers";
        public string SaleTableName { get; } = "Sales";
        public string PaymentTableName { get; } = "Payments";
        public string ProductTableName { get; } = "Products";
        public string CategoryTableName { get; } = "Categories";

        public string SearchBarPlaceHolder { get; } = "Búsqueda";
        public string SuccessOnOperation { get; } = "Operación existosa.";
        public string ErrorOnOperation { get; } = "Operación fallida:";
        public string DefaultLanguage { get; } = "es";
        public string ComboBoxPlaceholder { get; } = "Seleccionar columna";

        public ApplicationSettings()
        {
            //Bajar connection strings desde un archivo de configuraciones
        }




    }
}
