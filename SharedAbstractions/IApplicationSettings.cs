namespace Shared
{
    public interface IApplicationSettings
    {

        //Connection strings
        string SecurityConnection { get; }
        string EntitiesConnection { get; }

        //Tables
        string ClientTableName { get; }
        string UsuarioTableName { get; }
        string EmployeeTableName { get; }
        string SupplierTableName { get; }
        string SaleTableName { get; }
        string SaleDetailTableName { get; }
        string PaymentTableName { get; }
        string ProductTableName { get; }
        string ProductCategoryTableName { get; }
        string CategoryTableName { get; }
        string BitacoraTableName { get; }
        string UsuarioPermisoTableName { get; }
        string PermisoPermisoTableName { get; }
        string PermisoTableName { get; }

        //UI
        string SearchBarPlaceHolder { get; }
        string SuccessOnOperation { get; }
        string ErrorOnOperation { get; }
        string ComboBoxPlaceholder { get; }


        //Configurations
        string DefaultLanguage { get; }


    }
}
