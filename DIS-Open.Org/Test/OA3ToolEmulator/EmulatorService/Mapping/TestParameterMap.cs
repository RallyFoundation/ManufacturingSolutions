using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmulatorService.Entities;

namespace EmulatorService.Mapping
{
	public class TestParameterMap : EntityTypeConfiguration<TestParameter>
	{
		public TestParameterMap()
		{
			// Primary Key
			this.HasKey(t => t.TestParameterId);

			// Properties
			this.Property(t => t.Name)
				.HasMaxLength(32);
				
			// Table & Column Mappings
			this.ToTable("TestParameter");
			this.Property(t => t.TestParameterId).HasColumnName("TestParameterID");
			this.Property(t => t.TestId).HasColumnName("TestID");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Index).HasColumnName("Index");
			this.Property(t => t.Value).HasColumnName("Value");

			// Relationships
			this.HasRequired(t => t.Test)
				.WithMany(t => t.TestParameters)
				.HasForeignKey(d => d.TestId);
				
		}
	}
}

