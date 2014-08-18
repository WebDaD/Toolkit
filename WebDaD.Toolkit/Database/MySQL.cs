using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Compression;

namespace WebDaD.Toolkit.Database
{
    public class Database_MySQL : Database
    {
        public string server;
        public string database;
        public string user;
        public string password;

        private MySqlConnection connection;
        private MySqlCommand cmd;

        public static String GetConnectionString(string server, string database,string user, string password)
        {
            return "Server="+server+";Database="+database+";Uid="+user+";Pwd="+password+";";
        }

        public static Database_MySQL getDatabase(string server, string database,string user, string password)
        {
            return new Database_MySQL(server,database,user,password);
        }
        private Database_MySQL(string server, string database, string user, string password)
        {
            this.server = server;
            this.database=database;
            this.user=user;
            this.password=password;
            this.connection = new MySqlConnection(Database_MySQL.GetConnectionString(server,database,user,password));;
            this.cmd = new MySqlCommand();
            this.cmd.Connection = this.connection;
        }

        ~Database_MySQL()
        {
            Close();
            this.cmd.Dispose();
            this.connection.Dispose();
            this.cmd = null;
            this.connection = null;
        }

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
            MySqlDataReader re = this.cmd.ExecuteReader();
            int i=0;
            while (re.Read())
            {
                Row row = new Row(i);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
                i++;
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
            MySqlDataReader re = this.cmd.ExecuteReader();
            int i = 0;
            while (re.Read())
            {
                Row row = new Row(i);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
                i++;
            }
            re.Close();
            return r;
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
            MySqlDataReader re = this.cmd.ExecuteReader();
            int i = 0;
            while (re.Read())
            {
                Row row = new Row(i);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
                i++;
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

        public bool Execute(string sql)
        {
            this.cmd.CommandText = sql;
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            int r = this.cmd.ExecuteNonQuery();
            if (r > 0) return true;
            else return false;
        }

        public string GetLastInsertedID()
        {
            if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
            return this.cmd.LastInsertedId.ToString();
        }

        public bool isOpen()
        {
            return this.connection.State == System.Data.ConnectionState.Open;
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
            MySqlDataReader re = this.cmd.ExecuteReader();
            int i = 0;
            while (re.Read())
            {
                Row row = new Row(i);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
                i++;
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
            MySqlDataReader re = this.cmd.ExecuteReader();
            int i = 0;
            while (re.Read())
            {
                Row row = new Row(i);

                foreach (string f in fields)
                {
                    row.AddCell(f, re[f].ToString());
                }

                r.AddRow(row);
                i++;
            }
            re.Close();
            return r;
        }

        public bool Dump(string targetFile)
        {
            string tempfile = Path.GetDirectoryName(targetFile) + "database.sql";
            using (MySqlBackup mb = new MySqlBackup(cmd))
            {
                if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
                mb.ExportToFile(tempfile);
            }
            using (ZipArchive newFile = ZipFile.Open(targetFile, ZipArchiveMode.Create))
            {
                newFile.CreateEntryFromFile(tempfile, Path.GetFileName(tempfile), CompressionLevel.Optimal);
            }
            return true;
        }

        public bool Restore(string sourceFile)
        {
            string tempfile = Path.GetDirectoryName(sourceFile) + "database.sql";
            using (ZipArchive archive = ZipFile.OpenRead(sourceFile))
            {
                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    file.ExtractToFile(tempfile, true);
                }
            }
            using (MySqlBackup mb = new MySqlBackup(cmd))
            {
                if (this.connection.State != System.Data.ConnectionState.Open) this.connection.Open();
                mb.ImportFromFile(tempfile);
            }
            return true;
        }

        private string ft2String(FieldType p)
        {
            switch (p)
            {
                case FieldType.String: return "TEXT";
                case FieldType.ShortString: return "VARCHAR(100)";
                case FieldType.Date: return "DATE";
                case FieldType.DateTime: return "DATETIME";
                case FieldType.Integer: return "INT(11)";
                case FieldType.PrimaryInteger: return "INT(11) NOT NULL AUTO_INCREMENT";//TODO: check with file
                default://TODO: ERROR
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
