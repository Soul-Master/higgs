namespace Higgs.Web.Controls.JqGrid
{
    public class JqGridResult
    {
// ReSharper disable InconsistentNaming

        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public object rows { get; set; }
        public object userdata { get; set; }

 // ReSharper restore InconsistentNaming
    }
}