using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Higgs.Web.Helpers
{
    public static class ExpressionHelpers
    {
        public static string GetLogicalPath<TController>(this Expression<Func<TController, object>> routeExpression)
            where TController : IController
        {
            string controllerName, actionName;

            return routeExpression.GetLogicalPath(out controllerName, out actionName);
        }

        public static string GetLogicalPath<TController>(this Expression<Func<TController, object>> routeExpression, out string controllerName, out string actionName)
            where TController : IController
        {
            var part = routeExpression.Body;

            if (part.NodeType == ExpressionType.Call)
            {
                var callExpression = (MethodCallExpression)part;
                var actionMethod = callExpression.Method;
                var result = "~/";

                controllerName = typeof(TController).Name.Replace("Controller", "");
                actionName = actionMethod.Name;

                result += "{controller}/{action}/{id}";

                EvalRouteUrl(ref result, callExpression);

                return result;
            }
            
            throw new Exception(string.Format(Resources.ExpressionTypeIsNotEqual, "method call", part.NodeType));
        }

        private static bool EvalRouteUrl(ref string url, MethodCallExpression callExp)
        {
            var oldUrl = url;
            url = url.Replace("{controller}", callExp.Method.ReflectedType.Name.Replace("Controller", string.Empty));
            url = url.Replace("{action}", callExp.Method.Name);

            var methodPars = callExp.Method.GetParameters();
            for (var i = 0; i < methodPars.Length; i++)
            {
                if (!url.Contains("{" + methodPars[i].Name + "}")) continue;

                var currentValue = string.Empty;

                try
                {
                    currentValue = callExp.Arguments[i].GetValue().ToString();
                }
                catch { }

                url = url.Replace("{" + methodPars[i].Name + "}", currentValue);
            }

            // remove all undefined variable
            var removeUnused = new Regex(@"{[\s\S]+?}");
            url = removeUnused.Replace(url, string.Empty);

            return url == oldUrl;
        }

        public static IEnumerable<MemberInfo> GetMemberInfoesFromNewExpression<T>(this Expression<Func<T,object>> expression)
        {
            var ne = expression.Body as NewExpression;

            if (ne == null)
                throw new Exception(string.Format(Resources.ExpressionTypeIsNotEqual, "new expression", expression.Body.NodeType));

            foreach (var me in ne.Arguments.Select(arg => arg as MemberExpression))
            {
                if (me == null || me.Expression != expression.Parameters[0])
                    throw new Exception(Resources.ParameterTypeInNewExpressionIsInvalid);

                yield return me.Member;
            }
        }

        public static object GetValue(this Expression exp)
        {
            try
            {
                return Expression.Lambda(exp).Compile().DynamicInvoke();
            }
            catch
            {
                return null;
            }
        }
    }
}
