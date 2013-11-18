using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Higgs.Web.Helpers;
using Newtonsoft.Json;

namespace Higgs.Web.Controls.JqGrid.Models
{
    /// <summary>
    /// This class is used to store JqGrid data for rendering.
    /// </summary>
    public class Grid
    {
        public int CreateScriptDelay = 100;

        public string Id;
        public string AltClass;
        public bool? AltRows;
        public bool? AutoEncode;
        public bool? AutoWidth;
        public string Caption;
        public List<IColumn> Columns = new List<IColumn>();
        public GridDataType? DataType;
        public string EmptyRecords;
        public bool? FooterRow;
        public bool? UserDataOnFooter { get; set; }
        public bool? ForceFit;
        public bool? GridView;
        public bool? HeaderTitles;
        public int? Height;
        public bool? HiddenGrid;
        public bool? HideGrid;
        public bool? HoverRows;
        public bool? LoadOnce;
        public string LoadText;
        public GridLoadUi? LoadUi;
        public GridMultiKey? MultiKey;
        public bool? MultiBoxOnly;
        public bool? MultiSelect;
        public int? MultiSelectWidth;
        public string EditUrl;
        public string OnAfterInsertRow;
        public string OnBeforeRequest;
        public string OnBeforeSelectRow;
        public string OnGridComplete;
        public string OnLoadBeforeSend;
        public string OnLoadComplete;
        public string OnLoadError;
        public string OnCellSelect;
        public string OnDblClickRow;
        public string OnHeaderClick;
        public string OnPaging;
        public string OnRightClickRow;
        public string OnSelectAll;
        public string OnSelectRow;
        public string OnSortCol;
        public string OnResizeStart;
        public string OnResizeStop;
        public string OnSerializeGridData;
        public int? Page;
        public string PagerId;
        public GridPagerPos? PagerPos;
        public bool? PgButtons;
        public bool? PgInput;
        public string PgText;
        public GridRecordPos? RecordPos;
        public string RecordText;
        public GridRequestType? RequestType;
        public string ResizeClass;
        public int[] RowList;
        public int? RowNum;
        public bool? RowNumbers;
        public int? RowNumWidth;
        public bool? Scroll;
        public int? ScrollInt;
        public int? ScrollOffset;
        public bool? ScrollRows;
        public int? ScrollTimeout;
        public bool? ShrinkToFit;
        public string SortName;
        public GridSortOrder? SortOrder;
        public bool? TopPager;
        public bool? Toolbar;
        public GridToolbarPosition ToolbarPosition = GridToolbarPosition.top;
        public bool? SearchToolbar;
        public bool? SearchOnEnter;
        public bool? SearchClearButton;
        public bool? SearchToggleButton;
        public string Url;
        public string GetUrlFn;
        public bool? ViewRecords;
        public bool? ShowAllSortIcons;
        public GridDirection? SortIconDirection;
        public bool? SortOnHeaderClick;
        public int? Width;
        public List<CustomButton> CustomButtons = new List<CustomButton>();
        public List<string> ChainMethods = new List<string>();
        public object PostData;
        public object LocalData;
        public object InlineData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of grid</param>
        public Grid(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id must contain a value to identify the grid");
            }

            Id = id;
        }

        /// <summary>
        /// Creates and returns javascript + required html elements to render grid
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Create javascript
            var script = new StringBuilder();

            // Start script
            script.AppendLine("<script type=\"text/javascript\">");
            script.AppendLine("$(function () {");
            script.AppendLine("setTimeout(function(){");
            script.AppendLine("$('#" + Id + "').jqGrid({");

            // Altrows
            if (AltRows.HasValue) script.AppendFormat("altRows: {0},", AltRows.Value.ToString().ToLower()).AppendLine();

            // Altclass
            if (!string.IsNullOrWhiteSpace(AltClass)) script.AppendFormat("altclass: '{0}',", AltClass).AppendLine();

            // Autoencode
            if (AutoEncode.HasValue) script.AppendFormat("autoencode: {0},", AutoEncode.Value.ToString().ToLower()).AppendLine();

