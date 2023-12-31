using Luval.OpenAI.Audio;
using Luval.OpenAI.Completion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.UnitTest
{
    public class When_Using_Audio_Endpoint
    {
        [Fact]
        public void It_Should_Create_A_File()
        {
            var api = new TextToSpeechEndpoint(new ApiAuthentication(Util.Key));
            var request = new TextToSpeechRequest()
            {
                Format = AudioOutputFormat.mp3,
                Model = SpeechModel.TTSHD,
                Speed = 0.9f,
                Voice = Voice.onyx,
                Input = "Dear Oscar, May the words of the New Testament bring comfort and guidance as you embark on your journey to deepen your Catholic faith and cultivate a healthier, stronger version of yourself. One passage that resonates with your purpose is found in Paul's letter to the Corinthians, 1 Corinthians 6:19-20: \"Do you not know that your body is a temple of the Holy Spirit within you, whom you have from God? You are not your own, for you were bought with a price. So glorify God in your body.\" This passage reminds us that our bodies are sacred and that we have a responsibility to honor and care for them. By embracing your Catholic faith, you recognize that your body is a gift from God, entrusted to you to be nurtured and cherished. It calls you to treat your body with respect and gratitude, knowing that your wellbeing is not just for your own benefit but also for the sake of your loved ones. In your commitment to health and self-improvement, you are dedicating yourself to glorifying God in your body. By staying active, eating healthily, and prioritizing your physical and spiritual wellness, you are honoring the life you've been given and expressing gratitude for the blessings of family and faith. Remember that as you nurture your body and mind, you are also nurturing your soul. By keeping your heart open to God's guidance, you invite His presence into your life, allowing Him to guide and strengthen you along this journey. Your commitment to health is not simply about losing weight; it is about living a more fulfilling, vibrant life that reflects the values you hold dear. May this passage serve as a reminder of the sacredness of your body and the importance of caring for it. As you continue to grow in faith and health each day, may you find comfort, strength, and guidance in your Catholic faith. Blessings on your journey, Oscar!"
            };
            Debug.WriteLine(request.ToString());
            var result = api.SaveToFile(request, "onyx.mp3").GetAwaiter().GetResult();
        }
    }
}
