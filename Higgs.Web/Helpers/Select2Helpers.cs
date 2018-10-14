using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Higgs.Core;
using Newtonsoft.Json;

namespace Higgs.Web.Helpers
{
    public static class Select2Helpers
    {
        public static JsonNetResult ToSelect2Result<T>(this IEnumerable<T> list, Select2RequestModel model)
            where T : IKey
        {
            var keywords = (model.SearchTerm ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var temp =
            (
                from item in list
                where keywords.All(y =>
                 {
                     y = y.ToUpper();

                     return item.ToString().ToUpper().Contains(y);
                 })
                select new Select2Result
                {
                    Id = item.Id.ToString(),
                    Text = item.ToString()
                }
            );

            var count = temp.Count();
            var result = new Select2PagedResult
            {
                Results = temp.Skip(model.PageNum * model.PageSize).Take(model.PageSize).ToList()
            };
            result.Pagination.More = count > model.PageNum * model.PageSize + model.PageSize;

            return new JsonNetResult(HiggsResult.SerializerSettings)
            {
                Data = result
            };
        }

        public static JsonNetResult ToSelect2Result(this IEnumerable<KeyValuePair<string, string>> list,
            string searchTerm, Select2RequestModel model)
        {
            var keywords = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return list.Where(x => keywords.All(y =>
            {
                y = y.ToUpper();

                return x.Value.ToUpper().Contains(y);
            })).ToSelect2Result(model);
        }

        public static JsonNetResult ToSelect2Result(this IEnumerable<KeyValuePair<string, string>> list, Select2RequestModel model)
        {
            var count = list.Count();

            var result = new Select2PagedResult
            {
                Results = list.Skip(model.PageNum * model.PageSize).Take(model.PageSize)
                .Select(x => new Select2Result
                {
                    Id = x.Key,
                    Text = x.Value
                })
                .ToList()
            };
            result.Pagination.More = count > model.PageNum * model.PageSize + model.PageSize;

            return new JsonNetResult(HiggsResult.SerializerSettings)
            {
                Data = result
            };
        }
    }

    public interface ISelect2Result
    {
        string Id { get; set; }
        string Text { get; set; }
    }

    public class Select2Result : ISelect2Result
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public class Select2Pagination
    {
        public bool More { get; set; }
    }

    public class Select2PagedResult
    {
        public Select2PagedResult()
        {
            Pagination = new Select2Pagination();
        }

        public List<Select2Result> Results { get; set; }
        public Select2Pagination Pagination { get; set; }
    }

    public class Select2RequestModel
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
    }

    public class Select2RequestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            var model = new Select2RequestModel
            {
                SearchTerm = request.Form["searchTerm[term]"] ?? string.Empty,
                PageSize = int.Parse(request.Form["pageSize"] ?? "25"),
                PageNum = int.Parse(request.Form["searchTerm[page]"] ?? "1") - 1
            };

            return model;
        }
    }
}
