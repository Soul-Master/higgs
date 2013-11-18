using System;
using System.Linq;

namespace Higgs.Core.Security
{
    public static class BaseEncoder
    {
        #region Base62 

        private const string Base62Set = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string EncodeToBase62(this uint input)
        {
            return ((long)input).Encode(Base62Set);
        }

        public static string EncodeToBase62(this long input)
        {
            return input.Encode(Base62Set);
        }

        public static long DecodeAsBase62(this string input)
        {
            return input.Decode(Base62Set);
        }

        #endregion

        public static string Encode(this long input, string baseChars)
        {
            var r = string.Empty;
            var targetBase = baseChars.Length;

            do
            {
                r = string.Format("{0}{1}", baseChars[(int)(input % targetBase)], r);
                input /= targetBase;
            }
            while (input > 0);

            return r;
        }

        public static long Decode(this string input, string baseChars)
        {
            var srcBase = baseChars.Length;
            var r = (string)input.Reverse();

            return input.Select((t, i) => baseChars.IndexOf(r[i])).Select((charIndex, i) => charIndex*(long) Math.Pow(srcBase, i)).Sum();
        }
    }
}
