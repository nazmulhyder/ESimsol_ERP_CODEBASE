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
    public class ExportQualityController : Controller
    {
        #region ExportQuality
        public ActionResult ViewExportQualitys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<ExportQuality> oExportQualitys = new List<ExportQuality>();
            oExportQualitys = ExportQuality.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oExportQualitys);
        }

        [HttpPost]
        public JsonResult SaveExportQuality(ExportQuality oExportQuality)
        {
            try
            {
                oExportQuality = oExportQuality.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportQuality = new ExportQuality();
                oExportQuality.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportQuality);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExportQuality(ExportQuality oExportQuality)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportQuality.Delete(oExportQuality.ExportQualityID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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