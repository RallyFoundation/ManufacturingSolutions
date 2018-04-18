using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QA.UI.RuleConfiguration.ViewModel
{
    public class RuleItem : ObservableObject
    {
        private string fieldName;
        private object fieldValue;
        private string ruleType;
        public string FieldlName { get { return fieldName; }  set { fieldName = value; RaisePropertyChangedEvent("FieldName"); } }
        public object FieldlValue { get { return fieldValue; } set { fieldValue = value; RaisePropertyChangedEvent("FieldValue"); } }
        public string RuleType { get { return ruleType; } set { ruleType = value; RaisePropertyChangedEvent("RuleType"); } }
    }
}
