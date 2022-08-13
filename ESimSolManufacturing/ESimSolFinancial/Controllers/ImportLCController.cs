using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Globalization;
using System.Linq;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ImportLCController : Controller
    {
        #region 
        string _sErrorMessage = "";
        ImportLC _oImportLC = new ImportLC();
        List<ImportLC> _oImportLCs = new List<ImportLC>();
        ImportLCDetail _oPurchasePaymentContact = new ImportLCDetail();
        ImportLCDetailProduct _oImportLCDetailProduct = new ImportLCDetailProduct();
        private string sReturn = "";
        public ActionResult ViewImportLCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oImportLCs = new List<ImportLC>();
            _oImportLCs = ImportLC.GetsByStatus( ((int)EnumLCCurrentStatus.ReqForLC).ToString(), buid,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatusObjs = EnumObject.jGets(typeof(EnumLCCurrentStatus));
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType)).Where(x => x.id == (int)EnumImportPIType.Foreign || x.id == (int)EnumImportPIType.Local || x.id == (int)EnumImportPIType.NonLC || x.id == (int)EnumImportPIType.TTForeign || x.id == (int)EnumImportPIType.TTLocal);
            return View(_oImportLCs);
        }
        public ActionResult ViewImportLCDetails(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oImportLCs = new List<ImportLC>();
            _oImportLCs = ImportLC.GetsByStatus(((int)EnumLCCurrentStatus.LC_Open).ToString(), 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oImportLCs);
        }
        public ActionResult ViewImportLC(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();
            ImportSetup oImportSetup = new ImportSetup(); 
            _oImportLC = new ImportLC();
            oImportSetup = oImportSetup.GetByBU(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                _oImportLC = _oImportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLCDetails = ImportLCDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLC_Clauses = ImportLC_Clause.Gets(id, 0, (int)EnumLCCurrentStatus.ReqForLC, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oImportLC.LCAppType == EnumLCAppType.B2BLC) 
                {
                    oImportMasterLCs = ImportMasterLC.Gets(_oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);

             ViewBag.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.LCChargeTypes = EnumObject.jGets(typeof(EnumLCChargeType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.LCTerms = oLCTerms;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.Currencys = oCurrencys;

            List<EnumObject> oLCAppTypeObjs = new List<EnumObject>();
            List<EnumObject> oLCAppTypes = new List<EnumObject>();
            oLCAppTypeObjs = EnumObject.jGets(typeof(EnumLCAppType));

            foreach (EnumObject oItem in oLCAppTypeObjs)
            {
                if ( oItem.id == (int)EnumLCAppType.LC)
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
            ViewBag.LCAppTypes = oLCAppTypes;
            ViewBag.ImportMasterLCs = oImportMasterLCs;

            return View(_oImportLC);
        }
        public ActionResult ViewImportLCLog(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            ImportSetup oImportSetup = new ImportSetup();
            _oImportLC = new ImportLC();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                _oImportLC = _oImportLC.GetLog(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oImportLC.ImportLCLogID <= 0)
                {
                    _oImportLC = _oImportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oImportLC.ImportLCDetails = ImportLCDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oImportLC.ImportLC_Clauses = ImportLC_Clause.Gets(id, _oImportLC.ImportLCLogID, (int)_oImportLC.LCCurrentStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ImportLCDetail oItem in _oImportLC.ImportLCDetails)
                    {
                        if (oItem.AmendmentDate == DateTime.MinValue)
                        {
                            oItem.AmendmentDate = _oImportLC.ImportLCDate;
                        }
                    }
                }
                else
                {
                    _oImportLC.ImportLCDetails = ImportLCDetail.GetsLog(_oImportLC.ImportLCLogID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oImportLC.ImportLC_Clauses = ImportLC_Clause.Gets(id, _oImportLC.ImportLCLogID, (int)_oImportLC.LCCurrentStatus, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.LCChargeTypes = EnumObject.jGets(typeof(EnumLCChargeType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.LCTerms = oLCTerms;
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.Currencys = oCurrencys;

            List<EnumObject> oLCAppTypeObjs = new List<EnumObject>();
            List<EnumObject> oLCAppTypes = new List<EnumObject>();
            oLCAppTypeObjs = EnumObject.jGets(typeof(EnumLCAppType));

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
            ViewBag.LCAppTypes = oLCAppTypes;

            return View(_oImportLC);
        }
        public ActionResult ViewImportLCReceive(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            _oImportLC = new ImportLC();
            ImportSetup oImportSetup = new ImportSetup();
            _oImportLC = new ImportLC();
            oImportSetup = oImportSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                _oImportLC = _oImportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLCDetails = ImportLCDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (string.IsNullOrEmpty(_oImportLC.ReceivedByUserName))
            {
                _oImportLC.ReceivedByUserName = ((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.ImportSetup = oImportSetup;
            ViewBag.LCTerms = oLCTerms;
            ViewBag.Currencys = oCurrencys;
            return View(_oImportLC);
        }
        public ActionResult ViewImportLCRevise(int id, int buid)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            List<Currency> oCurrencys = new List<Currency>();
            _oImportLC = new ImportLC();
            if (id > 0)
            {
                _oImportLC = _oImportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLCDetails = ImportLCDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLC_Clauses = ImportLC_Clause.GetsByImportLCID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.LCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.ShipmentByObj = EnumObject.jGets(typeof(EnumShipmentBy));
            oLCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentInstructionObj = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.LCTerms = oLCTerms;
            ViewBag.Currencys = oCurrencys;
            return View(_oImportLC);
        }
        #endregion
        #region HTTP Save

        [HttpPost]
        public JsonResult Save(ImportLC oImportLC)
        {
            try
            {
                _oImportLC = oImportLC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.ImportLCDetails = ImportLCDetail.Gets(_oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               _oImportLC.ImportLC_Clauses = ImportLC_Clause.GetsByImportLCID(_oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLog(ImportLC oImportLC)
        {
            try
            {
                _oImportLC = oImportLC.SaveLog(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_UpdateStatus(ImportLC oImportLC)
        {
            oImportLC.RemoveNulls();
            try
            {
                _oImportLC = oImportLC;
                _oImportLC.LCCurrentStatus = (EnumLCCurrentStatus)_oImportLC.LCCurrentStatusInt;
                _oImportLC = _oImportLC.Save_UpdateStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private bool ValidateInput_SaveAmendment(ImportLC oImportLC)
        {

            if (oImportLC.ImportLCID <= 0)
            {
                _sErrorMessage = "Invalid  Operation.";
                return false;
            }
            if (oImportLC.ImportLCLogID <= 0)
            {
                _sErrorMessage = "Please enter, Amendmen request.";
                return false;
            }
           

            return true;
        }
     
    
        #endregion

        #region HTTP Delete

        [HttpPost]
        public JsonResult Delete(ImportLC oImportLC)
        {
            string sErrorMease = "";
            try
            {
                
                    sErrorMease = oImportLC.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
               
                
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePPC(ImportLCDetail oImportLCDetail)
        {
            string sErrorMease = "";
            try
            {
                if (oImportLCDetail.ImportLCDetailID > 0)
                {
                    sErrorMease = oImportLCDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    sErrorMease = "Invalid operation";
                }

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Update

        [HttpPost]
        public JsonResult UpdateForLCOpen(ImportLC oImportLC)
        {

            try
            {
                oImportLC.LCCurrentStatus = EnumLCCurrentStatus.LC_Open;
                _oImportLC = oImportLC.UpdateForLCOpen(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_ShipmentInTransit(ImportLC oImportLC)
        {

            try
            {
                oImportLC.LCCurrentStatus = EnumLCCurrentStatus.Shipment_InTransit;
                _oImportLC = oImportLC.UpdateForLCOpen(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_InSupplierNo(ImportLC oImportLC)
        {
            try
            {
                oImportLC.LCCurrentStatus = EnumLCCurrentStatus.InSupplierHand;
                _oImportLC = oImportLC.UpdateForLCOpen(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateImportLC_FileNo(ImportLC oImportLC)
        {
            try
            {
                oImportLC.LCCurrentStatus = EnumLCCurrentStatus.Shipment_InTransit;
                _oImportLC = oImportLC.UpdateImportLC_FileNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RequestConfirm(ImportLC oImportLC)
        {
            try
            {
                //oImportLC.LCCurrentStatus = EnumLCCurrentStatus.AmendmentConfirm;
                _oImportLC = oImportLC.RequestConfirm(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Purchase LC Clause
        public ActionResult ViewImportLCLetterIssue(int nImportLCID)
        {
            _oImportLC = new ImportLC();

            _oImportLC = _oImportLC.Get(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.ImportLC_Clauses = ImportLC_Clause.GetsByImportLCID(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return PartialView(_oImportLC);
        }

        [HttpPost]
        public JsonResult GetClauses(ImportLC oImportLC)
        {
            List<ImportLCClauseSetup> oImportLCClauseSetups = new List<ImportLCClauseSetup>();

            _oImportLC = _oImportLC.Get(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.ImportLCDetails = ImportLCDetail.Gets(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oImportLC.ImportLCDetails.Count > 0)
            {
                _oImportLC.ProductType = _oImportLC.ImportLCDetails[0].ProductType;
            }

            string sSQL="";
            string sReturn1 = "SELECT * FROM ImportLCClauseSetup";
            string sReturn = "";

            #region LCPaymentTypeInt
            if (_oImportLC.LCPaymentTypeInt > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCPaymentType IN (" + _oImportLC.LCPaymentTypeInt + ",0) ";
            }
            #endregion

            #region LCAppType

            if (_oImportLC.LCAppTypeInt > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCAppType IN (" + _oImportLC.LCAppTypeInt + ",0)";
            }
            #endregion
            #region productType
            if ((int)_oImportLC.ProductType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " productType IN (" + (int)_oImportLC.ProductType + ",0)";
            }
            #endregion
            #region BUID
            if (_oImportLC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID IN (" + _oImportLC.BUID + ",0)";
            }
            #endregion

            #region Activity

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Activity =1";
           
            #endregion

           sSQL = sReturn1 + sReturn;

           // oImportLCClauseSetups = ImportLCClauseSetup.GetsActiveImportLCClause(((User)Session[SessionInfo.CurrentUser]).UserID);
           oImportLCClauseSetups = ImportLCClauseSetup.GetsWithSQL(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
           

            foreach (ImportLCClauseSetup oItem in oImportLCClauseSetups)
            {
                if (_oImportLC.ImportLCDetails.Count > 0)
                {
                    if (oItem.Clause.Contains("@PITERM"))
                    {
                        oItem.Clause = oItem.Clause.Replace("@PITERM", _oImportLC.ImportLCDetails[0].LCTermsName.ToString());
                    }
                }
                if (oItem.Clause.Contains("@BENEF_LCTERM"))
                {
                    oItem.Clause = oItem.Clause.Replace("@BENEF_LCTERM", _oImportLC.LCTermsName_Bene);
                }
                if (oItem.Clause.Contains("@LCTERM"))
                {
                    oItem.Clause = oItem.Clause.Replace("@LCTERM", _oImportLC.LCTermsName.ToString());
                }
                if (oItem.Clause.Contains("@COVERNOTENO"))
                {
                    oItem.Clause = oItem.Clause.Replace("@COVERNOTENO", _oImportLC.LCCoverNoteNo + " DT: " + _oImportLC.CoverNoteDate.ToString("dd MMM yyyy"));
                }
                if (oItem.Clause.Contains("@INSURANCEINFO"))
                {
                    oItem.Clause = oItem.Clause.Replace("@INSURANCEINFO", _oImportLC.InsuranceCompanyName);
                }

                if (oItem.Clause.Contains("@SHIPMENTDATE"))
                {
                    oItem.Clause = oItem.Clause.Replace("@SHIPMENTDATE", _oImportLC.ShipmentDate.ToString("dd MMM yyyy"));
                }
                if (oItem.Clause.Contains("@EXPIREDATE"))
                {
                    oItem.Clause = oItem.Clause.Replace("@EXPIREDATE", _oImportLC.ExpireDate.ToString("dd MMM yyyy"));
                }
                if (oItem.Clause.Contains("@CURRENCY"))
                {
                    oItem.Clause = oItem.Clause.Replace("@CURRENCY", _oImportLC.CurrencyName);
                }
                if (oItem.Clause.Contains("@LCAFORMNO"))
                {
                    oItem.Clause = oItem.Clause.Replace("@LCAFORMNO", _oImportLC.LCANo);
                }
               
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCClauseSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClausesForAmendment(ImportLC oImportLC)
        {
            ImportLC oImportLCLog = new ImportLC();
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            List<ImportLC_Clause> oImportLC_Clauses = new List<ImportLC_Clause>();
          //  oImportLCClauseSetups = ImportLCClauseSetup.GetsActiveImportLCClause(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC = _oImportLC.Get(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportLC.ImportLCDetails = ImportLCDetail.Gets(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportLCLog = oImportLCLog.GetLog(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (Math.Round(oImportLCLog.Amount,2) != Math.Round(_oImportLC.Amount,2))
            {
                oImportLC_Clause = new ImportLC_Clause();
                oImportLC_Clause.Caption = "F32B:";
                oImportLC_Clause.Clause = "Please read the " + oImportLCLog.CurrencyName + " " + oImportLCLog.Amount + " in Instead of " + _oImportLC.CurrencyName + " " + _oImportLC.Amount;
                oImportLC_Clauses.Add(oImportLC_Clause);
            }
            if (!_oImportLC.IsPartShipmentAllow && _oImportLC.IsPartShipmentAllow != !oImportLCLog.IsPartShipmentAllow)
            {
                oImportLC_Clause = new ImportLC_Clause();
                oImportLC_Clause.Caption = "F43P:";
                oImportLC_Clause.Clause = "Please read the Not Allowed in instead of Allowed";
                oImportLC_Clauses.Add(oImportLC_Clause);
            }

            if (oImportLCLog.ShipmentDate.ToString("dd MMM yyyy") != _oImportLC.ShipmentDate.ToString("dd MMM yyyy"))
            {
                oImportLC_Clause = new ImportLC_Clause();
                oImportLC_Clause.Caption = "F44C:";
                oImportLC_Clause.Clause = "Pls insert new shipment date " + oImportLCLog.ShipmentDate.ToString("dd MMM yyyy")+" Instead of existing";
                oImportLC_Clauses.Add(oImportLC_Clause);
            }
            if (oImportLCLog.ExpireDate.ToString("dd MMM yyyy") != _oImportLC.ExpireDate.ToString("dd MMM yyyy"))
            {
                oImportLC_Clause = new ImportLC_Clause();
                oImportLC_Clause.Caption = "F31D:";
                oImportLC_Clause.Clause = "Pls insert new Expiry date " + oImportLCLog.ExpireDate.ToString("dd MMM yyyy") + " Instead of existing";
                oImportLC_Clauses.Add(oImportLC_Clause);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLC_Clauses);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult WaitForLCOpen(ImportLC oImportLC)
        {
            _oImportLCs = new List<ImportLC>();
            _oImportLCs = ImportLC.GetsByStatus(((int)EnumLCCurrentStatus.ReqForLC).ToString(), 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveClauses(ImportLC oImportLC)
        {
            List<ImportLC_Clause> oImportLC_Clauses = new System.Collections.Generic.List<ImportLC_Clause>();
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            try
            {
                if (oImportLC.ImportLCID > 0)
                {
                    _oImportLC = _oImportLC.Get(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oImportLC.ImportLC_Clauses=new System.Collections.Generic.List<ImportLC_Clause>();
                    _oImportLC.ImportLCClauseSetups = oImportLC.ImportLCClauseSetups;
                    if (oImportLC.ImportLCClauseSetups!=null)
                    {
                        foreach (ImportLCClauseSetup oItem in oImportLC.ImportLCClauseSetups)
                        {
                            oImportLC_Clause = new ImportLC_Clause();
                            oImportLC_Clause.Clause = oItem.Clause;
                            
                            oImportLC_Clause.ImportLCID = oImportLC.ImportLCID;
                            oImportLC_Clause.ImportLCLogID = oImportLC.ImportLCLogID;
                            oImportLC_Clause.LCCurrentStatusInt = oImportLC.LCCurrentStatusInt;
                            oImportLC_Clauses.Add(oImportLC_Clause);
                        }
                    }
                    _oImportLC.ImportLC_Clauses = ImportLC_Clause.SaveAll(oImportLC_Clauses, oImportLC, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                   
                }
                else
                {
                    _oImportLC.ErrorMessage = "Please Add Payment Contract First.";
                }
              
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveClausesAmendment(ImportLC oImportLC)
        {
            List<ImportLC_Clause> oImportLC_Clauses = new System.Collections.Generic.List<ImportLC_Clause>();
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            try
            {
                if (oImportLC.ImportLCID > 0)
                {
                    _oImportLC = _oImportLC.Get(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oImportLC.ImportLC_Clauses = new System.Collections.Generic.List<ImportLC_Clause>();
                    _oImportLC.ImportLC_Clauses = oImportLC.ImportLC_Clauses;
                    if (oImportLC.ImportLC_Clauses.Count != null)
                    {
                        foreach (ImportLC_Clause oItem in oImportLC.ImportLC_Clauses)
                        {
                            oImportLC_Clause = new ImportLC_Clause();
                            oImportLC_Clause.Clause = oItem.Clause;
                            oImportLC_Clause.Caption = oItem.Caption;
                            //oImportLC_Clause.Text = oItem.Clause;
                            oImportLC_Clause.ImportLCID = oImportLC.ImportLCID;
                            oImportLC_Clause.ImportLCLogID = oImportLC.ImportLCLogID;
                            oImportLC_Clause.LCCurrentStatusInt = oImportLC.LCCurrentStatusInt;
                            oImportLC_Clauses.Add(oImportLC_Clause);
                        }
                    }
                    _oImportLC.ImportLC_Clauses = ImportLC_Clause.SaveAll(oImportLC_Clauses, oImportLC,((User)Session[SessionInfo.CurrentUser]).UserID);


                }
                else
                {
                    _oImportLC.ErrorMessage = "Please Add Payment Contract First.";
                }

            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LetterPrint(int nImportLCID)
        {
            string sMasterLCNo = "";
            ImportLC oImportLC = new ImportLC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            Contractor oContractor = new Contractor();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();
            string sLCAppType = "";
            if (nImportLCID > 0)
            {
                _oImportLC = oImportLC.Get(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if(!String.IsNullOrEmpty(_oImportLC.BBankRefNo))
                {
                    _oImportLC.ImportLCNo = _oImportLC.ImportLCNo + "(Bangladesh Bank DC No: " + _oImportLC.BBankRefNo+")";
                }

                _oImportLC.BusinessUnit = oBusinessUnit.Get(_oImportLC.BUID,(int)Session[SessionInfo.currentUserID]);
                _oImportLC.ImportLC_Clauses = ImportLC_Clause.Gets(nImportLCID,0,(int)EnumLCCurrentStatus.ReqForLC, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPIs = ImportPI.GetsByLCID(nImportLCID, (int)Session[SessionInfo.currentUserID]);
                if (_oImportLC.LCAppType == EnumLCAppType.B2BLC)
                {
                    oImportMasterLCs = ImportMasterLC.Gets(nImportLCID, (int)Session[SessionInfo.currentUserID]);
                    sMasterLCNo = "";
                    foreach (ImportMasterLC oItem in oImportMasterLCs)
                    {
                        sMasterLCNo = sMasterLCNo + oItem.MasterLCNo + " DT:" + oItem.MasterLCDateSt + "\n";
                    }
              
                }
                oContractor = oContractor.Get(_oImportLC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oImportLC.LCAppTypeInt == (int)EnumLCAppType.LC )
                {
                    sLCAppType = ((int)EnumLCAppType.LC).ToString();
                }
                if (_oImportLC.LCAppTypeInt == (int)EnumLCAppType.B2BLC || _oImportLC.LCAppTypeInt == (int)EnumLCAppType.TT)
                {
                    sLCAppType = (_oImportLC.LCAppTypeInt).ToString();
                }

                string sSQL = "";
                if (_oImportLC.LCAppTypeInt > 0) sSQL = sSQL + " And LCAppType in (" + sLCAppType + ")";
                sSQL = sSQL + " And LCPaymentType in (0," + _oImportLC.LCPaymentTypeInt + ")";
                if (oImportPIs.Count > 0)
                {
                    if ((int)oImportPIs[0].ProductType > 0) sSQL = sSQL + " And ProductType in (0," + (int)oImportPIs[0].ProductType + ")";
                }
                sSQL = sSQL + " Order By LCPaymentType DESC";
                oImportLetterSetup = oImportLetterSetup.Get((int)EnumImportLetterType.LCOpeningRequest, (int)EnumImportLetterIssueTo.Bank, _oImportLC.BUID, nImportLCID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportLetterSetup.ContractorAddress = oContractor.Address;
                oImportLetterSetup.MasterLCs = sMasterLCNo;
            }
          
            //_oImportLC.PINos = sPINoWithDate+".";
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);
            rptImportLCLetterIssue oReport = new rptImportLCLetterIssue();
            byte[] abytes = oReport.PreparePrintLetter(_oImportLC, oImportLetterSetup, oImportPIs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult LetterPrint_Amendment(int nImportLCID)
        {
            ImportLC oImportLCLog = new ImportLC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            Contractor oContractor = new Contractor();
            EnumImportLetterType eEnumImportLetterType = EnumImportLetterType.LC_AmendmentRequest;

            if (nImportLCID > 0)
            {
                _oImportLC = _oImportLC.Get(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (!String.IsNullOrEmpty(_oImportLC.BBankRefNo))
                {
                    _oImportLC.ImportLCNo = _oImportLC.ImportLCNo + "(Bangladesh Bank DC No: " + _oImportLC.BBankRefNo + ")";
                }
                oImportLCLog = oImportLCLog.GetLog(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.BusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);
                _oImportLC.ImportLC_Clauses = ImportLC_Clause.Gets(nImportLCID, oImportLCLog.ImportLCLogID, _oImportLC.LCCurrentStatusInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportPIs = ImportPI.GetsByLCID(nImportLCID, (int)Session[SessionInfo.currentUserID]);
                _oImportLC.ImportLCDetails = ImportLCDetail.GetsLog(oImportLCLog.ImportLCLogID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportLC.Amount = oImportLCLog.Amount;
                _oImportLC.LCChargeType = oImportLCLog.LCChargeType;
                _oImportLC.LCChargeTypeInt = oImportLCLog.LCChargeTypeInt;

                oContractor = oContractor.Get(_oImportLC.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                string sSQL = "";
                if (_oImportLC.LCAppTypeInt > 0) sSQL = sSQL + " And LCAppType=" + _oImportLC.LCAppTypeInt + "";
                sSQL = sSQL + " And LCPaymentType in (0," + _oImportLC.LCPaymentTypeInt + ")";
                //if (oImportPIs.Count > 0)
                //{
                //    if (oImportPIs[0].ProductType > 0) sSQL = sSQL + " And ProductType in (0," + oImportPIs[0].ProductType + ")";
                //}
                sSQL = sSQL + " Order By LCPaymentType DESC";

                if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_ForAmendment)
                {
                    _oImportLC.LCRequestDate = oImportLCLog.LCRequestDate;
                    eEnumImportLetterType = EnumImportLetterType.LC_AmendmentRequest;
                }
                else if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Cancel)
                {
                    _oImportLC.LCRequestDate = oImportLCLog.LCRequestDate;
                    eEnumImportLetterType = EnumImportLetterType.LC_Cancelation;
                }
                else if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Partial_Cancel)
                {
                    _oImportLC.LCRequestDate = oImportLCLog.LCRequestDate;
                    eEnumImportLetterType = EnumImportLetterType.LC_Partial_Cancelation;
                }
                oImportLetterSetup = oImportLetterSetup.Get((int)eEnumImportLetterType, (int)EnumImportLetterIssueTo.Bank, _oImportLC.BUID, nImportLCID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oImportLetterSetup.ContractorAddress = oContractor.Address;
            }
            
            //_oImportLC.PINos = sPINoWithDate+".";
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);
            rptImportLCLetterIssue oReport = new rptImportLCLetterIssue();
            byte[] abytes = oReport.PreparePrintLetter(_oImportLC, oImportLetterSetup, oImportPIs, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_LCDetail(int nImportLCID)
        {
            string sSQL = "";
            ImportLC oImportLCLog = new ImportLC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<ImportInvoice> oImportInvoices = new System.Collections.Generic.List<ImportInvoice>();
            List<ImportInvoiceDetail> oImportInvoiceDetails = new System.Collections.Generic.List<ImportInvoiceDetail>();
            List<ImportPIDetail> oImportPIDetails = new System.Collections.Generic.List<ImportPIDetail>();
            List<ImportLC> oImportLCLogs = new System.Collections.Generic.List<ImportLC>();

            if (nImportLCID > 0)
            {
                _oImportLC = _oImportLC.Get(nImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (!String.IsNullOrEmpty(_oImportLC.BBankRefNo))
                {
                    _oImportLC.ImportLCNo = _oImportLC.ImportLCNo + "(Bangladesh Bank DC No: " + _oImportLC.BBankRefNo + ")";
                }
                oImportLCLogs = ImportLC.Gets("SELECT * from [View_ImportLCLog]  WHERE ImportLCID=" + _oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportLCLogs.Add(_oImportLC);


                oBusinessUnit = oBusinessUnit.Get(_oImportLC.BUID, (int)Session[SessionInfo.currentUserID]);
                _oImportLC.ImportLCDetails = ImportLCDetail.Gets(_oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oImportInvoices = ImportInvoice.Gets(_oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ImportPIDetail where ImportPIID in (Select ImportPIID from ImportLCDetail where ImportLCID=" + _oImportLC.ImportLCID + "and Activity=1 )";
                oImportPIDetails = ImportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "Select * from View_ImportInvoiceDetail where ImportInvoiceID  in (Select ImportInvoiceID from ImportInvoice where ImportLCID>0 and ImportLCID in (Select ImportLCID from ImportLC where ImportLCID=" + _oImportLC.ImportLCID + "))";
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            //_oImportLC.PINos = sPINoWithDate+".";
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            
            rptImportLC oReport = new rptImportLC();
            byte[] abytes = oReport.PrepareReport(_oImportLC, oImportPIDetails, oImportInvoices, oImportInvoiceDetails, oCompany, oBusinessUnit, oImportLCLogs);
            return File(abytes, "application/pdf");
        }
        #endregion


        #region Get Company Logo
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Load ImportLCDetailProducts in Purchase Invoice Product
        [HttpPost]
        public JsonResult RefreshPurchaseInvoiceProduct(ImportLC oImportLC)
        {

            try
            {
                oImportLC = oImportLC.Get(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

               if (oImportLC.ImportLCNo == "")
               {
                   _oImportLC.ErrorMessage = "Please open LC first.";
               }
               else
               {
                   _oImportLC.ImportLCDetailProducts = ImportLCDetailProduct.GetsByLCID(oImportLC.ImportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               }
            }
            catch (Exception ex)
            {
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsImportPIs(ImportPI oImportPI)
        {
            List<ImportLCDetail> oImportLCDetails = new List<ImportLCDetail>();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            ImportLCDetail oImportLCDetail = new ImportLCDetail();
            string sSQL = "Select * from View_ImportPI WHERE BUID = " + oImportPI.BUID + " AND [ImportPIStatus] IN (" + (int)EnumImportPIState.Accepted + ") AND ImportPIID NOT IN (SELECT ImportPIID FROM ImportLCDetail WHERE Activity=1 and ImportLCID != " + oImportPI.ImportLCID + ")";
             string sReturn = " ";
             if (oImportPI.SupplierID>0)
             {
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "SupplierID =" + oImportPI.SupplierID;
             }
             if (!String.IsNullOrEmpty(oImportPI.ImportPINo))
             {
                 oImportPI.ImportPINo = oImportPI.ImportPINo.Trim();
                 Global.TagSQL(ref sReturn);
                 sReturn = sReturn + "ImportPINo like'%" + oImportPI.ImportPINo + "%'";
             }
             //Global.TagSQL(ref sReturn, true);
             //sReturn = sReturn + " or ImportPIID IN (SELECT ImpLD.ImportPIID FROM View_ImportLCDetail as ImpLD WHERE Activity=1  AND ImpLD.ImportPIType="+(int)EnumImportPIType.LC+")";
             sSQL = sSQL + sReturn;
             oImportPIs = ImportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach(ImportPI oItem in oImportPIs)
            {
                oImportLCDetail = new ImportLCDetail();
                oImportLCDetail.ImportPINo = oItem.ImportPINo;
                oImportLCDetail.ImportPIID = oItem.ImportPIID;
                oImportLCDetail.SupplierID = oItem.SupplierID;
                oImportLCDetail.SupplierName = oItem.SupplierName;
                oImportLCDetail.LCTermsName = oItem.LCTermsName;
                oImportLCDetail.Amount = oItem.TotalValue - oItem.DiscountAmount;
                oImportLCDetail.Currency = oItem.CurrencySymbol;
                oImportLCDetail.CurrencyID = oItem.CurrencyID;
                oImportLCDetail.LCTermID = oItem.LCTermID;
                oImportLCDetail.DateOfApproved = oItem.DateOfApproved;
                oImportLCDetail.PaymentInstructionInt = oItem.PaymentInstructionTypeInt;
                oImportLCDetail.PaymentInstruction = (EnumPaymentInstruction)oItem.PaymentInstructionTypeInt;
                oImportLCDetail.ShipmentBy = oItem.ShipmentBy;
                oImportLCDetail.ProductType = oItem.ProductType;
                oImportLCDetail.ProductTypeName = oItem.ProductTypeName;
                oImportLCDetail.ImportPIType = oItem.ImportPIType;
                if (oItem.ImportPIType == EnumImportPIType.Foreign)
                {
                    oImportLCDetail.LCAppTypeInt = (int)EnumLCAppType.LC;
                }
                if (oItem.ImportPIType == EnumImportPIType.Foreign)
                {
                    oImportLCDetail.LCAppTypeInt = (int)EnumLCAppType.LC;
                }
               
                if (oItem.ImportPIType == EnumImportPIType.TTForeign)
                {
                    oImportLCDetail.LCAppTypeInt = (int)EnumLCAppType.TT;
                }
                oImportLCDetail.BankName = oItem.BankName;
                oImportLCDetail.IsTransShipmentAllow = oItem.IsTransShipmentAllow;
                oImportLCDetail.IsPartShipmentAllow = oItem.IsPartShipmentAllow;
                oImportLCDetail.AmendmentDate = DateTime.MinValue;
                oImportLCDetails.Add(oImportLCDetail);

            }
         
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        #region Master LC
        public JsonResult GetsImportMasterLCs(ImportMasterLC oImportMasterLC)
        {
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();
            List<MasterLC> oMasterLCs = new List<MasterLC>();
            ImportMasterLC oIMLC = new ImportMasterLC();
            string sSQL = "Select * from View_MasterLC WHERE  MasterLCType=" + (int)EnumMasterLCType.MasterLC;
            string sReturn = " ";

            if (!String.IsNullOrEmpty(oImportMasterLC.MasterLCNo))
            {
                oImportMasterLC.MasterLCNo = oImportMasterLC.MasterLCNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MasterLCNo like'%" + oImportMasterLC.MasterLCNo + "%'";
            }
            sSQL = sSQL + sReturn;
            oMasterLCs = MasterLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (MasterLC oItem in oMasterLCs)
            {
                oIMLC = new ImportMasterLC();
                oIMLC.MasterLCNo = oItem.MasterLCNo;
                oIMLC.MasterLCID = oItem.MasterLCID;
                oIMLC.ImportLCID = oImportMasterLC.ImportLCID;
                oIMLC.MasterLCDate = oItem.MasterLCDate;
                oImportMasterLCs.Add(oIMLC);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportMasterLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_MasterLC(ImportMasterLC oIMLC)
        {
            ImportMasterLC oImportMasterLC = new ImportMasterLC();
            try
            {
                oImportMasterLC = oIMLC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportMasterLC = new ImportMasterLC();
                oImportMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportMasterLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveWithMasterLC(ImportMasterLC oIMLC)
        {
            ImportMasterLC oImportMasterLC = new ImportMasterLC();
            try
            {
                oImportMasterLC = oIMLC.SaveWithMasterLC(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportMasterLC = new ImportMasterLC();
                oImportMasterLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportMasterLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_IMasterLC(ImportMasterLC oImportMasterLC)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oImportMasterLC.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    

        #endregion
        #endregion

        #region Text Search
        [HttpPost]
        public JsonResult GetsBySearchKey(ImportLC oImportLC)
        {
            _oImportLCs = new List<ImportLC>();
            try
            {
                if (oImportLC.ImportLCNo == "") oImportLC.ImportLCNo = null;
                string sSQL = "SELECT * FROM View_ImportLC WHERE BUID = " + oImportLC.BUID + " AND ImportLCNo Like '%" + oImportLC.ImportLCNo + "%'";
                _oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLCs = new List<ImportLC>();
                _oImportLC = new ImportLC();
                _oImportLC.ErrorMessage = ex.Message;
                _oImportLCs.Add(_oImportLC);
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion

        #region Searching
        [HttpGet]
        public JsonResult GetbyLCNo(string ImportLCNo, double ts)
        {
            ImportLCNo = ImportLCNo.Trim();
            _oImportLCs = new List<ImportLC>();
            string sSQL = "SELECT * FROM [View_ImportLC] where ImportLCNO like '%" + ImportLCNo + "%'";
            _oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewAdvanceSearch()
        {
            _oImportLC  = new  ImportLC();
            _oImportLC.BankBranchs = BankBranch.GetsOwnBranchs( ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oImportLC);
        }
        [HttpPost]
        public JsonResult GetsSearchedData(ImportLC ImportLC)
        {
            _oImportLCs = new List<ImportLC>();
            try
            {
                string sSQL = GetSQL(ImportLC);
                _oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oImportLCs = new List<ImportLC>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ImportLC oImportLC)
        {

            int nImportLCDate = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[2]);
            int nReceiveDate = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[3]);
            DateTime dReceiveDateStartDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[4]);
            DateTime dReceiveDateEndDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[5]);
            int nShipmentDate = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[6]);
            DateTime dShipmentStartDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[7]);
            DateTime dShipmentEndDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[8]);
            int nLCRequestDate = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[9]);
            DateTime dLCRequestStartDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[10]);
            DateTime dLCRequestEndDate = Convert.ToDateTime(oImportLC.ErrorMessage.Split('~')[11]);

            int nComboAmount = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[12]);
            double nAmountStart = Convert.ToDouble(oImportLC.ErrorMessage.Split('~')[13]);
            double nAmountEnd = Convert.ToDouble(oImportLC.ErrorMessage.Split('~')[14]);

            string sImportLCNo = oImportLC.ErrorMessage.Split('~')[15];
            string sContractorIDs = oImportLC.ErrorMessage.Split('~')[16];
            string sInsuranceCompanyIDs = oImportLC.ErrorMessage.Split('~')[17];
            string sBankBranch_NegoIDs = oImportLC.ErrorMessage.Split('~')[18];
            string sLCCurrentStatuss = oImportLC.ErrorMessage.Split('~')[19];
            int nBUID = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[20]);
            string sImportPINo = oImportLC.ErrorMessage.Split('~')[21];
            int nImportPIType = Convert.ToInt32(oImportLC.ErrorMessage.Split('~')[22]);
            string sReturn1 = "SELECT * FROM View_ImportLC ";

            #region ImportLCNo
            if (sImportLCNo != null && sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCNo LIKE " + "'%" + sImportLCNo + "%'";
            }
            #endregion

            #region Supplier id
            if (sContractorIDs != null && sContractorIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN(" + sContractorIDs + ")";
            }
            #endregion

            #region Insurance id
            if (sInsuranceCompanyIDs != null && sInsuranceCompanyIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InsuranceCompanyID IN(" + sInsuranceCompanyIDs + ")";
            }
            #endregion

            #region Negotiation Branch
            if (sBankBranch_NegoIDs !="")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankBranchID_Nego IN (" + sBankBranch_NegoIDs+")";
            }
            #endregion

            #region status
            if (sLCCurrentStatuss != null && sLCCurrentStatuss != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCCurrentStatus IN(" + sLCCurrentStatuss + ")";
            }
            #endregion

            #region Import lc Date Wise
            if (nImportLCDate > 0)
            {
                if (nImportLCDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ImportLCDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND ImportLCDate < '" + dStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nImportLCDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportLCDate != '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nImportLCDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportLCDate > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nImportLCDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportLCDate < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nImportLCDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ImportLCDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND ImportLCDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nImportLCDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ImportLCDate< '" + dStartDate.ToString("dd MMM yyyy") + "' OR ImportLCDate > '" + dEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Receive Date Wise
            if (nReceiveDate > 0)
            {
                if (nReceiveDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ReceiveDate>= '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "' AND ReceiveDate < '" + dReceiveDateStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nReceiveDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceiveDate != '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiveDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceiveDate > '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiveDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ReceiveDate < '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nReceiveDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ReceiveDate>= '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "' AND ReceiveDate < '" + dReceiveDateEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nReceiveDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ReceiveDate< '" + dReceiveDateStartDate.ToString("dd MMM yyyy") + "' OR ReceiveDate > '" + dReceiveDateEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Shipment Date Wise
            if (nShipmentDate > 0)
            {
                if (nShipmentDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nShipmentDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nShipmentDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Request Date Wise
            if (nLCRequestDate > 0)
            {
                if (nLCRequestDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (LCRequestDate>= '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "' AND LCRequestDate < '" + dLCRequestStartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nLCRequestDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCRequestDate != '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nLCRequestDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCRequestDate > '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nLCRequestDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCRequestDate < '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nLCRequestDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (LCRequestDate>= '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "' AND LCRequestDate < '" + dLCRequestEndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nLCRequestDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (LCRequestDate< '" + dLCRequestStartDate.ToString("dd MMM yyyy") + "' OR LCRequestDate > '" + dLCRequestEndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion

            #region Amount  Wise
            if (nComboAmount > 0)
            {
                if (nComboAmount == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nAmountStart;
                }
                if (nComboAmount == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nAmountStart;
                }
                if (nComboAmount == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nAmountStart;
                }
                if (nComboAmount == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nAmountStart;
                }
                if (nComboAmount == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (Amount>= " + nAmountStart + " AND Amount < " + nAmountEnd + ")";
                }
                if (nComboAmount == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (Amount <" + nAmountStart + " OR Amount > " + nAmountEnd + ")";
                }
            }
            #endregion

            #region ImportLCNo
            if (sImportPINo != null && sImportPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCID IN (SELECT ImportLCID FROM View_ImportLCDetail WHERE ImportPINo Like '%"+sImportPINo+"%' )";
            }
            #endregion

            #region Bu
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region Import PI Type
            if (nImportPIType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPIType =" + nImportPIType + " ";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " Order By ImportLCDate, ImportLCNo ASC";
            return sReturn;
        }

        #endregion 

        #region Request For LC
        public ActionResult ViewRequestForLC(int id, int buid)
        {
            ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
            oImportLCReqByExBill = oImportLCReqByExBill.GetByLC(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportLCReqByExBill.ImportLCReqByExBillDetails = ImportLCReqByExBillDetail.Gets(oImportLCReqByExBill.ImportLCReqByExBillID, ((User)Session[SessionInfo.CurrentUser]).UserID); 
            oImportLCReqByExBill.ImportLC = _oImportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankAccounts = BankAccount.GetsByBankBranch(oImportLCReqByExBill.ImportLC.BankBranchID_Nego, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.buid = buid;
            return View(oImportLCReqByExBill);
        }

        [HttpPost]
        public JsonResult SaveImportLCReqByExBill(ImportLCReqByExBill oImportLCReqByExBill)
        {
            ImportLCReqByExBill _oImportLCReqByExBill = new ImportLCReqByExBill();
            try
            {
                _oImportLCReqByExBill = oImportLCReqByExBill.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportLCReqByExBill = new ImportLCReqByExBill();
                _oImportLCReqByExBill.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCReqByExBill);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Excel
        public void PrintListInXL(string sParam, int nBUID)
        {
            ImportLC oILC = new ImportLC();
            oILC.ErrorMessage = sParam;
            string sSQL = GetSQL(oILC);
            _oImportLCs = ImportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oImportLCs.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Application Type", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "File No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Cover Note No", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Payment Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Supplier Name", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Negotiable Bank", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Current Status", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Currency", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total LC Value", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Invoice Value", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 18f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Import LC List");

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "Import LC List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion


                    #region Data
                    int nSL = 0;
                    foreach (ImportLC oItem in _oImportLCs)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LCAppTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FileNo, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ImportLCNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ImportLCDateInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LCCoverNoteNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LCPaymentTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BankName_Nego, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LCCurrentStatusInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.CurrencyName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Amount.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Amount_Invoice.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Balance.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 10, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportLCs.Sum(c => c.Amount).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportLCs.Sum(c => c.Amount_Invoice).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportLCs.Sum(c => c.Balance).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Import_LC_List.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

    }
}

