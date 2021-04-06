using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Sisyphus.Utilities
{
    public static class ConfigurationHelper
    {
        public static IConfiguration Configuration { get; set; }
    }
}
