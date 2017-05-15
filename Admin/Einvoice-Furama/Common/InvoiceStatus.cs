using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Einvoice_Customer.Common
{
    public class InvoiceStatus
    {
        public enum eInvoice_Status
        {
            HoaDonVuaKhoiTao = 0,
            HoaDonCoDuChuKy = 1,
            HoaDonDaKhaiBaoThue = 2,
            HoaDonSaiSotBiThayThue = 3,
            HoaDonSaiSotBiDieuChinh = 4,
            HoaDonHuy = 5
        }

        public static string StatusDescription(int status)
        {
            switch (status)
            {
                case (int)eInvoice_Status.HoaDonVuaKhoiTao:
                    {
                        return "Hóa đơn vừa khởi tạo";
                    }
                case (int)eInvoice_Status.HoaDonCoDuChuKy:
                    {
                        return "Hóa đơn đã phát hành";
                    }
                case (int)eInvoice_Status.HoaDonDaKhaiBaoThue:
                    {
                        return "Hóa đơn đã khai báo thuế";
                    }
                case (int)eInvoice_Status.HoaDonSaiSotBiThayThue:
                    {
                        return "Hóa đơn sai sót bị thay thế";
                    }
                case (int)eInvoice_Status.HoaDonSaiSotBiDieuChinh:
                    {
                        return "Hóa đơn sai sót bị điều chỉnh";
                    }
                case (int)eInvoice_Status.HoaDonHuy:
                    {
                        return "Hóa đơn hủy";
                    }
                default:
                    return "";
            }
        }
        public enum eInvoice_PaymentStatus
        {
            HoaDonChuaThanhToan = 0,
            HoaDonDaThanhToan = 1
        }

        public static string PaymentStatusDescription(int status)
        {
            switch (status)
            {
                case (int)eInvoice_PaymentStatus.HoaDonChuaThanhToan:
                    {
                        return Einvoice_Customer.Language.Resource.NotBill;
                    }
                case (int)eInvoice_PaymentStatus.HoaDonDaThanhToan:
                    {
                        return Einvoice_Customer.Language.Resource.HasBill;
                    }
                default:
                    return "";
            }
        }
        public enum eInvoice_KindOf
        {
            HoaDonThongThuong = 0,
            HoaDonThayThue = 1,
            HoaDonDieuChinhTang = 2,
            HoaDonDieuChinhGiam = 3,
            HoaDonDieuChinhThongTin = 4
        }
        public static string KindOfDescription(int status)
        {
            switch (status)
            {
                case (int)eInvoice_KindOf.HoaDonThongThuong:
                    {
                        return Einvoice_Customer.Language.Resource.InvoiceNormal;
                    }
                case (int)eInvoice_KindOf.HoaDonThayThue:
                    {
                        return Einvoice_Customer.Language.Resource.InvoiceReplace;// "Hóa đơn thay thế";
                    }
                case (int)eInvoice_KindOf.HoaDonDieuChinhTang:
                    {
                        return Einvoice_Customer.Language.Resource.InvoiceAdjustIn;// "Hóa đơn điều chỉnh tăng";
                    }
                case (int)eInvoice_KindOf.HoaDonDieuChinhGiam:
                    {
                        return Einvoice_Customer.Language.Resource.InvoiceAdjustD;// "Hóa đơn điều chỉnh giảm";
                    }
                case (int)eInvoice_KindOf.HoaDonDieuChinhThongTin:
                    {
                        return Einvoice_Customer.Language.Resource.InvoiceAdjustInfor; //"Hóa đơn điều chỉnh thông tin";
                    }
                default:
                    return "";
            }
        }
        public enum eInvoice_TransferStatus
        {
            HoaDonChuaChuyenDoi = -1,
            HoaDonDaChuyenDoi = 1
        }
        public static string eTransferStatusDescription(int status)
        {
            switch (status)
            {
                case (int)eInvoice_TransferStatus.HoaDonChuaChuyenDoi:
                    {
                        return "Hóa đơn chưa chuyển đổi";
                    }
                case (int)eInvoice_TransferStatus.HoaDonDaChuyenDoi:
                    {
                        return "Hóa đơn đã chuyển đổi";
                    }
                default:
                    return "";
            }
        }

        public static List<SelectListItem> GetListStatus()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonVuaKhoiTao).ToString(), Text="Hóa đơn vừa khởi tạo"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonCoDuChuKy).ToString(),Text="Hóa đơn đã phát hành"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonDaKhaiBaoThue).ToString(),Text="Hóa đơn đã khai báo thuế"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonSaiSotBiThayThue).ToString(),Text="Hóa đơn sai sót bị thay thế"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonSaiSotBiDieuChinh).ToString(),Text="Hóa đơn sai sót bị điều chỉnh"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_Status.HoaDonHuy).ToString(),Text="Hóa đơn hủy"}
             };
            statusList = data.ToList();
            return statusList;
        }
        public static List<SelectListItem> GetListKindOfInvStatus()
        {
            List<SelectListItem> kindOfInvList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_KindOf.HoaDonThongThuong).ToString(), Text="Hóa đơn thông thường"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_KindOf.HoaDonThayThue).ToString(),Text="Hóa đơn thay thế"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_KindOf.HoaDonDieuChinhTang).ToString(),Text="Hóa đơn điều chỉnh tăng"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_KindOf.HoaDonDieuChinhGiam).ToString(),Text="Hóa đơn điều chỉnh giảm"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_KindOf.HoaDonDieuChinhThongTin).ToString(),Text="Hóa đơn điều chỉnh thông tin"}
             };
            kindOfInvList = data.ToList();
            return kindOfInvList;
        }

        public static List<SelectListItem> GetListPaymentStatus()
        {
            List<SelectListItem> paymentStatusList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_PaymentStatus.HoaDonChuaThanhToan).ToString(), Text="Hóa đơn chưa thanh toán"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_PaymentStatus.HoaDonDaThanhToan).ToString(),Text="Hóa đơn đã thanh toán"}
             };
            paymentStatusList = data.ToList();
            return paymentStatusList;
        }
        public static List<SelectListItem> GetListTransferStatus()
        {
            List<SelectListItem> transferStatusList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_TransferStatus.HoaDonChuaChuyenDoi).ToString(), Text="Hóa đơn chưa chuyển đổi"},
                 new SelectListItem{ Value=((int)InvoiceStatus.eInvoice_TransferStatus.HoaDonDaChuyenDoi).ToString(),Text="Hóa đơn đã chuyển đổi"}
             };
            transferStatusList = data.ToList();
            return transferStatusList;
        }
    }
}