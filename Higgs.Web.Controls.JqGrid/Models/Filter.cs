using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Higgs.Web.Controls.JqGrid.Models
{
    [DataContract]
    public class Filter
    {
        // ReSharper disable InconsistentNaming

        [DataMember]
        public string groupOp { get; set; }

        [DataMember]
        public Rule[] rules { get; set; }

        // ReSharper restore InconsistentNaming

        public static Filter Create(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData)) return null;

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Filter));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

                return serializer.ReadObject(ms) as Filter;
            }
            catch
            {
                return null;
            }
        }
    }
}
