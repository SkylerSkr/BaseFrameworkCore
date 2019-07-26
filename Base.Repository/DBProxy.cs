using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Base.Common.Config;

namespace Base.Repository
{
    internal class DBProxy
    {
        internal static IDbConnection CreateClient()
        {
           var connString= Appsettings.app("SkrConnectString");
           return new SqlConnection(connString);
        }
    }
}
