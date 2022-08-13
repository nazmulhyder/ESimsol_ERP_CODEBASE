using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class ExportBillParticularController : Controller
    {
        #region Declaration
        ExportBillParticular _oExportBillParticular = new ExportBillParticular();
        List<ExportBillParticular> _oExportBillParticulars = new List<ExportBillParticular>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewExportBillParticulars(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);

            _oExportBillParticulars = new List<ExportBillParticular>();
            _oExportBillParticulars = ExportBillParticular.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumInOutTypes = Enum.GetValues(typeof(EnumInOutType)).Cast<EnumInOutType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oExportBillParticulars);
        }

        public ActionResult ViewExportBillParticular(int id)
        {
            _oExportBillParticular = new ExportBillParticular();
            if (id > 0)
            {
                _oExportBillParticular = _oExportBillParticular.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return PartialView(_oExportBillParticular);
        }

         [HttpPost]
        public JsonResult Save(ExportBillParticular oExportBillParticular)
        {
            _oExportBillParticular = new ExportBillParticular();
            try
            {
                _oExportBillParticular = oExportBillParticular;
                _oExportBillParticular.InOutType = (EnumInOutType)oExportBillParticular.InOutTypeInInt;
                _oExportBillParticular = _oExportBillParticular.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportBillParticular = new ExportBillParticular();
                _oExportBillParticular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBillParticular);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
         public JsonResult Get(ExportBillParticular oExportBillParticular)
         {
             _oExportBillParticular = new ExportBillParticular();
             try
             {
                 if (oExportBillParticular.ExportBillParticularID <= 0) { throw new Exception("Please select a valid contractor."); }
                 _oExportBillParticular = _oExportBillParticular.Get(oExportBillParticular.ExportBillParticularID, ((User)Session[SessionInfo.CurrentUser]).UserID);
             }
             catch (Exception ex)
             {
                 _oExportBillParticular.ErrorMessage = ex.Message;
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oExportBillParticular);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        [HttpPost]
         public JsonResult Delete(ExportBillParticular oExportBillParticular)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportBillParticular.Delete(oExportBillParticular.ExportBillParticularID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ActiveInactive(ExportBillParticular oExportBillParticular)
        {
            _oExportBillParticular = new ExportBillParticular();
            try
            {
                bool bActivity = true;
                _oExportBillParticular = _oExportBillParticular.Get(oExportBillParticular.ExportBillParticularID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                bActivity = (_oExportBillParticular.Activity == true ? false : true);
                _oExportBillParticular.Activity = bActivity;
                _oExportBillParticular = _oExportBillParticular.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportBillParticular = new ExportBillParticular();
                _oExportBillParticular.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportBillParticular);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Get
        [HttpPost]
        public JsonResult GetsAll()
        {
            _oExportBillParticulars = new List<ExportBillParticular>();
            _oExportBillParticulars = ExportBillParticular.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oExportBillParticulars);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
      
    }

  
}