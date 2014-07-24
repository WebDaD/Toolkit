using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Export
{
    public class Template
    {
        public static Dictionary<string, string> getTemplates(WebDaD.Toolkit.Database.Database db)
        {
            List<List<string>> d = new List<List<string>>();
            d = db.getRow(Template.table, new string[] { "id", "name" });

            Dictionary<string, string> r = new Dictionary<string, string>();
            foreach (List<string> item in d)
            {
                r.Add(item[0], item[1]);
            }

            if (d.Count > 0) return r;
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

        private string header;
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

        public Dictionary<string, string> FieldSet
        {
            get
            {
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("name", this.name);
                r.Add("beforeContent", this.beforeContent);
                r.Add("afterContent", this.afterContent);
                r.Add("header", this.header);
                r.Add("footer", this.footer);
                return r;
            }
        }

        public Template(WebDaD.Toolkit.Database.Database db, string id)
        {
            this.db = db;
            List<List<string>> d = new List<List<string>>();
            d = this.db.getRow(Template.table, new string[] { "id", "name","beforeContent","afterContent","header","footer"},"`id`='"+id+"'","",1);
            this.id = d[0][0];
            this.name = d[0][1];
            this.beforeContent = d[0][2];
            this.afterContent = d[0][3];
            this.header = d[0][4];
            this.footer = d[0][5];
        }

        public Template(WebDaD.Toolkit.Database.Database db)
        {
            this.db = db;
            this.name = "";
            this.beforeContent = "";
            this.afterContent = "";
            this.header = " | | ";
            this.footer = " | | ";
        }

        public bool Save()
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
            return ok;
        }

        public bool Delete()
        {
            bool ok = true;

            ok = db.Execute("DELETE FROM "+Template.table+" WHERE `id`="+this.id);
            return ok;
        }
    }
}
