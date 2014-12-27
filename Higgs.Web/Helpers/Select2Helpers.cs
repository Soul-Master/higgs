using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Core;

namespace Higgs.Web.Helpers
{
    public static class Select2Helpers
    {
        public static JsonNetResult ToSelect2Result<T>(this IEnumerable<T> list, string searchTerm, int pageSize,
            int pageNum) where T : IKey
        {
            var keywords = searchTerm.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return list.Where(x => keywords.All(y =>
            {
                y = y.ToUpper();

                return x.ToString().ToUpper().Contains(y);
            }))
                .ToSelect2Result(pageSize, pageNum);
        }

        public static JsonNetResult ToSelect2Result<T>(this IEnumerable<T> list, int pageSize, int pageNum)
            where T : IKey
        {
            pageNum--;

            var temp = list
                .Select(x => new Select2Result
                {
                    Id = x.Id.ToString(),
                    Text = x.ToString()
                })
                .ToList();

            var result = new Select2PagedResult
            {
                Total = temp.Count,
                Results = temp.Skip(pageNum*pageSize).Take(pageSize).ToList()
            };

            return new JsonNetResult(HiggsResult.SerializerSettings)
            {
                Data = result
            };
        }

        public static JsonNetResult ToSelect2Result(this IEnumerable<KeyValuePair<string, string>> list,
            string searchTerm, int pageSize, int pageNum)
        {
            var keywords = searchTerm.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return list.Where(x => keywords.All(y =>
            {
                y = y.ToUpper();

                return x.Value.ToUpper().Contains(y);
            })).ToSelect2Result(pageSize, pageNum);
        }

        public static JsonNetResult ToSelect2Result(this IEnumerable<KeyValuePair<string, string>> list, int pageSize,
            int pageNum)
        {
            pageNum--;

            var temp = list
                .Select(x => new Select2Result
                {
                    Id = x.Key,
                    Text = x.Value
                })
                .ToList();

            var result = new Select2PagedResult
            {
                Total = temp.Count,
                Results = temp.Skip(pageNum*pageSize).Take(pageSize).ToList()
            };

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

    public class Select2PagedResult
    {
        public int Total { get; set; }
        public List<Select2Result> Results { get; set; }
    }
}
