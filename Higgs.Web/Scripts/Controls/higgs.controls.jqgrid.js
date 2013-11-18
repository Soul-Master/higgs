/// <reference path="../../jquery-1.7.1-vsdoc.js" />
/// <reference path="../higgs.core.js" />
        
(function ()
{
    var jqGrid = {};
    jqGrid.exportExcel = function ( grid, exportHiddenCol, extraOptions )
    {
        if(grid instanceof $.Event)
        {
            // This function is binded with event handler.
            var event = grid;
            grid = $(event.target)
                            .parents('div.ui-jqgrid')
                            .find('.ui-jqgrid-btable');
            
            grid = grid[0];
        }
        
        var url = grid.p.url;
        var postData = $.extend(true, extraOptions || {}, grid.p.postData);

        if ( typeof url === 'function' ) url = url();

        var count = 0;
        $.each( grid.p.colModel, function ( key, col )
        {
            if ( !exportHiddenCol && (col.hidden || col.hidedlg) ) return;

            postData["colModel[" + count + "].name"] = ( col.name === null || col.name === "" ) ? '' : col.name;
            postData["colModel[" + count + "].title"] = col.label && (col.label + '').trim() ? col.label : col.name;
            
            count++;
        } );
        postData._export = true;

        $.sendData( url, postData );
    };
    
    $.higgs.controls.jqGrid = jqGrid;
})();