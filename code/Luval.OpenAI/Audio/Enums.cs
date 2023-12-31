using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Audio
{
    public enum SpeechModel
    {
        [EnumMember(Value = "tts-1")]
        TTS = 0,
        [EnumMember(Value = "tts-1-hd")]
        TTSHD = 1,
    }

    public enum Voice
    {
        alloy, 
        echo, 
        fable, 
        onyx, 
        nova,
        shimmer
    }

    public enum AudioOutputFormat
    {
        mp3 = 0, opus, aac, flac
    }
}
