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
    public class LocationBindController : PdfViewController
    {
        #region LB Location

        public ActionResult ViewLocationBinds(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<LB_Location> oLB_Locations = new List<LB_Location>();
            string sSQL = "SELECT * FROM LB_Location";
            oLB_Locations = LB_Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oLB_Locations);
        }

        [HttpPost]
        public JsonResult SaveLocationBind(LB_Location oLB_Location)
          {
            try
            {
                oLB_Location = oLB_Location.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLB_Location = new LB_Location();
                oLB_Location.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_Location);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ClassifiedChange(LB_Location oLB_Location)
        {
            try
            {
                if (oLB_Location.LB_LocationID <= 0) { throw new Exception("Please select an valid item."); }
                oLB_Location.LB_Is_Classified = !oLB_Location.LB_Is_Classified;
                oLB_Location.LB_ClassificationDate = DateTime.Now;
                oLB_Location.LB_ClasifiedBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                oLB_Location = oLB_Location.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLB_Location = new LB_Location();
                oLB_Location.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_Location);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetLB_Location(LB_Location oLB_Location)
        {
            try
            {
                oLB_Location = LB_Location.Get(oLB_Location.LB_LocationID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oLB_Location = new LB_Location();
                oLB_Location.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_Location);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

        #region LB_UserLocationMap
        public JsonResult GetLocationAutoComplete(string LB_IPV4)
       {
            List<LB_Location> oLocations = new List<LB_Location>();
            LB_IPV4 = LB_IPV4 == null ? "" : LB_IPV4;
            string sSql = "select * from LB_Location as HH where HH.LB_Is_Classified='true' and HH.LB_IPV4 LIKE '%" + LB_IPV4 + "%'";
            oLocations = LB_Location.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            var jsonResult = Json(oLocations, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ViewLocationMaps(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LB_UserLocationMap> oLB_UserLocationMaps = new List<LB_UserLocationMap>();


            List<LB_Location> oLB_Locations = new List<LB_Location>();
            string sSQL = "Select * from LB_Location";
            oLB_Locations = LB_Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<User> oUsers = new List<User>();
            sSQL = "Select * from View_User Where UserID<>0";
            oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.LB_Locations = oLB_Locations;
            ViewBag.Users = oUsers;

            return View(oLB_UserLocationMaps);
        }
        [HttpPost]
        public JsonResult SaveUserLocationMap1(LB_UserLocationMap oLB_UserLocationMap)
        {
            
            oLB_UserLocationMap = oLB_UserLocationMap.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
          
            if (oLB_UserLocationMap.LB_UserLocationMapID > 0)
            {
                oLB_UserLocationMap.ErrorMessage = "Successfull";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_UserLocationMap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteLocationMap(LB_UserLocationMap oLB_UserLocationMap)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLB_UserLocationMap.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetLocationByUser(User oUser)
        {
            List<LB_UserLocationMap> oLB_UserLocationMaps = new List<LB_UserLocationMap>();
            string sSql = "select * from View_LB_UserLocationMap where LB_UserID=" + oUser.UserID;
            oLB_UserLocationMaps = LB_UserLocationMap.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_UserLocationMaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost] 
        public JsonResult GetUserByLocation(LB_Location oLB_Location)
        {
            List<LB_UserLocationMap> oLB_UserLocationMaps = new List<LB_UserLocationMap>();
            string sSql = "select * from View_LB_UserLocationMap where LB_LB_LocationID=" + oLB_Location.LB_LocationID;
            oLB_UserLocationMaps = LB_UserLocationMap.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_UserLocationMaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         
        [HttpPost] 
        public JsonResult SaveUserLocationMap(LB_UserLocationMap oLB_UserLocationMap)
        {
            try
            {
                string sLocationIds = oLB_UserLocationMap.Params.Split('~')[0];
                string sUsersIds = oLB_UserLocationMap.Params.Split('~')[1];
               // string.IsNullOrEmpty(sLocationIds) ||
                if (string.IsNullOrEmpty(sUsersIds))
                    throw new Exception("Location or User doesn't foumd");


                List<LB_UserLocationMap> oLBULMs = new List<LB_UserLocationMap>();
                LB_UserLocationMap oLBULM = new LB_UserLocationMap();

                
                    sUsersIds.Split(',').Select(Int32.Parse).ToList().ForEach(x =>
                    {
                        sLocationIds.Split(',').Select(Int32.Parse).ToList().ForEach(p =>
                        {
                            oLBULM = new LB_UserLocationMap();
                            oLBULM.LB_UserID = x;
                            oLBULM.LB_LB_LocationID = p;
                            oLBULM.LB_ExpireDateTime = (oLB_UserLocationMap.LB_ExpireDateTime == DateTime.MinValue) ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)) : oLB_UserLocationMap.LB_ExpireDateTime;
                            oLBULMs.Add(oLBULM);
                        });
                    }
               );

                    oLB_UserLocationMap.LB_UserLocationMaps = oLBULMs;

                    oLB_UserLocationMap = oLB_UserLocationMap.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            catch (Exception ex)
            {
                oLB_UserLocationMap = new LB_UserLocationMap();
                oLB_UserLocationMap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_UserLocationMap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsUserByLocation(LB_Location oLB_Location)
        {
            List<User> oUsers = new List<User>();
            User oUser = new User();
            try
            {
                string sSQL = "Select * from View_User Where UserID In (Select LB_UserID from LB_UserLocationMap Where LB_LB_LocationID=" + oLB_Location.LB_LocationID + ")";
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oUsers = new List<User>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLocationByUser(User oUser)
        {
            //List<LB_Location> oLB_Locations = new List<LB_Location>();
            List<LB_UserLocationMap> oLB_UserLocationMaps = new List<LB_UserLocationMap>();
            try
            {
                //string sSQL = "Select * from LB_Location Where LB_LocationID In (Select LB_LB_LocationID from LB_UserLocationMap Where LB_UserID=" + oUser.UserID + ")";
                //oLB_Locations = LB_Location.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "Select * from View_LB_UserLocationMap Where LB_UserID=" + oUser.UserID + " AND ISNULL(LB_Is_Classified,0) = 1";
                oLB_UserLocationMaps = LB_UserLocationMap.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLB_UserLocationMaps = new List<LB_UserLocationMap>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLB_UserLocationMaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}