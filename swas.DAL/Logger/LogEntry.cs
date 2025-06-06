using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Logger
{
    
    public class LogEntry
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string LogLevel { get; set; }
        public int ThreadId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionSource { get; set; }
    }

}
