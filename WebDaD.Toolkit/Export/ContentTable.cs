using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WebDaD.Toolkit.Export
{
    public class ContentTable : Content
    {
        public ContentTable(DataType type, DataTable table)
            : base(type)
        {
            this.table = table;
        }

        private DataTable table;
        public DataTable Table { get { return table; } }
        public int MaxColLength
        {
            get
            {
                int max = 0;
                foreach (DataRow r in this.table.Rows)
                {
                    foreach (object item in r.ItemArray)
                    {
                        if (item.ToString().Length > max) max = item.ToString().Length;
                    }
                }
                return max;
            }
        }
        public int TableWidth
        {
            get
            {
                int max = 0;
                foreach (DataRow r in this.table.Rows)
                {
                    int tmp = 0;
                    foreach (object item in r.ItemArray)
                    {
                        tmp += item.ToString().Length;
                    }
                    if (tmp > max) max = tmp;
                }
                return max;
            }
        }
    }
}
