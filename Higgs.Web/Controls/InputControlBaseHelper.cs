using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Higgs.Web.Helpers;

namespace Higgs.Web.Controls
{
    public static class InputControlBaseHelper
    {
        public static TControl Name<TControl>(this TControl control, string controlName, bool displayLabel = true)
            where TControl : InputControlBase
        {
            control.Name = controlName;

            if (displayLabel && typeof(TControl) != typeof(HiggsHiddenField))
            {
                control.AddLabel(controlName);
            }

            return control;
        }

        public static TControl Name<TControl>(this TControl control, string controlName, string labelClassName)
            where TControl : InputControlBase
        {
            control.Name = controlName;

            if (typeof(TControl) != typeof(HiggsHiddenField))
            {
                control.AddLabel(controlName, labelClassName);
            }

            return control;
        }

        public static TControl Name<TControl>(this TControl control, Expression<Func<object>> exp, bool displayLabel = true)
            where TControl : InputControlBase
        {
            var label = exp.Compile().Invoke().ToString();

            if (control.IsSupportLabel && displayLabel)
            {
                control.AddLabel(label);
            }

            if (exp.Body is MemberExpression)
            {
                var me = (MemberExpression)exp.Body;

                if (me.Member.DeclaringType == null) throw new Exception("Unable to find Declaring Type of");

                control.Name = string.Format("{0}.{1}", me.Member.DeclaringType.Name, me.Member.Name);

                var viewName = me.Member.DeclaringType.Name;
                var namespaceName = me.Member.DeclaringType.Namespace;
                if (namespaceName == null) throw new NullReferenceException();

                HiggsScriptManager.AddRequiredScript(string.Format("~/_Resource/JavaScript/{0}/{1}", namespaceName.Substring(namespaceName.LastIndexOf('.') + 1), viewName));
            }
            else
            {
                control.Name = label;
            }

            return control;
        }

        public static TControl Name<TControl>(this TControl control, Expression<Func<object>> exp, string labelClassName)
            where TControl : InputControlBase
        {
            var label = exp.Compile().Invoke().ToString();

            if (control.IsSupportLabel)
            {
                control.AddLabel(label, labelClassName);
            }

            if (exp.Body is MemberExpression)
            {
                var me = (MemberExpression)exp.Body;

                if (me.Member.DeclaringType == null) throw new NullReferenceException();
                
                control.Name = string.Format("{0}.{1}", me.Member.DeclaringType.Name, me.Member.Name);

                var viewName = me.Member.DeclaringType.Name;
                var namespaceName = me.Member.DeclaringType.Namespace;
                if (namespaceName == null) throw new NullReferenceException();

                HiggsScriptManager.AddRequiredScript(string.Format("~/_Resource/JavaScript/{0}/{1}", namespaceName.Substring(namespaceName.LastIndexOf('.') + 1), viewName));
            }
            else
            {
                control.Name = label;
            }

            return control;
        }

        // TODO: 1.0 Create sharing function for duplicated code of BindData & BindValidationOutput
        public static TControl BindData<TControl, TModel>(this TControl control, TModel model, Expression<Func<TModel, object>> bindedProperty, string onErrorFn = null)
            where TControl : InputControlBase
        {
            MemberExpression me;
            if (bindedProperty.Body is MemberExpression)
            {
                me = bindedProperty.Body as MemberExpression;
            }
            else if (bindedProperty.Body is UnaryExpression)
            {
                // For supporting nullable type
                var ue = (UnaryExpression)bindedProperty.Body;

                me = ue.Operand as MemberExpression;
            }
            else
            {
                throw new ArgumentException(Resources.InvalidBindedPropertyException, "bindedProperty");
            }

            if (me != null)
            {
                if (string.IsNullOrWhiteSpace(control.Id))
                {
                    control.AutoGenerateIdFromBindingData(typeof(TModel).Name, me.Member.Name);
                }

                Action<string> addBindDataScript = controlName => HiggsScriptManager.AddjQueryObjectScript
                  (
                      control,
                      ".bindData(${0}.{1}, {2}{3})",
                      me.Expression.Type.Name,
                      me.Member.Name,
                      controlName.ToEscapeString(),
                      !string.IsNullOrWhiteSpace(onErrorFn) ? ", " + onErrorFn : string.Empty
                  );

                if (string.IsNullOrEmpty(control.Name))
                {
                    control.OnNameChange += new InputControlBase.NameChangeEventHandler(addBindDataScript);
                }
                else
                {
                    addBindDataScript(control.Name);
                }
            }
            else
            {
                throw new ArgumentException(Resources.InvalidBindedPropertyException, "bindedProperty");
            }

            return control;
        }

