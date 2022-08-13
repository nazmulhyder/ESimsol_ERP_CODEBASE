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
using System.Collections;

namespace ESimSolFinancial.Controllers
{
    public class CommentsHistoryController : Controller
    {
        #region CommentsHistory
        public ActionResult ViewCommentsHistorys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<CommentsHistory> oCommentsHistorys = new List<CommentsHistory>();
            oCommentsHistorys = CommentsHistory.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Header ="User Name : "+((User)Session[SessionInfo.CurrentUser]).UserName;
            ViewBag.User = ((User)Session[SessionInfo.CurrentUser]);
            ViewBag.Modules = oCommentsHistorys.GroupBy(x => new { x.ModuleID }, (key, grp) => new
            {
                ModuleID = key.ModuleID,
                ModuleName = grp.First().ModuleName
            }).ToList();
            return View(oCommentsHistorys);
        }
        public ActionResult ViewCommentsHistory(int nModuleID,int nModuleObjID, string sTitle)
        {
            CommentsHistory oCommentsHistory = new CommentsHistory();
            List<CommentsHistory> oCommentsHistorys = new List<CommentsHistory>();
            string sSQL = "SELECT * FROM CommentsHistory WHERE ModuleID =" + nModuleID + " AND ModuleObjID=" + nModuleObjID;
            oCommentsHistorys = CommentsHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            oCommentsHistory.ModuleID = nModuleID;
            oCommentsHistory.ModuleObjID = nModuleObjID;
            ViewBag.CommentsHistory = oCommentsHistory;
            ViewBag.Header = "Comments-By:-" + ((User)Session[SessionInfo.CurrentUser]).UserName +"||"+sTitle;
            ViewBag.User = ((User)Session[SessionInfo.CurrentUser]);
            ViewBag.Modules = EnumObject.jGets(typeof(EnumModuleName));

            return View(oCommentsHistorys);
        }


        [HttpPost]
        public JsonResult SaveCommentsHistory(CommentsHistory oCommentsHistory)
        {
            try
            {
                oCommentsHistory = oCommentsHistory.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oCommentsHistory = new CommentsHistory();
                oCommentsHistory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommentsHistory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCommentsHistory(CommentsHistory oCommentsHistory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oCommentsHistory.Delete(oCommentsHistory.CommentsHistoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(CommentsHistory oCommentsHistory)
        {
            try
            {
                oCommentsHistory = oCommentsHistory.Get(oCommentsHistory.CommentsHistoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oCommentsHistory = new CommentsHistory();
                oCommentsHistory.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommentsHistory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets(int nModuleID)
        {
            List<CommentsHistory> oCommentsHistorys = new List<CommentsHistory>();
            try
            {
                string sSQL = "Select * from View_CommentsHistory Where CommentsHistoryID<>0 And ModuleID ="+nModuleID;

                oCommentsHistorys = CommentsHistory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                CommentsHistory oCommentsHistory = new CommentsHistory();
                oCommentsHistory.ErrorMessage = ex.Message;
                oCommentsHistorys.Add(oCommentsHistory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCommentsHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}