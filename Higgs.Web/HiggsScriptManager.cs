using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Higgs.Core.Helpers;
using Higgs.Web.Controls;
using System.Web.Mvc;

namespace Higgs.Web
{
    public class HiggsScriptManager
    {
        public static MvcHtmlString Create(int runningScriptDelay = 0)
        {
            var sb = new StringBuilder();

            if (RequiredScript != null)
            {
                foreach (var scriptPath in RequiredScript)
                {
                    sb.Add("<script type=\"text/javascript\" src=\"{0}\"></script>", VirtualPathUtility.ToAbsolute(scriptPath, HttpContext.Current.Request.ApplicationPath));
                }
                RequiredScript = new List<string>();
            }

            if (RequiredStyleSheet != null)
            {
                foreach (var styleSheetPath in RequiredStyleSheet)
                {
                    sb.Add("<link rel=\"Stylesheet\" href=\"{0}\">", VirtualPathUtility.ToAbsolute(styleSheetPath, HttpContext.Current.Request.ApplicationPath));
                }
                RequiredStyleSheet = new List<string>();
            }

            sb.Add();

            var oldLength = sb.Length;
            var hasScript = false;
            sb.Add("<script type=\"text/javascript\">");
            sb.Add("$(function()");
            sb.Add("{");

            if (runningScriptDelay > 0)
            {
                sb.Add("setTimeout(function()");
                sb.Add("{");
            }

            if (Script != null)
            {
                if (Script.Length > 0)
                {
                    sb.Add(Script.ToString());
                    Script = null;
                    hasScript = true;
                }
            }

            if (GroupingScripts != null && GroupingScripts.Count > 0)
            {                
                foreach (var script in GroupingScripts.OrderBy(x => x.Key))
                {
                    sb.Add(script.Value.ToString());
                }

                GroupingScripts = null;
                hasScript = true;
            }

            if(runningScriptDelay > 0)
            {
                sb.Add("}}, {0});", runningScriptDelay.ToString());
            }

            sb.Add("});");
            sb.Add("</script>");

            if(!hasScript)
            {
                sb = sb.Remove(oldLength, sb.Length - oldLength);
            }

            SerializedType = null;

            return new MvcHtmlString(sb.ToString());
        }

        protected static StringBuilder Script
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.Script"] as StringBuilder;
            } 
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.Script"] = value;
            }
        }

        public static Dictionary<string, StringBuilder> GroupingScripts
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.GroupingScripts"] as Dictionary<string, StringBuilder>;
            }
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.GroupingScripts"] = value;
            }
        }

        public static List<Type> SerializedType
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.SerializedType"] as List<Type>;
            }
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.SerializedType"] = value;
            }
        }

        public static List<string> RequiredScript
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.RequiredScript"] as List<string>;
            }
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.RequiredScript"] = value;
            }
        }

        public static List<string> RequiredStyleSheet
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.RequiredStyleSheet"] as List<string>;
            }
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.RequiredStyleSheet"] = value;
            }
        }

        public static void AddRequiredScript(params string[] scriptPath)
        {
            foreach(var s in scriptPath)
            {
                if (RequiredScript == null)
                    RequiredScript = new List<string>();

                if (!RequiredScript.Contains(s))
                    RequiredScript.Add(s);
            }
        }

        public static void AddRequiredStyleSheet(params string[] styleSheetPath)
        {
            foreach (var s in styleSheetPath)
            {
                if (RequiredStyleSheet == null)
                    RequiredStyleSheet = new List<string>();

                if (!RequiredStyleSheet.Contains(s))
                    RequiredStyleSheet.Add(s);
            }
        }

        public static void AddScript(string script)
        {
            if (Script == null)
                Script = new StringBuilder();

            Script.AppendLine(script);
        }

        public static void AddScript(string groupName, string script)
        {
            if (GroupingScripts == null)
                GroupingScripts = new Dictionary<string, StringBuilder>();

            if (GroupingScripts.ContainsKey(groupName))
            {
                GroupingScripts[groupName].Add(script);
            }
            else
            {
                GroupingScripts.Add(groupName, new StringBuilder(script));
            }
        }

// ReSharper disable MethodOverloadWithOptionalParameter
        public static void AddScript(string groupName, string script, params object[] args)
// ReSharper restore MethodOverloadWithOptionalParameter
        {
            if (GroupingScripts == null)
                GroupingScripts = new Dictionary<string, StringBuilder>();

            if (GroupingScripts.ContainsKey(groupName))
            {
                GroupingScripts[groupName].Add(script, args);
            }
            else
            {
                var sb = new StringBuilder();
                sb.Add(script, args);

                GroupingScripts.Add(groupName, sb);
            }
        }

        public static void InsertScript(string insertBeforeGroupName, string groupName, string script)
        {
            AddScript(groupName, script);
            MoveGroupingScript(groupName, insertBeforeGroupName);
        }

// ReSharper disable MethodOverloadWithOptionalParameter
        public static void InsertScript(string insertBeforeGroupName, string groupName, string script, params object[] args)
// ReSharper restore MethodOverloadWithOptionalParameter
        {
            AddScript(groupName, script, args);
            MoveGroupingScript(groupName, insertBeforeGroupName);
        }

        public static void MoveGroupingScript(string movedGroupName, string insertBeforeGroupName)
        {
            var newGroupingScript = new Dictionary<string, StringBuilder>();
            var targetGroupingScript = GroupingScripts[movedGroupName];
            var match = false;

            if (string.IsNullOrEmpty(insertBeforeGroupName))
            {
                newGroupingScript.Add(movedGroupName, targetGroupingScript);
                match = true;
            }

            foreach (var script in GroupingScripts.Where(script => script.Key != movedGroupName))
            {
                if (script.Key == insertBeforeGroupName)
                {
                    newGroupingScript.Add(movedGroupName, targetGroupingScript);
                    match = true;
                }

                if (script.Key != null)
                    if (!newGroupingScript.ContainsKey(script.Key)) 
                        newGroupingScript.Add(script.Key, script.Value);
            }

            if (!match)
            {
                newGroupingScript.Add(movedGroupName, targetGroupingScript);
            }

            GroupingScripts = newGroupingScript;
        }

        public static bool HasScriptGroup(string groupName)
        {
            return GroupingScripts != null && GroupingScripts.ContainsKey(groupName);
        }

        protected static string LastjQuerySelector
        {
            get
            {
                return HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.LastjQuerySelector"] as string;
            }
            set
            {
                HttpContext.Current.Items["Higgs.Web.HiggsScriptManager.LastjQuerySelector"] = value;
            }
        }

        public static void AddjQueryObjectScript(ControlBase control, string script, params object[] functionPars)
        {
            AddjQueryObjectScript(control.Id, control.ScriptControlId, script, functionPars);
        }

        public static void AddjQueryObjectScript(string id, string scriptControlId, string script, params object[] functionPars)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            if (LastjQuerySelector == null || LastjQuerySelector != id)
            {
                if (scriptControlId.StartsWith(".") || scriptControlId.StartsWith("#"))
                {
                    AddScript("_" + id, "$('{0}');", scriptControlId);
                }
                else
                {
                    AddScript("_" + id, "$('#{0}');", scriptControlId);
                }
            }

            if (!script.EndsWith(";"))
                script += ";";

            GroupingScripts["_" + id] = GroupingScripts["_" + id].Remove(GroupingScripts["_" + id].Length - 3, 1);
            GroupingScripts["_" + id].Add("\t" + script, functionPars);

            LastjQuerySelector = id;
        }
    }
}
