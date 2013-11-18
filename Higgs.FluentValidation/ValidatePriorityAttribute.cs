using System;

namespace Higgs.FluentValidation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class ValidatePriorityAttribute : Attribute
    {
        public int Priority { get; set; }

        public ValidatePriorityAttribute(int priority)
        {
            Priority = priority;    
        }
    }
}
