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
        

        public Args(string[] args)
        {
            arg_value = new Dictionary<string, string>();
            //TODO: Parse args, fill dictionary, fill default atrs
        }
    }
}
