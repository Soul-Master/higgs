using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Validators;

namespace Higgs.FluentValidation
{
    public class CollectionCountValidator : PropertyValidator, ICollectionCountValidator
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public CollectionCountValidator(int min, int max)
            : this(min, max, () => "Undefined")
        {
        }

        public CollectionCountValidator(int min, int max, Expression<Func<string>> errorMessageResourceSelector)
            : base(errorMessageResourceSelector)
        {
            Max = max;
            Min = min;

            if (max < min)
            {
                throw new ArgumentOutOfRangeException("max", Resources.CollectionCountValidator_ErrorMessage);
            }
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var collection = context.PropertyValue as IEnumerable;
            var count = collection != null ? collection.Cast<object>().Count() : 0;

            if (count < Min || count > Max)
            {
                context.MessageFormatter
                    .AppendArgument("MinLength", Min)
                    .AppendArgument("MaxLength", Max)
                    .AppendArgument("TotalLength", count);

                return false;
            }

            return true;
        }
    }
}