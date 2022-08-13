using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;



namespace ESimSolFinancial.Controllers
{
    public class CountryController : PdfViewController
    {
        #region Declaration
        Country _oCountry = new Country();
        List<Country> _oCountrys = new List<Country>();
        #endregion

        public ActionResult ViewCountrys(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Country> oCountrys = new List<Country>();
            oCountrys = Country.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oCountrys);
        }
        public ActionResult ViewCountry(int nId, int buid, double ts)
        {
            Country oCountry = new Country();
            if (nId > 0)
            {
                oCountry = oCountry.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oCountry);
        }

        [HttpPost]
        public JsonResult Save(Country oCountry)
        {
            oCountry.RemoveNulls();
            _oCountry = new Country();
            try
            {
                _oCountry = oCountry.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCountry = new Country();
                _oCountry.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCountry);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Country oCountry)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCountry.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<Country> oCountrys = new List<Country>();
            oCountrys = Country.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCountrys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByName(Country oCountry)
        {
            List<Country> oCountrys = new List<Country>();
            string sSQL = "SELECT * FROM Country";
            if(!string.IsNullOrEmpty(oCountry.Name))
            {
                sSQL +=" Code+Name LIKE '%"+oCountry.Name+"%'";
            }
            oCountrys = Country.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            var jsonResult = Json(oCountrys, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
