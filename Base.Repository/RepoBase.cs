using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Base.Common.Tools;
using Base.Domain;
using Base.Domain.Enum;
using Base.Repository.Core;
using Dapper;
using DapperExtensions;

namespace Base.Repository
{
    /// <summary>
    /// 单表模型的曾删改查 处理
    /// </summary>
    public sealed class RepoBase: Singleton<RepoBase>
    {
        public long BulkInsert<T>(IEnumerable<T> entitys, string primaryKey, EnumDBType? dbType = null) where T : EntityBase
        {
            if (entitys.Count() > 1000)
            {
                //如果超过1000个 切成两份
                return BulkInsert(entitys.Take(1000), primaryKey) + BulkInsert(entitys.Skip(1000), primaryKey);
            }

            // 拼接sql的方式批量插入
            Type Ts = typeof(T);
            var tableName = Ts.Name.Replace("Entity", "");

            StringBuilder sb = new StringBuilder();

            #region 拼接需要添加的字段
            sb.Append("INSERT INTO ").Append(tableName).Append(" (");
            IEnumerable<string> p = Ts.GetProperties().ToList().Where(x => !x.Name.Equals(primaryKey)).Select(x => x.Name);
            sb.Append(string.Join(",", p.ToArray())).Append(") ");
            #endregion

            #region 按照顺序拼接值
            sb.Append("VALUES");
            entitys.ToList().ForEach(entity =>
            {
                sb.Append("(");
                foreach (var item in p)
                {
                    object o = Ts.GetProperty(item).GetValue(entity, null);
                    if (o == null)
                    { 
                        sb.Append("null");
                    }
                    else if (o is string || o is DateTime || o is Guid)
                    {
                        sb.Append("'").Append(Convert.ToString(o)).Append("'");
                    }
                    else if (o is bool)
                    {
                        sb.Append(((bool)o ? 1 : 0));
                    }
                    else
                    {
                        sb.Append(Convert.ToString(o));
                    }
                    sb.Append(',');
                }
                sb.Replace(",", "),", sb.Length - 1, 1);
            });
            sb.Remove(sb.Length - 1, 1);
            #endregion


            var dbClient = DBProxy.CreateClient(dbType);
            var iRowsCount = dbClient.Execute(sb.ToString(), new DynamicParameters());
            return iRowsCount;
        }

        public void BulkInsert<T>(IEnumerable<T> entitys, EnumDBType? dbType = null) where T : EntityBase
        {
            var dbClient = DBProxy.CreateClient(dbType);
            dbClient.Insert(entitys);
        }
        public long Add<T>(T entity, EnumDBType? dbType = null) where T : EntityBase
        {
            var dbClient = DBProxy.CreateClient(dbType);
            var identify = dbClient.Insert(entity);
            return identify;
        }

        public bool Delete<T>(T entity, EnumDBType? dbType = null) where T : EntityBase
        {
            var dbClient = DBProxy.CreateClient(dbType);
            return dbClient.Delete<T>(entity);
        }

        public bool Update<T>(T entity, EnumDBType? dbType = null) where T : EntityBase
        {
            var dbClient = DBProxy.CreateClient(dbType);
            return dbClient.Update(entity);

        }

        public T Get<T>(object id, EnumDBType? dbType = null) where T : EntityBase
        {
            var dbClient = DBProxy.CreateClient(dbType);
            var entity = dbClient.Get<T>(id);
            return entity;
        }

        public IEnumerable<T> GetWhere<T>(Expression<Func<T, bool>> predicate, EnumDBType? dbType = null) where T : EntityBase
        {
            var client = DBProxy.CreateClient(dbType);
            // 拼接sql的方式批量插入
            Type Ts = typeof(T);
            var sql = $"select * from {Ts.Name.Replace("Entity", "")} where {SqlExpress.Instance.GetSql(predicate)}";
            return client.Query<T>(sql);
        }

        #region 辅助方法

        /// <summary>
        /// 构造查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        private string QueryStringBuilder<T>(string selectColumns) where T : EntityBase
        {
            var tableName = typeof(T).Name.Replace("Entity", "");
            var whiteSpace = " ";
            var queryString = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT").Append(whiteSpace);
            sb.Append(selectColumns).Append(whiteSpace);
            sb.Append("FROM").Append(whiteSpace);
            sb.Append(tableName).Append(whiteSpace);
            sb.Append("INNER JOIN STRING_SPLIT(@Ids,',') TMPTABLE").Append(whiteSpace);
            sb.Append("ON").Append(whiteSpace);
            sb.Append($"{tableName}.PK_{tableName} = TMPTABLE.Value ").Append(whiteSpace);

            sb.Append(whiteSpace);


            queryString = sb.ToString();
            return queryString;
        }

        #endregion
    }
}
