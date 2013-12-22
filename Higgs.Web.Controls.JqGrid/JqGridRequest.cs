using System.Collections.Generic;
using System.Web.Mvc;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid
{
    /// <summary>
    /// Original version from http://www.codeproject.com/KB/aspnet/AspNetMVCandJqGrid.aspx
    /// </summary>
    [ModelBinder(typeof(JqGridRequestBinder))]
    public class JqGridRequest
    {
        public bool IsSearch { get; set; }
        public bool IsExport { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        public GridFilter Where { get; set; }

        #region Tree Grid data
        
        public string NodeId { get; set; }
        public string ParentId { get; set; }
        public int NodeLevel { get; set; }

        #endregion

        #region Export data

        public List<ExportColumnModel> ColumnModel { get; set; }
        public string ExportFileName { get; set; }

        #endregion
    }
}