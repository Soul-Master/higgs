using System.Runtime.Serialization;

namespace Higgs.Web.Controls.JqGrid.Models
{
    [DataContract]
    public class Rule
    {
        // ReSharper disable InconsistentNaming

        [DataMember]
        public string field { get; set; }
        [DataMember]
        public string op { get; set; }
        [DataMember]
        public string data { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
