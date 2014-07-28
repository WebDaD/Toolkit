using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public interface Joinable
    {
        string JoinOn(Joinable jointable);
        string GetTableName();
    }
}
