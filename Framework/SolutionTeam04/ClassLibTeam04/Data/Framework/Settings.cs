using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam04.Data.Framework
{
    static class Settings
    {
        public static string GetConnectionString()
        {
            string connectionString = "Trusted_Connection=True;";
            connectionString = "user id = PxlUser_04;";
            connectionString += "Password = 160CFv2!;";
            connectionString += $@"Server=10.128.4.7;";
            connectionString += $"Database=Db2025Team_04";
            return connectionString;
        }

    }

}
