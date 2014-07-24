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

        public Content(DataType type)
        {
            this.type = type;
        }
    }
}
