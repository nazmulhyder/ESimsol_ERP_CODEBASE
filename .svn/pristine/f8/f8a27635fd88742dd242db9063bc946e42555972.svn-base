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
    public class DeliveryOrderNameController : Controller
    {
        #region DeliveryOrderName
        public ActionResult ViewDeliveryOrderNames(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            oDeliveryOrderNames = DeliveryOrderName.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)); 
            return View(oDeliveryOrderNames);
        }

        [HttpPost]
        public JsonResult SaveDeliveryOrderName(DeliveryOrderName oDeliveryOrderName)
        {
            try
            {
                oDeliveryOrderName = oDeliveryOrderName.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDeliveryOrderName = new DeliveryOrderName();
                oDeliveryOrderName.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrderName);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDeliveryOrderName(DeliveryOrderName oDeliveryOrderName)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDeliveryOrderName.Delete(oDeliveryOrderName.DeliveryOrderNameID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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