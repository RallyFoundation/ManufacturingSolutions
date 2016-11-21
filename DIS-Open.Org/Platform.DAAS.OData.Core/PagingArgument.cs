using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core
{
    public class PagingArgument
    {
        private int currentPageIndex;
        private int eachPageSize;
        private int totalRecordsInDatabase;
        private int totalPageCount;

        public int CurrentPageIndex
        {
            get
            {
                return this.currentPageIndex;
            }
            set
            {
                this.currentPageIndex = Math.Abs(value);
            }
        }

        public int EachPageSize
        {
            get
            {
                return this.eachPageSize;
            }
            set
            {
                this.eachPageSize = Math.Abs(value);
            }
        }

        public int TotalPageCount
        {
            get
            {
                return this.GetTotalPageCount();
            }
        }

        private void setPageCount()
        {
            if (this.eachPageSize != 0)
            {
                this.totalPageCount = this.totalRecordsInDatabase / this.eachPageSize;

                if ((this.totalRecordsInDatabase % this.eachPageSize) > 0)
                {
                    this.totalPageCount += 1;
                }
            }
            else
            {
                this.totalPageCount = 0;
            }
        }

        public int GetTotalPageCount()
        {
            this.setPageCount();
            return this.totalPageCount;
        }

        public int GetTotalAffectedRecordCount()
        {
            return this.totalRecordsInDatabase;
        }

        public void Reset(int RecordCount)
        {
            this.totalRecordsInDatabase = Math.Abs(RecordCount);

            this.setPageCount();

            if (this.eachPageSize > this.totalRecordsInDatabase)
            {
                this.eachPageSize = this.totalRecordsInDatabase;
                this.currentPageIndex = 0;
            }

            if ((this.currentPageIndex > this.totalRecordsInDatabase) | (this.currentPageIndex > this.totalPageCount))
            {
                this.currentPageIndex = 0;
            }

            this.setPageCount();
        }
    }
}
