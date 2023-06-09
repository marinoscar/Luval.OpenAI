using Luval.OpenAI.Completion;
using System.Diagnostics;

namespace Luval.OpenAI.UnitTest
{
    public class When_Using_Completions
    {

        [Fact]
        public void It_Should_Do_A_Completion()
        {
            var api = new CompletionEndpoint(new ApiAuthentication(Util.Key));
            var result = api.SendAsync(new CompletionRequest() { Prompt = "tell me a joke", MaxTokens = 1000 }).Result;
        }

        [Fact]
        public async void It_Should_Do_A_Streaming_Of_A_Completion()
        {
            var api = new CompletionEndpoint(new ApiAuthentication(Util.Key));
            var req = new CompletionRequest() { Prompt = "write me an essay about Oscar Arias Sanchez", MaxTokens = 3900 };
            var count = 1;
            var sw = new StringWriter();
            CompletionResponse last;
            await foreach (var item in api.StreamAsync(req))
            {
                sw.Write(item);
                count++;
                last = item;
                Debug.WriteLine(count);
            }
            Debug.WriteLine(sw.ToString());
        }


    }
}