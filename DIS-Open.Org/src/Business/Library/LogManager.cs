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
using System.IO;
using System.Linq;
using System.Threading;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;

namespace DIS.Business.Library
{
    public class LogManager : ILogManager
    {
        #region priviate & protected member variables

        private const int logLengthLimitation = 20971520; // 20MB
        private ILogRepository logRepository;

        #endregion

        #region Constructors & Dispose

        public LogManager()
            : this(new LogRepository())
        {
        }

        public LogManager(string dbConnectionString) 
        {
            this.logRepository = new LogRepository(dbConnectionString);
        }

        public LogManager(ILogRepository logRepository)
        {
            if (logRepository == null)
                this.logRepository = new LogRepository();
            else
                this.logRepository = logRepository;
        }

        #endregion

        #region Public Methods

        public List<Category> GetCategories()
        {
            return logRepository.GetCategories();
        }

        public Log GetLogById(int logId)
        {
            return logRepository.GetLogById(logId);
        }

        public PagedList<Log> GetSystemLogs(int severity, DateTime? from, DateTime? to, int pageNumber, int pageSize, string sortBy, bool sortByDesc)
        {
            if (severity == 0)
                return new PagedList<Log>(new List<Log>().AsQueryable(), 0, pageSize);

            LogSearchCriteria criteria = new LogSearchCriteria()
            {
                Category = MessageLogger.SystemCategoryName,
                Severity = severity,
                DateFrom = from,
                DateTo = to != null ? to.Value.AddDays(1) : (DateTime?)null,
                PageSize = pageSize,
                PageNumber = pageNumber,
                ShouldIncludeMessage = false
            };

            if (!string.IsNullOrEmpty(sortBy))
            {
                criteria.SortBy = sortBy;
                criteria.SortByDesc = sortByDesc;
            }
            return SearchLogs(criteria);
        }

        public PagedList<Log> GetOperationLogs(string userName, DateTime? from, DateTime? to, int pageNumber, int pageSize, string sortBy, bool sortByDesc)
        {
            LogSearchCriteria criteria = new LogSearchCriteria()
            {
                Category = MessageLogger.OperationCategoryName,
                Severity = (int)TraceEventType.Information,
                UserName = userName,
                DateFrom = from,
                DateTo = to != null ? to.Value.AddDays(1) : (DateTime?)null,
                PageSize = pageSize,
                PageNumber = pageNumber,
                ShouldIncludeMessage = false
            };
            if (!string.IsNullOrEmpty(sortBy))
            {
                criteria.SortBy = sortBy;
                criteria.SortByDesc = sortByDesc;
            }
            return SearchLogs(criteria);
        }

        public int ExportLogs(string categoryName, DateTime? from, DateTime? to, bool shouldDeleteFromDb, string outputPath, CancellationTokenSource cancellationTokenSource)
        {
            const string exportFileNameFormat = "DIS_{0}_{1:yyyy-MM-dd}_{2:yyyy-MM-dd}.xml";
            LogSearchCriteria criteria = new LogSearchCriteria()
            {
                Category = categoryName,
                Severity = int.MaxValue,
                DateFrom = from,
                DateTo = to != null ? to.Value.AddDays(1) : (DateTime?)null,
                PageSize = MessageLogger.LogsPerFile,
                SortBy = "TimestampUtc",
                SortByDesc = false,
                ShouldIncludeMessage = true
            };

            int total = 0;
            int currentPage = 1;
            PagedList<Log> logs = null;
            try
            {
                while (cancellationTokenSource == null || !cancellationTokenSource.IsCancellationRequested)
                {
                    criteria.PageNumber = currentPage;
                    if (!shouldDeleteFromDb)
                        currentPage++;
                    logs = SearchLogs(criteria);
                    if (logs.Count == 0)
                        break;

                    total += logs.Count;
                    SplitAndWriteLogs(categoryName, outputPath, exportFileNameFormat, logs);
                    if (shouldDeleteFromDb)
                        logRepository.DeleteLogs(logs);

                    if (currentPage > logs.PageCount)
                        break;
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new DisException("Exception_PermissionsMsg");
            }

            return total;
        }

        private void SplitAndWriteLogs(string categoryName, string outputPath, string exportFileNameFormat, List<Log> logs)
        {
            int length = 0;
            List<Log> logsToWrite = new List<Log>();
            foreach (Log log in logs)
            {
                if (length + log.Message.Length > logLengthLimitation)
                {
                    File.WriteAllText(GetFileName(categoryName, exportFileNameFormat, outputPath, logsToWrite), logsToWrite.ToDataContract());
                    length = 0;
                    logsToWrite.Clear();
                }

                length += log.Message.Length;
                logsToWrite.Add(log);
            }
            File.WriteAllText(GetFileName(categoryName, exportFileNameFormat, outputPath, logsToWrite), logsToWrite.ToDataContract());
        }

        private string GetFileName(string categoryName, string exportFileNameFormat, string outputPath, List<Log> logs)
        {
            string fileName = Path.Combine(outputPath, string.Format(exportFileNameFormat,
                categoryName, logs.First().Timestamp, logs.Last().Timestamp));
            int i = 2;
            string newFileName = fileName;
            while (File.Exists(newFileName))
            {
                newFileName = string.Format("{0}_{1}{2}", Path.Combine(Path.GetDirectoryName(fileName),
                    Path.GetFileNameWithoutExtension(fileName)), i++, Path.GetExtension(fileName));
            }
            return newFileName;
        }

        #endregion

        private PagedList<Log> SearchLogs(LogSearchCriteria criteria)
        {
            if (criteria == null)
                throw new ApplicationException("Search criteria is null.");

            if (criteria.DateFrom != null)
                criteria.DateFrom = criteria.DateFrom.Value.ToUniversalTime();
            if (criteria.DateTo != null)
                criteria.DateTo = criteria.DateTo.Value.ToUniversalTime();

            if (criteria.PageSize < 0)
                criteria.PageSize = LogSearchCriteria.DefaultPageSize;
            else if (criteria.PageSize == 0)
                criteria.PageSize = int.MaxValue;

            return logRepository.SearchLogs(criteria);
        }
    }
}
