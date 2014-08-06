using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Helper
{
    public class ComboBoxItem
    {
        private string value;
        private string text;
        public string Value { get { return value; }}
        public string Text { get { return text; }}

        public ComboBoxItem(string value, string text)
        {
            this.value = value;
            this.text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
