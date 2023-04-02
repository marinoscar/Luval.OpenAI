using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.UnitTest
{
    public static class Util
    {
        private static SecureString key = null;

        public static SecureString Key
        {
            get
            {
                if (key == null) key = new NetworkCredential("", File.ReadAllText("private.txt")).SecurePassword;
                return key;
            }
        }
    }
}
