using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public interface CRUDable
    {
        bool Save();
        bool Delete();
        Dictionary<string,string> GetDictionary();
        List<CRUDable> GetFullList();
        bool Load(string id);
        List<string> GetIDList();
    }
}
