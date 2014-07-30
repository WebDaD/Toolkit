using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public class OrderBy
    {
        public readonly static int ASC = 1;
        public readonly static int DESC = 0;

        private string field;

        private int direction;

        public OrderBy(string field, int direction)
        {
            this.field = field;
            this.direction = direction;
        }

        public override string ToString()
        {
            return "ORDER BY `" + this.field + "` " + ((this.direction == ASC) ? "ASC" : "DESC");
        }

        public string ToShortString()
        {
            return this.field + "` " + ((this.direction == ASC) ? "ASC" : "DESC");
        }
    }
}
