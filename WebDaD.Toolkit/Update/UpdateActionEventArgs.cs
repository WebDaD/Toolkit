using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDaD.Toolkit.Update
{
    public class UpdateActionEventArgs : EventArgs
    {
            public string Message { get; set; }
            public int Percent { get; set; }
    }
}
