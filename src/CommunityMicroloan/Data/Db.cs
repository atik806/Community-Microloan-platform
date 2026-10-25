using System.Configuration;

namespace borrowersignup
{
    /// <summary>
    /// Single source for the database connection string. Edit the "b_info"
    /// entry in App.config to point the app at your own SQL Server instance.
    /// </summary>
    internal static class Db
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["b_info"].ConnectionString;
    }
}
