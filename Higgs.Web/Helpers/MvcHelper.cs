using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Higgs.Core.Helpers;
using Higgs.Web.Attributes;

namespace Higgs.Web.Helpers
{
    public static class MvcHelper
    {
        /// <summary>
        /// Use to get current action name by library outside MVC context.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentActionName()
        {
            if (HttpContext.Current == null) return null;

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            return routeData != null ? routeData.Values["action"].ToString() : null;
        }
        
        /// <summary>
        /// Use to get current action name by library outside MVC context.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentActionGroupName()
        {
            var actionName = GetCurrentActionName();

            if(actionName == null) return null;

            return AccessControl.GetActionInfo(actionName).ActionGroupName;
        }

        public static bool IsAuthorized<TController>(Expression<Func<TController, object>> routeExpression)
            where TController : IController
        {
            string controllerName, actionName;
            routeExpression.GetLogicalPath(out controllerName, out actionName);

            return IsAuthorized(controllerName, actionName);
        }

        public static bool IsAuthorized(string controllerName, string actionName)
        {
            var att = new AuthenticatedAttribute
            {
                ControllerName = controllerName,
                ActionName = actionName
            };
            
            return att.IsAuthorized();
        }

        public static void AddError<TModel>(this ModelStateDictionary modelStateDic, Expression<Func<TModel, object>> selectedPropertyExpression, string errorMessage)
        {
            var result = CoreExpressionHelper.GetMemberInfoesFromExpression(selectedPropertyExpression);
            var affectedKeys = result.Aggregate("", (current, mi) => current + (mi.Name + "|"));

            modelStateDic.AddModelError(affectedKeys.Substring(0, affectedKeys.Length - 1), errorMessage);
        }

        public static HiggsResult AddError<TModel>(this HiggsResult result, Expression<Func<TModel, object>> selectedPropertyExpression, string errorMessage)
        {
            var temp = CoreExpressionHelper.GetMemberInfoesFromExpression(selectedPropertyExpression);
            var affectedKeys = temp.Aggregate("", (current, mi) => current + (mi.Name + "|"));

            result.AddCustomError(affectedKeys.Substring(0, affectedKeys.Length - 1), errorMessage);

            return result;
        }

        public static HiggsResult Reload(this HiggsResult result)
        {
            result.RedirectTo = "";

            return result;
        }

        public static HiggsResult Redirect(this HiggsResult result, string url)
        {
            result.RedirectTo = url;

            return result;
        }

        public static HiggsResult Redirect<TController>(this HiggsResult result, Expression<Func<TController, object>> routeExp)
            where TController : BaseController
        {
            return result.Redirect(routeExp.GetLogicalPath());
        }

        public static HiggsResult Redirect(this HiggsResult result, ViewPageLocation location)
        {
            return result.Redirect(String.Format("~/{0}/{1}", location.ControllerName, location.ActionName));
        }

        public static HiggsResult AddCustomError(this HiggsResult result, string errorMessage)
        {
            if (!result.ErrorList.ContainsKey("custom-error"))
            {
                result.ErrorList.Add("custom-error", new List<string>());
            }

            result.ErrorList["custom-error"].Add(errorMessage);
            result.IsComplete = false;

            return result;
        }

        public static HiggsResult AddCustomError(this HiggsResult result, string key, string errorMessage)
        {
            if (!result.ErrorList.ContainsKey(key))
            {
                result.ErrorList.Add(key, new List<string>());
            }

            result.ErrorList[key].Add(errorMessage);
            result.IsComplete = false;

            return result;
        }

        public static HiggsResult AddCustomMsg(this HiggsResult result, string message)
        {
            const string key = "custom-msg";

            if (!result.ErrorList.ContainsKey(key))
            {
                result.ErrorList.Add(key, new List<string>());
            }
            result.ErrorList[key].Add(message);

            return result;
        }

        public static HiggsResult Data(this HiggsResult result, object data)
        {
            result.ResultData = data;

            return result;
        }

        public static HiggsResult AllowGet(this HiggsResult result, bool flag = true)
        {
            result.JsonRequestBehavior = flag ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet;

            return result;
        }

        public static bool IsFormDebug(this HttpRequest request)
        {
            bool isDebug;

            return Boolean.TryParse(request["Debug"], out isDebug) && isDebug;
        }

        public static bool IsJsonRequest(this ControllerContext context)
        {
            return context.RequestContext.HttpContext.Request.ContentType.Contains("application/json") ||
                context.RequestContext.HttpContext.Request.AcceptTypes != null &&
                (
                    context.RequestContext.HttpContext.Request.AcceptTypes.Contains("text/javascript") ||
                    context.RequestContext.HttpContext.Request.AcceptTypes.Contains("application/json")
                );
        }

        public static SelectList GetSelectListFromEnum(Type enumType)
        {
            var list = (
                            from byte val in Enum.GetValues(enumType)
                            select new SelectListItem
                            {
                                Value = val.ToString(), Text = Enum.GetName(enumType, val)
                            }
                        ).ToList();

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList InsertItem(this SelectList list, SelectListItem item, int index = 0)
        {
            var temp = list.ToList();
            temp.Insert(index, item);
            
            return new SelectList(temp, "Value", "Text");
        }

        public static SelectList AddItem(this SelectList list, SelectListItem item)
        {
            var temp = list.ToList();
            temp.Add(item);

            return new SelectList(temp, "Value", "Text");
        }

        // Original method in locate in System.Web.Mvc.ControllerTypeCache class
        public static bool IsControllerType(Type t)
        {
            return
                t != null &&
                t.IsPublic &&
                t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                !t.IsAbstract &&
                typeof(IController).IsAssignableFrom(t);
        }

        // Copy from System.Web.Mvc.ActionMethodSelector class
        public static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            var declaringType = methodInfo.GetBaseDefinition().DeclaringType;

            return declaringType != null && !(methodInfo.IsSpecialName || declaringType.IsAssignableFrom(typeof(Controller)));
        }

        public static SelectList ToSelectList(this IEnumerable items, string dataValueField = "Value", string dataTextField = "Text")
        {
            return new SelectList(items, dataValueField, dataTextField);
        }

        public static MvcHtmlString SelectedOption(this HtmlHelper helper, bool condition)
        {
            if (!condition) return null;

            return new MvcHtmlString("selected=\"selected\"");
        }

        public static MvcHtmlString Checked(this HtmlHelper helper, bool condition)
        {
            if (!condition) return null;

            return new MvcHtmlString("checked=\"checked\"");
        }
    }
}
