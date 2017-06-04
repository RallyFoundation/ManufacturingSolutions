using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Platform.DAAS.OData.Core.ServiceManagement;

namespace Platform.DAAS.OData.Framework
{
    public abstract class ODataEntityModel : IODataEntityModel<ODataModelBuilder>
    {
        public virtual IDictionary<string, object> GetDynamicProperties()
        {
            return null;
        }

        public virtual void Register(ODataModelBuilder Builder)
        {
            Builder.AddEntitySet(this.GetType().Name, Builder.AddEntityType(this.GetType()));
        }
    }
}
