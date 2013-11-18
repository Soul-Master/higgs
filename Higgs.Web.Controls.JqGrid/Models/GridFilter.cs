using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Higgs.Web.Controls.JqGrid.Models
{
    [DataContract]
    public class GridFilter
    {
        // ReSharper disable InconsistentNaming

        [DataMember]
        public string groupOp { get; set; }

        [DataMember]
        public Rule[] rules { get; set; }

        // ReSharper restore InconsistentNaming

        public static GridFilter Create(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData)) return null;

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(GridFilter));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

                return serializer.ReadObject(ms) as GridFilter;
            }
            catch
            {
                return null;
            }
        }
    }
}
