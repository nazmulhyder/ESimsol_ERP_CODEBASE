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
    public class DyeingOrderNoteController : Controller
    {
        #region DyeingOrderNote
        public ActionResult ViewDyeingOrderNote(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            oDyeingOrderNotes = DyeingOrderNote.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oDyeingOrderNotes);
        }

        [HttpPost]
        public JsonResult SaveDyeingOrderNote(DyeingOrderNote oDyeingOrderNote)
        {
            try
            {
                oDyeingOrderNote = oDyeingOrderNote.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDyeingOrderNote(DyeingOrderNote oDyeingOrderNote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDyeingOrderNote.Delete(oDyeingOrderNote.DyeingOrderNoteID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Gets(DyeingOrderNote oDyeingOrderNote)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            try
            {
                string sSQL = "Select * from DyeingOrderNote Where DyeingOrderNoteID<>0 And OrderNote Like '%%'";

                if (!string.IsNullOrEmpty(oDyeingOrderNote.OrderNote))
                {
                    sSQL = sSQL + " And OrderNote Like '%" + oDyeingOrderNote.OrderNote.Trim() + "%'";
                }

                oDyeingOrderNotes = DyeingOrderNote.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = ex.Message;
                oDyeingOrderNotes.Add(oDyeingOrderNote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByDO(DyeingOrderNote oDyeingOrderNote)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            try
            {
                oDyeingOrderNotes = DyeingOrderNote.GetByOrderID(oDyeingOrderNote.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = ex.Message;
                oDyeingOrderNotes.Add(oDyeingOrderNote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByCon(DyeingOrderNote oDyeingOrderNote)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            try
            {
                oDyeingOrderNotes = DyeingOrderNote.GetByConID(oDyeingOrderNote, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = ex.Message;
                oDyeingOrderNotes.Add(oDyeingOrderNote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_DONote(DyeingOrderNote oDyeingOrderNote)
        {
           
            try
            {
                foreach (DyeingOrderNote oItem in oDyeingOrderNote.DyeingOrderNotes)
                {
                    oItem.DyeingOrderID = oDyeingOrderNote.DyeingOrderID;
                }
                oDyeingOrderNote.ErrorMessage=oDyeingOrderNote.SaveAll(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}