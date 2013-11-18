using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public class HiggsLabel : ControlBase
    {
        public HiggsLabel(HtmlHelper helper, string inputId, string text) : base(helper, "label")
        {
            ForInputId = inputId;
            Text = text;
        }

        public HiggsLabel(HtmlHelper helper, ControlBase input, string text) : base(helper, "label")
        {
            ForInputId = input.Id;
            Text = text;
            input.OnIdChange += MainInput_OnChangeId;
        }

        public string ForInputId
        {
            get
            {
                if (!Attributes.ContainsKey("for")) return null;

                return Attributes["for"];
            }
            set
            {
                Attributes["for"] = value;
            }
        }
        public string Text
        {
            get { return InnerHtml; }
            set { InnerHtml = value; }
        }

        public void MainInput_OnChangeId(string newId)
        {
            ForInputId = newId;
        }
    }
}
