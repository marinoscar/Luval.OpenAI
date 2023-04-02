using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public class ApiAuthentication
    {

        public ApiAuthentication(SecureString key) : this(key, null)
        {

        }

        public ApiAuthentication(SecureString key, string organization)
        {
            Key = key;
            Organization = organization;
        }

        public SecureString Key { get; private set; }
        public string Organization { get; set; }

        public string GetKey() { return new NetworkCredential("", Key).Password; }
    }
}
