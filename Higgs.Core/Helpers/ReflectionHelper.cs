﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace Higgs.Core.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetProductName(this Assembly a)
        {
            // Get all Product attributes on this assembly
            var attributes = a.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

            return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
        }

        public static string GetProductTitle(this Assembly a)
        {
            // Get all Product attributes on this assembly
            var attributes = a.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            return attributes.Length == 0 ? "" : ((AssemblyTitleAttribute)attributes[0]).Title;
        }
        
        public static string GetVersion(this Assembly a)
        {
            return a.GetName().Version.ToString();
        }

        public static string GetFileVersion(this Assembly a)
        {
            // Get all Product attributes on this assembly
            var attributes = a.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);

            return attributes.Length == 0 ? "" : ((AssemblyFileVersionAttribute)attributes[0]).Version;
        }

        public static string GetInformationalVersion(this Assembly a)
        {
            // Get all Product attributes on this assembly
            var attributes = a.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);

            return attributes.Length == 0 ? "" : ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;
        }

        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> exp)
        {
            var methodCallExp = exp.Body as MethodCallExpression;

            return methodCallExp == null ? null : methodCallExp.Method;
        }

        public static Assembly GetCallerAssembly(this Assembly currentAssembly)
        {
            var frames = new StackTrace().GetFrames();

            if (frames == null) return null;
            frames = frames.Skip(1).ToArray();

            return 
            (
                from f in frames 
                select f.GetMethod() into method
                where method != null && method.ReflectedType != null
                select method.ReflectedType.Assembly
            ).FirstOrDefault(ass => ass != currentAssembly);
        }
    }
}
