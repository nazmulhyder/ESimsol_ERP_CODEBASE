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
    public class FDONoteController : Controller
    {
        #region FDONote
        public ActionResult ViewFDONote(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<FDONote> oFDONotes = new List<FDONote>();
            //oFDONotes = FDONote.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFDONotes);
        }

        [HttpPost]
        public JsonResult DeleteFDONote(FDONote oFDONote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFDONote.Delete(oFDONote.FDONoteID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsByDO(FDONote oFDONote)
        {
            List<FDONote> oFDONotes = new List<FDONote>();
            try
            {
                oFDONotes = FDONote.GetByOrderID(oFDONote.FDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFDONote = new FDONote();
                oFDONote.ErrorMessage = ex.Message;
                oFDONotes.Add(oFDONote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDONotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Save_DONote(FDONote oFDONote)
        {
           
            try
            {
                foreach (FDONote oItem in oFDONote.FDONotes)
                {
                    oItem.FDOID = oFDONote.FDOID;
                }
                oFDONote.ErrorMessage=oFDONote.SaveAll(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFDONote = new FDONote();
                oFDONote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFDONote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}