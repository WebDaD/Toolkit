using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public interface Joinable
    {
        string GetJoinOn(Joinable jointable);
        string GetTableName();
        List<string> GetFields(bool joinable);
    }
}
