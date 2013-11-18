using System.Web.Mvc;

namespace Higgs.Web.Controls
{
    public abstract class InputControlBase : ControlBase
    {
        protected InputControlBase(HtmlHelper helper) : base(helper)
        {
            IsLabelBeforeControl = true;
            IsSupportLabel = true;
        }

        public static string CssReadonly = "readonly";

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if(OnNameChange != null)
                {
                    OnNameChange(value);
                }
            }
        }

        public TagBuilder MainControl { get; set; }
        public bool IsLabelBeforeControl { get; internal set; }
        public bool IsSupportLabel { get; set; }
        public bool HasLabel { get; internal set; }
        public delegate void NameChangeEventHandler(string controlName);

        public event NameChangeEventHandler OnNameChange;
        
        internal virtual void AddLabel(string title, string className = null)
        {
            var label = new HiggsLabel(Helper, this, title);
            HasLabel = true;

            if(className != null) label.AddCssClass(className);

            if (IsLabelBeforeControl)
                ListTags.Insert(0, label);
            else
                ListTags.Add(label);
        }

        internal virtual void AutoGenerateIdFromBindingData(string modelName, string bindedPropertyName)
        {
            Id = string.Format("{0}_{1}", modelName, bindedPropertyName);

            MainControl.Attributes["id"] = Id;
            MainControl.Attributes["name"] = bindedPropertyName;
        }

        public new void AddCssClass(string className)
        {
            MainControl.AddCssClass(className);
        }
    }
}
