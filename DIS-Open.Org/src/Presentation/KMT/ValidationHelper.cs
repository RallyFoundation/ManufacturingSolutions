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
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;
using DIS.Business.Library;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel;
using Microsoft.Http;
using Microsoft.ServiceModel.Web;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// 
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetTraceText(this Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            while (null != ex)
            {
                if (builder.Length > 1)
                {
                    builder.AppendLine()
                        .AppendLine("-: Inner Exception :-")
                        .AppendLine();
                }
                builder.AppendLine(ex.Message)
                    .AppendLine(ex.StackTrace);

                ex = ex.InnerException;
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="displayMessage"></param>
        public static void ShowDialog(this Exception ex, string displayMessage = null)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                Window parent = ViewModelBase.GetCurrentWindow();

                if (ex is System.UnauthorizedAccessException)
                {
                    ShowMessageBox(MergedResources.AccessFileDenied, parent);
                }
                else if (ex is DisException)
                {
                    ShowMessageBox(((DisException)ex).GetErrorMessage(), parent);
                }
                else if (ex is EntityException && ex.InnerException is SqlException)
                {
                    ShowMessageBox(MergedResources.Exception_FailedToConnectDB, parent);
                }
                else if (ex is HttpStageProcessingException && ex.InnerException is WebException)
                {
                    ShowMessageBox(MergedResources.Exception_NetworkFailed, parent);
                }
                else if (ex is HttpStageProcessingException && ex.InnerException is TimeoutException)
                {
                    ShowMessageBox(MergedResources.WebServiceHostOrPortInvalid, parent);
                }
                else if (ex is WebProtocolException)
                {
                    ShowMessageBox(string.Format(MergedResources.Common_InternalErrorFormat, ((WebProtocolException)ex).StatusCode), parent);
                }
                else if (!string.IsNullOrEmpty(displayMessage))
                {
                    ShowMessageBox(displayMessage, parent);
                }
                else if (!string.IsNullOrEmpty(ex.Message))
                {
                    ShowMessageBox(ResourcesOfR6.Common_GeneralExceptionMsg, parent);
                }
                else
                {
                    Action show = () =>
                    {
                        ErrorDialog error = new ErrorDialog(ex);
                        error.Owner = parent;
                        error.ShowDialog();
                    };
                    if (parent != null)
                        parent.Dispatcher.Invoke(show);
                    else
                        show();
                }
            }));
        }

        public static void ShowMessageBox(string displayMessage, Window parent)
        {
            if (parent == null)
                MessageBox.Show(displayMessage, MergedResources.Common_Error);
            else
                MessageBox.Show(parent, displayMessage, MergedResources.Common_Error);
        }

        public static void ShowMessageBox(string displayMessage, string title)
        {
            Window parent = ViewModelBase.GetCurrentWindow();
            if (parent == null)
                MessageBox.Show(displayMessage, title);
            else
                MessageBox.Show(parent, displayMessage, title);
        }

        public static string GetWebServiceUrl(string host, string port)
        {
            int portNum;
            if (!int.TryParse(port, out portNum) || portNum <= 0 || portNum > 65535)
            {
                ValidationHelper.ShowMessageBox(MergedResources.WebServicePortIsInvalid, MergedResources.Common_Error);
            }
            else
            {
                try
                {
                    string urlString = string.Format(KmtConstants.HttpsUrlFormat, host, port);
                    Uri uri = new Uri(urlString);
                    if (HttpUtility.UrlEncode(urlString).ToLower() != HttpUtility.UrlEncode(uri.ToString()).ToLower())
                        throw new UriFormatException();
                    return uri.ToString();
                }
                catch (UriFormatException)
                {
                    ValidationHelper.ShowMessageBox(MergedResources.WebServiceHostIsInvalid, MergedResources.Common_Error);
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <returns></returns>
        public static string GetProxyUrl(string host, string port, bool useSsl)
        {
            int portNum;
            if (!int.TryParse(port, out portNum) || portNum <= 0 || portNum > 65535)
            {
                ValidationHelper.ShowMessageBox(MergedResources.WebServicePortIsInvalid, MergedResources.Common_Error);
            }
            else
            {
                try
                {
                    Uri uri = new Uri(string.Format(useSsl ?
                        "Https://{0}:{1}" : "http://{0}:{1}", host, port));
                    return uri.ToString();
                }
                catch (UriFormatException)
                {
                    ValidationHelper.ShowMessageBox(MergedResources.WebServiceHostIsInvalid, MergedResources.Common_Error);
                }
            }
            return null;
        }

        /// <summary>
        /// Validates date range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool ValidateDateRange(DateTime? from, DateTime? to)
        {

            bool isValid = false;
            if (from.HasValue && to.HasValue)
            {
                isValid = (from <= to);
                if (!isValid)
                {
                    ValidationHelper.ShowMessageBox(MergedResources.DateRangeInvalidMessage, MergedResources.Common_Error);
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }
    }
}
