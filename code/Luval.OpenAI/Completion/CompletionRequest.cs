using Luval.OpenAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Completion
{
    public class CompletionRequest : BaseModelRequest
    {
        public CompletionRequest() : this(Model.TextDavinci003)
        {
        }

        public CompletionRequest(Model model) : base(model)
        {
            Temperature = 0.7d;
        }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("top_p")]
        public double? TopP { get; set; }

        [JsonProperty("n")]
        public int? N { get; set; }

        [JsonProperty("stream")]
        public bool? Stream { get; set; }

        [JsonProperty("logprobs")]
        public int? Logprobs { get; set; }

        [JsonProperty("stop")]
        public string Stop { get; set; }
    }
}
