using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Higgs.Web.Controls.JqGrid.Builders;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid
{
    public static class ColumnBuilderHelper
    {
        // Make sure columnname is not part of the reserved names collection
        static readonly string[] ReservedNames = new[] { "subgrid", "cb", "rn" };

        public static ColumnBuilder Column(this GridBuilder builder, string columnName, bool isHidden)
        {
            // Make sure columnname is not left blank
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("No columnname specified");
            }

            if (ReservedNames.Contains(columnName))
            {
                throw new ArgumentException("Columnname '" + columnName + "' is reserved");
            }

            var columnBuilder = new ColumnBuilder(builder.CurrentGrid)
            {
                CurrentColumn =
                {
                    ColumnName = columnName,
                    Index = columnName,
                    Label = "",
                    Hidden = isHidden
                }
            };
            
            columnBuilder.Editable(!isHidden);

            return columnBuilder;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="columnName">Name of column, cannot be blank or set to 'subgrid', 'cb', and 'rn'</param>
        /// <param name="sortName"></param>
        /// <param name="isHidden"></param>
        public static ColumnBuilder Column(this GridBuilder builder, string columnName, string sortName, bool isHidden)
        {
            // Make sure columnname is not left blank
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("No columnname specified");
            }

            // Make sure columnname is not part of the reserved names collection
            var reservedNames = new [] { "subgrid", "cb", "rn" };

            if (reservedNames.Contains(columnName))
            {
                throw new ArgumentException("Columnname '" + columnName + "' is reserved");
            }

            var columnBuilder = new ColumnBuilder(builder.CurrentGrid)
            {
                CurrentColumn =
                {
                    ColumnName = columnName,
                    Index = sortName,
                    Label = "",
                    Hidden = isHidden
                }
            };

            columnBuilder.Editable(!isHidden);

            return columnBuilder;
        }

        public static ColumnBuilder Column(this GridBuilder builder, string columnName, string columnLabel = null)
        {
            return builder.Column(columnName, columnName, false)
                                .Label(columnLabel);
        }

        public static ColumnBuilder Column(this GridBuilder builder, string columnName, string columnLabel, int width)
        {
            return builder.Column(columnName, columnLabel)
                                .Width(width);
        }

        public static ColumnBuilder Column(this GridBuilder builder, string columnName, int width, GridAlign? align = null)
        {
            var columnBuilder = builder.Column(columnName)
                                                     .Width(width);

            if(align.HasValue)
            {
                columnBuilder.Align(align.Value);
            }

            return columnBuilder;
        }

        public static GridBuilder Column(this GridBuilder builder, IColumn customColumn)
        {
            builder.CurrentGrid.Columns.Add(customColumn);

            return builder;
        }

        /// <summary>
        /// This option allow to add a class to to every cell on that column. In the grid css 
        /// there is a predefined class ui-ellipsis which allow to attach ellipsis to a 
        /// particular row. Also this will work in FireFox too.
        /// Multiple calls to this function are allowed to set multiple classes
        /// </summary> <param name="builder">Column Builder</param>
        /// <param name="className">Classname</param>
        public static ColumnBuilder Class(this ColumnBuilder builder, string className)
        {
            builder.CurrentColumn.Classes.Add(className);
            return builder;
        }

        /// <summary>
        /// Set dateformat of datepicker when searchtype is set to datepicker (default: dd-mm-yy)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="searchDateFormat">Dateformat</param>
        public static ColumnBuilder SearchDateFormat(this ColumnBuilder builder, string searchDateFormat)
        {
            builder.CurrentColumn.SearchDateFormat = searchDateFormat;
            return builder;
        }

        /// <summary>
        /// Set searchterms if search type of this column is set to type select
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="searchTerms">Searchterm to add to dropdownlist</param>
        public static ColumnBuilder SearchTerms(this ColumnBuilder builder, string[] searchTerms)
        {
            builder.CurrentColumn.SearchTerms = searchTerms;
            return builder;
        }

        /// <summary>
        /// Defines the alignment of the cell in the Body layer, not in header cell. 
        /// Possible values: left, center, right. (default: left)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="align">Alignment of column (center, right, left</param>
        public static ColumnBuilder Align(this ColumnBuilder builder, GridAlign align)
        {
            builder.CurrentColumn.Align = align;
            return builder;
        }

        /// <summary>
        /// If set to asc or desc, the column will be sorted in that direction on first 
        /// sort.Subsequent sorts of the column will toggle as usual (default: null)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="firstSortOrder">First sort order</param>
        public static ColumnBuilder FirstSortOrder(this ColumnBuilder builder, GridSortOrder firstSortOrder)
        {
            builder.CurrentColumn.FirstSortOrder = firstSortOrder;
            return builder;
        }

        /// <summary>
        /// If set to true this option does not allow recalculation of the width of the 
        /// column if shrinkToFit option is set to true. Also the width does not change 
        /// if a setGridWidth method is used to change the grid width. (default: false)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="fixedWidth">Indicates if width of column is fixed</param>
        public static ColumnBuilder Fixed(this ColumnBuilder builder, bool fixedWidth)
        {
            builder.CurrentColumn.FixedWidth = fixedWidth;
            return builder;
        }

        /// <summary>
        /// Sets formatter with default formatoptions (as set in language file)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="formatter">Formatter</param>
        public static ColumnBuilder Formatter(this ColumnBuilder builder, GridFormatters formatter)
        {
            if (!string.IsNullOrWhiteSpace(builder.CurrentColumn.CustomFormatter))
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            builder.CurrentColumn.Formatter = new KeyValuePair<GridFormatters, string>(formatter, "");
            return builder;
        }

        public static ColumnBuilder Unformatter(this ColumnBuilder builder, string unformatter)
        {
            builder.CurrentColumn.Unformatter = unformatter;
            return builder;
        }

        /// <summary>
        /// Sets formatter with formatoptions (see jqGrid documentation for more info on formatoptions)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="formatter">Formatter</param>
        /// <param name="formatOptions">Formatoptions</param>
        public static ColumnBuilder Formatter(this ColumnBuilder builder, GridFormatters formatter, string formatOptions)
        {
            if (!string.IsNullOrWhiteSpace(builder.CurrentColumn.CustomFormatter))
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            builder.CurrentColumn.Formatter = new KeyValuePair<GridFormatters, string>(formatter, formatOptions);
            return builder;
        }

        /// <summary>
        /// Sets custom formatter. Usually this is a function. When set in the formatter option 
        /// this should not be enclosed in quotes and not entered with () - 
        /// just specify the name of the function
        /// The following variables are passed to the function:
        /// 'cellvalue': The value to be formated (pure text).
        /// 'options': Object { rowId: rid, colModel: cm} where rowId - is the id of the row colModel is 
        /// the object of the properties for this column getted from colModel array of jqGrid
        /// 'rowobject': Row data represented in the format determined from datatype option. 
        /// If we have datatype: xml/xmlstring - the rowObject is xml node,provided according to the rules 
        /// from xmlReader If we have datatype: json/jsonstring - the rowObject is array, provided according to 
        /// the rules from jsonReader
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="customFormatter"></param>
        /// <returns></returns>
        public static ColumnBuilder CustomFormatter(this ColumnBuilder builder, string customFormatter)
        {
            if (builder.CurrentColumn.Formatter.HasValue)
            {
                throw new Exception("You cannot set a formatter and a customformatter at the same time, please choose one.");
            }
            builder.CurrentColumn.CustomFormatter = customFormatter;
            return builder;
        }

        /// <summary>
        /// Defines if this column is hidden at initialization. (default: false)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="hidden">Boolean indicating if column is hidden</param>
        public static ColumnBuilder Hidden(this ColumnBuilder builder, bool hidden)
        {
            builder.CurrentColumn.Hidden = hidden;
            builder.Editable(!hidden);

            return builder;
        }

        /// <summary>
        /// Set the index name when sorting. Passed as sidx parameter. (default: Same as columnname)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="index">Name of index</param>
        public static ColumnBuilder Index(this ColumnBuilder builder, string index)
        {
            builder.CurrentColumn.Index = index;
            return builder;
        }

        /// <summary>
        /// In case if there is no id from server, this can be set as as id for the unique row id. 
        /// Only one column can have this property. If there are more than one key the grid finds 
        /// the first one and the second is ignored. (default: false)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="key">Indicates if key is set</param>
        public static ColumnBuilder Key(this ColumnBuilder builder, bool key)
        {
            builder.CurrentColumn.Key = key;
            builder.CurrentColumn.Editable = false;
            return builder;
        }

        /// <summary>
        /// Defines the heading for this column. If empty, the heading for this column comes from the name property.
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="label">Label name of column</param>
        public static ColumnBuilder Label(this ColumnBuilder builder, string label)
        {
            builder.CurrentColumn.Label = label;
            return builder;
        }

        /// <summary>
        /// Defines if the column can be resized (default: true)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="resizeable">Indicates if the column is resizable</param>
        public static ColumnBuilder Resizeable(this ColumnBuilder builder, bool resizeable = true)
        {
            builder.CurrentColumn.Resizeable = resizeable;
            return builder;
        }

        /// <summary>
        /// When used in search modules, disables or enables searching on that column. (default: true)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="search">Indicates if searching for this column is enabled</param>
        public static ColumnBuilder Search(this ColumnBuilder builder, bool search = true)
        {
            builder.CurrentColumn.Search = search;
            return builder;
        }

        /// <summary>
        /// Sets the searchtype of this column (text, select or datepicker) (default: text)
        /// Note: To use datepicker jQueryUI javascript should be included
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="searchType">Search type</param>
        public static ColumnBuilder SearchType(this ColumnBuilder builder, GridSearchType searchType)
        {
            builder.CurrentColumn.SearchType = searchType;
            return builder;
        }

        /// <summary>
        /// Indicates if column is sortable (default: true)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="sortable">Indicates if column is sortable</param>
        public static ColumnBuilder Sortable(this ColumnBuilder builder, bool sortable)
        {
            builder.CurrentColumn.Sortable = sortable;
            return builder;
        }

        /// <summary>
        /// If this option is false the title is not displayed in that column when we hover over a cell (default: true)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="title">Indicates if title is displayed when hovering over cell</param>
        public static ColumnBuilder Title(this ColumnBuilder builder, bool title)
        {
            builder.CurrentColumn.Title = title;
            return builder;
        }

        /// <summary>
        /// Set the initial width of the column, in pixels. This value currently can not be set as percentage (default: 150)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="width">Width in pixels</param>
        public static ColumnBuilder Width(this ColumnBuilder builder, int width)
        {
            builder.CurrentColumn.Width = width;
            return builder;
        }

        /// <summary>
        /// Set the initial width of the column, in pixels. This value currently can not be set as percentage (default: 150)
        /// </summary>
        /// <param name="builder">Column Builder</param>
        /// <param name="flag"></param>
        public static ColumnBuilder Editable(this ColumnBuilder builder, bool flag)
        {
            builder.CurrentColumn.Editable = flag;
            return builder;
        }

        public static ColumnBuilder EditOption(this ColumnBuilder builder, string key, string value)
        {
            builder.CurrentColumn.EditOptions.Add(key, value);
            return builder;
        }

        public static ColumnBuilder EditRule(this ColumnBuilder builder, string key, string value)
        {
            builder.CurrentColumn.EditRules.Add(key, value);
            return builder;
        }

        public static ColumnBuilder Required(this ColumnBuilder builder, bool isRequired = true)
        {
            builder.CurrentColumn.EditRules.Add("required", isRequired.ToString().ToLower());
            return builder;
        }

        public static ColumnBuilder EditRuleCustom(this ColumnBuilder builder, string customFunction)
        {
            builder.CurrentColumn.EditRules.Add("custom", "true");
            builder.CurrentColumn.EditRules.Add("custom_func", customFunction);

            return builder;
        }

        public static ColumnBuilder EditTypePassword(this ColumnBuilder builder, int? maxLength = null)
        {
            builder.CurrentColumn.EditType = "password";

            if (maxLength.HasValue)
            {
                builder.CurrentColumn.EditOptions.Add("maxlength", "8");
            }

            return builder;
        }

        public static ColumnBuilder EditTypeNumber(this ColumnBuilder builder, int? minValue = null, int? maxValue = null)
        {
            builder.CurrentColumn.EditRules.Add("number", "true");

            if (minValue != null)
            {
                builder.CurrentColumn.EditRules.Add("minValue", minValue.Value.ToString());
            }
            if (maxValue != null)
            {
                builder.CurrentColumn.EditRules.Add("maxValue", maxValue.Value.ToString());
            }

            return builder;
        }

        public static ColumnBuilder EditTypeInteger(this ColumnBuilder builder, int? minValue = null, int? maxValue = null)
        {
            builder.CurrentColumn.EditRules.Add("integer", "true");

            if (minValue != null)
            {
                builder.CurrentColumn.EditRules.Add("minValue", minValue.Value.ToString());
            }
            if (maxValue != null)
            {
                builder.CurrentColumn.EditRules.Add("maxValue", maxValue.Value.ToString());
            }

            return builder;
        }

        public static ColumnBuilder EditTypeEmail(this ColumnBuilder builder)
        {
            builder.CurrentColumn.EditRules.Add("email", "true");

            return builder;
        }

        public static ColumnBuilder EditTypeUrl(this ColumnBuilder builder)
        {
            builder.CurrentColumn.EditRules.Add("email", "true");

            return builder;
        }

        public static ColumnBuilder EditTypeDate(this ColumnBuilder builder)
        {
            builder.CurrentColumn.EditRules.Add("date", "true");

            return builder;
        }

        public static ColumnBuilder EditTypeTime(this ColumnBuilder builder)
        {
            builder.CurrentColumn.EditRules.Add("time", "true");

            return builder;
        }

        public static ColumnBuilder EditTypeSelect(this ColumnBuilder builder, SelectList list)
        {
            builder.CurrentColumn.EditType = "select";

            var optionValues = new StringBuilder();
            optionValues.Append("{ ");
            foreach (var l in list)
            {
                optionValues.AppendFormat("'{0}': '{1}', ", l.Value, l.Text);
            }
            optionValues = optionValues.Remove(optionValues.Length - 2, 2);
            optionValues.Append(" }");

            builder.EditOption("value", optionValues.ToString());

            return builder;
        }
        
        public static ColumnBuilder EditTypeCustom(this ColumnBuilder builder, string typeName)
        {
            builder.CurrentColumn.EditType = typeName;

            return builder;
        }

        public static ColumnBuilder EditTypeCustom(this ColumnBuilder builder, string customElementFn, string customValueFn)
        {
            builder.CurrentColumn.EditType = "custom";
            builder.CurrentColumn.EditOptions.Add("custom_element", customElementFn);
            builder.CurrentColumn.EditOptions.Add("custom_value", customValueFn);

            return builder;
        }

        public static ColumnBuilder SortName(this ColumnBuilder builder, string sortName)
        {
            builder.CurrentColumn.Index = sortName;

            return builder;
        }
    }
}