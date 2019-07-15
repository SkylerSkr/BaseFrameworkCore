using Base.Domain.Entitys;
using Base.Repository.Core;
using Base.SDK.Request.Test;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Repository.Repo
{
    public class TestRepo : RepositoryBase
    {
        public Tuple<int, List<T>> GetList<T>(TestGetListRequest req)
            where T : class
        {
            var sql = new StringBuilder(@"
            SELECT
            sui.UID
                , sui.ULoginName
                , sui.ULoginPWD
                , sui.URealName
                , ba.Bcontent
                FROM SysUserInfoes sui
            LEFT JOIN BlogArticles ba
            ON sui.ULoginName = ba.Bsubmitter
            WHERE 1=1 AND
            ");
            var param = new DynamicParameters();

            if (req.UID.HasValue)
            {
                sql.Append("sui.UID = @UID");
                param.Add("@UID", req.UID);
            }
            if (!string.IsNullOrEmpty(req.ULoginName))
            {
                sql.Append("sui.ULoginName = @ULoginName");
                param.Add("@ULoginName", req.ULoginName);
            }
            if (!string.IsNullOrEmpty(req.ULoginPWD))
            {
                sql.Append("sui.ULoginPWD = @ULoginPWD");
                param.Add("@ULoginPWD", req.ULoginPWD);
            }
            if (!string.IsNullOrEmpty(req.URealName))
            {
                sql.Append("sui.URealName = @URealName");
                param.Add("@URealName", req.URealName);
            }

            return QueryPage<SysUserInfoes, T>(sql, param, " ORDER BY sui.UID ",req.StartSize,req.PageSize);
        }
    }
}
