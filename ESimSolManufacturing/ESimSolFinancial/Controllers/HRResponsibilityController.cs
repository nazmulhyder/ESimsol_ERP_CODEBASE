using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using iTextSharp;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{

    public class HRResponsibilityController : Controller
    {
        #region Declartion
        HRResponsibility _oHRResponsibility = new HRResponsibility();
        List<HRResponsibility> _oHRResponsibilitys = new List<HRResponsibility>();
        #endregion


        public ActionResult ViewHRResponsibilitys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oHRResponsibilitys = new List<HRResponsibility>();

            _oHRResponsibilitys = HRResponsibility.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COS = oTempClientOperationSetting;
            return View(_oHRResponsibilitys);
        }

        public ActionResult ViewHRResponsibility(int id)
        {
           
            _oHRResponsibility = new HRResponsibility();

            if (id > 0)
            {
                _oHRResponsibility = _oHRResponsibility.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oHRResponsibility);
        }

        [HttpPost]
        public JsonResult Save(HRResponsibility oHRResponsibility)
        {
            _oHRResponsibility = new HRResponsibility();
            try
            {
                _oHRResponsibility = oHRResponsibility.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oHRResponsibility.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRResponsibility);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(HRResponsibility oHRResponsibility)
        {
            string sErrorMease = "";
            try
            {
                _oHRResponsibility = new HRResponsibility();
                sErrorMease = _oHRResponsibility.Delete(oHRResponsibility.HRRID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetHRResponsibilitys()
        {
            List<HRResponsibility> oHRResponsibilitys = new List<HRResponsibility>();
            try
            {
                oHRResponsibilitys = HRResponsibility.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oHRResponsibilitys = new List<HRResponsibility>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHRResponsibilitys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HR Responsibility Picker
        public ActionResult HRResponsibilitySearch()
        {
            List<HRResponsibility> oHRResponsibilitys = new List<HRResponsibility>();
            oHRResponsibilitys = HRResponsibility.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oHRResponsibilitys);
        }
        #endregion

        [HttpPost]
        public JsonResult getHRResponsibility(HRResponsibility oHRResponsibility)
        {
            try
            {
                _oHRResponsibility = new HRResponsibility();
                _oHRResponsibility = _oHRResponsibility.Get(oHRResponsibility.HRRID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oHRResponsibilitys = new List<HRResponsibility>();
                _oHRResponsibility.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRResponsibility);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
