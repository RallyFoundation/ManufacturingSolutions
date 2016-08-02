namespace Platform.DAAS.OData.Security
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class AuthEntityModelContainer : DbContext
    {
        public AuthEntityModelContainer(): base(String.Format("name={0}", ModuleConfiguration.DefaultAuthorizationStoreConnectionName))
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<DataScope> DataScopes { get; set; }
        public virtual DbSet<ObjectOperationAuthItem> ObjectOperationAuthItems { get; set; }
        public virtual DbSet<RoleOperation> RoleOperations { get; set; }
        public virtual DbSet<RoleDataScope> RoleDataScopes { get; set; }
    }
}
