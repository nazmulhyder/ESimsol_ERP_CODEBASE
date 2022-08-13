using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Dynamic;

namespace ESimSolFinancial.Controllers
{
    public class FabricProductionFaultController : Controller
    {

        #region FabricProductionFault
        public ActionResult View_FabricProductionFaults(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FabricProductionFault> oFabricProductionFaults = new List<FabricProductionFault>();
            oFabricProductionFaults = FabricProductionFault.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricProductionFaultes = FabricFaultTypeObj.Gets();
            ViewBag.BUTypes = EnumObject.jGets(typeof(EnumBusinessUnitType)).Where(x => x.id == (int)EnumBusinessUnitType.None || x.id == (int)EnumBusinessUnitType.Dyeing || x.id == (int)EnumBusinessUnitType.Weaving || x.id == (int)EnumBusinessUnitType.Finishing);
            return View(oFabricProductionFaults);
        }

        [HttpPost]
        public JsonResult Save(FabricProductionFault oFabricProductionFault)
        {
            try
            {
                oFabricProductionFault = oFabricProductionFault.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricProductionFault);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RefreshMenuSequence(List<FabricProductionFault> oFabricProductionFaults)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            List<FabricProductionFault> _FabricProductionFaults = new List<FabricProductionFault>();

            try
            {
                _FabricProductionFaults = oFabricProductionFault.RefreshSequence(oFabricProductionFaults,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _FabricProductionFaults = new List<FabricProductionFault>();
                 oFabricProductionFault = new FabricProductionFault();
                 oFabricProductionFault.ErrorMessage = ex.Message;
                _FabricProductionFaults.Add(oFabricProductionFault);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_FabricProductionFaults);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ActiveOrInactive(FabricProductionFault oFabricProductionFault)
        {
            try
            {
                oFabricProductionFault = oFabricProductionFault.ActiveOrInactive(oFabricProductionFault.FPFID, oFabricProductionFault.IsActive, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricProductionFault);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricProductionFault oFabricProductionFault)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricProductionFault.Delete(oFabricProductionFault.FPFID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
