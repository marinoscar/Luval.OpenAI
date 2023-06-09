﻿using Luval.OpenAI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Completion
{
    public class CompletionEndpoint : BaseEndpoint<CompletionRequest, CompletionResponse>
    {

        public static CompletionEndpoint CreateAzure(ApiAuthentication authentication, string resourceName, string deploymentName = "text-davinci-003", string apiVersion = "2022-12-01")
        {
            return new CompletionEndpoint(authentication, string.Format("https://{0}.openai.azure.com/openai/deployments/{1}/completions?api-version={2}",
                resourceName, deploymentName, apiVersion));
        }

        public static CompletionEndpoint CreateOpenAI(ApiAuthentication authentication, string endpoint = "https://api.openai.com/v1/completions")
        {
            return new CompletionEndpoint(authentication, endpoint);
        }

        public CompletionEndpoint(ApiAuthentication authentication) : this(authentication, "https://api.openai.com/v1/completions")
        {

        }

        public CompletionEndpoint(ApiAuthentication authentication, string endpoint) : base(authentication, endpoint)
        {
        }

        public Task<CompletionResponse> SendAsync(string prompt, int maxTokens, Model model, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(prompt, maxTokens, model, false, temperature));
        }

        public Task<CompletionResponse> SendAsync(string prompt, int maxTokens, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(prompt, maxTokens, Model.TextDavinci003, false, temperature));
        }

        public Task<CompletionResponse> SendAsync(string prompt, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(prompt, TokenCalculator.FromPrompt(prompt, Model.TextDavinci003.Id), Model.TextDavinci003, false, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamAsync(string prompt, int maxTokens, Model model, double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(prompt, maxTokens, model, true, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamAsync(string prompt, int maxTokens, double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(prompt, maxTokens, Model.TextDavinci003, true, temperature));
        }

        public IAsyncEnumerable<CompletionResponse> StreamAsync(string prompt, double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(prompt, TokenCalculator.FromPrompt(prompt, Model.TextDavinci003.Id), Model.TextDavinci003, true, temperature));
        }

        public override IAsyncEnumerable<CompletionResponse> StreamAsync(CompletionRequest request)
        {
            request.Stream = true;
            return base.StreamAsync(request);
        }

        private CompletionRequest CreateRequest(string prompt, int maxTokens, Model model, bool stream = false, double temperature = 0.7d)
        {
            return new CompletionRequest() { Prompt = prompt, MaxTokens = maxTokens, Model = model, Stream = stream, Temperature = temperature };
        }


    }
}
