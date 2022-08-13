using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class NegativeLedgerController : Controller
    {
        #region Declaration
        NegativeLedger _oNegativeLedger = new NegativeLedger();
        List<NegativeLedger> _oNegativeLedgers = new List<NegativeLedger>();
        string _sErrorMessage = "";
        #endregion
        public ActionResult ViewNegativeLedgers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oNegativeLedgers = new List<NegativeLedger>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.NegativeLedger).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oNegativeLedgers = NegativeLedger.Gets((int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oNegativeLedgers);
        }
        public ActionResult ViewGeneralLedger()
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));
            oSP_GeneralLedger.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oSP_GeneralLedger);
        }

        public ActionResult PrintNegativeLedgerInXL(string sParams)
        {
                _oNegativeLedgers = new List<NegativeLedger>();
                _oNegativeLedgers = NegativeLedger.Gets((int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);
                var stream = new MemoryStream();
                var serializer = new XmlSerializer(typeof(List<NegativeLedgerXL>));

                int nCount = 0; 
                double nTotalDebit = 0; 
                double nTotalCredit = 0;
                NegativeLedgerXL oNegativeLedgerXL = new NegativeLedgerXL();
                List<NegativeLedgerXL> oNegativeLedgerXLs = new List<NegativeLedgerXL>();
                foreach (NegativeLedger oItem in _oNegativeLedgers)
                {
                    nCount++;
                    oNegativeLedgerXL = new NegativeLedgerXL();
                    oNegativeLedgerXL.SLNo = nCount.ToString();
                    oNegativeLedgerXL.AccountHeadName = oItem.AccountHeadName;
                    oNegativeLedgerXL.OpeningBalance = oItem.OpenningBalance;
                    oNegativeLedgerXL.DebitAmount = oItem.DebitAmount;
                    oNegativeLedgerXL.CreditAmount = oItem.CreditAmount;
                    oNegativeLedgerXL.ClosingBalance = oItem.ClosingBalance;
                    oNegativeLedgerXLs.Add(oNegativeLedgerXL);
                    nTotalDebit = nTotalDebit + oItem.DebitAmount;
                    nTotalCredit = nTotalCredit + oItem.CreditAmount;
                }

                #region Total
                oNegativeLedgerXL = new NegativeLedgerXL();
                oNegativeLedgerXL.SLNo = "";
                oNegativeLedgerXL.AccountHeadName = "Total :";
                oNegativeLedgerXL.OpeningBalance = 0;
                oNegativeLedgerXL.DebitAmount = nTotalDebit;
                oNegativeLedgerXL.CreditAmount = nTotalCredit;
                oNegativeLedgerXL.ClosingBalance = 0;
                oNegativeLedgerXLs.Add(oNegativeLedgerXL);
                #endregion

                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oNegativeLedgerXLs);
                stream.Position = 0;

                //We return the XML from the memory as a .xls file
                return File(stream, "application/vnd.ms-excel", "Orders.xls");
        }

        public ActionResult PrintNegativeLedger(string sParams)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            _oNegativeLedgers = new List<NegativeLedger>();
            _oNegativeLedger.NegativeLedgers = NegativeLedger.Gets((int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oNegativeLedger.Company = oCompany;
            rptNegativeLedger orptNegativeLedger = new rptNegativeLedger();
            byte[] abytes = orptNegativeLedger.PrepareReport(_oNegativeLedger);
            return File(abytes, "application/pdf");
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}
