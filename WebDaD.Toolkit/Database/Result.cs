using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public class Result
    {
        private string query;
        private List<Row> rows;

        public int RowCount { get { return rows.Count; } }
        public string Query { get { return query; } }

        public List<Row> Rows { get { return rows; } }

        public Result(string query)
        {
            this.query = query;
            this.rows = new List<Row>();
        }

        public Dictionary<string,string> FirstRow{get {return rows[0].Cells;}}

        public void AddRow(Row row)
        {
            this.rows.Add(row);
        }
        public void AddRow(Dictionary<string, string> cells)
        {
            int i = this.rows.Count;
            this.rows.Add(new Row(i, cells));
        }
    }
}
