namespace Higgs.Web
{
    public class ResourceInfo
    {
        public ResourceInfo(string controllerName, string viewName)
        {
            ControllerName = controllerName;
            ViewName = viewName;
        }
        
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
    }
}
