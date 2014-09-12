using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO.Compression;
using System.IO;

namespace WebDaD.Toolkit.Database
{
    public class Database_SQLite : Database
    {
        public string datasource;

        private SQLiteConnection connection;
        private SQLiteCommand cmd;

        public bool isValid(List<Table> tables)
        {
            bool c = true;
            foreach (Table t in tables)
            {
                c = t.CompareWith(this);
                if (!c) break;
            }
            return c;
        }

        public static String GetConnectionString(string datasource)
        {
            return "Data Source=" + datasource;
        }
        public static Database_SQLite createFromConnectionString(string connectionString)
        {
            string datasource = "";
            string[] tmp = connectionString.Split(';');
            foreach (string item in tmp)
            {
                if (item.Contains("Data Source")) datasource = item.Split('=')[1].Trim();
            }
            return new Database_SQLite(datasource);
        }
        public DatabaseType getType() { return DatabaseType.SQLite; }

        public static Database_SQLite getDatabase(string datasource)
        {
            return new Database_SQLite(datasource);
        }
        private Database_SQLite(string datasource)
        {
            this.datasource = datasource;
            this.connection = new SQLiteConnection();
            this.connection.ConnectionString = this.datasource;
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
        public string ConnectionString()
        {
            return "Data Source=" + datasource;
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
            object o = this.cmd.ExecuteScalar();
            if (o != null) return o.ToString();
            else return "";
        }

        public Result getRow(string table, string[] fields, string filter = "", string orderby = "", int limit = 0)
        {
            this.cmd.CommandText = "SELECT " + String.Join(",", fields) + " FROM " + table;
            if (!String.IsNullOrEmpty(filter)) this.cmd.CommandText += " WHERE " + filter;
            if (!String.IsNullOrEmpty(orderby)) this.cmd.CommandText += " ORDER BY " + orderby;
            if (limit > 0) this.cmd.CommandText += " LIMIT " + limit.ToString();

            Result r = new Result(this.cmd.CommandText);
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                Row row = new Row(re.StepCount);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
            }
            re.Close();
            return r;
        }

        public bool Update(string table, Dictionary<string, string> fieldset, string filter)
        {
            string setter = "";
            foreach (KeyValuePair<string, string> item in fieldset)
            {
                setter += "`" + item.Key + "`='" + item.Value + "', ";
            }
            setter = setter.Remove(setter.Length - 2);
            this.cmd.CommandText = "UPDATE " + table + " SET " + setter + " WHERE " + filter;
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
            this.cmd.CommandText = "INSERT INTO " + table + " (" + fields + ") VALUES (" + values + ")";
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



        public Result Select(string sql)
        {
            List<string> fields = new List<string>();

            string fieldset = sql.Split(new string[] { "FROM" }, StringSplitOptions.None)[0].Replace("SELECT", "");
            string[] sf = fieldset.Split(',');
            foreach (string item in sf)
            {
                string adder = "";
                if (item.ToLower().Contains(" as "))
                {
                    adder = item.ToLower().Split(new string[] { " as " }, StringSplitOptions.None)[1].Trim();
                }
                else
                {
                    adder = item.Trim();
                }
                if (adder.Contains('.'))
                {
                    adder = adder.Split('.')[1];
                }
                fields.Add(adder);
            }

            this.cmd.CommandText = sql;
            Result r = new Result(this.cmd.CommandText);
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                Row row = new Row(re.StepCount);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
            }
            re.Close();
            return r;

        }

        public bool CreateTable(string table, Dictionary<string, FieldType> fields, string primary_field)
        {
            string sql = "CREATE TABLE IF NOT EXISTS " + table + " (";
            foreach (KeyValuePair<string, FieldType> item in fields)
            {
                sql += item.Key + " " + ft2String(item.Value);
                if (item.Key == primary_field)
                {
                    sql += " PRIMARY KEY";
                }
                sql += ", ";
            }
            sql = sql.Remove(sql.Length - 1);
            sql += ")";

            return this.Execute(sql);
        }
        public bool CreateTable(Table table)
        {
            string sql = "CREATE TABLE IF NOT EXISTS " + table.Name + " (";
            foreach (KeyValuePair<string, FieldType> item in table.Fields)
            {
                sql += item.Key + " " + ft2String(item.Value);
                if (item.Key == table.PrimaryKey)
                {
                    sql += " PRIMARY KEY";
                }
                sql += ", ";
            }
            sql = sql.Remove(sql.Length - 1);
            sql += ")";

            return this.Execute(sql);
        }
        public Table GetTable(string name)
        {
            Table t = new Table(name);
            Result r = Select("PRAGMA table_info(" + name + ")");
            foreach (Row item in r.Rows)
            {
                string fname = item.Cells["name"];
                FieldType ftype = string2ft(item.Cells["type"]);
                t.AddField(fname, ftype);
                if (item.Cells["pk"] == "1") t.SetPrimaryKey(fname);
            }

            return t;
        }

