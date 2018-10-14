using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using Higgs.Core.Helpers;
using Higgs.OpenXml;
using Newtonsoft.Json;

namespace Higgs.Web.Helpers
{
    public static class DataTablesHelpers
    {
        public static ActionResult ToDataTableResult<T>(this IQueryable<T> list, DataTablesRequestModel model, object additionalData = null, ExportFooter exportFooter = null, Action<List<T>, int> resultCallback = null)
            where T : class
        {
            var recordsTotal = list.Count();

            if (recordsTotal == 0)
            {
                if (resultCallback != null) resultCallback(null, recordsTotal);

                if (model.IsExport)
                {
                    return new HttpNotFoundResult();
                }
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

                if (terms.Count > 0)
                {
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

                            var propertyName = col.Data.Substring(0, 1).ToUpper() + col.Data.Substring(1);


                            filter += propertyName + ".ToString().Contains(@" + i + ")";
                        }

                        var condition = terms[i].Groups[3].Success ? terms[i].Groups[3].Value : terms[i].Groups[4].Value;
                        keywords[i] = condition;
                        filter += " )";
                    }

                    list = list.Where(filter, keywords);
                }
            }

            // Order
            var canOrder = model.Order != null && model.Order.Count > 0;
            if (canOrder)
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

            List<T> result;
            if (canOrder)
            {
                result = list.Skip(model.Start).Take(model.Length == -1 ? int.MaxValue : model.Length).ToList();
            }
            else
            {
                result = list.ToList();
            }

            if (resultCallback != null) resultCallback(result, recordsTotal);

