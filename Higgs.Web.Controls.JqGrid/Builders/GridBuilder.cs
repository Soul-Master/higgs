using System.Web;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid.Builders
{
    public class GridBuilder : BuilderBase, IHtmlString
    {
        public Grid CurrentGrid;
        public static string DefaultPagerName = "{0}-pager";

        public GridBuilder(Grid g)
        {
            CurrentGrid = g;
        }

        public override string ToString()
        {
            return CurrentGrid.ToString();
        }
        
        public string ToHtmlString()
        {
            return CurrentGrid.ToString();
        }
    }
}
