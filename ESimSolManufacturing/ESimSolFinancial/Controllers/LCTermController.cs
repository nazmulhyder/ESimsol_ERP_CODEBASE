using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using ESimSol.Reports;

namespace ESimSolFinancial.Controllers
{
    public class LCTermController : Controller
    {
        #region Declaration
        LCTerm _oLCTerm = new LCTerm();
        List<LCTerm> _oLCTerms = new List<LCTerm>();
        string _sErrorMessage = "";
        #endregion
       
        public ActionResult ViewLCTerms(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLCTerms = new List<LCTerm>();
            _oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oLCTerms);
        }

        public ActionResult ViewLCTerm(int id, double tsv)
        {
            _oLCTerm = new LCTerm();
            if (id > 0)
            {
                _oLCTerm = _oLCTerm.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return PartialView(_oLCTerm);
        }

        #region HTTP Post Method
        [HttpPost]
        public JsonResult Save(LCTerm oLCTerm)
        {
            _oLCTerm = new LCTerm();

            try
            {
                _oLCTerm = oLCTerm;
                _oLCTerm = _oLCTerm.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oLCTerm.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTerm);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(LCTerm oLCTerm)
        {
            _oLCTerm = new LCTerm();

            try
            {                
                _oLCTerm = _oLCTerm.Get( oLCTerm.LCTermID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oLCTerm.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLCTerm);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Delete(LCTerm oLCTerm)
        {
            string smessage = "";
            try
            {                
                smessage = oLCTerm.Delete(oLCTerm.LCTermID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Print(double nts)
        {
            LCTerm oLCTerm = new LCTerm();
            List<LCTerm> oLCTerms = new List<LCTerm>();
            oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sMessage = "LC Term List";
            rptLCTerm oReport = new rptLCTerm();
            byte[] abytes = oReport.PrepareReport(oLCTerms, oCompany, sMessage);
            return File(abytes, "application/pdf");
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}
