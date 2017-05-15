using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Einvoice_Customer.Models
{
    public class EinvoiceCustomerViewModel
    {
        public string index { get; set; }
        public string invToken { get; set; }

        [Display(Name = "TenHoaDon", ResourceType = typeof(Language.Resource))]
        public string name { get; set; }

        [Display(Name = "MauSo", ResourceType = typeof(Language.Resource))]
        public string pattern { get; set; }

        [Display(Name = "KyHieu", ResourceType = typeof(Language.Resource))]
        public string serial { get; set; }

        [Display(Name = "SoHoaDon", ResourceType = typeof(Language.Resource))]
        public string invnum { get; set; }

        [Display(Name = "TongTien", ResourceType = typeof(Language.Resource))]
        public string amount { get; set; }

        [Display(Name = "NgayXuat", ResourceType = typeof(Language.Resource))]
        public string publishdate { get; set; }

        [Display(Name = "XemHoaDon", ResourceType = typeof(Language.Resource))]
        public string signstatus { get; set; }

        [Display(Name = "TrangThai", ResourceType = typeof(Language.Resource))]
        public string status { get; set; }

        [Display(Name = "ThanhToan", ResourceType = typeof(Language.Resource))]
        public string payment { get; set; }

        // Dùng kiểm tra trạng thái hóa đơn : thanh toán hay chưa thanh toán
        // Nếu chưa thanh toán thì khi xem hóa đơn tắt đi chữ ký số
        public string paymentstatus { get; set; }
    }

}