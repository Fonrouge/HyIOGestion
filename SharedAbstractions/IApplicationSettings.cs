namespace Shared
{
    public interface IApplicationSettings
    {
        
        //Connection strings
        string SecurityConnection { get; }
        string EntitiesConnection { get; }

        //Tables
        string ClientTableName { get; }
        string UserTableName { get; }
        string EmployeeTableName { get; }
        string SupplierTableName { get; }
        string SaleTableName { get; }

        //UI
        string SearchBarPlaceHolder { get; }
        string SuccessOnOperation { get; }
        string ErrorOnOperation { get; }
        string ComboBoxPlaceholder { get; }


        //Configurations
        string DefaultLanguage { get; }
        

    }
}
