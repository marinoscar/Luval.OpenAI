using Luval.OpenAI.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Task<CompletionResponse> SendCompletionAsync(CompletionRequest request)
        {
            return PostRequestAsync<CompletionResponse>(request);
        }

        public Task<CompletionResponse> SendCompletionAsync(string prompt, int maxTokens, Model model, bool stream = false, double temperature = 0.7d)
        {
            return SendCompletionAsync(CreateRequest(prompt, maxTokens, model, stream, temperature));
        }

        public Task<CompletionResponse> SendCompletionAsync(string prompt, int maxTokens, bool stream = false, double temperature = 0.7d)
        {
            return SendCompletionAsync(CreateRequest(prompt, maxTokens, Model.TextDavinci003, stream, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamCompletionAsync(string prompt, int maxTokens, Model model, double temperature = 0.7d)
        {
            return StreamCompletionAsync(CreateRequest(prompt, maxTokens, model, true, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamCompletionAsync(string prompt, int maxTokens, double temperature = 0.7d)
        {
            return StreamCompletionAsync(CreateRequest(prompt, maxTokens, Model.TextDavinci003, true, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamCompletionAsync(CompletionRequest request)
        {
            request.Stream = true;
            return PostStreamRequestAsync<CompletionResponse>(request);
        }

        private CompletionRequest CreateRequest(string prompt, int maxTokens, Model model, bool stream = false, double temperature = 0.7d)
        {
            return new CompletionRequest() { Prompt = prompt, MaxTokens = maxTokens, Model = model, Stream = stream, Temperature = temperature };
        }


    }
}
