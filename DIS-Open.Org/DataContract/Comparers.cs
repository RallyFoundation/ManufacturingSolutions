using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
    public enum ComparisonSortDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public class KeyOperationResultFailedFlagComparer : IComparer<KeyOperationResult>
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(KeyOperationResult x, KeyOperationResult y)
        {
            int returnValue = 0;

            returnValue = x.Failed.CompareTo(y.Failed);

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }

    public class KeyOperationResultFailedTypeComparer : IComparer<KeyOperationResult>
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(KeyOperationResult x, KeyOperationResult y)
        {
            int returnValue = 0;

            returnValue = x.FailedType.CompareTo(y.FailedType);

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }
}
