namespace Higgs.Web.Helpers
{
    public static class JavaScriptHelper
    {
        // Methods
        public static string ToEscapeString(this object text)
        {
            return text.ToString().ToEscapeString();
        }

        public static string ToEscapeString(this string text)
        {
            return string.Format("'{0}'", text.Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("\n", @"\n"));
        }
    }
}