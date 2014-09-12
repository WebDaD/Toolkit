using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDaD.Toolkit.Database
{
    public class Table
    {
        private string name;
        public string Name { get { return this.name; } }

        private Dictionary<string, FieldType> fields;
        public Dictionary<string, FieldType> Fields { get { return this.fields; } }

        private string primaryKey;
        public string PrimaryKey { get { return this.primaryKey; } }

        public Table(string name)
        {
            this.name = name;
            this.fields = new Dictionary<string, FieldType>();
            this.primaryKey = "";
        }

        public void AddField(string name, FieldType type)
        {
            this.fields.Add(name, type);
        }

        public bool SetPrimaryKey(string name)
        {
            if (this.fields.ContainsKey(name))
            {
                this.primaryKey = name;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Compares this Object with the Database
        /// </summary>
        /// <param name="db">A valid Database Object</param>
        /// <returns>If the Table is in the Database or not</returns>
        public bool CompareWith(Database db)
        {
            if (this.Equals(db.GetTable(this.Name))) return true;
            else return false;
        }
    }
}
