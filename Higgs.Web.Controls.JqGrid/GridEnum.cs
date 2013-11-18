// Original source code from http://github.com/robinvanderknaap/Mvcjqgrid

// TODO: Change all value to valid pattern and create some attribute to define how to generate as JavaScript
namespace Higgs.Web.Controls.JqGrid
{
// ReSharper disable InconsistentNaming
    public enum GridSortOrder
    {
        asc,
        desc
    }

    public enum GridRequestType
    {
        get,
        post
    }

    public enum GridDataType
    {
        json,
        xml
    }

    public enum GridLoadUi
    {
        enable,
        disable,
        block
    }

    public enum GridMultiKey
    {
        altKey,
        ctrlKey,
        shiftKey
    }

    public enum GridPagerPos
    {
        center,
        left,
        right
    }

    public enum GridRecordPos
    {
        center,
        left,
        right
    }

    public enum GridAlign
    {
        center,
        left,
        right
    }

    public enum GridFormatters
    {
        integer,
        number,
        currency,
        date,
        email,
        link,
        showlink,
        checkbox,
        select
    }

    public enum GridToolbarPosition
    {
        top,
        bottom,
        both
    }

    public enum GridDirection
    {
        vertical,
        horizontal
    }

    public enum GridSearchType
    {
        text,
        select,
        datepicker
    }

    public enum GridButtonPosition
    {
        first,
        last
    }
// ReSharper restore InconsistentNaming
}