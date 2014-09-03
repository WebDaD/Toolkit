using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebDaD.Toolkit.Database;

namespace WebDaD.Toolkit.Export
{
    public class Template
    {
        public static readonly string TAB = "\t";
        public static readonly string HTML_SPACE = "&nbsp;";
        public static readonly string SPACE = " ";

        /// <summary>
        /// Used if Header / Footer is FULLLength (only use Header / Footer)
        /// </summary>
        public static readonly string FULLSTARTER = "%FULL%";

        /// <summary>
        /// Used for empty Fields, must be replaced by specific empty entity
        /// </summary>
        public static readonly string EMPTY = "%EMPTY%";

        /// <summary>
        /// Used to mark images, path is between those two
        /// </summary>
        public static readonly string IMAGE_TAG = "%IMG%";
        public static readonly string IMAGE_END = "%/IMG%";

        public static readonly string LINEBREAK = "%LB%";

        //Placeholders
        public static readonly string ADDR = "%ADDR%";
        public static readonly string OBJECT_ID = "%OBJECT_ID%";
        public static readonly string WORKER = "%WORKER%";
        public static readonly string DATE_CREATE = "%DATE_CREATE%";
        public static readonly string DATE_SECOND = "%DATE_SECOND%";

        public static string ReplacePlaceholder(string origin, string id, string addr, string worker, string datecreate, string datesecond)
        {
            if(!String.IsNullOrEmpty(addr))
                origin = origin.Replace(ADDR, "Adresse: "+LINEBREAK+addr.Replace("\n",LINEBREAK));
            else
                origin = origin.Replace(ADDR, "");

            if (!String.IsNullOrEmpty(id))
                origin = origin.Replace(OBJECT_ID, "ID: "+id);
            else
                origin = origin.Replace(OBJECT_ID, "");

            if (!String.IsNullOrEmpty(worker))
                origin = origin.Replace(WORKER, "Mitarbeiter: "+worker);
            else
                origin = origin.Replace(WORKER, "");

            if (!String.IsNullOrEmpty(datecreate))
                origin = origin.Replace(DATE_CREATE, "Datum: "+datecreate);
            else
                origin = origin.Replace(DATE_CREATE, "");

            if (!String.IsNullOrEmpty(datesecond))
                origin = origin.Replace(DATE_SECOND, datesecond); //Already contains LABEL!
            else
                origin = origin.Replace(DATE_SECOND, "");

            return origin;
        }

        public static Dictionary<string, string> getTemplates(WebDaD.Toolkit.Database.Database db)
        {
            WebDaD.Toolkit.Database.Result d = db.getRow(Template.table, new string[] { "id", "name" },"","");

            Dictionary<string, string> r = new Dictionary<string, string>();

            r.Add("0", "Keines");

            foreach (WebDaD.Toolkit.Database.Row item in d.Rows)
            {
                r.Add(item.Cells["id"],item.Cells["name"]);
            }

            if (d.RowCount > 0) return r;
            else return null;
        }

        private static string table = "templates";

        private WebDaD.Toolkit.Database.Database db;

