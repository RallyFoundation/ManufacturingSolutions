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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Presentation.KMT.Models;
using System.Security.Cryptography.X509Certificates;
using DIS.Common.Utility;
using DIS.Presentation.KMT.Commands;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ViewModel
{
    public class CertPickerViewModel : ViewModelBase
    {
        private DisCertView selectedCert;
        private List<DisCertView> certs;
        private Dictionary<string, string> certDetails;
        private bool isBusy;
        private DelegateCommand saveCommand;
        private IConfigProxy proxy;
        private IHeadQuarterProxy hqProxy;

        public CertPickerViewModel(IConfigProxy proxyParam, IHeadQuarterProxy hqProxyParam)
        {
            if (proxy == null)
                proxy = new ConfigProxy(KmtConstants.LoginUser);
            else
                proxy = proxyParam;
            if (hqProxyParam == null)
                hqProxy = new HeadQuarterProxy();
            else
                hqProxy = hqProxyParam;
            this.LoadCerts();
        }

        public ICommand SelectCertCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new DelegateCommand(() =>
                    {
                        this.SaveCert();
                    });
                return saveCommand;
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public List<DisCertView> Certs
        {
            get { return this.certs; }
            set
            {
                this.certs = value;
                RaisePropertyChanged("Certs");
            }
        }

        public Dictionary<string, string> CertDetails
        {
            get { return this.certDetails; }
            set
            {
                this.certDetails = value;
                RaisePropertyChanged("CertDetails");
            }
        }

        public DisCertView SelectedCert
        {
            get { return this.selectedCert; }
            set
            {
                this.selectedCert = value;
                this.CertDetails = this.selectedCert.CertDetails;
                RaisePropertyChanged("SelectedCert");
            }
        }

        private void LoadCerts()
        {
            X509Certificate2Collection coll = EncryptionHelper.GetCertificates(StoreLocation.CurrentUser);

            List<DisCertView> cs = new List<DisCertView>();
            foreach (X509Certificate2 c in coll)
            {
                cs.Add(new DisCertView(c));
            }
            this.Certs = cs;
        }

        private void SaveCert()
        {
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    DisCert cert = new DisCert
                    {
                        Subject = this.SelectedCert.Subject,
                        ThumbPrint = this.SelectedCert.ThumbPrint
                    };
                    if (KmtConstants.IsOemCorp)
                        proxy.UpdateCertificateSubject(cert);
                    else if (KmtConstants.IsTpiCorp)
                    {

                        if (KmtConstants.CurrentHeadQuarter == null)
                        {
                            KmtConstants.CurrentHeadQuarter = new HeadQuarter()
                            {
                                CertSubject = cert.Subject,
                                CertThumbPrint = cert.ThumbPrint,
                                IsCentralizedMode = true,
                                IsCarbonCopy = false
                            };
                            hqProxy.InsertHeadQuarter(KmtConstants.CurrentHeadQuarter);
                        }
                        KmtConstants.CurrentHeadQuarter.CertSubject = cert.Subject;
                        KmtConstants.CurrentHeadQuarter.CertThumbPrint = cert.ThumbPrint;
                        hqProxy.UpdateHeadQuarter(KmtConstants.CurrentHeadQuarter);
                    }

                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                        string.Format("Microsoft Certificate Subject was changed to {0}", cert.Subject));
                    proxy.UpdateMsServiceEnabledSwitch(true);

                    DiagnosticResult result = proxy.TestMsConnection(KmtConstants.HeadQuarterId);
                    if (result.DiagnosticResultType == DiagnosticResultType.Ok)
                        proxy.UpdateMsServiceEnabledSwitch(true);
                    else
                        proxy.UpdateMsServiceEnabledSwitch(false);
                    IsBusy = false;
                    Dispatch(() =>
                    {

                        this.View.Close();
                    });
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex);
                }
            });
        }
    }
}
