using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Export
{
    public class ContentText : Content
    {
        public ContentText(DataType type, string text)
            : base(type)
        {
            this.text = text;
        }

        private string text;
        public string Text { get { return text; }}
    }
}
