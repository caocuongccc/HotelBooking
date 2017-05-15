using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Einvoice_Customer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "TỔNG QUAN VỀ HÓA ĐƠN ĐIỆN TỬ CỦA VNPT.";
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index","EinvoiceCustomer");
            }
            return View("Index");
        }
        public ActionResult Support()
        {
            ViewBag.Message = "Hỗ Trợ.";
            return View("Support");
        }

        public ActionResult About()
        {
            ViewBag.Message = "TỔNG QUAN VỀ HÓA ĐƠN ĐIỆN TỬ CỦA VNPT.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Thông Tin Liên Hệ.";

            return View();
        }

        public ActionResult Soft()
        {
            ViewBag.Message = "Phần Mềm Hỗ Trợ.";

            return View();
        }
    }
}