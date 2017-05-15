using Einvoice_Customer.Models;
using HDDT_FURAMA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Einvoice_Customer.Service
{
    public class ChanelFactory : IDisposable
    {
        private HttpClient httpClient;
        public string PortalService = "Portal";
        public string LoginF = "Login";
        public string CustomerExistF = "CustomerExist";
        public string GetCustomerInfoF = "GetCustomerInfo";
        public string UpdateCustomerInfoF = "UpdateCustomerInfo";
        public string UpdatePasswordF = "UpdatePassword";
        public string SetWactchedF = "SetWactched";
        public string GetListInvoicesF = "GetListInvoices";
        public string GetFullListInvoicesF = "GetFullListInvoices";
        public string GetInvoiceByHash = "GetInvByHash";
        public ChanelFactory()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://lagunaservice.giaiphaphoadondientu.vn/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
        public async Task<T> PostObjectToWebApi<T>(string controller, string action, object obj)
        {
            var url = string.Format("/api/{0}/{1}", controller, action);
            var content = new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");
            var request = await httpClient.PostAsync(url, content);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return default(T);
        }
        public async Task<InvoiceCusSP> GetInvByHash(string hash)
        {
            var url = string.Format("/api/{0}/{1}?invTokenHash={2}", PortalService, GetInvoiceByHash, hash);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<InvoiceCusSP>(json);
                }
            }
            return default(InvoiceCusSP);
        }
        public async Task<T> Login<T>(string cusCode, string pass)
        {
            var url = string.Format("/api/{0}/{1}?username={2}&password={3}", PortalService, LoginF, cusCode, pass);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return default(T);
        }
        public async Task<T> CheckCusExist<T>(string code)
        {
            var url = string.Format("/api/{0}/{1}?code={2}", PortalService, CustomerExistF, code);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return default(T);
        }

        public async Task<Customer> GetCusInfo(string code)
        {
            var url = string.Format("/api/{0}/{1}?code={2}", PortalService, GetCustomerInfoF, code);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<Customer>(json);
                }
            }
            return default(Customer);
        }

        public async Task<List<InvoiceCusSP>> GetListInvByCusCode(string cusCode, string startDate, string endDate, int page, int pageSize)
        {
            var url = string.Format("/api/{0}/{1}?cusCode={2}&fromDate={3}&toDate={4}&pageNbr={5}&pageSize={6}", PortalService, GetListInvoicesF, cusCode, startDate, endDate, page, pageSize);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<InvoiceCusSP>>(json);
                }
            }
            return default(List<InvoiceCusSP>);
        }
        public async Task<List<InvoiceCusSP>> GetFullListInvByCusCode(string cusCode, string startDate, string endDate)
        {
            var url = string.Format("/api/{0}/{1}?cusCode={2}&fromDate={3}&toDate={4}", PortalService, GetFullListInvoicesF, cusCode, startDate, endDate);
            var request = await httpClient.GetAsync(url);
            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<InvoiceCusSP>>(json);
                }
            }
            return default(List<InvoiceCusSP>);
        }
    }
}