//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Text.RegularExpressions;

namespace DIS.Data.DataContract
{
    public class OemOptionalInfo : IXmlSerializable
    {
        public const string ZPcModelSkuName = "ZPC_MODEL_SKU";
        public const string ZOemExtIdName = "ZOEM_EXT_ID";
        public const string ZManufGeoLocName = "ZMANUF_GEO_LOC";
        public const string ZPgmEligValuesName = "ZPGM_ELIG_VAL";
        public const string ZChannelRelIdName = "ZCHANNEL_REL_ID";
        //new add for V1.6
        public const string ZFrmFactorCl1Name = "ZFRM_FACTOR_CL1";
        public const string ZFrmFactorCl2Name = "ZFRM_FACTOR_CL2";
        public const string ZScreenSizeName = "ZSCREEN_SIZE";
        public const string ZTouchScreenName = "ZTOUCH_SCREEN";

        private const string rootXmlElementName = "OEMOptionalInfo";
        private const string nameXmlElementName = "Name";
        private const string valueXmlElementName = "Value";
        private const string fieldXmlElementName = "Field";
        private const string NumbericRegEx = "^[0-9]+$";
        private const string NonTounchRegEx = @"^Non[\s-]?Touch$";
        private static Regex NonTounchReg = new Regex(NonTounchRegEx, RegexOptions.IgnoreCase);
        private bool skipCheckIfLoadedFromDb;

        private XElement xml
        {
            get
            {
                return new XElement(rootXmlElementName,
                    from p in Values
                    where !string.IsNullOrEmpty(p.Value)
                    select new XElement(fieldXmlElementName,
                        new XElement(nameXmlElementName, p.Key),
                        new XElement(valueXmlElementName, p.Value)));
            }
            set
            {
                List<XElement> fields = value.Elements(fieldXmlElementName).ToList();
                foreach (XElement f in fields)
                {
                    SetField(f.Element(nameXmlElementName).Value, f.Element(valueXmlElementName).Value);
                }
            }
        }

        public string ZPC_MODEL_SKU
        {
            get
            {
                return Values[ZPcModelSkuName];
            }
            set
            {
                SetField(ZPcModelSkuName, value);
            }
        }

        public string ZOEM_EXT_ID
        {
            get
            {
                return Values[ZOemExtIdName];
            }
            set
            {
                SetField(ZOemExtIdName, value);
            }
        }

        public string ZMANUF_GEO_LOC
        {
            get
            {
                return Values[ZManufGeoLocName];
            }
            set
            {
                SetField(ZManufGeoLocName, value);
            }
        }

        public string ZPGM_ELIG_VALUES
        {
            get
            {
                return Values[ZPgmEligValuesName];
            }
            set
            {
                SetField(ZPgmEligValuesName, value);
            }
        }

        public string ZCHANNEL_REL_ID
        {
            get
            {
                return Values[ZChannelRelIdName];
            }
            set
            {
                SetField(ZChannelRelIdName, value);
            }
        }

        //add for V1.6
        public string ZFRM_FACTOR_CL1
        {
            get { return Values[ZFrmFactorCl1Name]; }
            set { SetField(ZFrmFactorCl1Name, value); }
        }

        public string ZFRM_FACTOR_CL2
        {
            get { return Values[ZFrmFactorCl2Name]; }
            set { SetField(ZFrmFactorCl2Name, value); }
        }

        public string ZSCREEN_SIZE
        {
            get { return Values[ZScreenSizeName]; }
            set { SetField(ZScreenSizeName, value); }
        }

        public string ZTOUCH_SCREEN
        {
            get { return Values[ZTouchScreenName]; }
            set { SetField(ZTouchScreenName, value); }
        }

        public bool HasOHRData
        {
            get 
            {
                return !(string.IsNullOrEmpty(ZFRM_FACTOR_CL1) || string.IsNullOrEmpty(ZFRM_FACTOR_CL2) || string.IsNullOrEmpty(ZSCREEN_SIZE) || string.IsNullOrEmpty(ZTOUCH_SCREEN)||string.IsNullOrEmpty(ZPC_MODEL_SKU));
            }
        }

