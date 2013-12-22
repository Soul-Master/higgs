using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Higgs.Web.Controls.JqGrid.Models;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.JqGrid
{
    public static class LinqHelper
    {
        public static T[] FilterBy<T>(this IEnumerable<T> objList, JqGridRequest request, out int totalFilteredRow)
        {
            IQueryable<T> query;
            totalFilteredRow = 0;

            if (objList is IQueryable)
                query = objList as IQueryable<T>;
            else
                query = objList.AsQueryable();

            //filtring
            if (request.IsSearch)
            {
                query = query.Where(request.Where.rules, request.Where.groupOp == "AND");
                totalFilteredRow = query.Count();
            }

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.OrderWith(request.SortColumn, request.SortOrder);
                return query.Skip((request.PageIndex - 1)*request.PageSize).Take(request.PageSize).ToArray();
            }

            return query != null ? query.ToArray() : null;
        }

        /// <summary>
        /// <![CDATA[
        ///  This function can create jqGrid result by using data from both IEumerable<T> type and IQueryable<T>.
        ///  Case IEnumerable<T> like T[], List<T> and so on : 
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <param name="createRowFn"></param>
        /// <returns></returns>
        public static ContentResult AsJqTreeGridResult<T>(this IEnumerable<T> data, JqGridRequest request, Func<T, object> createRowFn = null)
        {
            if (data == null) throw new ArgumentNullException();

            ICollection filteredData;
            var totalRows = data.Count();

            if (totalRows == 0)
            {
                return WriteXml(0, 0, 0, null);
            }

            int totalFilteredRow;
            if (createRowFn != null)
            {
                filteredData = data.FilterBy(request, out totalFilteredRow)
                                    .Select(createRowFn.Invoke)
                                    .ToList();
            }
            else
            {
                filteredData = data.FilterBy(request, out totalFilteredRow)
                                    .ToList();
            }

            return WriteXml
            (
                filteredData.Count,
                request.PageIndex,
                request.IsSearch ? totalFilteredRow : totalRows,
                filteredData
            );
        }

        public static ActionResult ExportAsExcel(this IEnumerable data, List<ExportColumnModel> colModel = null)
        {
            var tempColModel = new List<ExportColumnModel>();

            if (data != null)
            {
                foreach (var obj in data)
                {
                    tempColModel.AddRange
                    (
                        from p in obj.GetType().GetProperties()
                        where p.CanRead
                        select new ExportColumnModel
                        {
                            Name = p.Name,
                            Title = p.Name
                        }
                    );

                    break;
                }
            }

            if (colModel != null)
            {
                colModel.RemoveAll(x => tempColModel.All(y => y.Name != x.Name));
            }
            else
            {
                colModel = tempColModel;
            }

            var stream = new MemoryStream();
            data.Export(stream, colModel);
            stream.Seek(0, SeekOrigin.Begin);

            return new StreamResult(stream, "application/ms-excel")
            {
                FileDownloadName = (MvcHelper.GetCurrentActionGroupName() ?? "Export") + "Data.xls"
            };
        }
        
        /// <summary>
        /// <![CDATA[
        ///  This function can create jqGrid result by using data from both IEumerable<T> type and IQueryable<T>.
        ///  Case IEnumerable<T> like T[], List<T> and so on : 
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="request"></param>
        /// <param name="createRowFn"></param>
        /// <returns></returns>
        public static ActionResult AsJqGridResult<T>(this IEnumerable<T> data, JqGridRequest request, Func<T, object> createRowFn = null, object userData = null)
        {
            ICollection filteredData;
            var totalRows = data.Count();

            if (request.IsExport)
            {
                request.PageIndex = 1;
                request.PageSize = int.MaxValue;
            }

            if (request.PageSize == -1)
            {
                request.PageSize = int.MaxValue;
            }

            if (totalRows == 0 && !request.IsExport)
            {
                return new JsonResult
                {
                    Data = new JqGridResult
                    {
                        total = 0,
                        page = 0,
                        records = 0,
                        rows = null,
                        userdata = userData
                    }
                };
            }

            int totalFilteredRow;
            if (createRowFn != null)
            {
                var temp = data.FilterBy(request, out totalFilteredRow).Select(createRowFn.Invoke).ToList();

                filteredData = temp;
            }
            else
            {
                var temp = data.FilterBy(request, out totalFilteredRow).ToList();

                filteredData = temp;
            }

            if (request.IsExport) return ExportAsExcel(filteredData, request.ColumnModel);
            
            return new JsonResult
            {
                Data = new JqGridResult
                {
                    total = (int)Math.Ceiling((request.IsSearch ? totalFilteredRow : totalRows)/ (double)request.PageSize),
                    page = request.PageIndex,
                    records = request.IsSearch ? totalFilteredRow : totalRows,
                    rows = filteredData,
                    userdata = userData
                }
            };
        }

        public static ContentResult WriteXml(int total, int pageIndex, int totalRows, ICollection data)
        {
            var builder = new StringBuilder();
            var writer = XmlWriter.Create(builder);

            writer.WriteStartDocument();

            writer.WriteStartElement("rows");

            writer.WriteStartElement("page");
            writer.WriteString(pageIndex.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("total");
            writer.WriteString(total.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("records");
            writer.WriteString(totalRows.ToString());
            writer.WriteEndElement();

            foreach(var x in data)
            {
                var t = x.GetType();

                writer.WriteStartElement("row");

                foreach (var p in t.GetProperties())
                {
                    writer.WriteStartElement("cell");

                    object tempValue = null;
                    
                    try
                    {
                        tempValue = p.GetValue(x, null);
                    } catch{}

                    if (tempValue != null)
                    {
                        writer.WriteString(tempValue.ToString());
                    }

                    writer.WriteEndElement();
                }

                writer.WriteStartElement("cell");
                writer.WriteString("0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteCData("NULL");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteString("false");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteString("false");
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();

            return new ContentResult
            {
                ContentType = "text/xml",
                Content = builder.ToString(),
                ContentEncoding = Encoding.UTF8
            };
        }

        public class StringSorting : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return StringComparer.CurrentCulture.Compare(x, y);
            }
        }

        /// <summary>
        ///     Orders the sequence by specific column and direction.
        ///     This method support multiple columns sorting like the following data
        ///     SortColumn = "aaa,"
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">Sort column</param>
        /// <param name="direction">The sort column.</param>
        public static IQueryable<T> OrderWith<T>(this IQueryable<T> query, string sortColumn, string direction)
        {
            var columns = sortColumn.Split(' ', ',');
            var directions = direction.Split(' ', ',');

            for (var i = 0; i < columns.Length; i++)
            {
                var dir = directions.Length > i ? directions[i] : directions[directions.Length - 1];
                var methodName = string.Format("OrderBy{0}", dir.ToLower() == "asc" ? "" : "descending");
                var parameter = Expression.Parameter(query.ElementType, "p");
                var memberAccess = columns[i].Split('.').Aggregate<string, MemberExpression>(null,
                                                                                             (current, property) =>
                                                                                             Expression.Property(
                                                                                                 current ??
                                                                                                 (parameter as
                                                                                                  Expression), property));
                var orderByLambda = Expression.Lambda(memberAccess, parameter);

                if (i > 0)
                {
                    methodName = string.Format("ThenBy{0}", dir.ToLower() == "asc" ? "" : "descending");
                }

                var result = Expression.Call(
                    typeof (Queryable),
                    methodName,
                    new[] {query.ElementType, memberAccess.Type},
                    query.Expression,
                    Expression.Quote(orderByLambda));

                query = query.Provider.CreateQuery<T>(result);
            }

            return query;
        }
        
        public static IQueryable<T> Where<T>(this IQueryable<T> query, Rule[] rules, bool isMatchAllRule)
        {
            Expression currentExp = null;
            var parameter = Expression.Parameter(query.ElementType, "x");

            foreach(var r in rules)
            {
                var column = r.field;
                var value = r.data;
                var operation = (WhereOperation) StringEnum.Parse(typeof (WhereOperation), r.op);
                
                if (string.IsNullOrEmpty(column))   continue;

                var memberAccess = column.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? (parameter as Expression), property));

                //change param value type
                //necessary to getting bool from string
                var filter = Expression.Constant
                (
                    Convert.ChangeType(value, memberAccess.Type)
                );

                Expression condition;
                switch (operation)
                {
// ReSharper disable PossiblyMistakenUseOfParamsMethod
                    case WhereOperation.Contains:                    condition = Expression.Call(memberAccess, WhereOperationMethods.Contains, filter); break;
                    case WhereOperation.NotContains:               condition = Expression.Not(Expression.Call(memberAccess, WhereOperationMethods.Contains, filter)); break;
                    case WhereOperation.BeginsWith:                 condition = Expression.Call(memberAccess, WhereOperationMethods.BeginsWith, filter); break;          
                    case WhereOperation.NotBeginsWith:           condition = Expression.Not(Expression.Call(memberAccess, WhereOperationMethods.BeginsWith, filter)); break;
                    case WhereOperation.EndsWith:                   condition = Expression.Call(memberAccess, WhereOperationMethods.EndsWith, filter); break;
                    case WhereOperation.NotEndsWith:              condition = Expression.Not(Expression.Call(memberAccess, WhereOperationMethods.EndsWith, filter)); break;
                    case WhereOperation.LessThan:                    condition = Expression.LessThan(memberAccess, filter); break;
                    case WhereOperation.LessThanOrEqual:        condition = Expression.LessThanOrEqual(memberAccess, filter); break;
                    case WhereOperation.GreaterThan:               condition = Expression.GreaterThan(memberAccess, filter); break;
                    case WhereOperation.GreaterThanOrEqual:   condition = Expression.GreaterThanOrEqual(memberAccess, filter); break;
                    case WhereOperation.Equal:                         condition = Expression.Equal(memberAccess, filter); break;
                    default:                                                       condition = Expression.NotEqual(memberAccess, filter); break;
// ReSharper restore PossiblyMistakenUseOfParamsMethod
                }

                currentExp = currentExp == null ? condition : (isMatchAllRule ?  Expression.AndAlso(currentExp, condition) : Expression.OrElse(currentExp, condition));
            }

            if (currentExp == null) return query;
            
            var result = Expression.Call(
                   typeof(Queryable), "Where",
                   new[] { query.ElementType },
                   query.Expression,
                   Expression.Lambda(currentExp, parameter));

            return query.Provider.CreateQuery<T>(result);
        }
    }
}