        private string id;
        public string ID { get { return id; } set { id = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private string beforeContent;
        public string BeforeContent { get { return beforeContent; } set { beforeContent = value; } }

        private string afterContent;
        public string AfterContent { get { return afterContent; } set { afterContent = value; } }

        private string textBefore;
        public string TextBefore_Left
        {
            get
            {
                return textBefore.Split('|')[0];
            }
            set
            {
                string[] ht = textBefore.Split('|');
                textBefore = value + "|" + ht[1];
            }
        }
        public string TextBefore_Right
        {
            get
            {
                return textBefore.Split('|')[1];
            }
            set
            {
                string[] ht = textBefore.Split('|');
                textBefore = ht[0] + "|" + value;
            }
        }
        private string header;
        public string Header {
            get { if (this.header.StartsWith(Template.FULLSTARTER))return Header_Left; else return null; }
            set
            {
                this.header = Template.FULLSTARTER+value;
            }
        }
        public string Header_Left
        {
            get
            {
                return header.Split('|')[0];
            }
            set
            {
                string[] ht = header.Split('|');
                header = value + "|" + ht[1] + "|" + ht[2];
            }
        }
        public string Header_Center
        {
            get
            {
                return header.Split('|')[1];
            }
            set
            {
                string[] ht = header.Split('|');
                header = ht[0] + "|" + value + "|" + ht[2];
            }
        }
        public string Header_Right
        {
            get
            {
                return header.Split('|')[2];
            }
            set
            {
                string[] ht = header.Split('|');
                header = ht[0] + "|" + ht[1] + "|" + value;
            }
        }

        private string footer;
        public string Footer
        {
            get { if (this.footer.StartsWith(Template.FULLSTARTER))return Footer_Left; else return null; }
            set
            {
                this.footer = Template.FULLSTARTER + value;
            }
        }
        public string Footer_Left
        {
            get
            {
                return footer.Split('|')[0];
            }
            set
            {
                string[] ht = footer.Split('|');
                footer = value + "|" + ht[1] + "|" + ht[2];
            }
        }
        public string Footer_Center
        {
            get
            {
                return footer.Split('|')[1];
            }
            set
            {
                string[] ht = footer.Split('|');
                footer = ht[0] + "|" + value + "|" + ht[2];
            }
        }
        public string Footer_Right
        {
            get
            {
                return footer.Split('|')[2];
            }
            set
            {
                string[] ht = footer.Split('|');
                footer = ht[0] + "|" + ht[1] + "|" + value;
            }
        }

        private Dictionary<string, string> cssDic;
        private string dbCSS { get {
            string r = "";
            foreach (KeyValuePair<string,string> item in this.cssDic)
            {
                r += item.Key + ":" + item.Value + "|";
            }
            r = r.Remove(r.Length - 1);
            return r;
        
        } }
        public Dictionary<string, string> CSS
        {
            get { return this.cssDic; }
        }
        private Dictionary<string, string> createDic(string css_string)
        {
            Dictionary<string, string> c = new Dictionary<string, string>();
            if (css_string.Contains('|'))
            {
                foreach (string item in css_string.Split('|'))
                {
                    string[] tmp = item.Split(':');
                    c.Add(tmp[0], tmp[1]);
                }
            }

            return c;
        }
        public Dictionary<string, string> FieldSet
        {
            get
            {
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("name", this.name);
                r.Add("beforeContent", this.beforeContent);
                r.Add("afterContent", this.afterContent);
                r.Add("textBefore", this.textBefore);
                r.Add("header", this.header);
                r.Add("footer", this.footer);
                r.Add("css", this.dbCSS);
                return r;
            }
        }

        private bool empty;

        public Template(WebDaD.Toolkit.Database.Database db, string id)
        {
            this.db = db;
            WebDaD.Toolkit.Database.Result d = this.db.getRow(Template.table, new string[] { "id", "name", "beforeContent", "afterContent","textBefore", "header", "footer","css" }, "`id`='" + id + "'", "", 1);
            this.id = d.FirstRow["id"];
            this.name = d.FirstRow["name"];
            this.beforeContent = d.FirstRow["beforeContent"];
            this.afterContent = d.FirstRow["afterContent"];
            this.textBefore = d.FirstRow["textBefore"];
            this.header = d.FirstRow["header"];
            this.footer = d.FirstRow["footer"];
            this.cssDic = createDic(d.FirstRow["css"]);
            this.empty = false;
        }

        

        public Template(WebDaD.Toolkit.Database.Database db)
        {
            this.db = db;
            this.name = "";
            this.beforeContent = "";
            this.afterContent = "";
            this.textBefore = "|";
            this.header = " | | ";
            this.footer = " | | ";
            this.cssDic = new Dictionary<string,string>();
            this.empty = true;
        }

        public Template()
        {
            this.name = "";
            this.beforeContent = "";
            this.afterContent = "";
            this.textBefore = "";
            this.header = "";
            this.footer = "";
            this.cssDic = new Dictionary<string, string>();
            this.empty = true;
        }

        public bool IsEmpty { get { return this.empty; } }

        /// <summary>
        /// Saves the Data into the Database and included Images into a folder
        /// </summary>
        /// <param name="images">A List of images to be force-copied</param>
        /// <returns>if all went good</returns>
        public bool Save(List<string> images, string path)
        {
            bool ok = true;
            if (String.IsNullOrEmpty(this.id))
            {
                ok = db.Insert(Template.table, this.FieldSet);
            }
            else
            {
                ok = db.Update(Template.table, this.FieldSet, "`id`='" + this.id + "'");

            }
            if (ok)
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (string img in images)
                {
                    File.Copy(img, path + Path.DirectorySeparatorChar + Path.GetFileName(img),true);
                }
            }

            return ok;
        }
        public string NiceID
        {
            get
            {
                string r = "T%ID5%";
               

                string reg_ID = @"%ID(\d+)%";
                string count = "";
                Regex reg = new Regex(reg_ID);
                Match m = reg.Match(r);
                if (m.Success)
                {
                    Group g = m.Groups[1];
                    count = g.ToString();
                    int c = Int32.Parse(count);
                    r = r.Replace("%ID" + count + "%", id.PadLeft(c, '0'));
                }


                return r;
            }
        }

        public bool Delete()
        {
            bool ok = true;

            ok = db.Execute("DELETE FROM "+Template.table+" WHERE `id`="+this.id);
            return ok;
        }
    }
}