            return new JsonNetResult(HiggsResult.SerializerSettings)
            {
                Data = new DataTablesResponseModel(model.Draw, result, recordsFiltered, recordsTotal)
                {
                    AdditionalData = additionalData
                }
            };
        }

        public static Stream CreateExcelReport<T>(this List<T> data, DataTablesRequestModel model, ExportFooter footer = null)
            where T : class
        {
            #region Create Document

            var doc = new ExcelDocument();
            var sheet = doc.AddSheet();

            #endregion

            #region Add Title

            var row = sheet.AddRow();
            sheet.AddCell(row, DefaultCellFormats.BoldUnderlineCenter)
                .SetValueInlineString(model.ReportTitle);
            sheet.MergeCell(row, 1, model.Columns.Count);
            sheet.AddRow();     // Separate Row

            #endregion

            #region Add Header

            var header1 = sheet.AddRow();
            var header2 = sheet.AddRow(false);
            var hasSubColumn = model.Columns.Any(x => x.ExportColSpan > 1);
            var colSpan = 0;

            for (var i = 0; i < model.Columns.Count; i++)
            {
                var col = model.Columns[i];

                if (!col.IsVisible) continue;

                if (col.ExportColSpan == 1 && colSpan == 0)
                {
                    if (hasSubColumn && col.ExportGroupTitle != null)
                    {
                        sheet.AddCell(header1, DefaultCellFormats.BoldCenterBorder)
                                .SetValueInlineString(col.ExportGroupTitle);

                        sheet.AddCell(header2, DefaultCellFormats.BoldCenterBorder)
                                .SetValueInlineString(col.ExportTitle ?? col.Name ?? col.Data);
                    }
                    else
                    {
                        var cell1 = sheet.AddCell(header1, DefaultCellFormats.BoldCenterBorder);
                        cell1.SetValueInlineString(col.ExportTitle ?? col.Name ?? col.Data);

                        if (hasSubColumn)
                        {
                            var cell2 = sheet.AddCell(header2, DefaultCellFormats.BoldCenterBorder);

                            sheet.MergeCell(cell1, cell2);
                        }
                    }
                }
                else
                {
                    if (col.ExportColSpan > 1)
                    {
                        colSpan = col.ExportColSpan;

                        var startCell = sheet.AddCell(header1, DefaultCellFormats.BoldCenterBorder)
                                                            .SetValueInlineString(col.ExportGroupTitle);

                        Cell endCell = null;
                        for (var j = i + 1; j < i + colSpan; j++)
                        {
                            endCell = sheet.AddCell(header1, DefaultCellFormats.BoldCenterBorder);
                        }

                        sheet.MergeCell(startCell, endCell);
                    }

                    sheet.AddCell(header2, DefaultCellFormats.BoldCenterBorder)
                            .SetValueInlineString(col.ExportTitle);

                    colSpan--;
                }
            }

            if (hasSubColumn)
            {
                sheet.AppendRow(header2);
            }

            #endregion

            #region Add Content

            var properties = typeof(T).GetProperties()
                                                        .Where(x => x.CanRead)
                                                        .ToDictionary(x => x.Name.ToUpperInvariant(), x => x);
            var lastRowValues = new Dictionary<int, LastRowData>();
            var pendingMergeCell = new Dictionary<string, string>();

            foreach (var item in data)
            {
                row = sheet.AddRow();

                for (var i = 0; i < model.Columns.Count; i++)
                {
                    var col = model.Columns[i];

                    // TODO: Test with colspan
                    if (!col.IsVisible) continue;

                    var cellStyle = col.ExportDataType.ToCellFormat();
                    var cell = sheet.AddCell(row, cellStyle);
                    if (string.IsNullOrEmpty(col.Data)) continue;

                    var colName = col.Data.ToUpperInvariant();
                    if (!properties.ContainsKey(colName)) continue;
                    
                    var prop = properties[colName];
                    var itemPropertyValue = prop.GetValue(item);

                    cell.SetValue(itemPropertyValue, prop.PropertyType);

                    if (col.ExportMergeData)
                    {
                        if(!lastRowValues.ContainsKey(i))
                        {
                            // First row
                            lastRowValues[i] = new LastRowData
                            {
                                StartCell = cell,
                                Value = itemPropertyValue
                            };
                            continue;
                        }

                        var lastValue = lastRowValues[i];
                        if(Convert.Equals(lastRowValues[i].Value, itemPropertyValue))
                        {
                            // Same value
                            pendingMergeCell[lastValue.StartCell.CellReference] = cell.CellReference;
                            cell.Remove();
                            cell = sheet.AddCell(row, cellStyle);
                        }
                        else
                        {
                            // Different value
                            lastValue.StartCell = cell;
                            lastValue.Value = itemPropertyValue;
                        }
                    }
                }
            }

            pendingMergeCell.ForEach(x =>
            {
                sheet.MergeCell(x.Key, x.Value);
            });

            #endregion

            #region Add Footer

            if (footer != null)
            {
                row = sheet.AddRow();

                var colIndex = 0;
                var footerCellIndex = 0;
                var footerMapping = new Dictionary<int, int>();

                for (var i = 0; i < footer.Cells.Count; i++)
                {
                    var cell = footer.Cells[i];
                    var lastVisibleIndex = (int?)null;

                    for (var j = 0; j < cell.ColSpan; j++)
                    {
                        if (model.Columns[footerCellIndex].IsVisible)
                        {
                            lastVisibleIndex = footerCellIndex;
                        }

                        footerCellIndex++;
                    }

                    if (lastVisibleIndex.HasValue) footerMapping[lastVisibleIndex.Value] = i;
                }

                for (var i = 0; i < model.Columns.Count; i++)
                {
                    var col = model.Columns[i];

                    if (!col.IsVisible)
                    {
                        continue;
                    }

                    colIndex++;
                    var footerCell = footerMapping.ContainsKey(i) ? footer.Cells[footerMapping[i]] : null;
                    var cell = sheet.AddCell(row, footerCell != null ? footerCell.CellFormat : DefaultCellFormats.Default);

                    if (footerCell == null) continue;
                    if (footerCell.Value != null)
                    {
                        var type = footerCell.Value.GetType();

                        if (type == typeof(string))
                        {
                            cell.SetValueInlineString((string)footerCell.Value);
                        }
                        else
                        {
                            cell.SetValue(footerCell.Value, type);
                        }
                    }

                    var columnName = SpreadsheetHelper.GetColumnName(colIndex);
                    switch (footerCell.FormulaType)
                    {
                        case FormulaType.Sum:
                            var startIndex = (hasSubColumn ? 5 : 4);
                            var cellFormula = new CellFormula
                            {
                                Text = string.Format("SUM(" + columnName + startIndex + ":" + columnName + (startIndex + data.Count - 1) + ")")
                            };
                            var cellValue = new CellValue { Text = "0" };

                            cell.AppendChild(cellFormula);
                            cell.AppendChild(cellValue);
                            break;
                    }
                }
            }

            #endregion

            // AutoFit for all column
            for (uint i = 0; i < model.Columns.Count; i++)
            {
                doc.Workbook.AutoFitColumn(sheet, (i + 1));
            }

            return doc.Save();
        }

        public static DefaultCellFormats ToCellFormat(this ExportDataType col)
        {
            DefaultCellFormats cellStyle;

            switch (col)
            {
                case ExportDataType.Integer:
                    cellStyle = DefaultCellFormats.OutsideBorderInteger;
                    break;
                case ExportDataType.Money:
                    cellStyle = DefaultCellFormats.OutsideBorderMoney;
                    break;
                case ExportDataType.Month:
                    cellStyle = DefaultCellFormats.OutsideBorderMonth;
                    break;
                case ExportDataType.Date:
                    cellStyle = DefaultCellFormats.OutsideBorderDate;
                    break;
                case ExportDataType.DateTime:
                    cellStyle = DefaultCellFormats.OutsideBorderDateTime;
                    break;
                case ExportDataType.LongText:
                    cellStyle = DefaultCellFormats.OutsideBorderLongText;
                    break;
                default:
                    cellStyle = DefaultCellFormats.OutsideBorder;
                    break;
            }

            return cellStyle;
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

    public class DataTableRequestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            var serverParams = request.Form["serverParams"];

            if (string.IsNullOrEmpty(serverParams)) return null;

            return JsonConvert.DeserializeObject<DataTablesRequestModel>(HttpUtility.UrlDecode(serverParams));
        }
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
        public int ExportColSpan { get; set; } = 1;
        public ExportDataType ExportDataType { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool ExportMergeData { get; set; } = false;
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
            ColSpan = 1;
        }

        public object Value { get; set; }
        public FormulaType FormulaType { get; set; }
        public DefaultCellFormats CellFormat { get; set; }
        public int ColSpan { get; set; }
    }

    public enum ExportDataType
    {
        InlineText,
        Integer,
        Money,
        Month,
        Date,
        DateTime,
        LongText
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

    public class LastRowData
    {
        public Cell StartCell { get; set; }
        public object Value { get; set; }
    }
}