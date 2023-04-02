﻿using Luval.OpenAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Chat
{
    public class ChatRequest : BaseModelRequest
    {
        public ChatRequest() : this(Model.GPTTurbo)
        {
        }

        public ChatRequest(Model model) : base(model)
        {
        }

        [JsonProperty("messages")]
        public IList<ChatMessageRequest> Messages { get; set; }

    }

    public class ChatMessageRequest
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
