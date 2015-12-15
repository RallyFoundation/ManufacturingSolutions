using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace DataIntegrator.Extensions.XSLT
{
    public class DefaultXSLTExtension
    {
        /// <summary>
        /// Gets the kilo representation of a decimal by supplying its value and the places to be rounded up 
        /// </summary>
        /// <param name="value">Value of the decimal</param>
        /// <param name="places">Places to be rounded up</param>
        /// <returns>Kilo representation of the decimal after rounding</returns>
        public string GetKiloValue(decimal value, int places)
        {
            string returnValue = value.ToString();

            decimal decimalValue = value;

            int k = 0;

            while (Math.Abs(decimalValue) >= 1000)
            {
                decimalValue = decimalValue / 1000;
                k++;
            }

            decimalValue = decimal.Round(decimalValue, places);

            returnValue = decimalValue.ToString() + "×1000" + "<sup>" + k.ToString() + "</sup>";

            return returnValue;
        }

        /// <summary>
        /// Gets the scientific representation of a decimal by supplying its value and the places to be rounded up
        /// </summary>
        /// <param name="value">Value of the decimal</param>
        /// <param name="places">Places to be rounded up</param>
        /// <returns>Scientific representation of a decimal after rounding</returns>
        public string GetScientificValue(string value, int places)
        {
            string returnValue = value;

            if ((value.Contains(".")) && ((value.StartsWith("0.0")) | (value.StartsWith("-0.0"))) && (value.IndexOf(".") == value.LastIndexOf(".")))
            {
                int indexOfNegativeSign = value.IndexOf("-");
                int zeroCount = 0;
                bool parsed = false;
                decimal decimalValue = -9;

                value = value.Substring(indexOfNegativeSign + 1);

                for (int i = 2; i < value.Length; i++)
                {
                    if (value.Substring(i, 1) != "0")
                    {
                        returnValue = value.Substring(i);
                        break;
                    }
                    else
                    {
                        zeroCount++;
                    }
                }

                returnValue = returnValue.Insert(1, ".");

                parsed = Decimal.TryParse(returnValue, out decimalValue);

                if (parsed)
                {
                    decimalValue = Decimal.Round(decimalValue, places);
                }

                returnValue = decimalValue.ToString();

                returnValue += "E-";
                returnValue += zeroCount.ToString();

                if (indexOfNegativeSign == 0)
                {
                    returnValue = "-" + returnValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Generates an XML node set according to the number and name of child nodes specified
        /// </summary>
        /// <param name="nodeName">Name of child nodes</param>
        /// <param name="nodeCount">Total number of child nodes</param>
        /// <returns>The XML node set generated</returns>
        public XPathNodeIterator GetVirtualXmlNodeSet(string nodeName, int nodeCount)
        {
            XPathNodeIterator returnValue = null;

            if ((!String.IsNullOrEmpty(nodeName)) && (nodeCount > 0))
            {
                string xmlString = "<?xml version='1.0' encoding='utf-8' standalone='yes'?><VirtualNodes>";

                for (int i = 1; i <= nodeCount; i++)
                {
                    xmlString += "<" + nodeName + ">";
                    xmlString += i.ToString();
                    xmlString += "</" + nodeName + ">";
                }

                xmlString += "</VirtualNodes>";

                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlString);

                XPathNavigator navigator = document.CreateNavigator();

                //returnValue = navigator.Select("//" + nodeName);

                //returnValue.MoveNext();

                returnValue = navigator.Select("/");
            }

            return returnValue;
        }

        /// <summary>
        /// Generates an XML node set according to the number, name, name space prefix and name space of child nodes specified
        /// </summary>
        /// <param name="nodeName">Name of child nodes</param>
        /// <param name="nodeCount">Total number of child nodes</param>
        /// <param name="nodePrefix">Name space prefix of child nodes</param>
        /// <param name="nameSpace">Name space of child nodes</param>
        /// <returns>The XML node set generated</returns>
        public XPathNodeIterator GetVirtualXmlNodeSetWithNameSpace(string nodeName, int nodeCount, string nodePrefix, string nameSpace)
        {
            XPathNodeIterator returnValue = null;

            if ((!String.IsNullOrEmpty(nodeName)) && (nodeCount > 0))
            {
                string xmlString = "<?xml version='1.0' encoding='utf-8' standalone='yes'?>";

                xmlString += "<VirtualNodes " + "xmlns:" + nodePrefix + "='" + nameSpace + "'>";

                for (int i = 1; i <= nodeCount; i++)
                {
                    xmlString += "<" + nodePrefix + ":" + nodeName + ">";
                    xmlString += i.ToString();
                    xmlString += "</" + nodePrefix + ":" + nodeName + ">";
                }

                xmlString += "</VirtualNodes>";

                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlString);

                XPathNavigator navigator = document.CreateNavigator();

                returnValue = navigator.Select("//" + nodePrefix + ":" + nodeName);

                returnValue.MoveNext();
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation and separator specified, and then calculates the summary value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <returns>Summary value of the elements in the collection</returns>
        public string GetSumValueString(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }
                    returnValue = decimalSumValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the summary value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>Summary value of the elements in the collection</returns>
        public string GetSumValueString(string valueCollectionString, string seperator, int decimals)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        decimalSumValue = Decimal.Round(decimalSumValue, decimals);
                    }

                    returnValue = decimalSumValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the summary value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>Summary value of the elements in the collection</returns>
        public string GetSumValueString(string valueCollectionString, string seperator, int decimals, int roundingMode)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        switch (roundingMode)
                        {
                            case (0):
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                            case (1):
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.AwayFromZero);
                                    break;
                                }
                            default:
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                        }
                    }

                    returnValue = decimalSumValue.ToString();
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Converts a string representation to a collection according to the string representation and separator specified, and then calculates the average value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <returns>Average value of the elements in the collection</returns>
        public string GetAvgValueString(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }

                    decimalSumValue = decimalSumValue / values.Length;

                    returnValue = decimalSumValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the average value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>Average value of the elements in the collection</returns>
        public string GetAvgValueString(string valueCollectionString, string seperator, int decimals)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }

                    decimalSumValue = decimalSumValue / values.Length;

                    if (decimals >= 0)
                    {
                        decimalSumValue = Decimal.Round(decimalSumValue, decimals);
                    }

                    returnValue = decimalSumValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the average value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>Average value of the elements in the collection</returns>
        public string GetAvgValueString(string valueCollectionString, string seperator, int decimals, int roundingMode)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            break;
                        }
                    }

                    decimalSumValue = decimalSumValue / values.Length;

                    if (decimals >= 0)
                    {
                        switch (roundingMode)
                        {
                            case (0):
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                            case (1):
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.AwayFromZero);
                                    break;
                                }
                            default:
                                {
                                    decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                        }
                    }

                    returnValue = decimalSumValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation and separator specified, and then calculates the variance value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <returns>Variance value of the elements in the collection</returns>
        public string GetVarianceValueString(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 1))//n-1
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;
                    decimal[] decimalValues = new decimal[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                            decimalValues[i] = decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            decimalValues = null;
                            break;
                        }
                    }

                    if ((decimalValues != null) && (decimalValues.Length == values.Length))
                    {
                        decimalValue = decimalSumValue / values.Length;
                        decimalSumValue = 0;

                        decimal decimalSquareValue = -9;

                        for (int j = 0; j < decimalValues.Length; j++)
                        {
                            decimalSquareValue = (decimalValues[j] - decimalValue);
                            decimalSquareValue = decimalSquareValue * decimalSquareValue;
                            decimalSumValue += decimalSquareValue;
                        }

                        //decimalSumValue = decimalSumValue / decimalValues.Length;

                        decimalSumValue = decimalSumValue / (decimalValues.Length - 1);

                        decimalSumValue = ((decimal)(Math.Sqrt(((double)(decimalSumValue)))));

                        returnValue = decimalSumValue.ToString();
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the variance value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>Variance value of the elements in the collection</returns>
        public string GetVarianceValueString(string valueCollectionString, string seperator, int decimals)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 1))//n-1
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;
                    decimal[] decimalValues = new decimal[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                            decimalValues[i] = decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            decimalValues = null;
                            break;
                        }
                    }

                    if ((decimalValues != null) && (decimalValues.Length == values.Length))
                    {
                        decimalValue = decimalSumValue / values.Length;
                        decimalSumValue = 0;

                        decimal decimalSquareValue = -9;

                        for (int j = 0; j < decimalValues.Length; j++)
                        {
                            decimalSquareValue = (decimalValues[j] - decimalValue);
                            decimalSquareValue = decimalSquareValue * decimalSquareValue;
                            decimalSumValue += decimalSquareValue;
                        }

                        //decimalSumValue = decimalSumValue / decimalValues.Length;

                        decimalSumValue = decimalSumValue / (decimalValues.Length - 1);

                        decimalSumValue = ((decimal)(Math.Sqrt(((double)(decimalSumValue)))));

                        if (decimals >= 0)
                        {
                            decimalSumValue = Decimal.Round(decimalSumValue, decimals);
                        }

                        returnValue = decimalSumValue.ToString();
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the variance value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>Variance value of the elements in the collection</returns>
        public string GetVarianceValueString(string valueCollectionString, string seperator, int decimals, int roundingMode)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 1))//n-1
                {
                    decimal decimalValue = -9;
                    decimal decimalSumValue = 0;
                    decimal[] decimalValues = new decimal[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            decimalSumValue += decimalValue;
                            decimalValues[i] = decimalValue;
                        }
                        else
                        {
                            decimalSumValue = 0;
                            decimalValues = null;
                            break;
                        }
                    }

                    if ((decimalValues != null) && (decimalValues.Length == values.Length))
                    {
                        decimalValue = decimalSumValue / values.Length;
                        decimalSumValue = 0;

                        decimal decimalSquareValue = -9;

                        for (int j = 0; j < decimalValues.Length; j++)
                        {
                            decimalSquareValue = (decimalValues[j] - decimalValue);
                            decimalSquareValue = decimalSquareValue * decimalSquareValue;
                            decimalSumValue += decimalSquareValue;
                        }

                        //decimalSumValue = decimalSumValue / decimalValues.Length;

                        decimalSumValue = decimalSumValue / (decimalValues.Length - 1);

                        decimalSumValue = ((decimal)(Math.Sqrt(((double)(decimalSumValue)))));

                        if (decimals >= 0)
                        {
                            switch (roundingMode)
                            {
                                case (0):
                                    {
                                        decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                        break;
                                    }
                                case (1):
                                    {
                                        decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.AwayFromZero);
                                        break;
                                    }
                                default:
                                    {
                                        decimalSumValue = Decimal.Round(decimalSumValue, decimals, MidpointRounding.ToEven);
                                        break;
                                    }
                            }
                        }

                        returnValue = decimalSumValue.ToString();
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation and separator specified, and then calculates the max value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <returns>Max value of the elements in the collection</returns>
        public string GetMaxValueString(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMaxValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMaxValue = decimalValue;
                            }

                            if (decimalValue > decimalMaxValue)
                            {
                                decimalMaxValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMaxValue = -9;
                            break;
                        }
                    }
                    returnValue = decimalMaxValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the max value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>Max value of the elements in the collection</returns>
        public string GetMaxValueString(string valueCollectionString, string seperator, int decimals)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMaxValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMaxValue = decimalValue;
                            }

                            if (decimalValue > decimalMaxValue)
                            {
                                decimalMaxValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMaxValue = -9;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        decimalMaxValue = Decimal.Round(decimalMaxValue, decimals);
                    }

                    returnValue = decimalMaxValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the max value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>Max value of the elements in the collection</returns>
        public string GetMaxValueString(string valueCollectionString, string seperator, int decimals, int roundingMode)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMaxValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMaxValue = decimalValue;
                            }

                            if (decimalValue > decimalMaxValue)
                            {
                                decimalMaxValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMaxValue = -9;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        switch (roundingMode)
                        {
                            case (0):
                                {
                                    decimalMaxValue = Decimal.Round(decimalMaxValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                            case (1):
                                {
                                    decimalMaxValue = Decimal.Round(decimalMaxValue, decimals, MidpointRounding.AwayFromZero);
                                    break;
                                }
                            default:
                                {
                                    decimalMaxValue = Decimal.Round(decimalMaxValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                        }
                    }

                    returnValue = decimalMaxValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation and separator specified, and then calculates the min value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <returns>Min value of the elements in the collection</returns>
        public string GetMinValueString(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMinValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMinValue = decimalValue;
                            }

                            if (decimalValue < decimalMinValue)
                            {
                                decimalMinValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMinValue = -9;
                            break;
                        }
                    }
                    returnValue = decimalMinValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the min value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>Min value of the elements in the collection</returns>
        public string GetMinValueString(string valueCollectionString, string seperator, int decimals)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMinValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMinValue = decimalValue;
                            }

                            if (decimalValue < decimalMinValue)
                            {
                                decimalMinValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMinValue = -9;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        decimalMinValue = Decimal.Round(decimalMinValue, decimals);
                    }

                    returnValue = decimalMinValue.ToString();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts a string representation to a collection according to the string representation, separator and number of decimal places to round to specified, and then calculates the min value of the elements in the collection
        /// </summary>
        /// <param name="valueCollectionString">String representation of the collection</param>
        /// <param name="seperator">Separator in the string representation of the collection</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>Min value of the elements in the collection</returns>
        public string GetMinValueString(string valueCollectionString, string seperator, int decimals, int roundingMode)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    decimal decimalValue = -9;
                    decimal decimalMinValue = -9;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (decimal.TryParse(values[i], out decimalValue))
                        {
                            if (i == 0)
                            {
                                decimalMinValue = decimalValue;
                            }

                            if (decimalValue < decimalMinValue)
                            {
                                decimalMinValue = decimalValue;
                            }
                        }
                        else
                        {
                            decimalMinValue = -9;
                            break;
                        }
                    }

                    if (decimals >= 0)
                    {
                        switch (roundingMode)
                        {
                            case (0):
                                {
                                    decimalMinValue = Decimal.Round(decimalMinValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                            case (1):
                                {
                                    decimalMinValue = Decimal.Round(decimalMinValue, decimals, MidpointRounding.AwayFromZero);
                                    break;
                                }
                            default:
                                {
                                    decimalMinValue = Decimal.Round(decimalMinValue, decimals, MidpointRounding.ToEven);
                                    break;
                                }
                        }
                    }

                    returnValue = decimalMinValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation of a decimal to a decimal value of decimal type, and then rounds it up to the specified number of decimal places 
        /// </summary>
        /// <param name="decimalValueString">A string representation of a decimal value</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <returns>A string representation of the decimal value after rounding</returns>
        public string GetRoundedDecimalString(string decimalValueString, int decimals)
        {
            string returnValue = decimalValueString;

            decimal decimalValue = -9;

            if ((decimals >= 0) && (Decimal.TryParse(decimalValueString, out decimalValue)))
            {
                decimalValue = Decimal.Round(decimalValue, decimals);

                returnValue = decimalValue.ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string representation of a decimal to a decimal value of decimal type, and then rounds it up to the specified number of decimal places 
        /// </summary>
        /// <param name="decimalValueString">A string representation of a decimal value</param>
        /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to</param>
        /// <param name="roundingMode">A value that specifies how to round an element value if it is midway between two other numbers, regarding to the IEEE-754-4 rounding mode (0:ToEven; 1: AwayFromZero; Default: ToEven)</param>
        /// <returns>A string representation of the decimal value after rounding</returns>
        public string GetRoundedDecimalString(string decimalValueString, int decimals, int roundingMode)
        {
            string returnValue = decimalValueString;

            decimal decimalValue = -9;

            if ((decimals >= 0) && (Decimal.TryParse(decimalValueString, out decimalValue)))
            {
                switch (roundingMode)
                {
                    case (0):
                        {
                            decimalValue = Decimal.Round(decimalValue, decimals, MidpointRounding.ToEven);
                            break;
                        }
                    case (1):
                        {
                            decimalValue = Decimal.Round(decimalValue, decimals, MidpointRounding.AwayFromZero);
                            break;
                        }
                    default:
                        {
                            decimalValue = Decimal.Round(decimalValue, decimals, MidpointRounding.ToEven);
                            break;
                        }
                }

                returnValue = decimalValue.ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// Compares two strings and determines if they are eaqual
        /// </summary>
        /// <param name="comparingValue">The value of the comparing string</param>
        /// <param name="comparedValue">The value of the compared string</param>
        /// <param name="caseSensitive">Whether is case sensitive</param>
        /// <returns>Result of comparison</returns>
        public bool CompareString(string comparingValue, string comparedValue, bool caseSensitive)
        {
            bool returnValue = false;

            if (caseSensitive)
            {
                returnValue = (comparingValue.ToLower() == comparingValue.ToLower());
            }
            else if (!caseSensitive)
            {
                returnValue = (comparedValue == comparingValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Compares two string array, and determines if they are completely the same(both arrays have the same number of elements, and each element exists in both arrays)
        /// </summary>
        /// <param name="comparingStringArray">The string representation of the comparing array</param>
        /// <param name="comparedStringArray">The string representation of the compared array</param>
        /// <param name="seperator">The seperator in the string representations</param>
        /// <returns>Result of comparison</returns>
        public bool CompareStringArray(string comparingStringArray, string comparedStringArray, string seperator)
        {
            bool returnValue = false;

            string[] seperatorStringArray = new string[] { seperator };
            string[] comparingArray = comparingStringArray.Split(seperatorStringArray, StringSplitOptions.RemoveEmptyEntries);
            string[] comparedArray = comparedStringArray.Split(seperatorStringArray, StringSplitOptions.RemoveEmptyEntries);

            if ((comparingArray != null) && (comparedArray != null))
            {
                if ((comparingArray.Length > 0) && (comparedArray.Length > 0) && (comparingArray.Length <= comparedArray.Length))
                {
                    Array.Sort(comparingArray);
                    Array.Sort(comparedArray);

                    int matchCount = 0;

                    for (int i = 0; i < comparedArray.Length; i++)
                    {
                        for (int j = 0; j < comparingArray.Length; j++)
                        {
                            if (comparingArray[j] == comparedArray[i])
                            {
                                matchCount++;
                                break;
                            }
                        }
                    }

                    if ((matchCount > 0) && (matchCount >= comparingArray.Length))
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Determines if a sequence takes a character according to the mask code specified
        /// </summary>
        /// <param name="sequenceString">The string representation of the sequence</param>
        /// <param name="seperatorString">The seperator in the string representation</param>
        /// <param name="maskCode">The mask code that describes and summarizes the charater of a sequence</param>
        /// <returns>Result of computing(true: the squence matches the character that the mask code describes; false: the squence does NOT matche the character that the mask code describes)</returns>
        public bool IsSequenceMask(string sequenceString, string seperatorString, string maskCode)
        {
            bool returnValue = false;

            int maskCount = 0;

            string[] seperatorStringArray = new string[] { seperatorString };
            string[] sequenceStringArray = sequenceString.Split(seperatorStringArray, StringSplitOptions.RemoveEmptyEntries);

            if ((sequenceStringArray != null) && (sequenceStringArray.Length > 0))
            {
                for (int i = 0; i < sequenceStringArray.Length; i++)
                {
                    if (sequenceStringArray[i] == maskCode)
                    {
                        maskCount++;
                    }
                }

                if ((maskCount > 0) && (maskCount == sequenceStringArray.Length))
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the max number of duplicated elements in an array 
        /// </summary>
        /// <param name="valueCollectionString">The string representation of the array</param>
        /// <param name="seperator">The seperator in the string representation</param>
        /// <returns>The max number of duplicated elements in the array</returns>
        public string GetMaxRepeatCount(string valueCollectionString, string seperator)
        {
            string returnValue = String.Empty;

            if ((!String.IsNullOrEmpty(valueCollectionString)) && (!String.IsNullOrEmpty(seperator)))
            {
                string[] values = valueCollectionString.Split(new string[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

                if ((values != null) && (values.Length > 0))
                {
                    List<string> valueCollection = new List<string>(values);

                    if ((valueCollection != null) && (valueCollection.Count > 0))
                    {
                        valueCollection.Sort();

                        int repeatCount = 0;
                        string currentValue = String.Empty;

                        List<int> repeatCounts = new List<int>();

                        for (int i = 0; i < valueCollection.Count; i++)
                        {
                            if (i == 0)
                            {
                                currentValue = valueCollection[i];
                            }

                            if (currentValue == valueCollection[i])
                            {
                                repeatCount++;
                            }
                            else if ((currentValue != valueCollection[i]) && (i < (valueCollection.Count - 1)))
                            {
                                repeatCounts.Add(repeatCount);

                                currentValue = valueCollection[i];
                                repeatCount = 1;
                            }

                            if (i == (valueCollection.Count - 1))
                            {
                                repeatCounts.Add(repeatCount);

                                currentValue = valueCollection[i];
                                repeatCount = 1;
                            }
                        }

                        if ((repeatCounts != null) && (repeatCounts.Count > 0))
                        {
                            repeatCounts.Sort();

                            repeatCount = 0;

                            for (int i = 0; i < repeatCounts.Count; i++)
                            {
                                if (repeatCounts[i] > repeatCount)
                                {
                                    repeatCount = repeatCounts[i];
                                }

                            }

                            returnValue = repeatCount.ToString();
                        }
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Generates a random number
        /// </summary>
        /// <returns>The random number generated</returns>
        public string GetRandomNumberString()
        {
            string returnValue = String.Empty;

            returnValue = new Random().Next().ToString();

            return returnValue;
        }

        /// <summary>
        /// Converts a date time value to a millisecond value with the reference starting date specified 
        /// </summary>
        /// <param name="dateTimeString">The string representation of the date time value to be converted</param>
        /// <param name="refDateTimeString">The string representation of the reference date time value(The default value is: 1970-01-01 00:00:00)</param>
        /// <returns>The milliseconds between the date time value and the reference date time value</returns>
        public string GetMillisecondsByDateTime(string dateTimeString, string refDateTimeString)
        {
            string returnValue = String.Empty;

            if (String.IsNullOrEmpty(refDateTimeString))
            {
                refDateTimeString = "1970-01-01 00:00:00";
            }

            if ((!String.IsNullOrEmpty(dateTimeString)) && (!String.IsNullOrEmpty(refDateTimeString)))
            {
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;

                if ((DateTime.TryParse(dateTimeString, out end)) && (DateTime.TryParse(refDateTimeString, out start)))
                {
                    TimeSpan span = end.Subtract(start);

                    returnValue = span.TotalMilliseconds.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts the current date time value to a millisecond value with the reference starting date specified 
        /// </summary>
        /// <param name="refDateTimeString">The string representation of the reference date time value(The default value is: 1970-01-01 00:00:00)</param>
        /// <returns>The milliseconds between the current date time value and the reference date time value</returns>
        public string GetMillisecondsOfCurrentDateTime(string refDateTimeString)
        {
            string returnValue = String.Empty;

            if (String.IsNullOrEmpty(refDateTimeString))
            {
                refDateTimeString = "1970-01-01 00:00:00";
            }

            if (!String.IsNullOrEmpty(refDateTimeString))
            {
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;

                if ((DateTime.TryParse(refDateTimeString, out start)))
                {
                    TimeSpan span = end.Subtract(start);

                    returnValue = span.TotalMilliseconds.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Get the string representation of the current date time point with the sepcified date time format pattern expression
        /// </summary>
        /// <param name="format">Date time format pattern expression</param>
        /// <returns>The string representation of the current date time point in the format of sepcified date time format pattern expression</returns>
        public string GetCurrentDateTimeStringByPattern(string format)
        {
            DateTime currentDateTime = DateTime.Now;

            return currentDateTime.ToString(format);
        }

        /// <summary>
        /// Converts a GUID to integer 
        /// </summary>
        /// <param name="guidValue">The value of the GUID</param>
        /// <returns>The integer after conversion</returns>
        public string GetIntByGUID(string guidValue)
        {
            string returnValue = String.Empty;

            if (!String.IsNullOrEmpty(guidValue))
            {
                Guid guid = new Guid(guidValue);

                if (guid != null)
                {
                    byte[] guidBytes = guid.ToByteArray();

                    if ((guidBytes != null) && (guidBytes.Length > 0))
                    {
                        int intValue = BitConverter.ToInt32(guidBytes, 0);

                        returnValue = intValue.ToString();
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string to an integer
        /// </summary>
        /// <param name="value">The value of the string</param>
        /// <returns>The integer afer conversion</returns>
        public string GetIntByString(string value)
        {
            string returnValue = String.Empty;

            if (!String.IsNullOrEmpty(value))
            {
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);

                if ((valueBytes != null) && (valueBytes.Length > 0))
                {
                    int intValue = BitConverter.ToInt32(valueBytes, 0);

                    returnValue = intValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Generates a new GUID 
        /// </summary>
        /// <returns>The new GUID generated</returns>
        public string GetNewGUID() 
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Get the base64 string of the bytes of the string with the default encoding
        /// </summary>
        /// <param name="value">Value of the original string</param>
        /// <returns>The base64 string of the bytes of the string with the default encoding</returns>
        public string GetStringBytesValueBase64String(string value) 
        {
            if (!String.IsNullOrEmpty (value))
            {
                byte[] bytes = Encoding.Default.GetBytes(value);

                return Convert.ToBase64String(bytes);
            }

            return "";
        }

        /// <summary>
        /// Get the base64 string of the bytes of an XML node(including its child nodes) with the default encoding
        /// </summary>
        /// <param name="node">An XML node(including its child nodes)</param>
        /// <returns>The base64 string of the bytes of the string of the inner XML of the XML node with the default encoding</returns>
        public string GetXmlNodeBytesValueBase64String(XPathNavigator node) 
        {
            string xml = node.InnerXml; 


            if (!String.IsNullOrEmpty(xml))
            {
                byte[] bytes = Encoding.Default.GetBytes(xml);

                return Convert.ToBase64String(bytes);
            }

            return "";
        }

        /// <summary>
        /// Get the base64 string of the bytes of an XML node(including its child nodes) with the default encoding
        /// </summary>
        /// <param name="node">An XML node(including its child nodes)</param>
        /// <param name="encodingName">The encoding name indicating the encoding of the output content. A table listing all available encoding names and their relative code page numbers can be found at :  http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx </param>
        /// <returns>The base64 string of the bytes of the string of the inner XML of the XML node with the default encoding</returns>
        public string GetXmlNodeBytesValueBase64String(XPathNavigator node, string encodingName)
        {
            string xml = node.InnerXml;

            if (!String.IsNullOrEmpty(xml))
            {
                Encoding encoding = String.IsNullOrEmpty(encodingName) ? Encoding.Default : Encoding.GetEncoding(encodingName);

                byte[] bytes = encoding.GetBytes(xml);

                return Convert.ToBase64String(bytes);
            }

            return "";
        }
    }
}
