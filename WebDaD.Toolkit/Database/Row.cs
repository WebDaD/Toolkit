using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public class Row
    {
        private int nr;
        private Dictionary<string,string> cells;

        public int Nr { get { return nr; } }

        public Dictionary<string, string> Cells { get { return cells; } }

        public List<string> CellContents
        {
            get
            {
                List<string> r = new List<string>();
                foreach (KeyValuePair<string,string> item in cells)
                {
                    r.Add(item.Value);
                }
                return r;
            }
        }

        public List<string> CellNames
        {
            get
            {
                List<string> r = new List<string>();
                foreach (KeyValuePair<string, string> item in cells)
                {
                    r.Add(item.Key);
                }
                return r;
            }
        }



        public Row(int nr)
        {
            this.nr = nr;
            this.cells = new Dictionary<string,string>();
        }
        public Row(int nr, Dictionary<string, string> cells)
        {
            this.nr = nr;
            this.cells = cells;
        }

        public void AddCell(string field, string content)
        {
            this.cells.Add(field, content);
        }
    }
}
