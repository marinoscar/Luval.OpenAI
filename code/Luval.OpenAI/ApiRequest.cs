using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public class ApiRequest : IDisposable
    {
        public HttpClient Client { get; set; }
        public HttpRequestMessage Request { get; set; }

        public void Dispose()
        {
            if(Client != null) Client.Dispose();
            if(Request != null) Request.Dispose();
            Client = null;
            Request = null;
        }
    }
}
