using System;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid.Builders
{
    public class ColumnBuilder : GridBuilder
    {
        public Grid Builder;
        public Column CurrentColumn;
        public static Func<Column, Column> DefaultOptions = x =>
        {
            x.Title = false;
            x.Editable = true;

            return x;
        };

        public ColumnBuilder(Grid g) : base(g)
        {
            Builder = g;
            CurrentColumn = DefaultOptions(new Column());

            g.Columns.Add(CurrentColumn);
        }
    }
}