        public Dictionary<string, string> Values { get; private set; }

        public OemOptionalInfo()
            : this(null, null, null, null, null, null, null, null, null)
        {
        }

        public OemOptionalInfo(string xml)
            : this(null, null, null, null, null, null, null, null, null)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                skipCheckIfLoadedFromDb = false; // import data from external.
                this.xml = XElement.Parse(xml);
            }
        }

        public OemOptionalInfo(string zPcModelSku, string zOemExtId,
          string zManufGeoLoc, string zPgmEligValues, string zChannelRelId, string zFrmFactor1,
          string zFrmFactor2, string screenSize, string touchScreen)
        {
            skipCheckIfLoadedFromDb = true;

            Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ZPC_MODEL_SKU = zPcModelSku;
            ZOEM_EXT_ID = zOemExtId;
            ZMANUF_GEO_LOC = zManufGeoLoc;
            ZPGM_ELIG_VALUES = zPgmEligValues;
            ZCHANNEL_REL_ID = zChannelRelId;

            ZFRM_FACTOR_CL1 = zFrmFactor1;
            ZFRM_FACTOR_CL2 = zFrmFactor2;
            ZSCREEN_SIZE = screenSize;
            ZTOUCH_SCREEN = touchScreen;
        }

        public override string ToString()
        {
            return xml.ToString();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            xml = XElement.Parse(reader.ReadInnerXml());
        }

        public void WriteXml(XmlWriter writer)
        {
            xml.WriteTo(writer);
        }

        public List<Field> ToFields()
        {
            List<Field> fields = new List<Field>();
            if (!string.IsNullOrEmpty(ZPC_MODEL_SKU))
                fields.Add(new Field() { Name = ZPcModelSkuName, Value = ZPC_MODEL_SKU });
            if (!string.IsNullOrEmpty(ZMANUF_GEO_LOC))
                fields.Add(new Field() { Name = ZManufGeoLocName, Value = ZMANUF_GEO_LOC });
            if (!string.IsNullOrEmpty(ZPGM_ELIG_VALUES))
                fields.Add(new Field() { Name = ZPgmEligValuesName, Value = ZPGM_ELIG_VALUES });
            if (!string.IsNullOrEmpty(ZOEM_EXT_ID))
                fields.Add(new Field() { Name = ZOemExtIdName, Value = ZOEM_EXT_ID });
            if (!string.IsNullOrEmpty(ZCHANNEL_REL_ID))
                fields.Add(new Field() { Name = ZChannelRelIdName, Value = ZCHANNEL_REL_ID });

            if (!string.IsNullOrEmpty(ZFRM_FACTOR_CL1))
                fields.Add(new Field() { Name = ZFrmFactorCl1Name, Value = ZFRM_FACTOR_CL1 });
            if (!string.IsNullOrEmpty(ZFRM_FACTOR_CL2))
                fields.Add(new Field() { Name = ZFrmFactorCl2Name, Value = ZFRM_FACTOR_CL2 });
            if (!string.IsNullOrEmpty(ZSCREEN_SIZE))
                fields.Add(new Field() { Name = ZScreenSizeName, Value = ZSCREEN_SIZE });
            if (!string.IsNullOrEmpty(ZTOUCH_SCREEN))
                fields.Add(new Field() { Name = ZTouchScreenName, Value = ZTOUCH_SCREEN });
            return fields;
        }

        public OemOptionalInfo(List<Field> fields)
            : this()
        {
            skipCheckIfLoadedFromDb = false; // import data from external.
            foreach (Field field in fields)
            {
                SetField(field.Name, field.Value);
            }
        }

        public static TouchEnum ConvertTouchEnum(string value)
        {
            if (string.Compare(value.Trim(), TouchEnum.Touch.ToString(), ignoreCase: true) == 0)
                return TouchEnum.Touch;
            else if (NonTounchReg.IsMatch(value.Trim()))
                return TouchEnum.Nontouch;
            else
                throw new ApplicationException(string.Format("{0} is not valid format.", ZTouchScreenName));
        }

        private void SetField(string fieldName, string value)
        {
            if (!skipCheckIfLoadedFromDb) // import from external
            {
                switch (fieldName.ToUpper())
                {
                    case ZOemExtIdName:
                        if (!string.IsNullOrEmpty(value) &&
                            !System.Text.RegularExpressions.Regex.IsMatch(value, NumbericRegEx))
                            throw new ApplicationException("OEM Extended Identifier value required numeric characters.");
                        break;
                    case ZFrmFactorCl1Name:
                        if (!string.IsNullOrEmpty(value))
                        {
                            FormFactorL1Enum outzFrmFactorCl1;
                            bool isValidFrmFactorCl1 = Enum.TryParse<FormFactorL1Enum>(value, true, out outzFrmFactorCl1);
                            if (!isValidFrmFactorCl1)
                                throw new ApplicationException(string.Format("{0} is not valid format.", ZFrmFactorCl1Name));

                            value = outzFrmFactorCl1.ToString();
                        }
                        break;
                    case ZFrmFactorCl2Name:
                        if (!string.IsNullOrEmpty(value))
                        {
                            FormFactorL2Enum outzFrmFactorCl2;
                            bool isValidFrmFactorCl2 = Enum.TryParse<FormFactorL2Enum>(value, true, out outzFrmFactorCl2);
                            if (!isValidFrmFactorCl2)
                                throw new ApplicationException(string.Format("{0} is not valid format.", ZFrmFactorCl2Name));

                            value = outzFrmFactorCl2.ToString();
                        }
                        break;
                    case ZTouchScreenName:
                        if (!string.IsNullOrEmpty(value))
                        {
                            TouchEnum outTouchEnum = ConvertTouchEnum(value);
                            value = outTouchEnum.ToString();
                        }
                        break;
                    case ZScreenSizeName:
                        if (!string.IsNullOrEmpty(value))
                        {
                            Decimal zScreenSizeOut;
                            bool isValidDecimal = Decimal.TryParse(value, out zScreenSizeOut);
                            if (!isValidDecimal)
                                throw new ApplicationException(string.Format("{0} is not valid decimal format.", ZScreenSizeName));
                        }
                        break;
                }
            }

            if (Values.ContainsKey(fieldName))
                Values[fieldName] = value;
            else
                Values.Add(fieldName, value);
        }
    }

    //add for OHR data
    public static class OHRData
    {
        private static Dictionary<FormFactorL1Enum, List<FormFactorL2Enum>> zfrm_factorValue;
        private static List<TouchEnum> zTOUCH_SCREENValue;

        static OHRData()
        {
            zfrm_factorValue = zfrm_factorValue = new Dictionary<FormFactorL1Enum, List<FormFactorL2Enum>>()
                    {
                        { FormFactorL1Enum.Desktop, new List<FormFactorL2Enum>{FormFactorL2Enum.Standard, FormFactorL2Enum.AIO}},
                        { FormFactorL1Enum.Notebook, new List<FormFactorL2Enum>{FormFactorL2Enum.Standard, FormFactorL2Enum.Ultraslim, FormFactorL2Enum.Convertible}},
                        { FormFactorL1Enum.Tablet, new List<FormFactorL2Enum>{FormFactorL2Enum.Standard}},
                    };

            zTOUCH_SCREENValue = new List<TouchEnum>() { TouchEnum.Touch, TouchEnum.Nontouch };
        }

        public static Dictionary<FormFactorL1Enum, List<FormFactorL2Enum>> ZFRM_FACTORValue
        {
            get
            {
                return zfrm_factorValue;
            }
        }

        public static List<TouchEnum> ZTOUCH_SCREENValue
        {
            get 
            {
                return zTOUCH_SCREENValue;
            }
        }
    }


}
