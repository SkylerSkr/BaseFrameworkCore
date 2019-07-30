using System;
using System.Collections.Generic;
using Base.Common.Attribute;
using Base.Domain.Entitys;
using Base.IBusinessService;
using Base.Repository;
using Base.Repository.Repo;
using Base.SDK.Model;
using Base.SDK.Request.Test;
using Base.SDK.Response;
using System.Linq;

namespace Base.BusinessService
{
    public class TestBiz : ITestBiz
    {
        private static readonly TestRepo TestRepo = new TestRepo();

        public List<PermissionItem> GetRoleModule()
        {
            List<PermissionItem> list = new List<PermissionItem>();
            list.Add(new PermissionItem() { UserId = 1, LinkUrl = "/api/values/get" });
            list.Add(new PermissionItem() { UserId = 1, LinkUrl = "/api/values/getlist" });
            list.Add(new PermissionItem() { UserId = 2, LinkUrl = "/api/values/get" });
            return list;
        }

        [Caching(AbsoluteExpiration = 10000)]
        public SingleApiResponse Get(TestGetRequest req)
        {
            //单表查询
            var result = RepoBase.Instance.GetWhere<SysUserInfoes>(x => x.UID == req.UID).ToList().FirstOrDefault();
            return new SingleApiResponse() { Data = result };
        }

        public SingleApiResponse Save(TestSaveRequest req)
        {
            //编辑
            if (req.UID.HasValue)
            {
                var result = RepoBase.Instance.GetWhere<SysUserInfoes>(x => x.UID == req.UID).ToList().FirstOrDefault();
                if (result == null) return new SingleApiResponse() { BizErrorMsg = "不包含此用户" };
                result.ULoginName = req.ULoginName;
                result.ULoginPWD = req.ULoginPWD;
                result.URealName = req.URealName;
                RepoBase.Instance.Update(result);
            }
            //新增
            else
            {
                RepoBase.Instance.Add(new SysUserInfoes() { ULoginName = req.ULoginName, ULoginPWD = req.ULoginPWD, URealName = req.URealName });
            }
            return new SingleApiResponse();
        }

        public SingleApiResponse Delete(TestSaveRequest req)
        {
            throw new Exception("xxx");
            var result = RepoBase.Instance.GetWhere<SysUserInfoes>(x => x.UID == req.UID).ToList().FirstOrDefault();
            if (result == null) return new SingleApiResponse() { BizErrorMsg = "不包含此用户" };

            RepoBase.Instance.Delete(result);
          
            return new SingleApiResponse();
        }

        [Caching(AbsoluteExpiration = 10000)]
        public ListApiResponse GetList(TestGetListRequest req)
        {
            var result = TestRepo.GetList<SysUserModel>(req);
            return new ListApiResponse() { Data = result.Item2, TotalCount = result.Item1 };
        }
    }
}
