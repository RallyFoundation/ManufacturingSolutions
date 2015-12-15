using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.Data
{
    public class QueryXPathFilter
    {
        public string LogicalExpression;
        public string ComparisonExpression;
        public string ContextNode;
        public int NodeType;
        public string NodeName;
        public string NodeValue;

        [XmlIgnore]
        public LogicalExpressionEnum QueryLogicalExpression 
        {
            set 
            {
                switch (value)
                {
                    case (LogicalExpressionEnum.And): 
                    {
                        this.LogicalExpression = "and";
                        break;
                    }
                    case (LogicalExpressionEnum.Or):
                    {
                        this.LogicalExpression = "or";
                        break;
                    }
                    default:
                    {
                        this.LogicalExpression = "and";
                        break;
                    }
                }
            }
        }

        [XmlIgnore]
        public ComparisonExpressionEnum QueryComparisonExpression 
        {
            set 
            {
                switch (value)
                {
                    case ComparisonExpressionEnum.EqualTo:
                    {
                        this.ComparisonExpression = "=";
                        break;
                    }
                    case ComparisonExpressionEnum.NotEqualTo:
                    {
                        this.ComparisonExpression = "!=";
                        break;
                    }
                    case ComparisonExpressionEnum.GreaterThan:
                    {
                        this.ComparisonExpression = ">";
                        break;
                    }
                    case ComparisonExpressionEnum.GreaterThanOrEqualTo:
                    {
                        this.ComparisonExpression = ">=";
                        break;
                    }
                    case ComparisonExpressionEnum.LessThan:
                    {
                        this.ComparisonExpression = "&lt;";
                        break;
                    }
                    case ComparisonExpressionEnum.LessThanOrEqualTo:
                    {
                        this.ComparisonExpression = "&lt;=";
                        break;
                    }
                    default:
                    {
                        this.ComparisonExpression = "=";
                        break;
                    }
                }
            }
        }

        [XmlIgnore]
        public string QueryContextNode 
        {
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.ContextNode = ".";
                }
                else
                {
                    this.ContextNode = value;
                }
            }
        }

        [XmlIgnore]
        public NodeTypeEnum QueryNodeType 
        {
            set 
            {
                switch (value)
                {
                    case NodeTypeEnum.Element:
                    {
                        this.NodeType = 0;
                        break;
                    }
                    case NodeTypeEnum.Attribute:
                    {
                        this.NodeType = 1;
                        break;
                    }
                    default:
                    {
                        this.NodeType = 0;
                        break;
                    }
                }
            }
        }

        [XmlIgnore]
        public string QueryNodeName 
        {
            set 
            {
                this.NodeName = value;
            }
        }

        [XmlIgnore]
        public object QueryNodeValue 
        {
            set 
            {
                this.NodeValue = value.ToString();

                if (value is string)
                {
                    this.NodeValue = "'" + this.NodeValue + "'";
                }
                else if (value is bool) 
                {
                    this.NodeValue += "()";
                }
            }
        }

        public string GetQueryExpression(bool appendParenthesiss, bool appendLogicalExpression) 
        {
            string returnValue = String.Empty;

            if (appendParenthesiss)
            {
                returnValue += "(";
            }

            returnValue += this.ContextNode;

            if (!this.ContextNode.EndsWith("/"))
            {
                returnValue += "/";
            }

            if (this.NodeType == 1)
            {
                returnValue += "@";
            }

            returnValue += this.NodeName + " ";
            returnValue += this.ComparisonExpression + " ";
            //returnValue += "'";
            returnValue += this.NodeValue;
            //returnValue += "'";

            if (appendParenthesiss)
            {
                returnValue += ")";
            }

            returnValue += " ";

            if (appendLogicalExpression)
            {
                returnValue += this.LogicalExpression;
                returnValue += " ";
            }

            return returnValue;
        }
    }

    public enum LogicalExpressionEnum 
    {
        And = 0,
        Or = 1
    }

    public enum ComparisonExpressionEnum 
    {
        EqualTo = 0,
        NotEqualTo = 1,
        GreaterThan = 2,
        GreaterThanOrEqualTo = 3,
        LessThan = 4,
        LessThanOrEqualTo = 5
    }

    public enum NodeTypeEnum 
    {
        Element = 0,
        Attribute = 1
    }
}