        private FieldType string2ft(string field)
        {
            switch(field)
            {
                case "TEXT": return FieldType.String;
                case "NUMBER": return FieldType.Integer;
                case "INTEGER NOT NULL AUTO_INCREMENT": return FieldType.PrimaryInteger;
                case "REAL": return FieldType.Float;
                default: return FieldType.String;//TODO: Error
            }
        }
        private string ft2String(FieldType p)
        {
            switch (p)
            {
                case FieldType.String:return "TEXT";
                case FieldType.ShortString: return "TEXT";
                case FieldType.Date: return "TEXT";
                case FieldType.DateTime: return "TEXT";
                case FieldType.Integer: return "NUMBER";
                case FieldType.PrimaryInteger: return "INTEGER NOT NULL AUTO_INCREMENT";//TODO: check with file
                case FieldType.Float: return "REAL";
                default://TODO: ERROR
                    return null;
            }
        }

        public Result getRow(string table, string[] fields, Condition[] c = null, GroupBy g = null, OrderBy[] o = null, int limit = 0)
        {
            this.cmd.CommandText = "SELECT " + String.Join(",", fields) + " FROM " + table;
            if (c != null)
            {
                this.cmd.CommandText += " WHERE (";
                foreach (Condition item in c)
                {
                    this.cmd.CommandText += item.ToString() + " AND ";
                }
                this.cmd.CommandText = this.cmd.CommandText.Remove(this.cmd.CommandText.Length - 5);
                this.cmd.CommandText += ") ";
            }
            if (g != null)
            {
                this.cmd.CommandText += g.ToString();
            }
            if (o != null)
            {
                this.cmd.CommandText += " ORDER BY ";
                foreach (OrderBy item in o)
                {
                    this.cmd.CommandText += item.ToShortString() + ", ";
                }
                this.cmd.CommandText = this.cmd.CommandText.Remove(this.cmd.CommandText.Length - 2);
            }
            if (limit != 0)
            {
                this.cmd.CommandText += " LIMIT " + limit.ToString();
            }
            Result r = new Result(this.cmd.CommandText);
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                Row row = new Row(re.StepCount);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
            }
            re.Close();
            return r;
        }

        public Result getRow(Joinable j, string[] fields, Condition[] c = null, GroupBy g = null, OrderBy[] o = null, int limit = 0)
        {
            this.cmd.CommandText = "SELECT " + String.Join(",", fields) + " FROM " + j.GetTableName();
            if (c != null)
            {
                this.cmd.CommandText += " WHERE (";
                foreach (Condition item in c)
                {
                    this.cmd.CommandText += item.ToString() + " AND ";
                }
                this.cmd.CommandText = this.cmd.CommandText.Remove(this.cmd.CommandText.Length - 5);
                this.cmd.CommandText += ") ";
            }
            if (g != null)
            {
                this.cmd.CommandText += g.ToString();
            }
            if (o != null)
            {
                this.cmd.CommandText += " ORDER BY ";
                foreach (OrderBy item in o)
                {
                    this.cmd.CommandText += item.ToShortString() + ", ";
                }
                this.cmd.CommandText = this.cmd.CommandText.Remove(this.cmd.CommandText.Length - 2);
            }
            if (limit != 0)
            {
                this.cmd.CommandText += " LIMIT " + limit.ToString();
            }
            Result r = new Result(this.cmd.CommandText);
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                Row row = new Row(re.StepCount);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
            }
            re.Close();
            return r;
        }

        /// <summary>
        /// Get a Join on TWO tables.
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="c"></param>
        /// <param name="g"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public Result Join(Joinable[] tables, Condition[] c, GroupBy g, OrderBy[] o)
        {
            string sql = "SELECT ";
            List<string> fields = new List<string>();
            foreach (Joinable j in tables)
            {
                sql += String.Join(",", j.GetFields(true)) + ", ";
                foreach (string item in j.GetFields(true))
                {
                    fields.Add(item);
                }
            }
            sql = sql.Remove(sql.Length - 2);
            sql += " FROM ";
            foreach (Joinable j in tables)
            {
                sql += j.GetTableName() + ", ";
            }
            sql = sql.Remove(sql.Length - 2);
            sql += " WHERE (";
            sql += tables[0].GetJoinOn(tables[1]) + "=" + tables[1].GetJoinOn(tables[0]); //TODO: make this work for more tables...
            sql += ") ";
            if (c != null)
            {
                sql += " AND (";
                foreach (Condition item in c)
                {
                    sql += item.ToString() + " AND ";
                }
                sql = sql.Remove(sql.Length - 5);
                sql += ") ";
            }
            if (g != null)
            {
                sql += g.ToString();
            }
            if (o != null)
            {
                sql += " ORDER BY ";
                foreach (OrderBy item in o)
                {
                    sql += item.ToShortString() + ", ";
                }
                sql = sql.Remove(sql.Length - 2);
            }
            this.cmd.CommandText = sql;
            Result r = new Result(this.cmd.CommandText);
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            SQLiteDataReader re = this.cmd.ExecuteReader();
            while (re.Read())
            {
                Row row = new Row(re.StepCount);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
            }
            re.Close();
            return r;
        }


        public bool Dump(string targetFile)
        {
            using (ZipArchive newFile = ZipFile.Open(targetFile, ZipArchiveMode.Create))
            {
                newFile.CreateEntryFromFile(this.datasource.Replace("Data Source=",""), Path.GetFileName(this.datasource.Replace("Data Source=","")));
            }
            return true;
        }

        public bool Restore(string sourceFile)
        {
            using (ZipArchive archive = ZipFile.OpenRead(sourceFile))
            {
                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    file.ExtractToFile(this.datasource.Replace("Data Source=", ""), true);
                }
            }
            return true;
        }
    }
}
