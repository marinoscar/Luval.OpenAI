using Luval.OpenAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public abstract class TextModelRequestBase : BaseModelRequest
    {

        public TextModelRequestBase(Model model) : base(model)
        {
            Temperature = 0.7d;
        }

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

        [JsonProperty("stop")]
        public string Stop { get; set; }

    }
}
