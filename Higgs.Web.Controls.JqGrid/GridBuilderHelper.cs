using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Higgs.Web.Controls.JqGrid.Builders;
using Higgs.Web.Controls.JqGrid.Models;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls.JqGrid
{
    public static class GridBuilderHelper
    {
        public static GridBuilder JqGrid(this HtmlHelper helper, string id, int createScriptDelay = 100)
        {
            var builder = new GridBuilder(new Grid(id));
            builder.Pager(string.Format(GridBuilder.DefaultPagerName, id));
            builder.CurrentGrid.CreateScriptDelay = createScriptDelay;

            return builder;
        }

        /// <summary>
        /// The class that is used for alternate rows. You can construct your own class and replace this value. 
        /// This option is valid only if altRows options is set to true (default: ui-priority-secondary)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="altClass">Classname for alternate rows</param>
        public static GridBuilder AltClass(this GridBuilder builder, string altClass)
        {
            builder.CurrentGrid.AltClass =altClass;
            return builder;
        }

        /// <summary>
        /// Set a zebra-striped grid (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="altRows">Boolean indicating if zebra-striped grid is used</param>
        public static GridBuilder AltRows(this GridBuilder builder, Boolean altRows)
        {
            builder.CurrentGrid.AltRows =altRows;
            return builder;
        }

#pragma warning disable 1570

        /// <summary>
        /// When set to true encodes (html encode) the incoming (from server) and posted 
        /// data (from editing modules). For example < will be converted to &lt (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="autoEncode">Boolean indicating if autoencode is used</param>
        public static GridBuilder AutoEncode(this GridBuilder builder, bool autoEncode)
        {
            builder.CurrentGrid.AutoEncode =autoEncode;
            return builder;
        }

#pragma warning restore 1570

        /// <summary>
        /// When set to true, the grid width is recalculated automatically to the width of the 
        /// parent element. This is done only initially when the grid is created. In order to 
        /// resize the grid when the parent element changes width you should apply custom code 
        /// and use a setGridWidth method for this purpose. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="autoWidth">Boolean indicating if autowidth is used</param>
        public static GridBuilder AutoWidth(this GridBuilder builder, bool autoWidth)
        {
            builder.CurrentGrid.AutoWidth =autoWidth;
            return builder;
        }

        /// <summary>
        /// Defines the caption layer for the grid. This caption appears above the header layer. 
        /// If the string is empty the caption does not appear. (default: empty)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="caption">Caption of grid</param>
        public static GridBuilder Caption(this GridBuilder builder, string caption)
        {
            builder.CurrentGrid.Caption =caption;
            return builder;
        }

        /// <summary>
        /// Defines what type of information to expect to represent data in the grid. Valid 
        /// options are json (default) and xml
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="dataType">Data type</param>
        public static GridBuilder DataType(this GridBuilder builder, GridDataType dataType)
        {
            builder.CurrentGrid.DataType =dataType;
            return builder;
        }

        /// <summary>
        /// Displayed when the returned (or the current) number of records is zero. 
        /// This option is valid only if viewrecords option is set to true. (default value is 
        /// set in language file)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="emptyRecords">Display string</param>
        public static GridBuilder EmptyRecords(this GridBuilder builder, string emptyRecords)
        {
            builder.CurrentGrid.EmptyRecords =emptyRecords;
            return builder;
        }

        /// <summary>
        /// If set to true this will place a footer table with one row below the grid records 
        /// and above the pager. The number of columns equal to the number of columns in the colModel 
        /// (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="footerRow">Boolean indicating whether footerrow is displayed</param>
        /// <param name="userDataOnFooter">Boolean indicating whether use userData to display in footer</param>
        public static GridBuilder FooterRow(this GridBuilder builder, bool footerRow, bool? userDataOnFooter = null)
        {
            builder.CurrentGrid.FooterRow =footerRow;
            builder.CurrentGrid.UserDataOnFooter =userDataOnFooter;

            return builder;
        }

        /// <summary>
        /// If set to true, when resizing the width of a column, the adjacent column (to the right) 
        /// will resize so that the overall grid width is maintained (e.g., reducing the width of 
        /// column 2 by 30px will increase the size of column 3 by 30px). 
        /// In this case there is no horizontal scrolbar. 
        /// Note: this option is not compatible with shrinkToFit option - i.e if 
        /// shrinkToFit is set to false, forceFit is ignored.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="forceFit">Boolean indicating if forcefit is enforced</param>
        public static GridBuilder ForceFit(this GridBuilder builder, bool forceFit)
        {
            builder.CurrentGrid.ForceFit =forceFit;
            return builder;
        }

        /// <summary>
        /// In the previous versions of jqGrid including 3.4.X,reading relatively big data sets 
        /// (Rows >=100 ) caused speed problems. The reason for this was that as every cell was 
        /// inserted into the grid we applied about 5-6 jQuery calls to it. Now this problem has 
        /// been resolved; we now insert the entry row at once with a jQuery append. The result is 
        /// impressive - about 3-5 times faster. What will be the result if we insert all the 
        /// data at once? Yes, this can be done with help of the gridview option. When set to true, 
        /// the result is a grid that is 5 to 10 times faster. Of course when this option is set 
        /// to true we have some limitations. If set to true we can not use treeGrid, subGrid, 
        /// or afterInsertRow event. If you do not use these three options in the grid you can 
        /// set this option to true and enjoy the speed. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="gridView">Boolean indicating gridview is enabled</param>
        public static GridBuilder GridView(this GridBuilder builder, bool gridView)
        {
            builder.CurrentGrid.GridView =gridView;
            return builder;
        }

        /// <summary>
        /// If the option is set to true the title attribute is added to the column headers (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="headerTitles">Boolean indicating if headertitles are enabled</param>
        public static GridBuilder HeaderTitles(this GridBuilder builder, bool headerTitles)
        {
            builder.CurrentGrid.HeaderTitles =headerTitles;
            return builder;
        }

        /// <summary>
        /// The height of the grid in pixels (default: 100%, which is the only acceptable percentage for jqGrid)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="height">Height in pixels</param>
        public static GridBuilder Height(this GridBuilder builder, int height)
        {
            builder.CurrentGrid.Height =height;
            return builder;
        }

        /// <summary>
        /// If set to true the grid initially is hidden. The data is not loaded (no request is sent) and only the 
        /// caption layer is shown. When the show/hide button is clicked for the first time to show the grid, the request 
        /// is sent to the server, the data is loaded, and the grid is shown. From this point on we have a regular grid. 
        /// This option has effect only if the caption property is not empty. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hiddenGrid">Boolean indicating if hiddengrid is enforced</param>
        public static GridBuilder HiddenGrid(this GridBuilder builder, bool hiddenGrid)
        {
            builder.CurrentGrid.HiddenGrid =hiddenGrid;
            return builder;
        }

        /// <summary>
        /// Enables or disables the show/hide grid button, which appears on the right side of the caption layer. 
        /// Takes effect only if the caption property is not an empty string. (default: true) 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hideGrid">Boolean indicating if show/hide button is enabled</param>
        public static GridBuilder HideGrid(this GridBuilder builder, bool hideGrid)
        {
            builder.CurrentGrid.HideGrid =hideGrid;
            return builder;
        }

        /// <summary>
        /// When set to false the mouse hovering is disabled in the grid data rows. (default: true)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="hoverRows">Indicates whether hoverrows is enabled</param>
        public static GridBuilder HoverRows(this GridBuilder builder, bool hoverRows)
        {
            builder.CurrentGrid.HoverRows =hoverRows;
            return builder;
        }

        /// <summary>
        /// If this flag is set to true, the grid loads the data from the server only once (using the 
        /// appropriate datatype). After the first request the datatype parameter is automatically 
        /// changed to local and all further manipulations are done on the client side. The functions 
        /// of the pager (if present) are disabled. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loadOnce">Boolean indicating if loadonce is enforced</param>
        public static GridBuilder LoadOnce(this GridBuilder builder, bool loadOnce)
        {
            builder.CurrentGrid.LoadOnce =loadOnce;
            return builder;
        }

        /// <summary>
        /// The text which appears when requesting and sorting data. This parameter override the value located 
        /// in the language file
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loadText">Loadtext</param>
        public static GridBuilder LoadText(this GridBuilder builder, string loadText)
        {
            builder.CurrentGrid.LoadText =loadText;
            return builder;
        }

        /// <summary>
        /// This option controls what to do when an ajax operation is in progress.
        /// 'disable' - disables the jqGrid progress indicator. This way you can use your own indicator.
        /// 'enable' (default) - enables “Loading” message in the center of the grid. 
        /// 'block' - enables the “Loading” message and blocks all actions in the grid until the ajax request 
        /// is finished. Note that this disables paging, sorting and all actions on toolbar, if any.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loadUi">Load ui mode</param>
        public static GridBuilder LoadUi(this GridBuilder builder, GridLoadUi loadUi)
        {
            builder.CurrentGrid.LoadUi =loadUi;
            return builder;
        }

        /// <summary>
        /// This parameter makes sense only when multiselect option is set to true. 
        /// Defines the key which will be pressed 
        /// when we make a multiselection. The possible values are: 
        /// 'shiftKey' - the user should press Shift Key 
        /// 'altKey' - the user should press Alt Key 
        /// 'ctrlKey' - the user should press Ctrl Key
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="multiKey">Key to multiselect</param>
        public static GridBuilder MultiKey(this GridBuilder builder, GridMultiKey multiKey)
        {
            builder.CurrentGrid.MultiKey =multiKey;
            return builder;
        }

        /// <summary>
        /// This option works only when multiselect = true. When multiselect is set to true, clicking anywhere 
        /// on a row selects that row; when multiboxonly is also set to true, the multiselection is done only 
        /// when the checkbox is clicked (Yahoo style). Clicking in any other row (suppose the checkbox is 
        /// not clicked) deselects all rows and the current row is selected. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="multiBoxOnly">Boolean indicating if multiboxonly is enforced</param>
        public static GridBuilder MultiBoxOnly(this GridBuilder builder, bool multiBoxOnly)
        {
            builder.CurrentGrid.MultiBoxOnly =multiBoxOnly;
            return builder;
        }

        /// <summary>
        /// If this flag is set to true a multi selection of rows is enabled. A new column 
        /// at the left side is added. Can be used with any datatype option. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="multiSelect">Boolean indicating if multiselect is enabled</param>
        public static GridBuilder MultiSelect(this GridBuilder builder, bool multiSelect)
        {
            builder.CurrentGrid.MultiSelect =multiSelect;
            return builder;
        }

        /// <summary>
        /// Determines the width of the multiselect column if multiselect is set to true. (default: 20)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="multiSelectWidth"></param>
        public static GridBuilder MultiSelectWidth(this GridBuilder builder, int multiSelectWidth)
        {
            builder.CurrentGrid.MultiSelectWidth =multiSelectWidth;
            return builder;
        }

        /// <summary>
        /// Set the initial number of selected page when we make the request.This parameter is passed to the url 
        /// for use by the server routine retrieving the data (default: 1)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="page">Number of page</param>
        public static GridBuilder Page(this GridBuilder builder, int page)
        {
            builder.CurrentGrid.Page =page;
            return builder;
        }

        /// <summary>
        /// If pagername is specified a pagerelement is automatically added to the grid 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pager">Id/name of pager</param>
        public static GridBuilder Pager(this GridBuilder builder, string pager)
        {
            builder.CurrentGrid.PagerId =pager;
            return builder;
        }

        /// <summary>
        /// Determines the position of the pager in the grid. By default the pager element
        /// when created is divided in 3 parts (one part for pager, one part for navigator 
        /// buttons and one part for record information) (default: center)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pagerPos">Position of pager</param>
        public static GridBuilder PagerPos(this GridBuilder builder, GridPagerPos pagerPos)
        {
            builder.CurrentGrid.PagerPos =pagerPos;
            return builder;
        }

        /// <summary>
        /// Determines if the pager buttons should be displayed if pager is available. Valid 
        /// only if pager is set correctly. The buttons are placed in the pager bar. (default: true)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pgButtons">Boolean indicating if pager buttons are displayed</param>
        public static GridBuilder PgButtons(this GridBuilder builder, bool pgButtons)
        {
            builder.CurrentGrid.PgButtons =pgButtons;
            return builder;
        }

        /// <summary>
        /// Determines if the input box, where the user can change the number of the requested page, 
        /// should be available. The input box appears in the pager bar. (default: true)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pgInput">Boolean indicating if pager input is available</param>
        public static GridBuilder PgInput(this GridBuilder builder, bool pgInput)
        {
            builder.CurrentGrid.PgInput =pgInput;
            return builder;
        }

        /// <summary>
        /// Show information about current page status. The first value is the current loaded page. 
        /// The second value is the total number of pages (default is set in language file)
        /// Example: "Page {0} of {1}"
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pgText">Current page status text</param>
        public static GridBuilder PgText(this GridBuilder builder, string pgText)
        {
            builder.CurrentGrid.PgText =pgText;
            return builder;
        }

        /// <summary>
        /// Determines the position of the record information in the pager. Can be left, center, right 
        /// (default: right)
        /// Warning: When pagerpos en recordpos are equally set, pager is hidden.        
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="recordPos">Position of record information</param>
        public static GridBuilder RecordPos(this GridBuilder builder, GridRecordPos recordPos)
        {
            builder.CurrentGrid.RecordPos =recordPos;
            return builder;
        }

        /// <summary>
        /// Represent information that can be shown in the pager. This option is valid if viewrecords 
        /// option is set to true. This text appears only if the total number of records is greater then 
        /// zero.
        /// In order to show or hide information the items between {} mean the following: {0} the 
        /// start position of the records depending on page number and number of requested records; 
        /// {1} - the end position {2} - total records returned from the data (default defined in language file)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="recordText">Record Text</param>
        public static GridBuilder RecordText(this GridBuilder builder, string recordText)
        {
            builder.CurrentGrid.RecordText =recordText;
            return builder;
        }

        /// <summary>
        /// Defines the type of request to make (“POST” or “GET”) (default: GET)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="requestType">Request type</param>
        public static GridBuilder RequestType(this GridBuilder builder, GridRequestType requestType)
        {
            builder.CurrentGrid.RequestType =requestType;
            return builder;
        }

        /// <summary>
        /// Assigns a class to columns that are resizable so that we can show a resize 
        /// handle (default: empty string)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="resizeClass"></param>
        /// <returns></returns>
        public static GridBuilder ResizeClass(this GridBuilder builder, string resizeClass)
        {
            builder.CurrentGrid.ResizeClass =resizeClass;
            return builder;
        }

        /// <summary>
        /// An array to construct a select box element in the pager in which we can change the number 
        /// of the visible rows. When changed during the execution, this parameter replaces the rowNum 
        /// parameter that is passed to the url. If the array is empty the element does not appear 
        /// in the pager. Typical you can set this like [10,20,30]. If the rowNum parameter is set to 
        /// 30 then the selected value in the select box is 30.
        /// </summary>
        /// <example>
        /// setRowList(new int[]{10,20,50})
        /// </example>
        /// <param name="builder"></param>
        /// <param name="rowList">List of rows per page</param>
        public static GridBuilder RowList(this GridBuilder builder, int[] rowList)
        {
            builder.CurrentGrid.RowList =rowList;
            return builder;
        }

        /// <summary>
        /// Sets how many records we want to view in the grid. This parameter is passed to the url 
        /// for use by the server routine retrieving the data. Note that if you set this parameter 
        /// to 10 (i.e. retrieve 10 records) and your server return 15 then only 10 records will be 
        /// loaded. Set this parameter to -1 (unlimited) to disable this checking. (default: 20)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="rowNum">Number of rows per page</param>
        public static GridBuilder RowNum(this GridBuilder builder, int rowNum)
        {
            builder.CurrentGrid.RowNum =rowNum;
            return builder;
        }

        /// <summary>
        /// If this option is set to true, a new column at the leftside of the grid is added. The purpose of 
        /// this column is to count the number of available rows, beginning from 1. In this case 
        /// colModel is extended automatically with a new element with name - 'rn'. Also, be careful 
        /// not to use the name 'rn' in colModel
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="rowNumbers">Boolean indicating if rownumbers are enabled</param>
        public static GridBuilder RowNumbers(this GridBuilder builder, bool rowNumbers)
        {
            builder.CurrentGrid.RowNumbers =rowNumbers;
            return builder;
        }

        /// <summary>
        /// Determines the width of the row number column if rownumbers option is set to true. (default: 25)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="rowNumWidth">Width of rownumbers column</param>
        public static GridBuilder RowNumWidth(this GridBuilder builder, int rowNumWidth)
        {
            builder.CurrentGrid.RowNumWidth =rowNumWidth;
            return builder;
        }

        /// <summary>
        /// Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the 
        /// vertical scrollbar to load data. When set to true the grid will always hold all the items from the 
        /// start through to the latest point ever visited. 
        /// When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to 
        /// load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scroll">Boolean indicating if scroll is enforced</param>
        public static GridBuilder Scroll(this GridBuilder builder, bool scroll)
        {
            builder.CurrentGrid.Scroll =scroll;
            if (builder.CurrentGrid.ScrollInt.HasValue)
            {
                throw new InvalidOperationException("You can't set scroll to both a boolean and an integer at the same time, please choose one.");
            }
            return builder;
        }

        /// <summary>
        /// Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the 
        /// vertical scrollbar to load data. When set to true the grid will always hold all the items from the 
        /// start through to the latest point ever visited. 
        /// When scroll is set to an integer value (eg 1), the grid will just hold the visible lines. This allow us to 
        /// load the data at portions whitout to care about the memory leaks. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scroll">When integer value is set (eg 1) scroll is enforced</param>
        public static GridBuilder Scroll(this GridBuilder builder, int scroll)
        {
            builder.CurrentGrid.ScrollInt =scroll;
            if (builder.CurrentGrid.Scroll.HasValue)
            {
                throw new InvalidOperationException("You can't set scroll to both a boolean and an integer at the same time, please choose one.");
            }
            return builder;
        }

        /// <summary>
        /// Determines the width of the vertical scrollbar. Since different browsers interpret this width 
        /// differently (and it is difficult to calculate it in all browsers) this can be changed. (default: 18)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scrollOffset">Scroll offset</param>
        public static GridBuilder ScrollOffset(this GridBuilder builder, int scrollOffset)
        {
            builder.CurrentGrid.ScrollOffset =scrollOffset;
            return builder;
        }

        /// <summary>
        /// When enabled, selecting a row with setSelection scrolls the grid so that the selected row is visible. 
        /// This is especially useful when we have a verticall scrolling grid and we use form editing with 
        /// navigation buttons (next or previous row). On navigating to a hidden row, the grid scrolls so the 
        /// selected row becomes visible. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scrollRows">Boolean indicating if scrollrows is enabled</param>
        public static GridBuilder ScrollRows(this GridBuilder builder, bool scrollRows)
        {
            builder.CurrentGrid.ScrollRows =scrollRows;
            return builder;
        }

        /// <summary>
        /// This controls the timeout handler when scroll is set to 1. (default: 200 milliseconds)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scrollTimeout">Scroll timeout in milliseconds</param>
        /// <returns></returns>
        public static GridBuilder ScrollTimeout(this GridBuilder builder, int scrollTimeout)
        {
            builder.CurrentGrid.ScrollTimeout =scrollTimeout;
            return builder;
        }

        /// <summary>
        /// This option describes the type of calculation of the initial width of each column 
        /// against the width of the grid. If the value is true and the value in width option 
        /// is set then: Every column width is scaled according to the defined option width. 
        /// Example: if we define two columns with a width of 80 and 120 pixels, but want the 
        /// grid to have a 300 pixels - then the columns are recalculated as follow: 
        /// 1- column = 300(new width)/200(sum of all width)*80(column width) = 120 and 2 column = 300/200*120 = 180. 
        /// The grid width is 300px. If the value is false and the value in width option is set then: 
        /// The width of the grid is the width set in option. 
        /// The column width are not recalculated and have the values defined in colModel. (default: true)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="shrinkToFit">Boolean indicating if shrink to fit is enforced</param>
        public static GridBuilder ShrinkToFit(this GridBuilder builder, bool shrinkToFit)
        {
            builder.CurrentGrid.ShrinkToFit =shrinkToFit;
            return builder;
        }

        /// <summary>
        /// Determines how the search should be applied. If this option is set to true search is started when 
        /// the user hits the enter key. If the option is false then the search is performed immediately after 
        /// the user presses some character. (default: true
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="searchOnEnter">Indicates if search is started on enter</param>
        public static GridBuilder SearchOnEnter(this GridBuilder builder, bool searchOnEnter)
        {
            builder.CurrentGrid.SearchOnEnter =searchOnEnter;
            return builder;
        }

        /// <summary>
        /// Enables toolbar searching / filtering
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="searchToolbar">Indicates if toolbar searching is enabled</param>
        public static GridBuilder SearchToolbar(this GridBuilder builder, bool searchToolbar)
        {
            builder.CurrentGrid.SearchToolbar =searchToolbar;
            return builder;
        }

        /// <summary>
        /// When set to true adds clear button to clear all search entries (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="searchClearButton"></param>
        /// <returns></returns>
        public static GridBuilder SearchClearButton(this GridBuilder builder, bool searchClearButton)
        {
            builder.CurrentGrid.SearchClearButton =searchClearButton;
            return builder;
        }

        /// <summary>
        /// When set to true adds toggle button to toggle search toolbar (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="searchToggleButton">Indicates if toggle button is displayed</param>
        public static GridBuilder SearchToggleButton(this GridBuilder builder, bool searchToggleButton)
        {
            builder.CurrentGrid.SearchToggleButton =searchToggleButton;
            return builder;
        }

        /// <summary>
        /// If enabled all sort icons are visible for all columns which are sortable (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="showAllSortIcons">Boolean indicating if all sorting icons should be displayed</param>
        public static GridBuilder ShowAllSortIcons(this GridBuilder builder, bool showAllSortIcons)
        {
            builder.CurrentGrid.ShowAllSortIcons =showAllSortIcons;
            return builder;
        }

        /// <summary>
        /// Sets direction in which sort icons are displayed (default: vertical)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sortIconDirection">Direction in which sort icons are displayed</param>
        public static GridBuilder SortIconDirection(this GridBuilder builder, GridDirection sortIconDirection)
        {
            builder.CurrentGrid.SortIconDirection =sortIconDirection;
            return builder;
        }

        /// <summary>
        /// If enabled columns are sorted when header is clicked (default: true)
        /// Warning, if disabled and setShowAllSortIcons is set to false, sorting will
        /// be effectively disabled
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sortOnHeaderClick">Boolean indicating if columns will sort on headerclick</param>
        /// <returns></returns>
        public static GridBuilder SortOnHeaderClick(this GridBuilder builder, bool sortOnHeaderClick)
        {
            builder.CurrentGrid.SortOnHeaderClick =sortOnHeaderClick;
            return builder;
        }

        public static GridBuilder Sort(this GridBuilder builder, string sortName, GridSortOrder sortOrder)
        {
            builder.CurrentGrid.SortName =sortName;
            builder.CurrentGrid.SortOrder =sortOrder;

            return builder;
        }

        /// <summary>
        /// The initial sorting name when we use datatypes xml or json (data returned from server). 
        /// This parameter is added to the url. If set and the index (name) matches the name from the
        /// colModel then by default an image sorting icon is added to the column, according to 
        /// the parameter sortorder.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sortName"></param>
        public static GridBuilder SortName(this GridBuilder builder, string sortName)
        {
            builder.CurrentGrid.SortName =sortName;
            return builder;
        }

        /// <summary>
        /// The initial sorting order when we use datatypes xml or json (data returned from server).
        /// This parameter is added to the url. Two possible values - asc or desc. (default: asc)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sortOrder">Sortorder</param>
        public static GridBuilder SortOrder(this GridBuilder builder, GridSortOrder sortOrder)
        {
            builder.CurrentGrid.SortOrder =sortOrder;
            return builder;
        }

        /// <summary>
        /// This option enabled the toolbar of the grid.  When we have two toolbars (can be set using setToolbarPosition)
        /// then two elements (div) are automatically created. The id of the top bar is constructed like 
        /// “t_”+id of the grid and the bottom toolbar the id is “tb_”+id of the grid. In case when 
        /// only one toolbar is created we have the id as “t_” + id of the grid, independent of where 
        /// this toolbar is created (top or bottom). You can use jquery to add elements to the toolbars.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="toolbar">Boolean indicating if toolbar is enabled</param>
        public static GridBuilder Toolbar(this GridBuilder builder, bool toolbar)
        {
            builder.CurrentGrid.Toolbar =toolbar;
            return builder;
        }

        /// <summary>
        /// Sets toolbarposition (default: top)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="toolbarPosition">Position of toolbar</param>
        public static GridBuilder ToolbarPosition(this GridBuilder builder, GridToolbarPosition toolbarPosition)
        {
            builder.CurrentGrid.ToolbarPosition =toolbarPosition;
            return builder;
        }

        /// <summary>
        /// When enabled this option place a pager element at top of the grid below the caption 
        /// (if available). If another pager is defined both can coexists and are refreshed in sync. 
        /// (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="topPager">Boolean indicating if toppager is enabled</param>
        public static GridBuilder TopPager(this GridBuilder builder, bool topPager)
        {
            builder.CurrentGrid.TopPager =topPager;
            return builder;
        }

        /// <summary>
        /// The url of the file that holds the request
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="url">Data url</param>
        /// <param name="dataType"></param>
        public static GridBuilder Url(this GridBuilder builder, string url, GridDataType? dataType = null)
        {
            builder.CurrentGrid.Url =url;

            if (dataType.HasValue)
            {
                builder.DataType(dataType.Value);
            }

            return builder;
        }

        public static GridBuilder UrlFunction(this GridBuilder builder, string fnName, GridDataType? dataType = null)
        {
            builder.CurrentGrid.GetUrlFn =fnName;

            if (dataType.HasValue)
            {
                builder.DataType(dataType.Value);
            }

            return builder;
        }

        /// <summary>
        /// Before use this extension method, you must provide "getUrl" JavaScript function 
        /// to resolve logical Url like "~/Product/GetData" to "http://www.my-site.com/Product/GetData"
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="builder"></param>
        /// <param name="exp"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static GridBuilder Url<TController>(this GridBuilder builder, Expression<Func<TController, object>> exp, GridDataType? dataType = null)
            where TController : IController
        {
            return builder.Url(CommonHelper.ResolveUrl(exp.GetLogicalPath()), dataType);
        }

        /// <summary>
        /// The edit url of the file that holds the request
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="url">Data url</param>
        public static GridBuilder EditUrl(this GridBuilder builder, string url)
        {
            builder.CurrentGrid.EditUrl =url;
            
            return builder;
        }

        /// <summary>
        /// Before use this extension method, you must provide "getUrl" JavaScript function 
        /// to resolve logical Url like "~/Product/GetData" to "http://www.my-site.com/Product/GetData"
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="builder"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static GridBuilder EditUrl<TController>(this GridBuilder builder, Expression<Func<TController, object>> exp)
            where TController : IController
        {
            return builder.EditUrl(CommonHelper.ResolveUrl(exp.GetLogicalPath()));
        }

        public static GridBuilder InlineData(this GridBuilder builder, object data)
        {
            builder.CurrentGrid.InlineData = data;

            return builder;
        }
        
        /// <summary>
        /// If true, jqGrid displays the beginning and ending record number in the grid, 
        /// out of the total number of records in the query. 
        /// This information is shown in the pager bar (bottom right by default)in this format: 
        /// “View X to Y out of Z”. 
        /// If this value is true, there are other parameters that can be adjusted, 
        /// including 'emptyrecords' and 'recordtext'. (default: false)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="viewRecords">Boolean indicating if recordnumbers are shown in grid</param>
        public static GridBuilder ViewRecords(this GridBuilder builder, bool viewRecords)
        {
            builder.CurrentGrid.ViewRecords =viewRecords;
            return builder;
        }

        /// <summary>
        /// If this option is not set, the width of the grid is a sum of the widths of the columns 
        /// defined in the colModel (in pixels). If this option is set, the initial width of each 
        /// column is set according to the value of shrinkToFit option.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="width">Width in pixels</param>
        public static GridBuilder Width(this GridBuilder builder, int width)
        {
            builder.CurrentGrid.Width =width;
            return builder;
        }

        /// <summary>
        /// This event fires after each inserted row.
        /// Variables available in call:
        /// 'rowid': Id of the inserted row 
        /// 'rowdata': An array of the data to be inserted into the row. This is array of type name- 
        /// value, where the name is a name from colModel 
        /// 'rowelem': The element from the response. If the data is xml this is the xml element of the row; 
        /// if the data is json this is array containing all the data for the row 
        /// Note: this event does not fire if gridview option is set to true
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onAfterInsertRow">Script to be executed</param>
        public static GridBuilder OnAfterInsertRow(this GridBuilder builder, string onAfterInsertRow)
        {
            builder.CurrentGrid.OnAfterInsertRow =onAfterInsertRow;
            return builder;
        }

        /// <summary>
        /// This event fires before requesting any data. Does not fire if datatype is function
        /// Variables available in call: None
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onBeforeRequest">Script to be executed</param>
        public static GridBuilder OnBeforeRequest(this GridBuilder builder, string onBeforeRequest)
        {
            builder.CurrentGrid.OnBeforeRequest =onBeforeRequest;
            return builder;
        }

        /// <summary>
        /// This event fires when the user clicks on the row, but before selecting it.
        /// Variables available in call:
        /// 'rowid': The id of the row. 
        /// 'e': The event object 
        /// This event should return boolean true or false. If the event returns true the selection 
        /// is done. If the event returns false the row is not selected and any other action if defined 
        /// does not occur.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onBeforeSelectRow">Script to be executed</param>
        public static GridBuilder OnBeforeSelectRow(this GridBuilder builder, string onBeforeSelectRow)
        {
            builder.CurrentGrid.OnBeforeSelectRow =onBeforeSelectRow;
            return builder;
        }

        /// <summary>
        /// This fires after all the data is loaded into the grid and all the other processes are complete. 
        /// Also the event fires independent from the datatype parameter and after sorting paging and etc.
        /// Variables available in call: None
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onGridComplete">Script to be executed</param>
        public static GridBuilder OnGridComplete(this GridBuilder builder, string onGridComplete)
        {
            builder.CurrentGrid.OnGridComplete =onGridComplete;
            return builder;
        }

        /// <summary>
        /// A pre-callback to modify the XMLHttpRequest object (xhr) before it is sent. Use this to set 
        /// custom headers etc. The XMLHttpRequest is passed as the only argument.
        /// Variables available in call:
        /// 'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onLoadBeforeSend">Script to be executed</param>
        public static GridBuilder OnLoadBeforeSend(this GridBuilder builder, string onLoadBeforeSend)
        {
            builder.CurrentGrid.OnLoadBeforeSend =onLoadBeforeSend;
            return builder;
        }

        /// <summary>
        /// This event is executed immediately after every server request. 
        /// Variables available in call:
        /// 'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onLoadComplete">Script to be executed</param>
        public static GridBuilder OnLoadComplete(this GridBuilder builder, string onLoadComplete)
        {
            builder.CurrentGrid.OnLoadComplete =onLoadComplete;
            return builder;
        }

        /// <summary>
        /// A function to be called if the request fails.
        ///  Variables available in call:
        ///  'xhr': The XMLHttpRequest
        ///  'status': String describing the type of error
        ///  'error': Optional exception object, if one occurred
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onLoadError">Script to be executed</param>
        public static GridBuilder OnLoadError(this GridBuilder builder, string onLoadError)
        {
            builder.CurrentGrid.OnLoadError =onLoadError;
            return builder;
        }

        /// <summary>
        /// Fires when we click on a particular cell in the grid.
        /// Variables available in call:
        /// 'rowid': The id of the row 
        /// 'iCol': The index of the cell,
        /// 'cellcontent': The content of the cell,
        /// 'e': The event object element where we click.
        /// (Note that this available when we are not using cell editing module 
        /// and is disabled when using cell editing).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onCellSelect">Script to be executed</param>
        public static GridBuilder OnCellSelect(this GridBuilder builder, string onCellSelect)
        {
            builder.CurrentGrid.OnCellSelect =onCellSelect;
            return builder;
        }

        /// <summary>
        /// Raised immediately after row was double clicked. 
        /// Variables available in call:
        /// 'rowid': The id of the row, 
        /// 'iRow': The index of the row (do not mix this with the rowid),
        /// 'iCol': The index of the cell. 
        /// 'e': The event object
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onDblClickRow">Script to be executed</param>
        public static GridBuilder OnDblClickRow(this GridBuilder builder, string onDblClickRow)
        {
            builder.CurrentGrid.OnDblClickRow =onDblClickRow;
            return builder;
        }

        /// <summary>
        /// Fires after clicking hide or show grid (hidegrid:true)
        /// Variables available in call:
        /// 'gridstate': The state of the grid - can have two values - visible or hidden
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onHeaderClick">Script to be executed</param>
        public static GridBuilder OnHeaderClick(this GridBuilder builder, string onHeaderClick)
        {
            builder.CurrentGrid.OnHeaderClick =onHeaderClick;
            return builder;
        }

        /// <summary>
        /// This event fires after click on [page button] and before populating the data. 
        /// Also works when the user enters a new page number in the page input box 
        /// (and presses [Enter]) and when the number of requested records is changed via 
        /// the select box.
        /// If this event returns 'stop' the processing is stopped and you can define your 
        /// own custom paging
        /// Variables available in call:
        /// 'pgButton': first,last,prev,next in case of button click, records in case when 
        /// a number of requested rows is changed and user when the user change the number of the requested page
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onPaging">Script to be executed</param>
        public static GridBuilder OnPaging(this GridBuilder builder, string onPaging)
        {
            builder.CurrentGrid.OnPaging =onPaging;
            return builder;
        }

        /// <summary>
        /// Raised immediately after row was right clicked. 
        /// Variables available in call:
        /// 'rowid': The id of the row, 
        /// 'iRow': The index of the row (do not mix this with the rowid),
        /// 'iCol': The index of the cell. 
        /// 'e': The event object
        /// Note - this event does not work in Opera browsers, since Opera does not support oncontextmenu event
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onRightClickRow">Script to be executed</param>
        public static GridBuilder OnRightClickRow(this GridBuilder builder, string onRightClickRow)
        {
            builder.CurrentGrid.OnRightClickRow =onRightClickRow;
            return builder;
        }

        /// <summary>
        /// This event fires when multiselect option is true and you click on the header checkbox. 
        /// Variables available in call:
        /// 'aRowids':  Array of the selected rows (rowid's). 
        /// 'status': Boolean variable determining the status of the header check box - true if checked, false if not checked. 
        /// Note that the aRowids alway contain the ids when header checkbox is checked or unchecked.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onSelectAll">Script to be executed</param>
        public static GridBuilder OnSelectAll(this GridBuilder builder, string onSelectAll)
        {
            builder.CurrentGrid.OnSelectAll =onSelectAll;
            return builder;
        }


        /// <summary>
        /// Raised immediately when row is clicked. 
        /// Variables available in function call:
        /// 'rowid': The id of the row,
        /// 'status': Tthe status of the selection. Can be used when multiselect is set to true. 
        /// true if the row is selected, false if the row is deselected.
        /// <param name="onSelectRow">Script to be executed</param>
        /// </summary>
        public static GridBuilder OnSelectRow(this GridBuilder builder, string onSelectRow)
        {
            builder.CurrentGrid.OnSelectRow =onSelectRow;
            return builder;
        }


        /// <summary>
        /// Raised immediately after sortable column was clicked and before sorting the data.
        /// Variables available in call:
        /// 'index': The index name from colModel
        /// 'iCol': The index of column, 
        /// 'sortorder': The new sorting order - can be 'asc' or 'desc'. 
        /// If this event returns 'stop' the sort processing is stopped and you can define your own custom sorting
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onSortCol">Script to be executed</param>
        public static GridBuilder OnSortCol(this GridBuilder builder, string onSortCol)
        {
            builder.CurrentGrid.OnSortCol =onSortCol;
            return builder;
        }

        /// <summary>
        /// Event which is called when we start resizing a column. 
        /// Variables available in call:
        /// 'event':  The event object
        /// 'index': The index of the column in colModel.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onResizeStart">Script to be executed</param>
        public static GridBuilder OnResizeStart(this GridBuilder builder, string onResizeStart)
        {
            builder.CurrentGrid.OnResizeStart =onResizeStart;
            return builder;
        }

        /// <summary>
        /// Event which is called after the column is resized.
        /// Variables available in call:
        /// 'newwidth': The new width of the column
        /// 'index': The index of the column in colModel.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onResizeStop">Script to be executed</param>
        public static GridBuilder OnResizeStop(this GridBuilder builder, string onResizeStop)
        {
            builder.CurrentGrid.OnResizeStop =onResizeStop;
            return builder;
        }

        /// <summary>
        /// If this event is set it can serialize the data passed to the ajax request. 
        /// The function should return the serialized data. This event can be used when 
        /// custom data should be passed to the server - e.g - JSON string, XML string and etc. 
        /// Variables available in call:
        /// 'postData': Posted data
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="onSerializeGridData">Script to be executed</param>
        public static GridBuilder OnSerializeGridData(this GridBuilder builder, string onSerializeGridData)
        {
            builder.CurrentGrid.OnSerializeGridData =onSerializeGridData;
            return builder;
        }

        public static GridBuilder AddChainMethod(this GridBuilder builder, string method)
        {
            builder.CurrentGrid.ChainMethods.Add(method);

            return builder;
        }

        public static GridBuilder PostData(this GridBuilder builder, object data)
        {
            builder.CurrentGrid.PostData = data;

            return builder;
        }

        public static GridBuilder LocalData(this GridBuilder builder, object data)
        {
            builder.CurrentGrid.LocalData = data;

            return builder;
        }
    }
}