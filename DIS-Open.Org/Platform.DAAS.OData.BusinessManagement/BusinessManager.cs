using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core.BusinessManagement;
using Platform.DAAS.OData.Core.DomainModel;

namespace Platform.DAAS.OData.BusinessManagement
{
    public class BusinessManager : IBusinessManager
    {
        public string AddBusinessConfiguration(Core.DomainModel.Business Business)
        {
            throw new NotImplementedException();
        }

        public int UpdateBusinessConfiguration(Core.DomainModel.Business Business)
        {
            throw new NotImplementedException();
        }

        Core.DomainModel.Business IBusinessManager.GetBusiness(string BusinessID)
        {
            throw new NotImplementedException();
        }

        Core.DomainModel.Configuration IBusinessManager.GetConfiguration(string ConfigurationID)
        {
            throw new NotImplementedException();
        }

        Core.DomainModel.Configuration IBusinessManager.GetConfiguration(string BusinessID, ConfigurationType ConfigurationType)
        {
            throw new NotImplementedException();
        }

        Core.DomainModel.Business[] IBusinessManager.ListBusiness()
        {
            throw new NotImplementedException();
        }

        Core.DomainModel.Business[] IBusinessManager.ListBusiness(bool IsIncludingConfigurations)
        {
            throw new NotImplementedException();
        }
    }
}
