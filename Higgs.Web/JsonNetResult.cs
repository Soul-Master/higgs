﻿using System;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace Higgs.Web
{
    public class JsonNetResult : JsonResult
    {
        private readonly JsonSerializerSettings setting;

        public JsonNetResult(JsonSerializerSettings setting)
        {
            this.setting = setting;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, setting);
            response.Write(serializedObject);
        }
    }
}
