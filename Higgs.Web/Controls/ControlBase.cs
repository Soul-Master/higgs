using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Higgs.Web.Controls
{
    public delegate void ControlChangeIdEventHandler(string id);

    public abstract class ControlBase : TagBuilder, IHtmlString
    {
        #region Css Class
        
        // TODO: Remove or move to config file.
        public static string CssFocus = "focus";
        public static string CssHasMenu = "has-menu";
        public static string CssWaterMark = "watermark";

        #endregion

        #region Resource Path

        // TODO: Remove or move to config file.
        public static string DropDownIconPath = "~/Photoes/DropDownIcon.png";

        #endregion

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value == null) return;

                _id = value;

                if(OnIdChange != null)
                    OnIdChange(value);
            }
        }
        public virtual bool IsVisible { get; set; }
        public List<TagBuilder> ListTags = new List<TagBuilder>();
        internal HtmlHelper Helper;
        public event ControlChangeIdEventHandler OnIdChange;
        public virtual string ScriptControlId { get { return Id; } }
        internal bool IsSingleTag;
        internal bool IsSettedTabIndex;

        protected ControlBase(HtmlHelper helper) : base("baseControl")
        {
            Helper = helper;
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            IsVisible = true;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected ControlBase(HtmlHelper helper, string tagName) : base(tagName)
        {
            IsSingleTag = true;
            Helper = helper;
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            IsVisible = true;
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public override string ToString()
        {
            if (!IsSettedTabIndex) this.TabIndex(Helper.GetNextAvailableTabIndex());
            if (IsSingleTag) return base.ToString();
            if (!IsVisible) return "";

            var sb = new StringBuilder();
            foreach (var tb in ListTags)
            {
                sb.Append(tb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// For support Razor view engine.
        /// </summary>
        /// <returns>HTML Unencoded</returns>
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}
