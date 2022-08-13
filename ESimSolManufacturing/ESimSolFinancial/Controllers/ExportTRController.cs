using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class ExportTRController : Controller
    {
        #region Declaration
        ExportTR _oExportTR = new ExportTR();
        List<ExportTR> _oExportTRs = new List<ExportTR>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewExportTRs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportTRs = new List<ExportTR>();
            _oExportTRs = ExportTR.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oExportTRs);
        }

        [HttpPost]
        public JsonResult PickExportTRs(ExportTR oExportTR) 
        {
            _oExportTRs = new List<ExportTR>();
            try
            {

                _oExportTRs = ExportTR.Gets(true, oExportTR.BUID,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportTRs.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oExportTR = new ExportTR();
                oExportTR.ErrorMessage = ex.Message;
                _oExportTRs.Add(oExportTR);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(ExportTR oExportTR)
        {
            _oExportTR = new ExportTR();
            try
            {
                _oExportTR = oExportTR;
                //_oExportTR.TruckReceiptDate = Convert.ToDateTime(oExportTR.TruckReceiptDateString);
                _oExportTR = _oExportTR.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTR = new ExportTR();
                _oExportTR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(ExportTR oExportTR)
        {
            _oExportTR = new ExportTR();
            try
            {
                if (oExportTR.ExportTRID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oExportTR = _oExportTR.Get(oExportTR.ExportTRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ExportTR oExportTR)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oExportTR.Delete(oExportTR.ExportTRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActiveInactive(ExportTR oExportTR)
        {
            _oExportTR = new ExportTR();
            try
            {
                bool bActivity = true;
                bActivity = (oExportTR.Activity == true ? false : true);
                oExportTR.Activity = bActivity;
                _oExportTR = oExportTR.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTR = new ExportTR();
                _oExportTR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}