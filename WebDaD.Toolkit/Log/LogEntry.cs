using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDaD.Toolkit.Log
{
    public class LogEntry
    {
        private DateTime errorDateTime;
        public DateTime ErrorDateTime {get{return this.errorDateTime;}}
        private ErrorLevel errorLevel;
        public ErrorLevel ErrorLevel { get { return this.errorLevel; } }
        private string tag;
        public string Tag { get { return this.tag; } }
        private string message;
        public string Message { get { return this.message; } }

        public LogEntry(string processedString)
        {
            string[] parts = processedString.Split(new string[] { " :: " },StringSplitOptions.None);
            this.errorDateTime = DateTime.Parse(parts[0]);
            this.errorLevel = (ErrorLevel)Enum.Parse(typeof(ErrorLevel), parts[1]);
            this.tag = parts[2];
            this.message = parts[3];
        }
        public LogEntry(DateTime errorDateTime, ErrorLevel errorLevel, string tag,string message)
        {
            this.errorDateTime = errorDateTime;
            this.errorLevel = errorLevel;
            this.tag = tag;
            this.message = message;
        }

        public override string ToString()
        {
            return this.errorDateTime.ToString("YYYY-MM-DDTHH:ii:ss") + " :: " + this.errorLevel.ToString() + " :: " + tag+" :: " + message;
        }
    }
}
