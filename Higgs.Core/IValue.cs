namespace Higgs.Core
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}
