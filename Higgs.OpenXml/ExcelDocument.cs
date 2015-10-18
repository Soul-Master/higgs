using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Higgs.Core.Helpers;
using Higgs.OpenXml;

namespace Higgs.OpenXml
{
    // Based on: http://dotscrapbook.wordpress.com/2012/03/23/openxml-spreadsheets-with-multiple-worksheets-using-net-sdk/
    public class ExcelDocument : IDisposable
    {
        protected MemoryStream Stream { get; set; } // The stream that the spreadsheet gets returned on
        public SpreadsheetDocument Spreadsheet { get; set; }
        public WorkbookPart WorkbookPart { get; set; }
        public Workbook Workbook { get; set; }
        public SheetData SheetData { get; set; }
        private Columns _cols;

        public ExcelDocument()
        {
            CreateSpreadsheet();
        }

        public void Dispose()
        {
            Stream.Dispose();
            Spreadsheet.Dispose();
        }

        public Worksheet this[string sheetId]
        {
            get
            {
                var wsp = (WorksheetPart)Spreadsheet.WorkbookPart.GetPartById(sheetId);

                return wsp.Worksheet;
            }
        }

        /// <summary>
        /// Create a basic spreadsheet template 
        /// The structure of OpenXML spreadsheet is something like this from what I can tell:
        ///                               Spreadsheet
        ///                                           |         
        ///                               WorkbookPart    
        ///                   /                      |                          \
        ///           Workbook WorkbookStylesPart WorksheetPart
        ///                 |                         |                           |
        ///            Sheets     StyleSheet                  Worksheet
        ///                |                        /        \       
        ///  (refers to SheetData   Columns  
        ///  Worksheetparts)            |   
        ///                                        Rows 
        /// 
        /// Obviously this only covers the bits in this class!
        /// </summary>
        /// <returns></returns>
        protected void CreateSpreadsheet()
        {
            Stream = new MemoryStream();
            Spreadsheet = SpreadsheetDocument.Create(Stream, SpreadsheetDocumentType.Workbook);
            WorkbookPart = Spreadsheet.AddWorkbookPart();
            Workbook = new Workbook();

            var callerAssembly = Assembly.GetExecutingAssembly().GetCallerAssembly();

            if (callerAssembly != null)
            {
                var fv = new FileVersion {ApplicationName = callerAssembly.GetProductTitle()};
                Workbook.Append(fv);
            }

            var ws = new Worksheet();
            SheetData = new SheetData();
            ws.AppendChild(SheetData); // Add sheet data to worksheet

            Spreadsheet.WorkbookPart.Workbook = Workbook;
            Spreadsheet.WorkbookPart.Workbook.Save();
            Workbook.AddDefaultStyleSheet();
        }

        public Stream Save()
        {
            WorkbookPart.Workbook.Save();
            Spreadsheet.Close();

            Stream.Flush();
            Stream.Position = 0;
            Spreadsheet.Dispose();

            return Stream;
        }

        public Worksheet AddSheet(string name = null)
        {
            var wsp = Spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
            wsp.Worksheet = new Worksheet();
            wsp.Worksheet.AppendChild(new Columns());
            wsp.Worksheet.AppendChild(new SheetData());
            wsp.Worksheet.Save();

            UInt32 sheetIndex;

            // If this is the first sheet, the ID will be 1. If this is not the first sheet, we calculate the ID based on the number of existing
            // sheets + 1.
            if (Spreadsheet.WorkbookPart.Workbook.Sheets == null)
            {
                Spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheetIndex = 1;
            }
            else
            {
                sheetIndex = Convert.ToUInt32(Spreadsheet.WorkbookPart.Workbook.Sheets.Count() + 1);
            }

            // Create the new sheet and add it to the workbookpart
            Spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
            {
                Id = Spreadsheet.WorkbookPart.GetIdOfPart(wsp),
                SheetId = sheetIndex,
                Name = name ?? "Sheet" + sheetIndex
            });

            _cols = new Columns(); // Created to allow bespoke width columns
            // Save our changes
            Spreadsheet.WorkbookPart.Workbook.Save();

            var currentSheetId = Spreadsheet.WorkbookPart.GetIdOfPart(wsp);
            var sheet = this[currentSheetId];

            return sheet;
        }

        // TODO: Revise code

        /// <summary>
        /// add the bespoke columns for the list spreadsheet
        /// </summary>
        public void CreateColumnWidth(string sheetId, uint startIndex, uint endIndex, double width)
        {
            // Find the columns in the worksheet and remove them all

            if (this[sheetId].Any(x => x.LocalName == "cols"))
                this[sheetId].RemoveChild<Columns>(_cols);

            // Create the column
            Column column = new Column();
            column.Min = startIndex;
            column.Max = endIndex;
            column.Width = width;
            column.CustomWidth = true;
            _cols.AppendChild(column); // Add it to the list of columns

            // Make sure that the column info is inserted *before* the sheetdata

            this[sheetId].InsertBefore(_cols, this[sheetId].First(x => x.LocalName == "sheetData"));
            this[sheetId].Save();
            Spreadsheet.WorkbookPart.Workbook.Save();
        }
    }
}