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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public class LogRepository : RepositoryBase, ILogRepository
    {
        public LogRepository() : base() 
        {

        }

        public LogRepository(string ConnectionString) : base(ConnectionString) 
        {

        }
        public List<Category> GetCategories() {
            using (var context = GetContext()) {
                return context.Categories.ToList();
            }
        }

        public PagedList<Log> SearchLogs(LogSearchCriteria criteria)
        {
            if (criteria == null)
                throw new ApplicationException("Search criteria is null.");

            using (var context = GetContext())
            {
                IQueryable<Log> query = from log in context.Logs
                                        join cl in context.CategoryLogs on log.LogId equals cl.LogId
                                        join c in context.Categories on cl.CategoryId equals c.CategoryId
                                        where c.CategoryName == criteria.Category
                                        select log;

                if (criteria.DateFrom != null)
                    query = query.Where(l => l.TimestampUtc >= criteria.DateFrom);

                if (criteria.DateTo != null)
                    query = query.Where(l => l.TimestampUtc <= criteria.DateTo);

                if (!string.IsNullOrEmpty(criteria.Title))
                    query = query.Where(l => l.Title == criteria.Title);

                if (criteria.Severity > 0)
                {
                    List<string> severities = new List<string>();
                    if ((criteria.Severity & (int)TraceEventType.Critical) != 0)
                        severities.Add(TraceEventType.Critical.ToString());
                    if ((criteria.Severity & (int)TraceEventType.Error) != 0)
                        severities.Add(TraceEventType.Error.ToString());
                    if ((criteria.Severity & (int)TraceEventType.Warning) != 0)
                        severities.Add(TraceEventType.Warning.ToString());
                    if ((criteria.Severity & (int)TraceEventType.Information) != 0)
                        severities.Add(TraceEventType.Information.ToString());
                    query = query.Where(l => severities.Contains(l.SeverityName));
                }

                query = query.SortBy(criteria.SortBy, criteria.SortByDesc);
                PagedList<int> logViewIds = new PagedList<int>(query.Select(l => l.LogId), criteria.StartIndex, criteria.PageSize);
                IQueryable<Log> secondQuery = context.Logs.Where(l => logViewIds.Contains(l.LogId));

                Dictionary<int, Log> logs = (criteria.ShouldIncludeMessage
                    ? secondQuery.Select(l => new LogToView
                    {
                        LogId = l.LogId,
                        TimestampUtc = l.TimestampUtc,
                        Message = l.FormattedMessage
                    })
                    : secondQuery.Select(l => new LogToView
                    {
                        LogId = l.LogId,
                        SeverityName = l.SeverityName,
                        Title = l.Title,
                        TimestampUtc = l.TimestampUtc,
                        Message = l.Message.Substring(0, criteria.MessageLength)
                    })).ToDictionary(l => l.LogId, l => (Log)l);

                return logViewIds.Transform<Log>(id => logs[id]);
            }
        }

        public Log GetLogById(int logId)
        {
            using (var context = GetContext())
            {
                return context.Logs.SingleOrDefault(l => l.LogId == logId);
            }
        }

        public void DeleteLogs(List<Log> logs)
        {
            string[] tableNames = new string[] {
                "CategoryLog",
                "Log",
            };
            string idList = string.Join(",", logs.Select(l => l.LogId));
            DeleteEntities(tableNames, "LogID", idList);
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
