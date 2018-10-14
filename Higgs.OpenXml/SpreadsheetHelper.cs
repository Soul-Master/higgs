using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using Font = DocumentFormat.OpenXml.Spreadsheet.Font;

namespace Higgs.OpenXml
{
    public static class SpreadsheetHelper
    {
        public static DoubleValue ColumnMaxWidth = 100;

        public static Stylesheet AddDefaultStyleSheet(this Workbook book)
        {
            var styleSheet = new Stylesheet(
                new NumberingFormats(
                    new NumberingFormat
                    {
                        NumberFormatId = 0,
                        FormatCode = "0"
                    },
                    new NumberingFormat
                    {
                        NumberFormatId = 1,
                        FormatCode = "dd mmm yyyy"
                    },
                    new NumberingFormat
                    {
                        NumberFormatId = 2,
                        FormatCode = "dd mmm yyyy hh:mm:ss"
                    },
                    new NumberingFormat
                    {
                        NumberFormatId = 3,
                        FormatCode = "#,##0"
                    },
                    new NumberingFormat
                    {
                        NumberFormatId = 4,
                        FormatCode = "#,##0.00"
                    },
                    new NumberingFormat
                    {
                        NumberFormatId = 5,
                        FormatCode = "mmm yyyy"
                    }
                    ),
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Italic font.
                        new Italic(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" }),
                    new Font(                                                               // Index 3 - The Underline font.
                        new Underline(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" }),
                    new Font(                                                               // Index 4 - The Underline+Bold font.
                        new Bold(),
                        new Underline(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" })
                    ),
                new Fills(
                    new Fill(new PatternFill { PatternType = PatternValues.None })    // Index 0 - The default fill.
                    ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                    ),
                new CellFormats(
                    new CellFormat
                    {
                        FontId = 0,
                        FillId = 0,
                        BorderId = 0,
                        Alignment = new Alignment
                        {
                            Vertical = VerticalAlignmentValues.Center
                        }
                    },    // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 - Bold 
                    new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 - Italic
                    new CellFormat { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Underline
                    new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 4 - Bold+Underline
                    new CellFormat(                                                                                                      // Index 5 - Alignment Center
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 6 - Alignment Left
                        new Alignment { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 7 - Alignment Right
                        new Alignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true,
                        Alignment = new Alignment
                        {
                            Vertical = VerticalAlignmentValues.Center
                        }
                    },   // Index 8 - Border
                    new CellFormat(                                                                                                      // Index 9 - Bold+Alignment Center
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 1, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 10 - Bold+Alignment Left
                        new Alignment { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 1, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 11 - Bold+Alignment Right
                        new Alignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 1, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 12 - Underline+Alignment Center
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 3, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 13 - Bold+Underline+Alignment Center
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 4, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat(                                                                                                      // Index 14 - Bold+Underline+Alignment Center+Border
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 1, FillId = 0, BorderId = 1, ApplyAlignment = true },
                    new CellFormat(new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center } // Index 15 - Border + Date
                        ) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 1 }, 
                    new CellFormat(                                                                                                      // Index 16 - Border + Integer
                        new Alignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 3},
                    new CellFormat(                                                                                                      // Index 17 - Border + Money
                        new Alignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 4 },
                    new CellFormat (
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ){ FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 5 }, // Index 18 - Border + Month,
                    new CellFormat(
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 1 }, // Index 19 - Border + Date
                    new CellFormat(
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true, NumberFormatId = 2 }, // Index 20 - Border + DateTime
                    new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyFont = true, NumberFormatId = 3 },       // Index 21 - Bold+Underline+Integer
                    new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyFont = true, NumberFormatId = 4 },       // Index 22 - Bold+Underline+Money
                    new CellFormat {
                        FontId = 0,
                        FillId = 0,
                        BorderId = 1,
                        Alignment = new Alignment { Vertical = VerticalAlignmentValues.Top, WrapText = true } }       // Index 23 - Long Text
                    )
                );

            var stylesPart = book.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = styleSheet;
            stylesPart.Stylesheet.Save();

            return styleSheet;
        }

        public static OpenXmlPackage SetPackageProperties(this OpenXmlPackage document, string authorName)
        {
            document.PackageProperties.Creator = authorName;
            document.PackageProperties.Created = DateTime.Now;
            document.PackageProperties.Modified = DateTime.Now;
            document.PackageProperties.LastModifiedBy = authorName;

            return document;
        }

        public static Row AddRow(this Worksheet sheet, bool isAppend = true)
        {
            var sd = sheet.OfType<SheetData>().First();
            var row = new Row
            {
                RowIndex = Convert.ToUInt32(sd.ChildElements.Count()) + 1
            };

            if (isAppend) sheet.AppendRow(row);

            return row;
        }

        public static Worksheet AppendRow(this Worksheet sheet, Row row)
        {
            var sd = sheet.OfType<SheetData>().First();
            sd.AppendChild(row);

            return sheet;
        }

        public static MergeCell MergeCell(this Worksheet sheet, Cell startCell, Cell endCell)
        {
            return sheet.MergeCell(startCell.CellReference.Value, endCell.CellReference.Value);
        }

        public static MergeCell MergeCell(this Worksheet sheet, string startCell, string endCell)
        {
            MergeCells mergeCells;

            if (sheet.Elements<MergeCells>().Any())
                mergeCells = sheet.Elements<MergeCells>().First();
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (sheet.Elements<CustomSheetView>().Any())
                    sheet.InsertAfter(mergeCells, sheet.Elements<CustomSheetView>().First());
                else
                    sheet.InsertAfter(mergeCells, sheet.Elements<SheetData>().First());
            }

            // Create the merged cell and append it to the MergeCells collection.
            var mergeCell = new MergeCell
            {
                Reference = new StringValue(startCell + ":" + endCell)
            };
            mergeCells.AppendChild(mergeCell);

            return mergeCell;
        }

        public static MergeCell MergeCell(this Worksheet sheet, int rowIndex, int startColumnIndex, int endColumnIndex)
        {
            var startCell = GetColumnName(startColumnIndex) + rowIndex;
            var endCell = GetColumnName(endColumnIndex) + rowIndex;

            return sheet.MergeCell(startCell, endCell);
        }

        public static MergeCell MergeCell(this Worksheet sheet, Row row, int startColumnIndex, int endColumnIndex)
        {
            var startCell = GetColumnName(startColumnIndex) + row.RowIndex;
            var endCell = GetColumnName(endColumnIndex) + row.RowIndex;

            return sheet.MergeCell(startCell, endCell);
        }

        public static bool IsMergedCell(this Worksheet sheet, string cellRef)
        {
            var mergeCells = sheet.Elements<MergeCells>().FirstOrDefault();

            if (mergeCells == null) return false;
            
            return mergeCells.OfType<MergeCell>().Any(x => IsInRange(x.Reference.Value, cellRef));
        }

        private static Cell AddCell(this Worksheet sheet, Row row, UInt32? styleIndex = null, bool isAppend = true)
        {
            var cell = new Cell();
            if (styleIndex.HasValue) cell.StyleIndex = styleIndex.Value;

            // Get the last cell's column
            var nextCol = "A";
            var c = (Cell)row.LastChild;

            if (c != null) // if there are some cells already there...
            {
                var numIndex = c.CellReference.ToString().IndexOfAny(new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                // Get the last column reference
                var lastCol = c.CellReference.ToString().Substring(0, numIndex);
                nextCol = GetNextColumnRef(lastCol);
            }

            while (sheet.IsMergedCell(nextCol + row.RowIndex))
            {
                nextCol = GetNextColumnRef(nextCol);
            }

            cell.CellReference = nextCol + row.RowIndex;

            if (isAppend)
            {
                row.AppendCell(cell);
            }

            return cell;
        }

        public static Cell AddCell(this Worksheet sheet, Row row, DefaultCellFormats cellFormat, bool isAppend = true)
        {
            return sheet.AddCell(row, (UInt32) cellFormat, isAppend);
        }

        public static Cell GetCell(this Worksheet sheet, string cellAddress)
        {
            return sheet.Descendants<Cell>().SingleOrDefault(c => cellAddress.Equals(c.CellReference));
        }

        public static CellFormat GetCellFormat(this Workbook book, uint styleIndex)
        {
            var workbookStylePart = book.WorkbookPart.WorkbookStylesPart;
            var cellFormats = workbookStylePart.Stylesheet.Elements<CellFormats>().First();

            return cellFormats.Elements<CellFormat>().ElementAt((int)styleIndex);
        }

        public static Font GetFontFormat(this Workbook book, uint styleIndex)
        {
            var workbookStylePart = book.WorkbookPart.WorkbookStylesPart;
            var fonts = workbookStylePart.Stylesheet.Elements<Fonts>().First();

            return fonts.Elements<Font>().ElementAt((int)styleIndex);
        }

        public static Fill GetFillFormat(this Workbook book, uint styleIndex)
        {
            var workbookStylePart = book.WorkbookPart.WorkbookStylesPart;
            var fills = workbookStylePart.Stylesheet.Elements<Fills>().First();

            return fills.Elements<Fill>().ElementAt((int)styleIndex);
        }

        public static Border GetBorderFormat(this Workbook book, uint styleIndex)
        {
            var workbookStylePart = book.WorkbookPart.WorkbookStylesPart;
            var borders = workbookStylePart.Stylesheet.Elements<Borders>().First();

            return borders.Elements<Border>().ElementAt((int)styleIndex);
        }

        public static bool IsInMergedCell(MergeCells mergeCell, string cellRef)
        {
            var mergeCells = mergeCell.OfType<MergeCell>();

            return mergeCells.Any(merge => IsInRange(merge.Reference.Value, cellRef));
        }

        public static bool IsInRange(string range, string cellRef)
        {
            var cellColName = GetColumnName(cellRef);
            var cellColIndex = GetColumnIndex(cellColName);
            var cellRowIndex = int.Parse(cellRef.Substring(cellColName.Length));

            var split = range.Split(':');
            var startColName = GetColumnName(split[0]);
            var startColIndex = GetColumnIndex(startColName);
            var startRowIndex = int.Parse(split[0].Substring(startColName.Length));

            var endColName = GetColumnName(split[1]);
            var endColIndex = GetColumnIndex(endColName);
            var endRowIndex = int.Parse(split[1].Substring(endColName.Length));

            return cellColIndex >= startColIndex && cellRowIndex >= startRowIndex &&
                   cellColIndex <= endColIndex && cellRowIndex <= endRowIndex;
        }

        public static Row AppendCell(this Row row, Cell cell)
        {
            row.AppendChild(cell);

            return row;
        }

        public static Cell SetValue<T>(this Cell cell, object data)
        {
            return cell.SetValue(data, typeof(T));
        }

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static Cell SetValue(this Cell cell, object data, Type dataType)
        {
            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                dataType = Nullable.GetUnderlyingType(dataType);
            }

            if (dataType == typeof(string) || dataType.IsEnum)
            {
                if (data != null)
                {
                    cell.SetValueInlineString(data.ToString());
                }
            }
            else if (dataType.IsNumericType())
            {
                cell.DataType = CellValues.Number;

                var v = new CellValue { Text = data != null ? data.ToString() : "0" };
                cell.AppendChild(v);
            }
            else if (dataType == typeof(DateTime))
            {
                cell.DataType = CellValues.Number;

                if (data != null)
                {
                    var dt = (DateTime)data;
                    var v = new CellValue { Text = dt.ToOADate().ToString() };
                    cell.AppendChild(v);
                }
            }
            else if (dataType == typeof(Boolean))
            {
                cell.DataType = CellValues.Boolean;

                if (data != null)
                {
                    var v = new CellValue { Text = data.ToString() };
                    cell.AppendChild(v);
                }
            }

            return cell;
        }

        public static Cell SetValueInlineString(this Cell cell, string value)
        {
            cell.DataType = CellValues.InlineString;

            var inlineString = new InlineString();
            inlineString.AppendChild(new Text
            {
                Text = value
            });

            cell.AppendChild(inlineString);

            return cell;
        }

        public static Column AutoFitColumn(this Workbook book, Worksheet sheet, uint columnIndex)
        {
            var columns = sheet.Elements<Columns>().First();

            var column = columns.Elements<Column>().FirstOrDefault(x => x.Min == columnIndex && x.Max == columnIndex);
            if (column == null)
            {
                column = new Column
                {
                    Min = columnIndex,
                    Max = columnIndex,
                    CustomWidth = true
                };
                columns.AppendChild(column);
            }

            var sheetData = sheet.OfType<SheetData>().First();
            var columnName = GetColumnName((int)columnIndex);
            var cells = sheetData.Descendants<Cell>().Where(x => x.CellReference.Value.StartsWith(columnName)).Take(100);
            var maxWidth = 0.0;

            foreach (var c in cells)
            {
                var width = book.GetCellWidth(c);

                if (width > maxWidth) maxWidth = width;
            }

            if (maxWidth > 0)
            {
                column.Width = maxWidth <= ColumnMaxWidth ? (DoubleValue)maxWidth : ColumnMaxWidth;
            }

            return column;
        }

        public static Column AutoFitColumn(this Workbook book, Worksheet sheet, uint columnIndex, Cell cell)
        {
            var columns = sheet.Elements<Columns>().First();

            var column = columns.Elements<Column>().FirstOrDefault(x => x.Min == columnIndex && x.Max == columnIndex);
            if (column == null)
            {
                column = new Column
                {
                    Min = columnIndex,
                    Max = columnIndex,
                    CustomWidth = true
                };
                columns.AppendChild(column);
            }
            column.Width = book.GetCellWidth(cell);

            return column;
        }

        #region Utils

        public static string GetColumnName(int columnIndex)
        {
            var dividend = columnIndex;
            var columnName = String.Empty;

            while (dividend > 0)
            {
                var modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier) + columnName;
                dividend = ((dividend - modifier) / 26);
            }

            return columnName;
        }

        public static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellName);

            return match.Value;
        }

