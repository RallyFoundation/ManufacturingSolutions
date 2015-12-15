using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace DIS.Business.Library
{
    [Serializable]
    public class DisException : Exception
    {
        #region Property

        /// <summary>
        /// The Error Code defined by resources file
        /// </summary>
        public string ErrorCode { get; private set; }

        #endregion

        #region Contructor

        public DisException()
            : base()
        {
        }

        public DisException(string errorCode)
            : base(errorCode)
        {
            ErrorCode = errorCode;
        }

        public DisException(string errorCode, Exception innerException)
            : base(errorCode, innerException)
        {
            ErrorCode = errorCode;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Get Error Message from resources file
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            string result = null;
            try
            {
                ResourceManager rm = new ResourceManager("DIS.Business.Library.Properties.Resources",
                         Assembly.GetAssembly(typeof(DisException)));
                result = rm.GetString(ErrorCode, System.Globalization.CultureInfo.CurrentCulture);
                if (string.IsNullOrEmpty(result))
                {
                    rm = new ResourceManager("DIS.Business.Library.Properties.ResourcesOfR6",
                       Assembly.GetAssembly(typeof(DisException)));
                    result = rm.GetString(ErrorCode, System.Globalization.CultureInfo.CurrentCulture);
                    if (string.IsNullOrEmpty(result))
                        result = ErrorCode;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
