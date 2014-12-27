using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Higgs.Web.OpenXml;

namespace Higgs.Web.Helpers
{
    public static class ExcelHelpers
    {
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
                    var cell1 = sheet.AddCell(header1, DefaultCellFormats.BoldCenterBorder);
                    cell1.SetValueInlineString(col.ExportTitle);

                    if (hasSubColumn)
                    {
                        var cell2 = sheet.AddCell(header2, DefaultCellFormats.BoldCenterBorder);

                        sheet.MergeCell(cell1, cell2);
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
                    var colName = col.Data.ToUpperInvariant();
                    var prop = properties[colName];
                    var itemPropertyValue = prop.GetValue(item);

                    cell.SetValue(itemPropertyValue, prop.PropertyType);
                }
            }

            #endregion

            #region Add Footer

            if (footer != null)
            {
                row = sheet.AddRow();

                var colIndex = 0;
                var skipCol = 0;
                for (var i = 0; i < model.Columns.Count; i++)
                {
                    var col = model.Columns[i];

                    if (!col.IsVisible)
                    {
                        skipCol++;
                        continue;
                    }

                    colIndex++;
                    var footerCell = footer.Cells[i - skipCol];
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
            for (var i = 0; i < model.Columns.Count; i++)
            {
                doc.Workbook.AutoFitColumn(sheet, (uint)(i + 1));
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
                default:
                    cellStyle = DefaultCellFormats.OutsideBorder;
                    break;
            }

            return cellStyle;
        }
    }
}
