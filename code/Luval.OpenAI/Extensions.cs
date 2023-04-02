using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI
{
    public static class Extensions
    {
        public static DateTime? ToDateTimeFromUnix(this long? value)
        {
            if(value == null) return null;
            return DateTimeOffset.FromUnixTimeSeconds(value.Value).DateTime;
        }
    }
}
