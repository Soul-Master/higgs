using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Higgs.Web.Controls.JqGrid.Models
{
    public class Column : IColumn
    {
        public GridAlign? Align;
        public List<string> Classes = new List<string>();
        public string ColumnName;
        public GridSortOrder? FirstSortOrder;
        public bool? FixedWidth;
        public KeyValuePair<GridFormatters, string>? Formatter;
        public string Unformatter;
        public string CustomFormatter;
        public string Index;
        public bool? Hidden;
        public bool? Key;
        public string Label;
        public bool? Resizeable;
        public bool? Search;
        public GridSearchType? SearchType;
        public string[] SearchTerms;
        public string SearchDateFormat;
        public bool? Sortable;
        public bool? Title;
        public int? Width;
        public bool? Editable;
        public string EditType;
        public Dictionary<string, string> EditOptions = new Dictionary<string, string>();
        public Dictionary<string, string> EditRules = new Dictionary<string, string>();

        /// <summary>
        /// Creates javascript string from column to be included in grid javascript
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var script = new StringBuilder();

            // Start column
            script.Append("{").AppendLine();

            // Align
            if (Align.HasValue) script.AppendFormat("align: '{0}',", Align).AppendLine();

            // Classes
            if (Classes.Count > 0) script.AppendFormat("classes: '{0}',", string.Join(" ", (from c in Classes select c).ToArray())).AppendLine();

            // Columnname
            script.AppendFormat("name:'{0}',", ColumnName).AppendLine();

            // FirstSortOrder
            if (FirstSortOrder.HasValue) script.AppendFormat("firstsortorder: '{0}',", FirstSortOrder).AppendLine();

            // FixedWidth
            if (FixedWidth.HasValue) script.AppendFormat("fixed: {0},", FixedWidth.Value.ToString().ToLower()).AppendLine();

            // Formatters
            if (Formatter.HasValue && string.IsNullOrWhiteSpace(Formatter.Value.Value)) script.AppendFormat("formatter: '{0}',", Formatter.Value.Key).AppendLine();
            if (Formatter.HasValue && !string.IsNullOrWhiteSpace(Formatter.Value.Value)) script.AppendLine("formatter: '" + Formatter.Value.Key + "', formatoption: {" + Formatter.Value.Value + "} ,");
            
            // Custom formatter
            if (!string.IsNullOrWhiteSpace(CustomFormatter)) script.AppendFormat("formatter: {0},", CustomFormatter).AppendLine();

            // Unformmater
            if (!string.IsNullOrWhiteSpace(Unformatter)) script.AppendFormat("unformat: {0},", Unformatter).AppendLine();

            // Hidden
            if (Hidden.HasValue) script.AppendFormat("hidden: {0},", Hidden.Value.ToString().ToLower()).AppendLine();

            // Key
            if (Key.HasValue) script.AppendFormat("key: {0},", Key.Value.ToString().ToLower()).AppendLine();

            // Label
            if (!string.IsNullOrEmpty(Label)) script.AppendFormat("label: '{0}',", Label).AppendLine();

            // Resizable
            if (Resizeable.HasValue) script.AppendFormat("resizable: {0},", Resizeable.Value.ToString().ToLower()).AppendLine();

            // Search
            if (Search.HasValue) script.AppendFormat("search: {0},", Search.Value.ToString().ToLower()).AppendLine();

            // SearchType
            if (SearchType.HasValue)
            {
                if (SearchType.Value == GridSearchType.text) script.AppendLine("stype:'text',");
                if (SearchType.Value == GridSearchType.select) script.AppendLine("stype:'select',");

            }

            // Searchoptions
            if (SearchType == GridSearchType.select || SearchType == GridSearchType.datepicker)
            {
                script.Append("searchoptions: {");

                // Searchtype select
                if (SearchType == GridSearchType.select)
                {
                    if (SearchTerms != null)
                    {
                        var emtpyOption = (SearchTerms.Any()) ? ":;" : ":";
                        script.AppendFormat("value: \"{0}{1}\"", emtpyOption, string.Join(";", from s in SearchTerms select s + ":" + s));
                    }
                    else
                    {
                        script.Append("value: ':'");
                    }
                }

                // Searchtype datepicker
                if (SearchType == GridSearchType.datepicker)
                {
                    if (string.IsNullOrWhiteSpace(SearchDateFormat))
                        script.Append("dataInit:function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'dd-mm-yy'});}");
                    else
                        script.Append("dataInit:function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'" + SearchDateFormat + "'});}");
                }
                script.AppendLine("},");
            }

            // Sortable
            if (Sortable.HasValue) script.AppendFormat("sortable: {0},", Sortable.Value.ToString().ToLower()).AppendLine();

            // Title
            if (Title.HasValue) script.AppendFormat("title: {0},", Title.Value.ToString().ToLower()).AppendLine();

            // Width
            if (Width.HasValue) script.AppendFormat("width:{0},", Width.Value).AppendLine();

            // Editable
            if (Editable.HasValue) script.AppendFormat("editable:{0},", Editable.Value.ToString().ToLower()).AppendLine();

            // Edit Type
            if (!string.IsNullOrWhiteSpace(EditType)) script.AppendFormat("edittype: '{0}',", EditType.ToLower()).AppendLine();

            // Edit Options
            if (Editable.HasValue && Editable.Value && EditOptions.Keys.Count > 0)
            {
                script.Append("editoptions: {");

                foreach (var option in EditOptions)
                {
                    script.AppendFormat("{0}: {1}, ", option.Key, option.Value);
                }
                script = script.Remove(script.Length - 2, 2);

                script.Append("},").AppendLine();
            }

            // Edit Rule Options
            if (Editable.HasValue && Editable.Value && EditRules.Keys.Count > 0)
            {
                script.Append("editrules: {");

                foreach (var option in EditRules)
                {
                    script.AppendFormat("{0}: {1}, ", option.Key, option.Value);
                }
                script = script.Remove(script.Length - 2, 2);

                script.Append("},").AppendLine();
            }

            // Index
            script.AppendFormat("index:'{0}'", Index).AppendLine();
            
            // End column
            script.Append("}");

            return script.ToString();
        }
    }
}