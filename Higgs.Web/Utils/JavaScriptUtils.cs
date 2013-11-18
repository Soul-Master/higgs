using System.Text;

namespace Higgs.Web.Utils
{
    public class JavaScriptUtils
    {
        ///  FUNCTION Enquote Public Domain 2002 JSON.org 
        ///  @author JSON.org 
        ///  @version 0.1 
        ///  Ported to C# by Are Bjolseth, teleplan.no 
        /// <summary>
        /// Produce a string in double quotes with backslash sequences in all the right places.
        /// </summary>
        /// <param name="s">A String</param>
        /// <returns>A String correctly formatted for insertion in a JSON message.</returns>
        public static string Enquote(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "\"\"";
            }
            var len = s.Length;
            var sb = new StringBuilder(len + 4);

            sb.Append('"');
            for (var i = 0; i < len; i += 1)
            {
                var c = s[i];
                switch (c)
                {
                    case '>':
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            //t = "000" + Integer.toHexString(c);
                            var tmp = new string(c, 1);
                            string t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }
    }
}
