using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class LCContactController : Controller
    {
        #region Declaration
        LCContact _oLCContact = new LCContact();
        List<LCContact> _oLCContacts = new List<LCContact>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewLCContacts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            
            _oLCContacts = new List<LCContact>();
            _oLCContacts = LCContact.Gets((int)Session[SessionInfo.currentUserID]);
            foreach (LCContact oItem in _oLCContacts)
            {
                oItem.BalanceDate = DateTime.Today;
            }
            return View(_oLCContacts);
        }


        [HttpPost]
        public JsonResult Save(LCContact oLCContact)
        {
            _oLCContact = new LCContact();
            try
            {
                _oLCContact = oLCContact;
                _oLCContact = _oLCContact.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCContact = new LCContact();
                _oLCContact.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCContact);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLCContacts(LCContact oLCContact)
        {
            _oLCContacts = oLCContact.GetsLCContacts((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCContacts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PrintLCContacts(DateTime date)
        {
            _oLCContact = new LCContact();
            _oLCContact.BalanceDate = date;
            try
            {
                _oLCContact.LCContacts = _oLCContact.GetsLCContacts((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLCContact = new LCContact();
                _oLCContacts = new List<LCContact>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oLCContact.Company = oCompany;

            if (_oLCContact.LCContacts.Count != 0)
            {
                string Messge = "LCContact List";
                rptLCContacts oReport = new rptLCContacts();
                byte[] abytes = oReport.PrepareReport(_oLCContact, Messge);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
            
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintLCContactLogs(DateTime FromDate,DateTime ToDate)
        {
            try
            {
                _oLCContact.LCContacts = LCContact.Gets("SELECT * FROM VIEW_LCContactLog WHERE BalanceDate BETWEEN '"+FromDate+"' AND '"+ToDate+"' ",(int)Session[SessionInfo.currentUserID] );
            }
            catch (Exception ex)
            {
                _oLCContact = new LCContact();
                _oLCContacts = new List<LCContact>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oLCContact.Company = oCompany;

            if (_oLCContact.LCContacts.Count != 0)
            {
                string Messge = "LCContact Log List";
                rptLCContactLogs oReport = new rptLCContactLogs();
                byte[] abytes = oReport.PrepareReport(_oLCContact, Messge);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }

            
        }

        
    }
}