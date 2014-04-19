using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.Linq;
using System.Data;
using NPOI.SS.UserModel;
using System.Collections;

namespace Higgs.Web.Controls.JqGrid
{
    public static class ExcelHelper
    {
        // TODO: Move to OOXML
        public static void Export(this IEnumerable data, Stream stream, List<ExportColumnModel> colModel)
        {
            var workbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Higgs RIA Framework";
            workbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "JqGrid Export Data";
            workbook.SummaryInformation = si;
            
            var sheet = workbook.CreateSheet("Export-Data");
            var count = 1;

            var firstRow = sheet.CreateRow(0);
            for(var i = 0;i < colModel.Count;i++)
            {
                firstRow
                    .CreateCell(i)
                    .SetCellValue(colModel[i].Title);
            }

            sheet.CreateFreezePane(0, 1);

            Dictionary<string, PropertyInfo> properties = null;
            foreach (var item in data)
            {
                if (properties == null)
                {
                    properties = item.GetType()
                                     .GetProperties()
                                     .ToDictionary(pi => pi.Name);
                }

                var row = sheet.CreateRow(count++);

                for (var i = 0; i < colModel.Count; i++)
                {
                    var cell = row.CreateCell(i);

                    if(!properties.ContainsKey(colModel[i].Name)) continue;
                    var cellValue = properties[colModel[i].Name].GetValue(item, null);

                    workbook.SetCellValue(cell, cellValue);
                }
            }

            for (var i = 0; i < colModel.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            workbook.Write(stream);
        }

        public static void Export(this DataTable data, Stream stream)
        {
            var workbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Higgs RIA Framework";
            workbook.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "JqGrid Export Data";
            workbook.SummaryInformation = si;

            var sheet = workbook.CreateSheet("Export-Data");
            var count = 1;

            var firstRow = sheet.CreateRow(0);
            for (var i = 0; i < data.Columns.Count; i++)
            {
                firstRow
                    .CreateCell(i)
                    .SetCellValue(data.Columns[i].ColumnName);
            }

            sheet.CreateFreezePane(0, 1);

            foreach (DataRow r in data.Rows)
            {
                var row = sheet.CreateRow(count++);

                for (var i = 0; i < data.Columns.Count; i++)
                {
                    var cell = row.CreateCell(i);
                    var cellValue = r[i];

                    workbook.SetCellValue(cell, cellValue);
                }
            }

            for (var i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            workbook.Write(stream);
        }

        private static readonly List<Type> _numericTypes = new List<Type>
        {
            typeof(byte),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(short),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(decimal)
        };

        private static readonly List<Type> _stringConvertableTypes = new List<Type>
        {
            typeof(string),
            typeof(Guid),
            typeof(bool)
        };

        public static void SetCellValue(this IWorkbook workbook, ICell cell, object cellValue)
        {
            var colType = cellValue != null ? cellValue.GetType() : null;

            if (colType == null || cellValue is DBNull) return;

            if (_numericTypes.Contains(colType))
            {
                cell.SetCellType(CellType.Numeric);
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (_stringConvertableTypes.Contains(colType))
            {
                cell.SetCellValue(cellValue.ToString());
            }
            else if (colType == typeof(DateTime))
            {
                var style = workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat();
                const string dateFormat = "dd MMM yyyy hh:mm:ss";

                cell.SetCellValue(((DateTime)cellValue).ToString(dateFormat));
                style.DataFormat = format.GetFormat(dateFormat);
                cell.SetCellType(CellType.String);
                cell.CellStyle = style;
            }
            else
            {
                var toStringMethod = colType.GetMethod("ToString", BindingFlags.Public);

                if (toStringMethod != null && toStringMethod.DeclaringType != typeof(object))
                {
                    // Has custom ToString method
                    cell.SetCellValue(cellValue.ToString());
                }
            }
        }
    }
} 
