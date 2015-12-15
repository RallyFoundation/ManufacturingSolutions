using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.Data
{
    [Serializable]
    [XmlType(AnonymousType = false)]
    public class SearchItem
    {
        private List<QueryXPathFilter> queryXPathFilters;

        private SortedDictionary<string, object> configurationItems;

        public string Condition { get; set; }

        public string Value { get; set; }

        public string JointWord { get; set; }

        public string FieldName { get; set; }

        public string ProviderName { get; set; }

        public void SetConfigurationItems(string key, object value) 
        {
            if (this.configurationItems == null)
            {
                this.configurationItems = new SortedDictionary<string, object>();
            }

            if (!this.configurationItems.ContainsKey(key))
            {
                this.configurationItems.Add(key, value);
            }
            else if (this.configurationItems.ContainsKey(key)) 
            {
                this.configurationItems[key] = value;
            }
        }

        public List<QueryXPathFilter> QueryXPathFilters 
        {
            get 
            {
                return this.queryXPathFilters;
            }
            set 
            {
                this.queryXPathFilters = value;
            }
        }

        [XmlIgnore]
        public SearchConditionEnum SearchCondition 
        {
            get 
            {
                SearchConditionEnum returnValue = SearchConditionEnum.EqualTo;

                switch (this.Condition)
                {
                    case("="):
                    {
                        returnValue = SearchConditionEnum.EqualTo;
                        break;
                    }
                    case ("!="): 
                    {
                        returnValue = SearchConditionEnum.NotEqualTo;
                        break;
                    }
                    case ("<>"):
                    {
                        returnValue = SearchConditionEnum.NotEqualTo;
                        break;
                    }
                    case(">"):
                    {
                        returnValue = SearchConditionEnum.GreaterThan;
                        break;
                    }
                    case (">="):
                    {
                        returnValue = SearchConditionEnum.GreaterThanOrEqualTo;
                        break;
                    }
                    case ("<"): 
                    {
                        returnValue = SearchConditionEnum.LessThan;
                        break;
                    }
                    case ("<="):
                    {
                        returnValue = SearchConditionEnum.LessThanOrEqualTo;
                        break;
                    }
                    case (" is "):
                    {
                        returnValue = SearchConditionEnum.Is;
                        break;
                    }
                    case (" is not "):
                    {
                        returnValue = SearchConditionEnum.IsNot;
                        break;
                    }
                    case (" in "):
                    {
                        returnValue = SearchConditionEnum.In;
                        break;
                    }
                    case (" not in "):
                    {
                        returnValue = SearchConditionEnum.NotIn;
                        break;
                    }
                    case(" like '"):
                    {
                        returnValue = SearchConditionEnum.StartsWith;
                        break;
                    }
                    case (" not like '"):
                    {
                        returnValue = SearchConditionEnum.NotStartWith;
                        break;
                    }
                    case (" like '%"):
                    {
                        returnValue = SearchConditionEnum.EndsWith;
                        break;
                    }
                    case (" not like '%"):
                    {
                        returnValue = SearchConditionEnum.NotEndWith;
                        break;
                    }
                    case (" like '%%"):
                    {
                        returnValue = SearchConditionEnum.Includes;
                        break;
                    }
                    case (" not like '%%"):
                    {
                        returnValue = SearchConditionEnum.NotInclude;
                        break;
                    }
                }

                return returnValue;
            }
            set 
            {
                switch (value)
                {
                    case (SearchConditionEnum.EqualTo):
                    {
                        this.Condition = "=";
                        break;
                    }
                    case (SearchConditionEnum.NotEqualTo):
                    {
                        string dbProviderName = this.ProviderName;

                        if (dbProviderName == "system.data.sqlclient")
                        {
                            this.Condition = "<>";
                        }
                        else if (dbProviderName == "oracle.dataaccess.client")
                        {
                            this.Condition = "!=";
                        }
                        else if (dbProviderName == "system.data.oracleclient")
                        {
                            this.Condition = "!=";
                        }
                        else
                        {
                            this.Condition = "<>";
                        }

                        break;
                    }
                    case (SearchConditionEnum.GreaterThan):
                    {
                        this.Condition = ">";
                        break;
                    }
                    case (SearchConditionEnum.GreaterThanOrEqualTo):
                    {
                        this.Condition = ">=";
                        break;
                    }
                    case (SearchConditionEnum.LessThan):
                    {
                        this.Condition = "<";
                        break;
                    }
                    case (SearchConditionEnum.LessThanOrEqualTo):
                    {
                        this.Condition = "<=";
                        break;
                    }
                    case (SearchConditionEnum.Is):
                    {
                        this.Condition = " is ";
                        break;
                    }
                    case (SearchConditionEnum.IsNot):
                    {
                        this.Condition = " is not ";
                        break;
                    }
                    case (SearchConditionEnum.In):
                    {
                        this.Condition = " in ";
                        break;
                    }
                    case (SearchConditionEnum.NotIn):
                    {
                        this.Condition = " not in ";
                        break;
                    }
                    case (SearchConditionEnum.StartsWith): 
                    {
                        this.Condition = " like '";
                        break;
                    }
                    case (SearchConditionEnum.NotStartWith):
                    {
                        this.Condition = " not like '";
                        break;
                    }
                    case (SearchConditionEnum.EndsWith):
                    {
                        this.Condition = " like '%";
                        break;
                    }
                    case (SearchConditionEnum.NotEndWith):
                    {
                        this.Condition = " not like '%";
                        break;
                    }
                    case (SearchConditionEnum.Includes):
                    {
                        this.Condition = " like '%%";
                        break;
                    }
                    case (SearchConditionEnum.NotInclude):
                    {
                        this.Condition = " not like '%%";
                        break;
                    }
                    default:
                    {
                        this.Condition = "=";
                        break;
                    }
                }
            }
        }

        [XmlIgnore]
        public object SearchValue 
        {
            get 
            {
                return this.Value;
            }
            set 
            {
                if (value is string)
                {
                    if (String.IsNullOrEmpty(value as string))
                    {
                        if ((this.SearchCondition != SearchConditionEnum.Is) && (this.SearchCondition != SearchConditionEnum.IsNot))
                        {
                            this.SearchCondition = SearchConditionEnum.Is;
                            this.Condition = " is ";
                        }
                        else if (this.SearchCondition == SearchConditionEnum.Is)
                        {
                            this.Condition = " is ";
                        }
                        else if (this.SearchCondition == SearchConditionEnum.IsNot)
                        {
                            this.Condition = " is not ";
                        }

                        this.Value = "null";
                    }
                    else
                    {
                        if ((this.SearchCondition == SearchConditionEnum.In) | (this.SearchCondition == SearchConditionEnum.NotIn))
                        {
                            this.Value = value as string;
                        }
                        else if((this.SearchCondition == SearchConditionEnum.StartsWith) | (this.SearchCondition == SearchConditionEnum.NotStartWith))
                        {
                            this.Value = value as string;
                        }
                        else if ((this.SearchCondition == SearchConditionEnum.EndsWith) | (this.SearchCondition == SearchConditionEnum.NotEndWith))
                        {
                            this.Value = value as string;
                        }
                        else if ((this.SearchCondition == SearchConditionEnum.Includes) | (this.SearchCondition == SearchConditionEnum.NotInclude))
                        {
                            this.Value = value as string;
                        }
                        else
                        {
                            this.Value = "'" + value as string + "'";
                        }
                    }
                }
                else if(value is DateTime)
                {
                    string dbProviderName = this.ProviderName;

                    string dateTimeValueString = ((DateTime)(value)).ToShortDateString();

                    if ((this.configurationItems != null) && (this.configurationItems.ContainsKey("DateTimeFormatPattern")))
                    {
                        dateTimeValueString = ((DateTime)(value)).ToString(this.configurationItems["DateTimeFormatPattern"].ToString(), System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }

                    if (dbProviderName == "system.data.sqlclient")
                    {
                        this.Value = "convert(datetime, '" + dateTimeValueString + "')";
                    }
                    else if (dbProviderName == "oracle.dataaccess.client")
                    {
                        this.Value = "to_timestamp('" + dateTimeValueString + "')";
                    }
                    else if (dbProviderName == "system.data.oracleclient")
                    {
                        this.Value = "to_timestamp('" + dateTimeValueString + "')";
                    }
                    else
                    {
                        this.Value = "convert(datetime, '" + dateTimeValueString + "')";
                    }
                }
                else if (value == null)
                {
                    if ((this.SearchCondition != SearchConditionEnum.Is) && (this.SearchCondition != SearchConditionEnum.IsNot))
                    {
                        this.SearchCondition = SearchConditionEnum.Is;
                        this.Condition = " is ";
                    }
                    else if (this.SearchCondition == SearchConditionEnum.Is)
                    {
                        this.Condition = " is ";
                    }
                    else if (this.SearchCondition == SearchConditionEnum.IsNot)
                    {
                        this.Condition = " is not ";
                    }

                    this.Value = "null";
                }
                else
                {
                    this.Value = value.ToString();
                }

                if ((this.SearchCondition == SearchConditionEnum.In) | (this.SearchCondition == SearchConditionEnum.NotIn))
                {
                    this.Value = "(" + this.Value + ")";
                }
                else if ((this.SearchCondition == SearchConditionEnum.StartsWith) | (this.SearchCondition == SearchConditionEnum.NotStartWith))
                {
                    this.Value = this.Value + "%'";
                }
                else if ((this.SearchCondition == SearchConditionEnum.EndsWith) | (this.SearchCondition == SearchConditionEnum.NotEndWith))
                {
                    this.Value = this.Value + "'";
                }
                else if ((this.SearchCondition == SearchConditionEnum.Includes) | (this.SearchCondition == SearchConditionEnum.NotInclude))
                {
                    this.Value = this.Value + "%%'";
                }
            }
        }

        [XmlIgnore]
        public JointWordEnum SearchJointWord 
        {
            get 
            {
                JointWordEnum returnValue = JointWordEnum.And;

                switch (this.JointWord.ToLower())
                {
                    case ("and"): 
                    {
                        returnValue = JointWordEnum.And;
                        break;
                    }
                    case ("or"):
                    {
                        returnValue = JointWordEnum.Or;
                        break;
                    }
                    default: 
                    {
                        returnValue = JointWordEnum.And;
                        break;
                    }
                }

                return returnValue;
            }
            set 
            {
                switch (value)
                {
                    case (JointWordEnum.And):
                    {
                        this.JointWord = "and";
                        break;
                    }
                    case (JointWordEnum.Or):
                    {
                        this.JointWord = "or";
                        break;
                    }
                    default:
                    {
                        this.JointWord = "and";
                        break;
                    }
                }
            }
        }

        [XmlIgnore]
        public string SearchFieldName 
        {
            get 
            {
                return this.FieldName;
            }
            set 
            {
                this.FieldName = value;
            }
        }

        public string GetQueryXPathFilterExpression(bool appendSquareBrackets)
        {
            string returnValue = "";

            if ((this.queryXPathFilters != null) && (this.queryXPathFilters.Count > 0))
            {
                for (int i = 0; i < this.queryXPathFilters.Count; i++)
                {
                    if (i < this.queryXPathFilters.Count - 1)
                    {
                        returnValue += this.queryXPathFilters[i].GetQueryExpression(true, true);
                    }
                    else if (i == this.queryXPathFilters.Count - 1)
                    {
                        returnValue += this.queryXPathFilters[i].GetQueryExpression(true, false);
                    }
                }

                if ((!String.IsNullOrEmpty(returnValue)) && (appendSquareBrackets))
                {
                    returnValue = "[" + returnValue + "]";
                }
            }

            return returnValue;
        }
    }

    public enum SearchConditionEnum 
    {
        EqualTo = 0,
        NotEqualTo = 1,
        GreaterThan = 2,
        GreaterThanOrEqualTo = 3,
        In = 4,
        NotIn = 5,
        Is = 6,
        IsNot = 7,
        LessThan = 8,
        LessThanOrEqualTo = 9,
        StartsWith = 10,
        NotStartWith = 11,
        EndsWith = 12,
        NotEndWith = 13,
        Includes = 14,
        NotInclude = 15
    }

    public enum JointWordEnum 
    {
        And = 0,
        Or = 1
    }
}
