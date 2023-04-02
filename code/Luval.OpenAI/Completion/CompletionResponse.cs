using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Completion
{
    public class CompletionResponse : BaseModelResponse
    {
        public CompletionResponse()
        {
            Choices = new List<CompletionChoice>();
        }

        [JsonProperty("choices")]
        public IList<CompletionChoice> Choices { get; set; }

        [JsonIgnore]
        CompletionChoice Choice => Choices.FirstOrDefault();

        public override string ToString()
        {
            return Choice?.ToString();
        }
    }

    public class CompletionChoice
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("logprobs")]
        public object Logprobs { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
