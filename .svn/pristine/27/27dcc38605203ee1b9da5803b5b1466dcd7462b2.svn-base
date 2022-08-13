using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class DymanicHeadSetupController : Controller
    {
        #region Declaration
        DymanicHeadSetup _oDymanicHeadSetup = new DymanicHeadSetup();
        List<DymanicHeadSetup> _oDymanicHeadSetups = new List<DymanicHeadSetup>();
      
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewDymanicHeadSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDymanicHeadSetups = new List<DymanicHeadSetup>();
            _oDymanicHeadSetups = DymanicHeadSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oDymanicHeadSetups);
        }

        public ActionResult ViewDymanicHeadSetup(int id)
        {
            _oDymanicHeadSetup = new DymanicHeadSetup();
            _oDymanicHeadSetup.ReferenceTypeObjObjs = new List<EnumObject>();
            if (id > 0)
            {
                _oDymanicHeadSetup = _oDymanicHeadSetup.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oDymanicHeadSetup.Activity = true;
            _oDymanicHeadSetup.ReferenceTypeObjObjs = EnumObject.jGets(typeof(EnumReferenceType));
            _oDymanicHeadSetup.ACMappingTypeObjs = EnumObject.jGets(typeof(EnumACMappingType));
            return View(_oDymanicHeadSetup);
        }

        [HttpPost]
        public JsonResult Save(DymanicHeadSetup oDymanicHeadSetup)
        {
            _oDymanicHeadSetup = new DymanicHeadSetup();
            try
            {
                oDymanicHeadSetup.Name = oDymanicHeadSetup.Name == null ? "" : oDymanicHeadSetup.Name;
                oDymanicHeadSetup.Note = oDymanicHeadSetup.Note == null ? "" : oDymanicHeadSetup.Note;
              //  oDymanicHeadSetup.ApplyModule = (EnumApplyModule)oDymanicHeadSetup.ApplyModuleInInt;
                _oDymanicHeadSetup = oDymanicHeadSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDymanicHeadSetup = new DymanicHeadSetup();
                _oDymanicHeadSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDymanicHeadSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DymanicHeadSetup oDymanicHeadSetup)
        {
            string sFeedBackMessage = "";
            try
            {
               
                sFeedBackMessage = oDymanicHeadSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SavePOTandCClauses(List<POTandCClause> oPOTandCClauses)
        {
            string sFeedBackMessage = "";
            try
            {
                POTandCClause oPOTandCClause = new POTandCClause();
                //oDymanicHeadSetup.DymanicHeadSetupID = id;
                sFeedBackMessage = oPOTandCClause.POTandCClauseSave(oPOTandCClauses, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oPOTandCClauses.Count > 0)
                {
                    oPOTandCClauses = POTandCClause.Gets(oPOTandCClauses[0].POID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPOTandCClauses);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Process(DymanicHeadSetup oDymanicHeadSetup)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oDymanicHeadSetup.Process(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DymanicHeadSetupPicker(double ts)// EnumApplyModule{None = 0,ONS_ODS = 1,Work_Order = 2}
        {
            List<DymanicHeadSetup> oDymanicHeadSetups = new List<DymanicHeadSetup>();
            oDymanicHeadSetups = DymanicHeadSetup.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oDymanicHeadSetups);
        }

    }
}
