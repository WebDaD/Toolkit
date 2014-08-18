using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDaD.Toolkit.Log
{
    public class Log
    {
        //export is always: [YYYY-MM-DD] :: [type] :: tag :: msg

        private LogType type;
        private WebDaD.Toolkit.Database.Database db;
        private string file;

        public LogEntry LatestEntry
        {
            get
            {
                switch (this.type)
                {
                    case LogType.Database: return getEntry(ErrorLevel.None);
                    case LogType.File: return getEntry(ErrorLevel.None);
                    default: return null;
                }
            }
        }
        public LogEntry LatestError
        {
            get
            {
                switch (this.type)
                {
                    case LogType.Database: return getEntry(ErrorLevel.Error);
                    case LogType.File: return getEntry(ErrorLevel.Error);
                    default: return null;
                }
            }
        }
        public LogEntry LatestWarning
        {
            get
            {
                switch (this.type)
                {
                    case LogType.Database: return getEntry(ErrorLevel.Warning);
                    case LogType.File: return getEntry(ErrorLevel.Warning);
                    default: return null;
                }
            }
        }
        public LogEntry LatestDebug
        {
            get
            {
                switch (this.type)
                {
                    case LogType.Database: return getEntry(ErrorLevel.Debug);
                    case LogType.File: return getEntry(ErrorLevel.Debug);
                    default: return null;
                }
            }
        }
        public LogEntry LatestInfo
        {
            get
            {
                switch (this.type)
                {
                    case LogType.Database: return getEntry(ErrorLevel.Info);
                    case LogType.File: return getEntry(ErrorLevel.Info);
                    default: return null;
                }
            }
        }

        private LogEntry getEntry(ErrorLevel errorLevel)
        {
            switch (this.type)
            {
                case LogType.Database:
                    string filter = "";
                    if (errorLevel == ErrorLevel.None) filter = "";
                    else filter = "errorlevel='" + errorLevel.ToString() + "'";
                    WebDaD.Toolkit.Database.Result r = this.db.getRow("log", new string[] { "errorlevel", "errordatetime", "tag","message" }, filter, "errordatetime DESC", 1);
                    if (r.RowCount > 0)
                    {
                        return new LogEntry(r.FirstRow["errordatetime"] + " :: " + r.FirstRow["errorlevel"] + " :: " + r.FirstRow["tag"]+" :: " + r.FirstRow["message"]);
                    }
                    else return null;
                case LogType.File:
                    List<LogEntry> lines = new List<LogEntry>();
                    string line = "";
                    using (StreamReader sr = new StreamReader(this.file))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(new LogEntry(line));
                        }
                    }
                    LogEntry latest = new LogEntry(DateTime.MinValue,ErrorLevel.None,"","");
                    foreach (LogEntry item in lines)
                    {
                        if (errorLevel != ErrorLevel.None)
                        {
                            if (item.ErrorLevel == errorLevel && item.ErrorDateTime > latest.ErrorDateTime) latest = item;
                        }
                        else
                        {
                            if (item.ErrorDateTime > latest.ErrorDateTime) latest = item;
                        }
                    }
                    return latest;
                default: return null;
            }

        }
        public Log(WebDaD.Toolkit.Database.Database db)
        {
            type = LogType.Database;
            this.db = db;
            Dictionary<string, WebDaD.Toolkit.Database.FieldType> fields = new Dictionary<string, WebDaD.Toolkit.Database.FieldType>();
            fields.Add("id", WebDaD.Toolkit.Database.FieldType.PrimaryInteger);
            fields.Add("errorlevel", WebDaD.Toolkit.Database.FieldType.ShortString);
            fields.Add("errordatetime", WebDaD.Toolkit.Database.FieldType.DateTime);
            fields.Add("message", WebDaD.Toolkit.Database.FieldType.String);
            fields.Add("tag", WebDaD.Toolkit.Database.FieldType.String);
            this.db.CreateTable("log", fields, "id");
        }
        public Log(string file)
        {
            type = LogType.File;
            this.file = file;
            if (!File.Exists(file)) File.Create(file);
        }


        private void write(ErrorLevel e, string tag,string msg)
        {
            switch (this.type)
            {
                case LogType.Database:
                    Dictionary<string, string> fieldset = new Dictionary<string, string>();
                    fieldset.Add("errorlevel", e.ToString());
                    fieldset.Add("errordatetime", DateTime.Now.ToString("YYYY-MM-DDTHH:ii:ss"));
                    fieldset.Add("message", msg);
                    fieldset.Add("tag", tag);
                    this.db.Insert("log", fieldset);
                    break;
                case LogType.File:
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                    {
                        //export is always: [YYYY-MM-DD] :: [type] :: msg
                        sw.WriteLine(DateTime.Now.ToString("YYYY-MM-DDTHH:ii:ss") + " :: " + e.ToString() + " :: "+tag+" :: " + msg);
                    }
                    break;
            }
        }

        /// <summary>
        /// An Info Entry
        /// </summary>
        /// <param name="msg">The Message</param>
        public void i(string tag,string msg)
        {
            write(ErrorLevel.Info, tag,msg);
        }
        /// <summary>
        /// A Debug Entry
        /// </summary>
        /// <param name="msg">The Message</param>
        public void d(string tag, string msg)
        {
            write(ErrorLevel.Debug, tag,msg);
        }
        /// <summary>
        /// An Error Entry
        /// </summary>
        /// <param name="msg">The Message</param>
        public void e(string tag, string msg)
        {
            write(ErrorLevel.Error, tag,msg);
        }
        /// <summary>
        /// A Warning Entry
        /// </summary>
        /// <param name="msg">The Message</param>
        public void w(string tag,string msg)
        {
            write(ErrorLevel.Warning, tag,msg);
        }
    }
}
