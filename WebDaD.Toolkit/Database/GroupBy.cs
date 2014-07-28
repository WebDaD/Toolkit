using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public class GroupBy
    {

        private string field;


        public GroupBy(string field)
        {
            this.field = field;
        }

        public override string ToString()
        {
            return "GROUP BY `"+this.field+"`";
        }

    }
}
