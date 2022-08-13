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
    public class LightSourceController : Controller
    {
        #region LightSource
        public ActionResult ViewLightSources(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<LightSource> oLightSources = new List<LightSource>();
            oLightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oLightSources);
        }

        [HttpPost]
        public JsonResult SaveLightSource(LightSource oLightSource)
        {
            try
            {
                oLightSource = oLightSource.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLightSource = new LightSource();
                oLightSource.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLightSource);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLightSource(LightSource oLightSource)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLightSource.Delete(oLightSource.LightSourceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsLightSource(LightSource oLightSource)
        {
            List<LightSource> oLightSources = new List<LightSource>();
            try
            {
                oLightSources = LightSource.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLightSource.ErrorMessage = ex.Message;
                oLightSources.Add(oLightSource);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLightSources);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}