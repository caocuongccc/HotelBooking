using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Einvoice_Customer.Models
{
    public class PortalServiceModel
    {
        public class UpdateCusRequest
        {
            public string XMLCusData { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }
        public class SetCustomerPasswordRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class LoginRequet
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        public class WatchedRequest
        {
            public string InvToken { get; set; }
            public bool IsWatched { get; set; }
        }
        public class GetListInvRequest
        {
            public string CusCode { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public int? PageNbr { get; set; }
            public int? PageSize { get; set; }

        }
    }
}