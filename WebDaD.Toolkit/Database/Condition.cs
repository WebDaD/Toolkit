using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public class Condition
    {
        private string field;

        private ConditionOperator coperator;

        private string value;

        public Condition(string field, ConditionOperator co, string value)
        {
            this.field = field;
            this.coperator = co;
            this.value = value;
        }
        public override string ToString()
        {
            string r = "`"+this.field+"`";
            switch (this.coperator)
            {
                case ConditionOperator.IS:
                    r += "=";
                    break;
                case ConditionOperator.ISNOT:
                    r += "!=";
                    break;
                case ConditionOperator.LESSTHAN:
                    r += "<";
                    break;
                case ConditionOperator.LESSOREQUALTHAN:
                    r += "<=";
                    break;
                case ConditionOperator.GREATERTHAN:
                    r += ">";
                    break;
                case ConditionOperator.GREATEROREQUALTHAN:
                    r += ">=";
                    break;
                case ConditionOperator.LIKE:
                    r += " LIKE ";
                    break;
                default:
                    break;
            }
            if (this.coperator != ConditionOperator.LIKE)
            {
                r += "'" + value + "'";
            }
            else
            {
                r += "'%" + value + "%'";
            }
            return r;
        }
    }
}
