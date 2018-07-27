using BackEndLibrary.Interfaces;

namespace BackEndLibrary 
{
    public class BaseSqlServerConnections : BaseExceptionsHandler, IConnection

    {
        /// <summary>
        /// This points to your database server
        /// </summary>
        protected string DatabaseServer = "KARENS-PC";
        /// <summary>
        /// Name of database containing required tables
        /// </summary>
        protected string DefaultCatalog = "";
        public string ConnectionString => $"Data Source={DatabaseServer};Initial Catalog={DefaultCatalog};Integrated Security=True";
    }

}
