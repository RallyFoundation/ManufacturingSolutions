using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmulatorService.Entities;

namespace EmulatorService.Mapping
{
	public class TestMap : EntityTypeConfiguration<Test>
	{
		public TestMap()
		{
			// Primary Key
			this.HasKey(t => t.TestId);

			// Properties
			this.Property(t => t.TestName)
				.IsRequired()
				.HasMaxLength(32);
				
			// Table & Column Mappings
			this.ToTable("Test");
			this.Property(t => t.TestId).HasColumnName("TestID");
			this.Property(t => t.TestName).HasColumnName("TestName");
			this.Property(t => t.IsPositive).HasColumnName("IsPositive");
			this.Property(t => t.Status).HasColumnName("Status");
			this.Property(t => t.ReadyDate).HasColumnName("ReadyDate");
			this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
		}
	}
}

