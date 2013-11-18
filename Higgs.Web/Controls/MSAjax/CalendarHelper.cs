namespace Higgs.Web.Controls.MSAjax
{
    public static class CalendarHelper
    {
        public static string CssCalendar = "calendar";

        public static HiggsInput Calendar(this HiggsInput txt, string afterCreateScript = null)
        {
            txt.TurnOffAutoComplete();

            HiggsScriptManager.AddRequiredStyleSheet
            (
                "~/Scripts/MSAjax/extended/Calendar/Calendar.css"
            );

            var options = string.Format("format: '{0}'", System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern);

            HiggsScriptManager.AddScript
            (
                "MSAjax",
                 "Sys.require(Sys.components.calendar, function() " +
                 "{{" +
                 "   $('#{0}').calendar({{{1}}});" +
                 (!string.IsNullOrEmpty(afterCreateScript) ? afterCreateScript : string.Empty) +
                "}});",
                txt.Id,
                options
            );

            txt.AddCssClass(CssCalendar);
            txt.TurnOffAutoComplete();

            return txt;
        }
    }
}
