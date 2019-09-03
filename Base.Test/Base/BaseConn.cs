using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Base.SDK.Base;
using Base.SDK.Request.Test;
using Base.SDK.Response;
using Newtonsoft.Json;

namespace Base.Test.Base
{
    public class BaseConn
    {
        public static TR Post<T, TR>(T body, bool HasToken = true)
            where T : IApiRequest<TR>
            where TR : IApiResponse
        {
            using (var client = new HttpClient())
            {
                if (HasToken)
                {
                    string token = GetToken();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                }
                HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{BaseConfig.Url}/{body.GetApiName()}", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var t = JsonConvert.DeserializeObject<TR>(result);
                    return t;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        RefreshToken();
                        return Post<T, TR>(body, HasToken);
                    }
                }
            }

            return default;
        }

        private static string GetToken()
        {
            if (string.IsNullOrEmpty(BaseConfig.Token))
            {
                RefreshToken();
            }
            return BaseConfig.Token;
        }

        private static void RefreshToken()
        {
            var request = new TestLoginRequest()
            {
                userRole="2"
            };
            var result = Post<TestLoginRequest, SingleApiResponse>(request, false);
            BaseConfig.Token = result.Data.ToString();
        }
    }
}
