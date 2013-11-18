using System.Collections.Generic;

namespace Higgs.Web
{
    public class ViewHierarchy
    {
        public ViewHierarchy(params string[] hierarchy)
        {
            Hierarchy = new LinkedList<string>(hierarchy);
        }

        public LinkedList<string> Hierarchy { get; set; }
    }
}
