using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WebDaD.Toolkit.Export
{
    public abstract class Content
    {
        private DataType type;
        public DataType Type { get { return type; } }

        public Content(DataType type)
        {
            this.type = type;
        }
    }
}
