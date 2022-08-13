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
    public class DeliveryZoneController : Controller
    {
        #region DeliveryZone
        public ActionResult ViewDeliveryZone(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            oDeliveryZones = DeliveryZone.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDeliveryZones);
        }

        [HttpPost]
        public JsonResult SaveDeliveryZone(DeliveryZone oDeliveryZone)
        {
            try
            {
                oDeliveryZone = oDeliveryZone.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryZone);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDeliveryZone(DeliveryZone oDeliveryZone)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDeliveryZone.Delete(oDeliveryZone.DeliveryZoneID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(DeliveryZone oDeliveryZone)
        {
            try
            {
                oDeliveryZone = oDeliveryZone.Get(oDeliveryZone.DeliveryZoneID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryZone);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(DeliveryZone oDeliveryZone)
        {
            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            try
            {
                string sSQL = "Select * from DeliveryZone Where DeliveryZoneID<>0 And DeliveryZoneName Like '%%'";

                if (!string.IsNullOrEmpty(oDeliveryZone.DeliveryZoneName))
                {
                    sSQL = sSQL + " And DeliveryZoneName Like '%" + oDeliveryZone.DeliveryZoneName.Trim() + "%'";
                }

                oDeliveryZones = DeliveryZone.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = ex.Message;
                oDeliveryZones.Add(oDeliveryZone);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryZones);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsByContractor(DeliveryZone oDeliveryZone)
        {
            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            try
            {
                string sSQL = "Select * from DeliveryZone Where DeliveryZoneID<>0";

                if (!string.IsNullOrEmpty(oDeliveryZone.DeliveryZoneName))
                {
                    sSQL = sSQL + " And DeliveryZoneName Like '%" + oDeliveryZone.DeliveryZoneName.Trim() + "%'";
                }
                if (!string.IsNullOrEmpty(oDeliveryZone.ErrorMessage))
                {
                    sSQL = sSQL + "And DeliveryZoneID in  (Select DeliveryZoneID from DyeingOrder where ContractorID in (" + oDeliveryZone.ErrorMessage + ") or DeliveryToID in (" + oDeliveryZone.ErrorMessage + "))";
                }

                oDeliveryZones = DeliveryZone.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = ex.Message;
                oDeliveryZones.Add(oDeliveryZone);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryZones);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }




        #endregion
    }
}