using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Completion
{
    public class ApiCompletionRequest : ApiRequestBase
    {

        public ApiCompletionRequest(ApiAuthentication authentication) : this(authentication, "https://api.openai.com/v1/completions")
        {

        }

        public ApiCompletionRequest(ApiAuthentication authentication, string endpoint) : base(authentication, endpoint)
        {
        }

        public virtual Task<CompletionResponse> SendCompletionAsync(CompletionRequest request)
        {
            return PostRequestAsync<CompletionResponse>((client, req) =>
            {
                client.SendAsync(req, HttpCompletionOption.ResponseContentRead);
            }, request);
        }
    }
}
