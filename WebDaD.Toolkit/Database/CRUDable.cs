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
        Dictionary<string,string> GetIDList();
        List<CRUDable> GetFullList();
    }
}
