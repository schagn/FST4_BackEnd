//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public person()
        {
            this.business_customer = new HashSet<business_customer>();
            this.order = new HashSet<order>();
            this.rating = new HashSet<rating>();
        }
    
        public System.Guid person_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string e_mail { get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public string street { get; set; }
        public string country { get; set; }
        public int zip_code { get; set; }
        public System.Guid type_id { get; set; }
        public int valid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<business_customer> business_customer { get; set; }
        public virtual city city { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> order { get; set; }
        public virtual type type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rating> rating { get; set; }
    }
}
