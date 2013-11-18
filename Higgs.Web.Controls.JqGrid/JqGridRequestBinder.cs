using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Higgs.Core.Helpers;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid
{
    public class JqGridRequestBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request;
                var parentId = request["parentid"];
                
                if(!string.IsNullOrEmpty(parentId))
                {
                    if(parentId == "NULL")
                    {
                        parentId = null;
                    }
                }

                var data = new JqGridRequest
                {
                    IsSearch = bool.Parse(request["_search"] ?? "false"),
                    IsExport = bool.Parse(request["_export"] ?? "false"),
                    PageIndex = int.Parse(request["page"] ?? "1"),
                    PageSize = int.Parse(request["rows"] ?? "10"),
                    SortColumn =  request["sidx"] ?? string.Empty,
                    SortOrder = request["sord"] ?? "asc",
                    Where = GridFilter.Create(request["filters"] ?? string.Empty),
                    NodeId = request["nodeid"],
                    ParentId = parentId,
                    NodeLevel = int.Parse(request["n_level"] ?? "0")
                };

                if(data.IsExport)
                {
                    var colData = new List<ExportColumnModel>();

                    request.Form.AllKeys
                        .Where(x => x.StartsWith("colModel[") && x.EndsWith(".name"))
                        .ForEach(key =>
                        {
                            var startIndex = key.IndexOf("[");
                            var index = key.Substring(startIndex + 1, key.IndexOf("]") - startIndex - 1);

                            colData.Add(new ExportColumnModel
                            {
                                Name = request[key],
                                Title = request[string.Format("colModel[{0}].title", index)]
                            });
                        });

                    data.ColumnModel = colData;
                }

                return data;
            }
            catch
            {
                return null;
            }
        }
    }
}