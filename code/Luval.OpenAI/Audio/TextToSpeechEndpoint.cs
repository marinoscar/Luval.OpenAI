using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Audio
{
    public class TextToSpeechEndpoint : ApiRequestBase
    {
        public TextToSpeechEndpoint(ApiAuthentication authentication, string endpoint = "https://api.openai.com/v1/audio/speech") : base(authentication, endpoint)
        {
        }

        public async Task<IReadOnlyList<byte>> SendAsync(TextToSpeechRequest request)
        {
            var response = await PostRequestAsync(request);
            return await LoadResponse(response);
        }

        public async Task<FileStream> SendToFileStreamAsync(TextToSpeechRequest request, string fileName)
        {
            var response = await PostRequestAsync(request);
            var result = new FileStream(path: fileName, mode: FileMode.OpenOrCreate, access: FileAccess.Write);
            await response.Content.CopyToAsync(result);
            return result;
        }

        public async Task<FileInfo> SaveToFile(TextToSpeechRequest request, string fileName)
        {
            using (var file = await SendToFileStreamAsync(request, fileName))
            {
                file.Close();
            }
            return new FileInfo(fileName);
        }

        protected virtual async Task<IReadOnlyList<byte>> LoadResponse(HttpResponseMessage message)
        {
            var result = new List<byte>();
            using (var memory = new MemoryStream())
            {
                await message.Content.CopyToAsync(memory);
                result.AddRange(memory.ToArray());
            }
            return result;
        }

        public static TextToSpeechEndpoint CreateOpenAI(ApiAuthentication authentication)
        {
            return new TextToSpeechEndpoint(authentication);
        }

        public static TextToSpeechEndpoint CreateOpenAI(string key)
        {
            return new TextToSpeechEndpoint(new ApiAuthentication(key));
        }
    }
}
