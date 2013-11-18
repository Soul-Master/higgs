using FluentValidation.Validators;

namespace Higgs.FluentValidation
{
    public interface ICollectionCountValidator : IPropertyValidator
    {
        int Min { get; }
        int Max { get; }
    }
}