using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public abstract class ApiRequestBase
    {
        public ApiRequestBase(ApiAuthentication authentication, string endpoint)
        {
            if (authentication == null) throw new ArgumentNullException(nameof(authentication));
            if (authentication.Key == default(SecureString)) throw new ArgumentNullException(nameof(authentication.Key));
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException(nameof(endpoint));

            Authentication = authentication;
            Endpoint = endpoint;
        }

        protected virtual ApiAuthentication Authentication { get; private set; }
        protected virtual string Endpoint { get; private set; }

        protected virtual void WithClient(Action<HttpClient, HttpRequestMessage> runClient, object? payload, HttpMethod method = null)
        {
            using (var client = new HttpClient())
            {
                var key = Authentication.GetKey();
                if (method == null) method = HttpMethod.Get;

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
                client.DefaultRequestHeaders.Add("api-key", key);
                client.DefaultRequestHeaders.Add("User-Agent", "Luval-OpenAI");

                var request = new HttpRequestMessage(method, Endpoint);

                if (!string.IsNullOrEmpty(Authentication.Organization))
                    client.DefaultRequestHeaders.Add("OpenAI-Organization", Authentication.Organization);

                if (payload != null)
                {
                    if (payload is HttpContent) request.Content = payload as HttpContent;
                    else
                    {
                        var jsonPayload = JsonConvert.SerializeObject(payload, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        request.Content = new StringContent(jsonPayload, UnicodeEncoding.UTF8, "application/json"); ;
                    }
                }
                runClient(client, request);
            }
        }

        protected virtual void WithPostClient(Action<HttpClient, HttpRequestMessage> runClient, object? payload)
        {
            WithClient(runClient, payload, HttpMethod.Post);
        }

        protected virtual async Task<T> SendRequestAsync<T>(Action<HttpClient, HttpRequestMessage> runClient, object? payload, HttpMethod method = null) where T : BaseModelResponse
        {
            var result = default(T);
            WithClient(async (c, req) =>
            {

                var response = await c.SendAsync(req, HttpCompletionOption.ResponseContentRead);
                ValidateResponse(response);
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(content);

            }, payload, method);
            return result;
        }

        protected virtual Task<T> PostRequestAsync<T>(Action<HttpClient, HttpRequestMessage> runClient, object? payload) where T : BaseModelResponse
        {
            return SendRequestAsync<T>(runClient, payload, HttpMethod.Post);
        }

        protected virtual Task<T> GetRequestAsync<T>(Action<HttpClient, HttpRequestMessage> runClient, object? payload) where T : BaseModelResponse
        {
            return SendRequestAsync<T>(runClient, payload, HttpMethod.Get);
        }



        protected virtual void ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException("Invalid authorinzation parameters provided");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException("Endpoint had an internal server error");
            }
            else
            {
                throw new HttpRequestException("Failed to complete the request");
            }
        }

    }
}
