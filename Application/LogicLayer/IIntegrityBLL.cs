namespace BLL.LogicLayer
{
    public interface IIntegrityBLL
    {
        bool ValidateGlobalIntegrity(string tabla, string connectionString);
    }
}
