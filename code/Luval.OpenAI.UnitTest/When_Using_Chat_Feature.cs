using Luval.OpenAI.Chat;
using Luval.OpenAI.Completion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.UnitTest
{
    public class When_Using_Chat_Feature
    {
        [Fact]
        public void It_Should_Do_A_Completion()
        {
            var api = new ChatEndpoint(new ApiAuthentication(Util.Key));
            api.SetSystemMessage("You are a helpful assistant.");
            api.AddUserMessage("Who won the world series in 2020?");
            api.AddAssitantMessage("The Los Angeles Dodgers won the World Series in 2020.");
            api.AddUserMessage("Where was it played?");
            var result = api.SendAsync().Result;
        }
    }
}
