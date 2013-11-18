using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HtmlBuilder : TagBuilder
    {
        public static List<string> MustSelfClosingTag = new List<string>
        {
            "input",
            "img"
        };

        public HtmlBuilder(string tagName) : base(tagName)
        {
            ChildControls = new List<HtmlBuilder>();
        }

        public HtmlBuilder(string tagName, string cssClassName, string cssStyle = "") : base(tagName)
        {
            ChildControls = new List<HtmlBuilder>();
            
            foreach (var cssClass in cssClassName.Split(' ').Where(cssClass => !string.IsNullOrWhiteSpace(cssClass)))
            {
                AddCssClass(cssClass);
            }

            var temp = cssStyle.Split(':');
            if (!string.IsNullOrWhiteSpace(cssStyle) && temp.Length == 2)
                Attributes.Add(temp[0], temp[1]);
        }

        public List<HtmlBuilder> ChildControls { get; set; }

        public override string ToString()
        {
            return ToString(TagRenderMode.Normal);
        }

        public new string ToString(TagRenderMode renderMode)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(InnerHtml))
            {
                foreach (var tb in ChildControls)
                {
                    sb.Append(tb.ToString(TagRenderMode.Normal));
                }

                InnerHtml = sb.ToString();
            }

            if(MustSelfClosingTag.Contains(TagName))
            {
                return renderMode == TagRenderMode.EndTag ? "" : base.ToString(TagRenderMode.SelfClosing);
            }

            return base.ToString(renderMode);
        }
    }
}
