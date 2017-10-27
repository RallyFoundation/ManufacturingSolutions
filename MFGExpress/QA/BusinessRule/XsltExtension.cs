using System;
using System.Linq;
using System.Collections.Generic;

namespace HardwareHashValidation
{
    public class XsltExtension
    {
        public XsltExtension()
        {
        }

        private Dictionary<string, string> EnclosureTypeByteValueMappings = new Dictionary<string, string>()
        {
            {"Desktop", "0x03"},
            {"Notebook", "0x0A"},
            {"All-in-One", "0x0D"},
            {"Tablet", "0x1E"},
            {"Convertible", "0x1F"},
            {"Detachable", "0x20"},
            {"Stick PC", "0x24"},
            {"Mini PC", "0x23"},
            {"IoT Gateway", "0x21"},
            {"Embedded PC", "0x22"}
        };

        private Dictionary<string, string> ByteValueEnclosureTypeMappings = new Dictionary<string, string>()
        {
            {"0x03", "Desktop"},
            {"0x0A", "Notebook"},
            {"0x0D", "All-in-One"},
            {"0x1E", "Tablet"},
            {"0x1F", "Convertible"},
            {"0x20", "Detachable"},
            {"0x24", "Stick PC"},
            {"0x23", "Mini PC"},
            {"0x21", "IoT Gateway"},
            {"0x22", "Embedded PC"}
        };

        public string GetHtmlSpacedString(string inputValue)
        {
            return inputValue.Replace(" ", "&nbsp;");
        }

        public bool IsValueInStringArray(string value, string arrayString, string separator = ",")
        {
            if (String.IsNullOrEmpty(separator))
            {
                separator = ",";
            }

            string[] array = arrayString.Split(new string[] { separator }, StringSplitOptions.None);

            return array.Contains(value);
        }

        public string GetEnclosureType(string byteValue)
        {
            if (!this.ByteValueEnclosureTypeMappings.ContainsKey(byteValue))
            {
                return "";
            }

            return this.ByteValueEnclosureTypeMappings[byteValue];
        }

        public string GetEnclosureByteValue(string enclosureType)
        {
            if (!this.EnclosureTypeByteValueMappings.ContainsKey(enclosureType))
            {
                return "";
            }

            return this.EnclosureTypeByteValueMappings[enclosureType];
        }

        public string GetLowerCase(string inputValue)
        {
            return !String.IsNullOrEmpty(inputValue) ? inputValue.ToLower() : "";
        }

        public string GetUpperCase(string inputValue)
        {
            return !String.IsNullOrEmpty(inputValue) ? inputValue.ToUpper() : "";
        }

        public bool CompareNumberSequence(string comparingSequenceString, string comparedSequenceString, string separator, int index)
        {
            string[] comparingArray = comparingSequenceString.Split(new string[] { separator }, StringSplitOptions.None);
            string[] comparedArray = comparedSequenceString.Split(new string[] { separator }, StringSplitOptions.None);

            int minLength = (index + 1);

            if (comparedArray == null || comparingArray == null || comparingArray.Length < minLength || comparedArray.Length < minLength)
            {
                return false;
            }

            int comparingValue = -1, comparedValue = -1;

            if (!int.TryParse(comparingArray[index], out comparingValue) || !int.TryParse(comparedArray[index], out comparedValue))
            {
                return false;
            }

            if (comparedValue >= comparingValue)
            {
                return true;
            }

            return false;
        } 
    }
}


