using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Helper
{
    public class Args
    {
        private Dictionary<string, string> arg_value;
        private bool silent;
        private bool autostart;
        private WebDaD.Toolkit.Database.Database db;

        public bool Silent { get { return this.silent; } }
        public bool AutoStart { get { return this.autostart; } }
        public Dictionary<string, string> Arguments { get { return this.arg_value; } }
        public WebDaD.Toolkit.Database.Database Database { get { return this.db; } }

        public Args(string[] args)
        {
            this.silent = false;
            this.autostart = false;
            this.arg_value = new Dictionary<string, string>();

            foreach (string arg in args)
            {
                if (arg.Equals("silent")) this.silent = true;
                else if (arg.Equals("autostart")) this.autostart = true;
                else if (arg.Contains("db"))
                {
                    string[] data = arg.Split('=')[1].Split('|');
                    WebDaD.Toolkit.Database.DatabaseType dbt = (WebDaD.Toolkit.Database.DatabaseType)Enum.Parse(typeof(WebDaD.Toolkit.Database.DatabaseType), data[0]);
                    switch (dbt)
                    {
                        case WebDaD.Toolkit.Database.DatabaseType.SQLite:
                            this.db = WebDaD.Toolkit.Database.Database_SQLite.createFromConnectionString(data[1]);
                            break;
                        case WebDaD.Toolkit.Database.DatabaseType.MySQL:
                            this.db = WebDaD.Toolkit.Database.Database_MySQL.createFromConnectionString(data[1]);
                            break;
                    }
                }
                else
                {
                    string[] tmp = arg.Split('=');
                    arg_value.Add(tmp[0], tmp[1]);
                }
            }
        }
    }
}
