using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public class ApiResponseData
    {
        public string Organization { get; set; }
        public string RequestId { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public string OpenAIVersion { get; set; }
        public string ModelId { get; set; }

        public static ApiResponseData TryToLoad(HttpResponseMessage response)
        {
            var r = new ApiResponseData();
            try
            {
                r.RequestId = response.Headers.GetValues("X-Request-ID").FirstOrDefault();
                r.ProcessingTime = TimeSpan.FromMilliseconds(int.Parse(response.Headers.GetValues("Openai-Processing-Ms").First()));
                r.Organization = response.Headers.GetValues("Openai-Organization").FirstOrDefault();
                r.ModelId = response.Headers.GetValues("Openai-Model").FirstOrDefault();
                r.OpenAIVersion = response.Headers.GetValues("Openai-Version").FirstOrDefault();
            }
            catch
            {
                Debug.WriteLine("FAILED TO EXTRACT HEADER DATA");
            }
            return r;
        }

        internal ApiResponseData()
        {

        }
    }
}
