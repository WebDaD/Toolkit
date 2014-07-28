using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace WebDaD.Toolkit.Database
{
    public class Database_SQLite : Database
    {
        public string datasource;

        private SQLiteConnection connection;
        private SQLiteCommand cmd;

        public static String GetConnectionString(string datasource)
        {
            return "Data Source=" + datasource;
        }

        public static Database_SQLite getDatabase(string datasource)
        {
            return new Database_SQLite(datasource);
        }
        private Database_SQLite(string datasource)
        {
            this.datasource = datasource;
            this.connection = new SQLiteConnection();
            this.connection.ConnectionString = "Data Source=" + this.datasource;
            this.cmd = new SQLiteCommand(this.connection);
        }

        ~Database_SQLite()
        {
            Close();
            this.cmd.Dispose();
            this.connection.Dispose();
            this.cmd = null;
            this.connection = null;
        }

        //open connection
        public bool Open()
        {
            try
            {
                this.connection.Open();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool isOpen()
        {
            return this.connection.State == System.Data.ConnectionState.Open;
        }

        //close connection
        public bool Close()
        {
            try
            {
                this.connection.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string getValue(string table, string field, string filter)
        {
            this.cmd.CommandText = "SELECT " + field + " FROM " + table + " WHERE " + filter + " LIMIT 1";
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            string r = this.cmd.ExecuteScalar().ToString();
            return r;
        }

        public List<List<string>> getRow(string table,string[] fields, string filter="",string orderby="", int limit=0)
        {
            List<List<string>> r = new List<List<string>>();
            
            this.cmd.CommandText = "SELECT "+String.Join(",",fields)+" FROM " + table;
            if (!String.IsNullOrEmpty(filter)) this.cmd.CommandText += " WHERE " + filter;
            if (!String.IsNullOrEmpty(orderby)) this.cmd.CommandText += " ORDER BY " + orderby;
            if (limit > 0) this.cmd.CommandText += " LIMIT " + limit.ToString();
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                List<string> rx = new List<string>(); 
                foreach (string f in fields)
                {
                    rx.Add(re[f].ToString());
                }
                r.Add(rx);
            }
            re.Close();
            return r;
        }

        public bool Update(string table,Dictionary<string,string> fieldset,string filter)
        {
            string setter = "";
            foreach (KeyValuePair<string,string> item in fieldset)
            {
                setter+= "`"+item.Key+"`='"+item.Value+"', ";
            }
            setter = setter.Remove(setter.Length - 2);
            this.cmd.CommandText = "UPDATE " + table + " SET "+setter+" WHERE " + filter;
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            int r = this.cmd.ExecuteNonQuery();
            if (r > 0) return true;
            else return false;
        }

        public bool Execute(string sql)
        {
            this.cmd.CommandText = sql;
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            int r = this.cmd.ExecuteNonQuery();
            if (r > 0) return true;
            else return false;
        }

        public bool Insert(string table, Dictionary<string, string> fieldset)
        {
            string fields = "";
            foreach (KeyValuePair<string, string> item in fieldset)
            {
                fields += "`" + item.Key + "`, ";
            }
            fields = fields.Remove(fields.Length - 2);
            string values = "";
            foreach (KeyValuePair<string, string> item in fieldset)
            {
                values += "'" + item.Value + "', ";
            }
            values = values.Remove(values.Length - 2);
            this.cmd.CommandText = "INSERT INTO " + table + " ("+fields+") VALUES (" + values + ")";
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            int r = this.cmd.ExecuteNonQuery();
            if (r > 0) return true;
            else return false;
        }

        public string GetLastInsertedID()
        {
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            return this.connection.LastInsertRowId.ToString();
        }
    }
}
