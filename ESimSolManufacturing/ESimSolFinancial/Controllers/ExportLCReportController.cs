

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ESimSolFinancial.Models;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using System.Data;
using Newtonsoft.Json;


namespace ESimSolFinancial.Controllers.ReportController
{
    public class ExportLCReportController : Controller
    {
        #region Declaration
        ExportLC _oExportLC = new ExportLC();
        ExportLCReport _oExportLCReport = new ExportLCReport();
        List<ExportLCReport> _oExportLCReports = new List<ExportLCReport>();
        ExportLCReportDetail _oExportLCReportDetail = new ExportLCReportDetail();
        List<ExportLCReportDetail> _oExportLCReportDetails = new List<ExportLCReportDetail>();
        #endregion

        public ActionResult View_ExportLCReports(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sSQL = "and  EPI.BUID = " + buid + " and CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Today.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Today.ToString("dd MMM yyyy") + "',106))";
            _oExportLCReports = new List<ExportLCReport>();
            _oExportLCReports = ExportLCReport.Gets(sSQL, EnumLCReportLevel.LCVersionLevel, (int)Session[SessionInfo.currentUserID]);

            ViewBag.LCReportLevelObjs = EnumObject.jGets(typeof(EnumLCReportLevel));//LCReportLevelObj.Gets();
            ViewBag.NegoBanks = BankBranch.GetsOwnBranchs((int)Session[SessionInfo.currentUserID]);
            ViewBag.AdviceBanks = BankBranch.GetsOwnBranchs((int)Session[SessionInfo.currentUserID]);
            //ViewBag.MarketingAccounts = MarketingAccount.GetsByBUAndGroup(buid, (int)Session[SessionInfo.currentUserID]); ;
            ViewBag.MarketingAccounts = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ExportLCStatus = EnumObject.jGets(typeof(EnumExportLCStatus)).Where(o => o.id != 0).ToList();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oExportLCReports);
        }
        public ActionResult View_ExportLCReportDetails(string sExportPIID, string sBtnID, string sMsg)
        {
            _oExportLCReportDetails = new List<ExportLCReportDetail>();
            _oExportLCReportDetails = this.GetExportLCReportDetails(sExportPIID, sBtnID);

            //if (!_bNoError)
            //{
            //    _oExportLCReportDetail = new ExportLCReportDetail();
            //    _oExportLCReportDetail.ErrorMessage = _sErrorMessage;
            //    _oExportLCReportDetails.Add(_oExportLCReportDetail);
            //}
            ViewBag.ExportLC = _oExportLC;
            ViewBag.ExportPIIDType = sBtnID;   //if ExportPIIDType=="ExportPI" then sExportPIID is ExportPIID And if ExportPIIDType ==  "ExportLC" then sExportPIID is ExportLCID

            int nCount = sExportPIID.Split('~').Length;
            if (nCount > 1)
            {
                sExportPIID = Convert.ToString(sExportPIID.Split('~')[0]);
            }

            ViewBag.ExportPIID = Convert.ToInt32(sExportPIID);
            return View(_oExportLCReportDetails);
        }
        private List<ExportLCReportDetail> GetExportLCReportDetails(string sExportPIID, string sIDType)
        {
            List<ExportLCReportDetail> oExportLCReportDetails = new List<ExportLCReportDetail>();
            _oExportLC = new ExportLC();

            int nExportLCID = 0;
            int nVersionNo = 0;

            //int nCount = sExportPIID.Split('~').Length;
            //if (nCount > 1)
            //{
            //    nExportLCID = Convert.ToInt32(sExportPIID.Split('~')[0]);
            //    nVersionNo = Convert.ToInt32(sExportPIID.Split('~')[1]);

            //    sExportPIID = nExportLCID.ToString();
            //}

            //if (Convert.ToInt32(sExportPIID) > 0)
            //{
            //    int nExportPIID = 0;
            //    EnumTextileUnit nTextileUnit = EnumTextileUnit.None;
            //    string sPIIDs = "";

            //    string sSQL = "";

            //    if (sIDType == "ExportPIID")
            //    {
            //        nExportPIID = Convert.ToInt32(sExportPIID);
            //        ExportPI oExportPI = new ExportPI();
            //        oExportPI = oExportPI.Get(nExportPIID, (int)Session[SessionInfo.currentUserID]);
            //        nTextileUnit = oExportPI.TextileUnit;
            //        sPIIDs = "" + nExportPIID + "";

            //        if (oExportPI.LCID > 0)
            //        {
            //            _oExportLC = _oExportLC.Get(oExportPI.LCID, (int)Session[SessionInfo.currentUserID]);
            //        }

            //    }
            //    else if (sIDType == "ExportLCID")
            //    {
            //        _oExportLC = _oExportLC.Get(nExportLCID, (int)Session[SessionInfo.currentUserID]);
            //        nTextileUnit = _oExportLC.TextileUnit;

            //        List<ExportPI> oExportPIs = new List<ExportPI>();
            //        sSQL = "SELECT * FROM View_ExportPI WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPILCMapping WHERE ExportLCID = " + nExportLCID + " AND VersionNo = " + nVersionNo + ")";
            //        oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            //        if (nTextileUnit == EnumTextileUnit.None)
            //        {
            //            if (oExportPIs.Count > 0)
            //            {
            //                nTextileUnit = oExportPIs[0].TextileUnit;
            //            }
            //        }

            //        sPIIDs = string.Join(",", oExportPIs.Select(o => o.ExportPIID).Distinct());
            //    }
            //    else
            //    {
            //        _bNoError = false;
            //        _sErrorMessage = "Wrong Info.";
            //    }

            //    if (!string.IsNullOrEmpty(sPIIDs))
            //    {

            //        #region Export PI Detail
            //        List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            //        sSQL = "SELECT * FROM View_ExportPIDetail WHERE ExportPIID IN (" + sPIIDs + ")";
            //        oExportPIDetails = ExportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //        #endregion

            //        #region Spinning
            //        if (nTextileUnit == EnumTextileUnit.Spinning)
            //        {

            //            #region SUDelivery Delivery Order Detail
            //            List<SUDeliveryOrderDetail> oSUDeliveryOrderDetails = new List<SUDeliveryOrderDetail>();
            //            List<SUDeliveryOrderDetail> oTempSUDODs = new List<SUDeliveryOrderDetail>();
            //            sSQL = "SELECT * FROM View_SUDeliveryOrderDetail WHERE ExportPIID IN (" + sPIIDs + ")";
            //            oSUDeliveryOrderDetails = SUDeliveryOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //            #endregion


            //            double nDOQty = 0,
            //                   nChallanQty = 0,
            //                   nDOValue = 0,
            //                   nChallanValue = 0;
            //            if (oExportPIDetails.Count > 0)
            //            {
            //                foreach (ExportPIDetail oItem in oExportPIDetails)
            //                {
            //                    oTempSUDODs = new List<SUDeliveryOrderDetail>();
            //                    oTempSUDODs = oSUDeliveryOrderDetails.Where(o => o.ProductID == oItem.ProductID & o.ExportPIID == oItem.ExportPIID).ToList();

            //                    if (oTempSUDODs.Count > 0)
            //                    {
            //                        nDOQty = oTempSUDODs.Select(o => o.Qty).Sum();
            //                        nChallanQty = oTempSUDODs.Select(o => o.ChallanQty).Sum();
            //                        nDOValue = oTempSUDODs.Select(o => o.Qty * o.UnitPrice).Sum();
            //                        nChallanValue = oTempSUDODs.Select(o => o.ChallanValue).Sum();

            //                        _oExportLCReportDetail = new ExportLCReportDetail();
            //                        _oExportLCReportDetail.ExportPIID = oItem.ExportPIID;
            //                        _oExportLCReportDetail.ExportPIDetailID = oItem.ExportPIDetailID;
            //                        _oExportLCReportDetail.PINo = oItem.PINo;
            //                        _oExportLCReportDetail.ProductName = oItem.ProductName;
            //                        _oExportLCReportDetail.CurrencySymbol = oItem.Currency;
            //                        _oExportLCReportDetail.MUName = oTempSUDODs[0].MUSymbol;
            //                        _oExportLCReportDetail.PIQty = oItem.Qty;
            //                        _oExportLCReportDetail.DOQty = nDOQty;
            //                        _oExportLCReportDetail.ChallanQty = nChallanQty;
            //                        _oExportLCReportDetail.PIValue = oItem.Amount;
            //                        _oExportLCReportDetail.DOValue = nDOValue;
            //                        _oExportLCReportDetail.ChallanValue = nChallanValue;
            //                    }
            //                    else
            //                    {
            //                        _oExportLCReportDetail = new ExportLCReportDetail();
            //                        _oExportLCReportDetail.ExportPIID = oItem.ExportPIID;
            //                        _oExportLCReportDetail.ExportPIDetailID = oItem.ExportPIDetailID;
            //                        _oExportLCReportDetail.PINo = oItem.PINo;
            //                        _oExportLCReportDetail.ProductName = oItem.ProductName;
            //                        _oExportLCReportDetail.CurrencySymbol = oItem.Currency;
            //                        _oExportLCReportDetail.MUName = oItem.MUName;
            //                        _oExportLCReportDetail.PIQty = oItem.Qty;
            //                        _oExportLCReportDetail.DOQty = nDOQty;
            //                        _oExportLCReportDetail.ChallanQty = nChallanQty;
            //                        _oExportLCReportDetail.PIValue = oItem.Amount;
            //                        _oExportLCReportDetail.DOValue = nDOValue;
            //                        _oExportLCReportDetail.ChallanValue = nChallanValue;
            //                    }
            //                    oExportLCReportDetails.Add(_oExportLCReportDetail);
            //                }
            //            }
            //        }
            //        #endregion

            //        #region Weaving
            //        else if (nTextileUnit == EnumTextileUnit.Weaving)
            //        {

            //            #region Fabric Delivery Order Detail
            //            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            //            List<FabricDeliveryOrderDetail> oTempFDODs = new List<FabricDeliveryOrderDetail>();
            //            sSQL = "SELECT * FROM View_FabricDeliveryOrderDetail WHERE ExportPIID IN (" + sPIIDs + ")";
            //            oFabricDeliveryOrderDetails = FabricDeliveryOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //            #endregion

            //            double nDOQty = 0,
            //                 nChallanQty = 0,
            //                 nDOValue = 0,
            //                 nChallanValue = 0;


            //            if (oExportPIDetails.Count > 0)
            //            {
            //                foreach (ExportPIDetail oItem in oExportPIDetails)
            //                {
            //                    oTempFDODs = new List<FabricDeliveryOrderDetail>();
            //                    oTempFDODs = oFabricDeliveryOrderDetails.Where(o => o.ProductID == oItem.ProductID & o.ExportPIID == oItem.ExportPIID).ToList();

            //                    if (oTempFDODs.Count > 0)
            //                    {
            //                        nDOQty = oTempFDODs.Select(o => o.Qty).Sum();
            //                        nChallanQty = oTempFDODs.Select(o => o.ChallanQty).Sum();
            //                        nDOValue = oTempFDODs.Select(o => o.Qty * o.UnitPrice).Sum();
            //                        nChallanValue = oTempFDODs.Select(o => o.ChallanValue).Sum();

            //                        _oExportLCReportDetail = new ExportLCReportDetail();
            //                        _oExportLCReportDetail.ExportPIID = oItem.ExportPIID;
            //                        _oExportLCReportDetail.ExportPIDetailID = oItem.ExportPIDetailID;
            //                        _oExportLCReportDetail.PINo = oItem.PINo;
            //                        _oExportLCReportDetail.ProductName = oItem.ProductName;
            //                        _oExportLCReportDetail.CurrencySymbol = oItem.Currency;
            //                        _oExportLCReportDetail.MUName = oTempFDODs[0].MUName;
            //                        _oExportLCReportDetail.PIQty = oItem.Qty;
            //                        _oExportLCReportDetail.DOQty = nDOQty;
            //                        _oExportLCReportDetail.ChallanQty = nChallanQty;
            //                        _oExportLCReportDetail.PIValue = oItem.Amount;
            //                        _oExportLCReportDetail.DOValue = nDOValue;
            //                        _oExportLCReportDetail.ChallanValue = nChallanValue;
            //                    }
            //                    else
            //                    {
            //                        _oExportLCReportDetail = new ExportLCReportDetail();
            //                        _oExportLCReportDetail.ExportPIID = oItem.ExportPIID;
            //                        _oExportLCReportDetail.ExportPIDetailID = oItem.ExportPIDetailID;
            //                        _oExportLCReportDetail.PINo = oItem.PINo;
            //                        _oExportLCReportDetail.ProductName = oItem.ProductName;
            //                        _oExportLCReportDetail.CurrencySymbol = oItem.Currency;
            //                        _oExportLCReportDetail.MUName = oItem.MUName;
            //                        _oExportLCReportDetail.PIQty = oItem.Qty;
            //                        _oExportLCReportDetail.DOQty = nDOQty;
            //                        _oExportLCReportDetail.ChallanQty = nChallanQty;
            //                        _oExportLCReportDetail.PIValue = oItem.Amount;
            //                        _oExportLCReportDetail.DOValue = nDOValue;
            //                        _oExportLCReportDetail.ChallanValue = nChallanValue;
            //                    }
            //                    oExportLCReportDetails.Add(_oExportLCReportDetail);
            //                }
            //            }
            //        }
            //        //else
            //        //{
            //        //    _bNoError = false;
            //        //    _sErrorMessage = "No Textile Unit Found.";
            //        //}
            //        #endregion
            //    }               
            //}
            //else
            //{
            //    _bNoError = false;
            //}

            //if (oExportLCReportDetails.Count > 0)
            //{
            //    _oExportLCReportDetail = new ExportLCReportDetail();
            //    _oExportLCReportDetail.ProductName = "Total : ";
            //    _oExportLCReportDetail.PIQty = oExportLCReportDetails.Select(o => o.PIQty).Sum();
            //    _oExportLCReportDetail.DOQty = oExportLCReportDetails.Select(o => o.DOQty).Sum();
            //    _oExportLCReportDetail.ChallanQty = oExportLCReportDetails.Select(o => o.ChallanQty).Sum();
            //    _oExportLCReportDetail.CurrencySymbol = oExportLCReportDetails[0].CurrencySymbol;
            //    _oExportLCReportDetail.MUName = "";
            //    _oExportLCReportDetail.PIValue = oExportLCReportDetails.Select(o => o.PIValue).Sum();
            //    _oExportLCReportDetail.DOValue = oExportLCReportDetails.Select(o => o.DOValue).Sum();
            //    _oExportLCReportDetail.ChallanValue = oExportLCReportDetails.Select(o => o.ChallanValue).Sum();
            //    oExportLCReportDetails.Add(_oExportLCReportDetail);
            //}

            return oExportLCReportDetails;
        }
        
