using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WebDaD.Toolkit.Export
{
    public class Content
    {
        private DataType type;
        public DataType Type { get { return type; } }

        private string text;
        public string Text { get { return text; } set { text = value; } }

        private Dictionary<string, string> paragraphs;
        public Dictionary<string, string> Paragraphs { get { return paragraphs; } set { paragraphs = value; } }

        private DataTable table;
        public DataTable Table { get { return table; } set { table = value; } }
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


        public Content(DataType type)
        {
            this.type = type;
        }

        public static readonly string EMPTY = "%EMPTY%";
    }
}
