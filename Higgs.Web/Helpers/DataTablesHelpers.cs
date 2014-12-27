using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Higgs.Web.Helpers
{
    public static class DataTablesHelpers
    {
        public static ActionResult ToDataTableResult<T>(this IQueryable<T> list, DataTablesRequestModel model, object additionalData = null, ExportFooter exportFooter = null)
            where T : class
        {
            var recordsTotal = list.Count();
            if (recordsTotal == 0)
            {
                return new JsonNetResult(HiggsResult.SerializerSettings)
                {
                    Data = new DataTablesResponseModel(model.Draw, new String[] { }, 0, 0)
                };
            }
            var regexInt = new Regex("^\\d+$", RegexOptions.Compiled);

            // Filter
            if (model.Search != null && !string.IsNullOrEmpty(model.Search.Value))
            {
                var filter = string.Empty;
                var filterRegex = new Regex("((\"([^\"]+)\")|([^ \"]+))+", RegexOptions.Compiled);
                var terms = filterRegex.Matches(model.Search.Value);
                var keywords = new object[terms.Count];

                for (var i = 0; i < terms.Count; i++)
                {
                    if (filter.Length > 0) filter += " AND ";

                    filter += "( ";
                    var isFirst = true;

                    for (var j = 0; j < model.Columns.Count; j++)
                    {
                        var col = model.Columns[j];

                        if (!col.Searchable) continue;
                        if (string.IsNullOrEmpty(col.Data)) continue;
                        if (regexInt.IsMatch(col.Data)) continue;

                        if (!isFirst) filter += " OR ";
                        else isFirst = false;

                        col.Data = col.Data.Substring(0, 1) + col.Data.Substring(1);
                        filter += col.Data + ".ToString().Contains(@" + i + ")";
                    }

                    var condition = terms[i].Groups[3].Success ? terms[i].Groups[3].Value : terms[i].Groups[4].Value;
                    keywords[i] = condition;
                    filter += " )";
                }

                if (terms.Count > 0) list = list.Where(filter, keywords);
            }

            // Order
            if (model.Order != null)
            {
                var orderBy = string.Empty;
                for (var i = 0; i < model.Order.Count; i++)
                {
                    var orderItem = model.Order[i];

                    if (orderBy.Length > 0)
                    {
                        orderBy += ",";
                    }

                    orderBy += model.Columns[orderItem.Column].Data + " " + orderItem.Dir;
                }
                list = list.OrderBy(orderBy);
            }

            var recordsFiltered = list.Count();

            // Export as Excel
            if (model.IsExport)
            {
                var excelStream = list.ToList().CreateExcelReport(model, exportFooter);
                var fileStreamResult = new FileStreamResult(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = model.ReportTitle != null ? model.ReportTitle + ".xlsx" : "Export.xlsx"
                };

                return fileStreamResult;
            }

            var result = list.Skip(model.Start).Take(model.Length == -1 ? int.MaxValue : model.Length);

            return new JsonNetResult(HiggsResult.SerializerSettings)
            {
                Data = new DataTablesResponseModel(model.Draw, result, recordsFiltered, recordsTotal)
                {
                    AdditionalData = additionalData
                }
            };
        }
    }

    public class DataTablesRequestModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public List<ColumnModel> Columns { get; set; }
        public List<OrderModel> Order { get; set; }
        public SearchModel Search { get; set; }

        // For export only
        public bool IsExport { get; set; }
        public string ReportTitle { get; set; }
    }

    public class ColumnModel
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchModel Search { get; set; }

        // For export only
        public string ExportTitle { get; set; }
        public string ExportGroupTitle { get; set; }
        public int ExportColSpan { get; set; }
        public ExportDataType ExportDataType { get; set; }
        public bool IsVisible { get; set; }
    }
    public class SearchModel
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class OrderModel
    {
        public int Column { get; set; }
        public OrderDirection Dir { get; set; }
    }

    public enum OrderDirection
    {
        Asc,
        Desc
    }

    public class ExportFooter
    {
        public ExportFooter()
        {
            Cells = new List<FooterCell>();
        }

        public List<FooterCell> Cells { get; set; }
    }

    public class FooterCell
    {
        public FooterCell()
        {

        }

        public object Value { get; set; }
        public FormulaType FormulaType { get; set; }
        public DefaultCellFormats CellFormat { get; set; }
    }

    public enum FormulaType
    {
        None,
        Sum
    }

    public enum ExportDataType
    {
        InlineText,
        Integer,
        Money,
        Month,
        Date,
        DateTime
    }

    public class DataTablesResponseModel
    {
        public int Draw { get; set; }
        public IEnumerable Data { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }

        // Custom
        public object AdditionalData { get; set; }

        public DataTablesResponseModel(int draw, IEnumerable data, int recordsFiltered, int recordsTotal)
        {
            Draw = draw;
            Data = data;
            RecordsFiltered = recordsFiltered;
            RecordsTotal = recordsTotal;
        }
    }

    public enum DefaultCellFormats : uint
    {
        Default = 0U,
        Bold = 1U,
        Italic = 2U,
        Underline = 3U,
        BoldUnderline = 4U,
        Center = 5U,
        Left = 6U,
        Right = 7U,
        OutsideBorder = 8U,
        BoldCenter = 9U,
        BoldLeft = 10U,
        BoldRight = 11U,
        UnderlineCenter = 12U,
        BoldUnderlineCenter = 13U,
        BoldCenterBorder = 14U,
        OutsideBorderDataDate = 15U,
        OutsideBorderInteger = 16U,
        OutsideBorderMoney = 17U,
        OutsideBorderMonth = 18U,
        OutsideBorderDate = 19U,
        OutsideBorderDateTime = 20U,
        BoldUnderlineInteger = 21U,
        BoldUnderlineMoney = 22U
    }
}