            // Autowidth
            if (AutoWidth.HasValue) script.AppendFormat("autowidth: {0},", AutoWidth.Value.ToString().ToLower()).AppendLine();

            // Caption
            if (!string.IsNullOrWhiteSpace(Caption)) script.AppendFormat("caption: '{0}',", Caption).AppendLine();

            // Datatype
            if (DataType.HasValue)  script.AppendLine(string.Format("datatype:'{0}',", DataType.Value));

            // Emptyrecords
            if (!string.IsNullOrWhiteSpace(EmptyRecords)) script.AppendFormat("emptyrecords: '{0}',", EmptyRecords).AppendLine();

            // FooterRow
            if (FooterRow.HasValue) script.AppendFormat("footerrow: {0},", FooterRow.Value.ToString().ToLower()).AppendLine();
            if (UserDataOnFooter.HasValue) script.AppendFormat("userDataOnFooter: {0},", UserDataOnFooter.Value.ToString().ToLower()).AppendLine();

            // Forcefit
            if (ForceFit.HasValue) script.AppendFormat("forceFit: {0},", ForceFit.Value.ToString().ToLower()).AppendLine();

            // Gridview
            if (GridView.HasValue) script.AppendFormat("gridview: {0},", GridView.Value.ToString().ToLower()).AppendLine();

            // HeaderTitles
            if (HeaderTitles.HasValue) script.AppendFormat("headertitles: {0},", HeaderTitles.Value.ToString().ToLower()).AppendLine();

            // Height (set 100% if no value is specified except when scroll is set to true otherwise layout is not as it is supposed to be)
            if (!Height.HasValue)
            {
                //if ((!_scroll.HasValue || _scroll.Value == false) && !_scrollInt.HasValue) script.AppendFormat("height: '{0}',", "100%").AppendLine();
            }
            else script.AppendFormat("height: {0},", Height).AppendLine();

            // Hiddengrid
            if (HiddenGrid.HasValue) script.AppendFormat("hiddengrid: {0},", HiddenGrid.Value.ToString().ToLower()).AppendLine();

            // Hidegrid
            if (HideGrid.HasValue) script.AppendFormat("hidegrid: {0},", HideGrid.Value.ToString().ToLower()).AppendLine();

            // HoverRows
            if (HoverRows.HasValue) script.AppendFormat("hoverrows: {0},", HoverRows.Value.ToString().ToLower()).AppendLine();

            // Loadonce
            if (LoadOnce.HasValue) script.AppendFormat("loadonce: {0},", LoadOnce.Value.ToString().ToLower()).AppendLine();

            // Loadtext
            if (!string.IsNullOrWhiteSpace(LoadText)) script.AppendFormat("loadtext: '{0}',", LoadText).AppendLine();

            // LoadUi
            if (LoadUi.HasValue) script.AppendFormat("loadui: '{0}',", LoadUi.Value).AppendLine();

            // MultiBoxOnly
            if (MultiBoxOnly.HasValue) script.AppendFormat("multiboxonly: {0},", MultiBoxOnly.Value.ToString().ToLower()).AppendLine();

            // MultiKey
            if (MultiKey.HasValue) script.AppendFormat("multikey: '{0}',", MultiKey.Value).AppendLine();

            // MultiSelect
            if (MultiSelect.HasValue) script.AppendFormat("multiselect: {0},", MultiSelect.Value.ToString().ToLower()).AppendLine();

            // MultiSelectWidth
            if (MultiSelectWidth.HasValue) script.AppendFormat("multiselectWidth: {0},", MultiSelectWidth.Value).AppendLine();

            // Page
            if (Page.HasValue) script.AppendFormat("page:{0},", Page.Value).AppendLine();

            // Pager
            if (!string.IsNullOrWhiteSpace(PagerId)) script.AppendFormat("pager:'#{0}',", PagerId).AppendLine();

            // PagerPos
            if (PagerPos.HasValue) script.AppendFormat("pagerpos: '{0}',", PagerPos).AppendLine();

