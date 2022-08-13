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
    public class POTandCSetupController : Controller
    {
        #region Declaration
        POTandCSetup _oPOTandCSetup = new POTandCSetup();
        List<POTandCSetup> _oPOTandCSetups = new List<POTandCSetup>();
        POTandCClause _oPOTandCClause = new POTandCClause();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewPOTandCSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPOTandCSetups = new List<POTandCSetup>();
            _oPOTandCSetups = POTandCSetup.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oPOTandCSetups);
        }

        public ActionResult ViewPOTandCSetup(int id)
        {
            _oPOTandCSetup = new POTandCSetup();
            if (id > 0)
            {
                _oPOTandCSetup = _oPOTandCSetup.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.POTermsObjs = EnumObject.jGets(typeof(EnumPOTerms));
            return View(_oPOTandCSetup);
        }

        [HttpPost]
        public JsonResult Save(POTandCSetup oPOTandCSetup)
        {
            _oPOTandCSetup = new POTandCSetup();
            try
            {
                _oPOTandCSetup = oPOTandCSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPOTandCSetup = new POTandCSetup();
                _oPOTandCSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPOTandCSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(POTandCSetup oPOTandCSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                _oPOTandCSetup = oPOTandCSetup;
                sFeedBackMessage = oPOTandCSetup.Delete(oPOTandCSetup,((User)Session[SessionInfo.CurrentUser]).UserID);
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
                //oPOTandCSetup.POTandCSetupID = id;
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
        public JsonResult SavePOTandCClause(POTandCClause oPOTandCClause)
        {
            _oPOTandCClause = new POTandCClause();
            try
            {
                _oPOTandCClause = new POTandCClause();
                _oPOTandCClause = oPOTandCClause.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            catch (Exception ex)
            {
                _oPOTandCClause.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oPOTandCClause);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePOTnC(POTandCClause oPOTandCClause)
        {
            string sFeedBackMessage = "";
            try
            {
                
                sFeedBackMessage = oPOTandCClause.Delete(oPOTandCClause, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Gets(POTandCSetup oPOTandCSetup)
        {
            List<POTandCSetup> oPOTandCSetups = new List<POTandCSetup>();

            oPOTandCSetups = POTandCSetup.GetsByBU(oPOTandCSetup.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPOTandCSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult POTandCSetupPicker(double ts)// EnumApplyModule{None = 0,ONS_ODS = 1,Work_Order = 2}
        {
            List<POTandCSetup> oPOTandCSetups = new List<POTandCSetup>();
            oPOTandCSetups = POTandCSetup.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(oPOTandCSetups);
        }

    }
}
