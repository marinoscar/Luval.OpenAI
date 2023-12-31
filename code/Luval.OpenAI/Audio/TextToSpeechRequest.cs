using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Audio
{
    public class TextToSpeechRequest
    {
        private float _speed;

        public TextToSpeechRequest()
        {
            Model = SpeechModel.TTSHD;
            Voice = Voice.onyx;
            Format = AudioOutputFormat.mp3;
            Speed = 1;
        }

        public TextToSpeechRequest(string input) : this()
        {
            Input = input;
        }

        [JsonProperty("model")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SpeechModel Model { get; set; }

        [JsonProperty("input")]
        public string? Input { get; set; }

        [JsonProperty("voice")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Voice Voice { get; set; }

        [JsonProperty("response_format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AudioOutputFormat Format { get; set; }

        [JsonProperty("speed")]
        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value < 0.25 || value > 4) throw new ArgumentOutOfRangeException(nameof(Speed), $"{nameof(Speed)} needs to be between 0.25 and 4.0");
                _speed = value;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
