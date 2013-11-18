using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;

namespace Higgs.FluentValidation
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public Dictionary<string, string> MappedProperty = new Dictionary<string, string>();
        public IRuleBuilderInitial<T, TProperty> MapRule<TProperty>(Expression<Func<T, TProperty>> toExpression, Expression<Func<T, TProperty>> fromExpression)
        {
            var copyRule = PropertyRule.Create(toExpression, () => CascadeMode);
            AddRule(copyRule);

            var emu = GetEnumerator();
            var fromProperty = fromExpression.GetMember().Name;
            var toProperty = toExpression.GetMember().Name;
            MappedProperty.Add(toProperty, fromProperty);

            while (emu.MoveNext())
            {
                var rule = emu.Current as PropertyRule;

                if (rule == null || rule.PropertyName != fromProperty) continue;

                foreach(var v in rule.Validators)
                {
                    copyRule.AddValidator(v);
                }
            }

            var ruleBuilder = new RuleBuilder<T, TProperty>(copyRule);
            return ruleBuilder;
        }
    }
}
