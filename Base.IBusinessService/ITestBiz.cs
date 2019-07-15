using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Request.Test;
using Base.SDK.Response;

namespace Base.IBusinessService
{
    public interface ITestBiz
    {
        SingleApiResponse Get(TestGetRequest req);

        SingleApiResponse Save(TestSaveRequest req);

        SingleApiResponse Delete(TestSaveRequest req);

        ListApiResponse GetList(TestGetListRequest req);
    }
}
