using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Higgs.Core.Security;

namespace Higgs.Web.Attributes
{
    public class AuthenticatedAttribute : AuthorizeAttribute
    {
        public static List<Permission> Permission { get; set; }
        public static Guid? CurrentRoleId 
        { 
            get
            {
                return HttpContext.Current.Items["Higgs.Web.Attributes.AuthenticatedAttribute.CurrentRoleId"] as Guid?;
            } 
            set
            {
                HttpContext.Current.Items["Higgs.Web.Attributes.AuthenticatedAttribute.CurrentRoleId"] = value;
            }
        }

        protected AuthorizationContext FilterContext { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public AuthenticatedAttribute()
        {
            Order = int.MaxValue;
        }

        public override void  OnAuthorization(AuthorizationContext filterContext)
        {
            FilterContext = filterContext;

 	        base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Current logic force any request to call IsValidUser method that will store first not null current RoleId
            ControllerName = FilterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            ActionName = FilterContext.ActionDescriptor.ActionName;

            if(
                base.AuthorizeCore(httpContext) && 
                (
                    IsValidUser(httpContext) ||
                    (httpContext.User.Identity.IsAuthenticated && MvcApplication.DefaultHomeView != null && ControllerName == MvcApplication.DefaultHomeView.ControllerName && ActionName == MvcApplication.DefaultHomeView.ActionName)
                )
            )
            {
                return true;
            }

            return
                (MvcApplication.DefaultLogOnView != null && ControllerName == MvcApplication.DefaultLogOnView.ControllerName && ActionName == MvcApplication.DefaultLogOnView.ActionName) ||
                (MvcApplication.DefaultLogOffView != null && ControllerName == MvcApplication.DefaultLogOffView.ControllerName && ActionName == MvcApplication.DefaultLogOffView.ActionName);
        }

        /// <summary>
        /// Create override method to create dynamic authentication.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected virtual bool IsValidUser(HttpContextBase httpContext)
        {
            return IsAuthorized(Guid.Empty);
        }

        public virtual bool IsAuthorized()
        {
            return IsAuthorized(CurrentRoleId.HasValue ? CurrentRoleId.Value : Guid.Empty);
        }

        public virtual bool IsAuthorized(Guid roleId)
        {
            if (roleId == Guid.Empty) return true;
            if (!CurrentRoleId.HasValue)
            {
                CurrentRoleId = roleId;
            }
            
            ControllerName = ControllerName.Replace("Controller", "");
            ActionTypePattern pattern;
            var actionGroupName = !string.IsNullOrWhiteSpace(ActionName) ? AccessControl.GetActionInfo(ActionName).ActionGroupName : null;
            var accessType = AccessControl.GetAccessType(ActionName, out pattern);

            if (accessType == AccessType.Update && FilterContext != null)
            {
                if (FilterContext.ActionDescriptor.GetCustomAttributes(typeof(HttpGetAttribute), true).Length > 0)
                {
                    accessType = AccessType.Read;
                }
            }

            var permission = Permission.Where
                                    (
                                        x => (!x.RoleId.HasValue || x.RoleId.Value == roleId) &&
                                                (x.GroupName == "*" || x.GroupName.Equals(ControllerName, StringComparison.CurrentCultureIgnoreCase)) &&
                                                (x.ActionName == null || x.ActionName == "*" || x.ActionName.Equals(actionGroupName, StringComparison.CurrentCultureIgnoreCase) || x.ActionName.Equals(ActionName, StringComparison.CurrentCultureIgnoreCase)) &&
                                                (!x.ActionType.HasValue || x.ActionType.Value == (byte)accessType)
                                    )
                                    .ToList();

            return permission.Count > 0 && permission.Min(x => x.IsGrant ? 1 : -1) > 0;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }
            
            if (MvcApplication.DefaultLogOnView != null)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;

                if (returnUrl.EndsWith("/"))
                    returnUrl = returnUrl.Substring(0, returnUrl.Length - 1);

                returnUrl = returnUrl.ToUpper();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                                {
                                                    { "controller", MvcApplication.DefaultLogOnView.ControllerName },
                                                    { "action",
                                                        MvcApplication.DefaultLogOnView.ActionName +
                                                        (returnUrl.Equals(filterContext.HttpContext.Request.ApplicationPath, StringComparison.CurrentCultureIgnoreCase) ? "/returnUrl/" + returnUrl : "")
                                                    }
                                                });
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}