        public static string GetColumnName(Cell cell)
        {
            return GetColumnName(cell.CellReference);
        }

        public static int GetColumnIndex(string columnName)
        {
            var characters = columnName.ToUpperInvariant().ToCharArray();
            var sum = 0;

            for (var i = 0; i < characters.Length; i++)
            {
                sum *= 26;
                sum += (characters[i] - 'A' + 1);
            }

            sum++;

            return sum;
        }

        public static SharedStringItem GetSharedStringItemById(this Workbook spreadSheet, int id)
        {
            return spreadSheet.WorkbookPart.SharedStringTablePart.SharedStringTable
                .Elements<SharedStringItem>().ElementAt(id);
        }

        public static double GetCellWidth(this Workbook spreadSheet, Cell cell)
        {
            string cellValue = null;

            if (cell.DataType != null)
            {
                if (cell.DataType == CellValues.SharedString)
                {
                    int id;

                    if (Int32.TryParse(cell.InnerText, out id))
                    {
                        var item = spreadSheet.GetSharedStringItemById(id);

                        if (item.Text != null)
                        {
                            cellValue = item.Text.Text;
                        }
                        else if (item.InnerText != null)
                        {
                            cellValue = item.InnerText;
                        }
                        else if (item.InnerXml != null)
                        {
                            cellValue = item.InnerXml;
                        }
                    }
                }
                else if (cell.DataType == CellValues.InlineString)
                {
                    var inlineString = cell.Elements<InlineString>().Single();
                    cellValue = inlineString.Text.Text;
                }
                else if (cell.DataType == CellValues.Number)
                {
                    var otherValue = cell.Elements<CellValue>().SingleOrDefault();

                    if (otherValue != null)
                    {
                        // TODO: Use number format to get real display number.
                        cellValue = double.Parse(otherValue.Text).ToString("N");
                    }
                }
                else
                {
                    var otherValue = cell.Elements<CellValue>().SingleOrDefault();
                    
                    if (otherValue != null)
                    {
                        cellValue = otherValue.Text;
                    }
                }
            }

            if (string.IsNullOrEmpty(cellValue)) return 0;

            var cellFormat = spreadSheet.GetCellFormat(cell.StyleIndex ?? 0);
            var font = spreadSheet.GetFontFormat(cellFormat.FontId);
            var fontStyle = font.Bold != null ? FontStyle.Bold : FontStyle.Regular;
            var stringFont = new System.Drawing.Font(font.FontName.Val.Value, (float)font.FontSize.Val.Value, fontStyle);
            
            return cellValue.Split(new []{'\n','\r'}, StringSplitOptions.RemoveEmptyEntries).Max(x => GetWidth(stringFont, x.Trim()));
        }

        public static double GetWidth(string font, int fontSize, string text)
        {
            var stringFont = new System.Drawing.Font(font, fontSize);

            return GetWidth(stringFont, text);
        }

        public static double GetWidth(System.Drawing.Font stringFont, string text)
        {
            // This formula is based on this article plus a nudge ( + 0.2M )
            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.column.width.aspx
            // Truncate(((256 * Solve_For_This + Truncate(128 / 7)) / 256) * 7) = DeterminePixelsOfString

            using (var g = Graphics.FromImage(new Bitmap(500, 100)))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                var textSize = g.MeasureString(text, stringFont);
                var width = (((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
                width = (double)decimal.Round((decimal)width, 2);

                if (width > 0.5) width += 2D;

                return width;
            }
        }

        // Increment the column reference in an Excel fashion, i.e. A, B, C...Z, AA, AB etc.
        // Partly stolen from somewhere on the Net and modified for my use.
        public static string GetNextColumnRef(string cellRef)
        {
            var sum = GetColumnIndex(cellRef);
            var columnName = String.Empty;

            while (sum > 0)
            {
                var modulo = (sum - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                sum = (sum - modulo) / 26;
            }

            return columnName;
        }

        #endregion
    }
}