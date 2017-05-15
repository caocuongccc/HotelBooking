using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Einvoice_Customer.Controllers
{
    public class ChangLanguageController : Controller
    {
        // GET: ChangLanguage
        [AllowAnonymous]
        public ActionResult ChangLanguage(string culture, string returnUrl)
        {
            var httpCookie = Request.Cookies["language"];
            if(httpCookie != null)
            {
                var cookie = Response.Cookies["language"];
                if(cookie != null)
                {
                    cookie.Value = culture;
                }
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index", "EinvoiceCustomer");
        }
    }
}