using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Einvoice_Customer.Models;
using System.Text;
using Einvoice_Customer.Service;
using System.Threading.Tasks;

namespace Einvoice_Customer.Controllers
{
    [Authorize]
    public class EinvoiceCustomerController : BaseController
    {
        // GET: EinvoiceCustomer
        // Hiển thị danh sách hóa đơn khi vào link /EinvoiceCustomer/
        // int Page : trang hiển thị, giá trị khi bấm vào button phân trang
        int Size_Of_Page = 10;
        public async Task<ActionResult> Index()
        {
            using (var chanel = new ChanelFactory())
            {
                string cusCode = User.Identity.Name;
                var cus = await GetByCusCode(cusCode.Trim());
                ViewBag.TenDonVi = cus.Name ?? "";

                //GetListInv
                var listInvs = await chanel.GetListInvByCusCode(cusCode.Trim(), null, null, 1, Size_Of_Page);

                if (listInvs == null)
                {
                    ViewBag.Message = Einvoice_Customer.Language.Resource.InvoiceNotFound;
                    return View("Message");
                }
                else
                {
                    {
                        if (listInvs.Count() > 0)
                        {
                            int total = listInvs[0].TotalRow;
                            if (total % Size_Of_Page > 0)
                            {
                                ViewBag.TotalPages = total / Size_Of_Page + 1;
                            }
                            else
                            {
                                ViewBag.TotalPages = total / Size_Of_Page;
                            }
                        }
                        else
                        {
                            ViewBag.TotalPages = 0;
                        }
                        return View(listInvs);
                    }
                }
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ViewByHash(string hash)
        {
            var inv = await GetInvByHash(hash);
            if (inv == null)
                return Content(Einvoice_Customer.Language.Resource.InvoiceNotFound);

            if (inv.PaymentStatus == 0)
            {
                await SetWatched(inv.InvToken, false);
            }
            else
            {
                await SetWatched(inv.InvToken, true);
            }
            ViewBag.Token = inv.InvToken;
            ViewBag.PaymentStatus = inv.PaymentStatus;
            ViewBag.IframeUrl = this.GetInvView(inv.InvToken, inv.PaymentStatus);
            return PartialView("_popupInvoice");
        }

        public async Task<ActionResult> Search(string startDate, string endDate)
        {
            using (var chanel = new ChanelFactory())
            {
                string cusCode = User.Identity.Name;
                //GetListInv
                var listInvs = await chanel.GetListInvByCusCode(cusCode.Trim(), startDate, endDate, 1, Size_Of_Page);

                if (listInvs == null)
                {
                    return PartialView("EInvoiceCustomerItem", listInvs);
                }
                else
                {
                    {
                        if (listInvs.Count() > 0)
                        {
                            int total = listInvs[0].TotalRow;
                            if (total % Size_Of_Page > 0)
                            {
                                ViewBag.TotalPages = total / Size_Of_Page + 1;
                            }
                            else
                            {
                                ViewBag.TotalPages = total / Size_Of_Page;
                            }
                        }
                        else
                        {
                            ViewBag.TotalPages = 0;
                        }
                        return PartialView("EInvoiceCustomerItem", listInvs);
                    }
                }
            }
        }
        public async Task<ActionResult> Paging(int page, string startDate, string endDate)
        {
            using (var chanel = new ChanelFactory())
            {
                string cusCode = User.Identity.Name;

                //GetListInv
                var listInvs = await chanel.GetListInvByCusCode(cusCode.Trim(), startDate, endDate, page, Size_Of_Page);

                return PartialView("EInvoiceCustomerItemContent", listInvs);
            }
        }

        public async Task<ActionResult> ExportPDF(string startDate, string endDate)
        {
            using (var chanel = new ChanelFactory())
            {
                string cusCode = User.Identity.Name;
                var cus = await GetByCusCode(cusCode.Trim());
                ViewBag.TenDonVi = cus.Name;
                //GetListInv
                var listInvs = await chanel.GetFullListInvByCusCode(cusCode.Trim(), startDate, endDate);
                if (listInvs == null)
                {
                    listInvs = new List<InvoiceCusSP>();
                }
                Rotativa.PartialViewAsPdf vPdf = new Rotativa.PartialViewAsPdf("_ListPDF", listInvs)
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    IsJavaScriptDisabled = false,
                    IsBackgroundDisabled = false,
                    MinimumFontSize = 10,
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(15, 10, 15, 10)

                };
                return vPdf;
            }
        }
        // Hiển thị popup xem hóa đơn khi bấm vào button Xem
        // token : giá trị token của hóa đơn hiển thị
        // paymentstatus : trạng thái thanh toán của hóa đơn
        public async Task<ActionResult> View(int id, string token, int paymentstatus)
        {
            ViewBag.Token = token;
            ViewBag.PaymentStatus = paymentstatus;
            //db.Update<Invoice>(@"Set IsWatch=@0 where InvToken=@1", true, token);
			if(paymentstatus == 0)
			{
				await SetWatched(token,false);
			}
			else
			{
				await SetWatched(token,true);
			}
            
            ViewBag.IframeUrl = this.GetInvView(token, paymentstatus);
            return PartialView("_popupInvoice");
        }

        private async Task SetWatched(string token, bool isWatch)
        {
              using (var chanel = new ChanelFactory())
              {
                  var obj = new Einvoice_Customer.Models.PortalServiceModel.WatchedRequest()
                  {
                      InvToken = token,
                      IsWatched = isWatch
                  };
                  await chanel.PostObjectToWebApi<Einvoice_Customer.Models.PortalServiceModel.WatchedRequest>("Portal","SetWactched",obj);
              }
        }

        // Hiển thị hóa đơn dạng pdf khi bấm vào button PDF
        // token : giá trị token của hóa đơn hiển thị
        // paymentstatus : trạng thái thanh toán của hóa đơn
        public async Task<ActionResult> ViewPDF(string token, int paymentstatus)
        {
            ViewBag.Token = token;
            ViewBag.PaymentStatus = paymentstatus;
            
            // Lấy hóa đơn dạng pdf của khách hàng thông qua hàm webservice
            string pdf = PortalService.downloadInvPDF(token, this.UserService, this.PassService);

            if (pdf == "ERR:11")
            {
                ViewBag.Message = Einvoice_Customer.Language.Resource.notpayinvoice;
                return View("Message");
            }
			await SetWatched(token,true);
            byte[] bytes = System.Convert.FromBase64String(pdf);

            return File(bytes, "application/octet-stream", "hoadon.pdf");
        }

        // Hiển thị hóa đơn dạng .inv khi bấm vào button Download
        // token : giá trị token của hóa đơn hiển thị
        // paymentstatus : trạng thái thanh toán của hóa đơn
        public async Task<ActionResult> ViewINV(string token, int paymentstatus)
        {
            ViewBag.Token = token;
            ViewBag.PaymentStatus = paymentstatus;

            // Lấy hóa đơn dạng .inv của khách hàng thông qua hàm webservice
            string inv = PortalService.downloadInv(token, this.UserService, this.PassService);

            if (inv == "ERR:11")
            {
                ViewBag.Message = Einvoice_Customer.Language.Resource.notpayinvoice;
                return View("Message");
            }
            await SetWatched(token, true);
            var contentType = "text/xml";
            var bytes = Encoding.UTF8.GetBytes(inv);
            var result = new FileContentResult(bytes, contentType);
            result.FileDownloadName = "hoadon.inv";
            return result;
        }
    }
}
