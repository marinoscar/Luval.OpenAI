using Luval.OpenAI.Completion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Chat
{
    public class ChatResponse : BaseModelResponse
    {
        public ChatResponse()
        {
            Choices = new List<ChatChoice>();
        }

        [JsonProperty("choices")]
        public IList<ChatChoice> Choices { get; set; }

        [JsonIgnore]
        public ChatChoice Choice => Choices.FirstOrDefault();

        public override string ToString()
        {
            return Choice?.ToString();
        }
    }

    public class ChatChoice
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
        
        [JsonProperty("message")]
        public ChatMessageRequest Message { get; set; }

        public override string ToString()
        {
            return Message?.Content;
        }
    }

}
