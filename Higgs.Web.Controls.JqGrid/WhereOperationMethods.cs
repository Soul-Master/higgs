using System.Reflection;
using Higgs.Core.Helpers;

namespace Higgs.Web.Controls.JqGrid
{
    public static class WhereOperationMethods
    {
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
        public static MethodInfo Contains = ReflectionHelper.GetMethodInfo<string>(x => x.Contains(string.Empty));
        public static MethodInfo BeginsWith = ReflectionHelper.GetMethodInfo<string>(x => x.StartsWith(string.Empty));
        public static MethodInfo EndsWith = ReflectionHelper.GetMethodInfo<string>(x => x.EndsWith(string.Empty));
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
    }
}