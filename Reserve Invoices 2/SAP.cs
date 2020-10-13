using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve_Invoices_2
{
    public class SAP
    {
        public static string[] Companies
        {
            get
            {
                return new string[] { "LONGDAN", "KIMSON" };
            }
        }
        public Company oCompany { get; }
        public SAP(string companydb)
        {
            // SAP Server
            oCompany = new Company();

            // Database
            oCompany.CompanyDB = companydb;

            // Server
            oCompany.Server = ConfigurationManager.AppSettings["server"];

            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017;
            oCompany.LicenseServer = ConfigurationManager.AppSettings["license"];
            oCompany.UserName = ConfigurationManager.AppSettings["username"];
            oCompany.Password = ConfigurationManager.AppSettings["password"];
            oCompany.UseTrusted = false;

            oCompany.Connect();
        }
    }
}
