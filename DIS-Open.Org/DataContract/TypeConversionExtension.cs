using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
    public static class TypeConversionExtension
    {
        public static List<KeyInfo> ToKeys(this IEnumerable<OhrKey> ohrKeys)
        {
            return ohrKeys.GroupBy(o => o.KeyId).Select(item => new KeyInfo()
                {
                    KeyId = item.Key,
                    OemOptionalInfo = item.ToOemOptionInfo(),
                }).ToList();
        }

        public static OemOptionalInfo ToOemOptionInfo(this IEnumerable<OhrKey> ohrKeys)
        {
            var dic = ohrKeys.ToDictionary(k => k.Name, v => v.Value);

            OemOptionalInfo info = new OemOptionalInfo();
            if (dic.ContainsKey(OemOptionalInfo.ZFrmFactorCl1Name))
                info.ZFRM_FACTOR_CL1 = dic[OemOptionalInfo.ZFrmFactorCl1Name];
            if (dic.ContainsKey(OemOptionalInfo.ZFrmFactorCl2Name))
                info.ZFRM_FACTOR_CL2 = dic[OemOptionalInfo.ZFrmFactorCl2Name];
            if (dic.ContainsKey(OemOptionalInfo.ZTouchScreenName))
                info.ZTOUCH_SCREEN = dic[OemOptionalInfo.ZTouchScreenName];
            if (dic.ContainsKey(OemOptionalInfo.ZScreenSizeName))
                info.ZSCREEN_SIZE = dic[OemOptionalInfo.ZScreenSizeName];
            if (dic.ContainsKey(OemOptionalInfo.ZPcModelSkuName))
                info.ZPC_MODEL_SKU = dic[OemOptionalInfo.ZPcModelSkuName];
            //if (dic.ContainsKey(OemOptionalInfo.ZOemExtIdName))
            //    info.ZOEM_EXT_ID = dic[OemOptionalInfo.ZOemExtIdName];
            //if (dic.ContainsKey(OemOptionalInfo.ZManufGeoLocName))
            //    info.ZMANUF_GEO_LOC = dic[OemOptionalInfo.ZManufGeoLocName];
            //if (dic.ContainsKey(OemOptionalInfo.ZPgmEligValuesName))
            //    info.ZPGM_ELIG_VALUES = dic[OemOptionalInfo.ZPgmEligValuesName];
            //if (dic.ContainsKey(OemOptionalInfo.ZChannelRelIdName))
            //    info.ZCHANNEL_REL_ID = dic[OemOptionalInfo.ZChannelRelIdName];

            return info;
        }

        public static List<OhrKey> ToOhrKeys(this List<KeyInfo> keys)
        {
            return keys.SelectMany(k => k.ToOhrKey()).ToList();
        }

        public static List<OhrKey> ToOhrKey(this KeyInfo key)
        {
            List<OhrKey> result = new List<OhrKey>();
            result.Add(new OhrKey(key.KeyId));

            if (key.OemOptionalInfo != null)
            {
                result.AddRange(FromOemOptionalInfo(key.KeyId, key.OemOptionalInfo));
            }

            return result;
        }

        private static List<OhrKey> FromOemOptionalInfo(long keyId, OemOptionalInfo info)
        {
            List<OhrKey> result = new List<OhrKey>();

            //if (!string.IsNullOrEmpty(info.ZCHANNEL_REL_ID))
            //{
            //    result.Add(new OhrKey(keyId, OemOptionalInfo.ZChannelRelIdName, info.ZCHANNEL_REL_ID));
            //}
            //if (!string.IsNullOrEmpty(info.ZMANUF_GEO_LOC))
            //{
            //    result.Add(new OhrKey(keyId, OemOptionalInfo.ZManufGeoLocName, info.ZMANUF_GEO_LOC));
            //}
            //if (!string.IsNullOrEmpty(info.ZOEM_EXT_ID))
            //{
            //    result.Add(new OhrKey(keyId, OemOptionalInfo.ZOemExtIdName, info.ZOEM_EXT_ID));
            //}
            //if (!string.IsNullOrEmpty(info.ZPGM_ELIG_VALUES))
            //{
            //    result.Add(new OhrKey(keyId, OemOptionalInfo.ZPgmEligValuesName, info.ZPGM_ELIG_VALUES));
            //}
            if (!string.IsNullOrEmpty(info.ZFRM_FACTOR_CL1))
            {
                result.Add(new OhrKey(keyId, OemOptionalInfo.ZFrmFactorCl1Name, info.ZFRM_FACTOR_CL1));
            }
            if (!string.IsNullOrEmpty(info.ZFRM_FACTOR_CL2))
            {
                result.Add(new OhrKey(keyId, OemOptionalInfo.ZFrmFactorCl2Name, info.ZFRM_FACTOR_CL2));
            }
            if (!string.IsNullOrEmpty(info.ZTOUCH_SCREEN))
            {
                result.Add(new OhrKey(keyId, OemOptionalInfo.ZTouchScreenName, info.ZTOUCH_SCREEN));
            }
            if (!string.IsNullOrEmpty(info.ZSCREEN_SIZE))
            {
                result.Add(new OhrKey(keyId, OemOptionalInfo.ZScreenSizeName, info.ZSCREEN_SIZE));
            }
            if (!string.IsNullOrEmpty(info.ZPC_MODEL_SKU))
            {
                result.Add(new OhrKey(keyId, OemOptionalInfo.ZPcModelSkuName, info.ZPC_MODEL_SKU));
            }
            return result;
        }
    }
}
