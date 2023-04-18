using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Chat
{
    public class ChatHistory
    {
        public ChatHistory()
        {
            Id = Guid.NewGuid().ToString();
            UtcCreatedOn = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public DateTime UtcCreatedOn { get; set; }
        public string UserId { get; set; }
        public ChatResponse Response { get; set; }
    }
}
