//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Platform.DAAS.OData.Security
{
    using System;
    using System.Collections.Generic;
    
    public partial class DataScope
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataScope()
        {
            this.RoleDataScopes = new HashSet<RoleDataScope>();
        }
    
        public string Id { get; set; }
        public string ScopeName { get; set; }
        public string ScopeType { get; set; }
        public string DataType { get; set; }
        public string DataIdentifier { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleDataScope> RoleDataScopes { get; set; }
    }
}
