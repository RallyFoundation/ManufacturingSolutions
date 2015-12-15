using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Mapping
{
    public class TempKeyIdMap : EntityTypeConfiguration<TempKeyId>
    {
        public TempKeyIdMap()
        {
            // Primary Key
            this.HasKey(t => t.KeyId);

            // Table & Column Mappings
            this.ToTable("TempKeyId");
            this.Property(t => t.KeyId).HasColumnName("KeyId");
        }
    }
}
