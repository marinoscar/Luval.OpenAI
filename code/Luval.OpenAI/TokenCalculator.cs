using Luval.OpenAI.Chat;
using Luval.OpenAI.Models;
using SharpToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Luval.OpenAI.Models.Model;

namespace Luval.OpenAI
{
    public static class TokenCalculator
    {

        public static int FromPrompt(string prompt, string modelName = "gpt-3.5-turbo", double coverage = 0.98)
        {
            if (coverage > 1 || coverage < 0.75d) throw new ArgumentOutOfRangeException(nameof(coverage), "The coverage indicates how much of the total tokes will be used, the value has to be between 0.75 and 1");
            if (string.IsNullOrEmpty(prompt)) throw new ArgumentNullException(nameof(prompt));
            if (!ModelMaxTokens.Instance.ContainsKey(modelName)) throw new ArgumentException($"There is no max token specification for model {modelName}");

            var modelMaxTokens = ModelMaxTokens.Instance[modelName];

            var tokens = TotalPromptTokens(prompt, modelName);
            if (tokens > modelMaxTokens) throw new ArgumentOutOfRangeException(nameof(prompt), "The prompt exceeds the max number of tokens allowed");

            var maxTokens = Convert.ToInt32((modelMaxTokens - tokens) * coverage);

            return maxTokens;
        }

        public static int FromChat(IEnumerable<ChatMessageRequest> chats, string modelName = "gpt-3.5-turbo")
        {
            var sw = new StringWriter();
            foreach (var chat in chats)
            {
                sw.WriteLine("{0} : {1}", chat.Role, chat.Content);
            }
            return FromPrompt(sw.ToString(), modelName);
        }

        public static int TotalPromptTokens(string text, Model model)
        {
            return GetTokens(text, model).Count;
        }

        public static int TotalPromptTokens(string text, string modelName)
        {
            return GetTokens(text, modelName).Count;
        }

        public static List<int> GetTokens(string text, Model model)
        {
            return GetTokens(text, model.Id);
        }

        public static List<int> GetTokens(string text, string modelName)
        {
            var actualModel = modelName;
            //checks for the 32k model that is not mapped in the nuget package
            if (actualModel.ToLowerInvariant().StartsWith("gpt-4")) actualModel = "gpt-4";
            var enc = GptEncoding.GetEncodingForModel(actualModel);
            return enc.Encode(text);
        }

    }
}
