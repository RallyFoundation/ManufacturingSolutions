using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.Contract
{
    public class CustomerConfigurationTypeComparer: IComparer<Configuration>
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(Configuration x, Configuration y)
        {
            int returnValue = 0;

            returnValue = x.ConfigurationType.CompareTo(y.ConfigurationType);

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }

    public class CustomerConfigurationDBConnectionStringComparer : IComparer<Configuration> 
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(Configuration x, Configuration y)
        {
            int returnValue = 0;

            returnValue = x.DbConnectionString.ToLower().CompareTo(y.DbConnectionString.ToLower());

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }

    public class CustomerNameComparer : IComparer<Customer> 
    {
        public ComparisonSortDirection SortDirection { get; set; }

        public int Compare(Customer x, Customer y)
        {
            int returnValue = 0;

            returnValue = x.Name.CompareTo(y.Name);

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }

    public class CustomerIDComparer : IComparer<Customer>
    {
        public ComparisonSortDirection SortDirection { get; set; }

        public int Compare(Customer x, Customer y)
        {
            int returnValue = 0;

            returnValue = x.ID.CompareTo(y.ID);

            if (this.SortDirection == ComparisonSortDirection.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }
    }

    //public class CustomerReferenceIDComparer : IComparer<Customer>
    //{
    //    public ComparisonSortDirection SortDirection { get; set; }

    //    public int Compare(Customer x, Customer y)
    //    {
    //        int returnValue = 0;

    //        returnValue = x.ReferenceID.CompareTo(y.ReferenceID);

    //        if (this.SortDirection == ComparisonSortDirection.Descending)
    //        {
    //            returnValue *= -1;
    //        }

    //        return returnValue;
    //    }
    //}

    public enum ComparisonSortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
