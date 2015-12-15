using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmulatorService.Entities;

namespace EmulatorService.Mapping
{
	public class TestResultMap : EntityTypeConfiguration<TestResult>
	{
		public TestResultMap()
		{
			// Primary Key
			this.HasKey(t => t.TestResultId);

			// Properties
			this.Property(t => t.Name)
				.HasMaxLength(32);
				
			// Table & Column Mappings
			this.ToTable("TestResult");
			this.Property(t => t.TestResultId).HasColumnName("TestResultID");
			this.Property(t => t.TestId).HasColumnName("TestID");
			this.Property(t => t.ActualResult).HasColumnName("ActualResult");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Index).HasColumnName("Index");
			this.Property(t => t.Value).HasColumnName("Value");
			this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
			this.Property(t => t.Comments).HasColumnName("Comments");

			// Relationships
			this.HasRequired(t => t.Test)
				.WithMany(t => t.TestResults)
				.HasForeignKey(d => d.TestId);
				
		}
	}
}

