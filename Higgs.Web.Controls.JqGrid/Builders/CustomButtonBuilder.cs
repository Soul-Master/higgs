using System;
using Higgs.Web.Controls.JqGrid.Models;

namespace Higgs.Web.Controls.JqGrid.Builders
{
    public class CustomButtonBuilder : GridBuilder
    {
        public Grid Builder;
        public CustomButton Current;
        public static Func<CustomButton, CustomButton> DefaultOptions = x =>
        {
            x.Position = GridButtonPosition.last;

            return x;
        };

        public CustomButtonBuilder(Grid g) : base(g)
        {
            Builder = g;
            Current = DefaultOptions(new CustomButton(g.PagerId));

            g.CustomButtons.Add(Current);
        }
    }
}
