using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Domain;
using Base.Domain.Enum;
using Dapper;

namespace Base.Repository.Core
{
    public abstract class RepositoryBase
    {
        internal virtual Tuple<int, List<TOut>> QueryPage<TIn, TOut>(StringBuilder sql, DynamicParameters param,string orderSql=null,int? startSize=null,int? pageSize=null, EnumDBType? dbType = null)
            where TIn : EntityBase
            where TOut : class
        {
            var dbClient = DBProxy.CreateClient(dbType);
            //计数
            var countSql = $"select count(*) from ({sql.ToString()})tCount;";
            var totalCount = dbClient.ExecuteScalar(countSql, param);

            //分页
            if (pageSize.HasValue)
            {
                sql= new StringBuilder($"{sql.ToString()} {orderSql} OFFSET {startSize} ROW FETCH NEXT {pageSize} ROW ONLY ;");
            }
            var lst = dbClient.Query<TOut>(sql.ToString(), param).ToList();
            return new Tuple<int, List<TOut>>(Convert.ToInt32(totalCount), lst);
        }


    }
}
