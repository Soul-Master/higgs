using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Higgs.Web.Attributes;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public static class ControlBaseHelper
    {
        public static T Visible<T>(this T control, bool isVisible = true)
            where T : ControlBase
        {
            control.IsVisible = isVisible;

            return control;
        }

        public static T Visible<T, TController>(this T control, Expression<Func<TController, object>> routeExpression)
            where T : ControlBase
            where TController : IController
        {
            string controllerName, actionName;
            routeExpression.GetLogicalPath(out controllerName, out actionName);

            var att = new AuthenticatedAttribute
            {
                ControllerName = controllerName,
                ActionName = actionName
            };
            control.IsVisible = att.IsAuthorized();

            return control;
        }

        public static T TabIndex<T>(this T control, uint index)
            where T : ControlBase
        {
            control.IsSettedTabIndex = true;

            if (control is InputControlBase)
            {
                var inputControl = control as InputControlBase;
                inputControl.MainControl.Attributes["tabindex"] = index.ToString();
            }
            else if(!control.IsSingleTag)
            {
                control.ListTags[0].Attributes["tabindex"] = index.ToString();
            }
            else
            {
                control.Attributes["tabindex"] = index.ToString();
            }

            return control;
        }

        public static T WaterMark<T>(this T control, string markText)
            where T : HiggsInput
        {
            return control.WaterMark(markText, null);
        }

        public static T WaterMark<T>(this T control, string markText, string displayControlId)
            where T : HiggsInput
        {
            HiggsScriptManager.AddjQueryObjectScript
            (
                control,
                ".waterMark({0}{1})",
                markText.ToEscapeString(),
                string.IsNullOrEmpty(displayControlId) ? string.Empty : ", '#" + displayControlId + "'"
            );

            return control;
        }

        public static bool HasClass<T>(this T control, string className)
            where T : InputControlBase
        {
            if (control.MainControl.Attributes.ContainsKey("class"))
            {
                var cssClass = control.MainControl.Attributes["class"];

                if (cssClass != null)
                {
                    return cssClass.Split(' ').Any(c => c == className);
                }
            }

            return false;
        }

        internal static TagBuilder SetCss(this TagBuilder tag, object style)
        {
            var css = new RouteValueDictionary(style);
            var temp = new StringBuilder();
            Func<string, string> getStyleName = s =>
            {
                var styleName = string.Empty;

                foreach(var c in s)
                {
                    if(char.IsUpper(c))
                    {
                        styleName += "-";
                    }

                    styleName += char.ToLower(c);
                }

                return styleName;
            };

            foreach (var x in css)
            {
                temp.Append(string.Format("{0}: {1};", getStyleName(x.Key), x.Value));
            }

            if (tag.Attributes.ContainsKey("style"))
            {
                tag.Attributes["style"] = tag.Attributes["style"] + temp;
            }
            else
            {
                tag.Attributes.Add("style", temp.ToString());
            }
            
            return tag;
        }

        public static T Style<T>(this T control, object style)
            where T : ControlBase
        {
            if (control is InputControlBase)
            {
                var controlBase = control as InputControlBase;
                
                controlBase.MainControl.SetCss(style);
            }
            else
            {
                if(control.IsSingleTag)
                {
                    control.SetCss(style);
                }
                else
                {
                    control.ListTags[0].SetCss(style);   
                }
            }

            return control;
        }
    }
}
