using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Base.Common.Config;
using Base.Domain.Enum;

namespace Base.Repository
{
    internal class DBProxy
    {
        internal static IDbConnection CreateClient(EnumDBType? dbType)
        {
            var connString = Appsettings.app($"{dbType ?? EnumDBType.Japan}ConnectString");
            return new SqlConnection(connString);
        }
    }
}
