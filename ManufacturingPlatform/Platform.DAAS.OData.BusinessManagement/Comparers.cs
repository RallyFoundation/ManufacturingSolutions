using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.BusinessManagement
{
    public class ConfigurationTypeComparer: IComparer<Core.DomainModel.Configuration>
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(Core.DomainModel.Configuration x, Core.DomainModel.Configuration y)
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

    public class ConfigurationDBConnectionStringComparer : IComparer<Core.DomainModel.Configuration> 
    {
        public ComparisonSortDirection SortDirection { get; set; }
        public int Compare(Core.DomainModel.Configuration x, Core.DomainModel.Configuration y)
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

    public class BusinessNameComparer : IComparer<Core.DomainModel.Business> 
    {
        public ComparisonSortDirection SortDirection { get; set; }

        public int Compare(Core.DomainModel.Business x, Core.DomainModel.Business y)
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

    public class BusinessIDComparer : IComparer<Core.DomainModel.Business>
    {
        public ComparisonSortDirection SortDirection { get; set; }

        public int Compare(Core.DomainModel.Business x, Core.DomainModel.Business y)
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

    public enum ComparisonSortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
