using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sisyphus.Models
{
    public class AppLog
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
    }

    public enum LogType
    {
        Info = 1,
        Warning = 2,
        Error = 3,


    }
}