            // PgButtons
            if (PgButtons.HasValue) script.AppendFormat("pgbuttons:{0},", PgButtons.Value.ToString().ToLower()).AppendLine();

            // PgInput
            if (PgInput.HasValue) script.AppendFormat("pginput: {0},", PgInput.Value.ToString().ToLower()).AppendLine();

            // PGText
            if (!string.IsNullOrWhiteSpace(PgText)) script.AppendFormat("pgtext: '{0}',", PgText).AppendLine();

            // RecordPos
            if (RecordPos.HasValue) script.AppendFormat("recordpos: '{0}',", RecordPos.Value).AppendLine();

            // RecordText
            if (!string.IsNullOrWhiteSpace(RecordText)) script.AppendFormat("recordtext: '{0}',", RecordText).AppendLine();

            // Request Type
            if (RequestType.HasValue) script.AppendFormat("mtype: '{0}',", RequestType).AppendLine();

            // ResizeClass
            if (!string.IsNullOrWhiteSpace(ResizeClass)) script.AppendFormat("resizeclass: '{0}',", ResizeClass).AppendLine();

            // Rowlist
            if (RowList != null) script.AppendFormat("rowList: [{0}],", string.Join(",", ((from p in RowList select p.ToString()).ToArray()))).AppendLine();

            // Rownum
            if (RowNum.HasValue) script.AppendFormat("rowNum:{0},", RowNum.Value).AppendLine();

            // Rownumbers
            if (RowNumbers.HasValue) script.AppendFormat("rownumbers: {0},", RowNumbers.Value.ToString().ToLower()).AppendLine();

            // RowNumWidth
            if (RowNumWidth.HasValue) script.AppendFormat("rownumWidth: {0},", RowNumWidth.Value).AppendLine();

            // Scroll (setters make sure either scroll or scrollint is set, never both)
            if (Scroll.HasValue) script.AppendFormat("scroll:{0},", Scroll.ToString().ToLower()).AppendLine();
            if (ScrollInt.HasValue) script.AppendFormat("scroll:{0},", ScrollInt.Value).AppendLine();

            // ScrollOffset
            if (ScrollOffset.HasValue) script.AppendFormat("scrollOffset: {0},", ScrollOffset.Value).AppendLine();

            // ScrollRows
            if (ScrollRows.HasValue) script.AppendFormat("scrollrows: {0},", ScrollRows.ToString().ToLower()).AppendLine();

            // ScrollTimeout
            if (ScrollTimeout.HasValue) script.AppendFormat("scrollTimeout: {0},", ScrollTimeout.Value).AppendLine();

            // Sortname
            if (!string.IsNullOrWhiteSpace(SortName)) script.AppendFormat("sortname: '{0}',", SortName).AppendLine();

            // Sorticons
            if (ShowAllSortIcons.HasValue || SortIconDirection.HasValue || SortOnHeaderClick.HasValue)
            {
                // Set defaults
                if (!ShowAllSortIcons.HasValue) ShowAllSortIcons = false;
                if (!SortIconDirection.HasValue) SortIconDirection = GridDirection.vertical;
                if (!SortOnHeaderClick.HasValue) SortOnHeaderClick = true;

                script.AppendFormat("viewsortcols: [{0},'{1}',{2}],", ShowAllSortIcons.Value.ToString().ToLower(), SortIconDirection, SortOnHeaderClick.Value.ToString().ToLower()).AppendLine();
            }

            // Shrink to fit
            if (ShrinkToFit.HasValue) script.AppendFormat("shrinkToFit: {0},", ShrinkToFit.Value.ToString().ToLower()).AppendLine();

            // Sortorder
            if (SortOrder.HasValue) script.AppendFormat("sortorder: '{0}',", SortOrder.Value).AppendLine();

            // Toolbar
            if (Toolbar.HasValue) script.AppendFormat("toolbar: [{0},\"{1}\"],", Toolbar.Value.ToString().ToLower(), ToolbarPosition).AppendLine();

