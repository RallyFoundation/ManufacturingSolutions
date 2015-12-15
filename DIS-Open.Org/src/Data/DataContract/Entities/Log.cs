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
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    [DataContract]
    public  class Log
    {
        private const string operationLogTitlePattern = @"^(?<title>KMT) - (?<userName>\w+)$";
        
        public Log()
        {
            this.CategoryLogs = new List<CategoryLog>();
        }

        [DataMember]
        public int LogId { get; set; }
        public Nullable<int> EventId { get; set; }
        public int Priority { get; set; }
        public string SeverityName { get; set; }
        public string Title { get; set; }
        [DataMember]
        public System.DateTime TimestampUtc { get; set; }
        public string MachineName { get; set; }
        public string AppDomainName { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ThreadName { get; set; }
        public string Win32ThreadId { get; set; }
        [DataMember]
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
        public ICollection<CategoryLog> CategoryLogs { get; set; }

        public DateTime Timestamp {
            get {
                DateTimeOffset offset = new DateTimeOffset(TimestampUtc, TimeZoneInfo.Utc.GetUtcOffset(TimestampUtc));
                return offset.LocalDateTime;
            }
        }

        public TraceEventType Severity {
            get {
                return (TraceEventType)Enum.Parse(typeof(TraceEventType), SeverityName);
            }
        }

        public string DisplayTitle {
            get {
                Match m = Regex.Match(Title, operationLogTitlePattern);
                if (m.Success)
                    return m.Groups["title"].Value;
                else
                    return Title;
            }
        }

        public string Operator {
            get {
                Match m = Regex.Match(Title, operationLogTitlePattern);
                if (m.Success)
                    return m.Groups["userName"].Value;
                else
                    return null;
            }
        }

        public string InlineMessage {
            get {
                return Regex.Replace(Message, @"[\r\n]+", "  ");
            }
        }
    }
}

