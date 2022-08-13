using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ExportTnCCaptionController : Controller
    {
        #region ExportTnCCaption
        public ActionResult ViewExportTnCCaptions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<ExportTnCCaption> oExportTnCCaptions = new List<ExportTnCCaption>();
            oExportTnCCaptions = ExportTnCCaption.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)); 
            return View(oExportTnCCaptions);
        }

        [HttpPost]
        public JsonResult SaveExportTnCCaption(ExportTnCCaption oExportTnCCaption)
        {
            try
            {
                oExportTnCCaption = oExportTnCCaption.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportTnCCaption = new ExportTnCCaption();
                oExportTnCCaption.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportTnCCaption);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExportTnCCaption(ExportTnCCaption oExportTnCCaption)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportTnCCaption.Delete(oExportTnCCaption.ExportTnCCaptionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}