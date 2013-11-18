using System;
using System.Web;

namespace Higgs.Web
{
    public class CombinedFileInfo
    {
        public CombinedFileInfo()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string CombinedPath { get; set; }
        public CombinedFileType FileType { get; set; }
        public string[] FilePaths { get; set; }
        public string ExactFilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(CombinedPath);
            }
        }
        public string CheckSum { get; set; }
    }
}
