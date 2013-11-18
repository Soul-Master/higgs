using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Higgs.Core.Helpers
{
    public static class StringHelper
    {
        // Return defaultValue parameter when x parameter is null or empty.
        public static string Value(this string x, string defaultValue)
        {
            return string.IsNullOrEmpty(x) ? defaultValue : x;
        }

        // Shortcut function for string.Format(x,args)
        public static string Eval(this string x, params object[] args)
        {
            return string.Format(x, args);
        }

        public static void Add(this StringBuilder sb)
        {
            sb.AppendLine();
        }

        public static void Add(this StringBuilder sb, string msg, params object[] args)
        {
            if(args.Length > 0)
            {
                sb.AppendFormat(msg + Environment.NewLine, args);
            }
            else
            {
                sb.AppendLine(msg);
            }
        }

        public static void AddWithoutNewLine(this StringBuilder sb, string msg, params object[] args)
        {
            if (args.Length > 0)
            {
                sb.AppendFormat(msg, args);
            }
            else
            {
                sb.AppendLine(msg);
            }
        }

        public static string AfterLastMatch(this string msg, char value)
        {
            var lastIndexOf = msg.LastIndexOf(value);

            return lastIndexOf < 0 ? msg : msg.Substring(lastIndexOf + 1);
        }

        /// <summary>
        ///  Message should matches one of the following patterns.
        ///  - Please input your {Name} in textbox.
        ///  - My name is {LastName|FirstName}. The expression value is value of first property that has value(not null, empty or whitespace).
        /// </summary>
        /// <param name="msg">Message to be formatted.</param>
        /// <param name="data">Data object that has specify property name.</param>
        /// <param name="defaultValue">String to be display when value of current property is null, empty or whitespace.</param>
        /// <returns></returns>
        public static string Format<T>(this string msg, T data, string defaultValue = null)
            where T : class
        {
            if (data == null) return msg;

            var type = data.GetType();
            var reg = new Regex(@"{([a-zA-Z_][a-zA-Z0-9_]*?)(\|([a-zA-Z_][a-zA-Z0-9_]*?))*?\}");
            var startIndex = 0;

            while (true)
            {
                if (startIndex < 0 || startIndex >= msg.Length) break;

                var m = reg.Match(msg, startIndex);
                startIndex = m.Index + m.Length;

                if (!m.Success) break;

                string firstValidValue = null;
                var isReplace = false;
                foreach (Group g in m.Groups)
                {
                    foreach (Capture c in g.Captures)
                    {
                        var pName = c.Value.Replace("|", "").Replace("{", "").Replace("}", "");
                        var p = type.GetProperty(pName);

                        if (p == null || !p.CanRead) continue;

                        var value = "";
                        var isValidProperty = false;

                        try
                        {
                            value = p.GetValue(data, null).ToString();
                            isValidProperty = true;

                            if (firstValidValue == null)
                            {
                                firstValidValue = value;
                            }
                        }
                        catch{}

                        if (!isValidProperty || (string.IsNullOrWhiteSpace(value) && m.Groups.Count != 1)) continue;

                        msg = msg.Substring(0, m.Index) + value + msg.Substring(m.Index + m.Length);
                        isReplace = true;
                        startIndex = m.Index + value.Length;
                        break;
                    }

                    if (isReplace) break;
                    if(defaultValue != null)
                    {
                        msg = msg.Substring(0, m.Index) + defaultValue + msg.Substring(m.Index + m.Length);
                        startIndex = m.Index + defaultValue.Length;
                        isReplace = true;
                        break;
                    }
                }

                if (isReplace || firstValidValue == null) continue;

                msg = msg.Substring(0, m.Index) + firstValidValue + msg.Substring(m.Index + m.Length);
                startIndex = m.Index + firstValidValue.Length;
            }

            return msg;
        }

        public static string Limit(this string msg, int maxLength)
        {
            if (string.IsNullOrEmpty(msg) || msg.Length < maxLength)
            {
                return msg;
            }
            
            return msg.Substring(0, maxLength);
        }

        public static string ReplaceWhiteSpace(this string msg, string replaceChar = " ")
        {
            var reg = new Regex(@"\s");

            return reg.Replace(msg, replaceChar);
        }
    }
}


