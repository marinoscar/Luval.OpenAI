﻿using Luval.OpenAI.Completion;
using Luval.OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Chat
{
    public class ChatEndpoint : BaseEndpoint<ChatRequest, ChatResponse>
    {

        public static ChatEndpoint CreateAzure(ApiAuthentication authentication, string resourceName, Model model, string deploymentName = "gpt-35-turbo", string apiVersion = "2023-03-15-preview")
        {
            return new ChatEndpoint(authentication, string.Format("https://{0}.openai.azure.com/openai/deployments/{1}/chat/completions?api-version={2}",
                resourceName, deploymentName, apiVersion), model);
        }

        public static ChatEndpoint CreateAzure(ApiAuthentication authentication, string resourceName, string deploymentName = "gpt-35-turbo", string apiVersion = "2023-03-15-preview")
        {
            return new ChatEndpoint(authentication, string.Format("https://{0}.openai.azure.com/openai/deployments/{1}/chat/completions?api-version={2}",
                resourceName, deploymentName, apiVersion));
        }

        public static ChatEndpoint CreateOpenAI(ApiAuthentication authentication, string endpoint = "https://api.openai.com/v1/chat/completions")
        {
            return new ChatEndpoint(authentication, endpoint);
        }

        public static ChatEndpoint CreateOpenAI(ApiAuthentication authentication, Model model, string endpoint = "https://api.openai.com/v1/chat/completions")
        {
            return new ChatEndpoint(authentication, endpoint, model);
        }

        public ChatEndpoint(ApiAuthentication authentication) : this(authentication, "https://api.openai.com/v1/chat/completions", Model.GPTTurbo16k)
        {

        }

        public ChatEndpoint(ApiAuthentication authentication, string endpoint) : this(authentication, endpoint, Model.GPTTurbo16k)
        {
        }

        public ChatEndpoint(ApiAuthentication authentication, string endpoint, Model model) : base(authentication, endpoint)
        {
            SetSystemMessage("You are a helpful assistant.");
            Model = model;
        }

        private List<ChatMessageRequest> _chatRequests;

        protected IList<ChatMessageRequest> ChatMessages
        {
            get
            {
                if (_chatRequests == null) { _chatRequests = new List<ChatMessageRequest>(); }
                return _chatRequests;
            }
        }

        public virtual Model Model { get; set; }

        public void SetSystemMessage(string message)
        {
            var system = ChatMessages.FirstOrDefault(i => i.Role == "system");
            if (system == null)
            {
                ChatMessages.Insert(0, new ChatMessageRequest() { Role = "system", Content = message });
                return;
            }
            system.Content = message;
        }

        public void ClearMessages()
        {
            ChatMessages.Clear();
        }

        public void AddUserMessage(string message)
        {
            ChatMessages.Add(new ChatMessageRequest() { Role = "user", Content = message });
        }

        public void AddAssitantMessage(string message)
        {
            ChatMessages.Add(new ChatMessageRequest() { Role = "assistant", Content = message });
        }

        public Task<ChatResponse> SendAsync(Model model, int maxTokens, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(maxTokens, model, false, temperature));
        }

        public Task<ChatResponse> SendAsync(int maxTokens, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(maxTokens, Model.GPTTurbo, false, temperature));
        }

        public Task<ChatResponse> SendAsync(Model model, double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(TokenCalculator.FromChat(ChatMessages, model.Id), model, false, temperature));
        }

        public Task<ChatResponse> SendAsync(double temperature = 0.7d)
        {
            return SendAsync(CreateRequest(TokenCalculator.FromChat(ChatMessages, Model.Id), Model, false, temperature));
        }

        public override Task<ChatResponse> SendAsync(ChatRequest request)
        {
            request.Messages = ChatMessages;
            return base.SendAsync(request);
        }

        public IAsyncEnumerable<ChatResponse> StreamAsync(double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(TokenCalculator.FromChat(ChatMessages, Model.Id), Model, true, temperature));
        }

        public IAsyncEnumerable<ChatResponse> StreamAsync(int maxTokens, double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(maxTokens, Model.GPTTurbo, true, temperature));
        }

        public IAsyncEnumerable<ChatResponse> StreamAsync(int maxTokens, Model model, double temperature = 0.7d)
        {
            return StreamAsync(CreateRequest(maxTokens, model, true, temperature));
        }

        public override IAsyncEnumerable<ChatResponse> StreamAsync(ChatRequest request)
        {
            request.Stream = true;
            request.Messages = ChatMessages;
            return base.StreamAsync(request);
        }

        private ChatRequest CreateRequest(int maxTokens, Model model, bool stream = false, double temperature = 0.7d)
        {
            return new ChatRequest()
            {
                MaxTokens = maxTokens,
                Model = model,
                Stream = stream,
                Temperature = temperature,
                Messages = ChatMessages
            };
        }

    }
}
