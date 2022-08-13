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

namespace ESimSolFinancial.Controllers
{
    public class RouteLocationController : Controller
    {
        RouteLocation _oRouteLocation = new RouteLocation();

        public ActionResult ViewRouteLocations(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RouteLocation> oRouteLocations = new List<RouteLocation>();
            oRouteLocations = RouteLocation.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oRouteLocations);
        }

        public ActionResult ViewRouteLocation(int id, decimal ts)
        {
            RouteLocation oRouteLocation = new RouteLocation();
            oRouteLocation = oRouteLocation.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oRouteLocation.LocationTypes = EnumObject.jGets(typeof(EnumRouteLocation));
            return View(oRouteLocation);
        }

        #region LoadRouteLocations
        [HttpGet]
        public JsonResult LoadRouteLocation()
        {
            List<enumRouteLocationload> oenumRouteLocationloads = new List<enumRouteLocationload>();
            enumRouteLocationload oenumRouteLocationload = new enumRouteLocationload();
            try
            {
                foreach (int oItem in Enum.GetValues(typeof(EnumRouteLocation)))
                {
                        oenumRouteLocationload = new enumRouteLocationload();
                        oenumRouteLocationload.id = oItem;
                        oenumRouteLocationload.Value = ((EnumRouteLocation)oItem).ToString();
                        oenumRouteLocationloads.Add(oenumRouteLocationload);
                }
            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oenumRouteLocationloads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion 
        [HttpPost]
        public JsonResult Save(RouteLocation oRouteLocation)
        {
            _oRouteLocation = new RouteLocation();
            try
            {
                _oRouteLocation = oRouteLocation;
                if(String.IsNullOrEmpty(oRouteLocation.Description))
                {
                    oRouteLocation.Description="";
                }
                _oRouteLocation = _oRouteLocation.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRouteLocation = new RouteLocation();
                _oRouteLocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRouteLocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(RouteLocation oRouteLocation)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oRouteLocation.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
    }

    public class enumRouteLocationload
    {
        public enumRouteLocationload()
        {
            id = 0;
            Value = "";
        }

        public int id { get; set; }
        public string Value { get; set; }
    }

}
