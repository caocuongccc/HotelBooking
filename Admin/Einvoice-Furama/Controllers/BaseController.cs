using System;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Einvoice_Customer.Models;
using Einvoice_Customer.Service;
using System.Threading.Tasks;
using HDDT_FURAMA;

namespace Einvoice_Customer.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        // Biến lưu acc của webservice
        //public string userservice = "glservice";
        //public string passervice = "1234567";
        //public string userservice = "ktservice";
        //public string passervice = "1234567";
        //public string userservice = "qbservice";
        //public string passervice = "Hoadondientuqb!@123";
        //public string userservice = "pyservice";
        //public string passervice = "py@123##";

        //public string userservice = "toyotadngservice";
        //public string passervice = "Toyotadn@123##";

        //public string userservice = "vdkservice";
        //public string passervice = "abcd1234A@";

        //public string userservice = "asiaparkservice";
        //public string passervice = "Asiapark@123##";
        public string GenPass()
        {
            int id = 0;
            string st = "";
            Random rd = new Random();
            char[] key = new char[65];
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                key[id] = ch;
                id++;
            }
            for (char ch = 'a'; ch < 'z'; ch++)
            {
                key[id] = ch;
                id++;
            }
            for (int i = 0; i <= 9; i++)
            {
                key[id] = char.Parse(i.ToString());
                id++;
            }
            for (int i = 0; i < 8; i++)
            {
                st += key[rd.Next(0, 60)].ToString();
            }
            return st;
        }
        //public PetaPoco.Database db = new PetaPoco.Database("TOYOTA");
        private string userService;

        public string UserService
        {
            get { return userService ?? (userService = System.Configuration.ConfigurationManager.AppSettings["username"]); }
        }
        private string passService;

        public string PassService
        {
            get { return passService ?? (passService = System.Configuration.ConfigurationManager.AppSettings["password"]); }
        }

        private EmailServices emailService;

        public EmailServices EmailService
        {
            get { return emailService ?? (emailService = new EmailServices()); }
        }
        

        public async Task<InvoiceCusSP> GetInvByHash(string hash)
        {
            using (var chanel = new ChanelFactory())
            {
                var cus = await chanel.GetInvByHash(hash);
                return cus;
            }
        }

        public string GetInvView(string invToken, int paymentstatus )
        {
            try
            {
                string ReturnValue = "";
                if (paymentstatus == 0)
                {
                    ReturnValue = "";// PortalService.getInvViewNoPay(invToken, UserService, PassService);
                }
                else
                {
                    ReturnValue = "";// PortalService.convertForStore(invToken, UserService, PassService);
                }
                if (ReturnValue == "ERR:1")
                {
                    ReturnValue = Einvoice_Customer.Language.Resource.Error1;
                }
                return ReturnValue;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public Byte[] ConvertStringToByteArray(String s)
        {
            return new UnicodeEncoding().GetBytes(s);
        }

        public string HashPassword(string txtpass)
        {
            byte[] data1ToHash = ConvertStringToByteArray(txtpass);
            byte[] hashvalue2 = new MD5CryptoServiceProvider().ComputeHash(data1ToHash);
            return BitConverter.ToString(hashvalue2);
        }

        public string GetMD5(string str)
        {
            UTF8Encoding ue = new UTF8Encoding();
            byte[] bytes = ue.GetBytes(str);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString;
        }
        public async Task<Customer> GetByCusCode(string cusCode)
        {
            using (var chanel = new ChanelFactory())
            {
                var cus = await chanel.GetCusInfo(cusCode);
                return cus;
            }
        }
        public async Task UpdateCus(Customer cus)
        {
            using (var chanel = new ChanelFactory())
            {
                await chanel.PostObjectToWebApi<Customer>(chanel.PortalService, chanel.UpdateCustomerInfoF, cus);
            }
        }
        public CustomerModel ConvertCustomerXMLToCustomerModel(Customer item)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(CustomerModel));
            var cusModel = (CustomerModel)deserializer.Deserialize(new StringReader(item.XML));
            return cusModel;
        }
        public string GeneralXML_Customer(CustomerModel model)
        {
            string mail;
            if (String.IsNullOrEmpty(model.Email))
            {
                mail = System.Configuration.ConfigurationManager.AppSettings["DefaultMailGetInvoice"];
            }
            else
            {
                mail = model.Email;
            }
            string XML = "";
            XML += "<Customer>";
            XML += "<Name><![CDATA[" + model.Name + "]]></Name>";
            XML += "<Code><![CDATA[" + model.Code + "]]></Code>";
            XML += "<TaxCode><![CDATA[" + model.TaxCode + "]]></TaxCode>";
            XML += "<Address><![CDATA[" + model.Address + "]]></Address>";
            XML += "<BankAccountName><![CDATA[" + model.BankAccountName + "]]></BankAccountName>";
            XML += "<BankName><![CDATA[" + model.BankName + "]]></BankName>";
            XML += "<BankNumber><![CDATA[" + model.BankNumber + "]]></BankNumber>";
            XML += "<Email><![CDATA[" + mail + "]]></Email>";
            XML += "<Fax><![CDATA[" + model.Fax + "]]></Fax>";
            XML += "<Phone><![CDATA[" + model.Phone + "]]></Phone>";
            XML += "<ContactPerson><![CDATA[" + model.ContactPerson + "]]></ContactPerson>";
            XML += "<RepresentPerson><![CDATA[" + model.RepresentPerson + "]]></RepresentPerson>";
            XML += "<CusType><![CDATA[" + 0 + "]]></CusType>";
            XML += "</Customer>";
            return XML;
        }
    }
}