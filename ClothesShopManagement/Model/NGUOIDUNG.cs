//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClothesShopManagement.Model
{
    using ClothesShopManagement.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class NGUOIDUNG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NGUOIDUNG()
        {
            this.HOADONs = new HashSet<HOADON>();
            this.PHIEUNHAPs = new HashSet<PHIEUNHAP>();
        }
    
        public string MAND { get; set; }
        public string TENND { get; set; }
        public Nullable<System.DateTime> NGSINH { get; set; }
        public string GIOITINH { get; set; }
        public string SDT { get; set; }
        public string DIACHI { get; set; }
        public string USERNAME { get; set; }
        public string PASS { get; set; }
        public bool QTV { get; set; }
        public bool TTND { get; set; }
        private string _AVA;
        public string AVA { get => _AVA.Contains(Const._localLink) ? _AVA : (Const._localLink + _AVA); set { _AVA = value; } }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUNHAP> PHIEUNHAPs { get; set; }
    }
}