            // Toppager
            if (TopPager.HasValue) script.AppendFormat("toppager: {0},", TopPager.Value.ToString().ToLower()).AppendLine();

            // Url
            if (!string.IsNullOrWhiteSpace(Url))
            {
                // TODO: Create shared function for generating script that support both string or statement
                if (Url.Contains("("))
                {
                    script.AppendFormat("url:{0},", Url).AppendLine();
                }
                else
                {
                    script.AppendFormat("url:'{0}',", Url).AppendLine();
                }
            }
            else if (!string.IsNullOrWhiteSpace(GetUrlFn))
            {
                script.AppendFormat("url:{0},", GetUrlFn).AppendLine();
            }
            
            // EditUrl
            if (!string.IsNullOrWhiteSpace(EditUrl))
            {
                script.AppendFormat("editurl:'{0}',", EditUrl).AppendLine();
            }

            // Post Data
            if (PostData != null)
            {
                script.AppendFormat("postData:{0},", JsonConvert.SerializeObject(PostData));
            }

            // Local Data
            if (LocalData != null)
            {
                script.AppendFormat("datatype: 'local',");
                script.AppendFormat("data:{0},", JsonConvert.SerializeObject(LocalData));
            }

            // Inline Data
            if (InlineData != null)
            {
                script.AppendFormat("inlineData:{0},", JsonConvert.SerializeObject(InlineData));
            }

            // View records
            if (ViewRecords.HasValue) script.AppendFormat("viewrecords: {0},", ViewRecords.Value.ToString().ToLower()).AppendLine();

            // Width
            if (Width.HasValue) script.AppendFormat("width:'{0}',", Width.Value).AppendLine();

            // onAfterInsertRow
            if (!string.IsNullOrWhiteSpace(OnAfterInsertRow)) script.AppendFormat("afterInsertRow: {0},", OnAfterInsertRow).AppendLine();

            // onBeforeRequest
            if (!string.IsNullOrWhiteSpace(OnBeforeRequest)) script.AppendFormat("beforeRequest: {0},", OnBeforeRequest).AppendLine();

            // onBeforeSelectRow
            if (!string.IsNullOrWhiteSpace(OnBeforeSelectRow)) script.AppendFormat("beforeSelectRow: {0},", OnBeforeSelectRow).AppendLine();

            // onGridComplete
            if (!string.IsNullOrWhiteSpace(OnGridComplete)) script.AppendFormat("gridComplete: {0},", OnGridComplete).AppendLine();

            // onLoadBeforeSend
            if (!string.IsNullOrWhiteSpace(OnLoadBeforeSend)) script.AppendFormat("loadBeforeSend: {0},", OnLoadBeforeSend).AppendLine();

            // onLoadComplete
            if (!string.IsNullOrWhiteSpace(OnLoadComplete)) script.AppendFormat("loadComplete: {0},", OnLoadComplete).AppendLine();

            // onLoadError
            if (!string.IsNullOrWhiteSpace(OnLoadError)) script.AppendFormat("loadError: {0},", OnLoadError).AppendLine();

            // onCellSelect
            if (!string.IsNullOrWhiteSpace(OnCellSelect)) script.AppendFormat("onCellSelect: {0},", OnCellSelect).AppendLine();

            // onDblClickRow
            if (!string.IsNullOrWhiteSpace(OnDblClickRow)) script.AppendFormat("ondblClickRow: {0},", OnDblClickRow).AppendLine();

            // onHeaderClick
            if (!string.IsNullOrWhiteSpace(OnHeaderClick)) script.AppendFormat("onHeaderClick: {0},", OnHeaderClick).AppendLine();

            // onPaging
            if (!string.IsNullOrWhiteSpace(OnPaging)) script.AppendFormat("onPaging: {0},", OnPaging).AppendLine();

            // onRightClickRow
            if (!string.IsNullOrWhiteSpace(OnRightClickRow)) script.AppendFormat("onRightClickRow: {0},", OnRightClickRow).AppendLine();

