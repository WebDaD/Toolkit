using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Export
{
    public class ContentParagraphs : Content
    {
        public ContentParagraphs(DataType type, Dictionary<string, string> paragraphs)
            : base(type)
        {
            this.paragraphs = paragraphs;
        }

        private Dictionary<string, string> paragraphs;
        public Dictionary<string, string> Paragraphs { get { return paragraphs; }}
    }
}
