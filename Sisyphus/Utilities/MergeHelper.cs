using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;

namespace Sisyphus.Utilities
{
    public static class MergeHelper
    {
        public static string Merge( string input, Dictionary<string, string> mergeValues )
        {


            var result = Engine.Razor.RunCompile( input, input, null, mergeValues );

            return result;
        }
    }
}
