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
    public class ExportTermsAndConditionController : Controller
    {
        #region Declaration
        ExportTermsAndCondition _oExportTermsAndCondition = new ExportTermsAndCondition();
        List<ExportTermsAndCondition> _oExportTermsAndConditions = new List<ExportTermsAndCondition>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult ViewExportTermsAndConditions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportTermsAndConditions = new List<ExportTermsAndCondition>();
            List<ExportTnCCaption> oExportTnCCaptions = new List<ExportTnCCaption>();
            _oExportTermsAndConditions = ExportTermsAndCondition.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportTnCCaptions = ExportTnCCaption.Gets(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumPITerms = EnumObject.jGets(typeof(EnumPITerms));// Enum.GetValues(typeof(EnumPITerms)).Cast<EnumInOutType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumDocFors = EnumObject.jGets(typeof(EnumDocFor));

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BusinessUnits = oBusinessUnits;  //Enum.GetValues(typeof(EnumBusinessUnitType)).Cast<EnumBusinessUnitType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            ViewBag.ExportTnCCaptions = oExportTnCCaptions;
            return View(_oExportTermsAndConditions);
        }

        [HttpPost]
        public JsonResult Save(ExportTermsAndCondition oExportTermsAndCondition)
        {
            _oExportTermsAndCondition = new ExportTermsAndCondition();
            try
            {
                _oExportTermsAndCondition = oExportTermsAndCondition;
                _oExportTermsAndCondition = _oExportTermsAndCondition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTermsAndCondition = new ExportTermsAndCondition();
                _oExportTermsAndCondition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTermsAndCondition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(ExportTermsAndCondition oExportTermsAndCondition)
        {
            _oExportTermsAndCondition = new ExportTermsAndCondition();
            try
            {
                if (oExportTermsAndCondition.ExportTermsAndConditionID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oExportTermsAndCondition = _oExportTermsAndCondition.Get(oExportTermsAndCondition.ExportTermsAndConditionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTermsAndCondition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTermsAndCondition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ExportTermsAndCondition oExportTermsAndCondition)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportTermsAndCondition.Delete(oExportTermsAndCondition.ExportTermsAndConditionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteALL(ExportPITandCClause oExportPITandCClause)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportPITandCClause.DeleteALL( ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ActiveInactive(ExportTermsAndCondition oExportTermsAndCondition)
        {
            _oExportTermsAndCondition = new ExportTermsAndCondition();
            try
            {
                bool bActivity = true;
                bActivity = (oExportTermsAndCondition.Activity == true ? false : true);
                oExportTermsAndCondition.Activity = bActivity;
                _oExportTermsAndCondition = oExportTermsAndCondition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTermsAndCondition = new ExportTermsAndCondition();
                _oExportTermsAndCondition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTermsAndCondition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(ExportTermsAndCondition oExportTermsAndCondition)
        {
            _oExportTermsAndConditions = new List<ExportTermsAndCondition>();
            try
            {
                _oExportTermsAndConditions = ExportTermsAndCondition.Gets(true, oExportTermsAndCondition.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTermsAndCondition = new ExportTermsAndCondition();
                _oExportTermsAndCondition.ErrorMessage = ex.Message;
                _oExportTermsAndConditions.Add(_oExportTermsAndCondition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTermsAndConditions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByType(ExportTermsAndCondition oExportTermsAndCondition)
        {
            _oExportTermsAndConditions = new List<ExportTermsAndCondition>();
            try
            {
                //there In Not field set Doc For values like as 1,2 or 1,3 etc
                _oExportTermsAndConditions = ExportTermsAndCondition.GetsByTypeAndBU(oExportTermsAndCondition.Note, oExportTermsAndCondition.BUID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportTermsAndCondition = new ExportTermsAndCondition();
                _oExportTermsAndCondition.ErrorMessage = ex.Message;
                _oExportTermsAndConditions.Add(_oExportTermsAndCondition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportTermsAndConditions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshSequence(ExportTermsAndCondition oExportTermsAndCondition)
        {
            try
            {
                oExportTermsAndCondition = oExportTermsAndCondition.RefreshSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportTermsAndCondition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportTermsAndCondition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}