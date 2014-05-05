using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Higgs.Web
{
    public class HiggsResult : JsonNetResult
    {
        public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        // Methods
        public HiggsResult()
            : base(SerializerSettings)
        {
            ErrorList = new Dictionary<string, List<string>>();
            IsSuccess = true;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Data = new { ErrorList, IsSuccess, RedirectTo, Data = ResultData };
            
            base.ExecuteResult(context);
        }

        // Properties
        public Dictionary<string, List<string>> ErrorList { get; set; }

        public bool IsSuccess { get; set; }

        public string RedirectTo { get; set; }

        public object ResultData { get; set; }
    }
}
