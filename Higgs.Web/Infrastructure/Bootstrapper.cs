using System.Web;

namespace Higgs.Web.Infrastructure
{
    public static class Bootstrapper
    {
        public static void Run(HttpApplication currentApplication)
        {
            Application = currentApplication;

            //var myContainer = new UnityContainer();
            //var section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;

            //if (section == null) return;

            //section.Containers.Default.Configure(myContainer);

            //var tasks = myContainer.ResolveAll<ITask>();
            //foreach (var t in tasks)
            //{
            //    t.Execute();
            //}
        }

        public static HttpApplication Application
        {
            get;
            private set;
        }

        public static string MapPath(string path)
        {
            return Application.Server.MapPath(path);
        }
    }
}