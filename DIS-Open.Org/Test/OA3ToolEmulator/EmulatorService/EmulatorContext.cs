using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EmulatorService.Entities;
using EmulatorService.Mapping;

namespace EmulatorService
{
    public class EmulatorContext : DbContext
    {
        static EmulatorContext()
        {
            Database.SetInitializer<EmulatorContext>(null);
        }

        public EmulatorContext()
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestParameter> TestParameters { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Configurations.Add(new TestMap());
            modelBuilder.Configurations.Add(new TestParameterMap());
            modelBuilder.Configurations.Add(new TestResultMap());
        }
    }
}

