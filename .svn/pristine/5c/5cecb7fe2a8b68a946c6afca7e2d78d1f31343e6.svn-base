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
    public class ClaimReasonController : PdfViewController
    {
        #region Declaration
        ClaimReason _oClaimReason = new ClaimReason();
        List<ClaimReason> _oClaimReasons = new List<ClaimReason>();
        #endregion

        public ActionResult ViewClaimReasons(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ClaimReason> oClaimReasons = new List<ClaimReason>();
            oClaimReasons = ClaimReason.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oClaimReasons);
        }
        public ActionResult ViewClaimReason(int nId, int buid, double ts)
        {
            ClaimReason oClaimReason = new ClaimReason();
            if (nId > 0)
            {
                oClaimReason = oClaimReason.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oClaimReason.BUID = buid;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OperationTypes = EnumObject.jGets(typeof(EnumClaimOperationType));
            return View(oClaimReason);
        }

        [HttpPost]
        public JsonResult Save(ClaimReason oClaimReason)
        {
            oClaimReason.RemoveNulls();
            _oClaimReason = new ClaimReason();
            try
            {
                _oClaimReason = oClaimReason.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oClaimReason = new ClaimReason();
                _oClaimReason.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oClaimReason);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ClaimReason oClaimReason)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oClaimReason.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ActivateClaimReason(ClaimReason oClaimReason)
        {
            _oClaimReason = new ClaimReason();
            _oClaimReason = oClaimReason.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oClaimReason);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<ClaimReason> oClaimReasons = new List<ClaimReason>();
            oClaimReasons = ClaimReason.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oClaimReasons);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