        #region Searching
        [HttpPost]
        public JsonResult AdvanchSearch(ExportLCReport oExportLCReport)
        {
            _oExportLCReports = new List<ExportLCReport>();            
            try
            {
                string sSQL = GetSQL(oExportLCReport.SearchingCriteria, oExportLCReport.BUID, (EnumLCReportLevel)oExportLCReport.LCReportLevelInt);
                _oExportLCReports = ExportLCReport.Gets(sSQL, (EnumLCReportLevel)oExportLCReport.LCReportLevelInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportLCReport = new ExportLCReport();
                _oExportLCReport.ErrorMessage = ex.Message;
                _oExportLCReports.Add(_oExportLCReport);
            }

            var jsonResult = Json(_oExportLCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQL(string sSearchingData, int nBUID, EnumLCReportLevel nLCReportLevel)
        {
            string sTemp = "";
            DateTime dLCOpenStartDate= DateTime.Now;
            DateTime dLCOpenEndDate = DateTime.Now;
            DateTime dLCReceivedStartDate = DateTime.Now;
            DateTime dLCReceivedEndDate = DateTime.Now;
            EnumCompareOperator eLCOpenDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
             sTemp = sSearchingData.Split('~')[1];
             if (!String.IsNullOrEmpty(sTemp))
             {
                 dLCOpenStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
             }
             sTemp = sSearchingData.Split('~')[2];
             if (!String.IsNullOrEmpty(sTemp))
             {
                 dLCOpenEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
             }
            
          
            EnumCompareOperator eLCReceivedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            sTemp = sSearchingData.Split('~')[4];
            if (!String.IsNullOrEmpty(sTemp))
            {
                dLCReceivedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            }
            sTemp = sSearchingData.Split('~')[5];
            if (!String.IsNullOrEmpty(sTemp))
            {
                dLCReceivedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            }
            string sApplicantIDs =Convert.ToString(sSearchingData.Split('~')[6]);
            string sBankBranchID_Issue = Convert.ToString(sSearchingData.Split('~')[7]);
            int nBankBranchID_Nego = Convert.ToInt32(sSearchingData.Split('~')[8]);
            int nMKTPerson = Convert.ToInt32(sSearchingData.Split('~')[9]);

            string sReturn = " ";

            #region BUID
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPI.BUID = " + nBUID.ToString();
            }
            #endregion

            #region Applicant
            if (!string.IsNullOrEmpty(sApplicantIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApplicantID IN (" + sApplicantIDs + ") ";
            }
            #endregion

            #region Nego Bank
            if (nBankBranchID_Nego > 0 && nBankBranchID_Nego !=null)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankBranchID_Negotiation  IN (" + nBankBranchID_Nego + ") ";
            }
            #endregion

            #region Issue Bank
            if (!string.IsNullOrEmpty(sBankBranchID_Issue))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BankBranchID_Issue IN(" + sBankBranchID_Issue + ")";
            }
            #endregion

            #region LCOpen Date
            if (eLCOpenDate != EnumCompareOperator.None)
            {
                string stemp = "";
                if (nLCReportLevel == EnumLCReportLevel.LCLevel)
                {
                     stemp = "ELC.OpeningDate";
                }
                else
                {
                    stemp = "EPILCM.[Date]";
                }
                if (eLCOpenDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + stemp + ",106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCOpenDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12), " + stemp + ",106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCOpenDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + stemp + ",106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCOpenDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + stemp + ",106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCOpenDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + stemp + ",106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCOpenDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + stemp + ",106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCOpenEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region LCReceiveDate
            if (eLCReceivedDate != EnumCompareOperator.None)
            {
                if (eLCReceivedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCReceivedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCReceivedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCReceivedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCReceivedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eLCReceivedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dLCReceivedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region MKTPerson
            if (nMKTPerson!=0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EPILCM.ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE MKTEmpID = " + nMKTPerson + ")";
            }
            #endregion

            
            return sReturn;
        }
        #endregion

        #region Excel Print
        public void ExcelExportLCReport(string sParams, int nBUID, int nLCReportLevelInt)
        {
            _oExportLCReports = new List<ExportLCReport>();
            _oExportLCReport = new ExportLCReport();
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            string sSQL = GetSQL(sParams, nBUID, (EnumLCReportLevel)nLCReportLevelInt);
            _oExportLCReports = ExportLCReport.Gets(sSQL, (EnumLCReportLevel)nLCReportLevelInt, (int)Session[SessionInfo.currentUserID]);

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export LC Report List");
                sheet.Name = "Export LC Report";

                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 22; //Buyer
                sheet.Column(4).Width = 13; //LC No.
                sheet.Column(5).Width = 13; //LCOpenDateSt
                sheet.Column(6).Width = 22; //AmendmentNo / VersionNo
                sheet.Column(7).Width = 13; //AmendmentDateSt

                sheet.Column(8).Width = 15; //Total LC/Am Quantity
                sheet.Column(9).Width = 15; //Total LC/Am Value ($)
                sheet.Column(10).Width = 28; //Last Challan Date
                sheet.Column(11).Width = 28; //Total Delivery Quantity
                sheet.Column(12).Width = 28; //Total Delivery Amount
                sheet.Column(13).Width = 28; //Balance Quantity
                sheet.Column(14).Width = 28; //Balance Amount ($)
                sheet.Column(15).Width = 22; //Invoice No
                sheet.Column(16).Width = 22; //Invoice Date.
                //sheet.Column(17).Width = 22; //YetToDOValue
                //sheet.Column(18).Width = 22; //ChallanValueSt
                //sheet.Column(19).Width = 22; //YetToChallanValue
                sheet.Column(17).Width = 22; //InvoiceValueSt
                sheet.Column(18).Width = 22; //YetToInvoiceValue


                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily LC Receive"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                //if (_oExportLCReports.Count > 0)
                //{
                //    sheet.Cells[rowIndex, 2, rowIndex, 21].Merge = true;
                //    cell = sheet.Cells[rowIndex, 2]; cell.Value = _oExportLCReports[0].TextileUnitSt; cell.Style.Font.Bold = true;
                //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    rowIndex = rowIndex + 3;
                //}
                #endregion

                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "L/C Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "AM No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "AM Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Total LC/Am Quantity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Total LC/Am Value ($)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = "Last Challan Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Total Delivery Quantity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = "Total Delivery Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = "Balance Quantity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 14]; cell.Value = "Balance Amount ($)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 15]; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 16]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, 17]; cell.Value = "Yet To DO Value"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, 18]; cell.Value = "Challan Value"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, 19]; cell.Value = "Yet To Challan Value"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 17]; cell.Value = "Invoice Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 18]; cell.Value = "Yet To Invoice Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                foreach (ExportLCReport oItem in _oExportLCReports)
                {
                    nCount++;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LCOpenDateSt; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.VersionNo.ToString(); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.AmendmentDateSt; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 8]; cell.Value =Math.Round(oItem.Qty,2); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 9]; cell.Value = Math.Round(oItem.LCValue,2); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.LastDeliveryDateSt; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.Qty_DC; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.ChallanValue; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 13]; cell.Value = (oItem.Qty - oItem.Qty_DC); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 14]; cell.Value = (oItem.YetToChallanValue); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
                    //cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.InvoiceDateSt; cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    ////cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.ChallanValue.ToString(); cell.Style.Font.Bold = false;
                    ////border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    ////cell = sheet.Cells[rowIndex, 19]; cell.Value = (oItem.LCValue - oItem.ChallanValue).ToString(); cell.Style.Font.Bold = false;
                    ////border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.InvoiceValue.ToString(); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, 18]; cell.Value = (oItem.LCValue - oItem.InvoiceValue).ToString(); cell.Style.Font.Bold = false;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportLCReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void ExcelExportLCReportDetail(string sExportPIID, string sIDType)
        {
            _oExportLCReportDetails = new List<ExportLCReportDetail>();
            _oExportLCReportDetails = this.GetExportLCReportDetails(sExportPIID, sIDType);

            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export LC Report List");
                sheet.Name = "Export LC Report";

                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 22; //PINo
                sheet.Column(4).Width = 20; //ProductName
                sheet.Column(5).Width = 13; //Unit
                sheet.Column(6).Width = 22; //PIQtySt
                sheet.Column(7).Width = 13; //DOQtySt
                sheet.Column(8).Width = 13; //YetToDoQtySt
                sheet.Column(9).Width = 13; //ChallanQtySt
                sheet.Column(10).Width = 13; //YetToChallanQtySt
                sheet.Column(11).Width = 13; //PIValueSt
                sheet.Column(12).Width = 13; //DOValueSt
                sheet.Column(13).Width = 13; //ChallanValueSt

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Export LC Report List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region ExportLC Information
                if (_oExportLC.ExportLCID > 0)
                {
                    string sVersionNo = (_oExportLC.VersionNo > 0 ? "A.No : " + _oExportLC.VersionNo + " A.Date : " + _oExportLC.AmendmentDateSt : "");
                    sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "LC No : " + _oExportLC.ExportLCNo + sVersionNo + "            " + " LC Opening Date : " + _oExportLC.OpeningDateST + "             " + "Applicant : " + _oExportLC.ApplicantName; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;
                }
                #endregion

                #region Column Header

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 3]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6]; cell.Value = "PI Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 7]; cell.Value = "DO Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Yet To DO Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Challan Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 10]; cell.Value = "Yet To Challan Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 11]; cell.Value = "PI Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 12]; cell.Value = "DO Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 13]; cell.Value = "Challan Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                rowIndex = rowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                foreach (ExportLCReportDetail oItem in _oExportLCReportDetails)
                {
                    nCount++;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.PIQty.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.DOQty.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = (oItem.PIQty - oItem.DOQty).ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.ChallanQty.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = (oItem.PIQty - oItem.ChallanQty).ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.PIValue.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.DOValue.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.ChallanValue.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                  
                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportLCReportDetail.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region print
        public ActionResult ExportReportPrintList(string sParams, int nBUID, int nLCReportLevel, bool bPrintWithMLC)
        {
            _oExportLCReports = new List<ExportLCReport>();
            string sSQL = GetSQL(sParams, nBUID, (EnumLCReportLevel)nLCReportLevel);
            _oExportLCReports = ExportLCReport.Gets(sSQL, (EnumLCReportLevel)nLCReportLevel, (int)Session[SessionInfo.currentUserID]);
            if (_oExportLCReports.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptExportLCReport oReport = new rptExportLCReport();
                byte[] abytes = oReport.PrepareReport(_oExportLCReports, nLCReportLevel, oCompany, bPrintWithMLC);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }

        public void ExportReportPrintListInExcel(string sParams, int nBUID, int nLCReportLevel)
        {
            _oExportLCReports = new List<ExportLCReport>();
            string sSQL = GetSQL(sParams, nBUID, (EnumLCReportLevel)nLCReportLevel);
            _oExportLCReports = ExportLCReport.Gets(sSQL, (EnumLCReportLevel)nLCReportLevel, (int)Session[SessionInfo.currentUserID]);

            if (_oExportLCReports.Count > 0)
            {
                #region Dataget
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                #endregion
                #region Export Excel
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
     

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export LC Report (" + EnumObject.jGet((EnumLCReportLevel)nLCReportLevel) + " )");
                    sheet.Name = "Export LC Report (" + EnumObject.jGet((EnumLCReportLevel)nLCReportLevel) + " )";
                    sheet.Column(2).Width = 7;//SL
                    sheet.Column(3).Width = 25;//LC no
                    sheet.Column(4).Width = 35;//Buyer
                    sheet.Column(5).Width = 35;//Concern
                    sheet.Column(6).Width = 13;//Lc Date
                    nEndCol = 7;
                    if (nLCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
                    {
                        sheet.Column(nEndCol).Width = 18;//Amendment NO
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 18;//Amendment Date
                        nEndCol++;
                    }
                    else if (nLCReportLevel == (int)EnumLCReportLevel.PILevel)
                    {
                        sheet.Column(nEndCol).Width = 18;//Amendment NO
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 18;//Amendment Date
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 15;//PI NO
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 15;//PI Date
                        nEndCol++;
                    }
                    else if (nLCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                    {
                        sheet.Column(nEndCol).Width = 18;//Amendment NO
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 18;//Amendment Date
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 15;//PI NO
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 15;//PI Date
                        nEndCol++;
                        sheet.Column(nEndCol).Width = 25;//Product 
                        nEndCol++;
                    }
                    sheet.Column(nEndCol).Width = 30;//nego bank
                    nEndCol++;
                    sheet.Column(nEndCol).Width = 30;//issue bank
                    nEndCol++;
                    sheet.Column(nEndCol).Width = 15;//Qty
                    nEndCol++;
                    sheet.Column(nEndCol).Width = 20;//value
                    

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex = nRowIndex + 1;

                  
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value =  oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Left.Style = ExcelBorderStyle.Thin;
                    nRowIndex = nRowIndex + 1;

                  

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Export LC Report (" + EnumObject.jGet((EnumLCReportLevel)nLCReportLevel) + " )"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15;  cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex = nRowIndex + 1;



                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Value = " "; cell.Merge = true; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;
                    border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Column Header
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "#SL"; cell.Merge = true; cell.Style.Font.Bold = true; 
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "LC No"; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Concern Person"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "LC Date"; cell.Merge = true; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nEndCol = 6;//reset count
                    if (nLCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
                    {
                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment No"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment Date"; cell.Merge = true; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else if (nLCReportLevel == (int)EnumLCReportLevel.PILevel)
                    {
                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment No"; cell.Merge = true; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment Date"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "PI No"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "PI Date"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    else if (nLCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                    {
                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment No"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Amendment Date"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "PI No"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "PI Date"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Product"; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Negotiation Bank"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Issue Bank"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Quantity"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      if (nLCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                    {
                    nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Unit Price"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      }

                    nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = "Value"; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    nStartRow = nRowIndex; nEndRow = nRowIndex;
                    #endregion

                    #region Report Data
                    int nCount = 0;
                    for (int i = 0; i < _oExportLCReports.Count; i++)
                    {
                        nCount++;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None;border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = _oExportLCReports[i].LCNo; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oExportLCReports[i].ContractorName; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = _oExportLCReports[i].MKTPersonName; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = _oExportLCReports[i].LCOpenDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nEndCol = 6;//reset count
                        if (nLCReportLevel == (int)EnumLCReportLevel.LCVersionLevel)
                        {
                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentNoWithExportLCID.Split('~')[0].ToString(); cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        else if (nLCReportLevel == (int)EnumLCReportLevel.PILevel)
                        {
                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentNoWithExportLCID.Split('~')[0].ToString(); cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].PINo; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].PIDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        else if (nLCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                        {
                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentNoWithExportLCID.Split('~')[0].ToString(); cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].AmendmentDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].PINo; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].PIDateSt; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].ProductName; cell.Merge = true; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].BankName_Nego; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].BankName_Issue; cell.Merge = true; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].Qty; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (nLCReportLevel == (int)EnumLCReportLevel.ProductLevel)
                        {
                            nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].UnitPrice; cell.Merge = true; cell.Style.Font.Bold = false;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        nEndCol++; cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = _oExportLCReports[i].Amount; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = ExcelBorderStyle.None; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                    }
                    #endregion
                    

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=LCReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
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
        private ExportLCReport GetExportLCReportTotal(List<ExportLCReport> oExportLCReports)
        {
            ExportLCReport oExportLCReport = new ExportLCReport();
            //oExportLCReport.IssueBankName = "Total : ";
            //oExportLCReport.LCValue = oExportLCReports.Select(o => o.LCValue).Sum();
            //oExportLCReport.DOValue = oExportLCReports.Select(o => o.DOValue).Sum();
            //oExportLCReport.ChallanValue = oExportLCReports.Select(o => o.ChallanValue).Sum();
            //oExportLCReport.InvoiceValue = oExportLCReports.Select(o => o.InvoiceValue).Sum();
            return oExportLCReport;
        }
     
        #region Pring Excel
        
        public void PrintExcel(FormCollection data)
        {
            List<ExportLCReport> oExportLCReports = new List<ExportLCReport>();
            ExportLCReport oExportLCReport = new ExportLCReport();
            oExportLCReport = JsonConvert.DeserializeObject<ExportLCReport>(data["ExportLCReport"]);

            if (oExportLCReport.PINo == "" && oExportLCReport.LCNo == "" && oExportLCReport.ErrorMessage == "")
            {
                _oExportLCReports = ExportLCReport.GetsReport(" and ExportLCID in (Select ExportLCID from ExportLC where ExportLC.HaveQuery=1 and BUID= " + oExportLCReport.BUID + ") ", 1, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sSQL = GetSQLForReport(oExportLCReport);
                _oExportLCReports = ExportLCReport.GetsReport(sSQL, 1, (int)Session[SessionInfo.currentUserID]);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "File No", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Customer Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Buyer Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Nego. Bank Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Issue Bank Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "A No", Width = 8f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Amendment Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Receive Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Value", Width = 10f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Gr.Total L/C Value", Width = 10f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Shipment Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Expiry Date", Width = 13f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Doc Amount", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Pending Doc", Width = 10f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Concern Person", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Tenor", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "UD Recd", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Draft", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Remarks", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "ACC(Buyer)", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "ACC(Bank)", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "UD", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Doc Send to Bank", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "C/P", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "LC Terms", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Status", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export LC Report");
                sheet.Name = "Export LC Report";

                #endregion

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 17;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Export LC Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                double total = 0.0;
                 int nExportLCID = 0, nRowSpan =0;

                _oExportLCReports=_oExportLCReports.OrderBy(x=>x.ExportLCID).ToList();

                foreach (var obj in _oExportLCReports)
                {
                    if (obj.ExportLCID != nExportLCID)
                    {
                        nRowSpan = _oExportLCReports.Where(x => x.ExportLCID == obj.ExportLCID).ToList().Count() - 1;
                        obj.CurrencySymbol = "$";
                        nStartCol = 2;
                        ExcelTool.Formatter = "";
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, (++nCount).ToString());
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.FileNo);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ContractorName);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.BuyerName);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.BankName_Nego);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.BankName_Issue);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.LCNo);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.LCOpenDateSt);
                    }
                    else
                    {
                        nStartCol = 10;
                    }
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.VersionNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.AmendmentDateSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCReceiveDateSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PIDateSt, false);
                    ExcelTool.Formatter = obj.CurrencySymbol +" #,##0.00";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Amount, 2).ToString(), true, false, ExcelHorizontalAlignment.Right,true);
                    if (obj.ExportLCID != nExportLCID)
                    {
                        ExcelTool.FillCellMerge(ref sheet, _oExportLCReports.Where(x => x.ExportLCID == obj.ExportLCID).Sum(x => x.Amount), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                    }
                    else
                    {
                        nStartCol = 17;
                    }


                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ShipmentDateSt.ToString(), false);

                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExpiryDateSt, false);
                    ExcelTool.Formatter = obj.CurrencySymbol + " #,##0.00";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Amount_Bill.ToString(), true, false, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToBill.ToString(), true, false, ExcelHorizontalAlignment.Right, true);

                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_DC.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MKTPersonName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCTermsName.ToString(), false);
                    ExcelTool.Formatter = "";
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerAccSt.ToString(), false);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BankAccSt.ToString(), false);
                 
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.UDRcvType.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.GetOriginalCopySt.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.NoteQuery.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCStatusSt.ToString(), false);
                    nExportLCID = obj.ExportLCID;
                    nRowIndex++;
                    total = total + obj.Amount;
                }
                #endregion
                //ExcelTool.FillCell(sheet, nRowIndex, 3, total.ToString(), false);

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = "Total :  "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                cell = sheet.Cells[nRowIndex, 12, nRowIndex, nEndCol-1]; cell.Merge = true;
                cell.Value = "  " + _oExportLCReports[0].CurrencySymbol + total; cell.Style.Font.Bold = true;
                cell.Value = "  " + _oExportLCReports[0].CurrencySymbol + total; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename= Export LC Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        //#endregion

        #region DU EXport YARN Report

        public ActionResult View_ExportProductReports(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oExportLCReports = new List<ExportLCReport>();
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oExportLCReports);
        }

        [HttpPost]
        public JsonResult GetsProductReport(ExportLCReport oExportLCReport)
        {
            _oExportLCReports = new List<ExportLCReport>();
            try
            {
                _oExportLCReports = ExportLCReport.GetsReportProduct(oExportLCReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportLCReport = new ExportLCReport();
                _oExportLCReport.ErrorMessage = ex.Message;
                _oExportLCReports.Add(_oExportLCReport);
            }

            List<ExportLCReport>  oExportLCReport_Dist = new List<ExportLCReport>();

            if (oExportLCReport.LCReportTypeInt==1)
                oExportLCReport_Dist=_oExportLCReports.GroupBy(x => x.ProductID).Select(g => g.First()).ToList();
            else
                oExportLCReport_Dist = _oExportLCReports.GroupBy(x => x.MKTEmpID).Select(g => g.First()).ToList();

            var tuple = new Tuple<List<ExportLCReport>, List<ExportLCReport>>(new List<ExportLCReport>(), new List<ExportLCReport>());
            tuple = new Tuple<List<ExportLCReport>, List<ExportLCReport>>(_oExportLCReports,oExportLCReport_Dist);
            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult PrintExportLCReportProduct(String sTemp)
        {
            List<ExportLCReport> oExportLCReportProductList = new List<ExportLCReport>();
            ExportLCReport oExportLCReportProduct = new ExportLCReport();

            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                oExportLCReportProduct.LCReportTypeInt = Convert.ToInt32(sTemp.Split('~')[0]);
                oExportLCReportProduct.LCReportLevelInt = Convert.ToInt32(sTemp.Split('~')[1]);
                oExportLCReportProduct.DateYear = Convert.ToDouble(sTemp.Split('~')[2]);
                oExportLCReportProduct.BUID = Convert.ToInt32(sTemp.Split('~')[3]);
                oExportLCReportProductList = ExportLCReport.GetsReportProduct(oExportLCReportProduct, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oExportLCReportProduct.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sHeader="Year Wise LC Receive Report";
            if(oExportLCReportProduct.LCReportLevelInt==1)
                    sHeader="Month Wise LC Receive Report";

            rptExportLCReportProduct oReport = new rptExportLCReportProduct();
            byte[] abytes = oReport.PrepareReport(oExportLCReportProductList, oExportLCReportProduct, oCompany, oBusinessUnit, sHeader);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Export L/C Report
        public ActionResult ViewExportLCReports(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oExportLCReports = new List<ExportLCReport>();
            if (buid>0)
            {
                _oExportLCReports = ExportLCReport.GetsReport(" and ExportLCID in (Select ExportLCID from ExportLC where ExportLC.HaveQuery=1 and BUID= " + buid + ") ", 1, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oExportLCReports = ExportLCReport.GetsReport(" and ExportLCID in (Select ExportLCID from ExportLC where ExportLC.HaveQuery=1 ) ", 1, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.LCReportLevelObjs = EnumObject.jGets(typeof(EnumLCReportLevel));//LCReportLevelObj.Gets();
            ViewBag.NegoBanks = BankBranch.GetsOwnBranchs((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.AdviceBanks = BankBranch.GetsOwnBranchs((int)Session[SessionInfo.currentUserID]);
            ViewBag.ExportLCStatus = EnumObject.jGets(typeof(EnumExportLCStatus)).Where(o => o.id != 0).ToList();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType)).Where(x => x.id != 0);
            return View(_oExportLCReports);
        }
        #region Searching For Reporting
        [HttpPost]
        public JsonResult AdvanceSearchForReport(ExportLCReport oExportLCReport)
        {
            _oExportLCReports = new List<ExportLCReport>();
            try
            {
                if (oExportLCReport.PINo == "" && oExportLCReport.LCNo == "" && oExportLCReport.ErrorMessage == "")
                {
                    _oExportLCReports = ExportLCReport.GetsReport(" and ExportLCID in (Select ExportLCID from ExportLC where ExportLC.HaveQuery=1 and BUID= " + oExportLCReport.BUID + ") ", 1, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = GetSQLForReport(oExportLCReport);
                    _oExportLCReports = ExportLCReport.GetsReport(sSQL, 1, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oExportLCReport = new ExportLCReport();
                _oExportLCReport.ErrorMessage = ex.Message;
                _oExportLCReports.Add(_oExportLCReport);
            }
            var jsonResult = Json(_oExportLCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQLForReport(ExportLCReport oExportLCReport)
        {
            //bool bIsLCInHand = false;

            string sString = " ";
            if (!string.IsNullOrEmpty(oExportLCReport.PINo))
            {
                sString = sString + " AND PINo like ''%" + oExportLCReport.PINo + "%''";
            }
            if (!string.IsNullOrEmpty(oExportLCReport.LCNo))
            {
                sString = sString + " AND ExportLCID IN (SELECT ExportLCID FROM ExportLC Where ExportLCNo like ''%" + oExportLCReport.LCNo + "%'')";
            }

            if (oExportLCReport.BUID > 0)//Selected BUID
            {
                sString = sString + " AND BUID = " + oExportLCReport.BUID + " ";
            }
            //else if (oExportLCReport.BusinessUnitID > 0) //Default BUID
            //{
            //    sString = sString + " AND BUID = " + oExportLCReport.BusinessUnitID + " ";
            //}

            if (!string.IsNullOrEmpty(oExportLCReport.ErrorMessage))
            {
                int nCount = 0;
                int ncboLCOpenDate = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromLCOpenDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToLCOpenDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                int ncboLCReceiveDate = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromLCReceiveDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToLCReceiveDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                int ncboLCReceiveDateReport = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromLCReceiveDateReport = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToLCReceiveDateReport = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                int ncboAmendmentDate = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromAmendmentDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToAmendmentDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                string sContractorId = oExportLCReport.ErrorMessage.Split('~')[++nCount];
                string sBuyerId = oExportLCReport.ErrorMessage.Split('~')[++nCount];

                int ncboShipmentDate = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromShipmentDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToShipmentDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                int ncboExpireDate = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dFromExpireDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                DateTime dToExpireDate = Convert.ToDateTime(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                int nLCType = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);
                int nStatus = Convert.ToInt32(oExportLCReport.ErrorMessage.Split('~')[++nCount]);

                #region Open Date
                if (ncboLCOpenDate != (int)EnumCompareOperator.None)
                {
                    if (ncboLCOpenDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboLCOpenDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboLCOpenDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboLCOpenDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboLCOpenDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboLCOpenDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCOpenDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCOpenDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                }
                #endregion

                #region Receive Date
                if (ncboLCReceiveDate != (int)EnumCompareOperator.None)
                {
                    if (ncboLCReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCReceiveDate.ToString("dd MMM yyyy") + "'', 106))";
                    }
                }
                #endregion
                #region Receive Date(Report)
                if (ncboLCReceiveDateReport != (int)EnumCompareOperator.None)
                {
                    if (ncboLCReceiveDateReport == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDateReport == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDateReport == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDateReport == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDateReport == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                    else if (ncboLCReceiveDateReport == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),ReportDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToLCReceiveDateReport.ToString("dd MMM yyyy") + "'', 106))";
                    }
                }
                #endregion

                #region Amendment Date
                if (ncboAmendmentDate != (int)EnumCompareOperator.None)
                {
                    sString = sString + " AND ";

                    if (ncboAmendmentDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sString = sString + "  CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106)) ";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sString = sString + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106)) )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sString = sString + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sString = sString + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.Between)
                    {
                        sString = sString + " CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToAmendmentDate.ToString("dd MMM yyyy") + "'',106))";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sString = sString + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),''" + dFromAmendmentDate.ToString("dd MMM yyyy") + "'',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),''" + dToAmendmentDate.ToString("dd MMM yyyy") + "'',106))  )";
                    }
                }
                #endregion

                #region Contractor
                if (!string.IsNullOrEmpty(sContractorId))
                {
                    sString = sString + " AND ExportLCID in (Select ExportLCID from ExportLC where ApplicantID IN (" + sContractorId + "))";
                }
                #endregion

                #region Buyer
                if (!string.IsNullOrEmpty(sBuyerId))
                {
                    sString = sString + " AND BuyerID IN (" + sBuyerId + ")";
                }
                #endregion

                #region Shipment Date
                if (ncboShipmentDate != (int)EnumCompareOperator.None)
                {
                    if (ncboShipmentDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboShipmentDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboShipmentDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboShipmentDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboShipmentDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboShipmentDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromShipmentDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToShipmentDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                }
                #endregion

                #region Expire Date
                if (ncboExpireDate != (int)EnumCompareOperator.None)
                {
                    if (ncboExpireDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboExpireDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboExpireDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboExpireDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboExpireDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                    else if (ncboExpireDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sString);
                        sString = sString + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dFromExpireDate.ToString("dd MMM yyyy") + "'', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dToExpireDate.ToString("dd MMM yyyy") + "'', 106)))";
                    }
                }
                #endregion

                #region LC Type
                if (nLCType > 0)
                {
                    sString = sString + " AND ExportLCID in (SELECT ExportLCID from ExportLC WHERE ExportLCType =" + nLCType + ")";
                }
                #endregion

                #region Status
                if (nStatus > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oExportLCReport.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (nStatus == 1) //LC in Hand
                    {
                        sString = sString + " AND ExportPIID in (  Select ExportPIID from (  Select   ExportPIID  , Qty  ,isnull( (Select SUM(Qty) from ExportBillDetail where ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID=exportPI.exportPIID)),0) as Qty_Bill  from exportPI where ExportPI.PIStatus in (2,3,4,5) and isnull(ExportPI.LCID,0)>0) as d where (d.Qty-isnull(d.Qty_Bill,0))>0.9)";     
                    }
                    else if (nStatus == 2)//Challan Create but Bill not Create
                    {
                        if (oBU.BusinessUnitType == EnumBusinessUnitType.Finishing || oBU.BusinessUnitType == EnumBusinessUnitType.Weaving)
                        {
                            sString = sString + " AND ExportPIID IN ( Select ExportPIID from (Select ExportPIID,Qty,isnull((Select SUM(Qty) from FabricDeliveryChallanDetail as FDCD where FDCD.FDODID in (Select FDODID from FabricDeliveryOrderDetail where ExportPIID=TT.ExportPIID)),0)  as QtyDC,isnull((Select SUM(Qty) from ExportBillDetail as EBill where EBill.ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID=TT.ExportPIID)),0) as QtyBill from ExportPI as TT  where BUID=" + oExportLCReport.BUID + " ) as HH where HH.QtyDC>0 and HH.QtyDC+5>HH.QtyBill) ";
                        }
                        else if (oBU.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                        {
                            sString = sString + " AND ExportPIID IN (Select ExportPIID from (Select ExportPIID,Qty,isnull((Select SUM(Qty) from DUDeliveryChallanDetail as DUDCD where DUDCD.DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderDetail.DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ExportPIID=TT.ExportPIID))),0) as QtyDC,isnull((Select SUM(Qty) from ExportBillDetail as EBill where EBill.ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID=TT.ExportPIID)),0) as QtyBill from ExportPI as TT where BUID=" + oExportLCReport.BUID + ") as HH where HH.QtyDC>0 and HH.QtyDC+5>HH.QtyBill)";
                        }
                    }
                    else if (nStatus == 3)//pending DO
                    {
                     
                        if (oBU.BusinessUnitType == EnumBusinessUnitType.Finishing || oBU.BusinessUnitType == EnumBusinessUnitType.Weaving)
                        {
                            sString = sString + " AND ExportPIID IN (Select ExportPIID from (Select Qty, ExportPIID, isnull((select SUM(Qty) from FabricDeliveryOrderDetail where ExportPIID=TT.ExportPIID) ,0) as Qty_DC from ExportPI as TT  where BUID=" + oExportLCReport.BUID + ") as HH where HH.Qty>hh.Qty_DC) ";
                        }
                        else if (oBU.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                        {
                            sString = sString + " AND ExportPIID IN (Select ExportPIID from (Select Qty, ExportPIID, isnull((select SUM(Qty) from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ExportPIID=TT.ExportPIID))),0) as Qty_DO from ExportPI as TT  where BUID=" + oExportLCReport.BUID + ") as HH where HH.Qty>hh.Qty_DO+0.9)";
                        }
                    }
                    else if (nStatus == 4)//pending Challan
                    {

                        if (oBU.BusinessUnitType == EnumBusinessUnitType.Finishing || oBU.BusinessUnitType == EnumBusinessUnitType.Weaving)
                        {
                            sString = sString + " AND ExportPIID IN (Select ExportPIID from (Select Qty, ExportPIID,isnull((select SUM(Qty) from FabricDeliveryChallanDetail as FDCD where FDCD.FDODID in (Select FDODID from FabricDeliveryOrderDetail where ExportPIID=TT.ExportPIID)) ,0) as Qty_DC from ExportPI as TT  where BUID=" + oExportLCReport.BUID + ") as HH where HH.Qty>hh.Qty_DC) ";
                        }
                        else if (oBU.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                        {
                            sString = sString + " AND ExportPIID IN (Select ExportPIID from (Select Qty, ExportPIID,isnull((select SUM(Qty) from DUDeliveryChallanDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ExportPIID=TT.ExportPIID))),0) as Qty_DC from ExportPI as TT  where BUID=" + oExportLCReport.BUID + ") as HH where HH.Qty>hh.Qty_DC)";
                        }
                    }
                    
                    else if (nStatus == 5) //pending UD
                    {
                        sString = sString + " AND ExportLCID NOT IN (SELECT EUD.ExportLCID FROM ExportUD AS EUD) ";
                    }
                    else if (nStatus == 6) //pending UP
                    {
                        sString = sString + " AND ExportLCID NOT IN (SELECT ZZ.ExportLCID FROM ExportUD AS ZZ WHERE ZZ.ExportUDID IN (SELECT XX.ExportUDID FROM ExportUPDetail AS XX)) ";
                    }

                }
                #endregion

                
            }
            return sString;
        }
        #endregion
        #endregion
    }
}