        public static TControl BindValidationOutput<TControl, TModel>(this TControl control, TModel model, Expression<Func<TModel, object>> bindedProperty)
            where TControl : InputControlBase
        {
            MemberExpression me;
            if (bindedProperty.Body is MemberExpression)
            {
                me = bindedProperty.Body as MemberExpression;
            }
            else if (bindedProperty.Body is UnaryExpression)
            {
                // For supporting nullable type
                var ue = (UnaryExpression)bindedProperty.Body;
                me = ue.Operand as MemberExpression;
            }
            else
            {
                throw new ArgumentException(Resources.InvalidBindedPropertyException, "bindedProperty");
            }

            if (me != null)
            {
                if (string.IsNullOrWhiteSpace(control.Id))
                {
                    control.AutoGenerateIdFromBindingData(typeof(TModel).Name, me.Member.Name);
                }

                Action<string> addBindDataScript = controlName => HiggsScriptManager.AddjQueryObjectScript
                  (
                      control,
                      ".bindValidationOutput(${0}.{1})",
                      me.Expression.Type.Name,
                      me.Member.Name
                  );

                if (string.IsNullOrEmpty(control.Name))
                {
                    control.OnNameChange += new InputControlBase.NameChangeEventHandler(addBindDataScript);
                }
                else
                {
                    addBindDataScript(control.Name);
                }
            }
            else
            {
                throw new ArgumentException(Resources.InvalidBindedPropertyException, "bindedProperty");
            }

            return control;
        }

        public static TControl AddClass<TControl>(this TControl control, params string[] className)
            where TControl : InputControlBase
        {
            var cssClass = control.MainControl.Attributes["class"] ?? string.Empty;
            cssClass = className.Aggregate(cssClass, (current, s) => current + (" " + s));

            control.MainControl.Attributes["class"] = cssClass;

            return control;
        }

        public static T TurnOffAutoComplete<T>(this T input)
            where T : InputControlBase
        {
            input.MainControl.Attributes["autocomplete"] = "off";

            return input;
        }

        public static T ReadOnly<T>(this T input, bool flag = true)
            where T : InputControlBase
        {
            if (flag)
            {
                input.MainControl.Attributes["readonly"] = "true";
                input.MainControl.AddCssClass("readonly");
            }
            else
            {
                if (input.MainControl.Attributes.ContainsKey("readonly"))
                {
                    input.MainControl.Attributes.Remove("readonly");
                }

                input.MainControl.RemoveClass("readonly");
            }

            return input;
        }

        public static T Disabled<T>(this T input, bool flag = true)
            where T : InputControlBase
        {
            if (flag)
            {
                input.MainControl.Attributes["disabled"] = "true";
                input.MainControl.AddCssClass("readonly");
            }

            return input;
        }

        private const int MaxOptionButton = 3;
        public static T AddOptionButton<T>(this T input, string iconPath, string buttonTitle, string onClickFn = null)
            where T : InputControlBase
        {
            var index = 1;

            while(index <= MaxOptionButton)
            {
                if (input.MainControl.RemoveClass(string.Format("has-{0}optionButton", index)))
                {
                    index++;
                    break;
                }

                index++;
            }

            if(index > MaxOptionButton)
            {
                index = 1;
            }

            input.MainControl.AddCssClass(string.Format("has-{0}optionButton", index));
            
            var optionButton = new TagBuilder("a");
            optionButton.AddCssClass("optionButton");
            optionButton.Attributes["title"] = buttonTitle;
            optionButton.Attributes["href"] = "javascript:void(0)";

            if (onClickFn != null)
            {
                optionButton.Attributes["onclick"] = onClickFn;
            }

            optionButton.InnerHtml = string.Format("<img src='{0}' alt='{1}' />", input.Helper.ResolveUrl(iconPath), buttonTitle);
            input.ListTags.Add(optionButton);

            return input;
        }
        
        public static TModel OnChange<TModel>(this TModel control, string scriptFn)
            where TModel : InputControlBase
        {
            Action<string> addBindDataScript = controlName => HiggsScriptManager.AddjQueryObjectScript
              (
                  control,
                  ".change({0})",
                  scriptFn
              );

            if (string.IsNullOrEmpty(control.Name))
            {
                control.OnNameChange += new InputControlBase.NameChangeEventHandler(addBindDataScript);
            }
            else
            {
                addBindDataScript(control.Name);
            }

            return control;
        }
        
        public static TControl SetAttribute<TControl>(this TControl control, string key, string value)
            where TControl : InputControlBase
        {
            control.MainControl.Attributes[key] = value;

            return control;
        }
    }
}
