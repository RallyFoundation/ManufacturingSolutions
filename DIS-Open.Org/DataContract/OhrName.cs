using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
    public static class OhrName
    {
        // Convert name ZFRM_FACTOR_CL1 to FRM_FACTOR_CL1
        public static string FRMFACTORCL1 = OemOptionalInfo.ZFrmFactorCl1Name.Substring(1);
        // Convert name ZFRM_FACTOR_CL2 to FRM_FACTOR_CL2
        public static string FRMFACTORCL2 = OemOptionalInfo.ZFrmFactorCl2Name.Substring(1);
        // Convert name ZTOUCH_SCREEN to TOUCH_SCREEN
        public static string TOUCHSCREEN = OemOptionalInfo.ZTouchScreenName.Substring(1);
        // Convert name ZSCREEN_SIZE to SCREEN_SIZE
        public static string SCREENSIZE = OemOptionalInfo.ZScreenSizeName.Substring(1);
        // Convert name ZPC_MODEL_SKU to PC_MODEL_SKU
        public static string PCMODELSKU = OemOptionalInfo.ZPcModelSkuName.Substring(1);

        public static string ZOEMEXTID = OemOptionalInfo.ZOemExtIdName;
        public static string ZMANUFGEOLOC = OemOptionalInfo.ZManufGeoLocName;
        public static string ZPGMELIGVAL = OemOptionalInfo.ZPgmEligValuesName;
        public static string ZCHANNELRELID = OemOptionalInfo.ZChannelRelIdName;
        public static string ZPCMODELSKU = OemOptionalInfo.ZPcModelSkuName;

        public const string ProductKeyID = "ProductKeyID";
    }
}
