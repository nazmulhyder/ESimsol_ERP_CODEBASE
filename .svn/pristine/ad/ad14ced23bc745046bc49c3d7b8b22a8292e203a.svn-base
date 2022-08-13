using System;
using System.Collections.Generic;
using System.Data;
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
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportLCController : Controller
    {
        #region By Mamun 03 May 2015
        string _sErrorMessage = "";
        ExportLC _oExportLC = new ExportLC();
        List<ExportLC> _oExportLCs = new List<ExportLC>();
        int nLCType = 0;

        public ActionResult ViewExportLCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportLCs = new List<ExportLC>();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            ExportLC oExportLC = new ExportLC();
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
            _oExportLCs = ExportLC.Gets(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(),buid,"", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Companys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.NotifyBy = EnumObject.jGets(typeof(EnumNotifyBy));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.MenuID = menuid.ToString();
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
          
            return View(_oExportLCs);
        }
    
        public ActionResult ViewExportLC(int id, int buid)
        {
            _oExportLC = new ExportLC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<BankBranch> oBankBranchs_Forwarding= new List<BankBranch>();
            BankBranch oBankBranch_Forwarding= new BankBranch();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (id > 0)
            {
                _oExportLC = _oExportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oBankBranch_Forwarding = BankBranch.Get(_oExportLC.BankBranchID_Forwarding, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBankBranchs_Forwarding.Add(oBankBranch_Forwarding);
                
            }
            else
            {
                _oExportLC.ExportLCType = EnumExportLCType.LC;
            }
            ViewBag.BankBranchs_Forwarding=oBankBranchs_Forwarding;
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Companys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.TextileUnits = BusinessUnitTypeObj.Gets();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.NotifyBy = EnumObject.jGets(typeof(EnumNotifyBy));
            ViewBag.ExportLCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            // EnumBusinessUnitType
            return View(_oExportLC);
        }

        [HttpPost]
        public JsonResult UpdateExportPILCMapping(ExportPILCMapping oExportPILCMapping)
        {
            ExportPILCMapping _oExportPILCMapping = new ExportPILCMapping();
            try
            {
                _oExportPILCMapping = oExportPILCMapping;
                _oExportPILCMapping = _oExportPILCMapping.UpdateExportPILCMapping(_oExportPILCMapping,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportPILCMapping = new ExportPILCMapping();
                _oExportPILCMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPILCMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewExportLC_Amendment(int id, int buid)
        {
            _oExportLC = new ExportLC();
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
            if (id > 0)
            {
                _oExportLC = _oExportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.Gets(_oExportLC.ExportLCID, _oExportLC.VersionNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oExportLC.ExportBills = new List<ExportBill>();
                //_oExportLC.ExportBills = ExportBill.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.Companys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.TextileUnits = BusinessUnitTypeObj.Gets();
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            // EnumBusinessUnitType
            return View(_oExportLC);
        }
        public ActionResult ViewExportLC_ARequest(int id, int buid)
        {
            _oExportLC = new ExportLC();
            List<ExportLCAmendmentRequest> oExportLCAmendmentRequests = new List<ExportLCAmendmentRequest>();
            if (id > 0)
            {
                _oExportLC = _oExportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportLCAmendmentRequests = ExportLCAmendmentRequest.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          
            ViewBag.BUID = buid;
            ViewBag.ExportLCAmendmentRequests = oExportLCAmendmentRequests;
            return View(_oExportLC);
        }
        public ActionResult ViewExportLC_Bills(int id, int buid)
        {
            _oExportLC = new ExportLC();
            List<ExportBill> oExportBills = new List<ExportBill>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            int nDay_Shipment=0;
            int nDay_Expiry=0;
           
            if (id > 0)
            {
                _oExportLC = _oExportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                buid = _oExportLC.BUID;
                oExportBills = ExportBill.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            nDay_Shipment = Global.DateDiff("D", DateTime.Today,_oExportLC.ShipmentDate);
            nDay_Expiry = Global.DateDiff("D", DateTime.Today,_oExportLC.ExpiryDate);

            oMeasurementUnitCon = oMeasurementUnitCon.GetByBU(_oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.MUnitCon = oMeasurementUnitCon;
            ViewBag.Day_Shipment = nDay_Shipment;
            ViewBag.Day_Expiry = nDay_Expiry;
            ViewBag.ExportBills = oExportBills;
            return View(_oExportLC);
        }
        public ActionResult ViewExportLCLogs(int id, int buid)
        {
           
            _oExportLCs = new List<ExportLC>();

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
            if (id > 0)
            {
                _oExportLCs = ExportLC.GetsLog(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
          
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;

            return View(_oExportLCs);
        }
        public ActionResult ViewExportLCLog(int id, int buid)
        {
            _oExportLC = new ExportLC();
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
            if (id > 0)
            {
                _oExportLC = _oExportLC.GetLog(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.Gets(_oExportLC.ExportLCID, _oExportLC.VersionNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oExportLC);
        }
        public ActionResult ViewExportLCUD(int id, int buid)
        {
            _oExportLC = new ExportLC();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<ExportPILCMapping> oExportLCVersions = new List<ExportPILCMapping>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (id > 0)
            {
                _oExportLC = _oExportLC.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID,  ((User)Session[SessionInfo.CurrentUser]).UserID);
              

            }
            oExportLCVersions = _oExportLC.ExportPILCMappings.GroupBy(x => x.VersionNo).Select(x => x).FirstOrDefault().ToList();

            ViewBag.ExportLCVersions = oExportLCVersions;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oExportLC);
        }
        [HttpPost]
        public JsonResult GetsExportPI(ExportPI oExportPI) 
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            List<ExportPILCMapping> oPILCMappings = new List<ExportPILCMapping>();
            ExportPILCMapping oExportPILCMapping = new ExportPILCMapping();
           
            try
            {
                
                string sSQL = "";
                string sSQL1 = "SELECT * FROM View_ExportPI WHERE PaymentType in (0,1) and PIStatus in (" + (int)(EnumPIStatus.PIIssue) + "," + (int)(EnumPIStatus.BindWithLC) + "," + (int)(EnumPIStatus.RequestForRevise) + ") AND PINo LIKE '%" + oExportPI.PINo + "%' AND ISNULL(LCID,0)<=0  AND ExportPIID NOT IN (SELECT ExportPIID FROM MasterPIMapping )";
                if (oExportPI.BUID > 0)
                {
                    sSQL1 += " And BUID = " + oExportPI.BUID + " ";
                }
                
                if (oExportPI.ContractorID > 0)
                {
                    sSQL1 += " And ContractorID = " + oExportPI.ContractorID + " ";
                }
                string sSQL2 = " OR PaymentType in (0,1) and ExportPIID  IN (SELECT ExportPILCMapping.ExportPIID FROM ExportPILCMapping WHERE Activity=1 AND Flag=1 AND ExportPILCMapping.ExportLCID=" + oExportPI.LCID + ")";
                sSQL = sSQL1 + sSQL2;
                oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (ExportPI oItem in oExportPIs)
                {
                    oExportPILCMapping = new ExportPILCMapping();
                    oExportPILCMapping.ExportPIID = oItem.ExportPIID;
                    //oExportPILCMapping.PICode = oItem.PICode;
                    oExportPILCMapping.PINo = oItem.PINo;
                    oExportPILCMapping.Currency = oItem.Currency;
                    oExportPILCMapping.Amount = oItem.Amount;
                    // oExportPILCMapping.ReviseNo = Convert.ToString(oItem.ReviseNo);
                    oExportPILCMapping.ContractorID = oItem.ContractorID;
                    oExportPILCMapping.IssueDate = oItem.IssueDate;
                    oExportPILCMapping.LCTermID = oItem.LCTermID;
                    oExportPILCMapping.BankBranchID = oItem.BankBranchID;
                    oExportPILCMapping.LCTermsName = oItem.LCTermsName;
                    oExportPILCMapping.BuyerName = oItem.BuyerName;
                    oExportPILCMapping.PIStatus = oItem.PIStatus;
                    oExportPILCMapping.ContractorName = oItem.ContractorName;
                    oExportPILCMapping.BankName = oItem.BankName + '[' + oItem.BranchName+']';
                    oExportPILCMapping.Qty = oItem.Qty;
                    oPILCMappings.Add(oExportPILCMapping);
                }
            }
            catch (Exception ex)
            {
                oExportPILCMapping = new ExportPILCMapping();
                oExportPILCMapping.ErrorMessage = ex.Message;
                oPILCMappings.Add(oExportPILCMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPILCMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsExportPILCMapping(ExportLC oExportLC)
        {
            ExportPILCMapping oExportPILCMapping = new ExportPILCMapping();
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();

            try
            {
                oExportPILCMappings = ExportPILCMapping.Gets(oExportLC.ExportLCID, oExportLC.VersionNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPILCMapping.ErrorMessage = ex.Message;
                oExportPILCMappings.Add(oExportPILCMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPILCMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetbyLCNo(ExportLC oExportLC)
        {
            int ncboAmendmentDate = 0;
            if (oExportLC.IsRecDateSearch)
            {
                ncboAmendmentDate = (int)EnumCompareOperator.EqualTo;
            }
        
            _oExportLCs = new List<ExportLC>();
            string sReturn1 = "SELECT * FROM View_ExportLC ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "'";
            }
            #endregion

            #region BUID
            if (oExportLC.BUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID=" + oExportLC.BUID;
            }
            #endregion

            #region Amendment Date
            if (ncboAmendmentDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (ncboAmendmentDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and  CONVERT(DATE,CONVERT(VARCHAR(12),[LCReceiveDate] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oExportLC.LCRecivedDate.ToString("dd MMM yyyy") + "',106)) )";
                }
            }
            #endregion

            string sSQL = sReturn1 + sReturn;
            _oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Amendment Request
    
    
        [HttpPost]
        public JsonResult GetsExportLCAmendmentClauses(ExportLCAmendmentRequest oExportLCAmendmentRequest)
        {
            ExportLCAmendmentClause oExportLCAmendmentClause = new ExportLCAmendmentClause();

            List<ExportLCAmendmentClause> oExportLCAmendmentClauses = new List<ExportLCAmendmentClause>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
        
            try
            {
                if (oExportLCAmendmentRequest.ExportLCAmendmentRequestID <= 0)
                {
                    _oExportLC = _oExportLC.Get(oExportLCAmendmentRequest.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oExportPIs = ExportPI.Gets("SELECT * from View_ExportPI  WHERE ExportPIID  in (Select ExportPIID from ExportPILCMapping where ExportPILCMapping.Activity=1 and ExportLCID=" + _oExportLC.ExportLCID + " and VersionNo=" + _oExportLC.VersionNo + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    string sTemp = "";
                    int _nExpiryDayCount = 0;
                    int _nShipmentDayCount = 0;
                
                    /// Make Dynamic Caluse Compare PI T&C and LC T&C
                    foreach (ExportPI oTempPI in oExportPIs)
                    {

                        if (_oExportLC.LiborRate != oTempPI.IsLIBORRate)
                        {

                            sTemp = "Usance Period Interest should be paid by the applicant @ LIBOR rate.";
                            oExportLCAmendmentClause = new ExportLCAmendmentClause();
                            oExportLCAmendmentClause.Clause = sTemp;
                            oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        }
                        if (_oExportLC.LCTramsID != oTempPI.LCTermID)
                        {

                            sTemp = "Payment terms should be " + oTempPI.LCTermsName + " days instead of " + _oExportLC.LCTermsName + " .";
                            oExportLCAmendmentClause = new ExportLCAmendmentClause();
                            oExportLCAmendmentClause.Clause = sTemp;
                            oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        }
                        if (_oExportLC.OverDueRate != oTempPI.OverdueRate)
                        {
                            sTemp = "Overdue Period Interest should be paid @ " + oTempPI.OverdueRate + "%";

                            oExportLCAmendmentClause = new ExportLCAmendmentClause();
                            oExportLCAmendmentClause.Clause = sTemp;
                            oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        }
                        if (_oExportLC.BBankFDD != oTempPI.IsBBankFDD)
                        {

                            sTemp = "Payment must be US Dollar in Bangladesh Bank FDD.";
                            oExportLCAmendmentClause = new ExportLCAmendmentClause();
                            oExportLCAmendmentClause.Clause = sTemp;
                            oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        }
                        if (_oExportLC.DCharge > 25)
                        {

                            sTemp = "Pls, Reduce the Discrepences charges. We Rcvd The L/C's which The Discrepences Charges 20-25 USD.";

                            oExportLCAmendmentClause = new ExportLCAmendmentClause();
                            oExportLCAmendmentClause.Clause = sTemp;
                            oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        }
                        _nExpiryDayCount = Global.DateDiff("d", _oExportLC.OpeningDate, _oExportLC.ExpiryDate);
                        _nShipmentDayCount = Global.DateDiff("d", _oExportLC.OpeningDate, _oExportLC.ShipmentDate);

                        //if (_nExpiryDayCount < 30 || _nShipmentDayCount < 15)
                        //{
                        //    sTemp = "Shipment date & expired date extened up to " + _oExportLC.ShipmentDate.ToString("dd MMM yyyy") + " to " + _oExportLC.ExpiryDate.ToString("dd MMM yyyy") + ".";
                        //    oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        //    oExportLCAmendmentClause.Clause = sTemp;
                        //    oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                        //}
                        sTemp = " PI is 90 days but LC is " + _oExportLC.LCTermsName + " Please make amendment..";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);


                        sTemp = "Shipment and Expiry date has already expired Please make amendment.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need U D Urgently.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need Acceptance Urgently.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need amendment for Over Due interest Clause include.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need amendment for LIBOR interest Clause include.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need Maturity Urgently.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Need Payment Urgently.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);

                        sTemp = "Make amendment for U D.";
                        oExportLCAmendmentClause = new ExportLCAmendmentClause();
                        oExportLCAmendmentClause.Clause = sTemp;
                        oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
                    }
                   
                }
                else
                {
                    oExportLCAmendmentRequest = oExportLCAmendmentRequest.Get(oExportLCAmendmentRequest.ExportLCAmendmentRequestID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oExportLCAmendmentClauses = ExportLCAmendmentClause.Gets(oExportLCAmendmentRequest.ExportLCAmendmentRequestID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oExportLCAmendmentClause = new ExportLCAmendmentClause();
                oExportLCAmendmentClause.Clause = ex.Message;
                oExportLCAmendmentClauses.Add(oExportLCAmendmentClause);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportLCAmendmentClauses);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        private bool ValidateInput_AmendmentRequest(ExportLCAmendmentRequest oExportLCAmendmentRequest)
        {
            if (oExportLCAmendmentRequest.ExportLCID <= 0)
            {
                _sErrorMessage = "Invalid LC reference.";
                return false;
            }
            if (oExportLCAmendmentRequest.ExportLCAmendmentClauses.Count <= 0)
            {
                _sErrorMessage = "Please, Checked or Add Clause from List.";
                return false;
            }
            return true;
        }
        [HttpPost]
        public JsonResult Save_AmendmentRequest(ExportLCAmendmentRequest oExportLCAmendmentRequest)
        {
            oExportLCAmendmentRequest.RemoveNulls();
            try
            {
                if (this.ValidateInput_AmendmentRequest(oExportLCAmendmentRequest))
                {
                    oExportLCAmendmentRequest = oExportLCAmendmentRequest.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oExportLCAmendmentRequest.ExportLC = _oExportLC.Get(oExportLCAmendmentRequest.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oExportLCAmendmentRequest.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                oExportLCAmendmentRequest = new ExportLCAmendmentRequest();
                oExportLCAmendmentRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportLCAmendmentRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_AmendmentRequest(ExportLCAmendmentRequest oExportLCAmendmentRequest)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oExportLCAmendmentRequest.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);


            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintAmendmentRequest(int id)
        {
            ExportLCAmendmentRequest oExportLCAmendmentRequest = new ExportLCAmendmentRequest();
            oExportLCAmendmentRequest = oExportLCAmendmentRequest.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportLCAmendmentRequest.ExportLCAmendmentClauses = ExportLCAmendmentClause.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oExportLC = _oExportLC.Get(oExportLCAmendmentRequest.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oExportLC.ExportLCID > 0)
            {
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.MasterLCMappings = new List<MasterLCMapping>();
                _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptExportLCAmendmentRequest oReport = new rptExportLCAmendmentRequest();
            byte[] abytes = oReport.PrepareReport(oExportLCAmendmentRequest, _oExportLC, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region HTTP Save
        private bool ValidateInput(ExportLC oExportLC)
        {

            if (oExportLC.ApplicantID <= 0)
            {
                _sErrorMessage = "Please Pick, Applicant Name.";
                return false;
            }
            if (oExportLC.BBranchID_Issue<= 0)
            {
                _sErrorMessage = "Please Pick,Issue Bank.";
                return false;
            }
            if (oExportLC.BBranchID_Advice <= 0)
            {
                _sErrorMessage = "Please Select, Advice Bank.";
                return false;
            }
            if (oExportLC.CurrencyID <= 0)
            {
                _sErrorMessage = "Please Select Currency.";
                return false;
            }


            return true;
        }
        [HttpPost]
        public JsonResult SaveExportLC(ExportLC oExportLC)
        {
            oExportLC.RemoveNulls();
            try
            {
                _oExportLC = oExportLC;
                _oExportLC.ExportLCNo = _oExportLC.ExportLCNo.Trim();
                if (this.ValidateInput(_oExportLC))
                {
                    _oExportLC = _oExportLC.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExportLC.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExportLCTnC(ExportLC oExportLC)
        {
            oExportLC.RemoveNulls();
            try
            {
                _oExportLC = oExportLC;
                _oExportLC.ExportLCNo = _oExportLC.ExportLCNo.Trim();
                if (this.ValidateInput(_oExportLC))
                {
                    _oExportLC = _oExportLC.SaveMLC(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oExportLC.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_ExportPILCMapping(ExportPILCMapping oExportPILCMapping)
        {
            oExportPILCMapping.RemoveNulls();
            try
            {
                oExportPILCMapping = oExportPILCMapping.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPILCMapping = new ExportPILCMapping();
                oExportPILCMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPILCMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLog(ExportLC oExportLC)
        {

            try
            {
                _oExportLC = oExportLC.SaveLog(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_ExportLCStatusUpdate(ExportLC oExportLC)
        {
            oExportLC.RemoveNulls();
            try
            {

                _oExportLC = oExportLC;
                _oExportLC.CurrentStatus = (EnumExportLCStatus)_oExportLC.SLNo;
                _oExportLC = _oExportLC.UpdateExportLCStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult UpdateForGetOrginalCopy(ExportLC oExportLC)
        {
            _oExportLC = new ExportLC();
            string sErrorMease = "";
            try
            {
                _oExportLC = _oExportLC.UpdateForGetOrginalCopy(oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UpdateUDInfo(ExportLC oExportLC)
        //{
        //    _oExportLC = new ExportLC();
        //    string sErrorMease = "";
        //    try
        //    {
        //        _oExportLC = oExportLC.UpdateUDInfo((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        sErrorMease = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oExportLC);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult UpdateHaveQuery(ExportLC oExportLC)
        {
            _oExportLC = new ExportLC();
            string sErrorMease = "";
            try
            {
                _oExportLC = oExportLC.UpdateUDInfo((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approved(ExportLC oExportLC)
        {
            oExportLC.RemoveNulls();
            try
            {

                _oExportLC = oExportLC;
                if (this.ValidateInput(_oExportLC))
                {
                    _oExportLC.CurrentStatus = EnumExportLCStatus.Approved;
                    _oExportLC = _oExportLC.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {
                    _oExportLC.ErrorMessage = _sErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetExportLC(ExportLC oExportLC)
        {
            _oExportLC = new ExportLC();
            try
            {
                _oExportLC = _oExportLC.Get(oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportLC.MasterLCMappings = MasterLCMapping.Gets(oExportLC.ExportLCID,((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oExportLC.ExportBills = new List<ExportBill>();
                //_oExportLC.Companys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oExportLC.BankBranchs = BankBranch.GetsOwnBranchs(((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oExportLC.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetExportLC_ExportBill(ExportLC oExportLC)
        //{
        //    _oExportLC = new ExportLC();
        //    try
        //    {
        //        _oExportLC = _oExportLC.Get(oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oExportLC.ExportPILCMappings = new List<ExportPILCMapping>();
        //        _oExportLC.MasterLCMappings = new List<MasterLCMapping>();
        //        _oExportLC.ExportBills = new System.Collections.Generic.List<ExportBill>();
        //        _oExportLC.ExportBills = ExportBill.Gets(oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
        //    }
        //    catch (Exception ex)
        //    {
        //        _oExportLC = new ExportLC();
        //        _oExportLC.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oExportLC);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        private bool ValidateInput_SaveAmendment(ExportLC oExportLC)
        {

            if (oExportLC.ExportLCID <= 0)
            {
                _sErrorMessage = "Invalid  Operation.";
                return false;
            }
            //if (oExportLC.PLCLogID <= 0)
            //{
            //    _sErrorMessage = "Please enter, Amendmen request.";
            //    return false;
            //}
           

            return true;
        }
       
        #endregion

        #region HTTP Delete

        [HttpPost]
        public JsonResult DeleteExportLC(ExportLC oExportLC)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oExportLC.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeletePILCMapping(ExportPILCMapping oExportPILCMapping)
        {
            string sErrorMease = "";
            try
            {
                if (oExportPILCMapping.ExportPILCMappingID > 0)
                {
                    sErrorMease = oExportPILCMapping.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        [HttpPost]
        public JsonResult DeletePILCMappingLog(ExportPILCMapping oExportPILCMapping)
        {
            string sErrorMease = "";
            try
            {
                if (oExportPILCMapping.ExportPIID > 0)
                {
                    sErrorMease = "";//oExportPILCMapping.DeleteLog(((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #endregion

        #region Advance Search
    

        #region HttpGet For Search
        [HttpPost]
        public JsonResult AdvanchSearch(ExportLC oExportLC)
        {
            _oExportLCs = new List<ExportLC>();
            try
            {
                nLCType = (int)oExportLC.ExportLCType;
                string sSQL = GetSQL(oExportLC.ErrorMessage);
                _oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oExportLCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(string sTemp)
        {
            string sReturn1 = "SELECT * FROM View_ExportLC ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFileNo = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                int nBankBranch_Nego = Convert.ToInt32(sTemp.Split('~')[3]);
                int nBankBranch_Advise = Convert.ToInt32(sTemp.Split('~')[4]);
                string sBBranchIDs_IssueBank = Convert.ToString(sTemp.Split('~')[5]);

                int nCboLCOpenDate = Convert.ToInt32(sTemp.Split('~')[6]);
                DateTime dFromLCOpenDate = DateTime.Now;
                DateTime dToLCOpenDate = DateTime.Now;
                if (nCboLCOpenDate>0)
                {
                    dFromLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                    dToLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[8]);
                }

                int ncboReceiveDate = Convert.ToInt32(sTemp.Split('~')[9]);
                DateTime dFromLCReceiveDate = DateTime.Now;
                DateTime dToLCReceiveDate = DateTime.Now;
                if (ncboReceiveDate>0) 
                {
                    dFromLCReceiveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                    dToLCReceiveDate = Convert.ToDateTime(sTemp.Split('~')[11]);
                }
                
                int nCboShipmentDate = Convert.ToInt32(sTemp.Split('~')[12]);
                DateTime dFromShipmentDate = DateTime.Now;
                DateTime dToShipmentDate = DateTime.Now;
                if (nCboShipmentDate>0)
                {
                    dFromShipmentDate = Convert.ToDateTime(sTemp.Split('~')[13]);
                    dToShipmentDate = Convert.ToDateTime(sTemp.Split('~')[14]);
                }

                int nCboExpireDate = Convert.ToInt32(sTemp.Split('~')[15]);
                DateTime dFromExpireDate = DateTime.Now;
                DateTime dToExpireDate = DateTime.Now;
                if (nCboExpireDate>0)
                {
                    dFromExpireDate = Convert.ToDateTime(sTemp.Split('~')[16]);
                    dToExpireDate = Convert.ToDateTime(sTemp.Split('~')[17]);
                }

                int ncboAmendmentDate = Convert.ToInt32(sTemp.Split('~')[18]);
                DateTime dFromAmendmentDate = DateTime.Now;
                DateTime dToAmendmentDate = DateTime.Now;
                if (ncboAmendmentDate > 0)
                {
                    dFromAmendmentDate = Convert.ToDateTime(sTemp.Split('~')[19]);
                    dToAmendmentDate = Convert.ToDateTime(sTemp.Split('~')[20]);
                }
                string sLCStatus = Convert.ToString(sTemp.Split('~')[21]);
                int nBUID = Convert.ToInt32(sTemp.Split('~')[22]);

                int nUDRecd = Convert.ToInt32(sTemp.Split('~')[23]);
                int ncboHaveQuery = Convert.ToInt32(sTemp.Split('~')[24]);
                int ncboGetOriginalCopy = Convert.ToInt32(sTemp.Split('~')[25]);
                bool bIsExportDoIsntCreateYet = Convert.ToBoolean(sTemp.Split('~')[26]);
                bool bDeliveryChallanIssueButBillNotCreated = Convert.ToBoolean(sTemp.Split('~')[27]);                
                bool bIsLCInHand = false;
                //bool bDeliveryChallanIssueButBillNotCreated = Convert.ToBoolean(sTemp.Split('~')[27]);

                if (sTemp.Split('~').Length > 29)
                    bool.TryParse(sTemp.Split('~')[29], out bIsLCInHand);
                #endregion

                #region Make Query

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCID IN (SELECT EPI.LCID FROM ExportPI AS EPI WHERE EPI.PINo LIKE '%" + sExportPINo + "%') ";
                }
                #endregion

                #region FileNo
                if (!string.IsNullOrEmpty(sFileNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FileNo = '" + sFileNo + "' ";
                }
                #endregion

                #region Applicant
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApplicantID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BankBranch_Nego
                if (nBankBranch_Nego > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankBranchID_Negotiation =" + nBankBranch_Nego;
                }
                #endregion

                #region BankBranch_Advise
                if (nBankBranch_Advise > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankBranchID_Advice =" + nBankBranch_Advise;
                }
                #endregion

                #region Issue Bank
                if (!String.IsNullOrEmpty(sBBranchIDs_IssueBank))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BankBranchID_Issue in(" + sBBranchIDs_IssueBank + ") ";
                }
                #endregion

               
                #region LC Open Date
                if (nCboLCOpenDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboLCOpenDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region L/C Receive Date
                if (ncboReceiveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRecivedDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region Shipment Date
                if (nCboShipmentDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region Expire Date
                if (nCboExpireDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboExpireDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboExpireDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboExpireDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboExpireDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboExpireDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboExpireDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region Amendment Date
                if (ncboAmendmentDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);

                    if (ncboAmendmentDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and  CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) )";
                    }

                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }

                }
                #endregion

                #region Get Original Copy
                if (ncboGetOriginalCopy>0)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboGetOriginalCopy == 1)
                    {
                        sReturn = sReturn + " GetOriginalCopy = 0 "; // Yet Not Receive
                    }
                    if (ncboGetOriginalCopy == 2)
                    {
                        sReturn = sReturn + " GetOriginalCopy = 1 ";// Receive
                    }
                }
                #endregion
                #region HaveQuery
                if (ncboHaveQuery > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboHaveQuery == 1)
                    {
                        sReturn = sReturn + " HaveQuery = 1 "; // Yes
                    }
                    if (ncboHaveQuery == 2)
                    {
                        sReturn = sReturn + " HaveQuery = 0 ";// No
                    }
                }
                #endregion
                #region UD Receive
                if (nUDRecd > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nUDRecd == 1)
                    {
                        sReturn = sReturn + " ExportLCID NOT IN (Select ExportLCID from ExportUD)"; // Yet Not Receive
                    }
                    if (nUDRecd == 2)
                    {
                        sReturn = sReturn + " ExportLCID IN (Select ExportLCID from ExportUD) ";// Receive
                    }
                }
                #endregion

                #region Business Unit
                if (nBUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID = " + nBUID + " ";
                }
                #endregion

                #region Export Do Isn't Create Yet
                if (bIsExportDoIsntCreateYet)
                {
                    Global.TagSQL(ref sReturn);

                    if (nBUID == (int)EnumBusinessUnitType.Integrated)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM SUDeliveryOrder WHERE DOType = " + (int)EnumDOType.Export + ")) ";
                    }
                    else if(nBUID == (int)EnumBusinessUnitType.Plastic)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM FabricDeliveryOrder)) ";
                    }
                    else if (nBUID == (int)EnumBusinessUnitType.None)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM FabricDeliveryOrder)) AND ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM SUDeliveryOrder WHERE DOType = " + (int)EnumDOType.Export + "))";
                    }
                }
                #endregion

                #region Status Wise
                if (!string.IsNullOrEmpty(sLCStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CurrentStatus IN (" + sLCStatus + ") ";
                }
                #endregion

                #region Delivery Challan Issue But Bill Not Created
                if (bDeliveryChallanIssueButBillNotCreated)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID IN (SELECT ExportPIID FROM SUDeliveryOrderDetail WHERE SUDeliveryOrderDetailID IN (SELECT SUDeliveryOrderDetailID FROM SUDeliveryChallanDetail))) AND ExportLCID NOT IN (SELECT ExportLCID FROM ExportBill) ";
                    if (oBU.BusinessUnitType == EnumBusinessUnitType.Finishing || oBU.BusinessUnitType == EnumBusinessUnitType.Weaving)
                    {
                        sReturn = sReturn + "  ExportLCID IN ( Select LCID from (Select LCID,Qty,isnull((Select SUM(Qty) from FabricDeliveryChallanDetail as FDCD where FDCD.FDODID in (Select FDODID from FabricDeliveryOrderDetail where ExportPIID=TT.ExportPIID)),0)  as QtyDC,isnull((Select SUM(Qty) from ExportBillDetail as EBill where EBill.ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID=TT.ExportPIID)),0) as QtyBill from ExportPI as TT  where  LCID>0 and BUID=" + nBUID + " ) as HH where HH.QtyDC>0 and HH.QtyDC+5>HH.QtyBill) ";
                    }
                    else if (oBU.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                    {
                        sReturn = sReturn + "  ExportLCID IN (Select LCID from (Select LCID,Qty,isnull((Select SUM(Qty) from DUDeliveryChallanDetail as DUDCD where DUDCD.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderDetail.DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ExportPIID=TT.ExportPIID))),0) as QtyDC,isnull((Select SUM(Qty) from ExportBillDetail as EBill where EBill.ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID=TT.ExportPIID)),0) as QtyBill from ExportPI as TT where  LCID>0 and BUID=" + nBUID + ") as HH where HH.QtyDC>0 and HH.QtyDC+5>HH.QtyBill)";
                    }
                }
                #endregion

                #region LCInHand
                if (bIsLCInHand)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(Amount-isnull(AmountBill,0))>0.5 and CurrentStatus not in (" + (int)EnumExportLCStatus.Cancel + "," + (int)EnumExportLCStatus.Close + "," + (int)EnumExportLCStatus.Partial_Cancel +")";

                }
                #endregion

                #region UD Receive
                if (nLCType > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCType = " + nLCType;                     
                }
                #endregion

                #endregion

            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        [HttpPost]
        public JsonResult GetsDistingItem(ExportLC oExportLC)
        {
            string sSQL = "";
            _oExportLCs = new List<ExportLC>();
            try
            {

                #region EndUse
                if (oExportLC.FrightPrepaid == "FrightPrepaid")
                {
                    if (!string.IsNullOrEmpty(oExportLC.ErrorMessage))
                    {
                        sSQL = "SELECT DISTINCT(ExportLC.FrightPrepaid) As DistinctItem  FROM ExportLC where LEN(FrightPrepaid) > 1 and FrightPrepaid LIKE '%" + oExportLC.ErrorMessage + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(ExportLC.FrightPrepaid) As DistinctItem  FROM ExportLC where LEN(FrightPrepaid) > 1";
                    }
                }
                #endregion
                #region EndUse
                if (oExportLC.HSCode == "HSCode")
                {

                    if (!string.IsNullOrEmpty(oExportLC.ErrorMessage))
                    {
                        sSQL = "SELECT DISTINCT(ExportLC.HSCode) As DistinctItem  FROM ExportLC where LEN(HSCode) > 1 and HSCode LIKE '%" + oExportLC.ErrorMessage + "%'";
                    }
                    else
                    {
                        sSQL = "SELECT DISTINCT(ExportLC.HSCode) As DistinctItem  FROM ExportLC where LEN(HSCode) > 1";
                    }
                }
                #endregion


                _oExportLCs = ExportLC.Gets_DistinctItem(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportLCs.Count <= 0)
                {
                    _oExportLC = new ExportLC();
                    _oExportLC.ErrorMessage = "";
                    _oExportLCs.Add(_oExportLC);
                }

            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
                _oExportLCs.Add(_oExportLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 
    
        [HttpPost]
        public JsonResult GetsAmendmentNo(ExportLC oExportLC)
        {
            List<ExportPILCMapping> oExportPILCMappings = new List<ExportPILCMapping>();
            ExportPILCMapping oExportPILCMapping = new ExportPILCMapping();
            try
            {
                if (oExportLC.ExportLCID > 0)
                {
                    List<ExportPILCMapping> oTempExportPILCMappings = new List<ExportPILCMapping>();

                    string sSQL = "SELECT * FROM View_ExportPILCMapping WHERE ExportLCID=" + oExportLC.ExportLCID;
                    oTempExportPILCMappings = ExportPILCMapping.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oTempExportPILCMappings.Count > 0)
                    {
                        string sVersionNos = string.Join(",", oTempExportPILCMappings.Select(o => o.VersionNo).Distinct());
                        string[] splitVersionNo = sVersionNos.Split(',');
                        foreach (string sVersionNo in splitVersionNo)
                        {
                            oExportPILCMapping = new ExportPILCMapping();
                            oExportPILCMapping.ExportLCID = oExportLC.ExportLCID;
                            oExportPILCMapping.VersionNo = Convert.ToInt32(sVersionNo);
                            oExportPILCMappings.Add(oExportPILCMapping);
                        }
                    }
                    else
                    {
                        oExportPILCMapping = new ExportPILCMapping();
                        oExportPILCMapping.ExportLCID = oExportLC.ExportLCID;
                        oExportPILCMapping.VersionNo = 0;
                        oExportPILCMappings.Add(oExportPILCMapping);
                    }
                }
            }
            catch (Exception ex)
            {
                oExportPILCMapping = new ExportPILCMapping();
                oExportPILCMapping.ErrorMessage = ex.Message;
                oExportPILCMappings.Add(oExportPILCMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPILCMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
      
        [HttpPost]
        public JsonResult GetsApplicantFrightPrepaid(ExportLC oExportLC)
        {
            _oExportLCs = new List<ExportLC>();
            try
            {
                if (oExportLC.ApplicantID > 0)
                {
                    string sSQL = "SELECT * FROM View_ExportLC WHERE  LEN(FrightPrepaid) > 1 AND FrightPrepaid IN (SELECT DISTINCT(FrightPrepaid) FROM ExportLC)";
                    if (!string.IsNullOrEmpty(oExportLC.FrightPrepaid))
                    {
                        sSQL = sSQL + " AND FrightPrepaid LIKE '%" + oExportLC.FrightPrepaid + "%' ";
                    }
                    sSQL = sSQL + " ORDER BY FrightPrepaid";
                    List<ExportLC> oExportLCs = new List<ExportLC>();
                    oExportLCs = ExportLC.GetsSQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    string sFrightPrepaids = string.Join(",", oExportLCs.Select(o => o.FrightPrepaid).Distinct());
                    string[] splitFrightPrepaids = sFrightPrepaids.Split(',');
                    List<ExportLC> oTempELCs = new List<ExportLC>();
                    foreach (string sFrightPrepaid in splitFrightPrepaids)
                    {
                        oTempELCs = new List<ExportLC>();
                        oTempELCs = oExportLCs.Where(o => o.FrightPrepaid == sFrightPrepaid).ToList();
                        _oExportLCs.Add(oTempELCs[0]);
                    }
                }
                else
                {
                    oExportLC = new ExportLC();
                    oExportLC.ErrorMessage = "No Applicant Found.";
                    _oExportLCs.Add(oExportLC);
                }
            }
            catch (Exception ex)
            {
                oExportLC = new ExportLC();
                oExportLC.ErrorMessage = ex.Message;
                _oExportLCs.Add(oExportLC);
            }
            var jsonResult = Json(_oExportLCs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
      
        #endregion

        #region Attachment (Export LC)
        [HttpPost]
        public string UploadAttchment(HttpPostedFileBase file, ExportLCAttachment oExportLCAttachment)
        {
            string sFeedBackMessage = "File Upload successfully";
            byte[] data;
            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    double nMaxLength = 1024 * 1024;
                    if (data == null || data.Length <= 0)
                    {
                        sFeedBackMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sFeedBackMessage = "You can select maximum 1MB file size!";
                    }
                    else if (oExportLCAttachment.ExportLCID <= 0)
                    {
                        sFeedBackMessage = "Your Selected Export LC Is Invalid!";
                    }
                    else
                    {
                        oExportLCAttachment.AttatchFile = data;
                        oExportLCAttachment.AttatchmentName = file.FileName;
                        oExportLCAttachment.FileType = file.ContentType;
                        oExportLCAttachment = oExportLCAttachment.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
                else
                {
                    sFeedBackMessage = "Please select a file!";
                }
            }
            catch (Exception ex)
            {
                sFeedBackMessage = "";
                sFeedBackMessage = ex.Message;
            }

            return sFeedBackMessage;
        }

        [HttpPost]
        public ActionResult DownloadAttachment(FormCollection oFormCollection)
        {
            ExportLCAttachment oExportLCAttachment = new ExportLCAttachment();
            try
            {
                int nID = Convert.ToInt32(oFormCollection["ExportLCAttchmentID"]);
                oExportLCAttachment = ExportLCAttachment.GetWithAttachFile(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportLCAttachment.AttatchFile != null)
                {
                    var file = File(oExportLCAttachment.AttatchFile, oExportLCAttachment.FileType);
                    file.FileDownloadName = oExportLCAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oExportLCAttachment.AttatchmentName);
            }
        }

        [HttpPost]
        public JsonResult GetsAttachmentById(ExportLCAttachment oExportLCAttachment)
        {
            List<ExportLCAttachment> oExportLCAttachments = new List<ExportLCAttachment>();
            try
            {
                oExportLCAttachments = ExportLCAttachment.GetsAttachmentById(oExportLCAttachment.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportLCAttachments = new List<ExportLCAttachment>();
                oExportLCAttachment = new ExportLCAttachment();
                oExportLCAttachment.ErrorMessage = ex.Message;
                oExportLCAttachments.Add(oExportLCAttachment);
            }
            var jsonResult = Json(oExportLCAttachments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult DeleteExportLCAttachment(ExportLCAttachment oExportLCAttachment)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportLCAttachment.Delete(oExportLCAttachment.ExportLCAttachmentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<ExportLCAttachment> GetsExportLCAttachment(ExportLCAttachment oExportLCAttachment)
        {
            List<ExportLCAttachment> oExportLCAttachments = new List<ExportLCAttachment>();
            oExportLCAttachments = ExportLCAttachment.GetsAttachmentById(oExportLCAttachment.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return oExportLCAttachments;
        }

        #endregion

        #region ExportLC Log
        [HttpPost]
        public JsonResult GetExportLCLog(ExportLCReport oExportLCReport)
        {
            _oExportLC = new ExportLC();
            try
            {
                _oExportLC = _oExportLC.GetLogForVersionNo(oExportLCReport.ExportLCID, oExportLCReport.VersionNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportLC.ExportLCLogID >0)
                {
                  //  string sSQL = "SELECT * FROM View_ExportPILCMappingLog WHERE ExportLCID = " + _oExportLC.ExportLCID + " AND VersionNo = '" + oExportLCReport.VersionNo + "'";
                    _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsLogByLCID(_oExportLC.ExportLCLogID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oExportLC = _oExportLC.Get(oExportLCReport.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportLC.ExportPILCMappings = ExportPILCMapping.Gets(_oExportLC.ExportLCID, _oExportLC.VersionNo,((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oExportLC.MasterLCMappings = MasterLCMapping.Gets(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get master LC
        [HttpPost]
        public JsonResult GetExportLCByBU(ExportLC oExportLC)
        {
            _oExportLCs = new List<ExportLC>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportLC WHERE BUID = "+oExportLC.BUID;
                if(!string.IsNullOrEmpty(oExportLC.ExportLCNo))
                {
                    sSQL += "  AND ExportLCNo LIKE '%"+oExportLC.ExportLCNo+"%'";
                }
                _oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportLC = new ExportLC();
                _oExportLC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLCs);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region Print
        public ActionResult PrintLCStatement(int nExportLCID,  double nts)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oExportLC = _oExportLC.Get(nExportLCID,((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptExportLC oReport = new rptExportLC();
            byte[] abytes = oReport.PrepareReport(_oExportLC, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintLCReceiving(int nExportLCID, double nts)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<ExportBill> oExportBills = new List<ExportBill>();
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();

            List<ExportLC> oExportLCsLog = new List<ExportLC>();
            

            string sSQL = "";
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            _oExportLC = _oExportLC.Get(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportLC.ExportPILCMappings = ExportPILCMapping.GetsByLCID(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportLCsLog = ExportLC.GetsLog(_oExportLC.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportLCsLog.Add(_oExportLC);
            sSQL = "SELECT * FROM View_ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID=" + nExportLCID + ")";
            oExportPIDetails = ExportPIDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "SELECT * FROM View_ExportBill WHERE ExportLCID=" + nExportLCID;
            oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "SELECT * FROM View_ExportBillDetail WHERE ExportBillID in (SELECT ExportBillID FROM ExportBill WHERE ExportLCID="+ nExportLCID +")";
            oExportBillDetails=ExportBillDetail.GetsBySQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oExportLC.DeliveryToName = string.Join(",", _oExportLC.ExportPILCMappings.Select(x => x.BuyerName).Distinct().ToList());

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oExportLC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing || oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving)
            {
                sSQL = "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE ExportPIID in (" + string.Join(",", _oExportLC.ExportPILCMappings.Select(x => x.ExportPIID).ToList()) + ")))";
                //sSQL = "SELECT * FROM View_FabricDeliveryChallanDetail as FDCD WHERE FDCD.FDODID IN (SELECT FDOD.FDODID FROM FabricDeliveryOrderDetail as FDOD WHERE ExportPIID in (" + string.Join(",", _oExportLC.ExportPILCMappings.Select(x => x.ExportPIID).ToList()) + "))";
                oFDCRegisters = FDCRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing )
            {
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and  ExportPIID in (" + string.Join(",", _oExportLC.ExportPILCMappings.Select(x => x.ExportPIID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and  ExportPIID in (" + string.Join(",", _oExportLC.ExportPILCMappings.Select(x => x.ExportPIID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            rptExportLCReceiving oReport = new rptExportLCReceiving();
            byte[] abytes = oReport.PrepareReport(_oExportLC, oExportPIDetails, oCompany, oBusinessUnit, oExportBills, oExportBillDetails, oFDCRegisters, oDyeingOrderReports, oExportLCsLog);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Excel
        [HttpPost]
        public ActionResult SetDataInSession(ExportLC oExportLC)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportLC);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void PrintListInXL(int nBUID)
        {
            ExportLC oELC = new ExportLC();
            oELC = (ExportLC)Session[SessionInfo.ParamObj];
            string sSQL = GetSQL(oELC.ErrorMessage);
            _oExportLCs = ExportLC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oExportLCs.Count > 0)
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
                table_header.Add(new TableHeader { Header = "File No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Applicant Name", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C No", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shipment Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Expiry Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Opening Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC Recived Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Issue Bank Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Branch Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Note Query", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Get Orginal Copy", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Invoice Amount", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To Invoice", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "A.No", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "A.Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 15f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export LC List");

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

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "Export LC List"; cell.Style.Font.Bold = true;
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
                    foreach (ExportLC oItem in _oExportLCs)
                    {
                        nStartCol = 2;
                        //ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.FileNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ApplicantName, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExportLCNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.Formatter = oItem.Currency + " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Amount.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ShipmentDateST, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExpiryDateST, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.OpeningDateST, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LCRecivedDateST, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BankName_Issue, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BBranchName_Issue, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.NoteQuery, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.GetOrginalCopySt, false, ExcelHorizontalAlignment.Left, false, false);

                        ExcelTool.Formatter = oItem.Currency + " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.AmountBill.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        //ExcelTool.Formatter = oItem.Currency +  " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (oItem.Amount-oItem.AmountBill).ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        ExcelTool.Formatter = "#,##0;(#,##0)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.VersionNo.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.AmendmentDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.CurrentStatusInST, false, ExcelHorizontalAlignment.Left, false, false);

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = _oExportLCs[0].Currency + " #,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oExportLCs.Sum(c => c.Amount).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oExportLCs.Sum(c => c.AmountBill).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oExportLCs.Sum(c => (c.Amount - c.AmountBill)).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Export_LC_List.xlsx");
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

