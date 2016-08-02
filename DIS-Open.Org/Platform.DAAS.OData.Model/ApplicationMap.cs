using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Platform.DAAS.OData.Model.Mapping
{
    public class ApplicationMap : EntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            this.ToTable("applications");
            
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.Description).HasColumnName("description");
            this.Property(t => t.Owner).HasColumnName("owner");
            this.Property(t => t.Status).HasColumnName("status");
        }
    }
}
