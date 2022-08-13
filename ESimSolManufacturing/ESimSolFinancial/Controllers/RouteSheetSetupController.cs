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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class RouteSheetSetupController : Controller
    {
        #region Declaration
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();
        List<RouteSheetSetup> _oRouteSheetSetups = new List<RouteSheetSetup>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion

        public ActionResult ViewRouteSheetSetup(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRouteSheetSetup = new RouteSheetSetup();
            _oRouteSheetSetup = _oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Weight, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PrintNoList = EnumObject.jGets(typeof(EnumExcellColumn));
            ViewBag.RestartByList = EnumObject.jGets(typeof(EnumRestartPeriod));
            ViewBag.DyesChemicalViewTypes = EnumObject.jGets(typeof(EnumDyesChemicalViewType));

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.DCEntryValType = EnumObject.jGets(typeof(EnumDCEntryType));
            ViewBag.RSStateForCost = EnumObject.jGets(typeof(EnumRSState));
            ViewBag.WorkingUnits = oWorkingUnits;

            return View(_oRouteSheetSetup);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(RouteSheetSetup oRouteSheetSetup)
        {
            _oRouteSheetSetup = new RouteSheetSetup();
            try
            {
                _oRouteSheetSetup = oRouteSheetSetup;
                _oRouteSheetSetup = _oRouteSheetSetup.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oRouteSheetSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteSheetSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
                smessage = oRouteSheetSetup.Delete(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        


    }
}
