using System.Text;

namespace Higgs.Web.Controls.JqGrid.Models
{
    public class CustomButton
    {
        public bool IsSeparator { get; set; }
        public string ContainerId;
        public string Id;
        public string Caption;
        public string Title;
        public string ButtonIcon = "";
        public string Cursor;
        public GridButtonPosition? Position;

        public CustomButton(string containerId)
        {
            ContainerId = containerId;
        }

        public string OnClickButton;

        public override string ToString()
        {
            var script = new StringBuilder();

            if (!IsSeparator)
            {
                script.AppendFormat(".navButtonAdd('#{0}', ", ContainerId).AppendLine();
                script.AppendLine("{");

                if (!string.IsNullOrWhiteSpace(Id)) script.AppendFormat("id: '{0}',", Id).AppendLine();
                if (!string.IsNullOrWhiteSpace(Caption)) script.AppendFormat("caption: '{0}',", Caption).AppendLine();
                if (ButtonIcon != null) script.AppendFormat("buttonicon: '{0}',", ButtonIcon).AppendLine();
                if (Position.HasValue) script.AppendFormat("position: '{0}',", Position).AppendLine();
                if (!string.IsNullOrWhiteSpace(Title)) script.AppendFormat("title: '{0}',", Title).AppendLine();
                if (!string.IsNullOrWhiteSpace(Cursor)) script.AppendFormat("cursor: '{0}',", Cursor).AppendLine();
                if (!string.IsNullOrWhiteSpace(OnClickButton))
                    script.AppendFormat("onClickButton: {0},", OnClickButton).AppendLine();

                script = script.Remove(script.Length - 3, 3);
                script.AppendLine().Append("})");
            }
            else
            {
                script.AppendFormat(".navSeparatorAdd('#{0}', ", ContainerId);
                script.Append("{ ");

                if (!string.IsNullOrWhiteSpace(ButtonIcon)) script.AppendFormat("sepclass: '{0}',", ButtonIcon);
                if (Caption != null) script.AppendFormat("sepcontent: '{0}',", Caption);

                script = script.Remove(script.Length - 1, 1);
                script.AppendLine().Append("})");
            }

            return script.ToString();
        }
    }
}