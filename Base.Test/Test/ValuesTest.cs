using System;
using System.Collections.Generic;
using System.Text;
using Base.SDK.Request.Test;
using Base.SDK.Response;
using Base.Test.Base;
using NUnit.Framework;

namespace Base.Test.Test
{
    [TestFixture]
    public class ValuesTest
    {
        [Test]
        public void UpLoadFile()
        {
            var response = BaseConn.Post<TestGetRequest, SingleApiResponse>(new TestGetRequest()
            {
                UID=1
            });
            Assert.True(response.IsSuccess);
            Assert.True(response.IsBizSuccess);
        }
    }
}
