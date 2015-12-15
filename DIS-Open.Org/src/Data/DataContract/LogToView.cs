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
using System.ComponentModel.DataAnnotations;

namespace DIS.Data.DataContract
{
    public class LogToView
    {
        public int LogId { get; set; }
        public string SeverityName { get; set; }
        public string Title { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Message { get; set; }
        
        public static explicit operator Log(LogToView log) {
            return new Log() {
                LogId = log.LogId,
                SeverityName = log.SeverityName,
                Title = log.Title,
                Message = log.Message,
                TimestampUtc = log.TimestampUtc
            };
        }
    }
}
