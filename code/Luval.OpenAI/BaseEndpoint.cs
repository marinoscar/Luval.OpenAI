using Luval.OpenAI.Completion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public abstract class BaseEndpoint<TRequest, TResponse> : ApiRequestBase 
        where TRequest : BaseModelRequest 
        where TResponse : BaseModelResponse
    {
        protected BaseEndpoint(ApiAuthentication authentication, string endpoint) : base(authentication, endpoint)
        {
            
        }

        public virtual Task<TResponse> SendAsync(TRequest request)
        {
            return PostRequestAsync<TResponse>(request);
        }

        public virtual IAsyncEnumerable<TResponse> StreamAsync(TRequest request)
        {
            return PostStreamRequestAsync<TResponse>(request);
        }
    }
}
