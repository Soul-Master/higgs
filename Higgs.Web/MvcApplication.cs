using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Higgs.Core.Helpers;
using Higgs.Web.Attributes;
using Higgs.Web.Configurations;

namespace Higgs.Web
{
    public class MvcApplication : HttpApplication
    {
        // TODO: Keep default view in Config section.
        public static ViewPageLocation DefaultHomeView { get; protected set; }
        public static ViewPageLocation DefaultLogOnView { get; protected set; }
        public static ViewPageLocation DefaultLogOffView { get; protected set; }
        public static List<string> StaticFiles { get; protected set; }

        public MvcApplication()
        {
            StaticFiles = new List<string>();
        }
        
        public void Start<TViewEngine>()
            where TViewEngine : class, IViewEngine
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(DependencyResolver.Current.GetService<TViewEngine>());

            Start();
        }
        
        public virtual void Start()
        {
            var config = HiggsWebConfigSection.Current;

            foreach (IgnorePathElement ignorePath in config.Routing.IgnorePaths)
            {
                RouteTable.Routes.IgnoreRoute(ignorePath.Path + "/{*pathInfo}");
            }
            
            ScanStaticFiles();

            #region Map custom route

            var currentController = string.Empty;
            var count = 1;

            foreach (var item in GetMapRoutes())
            {
                if (string.IsNullOrEmpty(item.RouteUrl))
                {
                    var parameters = item.ActionMethod.GetParameters();

                    if (parameters.Length == 0)
                    {
                        continue;
                    }

                    item.RouteUrl = string.Format("~/{{{0}}}", parameters[0].Name);
                }

                if (item.ControllerName != currentController)
                {
                    count = 1;
                    currentController = item.ControllerName;
                }

                RouteTable.Routes.MapRoute(string.Format("{0}_CustomRoute{1}", item.ControllerName, count), item.RouteUrl.StartsWith("~/") ? string.Format("{0}/{1}", item.ControllerName, item.ActionName) + item.RouteUrl.Substring(1) : item.RouteUrl, new { controller = item.ControllerName, action = item.ActionName });

                count++;
            }

            //TODO: Keep in config for Enable/Disable Higgs Resource
            RouteTable.Routes.MapRoute("HiggsResource", "_Resource/{action}/{controllername}/{viewname}", new { controller = "_Resource" });

            #endregion
        }

        public IEnumerable<ViewPageLocation> GetMapRoutes()
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            
            return 
                from asm in asms
                from t in asm.GetTypes()
                from m in t.GetMethods()
                from a in (MapRouteAttribute[])m.GetCustomAttributes(typeof(MapRouteAttribute), true)
                where 
                    t != null && t.IsPublic && t.Namespace != null &&  !t.IsAbstract &&
                    ControllerBuilder.Current.DefaultNamespaces.Contains(t.Namespace) && 
                    t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                let controllerName = t.Name.Substring(0, t.Name.Length - 10)
                select new ViewPageLocation
                {
                    ControllerName = controllerName, 
                    ActionName = m.Name, 
                    RouteUrl = a.RouteUrl, 
                    ActionMethod = m
                };
        }

        public void ScanStaticFiles()
        {
            var config = HiggsWebConfigSection.Current.Routing.StaticView;
            if (!config.Enable) return;

            var viewDirectory = Server.MapPath(config.ViewsFolder);
            var files = Directory.GetFiles(viewDirectory, config.StaticFilePattern, SearchOption.AllDirectories);
            var controller = DependencyResolver.Current.GetService<IStaticFileController>() ?? new BaseStaticFileController();

            if (controller == null) throw new NullReferenceException("Unable to get Static file controller instance. Please register static file controller(or IStaticFileController) in your IoC.");

            var controllerType = controller.GetType();
            AddControllerNs(controllerType.Namespace);
            var controllerName = controllerType.Name;
            controllerName = controllerName.Substring(0, controllerName.Length - 10);
            var webDir = Server.MapPath("~/");
            var excludeFolders = new List<string>();

            for(var i = 0;i < config.ExcludeFolders.Count; i++)
            {
                var p = config.ExcludeFolders[i].Path;

                if (!p.StartsWith("~/")) p = config.ViewsFolder + (p.StartsWith("/") ? p : "/" + p);

                excludeFolders.Add(p);
            }

            for (var i = 0; i < files.Length; i++)
            {
                var viewPath = IOHelpers.GetLogicalPath(webDir, files[i]);

                if (excludeFolders.Any(exFolder => viewPath.ToLower().StartsWith(exFolder.ToLower()))) break;

                var viewUrl = GetViewUrl(viewPath);
                var fileName = GetFileName(viewUrl);

                if(fileName.ToUpper() == config.DefaultFileName.ToUpper())
                {
                    RouteTable.Routes.MapRoute
                    (
                        "__StaticPage" + i + "_default",
                        viewUrl.Replace(fileName, string.Empty),
                        new { controller = controllerName, action = "DefaultView", path = viewPath },
                        new { httpMethod = new HttpMethodConstraint("GET") }
                    );
                }

                RouteTable.Routes.MapRoute
                (
                    "__StaticPage" + i,
                    viewUrl,
                    new { controller = controllerName, action = "DefaultView", path = viewPath },
                    new { httpMethod = new HttpMethodConstraint("GET") }
                );

                StaticFiles.Add(viewPath);
            }
        }

        private static string GetViewUrl(string logicalPath)
        {
            var config = HiggsWebConfigSection.Current.Routing.StaticView;
            logicalPath = logicalPath.Replace(config.ViewsFolder + "/", string.Empty);

            var lastIndexOfDir = logicalPath.LastIndexOf('/');
            var lastIndexOfExtension = logicalPath.IndexOf('.', lastIndexOfDir);

            return logicalPath.Substring(0, lastIndexOfExtension);
        }

        private static string GetFileName(string viewUrl)
        {
            return viewUrl.Substring(viewUrl.LastIndexOf('/') + 1);
        }

        public void Application_EndRequest(object sender, EventArgs e)
        {
            if (Response.StatusCode == CustomHttpStatusCode.UnauthorizedUser)
            {
                Response.StatusCode = User.Identity.IsAuthenticated ? 403 : 401;
            }
        }

        public void AddControllerNs<TController>()
        {
            var controllerType = typeof (TController);

            AddControllerNs(controllerType.Namespace);
        }

        public void AddControllerNs(string name)
        {
            if (!ControllerBuilder.Current.DefaultNamespaces.Contains(name)) ControllerBuilder.Current.DefaultNamespaces.Add(name);
        }
    }
}
