using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public interface Database
    {
        bool Open();
        bool Close();
        string getValue(string table, string field, string filter);
        List<List<string>> getRow(string table, string[] fields, string filter = "",string orderby="", int limit = 0);
        bool Update(string table, Dictionary<string, string> fieldset, string filter);
        bool Insert(string table, Dictionary<string, string> fieldset);
        bool Execute(string sql);
        string GetLastInsertedID();
        bool isOpen();
    }
}
