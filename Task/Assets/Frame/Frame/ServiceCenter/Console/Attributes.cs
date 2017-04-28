// -----------------------------------------------------------------
// File:    Attributes.cs
// Author:  mouguangyi
// Date:    2017.01.22
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Text.RegularExpressions;

namespace GameBox.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ConsoleRouteAttribute : Attribute
    {
        public delegate void Callback(IConsoleContext context);

        public ConsoleRouteAttribute(string route, string methods = @"(GET|HEAD)", bool runOnMainThread = true)
        {
            this.route = new Regex(route, RegexOptions.IgnoreCase);
            this.methods = new Regex(methods);
            this.runOnMainThread = runOnMainThread;
        }

        public Regex route;
        public Regex methods;
        public bool runOnMainThread;
        public Callback callback;
    }
}