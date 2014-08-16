namespace Higgs.Core
{
    public interface IValue<out T>
    {
// ReSharper disable UnusedMemberInSuper.Global
        T Value
        {
            get;
        }
// ReSharper restore UnusedMemberInSuper.Global
    }
}
