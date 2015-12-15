using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ViewModel
{
    public class OhrKeyInfo
    {
        public long KeyId { get; set; }
        public KeyDescription KeyInfoId { get; set; }
        public string ProductKey { get; set; }
        public KeyDescription ZFRM_FACTOR_CL1 { get; set; }
        public KeyDescription ZFRM_FACTOR_CL2 { get; set; }
        public KeyDescription ZSCREEN_SIZE { get; set; }
        public KeyDescription ZTOUCH_SCREEN { get; set; }
        public KeyDescription ZPC_MODEL_SKU { get; set; }
        public void SetValue(OhrKey ohrKey)
        {
            switch (ohrKey.Name)
            {
                case OhrName.ProductKeyID:
                    SetField(ohrKey, KeyInfoId);
                    break;
                case OemOptionalInfo.ZFrmFactorCl1Name:
                    SetField(ohrKey, ZFRM_FACTOR_CL1);
                    break;
                case OemOptionalInfo.ZFrmFactorCl2Name:
                    SetField(ohrKey, ZFRM_FACTOR_CL2);
                    break;
                case OemOptionalInfo.ZTouchScreenName:
                    SetField(ohrKey, ZTOUCH_SCREEN);
                    break;
                case OemOptionalInfo.ZScreenSizeName:
                    SetField(ohrKey, ZSCREEN_SIZE);
                    break;
                case OemOptionalInfo.ZPcModelSkuName:
                    SetField(ohrKey, ZPC_MODEL_SKU);
                    break;
            }   
        }

        private void SetField(OhrKey ohrKey, KeyDescription field)
        {
            if (ohrKey.Ohr.MsReceivedDate >= field.MsReceivedDateUtc)
            {
                field.Value = ohrKey.Value;
                field.ReasonCode = ohrKey.ReasonCode;
                field.ReasonCodeDescription = ohrKey.ReasonCodeDescription;
            }
        }
    }

    public class KeyDescription
    {
        public KeyDescription(string value)
        {
            ReasonCode = Constants.CBRAckReasonCode.ActivationEnabled;
            Value = value;
            MsReceivedDateUtc = DateTime.MinValue;
        }

        public DateTime MsReceivedDateUtc { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonCodeDescription { get; set; }
        public string Value { get; set; }
        public string Description
        {
            get
            {
                return string.Format("{0}/{1}", ReasonCode, ReasonCodeDescription);
            }
        }
    }
}
