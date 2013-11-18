using Higgs.Web.Controls.JqGrid.Builders;

namespace Higgs.Web.Controls.JqGrid
{
    public static class CustomButtonBuilderHelper
    {
        public static CustomButtonBuilder NavButton(this GridBuilder builder, string caption, string buttonIcon = null)
        {
            var buttonBuilder = new CustomButtonBuilder(builder.CurrentGrid)
            {
                Current = {Caption = caption}
            };

            if (!string.IsNullOrWhiteSpace(buttonIcon))
            {
                buttonBuilder.Current.ButtonIcon = buttonIcon;
            }

            return buttonBuilder;
        }

        public static GridBuilder ButtonSeparator(this GridBuilder builder, string caption = null, string sepClass = null)
        {
            var buttonBuilder = new CustomButtonBuilder(builder.CurrentGrid)
            {
                Current = {IsSeparator = true}
            };

            if (!string.IsNullOrWhiteSpace(sepClass))
            {
                buttonBuilder.Current.ButtonIcon = sepClass;
            }
            if (caption != null)
            {
                buttonBuilder.Current.Caption = caption;
            }

            return builder;
        }

        public static CustomButtonBuilder ButtonId(this CustomButtonBuilder builder, string id)
        {
            builder.Current.Id = id;

            return builder;
        }

        public static CustomButtonBuilder ButtonCaption(this CustomButtonBuilder builder, string caption)
        {
            builder.Current.Caption = caption;

            return builder;
        }

        public static CustomButtonBuilder ButtonIcon(this CustomButtonBuilder builder, string buttonIcon)
        {
            builder.Current.ButtonIcon = buttonIcon;

            return builder;
        }

        public static CustomButtonBuilder ButtonTitle(this CustomButtonBuilder builder, string title)
        {
            builder.Current.Title = title;

            return builder;
        }

        public static CustomButtonBuilder ButtonPostion(this CustomButtonBuilder builder, GridButtonPosition position)
        {
            builder.Current.Position = position;

            return builder;
        }

        public static CustomButtonBuilder ButtonCursor(this CustomButtonBuilder builder, string cursor)
        {
            builder.Current.Cursor = cursor;

            return builder;
        }

        public static CustomButtonBuilder OnClickButton(this CustomButtonBuilder builder, string onClickButton)
        {
            builder.Current.OnClickButton = onClickButton;

            return builder;
        }
    }
}