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
    public class ImportLetterSetupController : Controller
    {
        #region Declaration
        ImportLetterSetup _oImportLetterSetup = new ImportLetterSetup();
        List<ImportLetterSetup> _oImportLetterSetups = new List<ImportLetterSetup>();
        #endregion

        public ActionResult View_ImportLetterSetups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();
            oImportLetterSetups = ImportLetterSetup.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.ImportLetterTypes = EnumObject.jGets(typeof(EnumImportLetterType));
            ViewBag.ImportLetterIssueTo = EnumObject.jGets(typeof(EnumImportLetterIssueTo));
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<BankBranch> oBankBranchs = new List<BankBranch>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;

            List<EnumObject> oLCAppTypeObjs = new List<EnumObject>();
            List<EnumObject> oLCAppTypes = new List<EnumObject>();
            oLCAppTypeObjs = EnumObject.jGets(typeof(EnumLCAppType));
            ImportSetup oImportSetup = new ImportSetup(); 
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (EnumObject oItem in oLCAppTypeObjs)
            {
                if (oItem.id == (int)EnumLCAppType.LC)
                {
                    oLCAppTypes.Add(oItem);
                }
                
                if (oImportSetup.IsApplyMasterLC == true && oItem.id == (int)EnumLCAppType.B2BLC)
                {
                    oLCAppTypes.Add(oItem);
                }
                if (oImportSetup.IsApplyTT == true && oItem.id == (int)EnumLCAppType.TT)
                {
                    oLCAppTypes.Add(oItem);
                }


            }
            oBankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.LCAppTypes = oLCAppTypes;
            ViewBag.BankBranchs = oBankBranchs;
            ViewBag.ProductNature = EnumObject.jGets(typeof(EnumProductNature));
            return View(oImportLetterSetups);
        }

        public ActionResult ViewSetupSequence(int buid, double ts)
        {
            int nSequence = 0;
            _oImportLetterSetups = new List<ImportLetterSetup>();
            List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();            
            oImportLetterSetups = ImportLetterSetup.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]);

            foreach (ImportLetterSetup oItem in oImportLetterSetups)
            {
                nSequence++;
                _oImportLetterSetup = new ImportLetterSetup();
                _oImportLetterSetup.Sequence = nSequence;
                _oImportLetterSetup.ImportLetterSetupID = oItem.ImportLetterSetupID;
                _oImportLetterSetup.LetterName = oItem.LetterName;
                _oImportLetterSetups.Add(_oImportLetterSetup);
            }
            return PartialView(_oImportLetterSetups);
        }

        [HttpPost]
        public JsonResult UpdateSequence(ImportLetterSetup oImportLetterSetup)
        {
            _oImportLetterSetups = new List<ImportLetterSetup>();
            try
            {
                _oImportLetterSetups = ImportLetterSetup.UpdateSequence(oImportLetterSetup, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportLetterSetup = new ImportLetterSetup();
                _oImportLetterSetups = new List<ImportLetterSetup>();
                _oImportLetterSetup.ErrorMessage = ex.Message;
                _oImportLetterSetups.Add(_oImportLetterSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLetterSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    

        [HttpPost]
        public JsonResult Save(ImportLetterSetup oImportLetterSetup)
        {
            oImportLetterSetup.RemoveNulls();
            _oImportLetterSetup = new ImportLetterSetup();
            try
            {
                _oImportLetterSetup = oImportLetterSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLetterSetup = new ImportLetterSetup();
                _oImportLetterSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveCopy(ImportLetterSetup oImportLetterSetup)
        {
            oImportLetterSetup.RemoveNulls();
            _oImportLetterSetup = new ImportLetterSetup();
            try
            {

                oImportLetterSetup.ImportLetterSetupID = 0;
                _oImportLetterSetup = oImportLetterSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLetterSetup = new ImportLetterSetup();
                _oImportLetterSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ActivateImportLetterSetup(ImportLetterSetup oImportLetterSetup)
        {
            _oImportLetterSetup = new ImportLetterSetup();
            string sMsg = "";
            _oImportLetterSetup = oImportLetterSetup.Activate(oImportLetterSetup, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oImportLetterSetup.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLetterSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ImportLetterSetup oImportLetterSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oImportLetterSetup.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();
            oImportLetterSetups = ImportLetterSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oImportLetterSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ImportLetterSetupPicker(double ts)// EnumApplyModule{None = 0,ONS_ODS = 1,Work_Order = 2}
        //{
        //    List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();
        //    oImportLetterSetups = ImportLetterSetup.Gets(true, (Guid)Session[SessionInfo.wcfSessionID]);
        //    return PartialView(oImportLetterSetups);
        //}

    }
}
