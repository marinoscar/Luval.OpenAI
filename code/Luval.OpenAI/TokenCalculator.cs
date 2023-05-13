using Luval.OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public static class TokenCalculator
    {
        private const int _max = 4097;

        public static int FromPrompt(string prompt) { return FromPrompt(prompt, _max); }
        public static int FromChat(IEnumerable<ChatMessageRequest> chats) { return FromChat(chats, _max); }

        public static int FromPrompt(string prompt, int modelMaxTokens)
        {
            if (string.IsNullOrEmpty(prompt)) throw new ArgumentNullException(nameof(prompt));

            var tokens = (int)(prompt.Length / 2.5);
            if (tokens > modelMaxTokens) throw new ArgumentOutOfRangeException(nameof(prompt), "The prompt exceeds the max number of tokens allowed");

            var maxTokens = (int)((modelMaxTokens - tokens) * 0.99);

            return maxTokens;
        }

        public static int FromChat(IEnumerable<ChatMessageRequest> chats, int modelMaxTokens)
        {
            var sw = new StringWriter();
            foreach (var chat in chats)
            {
                sw.WriteLine("{0} : {1}", chat.Role, chat.Content);
            }
            return FromPrompt(sw.ToString(), modelMaxTokens);
        }
    }
}
