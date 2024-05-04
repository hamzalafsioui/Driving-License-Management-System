using System;
using System.Configuration;

namespace DVLD_DataAccess
{
    static class clsDataAccessSettings
    {
        public static string ConnectionString
        {
            get
            {
                try
                {
                    string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    return connectionString;
                }
                catch (Exception ex) { clsDataAccessLogs.CreateExceptionLog(ex.ToString()); }

                return "";
            }
        }

    }
}
