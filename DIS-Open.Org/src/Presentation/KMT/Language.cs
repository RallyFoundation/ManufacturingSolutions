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
using System.Collections.ObjectModel;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// 
    /// </summary>
    public class Language
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Language Default = new Language()
        {
            LanguageName = Properties.MergedResources.Common_English,
            LanguageCode = "en-US"
        };

        /// <summary>
        /// 
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Language> GetLangs()
        {
            return new ObservableCollection<Language>()
                { 
                    Default,
                    new Language()
                    {
                        LanguageName = Properties.MergedResources.Common_Taiwan,
                        LanguageCode = "zh-TW"
                    },
                    new Language()
                    {
                        LanguageName = Properties.MergedResources.Common_Chinese,
                        LanguageCode = "zh-CN"
                    },
                    new Language ()
                    {
                        LanguageName = Properties.MergedResources.Common_Japanese,
                        LanguageCode = "ja-JP"
                    },
                    
                     new Language ()
                    {
                        LanguageName = Properties.MergedResources.Common_Portuguese,
                        LanguageCode = "pt-BR"
                    },
                    new Language()
                    {
                        LanguageName = Properties.ResourcesOfR6.Common_Spanish,
                        LanguageCode = "es"
                    }
                    
                };
        }
    }
}
