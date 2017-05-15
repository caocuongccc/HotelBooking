using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Einvoice_Customer.Models
{
    public class InvoiceCusSP
    {
        public int ROWNUM { get; set; }
        public int TotalRow { get; set; }
        public int Id { get; set; }
        public string Pattern { get; set; }
        public string Serial { get; set; }
        public int InvNo { get; set; }
        public string InvToken { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? KindOfService { get; set; }
        public bool IsWatch { get; set; }
        public int Status { get; set; }
        public int PaymentStatus { get; set; }
        public int KindOfInv { get; set; }
        public string CusCode { get; set; }
        public string CusName { get; set; }
        public string CusTaxCode { get; set; }
        public string PaymentCode { get; set; }
        public int Amount { get; set; }
    }
}