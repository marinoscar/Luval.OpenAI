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

        protected virtual ApiRequest CreateApiRequest(object? payload, HttpMethod method = null)
        {
            var client = new HttpClient();
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
                    request.Content = new StringContent(jsonPayload, UnicodeEncoding.UTF8, "application/json");
                }
            }
            return new ApiRequest() { Client = client, Request = request };
        }
        protected virtual ApiRequest CreatePostApiRequest(object? payload)
        {
            return CreateApiRequest(payload, HttpMethod.Post);
        }

        protected virtual ApiRequest CreateGetApiRequest(object? payload)
        {
            return CreateApiRequest(payload, HttpMethod.Get);
        }

        protected virtual async Task<T> SendRequestAsync<T>(object? payload, HttpMethod method = null) where T : BaseModelResponse
        {
            var result = default(T);
            using (var req = CreateApiRequest(payload, method))
            {
                var response = await SendClientRequest(req.Client, req.Request, HttpCompletionOption.ResponseContentRead);
                ValidateResponse(response);
                result = ParseResponse<T>(response);
            }
            return result;
        }

        protected virtual Task<HttpResponseMessage> SendClientRequest(HttpClient client, HttpRequestMessage requestMessage, HttpCompletionOption option)
        {
            return client.SendAsync(requestMessage, option);
        }

        protected virtual Task<Stream> OpenResponseStream(HttpContent content)
        {
            return content.ReadAsStreamAsync();
        }

        protected virtual Task<string?> OpenLineStream(StreamReader reader)
        {
            return reader.ReadLineAsync();
        }

        protected virtual async IAsyncEnumerable<T> StreamRequestAsync<T>(object? payload, HttpMethod method = null) where T : BaseModelResponse
        {
            using (var req = CreateApiRequest(payload, method))
            {
                var response = await SendClientRequest(req.Client, req.Request, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode) throw new ApplicationException(string.Format("Invalid request: {0}", response.StatusCode));
                var contentResult = new StringWriter();
                using (var stream = await OpenResponseStream(response.Content))
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = await OpenLineStream(reader)) != null)
                    {
                        contentResult.WriteLine(line);

                        if (line.StartsWith("data:"))
                            line = line.Substring("data:".Length);

                        line = line.TrimStart();

                        if (line == "[DONE]")
                        {
                            yield break;
                        }
                        else if (line.StartsWith(":"))
                        { }
                        else if (!string.IsNullOrWhiteSpace(line))
                        {
                            var res = JsonConvert.DeserializeObject<T>(line);

                            res.ResponseData = ApiResponseData.TryToLoad(response);

                            yield return res;
                        }
                    }
                }
            }
        }

        protected virtual IAsyncEnumerable<T> PostStreamRequestAsync<T>(object? payload) where T : BaseModelResponse
        {
            return StreamRequestAsync<T>(payload, HttpMethod.Post);
        }

        protected virtual IAsyncEnumerable<T> GetStreamRequestAsync<T>(object? payload) where T : BaseModelResponse
        {
            return StreamRequestAsync<T>(payload, HttpMethod.Get);
        }

        protected virtual T ParseResponse<T>(HttpResponseMessage response) where T : BaseModelResponse
        {
            var sw = new StringWriter();
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                sw.Write(reader.ReadToEnd());
            }
            return JsonConvert.DeserializeObject<T>(sw.ToString());
        }

        protected virtual Task<T> PostRequestAsync<T>(object? payload) where T : BaseModelResponse
        {
            return SendRequestAsync<T>(payload, HttpMethod.Post);
        }

        protected virtual Task<T> GetRequestAsync<T>(object? payload) where T : BaseModelResponse
        {
            return SendRequestAsync<T>(payload, HttpMethod.Get);
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
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = response.Content.ReadAsStringAsync().Result;
                throw new HttpRequestException($"Badd request {response.ReasonPhrase}", new HttpRequestException(error));
            }
            else
            {
                throw new HttpRequestException("Failed to complete the request");
            }
        }

    }
}
