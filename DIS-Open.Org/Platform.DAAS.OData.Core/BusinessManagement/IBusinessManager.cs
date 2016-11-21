using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.Core.BusinessManagement
{
    public interface IBusinessManager
    {
        string AddBusinessConfiguration(Business Business);

        int UpdateBusinessConfiguration(Business Business);

        Business[] ListBusiness();

        Business[] ListBusiness(bool IsIncludingConfigurations);

        Business[] SearchBusiness(Func<IList<SearchingArgument>, object> QueryExpressionFunction, IList<SearchingArgument> SearchingArguments, PagingArgument PagingArgument);

        Business GetBusiness(string BusinessID);

        Configuration GetConfiguration(string BusinessID, ConfigurationType ConfigurationType);

        Configuration GetConfiguration(string ConfigurationID);
    }
}
