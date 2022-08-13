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
    public class CurrencyConversionController : PdfViewController
    {
        #region Declaration
        CurrencyConversion _oCurrencyConversion = new CurrencyConversion();
        List<CurrencyConversion> _oCurrencyConversions = new List<CurrencyConversion>();
        #endregion

        public ActionResult ViewCurrencyConversions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<CurrencyConversion> oCurrencyConversions = new List<CurrencyConversion>();
            oCurrencyConversions = CurrencyConversion.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oCurrencyConversions);
        }
        public ActionResult ViewCurrencyConversion(int nId, int buid, double ts)
        {
            CurrencyConversion oCurrencyConversion = new CurrencyConversion(); //oCurrencyConversion.ConversionRate;
            if (nId > 0)
            {
                oCurrencyConversion = oCurrencyConversion.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU((int)EnumOperationalDept.Import_Own + "," + (int)EnumOperationalDept.Export_Own + "," + (int)EnumOperationalDept.Accounts, buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oCurrencyConversion);
        }

        [HttpPost]
        public JsonResult Save(CurrencyConversion oCurrencyConversion)
        {
            oCurrencyConversion.RemoveNulls();
            _oCurrencyConversion = new CurrencyConversion();
            try
            {
                _oCurrencyConversion = oCurrencyConversion.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCurrencyConversion = new CurrencyConversion();
                _oCurrencyConversion.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCurrencyConversion);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                CurrencyConversion oCurrencyConversion = new CurrencyConversion();
                sFeedBackMessage = oCurrencyConversion.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<CurrencyConversion> oCurrencyConversions = new List<CurrencyConversion>();
            oCurrencyConversions = CurrencyConversion.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCurrencyConversions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
