using Luval.OpenAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Completion
{
    public class CompletionRequest : TextModelRequestBase
    {
        public CompletionRequest() : this(Model.TextDavinci003)
        {
        }

        public CompletionRequest(Model model) : base(model)
        {
        }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        

        [JsonProperty("logprobs")]
        public int? Logprobs { get; set; }
    }
}
