﻿using System.Web;
using System.Web.Mvc;

namespace Einvoice_Customer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
