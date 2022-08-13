using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class FabricSalesContractNoteController : Controller
    {
        #region Declaration
        FabricSalesContractNote _oFabricSalesContractNote = new FabricSalesContractNote();
        List<FabricSalesContractNote> _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewFabricSalesContractNotes(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            _oFabricSalesContractNotes = FabricSalesContractNote.Gets(0,((User)Session[SessionInfo.CurrentUser]).UserID);
           ViewBag.BUID = buid;
            return View(_oFabricSalesContractNotes);
        }

        [HttpPost]
        public JsonResult Save(FabricSalesContractNote oFabricSalesContractNote)
        {
            _oFabricSalesContractNote = new FabricSalesContractNote();
            try
            {
                _oFabricSalesContractNote = oFabricSalesContractNote;
                _oFabricSalesContractNote = _oFabricSalesContractNote.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSalesContractNote = new FabricSalesContractNote();
                _oFabricSalesContractNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_FSCNote(FabricSalesContract oFabricSalesContract)
        {
            oFabricSalesContract.RemoveNulls();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            try
            {

                foreach(FabricSalesContractNote oItem in oFabricSalesContract.FabricSalesContractNotes)
                {
                    oItem.FabricSalesContractID = oFabricSalesContract.FabricSalesContractID;
                }
                _oFabricSalesContractNote.SaveAll(oFabricSalesContract.FabricSalesContractNotes, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSalesContract.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSalesContract.FabricSalesContractNotes = oFabricSalesContractNotes;

            }
            catch (Exception ex)
            {
                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(FabricSalesContractNote oFabricSalesContractNote)
        {
            _oFabricSalesContractNote = new FabricSalesContractNote();
            try
            {
                if (oFabricSalesContractNote.FabricSalesContractNoteID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oFabricSalesContractNote = _oFabricSalesContractNote.Get(oFabricSalesContractNote.FabricSalesContractNoteID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSalesContractNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(FabricSalesContractNote oFabricSalesContractNote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage =  oFabricSalesContractNote.Delete( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteAll(FabricSalesContractNote oFabricSalesContractNote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricSalesContractNote.DeleteAll(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Gets(FabricSalesContractNote oFabricSalesContractNote)
        {
            _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            try
            {
                _oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSalesContractNote.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSalesContractNote = new FabricSalesContractNote();
                _oFabricSalesContractNote.ErrorMessage = ex.Message;
                _oFabricSalesContractNotes.Add(_oFabricSalesContractNote);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      

        #endregion
    }
}