            // onSelectAll
            if (!string.IsNullOrWhiteSpace(OnSelectAll)) script.AppendFormat("onSelectAll: {0},", OnSelectAll).AppendLine();

            // onSelectRow event
            if (!string.IsNullOrWhiteSpace(OnSelectRow)) script.AppendFormat("onSelectRow: {0},", OnSelectRow).AppendLine();

            // onSortCol
            if (!string.IsNullOrWhiteSpace(OnSortCol)) script.AppendFormat("onSortCol: {0},", OnSortCol).AppendLine();

            // onResizeStart
            if (!string.IsNullOrWhiteSpace(OnResizeStart)) script.AppendFormat("resizeStart: {0},", OnResizeStart).AppendLine();

            // onResizeStop
            if (!string.IsNullOrWhiteSpace(OnResizeStop)) script.AppendFormat("resizeStop: {0},", OnResizeStop).AppendLine();

            // onSerializeGridData
            if (!string.IsNullOrWhiteSpace(OnSerializeGridData)) script.AppendFormat("serializeGridData: {0},", OnSerializeGridData).AppendLine();

            // Colmodel
            script.AppendLine("colModel: [");
            var colModel = string.Join(",", ((from c in Columns select c.ToString()).ToArray()));
            script.AppendLine(colModel);
            script.AppendLine("]");

            // End jqGrid call
            if (CustomButtons.Count == 0 || string.IsNullOrWhiteSpace(PagerId))
            {
                if (ChainMethods.Count == 0)
                {
                    script.AppendLine("});");
                }
                else
                {
                    script.Append("})");

                    foreach (var method in ChainMethods)
                    {
                        script.AppendLine();
                        script.Append(method.StartsWith(".") ? method : "." + method);
                    }

                    script.AppendLine(";");
                }
            }
            else
            {
                script.AppendLine("})");

                // TODO: Create this script with navigation grid object.
                script.AppendFormat(".navGrid('#{0}', {{ view: false, del: false, add: false, edit: false, search: false, refresh: false }}, {{}}, {{}}, {{}}, {{}}, {{}})", PagerId).AppendLine();

                foreach (var customButton in CustomButtons)
                {
                    script.Append(customButton);
                }
                script.Append(";");
            }

            // Search clear button
            if (SearchToolbar == true && SearchClearButton.HasValue && !string.IsNullOrWhiteSpace(PagerId) && SearchClearButton.Value)
            {
                script.AppendLine("$('#" + Id + "').jqGrid('navGrid',\"#" + PagerId + "\",{edit:false,add:false,del:false,search:false,refresh:false}); ");
                script.AppendLine("$('#" + Id + "').jqGrid('navButtonAdd',\"#" + PagerId + "\",{caption:\"Clear\",title:\"Clear Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){mygrid[0].clearToolbar(); }}); ");
            }
            // Search toolbar
            if (SearchToolbar == true)
            {
                script.Append("$('#" + Id + "').jqGrid('filterToolbar', {stringResult:true");
                if (SearchOnEnter.HasValue) script.AppendFormat(", searchOnEnter:{0}", SearchOnEnter.Value.ToString().ToLower());
                script.AppendLine("});");
            }

            // End script
            script.AppendLine("}, " + CreateScriptDelay + ")");
            script.AppendLine("});");
            script.AppendLine("</script>");

            // Create table which is used to render grid
            var table = new StringBuilder();
            table.AppendFormat("<table id=\"{0}\"></table>", Id);

            // Create pager element if is set
            var pager = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(PagerId))
            {
                pager.AppendFormat("<div id=\"{0}\"></div>", PagerId);
            }

            // Create toppager element if is set
            var topPager = new StringBuilder();
            if (TopPager == true)
            {
                topPager.AppendFormat("<div id=\"{0}_toppager\"></div>", Id);
            }

            // Insert grid id where needed (in columns)
            script.Replace("##gridid##", Id);

            // Return script + required elements
            return "" + script + table + pager + topPager;
        }
    }
}