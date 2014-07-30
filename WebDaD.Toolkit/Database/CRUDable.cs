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
        CRUDable New();
        Dictionary<string,string> GetIDList();
        CRUDable GetSingleInstance(string id);
        List<CRUDable> GetFullList();
    }
}
