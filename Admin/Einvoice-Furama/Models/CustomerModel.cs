using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Einvoice_Customer.Models
{
    [XmlRoot(ElementName = "Customer")]
    public class CustomerModel
    {
        [StringLength(150, ErrorMessage = "Tên khách hàng không thể vượt quá 150 ký tự.")]
        [Required(ErrorMessage = "Chưa nhập tên khách hàng")]
        [XmlElement(ElementName = "Name")]
        [Display(Name = "CusName", ResourceType = typeof(Language.Resource))]
        public string Name { get; set; }
        [XmlElement(ElementName = "Code")]
        [Display(Name = "CusCode", ResourceType = typeof(Language.Resource))]
        public string Code { get; set; }
        [XmlElement(ElementName = "TaxCode")]
        [Display(Name = "TaxCode", ResourceType = typeof(Language.Resource))]
        public string TaxCode { get; set; }
        [Required(ErrorMessage = "Chưa nhập địa chỉ")]
        [XmlElement(ElementName = "Address")]
        [Display(Name = "Addr", ResourceType = typeof(Language.Resource))]
        public string Address { get; set; }
        [XmlElement(ElementName = "BankAccountName")]
        [Display(Name = "BankAccountName", ResourceType = typeof(Language.Resource))]
        public string BankAccountName { get; set; }
        [XmlElement(ElementName = "BankName")]
        [Display(Name = "BankName", ResourceType = typeof(Language.Resource))]
        public string BankName { get; set; }
        [XmlElement(ElementName = "BankNumber")]
        [Display(Name = "BankNumber", ResourceType = typeof(Language.Resource))]
        public string BankNumber { get; set; }
        [EmailAddress(ErrorMessage = "Không đúng định dạng mail")]
        [XmlElement(ElementName = "Email")]
        [Display(Name = "Email", ResourceType = typeof(Language.Resource))]
        public string Email { get; set; }
        [XmlElement(ElementName = "Fax")]
        [Display(Name = "Fax", ResourceType = typeof(Language.Resource))]
        public string Fax { get; set; }
        [XmlElement(ElementName = "Phone")]
        [Display(Name = "Phone", ResourceType = typeof(Language.Resource))]
        public string Phone { get; set; }
        [XmlElement(ElementName = "ContactPerson")]
        [Display(Name = "ContactPerson", ResourceType = typeof(Language.Resource))]
        public string ContactPerson { get; set; }
        [XmlElement(ElementName = "RepresentPerson")]
        [Display(Name = "RepresentPerson", ResourceType = typeof(Language.Resource))]
        public string RepresentPerson { get; set; }
        [XmlElement(ElementName = "CusType")]
        [Display(Name = "CusType", ResourceType = typeof(Language.Resource))]
        public string CusType { get; set; }
    }
}
