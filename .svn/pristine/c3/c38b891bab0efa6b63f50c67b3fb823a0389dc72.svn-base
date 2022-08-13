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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportPIRWUController : Controller
    {
        #region Declaration
        ExportPIRWU _oExportPIRWU = new ExportPIRWU();
        List<ExportPIRWU> _oExportPIRWUs = new List<ExportPIRWU>(); 
        DUStepWiseSetup _oDUStepWiseSetup = new DUStepWiseSetup();
        #endregion

        public ActionResult View_ExportPIReport_WU(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oExportPIRWU = new ExportPIRWU();
            _oExportPIRWUs = new List<ExportPIRWU>();
            //_oExportPIRWUs = ExportPIRWU.Gets("Select * From View_ExportPIReport_WU where BUID = '" + buid + "'", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            if(buid > 0){
                ViewBag.Units = BusinessUnit.Gets("SELECT * FROM BusinessUnit WHERE BusinessUnitID = " + buid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                ViewBag.Units = BusinessUnit.Gets("SELECT * FROM BusinessUnit WHERE BusinessUnitType IN (5,6) ", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PIStatus = EnumObject.jGets(typeof(EnumPIStatus)).Where(x => x.id != (int)EnumPIStatus.Initialized); 
            return View(_oExportPIRWUs);
        }
        #region Search
        public JsonResult Search(ExportPIRWU oExportPIRWU)
        {
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            try
            {
                string sSql = GetSQL(oExportPIRWU);
                oExportPIRWUs = ExportPIRWU.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportPIRWU.ErrorMessage = ex.Message;
                oExportPIRWUs.Add(oExportPIRWU);
            }
            var jSonResult = Json(oExportPIRWUs, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        private string GetSQL(ExportPIRWU oExportPIRWU)
        {
            string string1 = "SELECT * FROM View_ExportPIReport_WU";
            string string2 = "";
            if (!string.IsNullOrEmpty(oExportPIRWU.PINo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " PINo LIKE '%" + oExportPIRWU.PINo + "%'";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.MKTPName))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " FabricNo LIKE '%" + oExportPIRWU.MKTPName + "%'";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.PONo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " FSCNo LIKE '%" + oExportPIRWU.PONo + "%'";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.ExeNo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ExeNo LIKE '%" + oExportPIRWU.ExeNo + "%'";
            }
            Global.TagSQL(ref string2);
            string2 = string2 + " BUID = '" + oExportPIRWU.BUID + "'";
            return string1 + string2;

        }
        public JsonResult AdvanceSearch(ExportPIRWU oExportPIRWU)
        {
            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            try
            {
                string sSql = GetSQLAdvanceSearch(oExportPIRWU);
                oExportPIRWUs = ExportPIRWU.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportPIRWU.ErrorMessage = ex.Message;
                oExportPIRWUs.Add(oExportPIRWU);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIRWUs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveDispoNo(ExportPIRWU oExportPIRWU)
        {
            ExportPIRWU objExportPIRWU = new ExportPIRWU();
            try
            {
                objExportPIRWU = oExportPIRWU.RemoveDispoNo(oExportPIRWU, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                objExportPIRWU.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objExportPIRWU);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        private string GetSQLAdvanceSearch(ExportPIRWU oExportPIRWU)
        {
            string string1 = "SELECT * FROM View_ExportPIReport_WU"; 
            string string2 = "";
            int nCount = 5;
            oExportPIRWU.MKTPName = oExportPIRWU.ErrorMessage.Split('~')[2].Trim();
            int nDateCriteria_Issue = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            DateTime dStart_Issue = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]),
                    dEnd_Issue = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);

            int nDateCriteria_Validity = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            DateTime dStart_Validity = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]),
                    dEnd_Validity = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);

            string sMktAccountIDs = oExportPIRWU.ErrorMessage.Split('~')[2];
            string sMktPersonIDs = oExportPIRWU.ErrorMessage.Split('~')[3];

            nCount = 12;
            int nBUID = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            int nStatus = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            int nProcessType = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            string sStyleNo = oExportPIRWU.ErrorMessage.Split('~')[nCount++];
            string sLCNo = oExportPIRWU.ErrorMessage.Split('~')[nCount++];
            int nBank = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            int nPIStatus = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            int nPaymentStatus = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);

            int nDateCriteria_LC = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            DateTime dStart_LC = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]),
                    dEnd_LC = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);

            int nDateCriteria_Received = Convert.ToInt32(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            DateTime dStart_Received = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]),
                    dEnd_Received = Convert.ToDateTime(oExportPIRWU.ErrorMessage.Split('~')[nCount++]);
            
            DateObject.CompareDateQuery(ref string2, "IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);

            if (!string.IsNullOrEmpty(oExportPIRWU.ContractorName))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ContractorID IN (" + oExportPIRWU.ContractorName + ")";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.BuyerName))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " BuyerID IN (" + oExportPIRWU.BuyerName + ")";
            }
            if (!string.IsNullOrEmpty(sMktAccountIDs))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " MKTEmpID IN (SELECT MarketingAccountID AS MKTEmpID FROM View_MarketingAccount WHERE MarketingAccountID IN (SELECT MarketingAccountID FROM View_MarketingAccount WHERE GroupID IN (" + sMktAccountIDs + ")))";
            }
            if (!string.IsNullOrEmpty(sMktPersonIDs))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " MKTEmpID IN (" + sMktPersonIDs + ") ";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.Construction))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " Construction LIKE '%" + oExportPIRWU.Construction + "%'";
            }
            if (oExportPIRWU.OrderSheetDetailID == 1)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " OrderSheetDetailID = 0";
            }
            if (oExportPIRWU.OrderSheetDetailID == 2)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " OrderSheetDetailID > 0";
            }

            if (nBUID > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " BUID = " + nBUID ;
            }
            if (nStatus > 0)
            {
                if (nStatus == 1)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ISNULL(ExportLCID,0) > 0 ";
                }
                else if (nStatus == 2)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ISNULL(ExportLCID,0) <= 0 ";
                }
            }
            
            if (nProcessType > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(FabricWeave,0) = " + nProcessType;
            }
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " StyleNo LIKE '%" + sStyleNo + "%' ";
            }
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ExportLCNo LIKE '%" + sLCNo + "%' ";
            }
            if (nBank > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(BankBranchID,0) = " + nBank;
            }
            if (nPIStatus > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(PIStatus,0) = " + nPIStatus;
            }
            if (nPaymentStatus > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(PaymentType,0) = " + nPaymentStatus;
            }
            DateObject.CompareDateQuery(ref string2, "ExportLCDate", nDateCriteria_LC, dStart_LC, dEnd_LC);
            DateObject.CompareDateQuery(ref string2, "ValidityDate", nDateCriteria_Validity, dStart_Validity, dEnd_Validity);
            //DateObject.CompareDateQuery(ref string2, "ExportLCDate", nDateCriteria_Received, dStart_Received, dEnd_Received);

            #region Received Date
            if (nDateCriteria_Received != (int)EnumCompareOperator.None)
            {
                if (nDateCriteria_Received == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
            }
            #endregion

            return string1 + string2 + " order by ExportPIID";
        }
        #endregion

        #region Print
        public JsonResult PrintExel(string tempString)
        {
            string string1 = "SELECT * FROM View_ExportPIReport_WU";
            string string2 = "";

            ExportPIRWU oExportPIRWU = new ExportPIRWU();
            int nCount = 0;
            oExportPIRWU.ContractorName = tempString.Split('~')[nCount++].Trim();
            oExportPIRWU.BuyerName = tempString.Split('~')[nCount++].Trim();
            string sMktAccountIDs = tempString.Split('~')[nCount++].Trim();
            string sMktPersonIDs = tempString.Split('~')[nCount++].Trim();
            oExportPIRWU.Construction = tempString.Split('~')[nCount++].Trim();
            
            int nDateCriteria_Issue = Convert.ToInt32(tempString.Split('~')[nCount++]);
            DateTime dStart_Issue = Convert.ToDateTime(tempString.Split('~')[nCount++]);
            DateTime dEnd_Issue = Convert.ToDateTime(tempString.Split('~')[nCount++]);

            int nDateCriteria_Validity = Convert.ToInt32(tempString.Split('~')[nCount++]);
            DateTime dStart_Validity = Convert.ToDateTime(tempString.Split('~')[nCount++]),
                    dEnd_Validity = Convert.ToDateTime(tempString.Split('~')[nCount++]);

            oExportPIRWU.OrderSheetDetailID = Convert.ToInt32(tempString.Split('~')[nCount++]);
            int nBUID = Convert.ToInt32(tempString.Split('~')[nCount++]);
            int nStatus = Convert.ToInt32(tempString.Split('~')[nCount++]);
            int nProcessType = Convert.ToInt32(tempString.Split('~')[nCount++]);
            string sStyleNo = tempString.Split('~')[nCount++];
            string sLCNo = tempString.Split('~')[nCount++];
            int nBank = Convert.ToInt32(tempString.Split('~')[nCount++]);
            int nPIStatus = Convert.ToInt32(tempString.Split('~')[nCount++]);
            int nPaymentStatus = Convert.ToInt32(tempString.Split('~')[nCount++]);

            int nDateCriteria_LC = Convert.ToInt32(tempString.Split('~')[nCount++]);
            DateTime dStart_LC = Convert.ToDateTime(tempString.Split('~')[nCount++]),
                    dEnd_LC = Convert.ToDateTime(tempString.Split('~')[nCount++]);

            int nDateCriteria_Received = Convert.ToInt32(tempString.Split('~')[nCount++]);
            DateTime dStart_Received = Convert.ToDateTime(tempString.Split('~')[nCount++]),
                    dEnd_Received = Convert.ToDateTime(tempString.Split('~')[nCount++]);            
            
            DateObject.CompareDateQuery(ref string2, "IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            if (!string.IsNullOrEmpty(oExportPIRWU.ContractorName))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ContractorID IN (" + oExportPIRWU.ContractorName + ")";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.BuyerName))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " BuyerID IN (" + oExportPIRWU.BuyerName + ")";
            }
            if (!string.IsNullOrEmpty(sMktAccountIDs))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " MKTEmpID IN (SELECT MarketingAccountID AS MKTEmpID FROM View_MarketingAccount WHERE GroupID IN (" + sMktAccountIDs + "))";
            }
            if (!string.IsNullOrEmpty(sMktPersonIDs))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " MKTEmpID IN (" + sMktPersonIDs + ") ";
            }
            if (!string.IsNullOrEmpty(oExportPIRWU.Construction))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " Construction LIKE '%" + oExportPIRWU.Construction + "%'";
            }
            if (oExportPIRWU.OrderSheetDetailID == 1)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " OrderSheetDetailID = 0";
            }
            if (oExportPIRWU.OrderSheetDetailID == 2)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " OrderSheetDetailID > 0";
            }

            if (nBUID > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " BUID = " + nBUID;
            }
            if (nStatus > 0)
            {
                if (nStatus == 1)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ISNULL(ExportLCID,0) > 0 ";
                }
                else if (nStatus == 2)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ISNULL(ExportLCID,0) <= 0 ";
                }
            }

            if (nProcessType > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(FabricWeave,0) = " + nProcessType;
            }
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " StyleNo LIKE '%" + sStyleNo + "%' ";
            }
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ExportLCNo LIKE '%" + sLCNo + "%' ";
            }
            if (nBank > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(BankBranchID,0) = " + nBank;
            }
            if (nPIStatus > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(PIStatus,0) = " + nPIStatus;
            }
            if (nPaymentStatus > 0)
            {
                Global.TagSQL(ref string2);
                string2 = string2 + " ISNULL(PaymentType,0) = " + nPaymentStatus;
            }
            DateObject.CompareDateQuery(ref string2, "ExportLCDate", nDateCriteria_LC, dStart_LC, dEnd_LC);
            DateObject.CompareDateQuery(ref string2, "ValidityDate", nDateCriteria_Validity, dStart_Validity, dEnd_Validity);

            #region Received Date
            if (nDateCriteria_Received != (int)EnumCompareOperator.None)
            {
                if (nDateCriteria_Received == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nDateCriteria_Received == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref string2);
                    string2 = string2 + " ExportPIID IN (SELECT ELCM.ExportPIID FROM ExportPILCMapping AS ELCM WHERE ELCM.Activity > 0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),ELCM.LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_Received.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_Received.ToString("dd MMM yyyy") + "', 106)))";
                }
            }
            #endregion

            List<ExportPIRWU> oExportPIRWUs = new List<ExportPIRWU>();
            //oExportPIRWU.ErrorMessage = tempString;
            //string sSql = GetSQLAdvanceSearch(oExportPIRWU);
            try
            {
                string sSql = string1 + string2 + " order by ExportPIID ";
                oExportPIRWUs = ExportPIRWU.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportPIRWU.ErrorMessage = ex.Message;
                oExportPIRWUs.Add(oExportPIRWU);
            }
            if (oExportPIRWUs.Count > 0)
            {
                ExelReport(oExportPIRWUs, oExportPIRWU.BUID);
            }
            return Json(string1+string2);
        }
        public void ExelReport(List<ExportPIRWU> oExportPIRWUs, int BUID)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 5f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Issue Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Contractor Name", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Buyer Name", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Marketing Person", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Compossion", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Mkt Ref", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Construction", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Fabric Type", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Weave", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Color", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Style No", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Bank", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Buyer Ref", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Finish Type", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Unit Price", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "M. Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Amount", Width = 10f, IsRotate = false, Align = TextAlign.Center });

            //PI Value	LC NO	LC Date	LC Shipment Date	LC Expiry Date	PI Status	PP Submission Date	Delivery Start Date	Delivery complete Date

            table_header.Add(new TableHeader { Header = "PI Value", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC NO", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC Shipment Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC Expiry Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI Status", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PP Submission Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Del. Start Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Del. complete Date", Width = 20f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "Qty DO", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Qty DC", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Yet To Do", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Yet To Delivery", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Delay", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PI Report");
                sheet.Name = "PI Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 36;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "PI Report"; cell.Style.Font.Bold = true;
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
                int nCount = 0; nEndCol = table_header.Count() + nStartCol; int nExportPIID = 0, nRowSpan =0;
                string sCurrencySymbol = "", sMUName ="";
                foreach (var obj in oExportPIRWUs)
                {
                    if (obj.ExportPIID != nExportPIID)
                    {
                        nStartCol = 2;
                        nRowSpan = oExportPIRWUs.Where(x => x.ExportPIID == obj.ExportPIID).ToList().Count() - 1;
                        //nRowSpan = (nRowSpan<)

                        ExcelTool.Formatter = "";
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, (++nCount).ToString());
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.PINo);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.IssueDateSt);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ContractorName);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.BuyerName);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.MKTPName);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo, false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.IssueDateSt, false);
                        //ExcelTool.Formatter = "";// #,##0;(#,##0)";
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ContractorName, false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName.ToString(), false);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MKTPName.ToString(), false);
                    }
                    else
                    {
                        nStartCol = 8;
                    }

            
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PONo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SCDateSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExeNo, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Construction == null ? "" : obj.Construction.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricTypeName == null ? "" : obj.FabricTypeName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricWeave == null ? "" : obj.FabricWeave.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ColorInfo == null ? "" : obj.ColorInfo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo == null ? "" : obj.StyleNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BankName == null ? "" : obj.BankName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerRef == null ? "" : obj.BuyerRef.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FinishTypeName == null ? "" : obj.FinishTypeName.ToString(), false);

                    ExcelTool.Formatter = obj.Currency + "##,##,##0.00;(##,##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);

                    ExcelTool.Formatter = "##,##,##0.00;(##,##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty, 2).ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUName.ToString(), false);

                    ExcelTool.Formatter = obj.Currency + "##,##,##0.00;(##,##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (obj.Qty * obj.UnitPrice).ToString(), true);

                    if (obj.ExportPIID != nExportPIID)
                    {
                        var oPIRWU = oExportPIRWUs.Where(x => x.ExportPIID == obj.ExportPIID ).ToList();
                        nRowSpan = oPIRWU.Count() - 1;


                        //PI Value	LC NO	LC Date	LC Shipment Date	LC Expiry Date	PI Status	PP Submission Date	Delivery Start Date	Delivery complete Date

                        ExcelTool.FillCellMerge(ref sheet, oPIRWU.Sum(x => x.Qty * x.UnitPrice), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ExportLCNo);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ExportLCDateSt);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ShipmentDateSt);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.ExpiryDateSt);
                        ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.PIStatusSt);
                        //ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, "");
                        //ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.DeliveryStartDateSt);
                        //ExcelTool.FillCellMerge(ref sheet, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, obj.DeliveryCompleteDateSt);
                        
                        nExportPIID = obj.ExportPIID;
                    }
                    else
                    {
                        nStartCol = 31;
                    }

                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DeliveryStartDateSt, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DeliveryCompleteDateSt, false);

                    ExcelTool.Formatter = "##,##,##0.00;(##,##,##0.00)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_DO.ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_DC.ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToDo.ToString(), true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.YetToDelivery.ToString(), true);

                    int nDays=0;
                    if (obj.ExportLCDate != DateTime.MinValue)
                        nDays = (int)(obj.IssueDate - obj.ExportLCDate).TotalDays;
                    else
                        nDays = (int)(obj.IssueDate - DateTime.Now).TotalDays;                  
                    if (nDays < 0) nDays *= -1;
                    ExcelTool.Formatter = "##,##,###;(##,##,###)";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nDays.ToString(), true);

                    nRowIndex++;
                    sCurrencySymbol = obj.Currency;
                }

                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 19, true, ExcelHorizontalAlignment.Right); nStartCol++;
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.Qty).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);


                ExcelTool.Formatter = sCurrencySymbol + "##,##,##0.00;(##,##,##0.00)";
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => (x.Qty * x.UnitPrice)).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => (x.Qty * x.UnitPrice)).ToString(), true);
                //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.Qty).ToString(), true);

                ExcelTool.FillCell(sheet, nRowIndex,  nStartCol++, "", false);
                ExcelTool.FillCell( sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);

                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);

                ExcelTool.Formatter = "##,##,##0.00;(##,##,##0.00)";
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.Qty_DO).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.Qty_DC).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.YetToDo).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oExportPIRWUs.Sum(x => x.YetToDelivery).ToString(), true);
                ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, "", false);
                #endregion

                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PI_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Get Fabric Pricing Info In PI
        [HttpPost]
        public JsonResult GetsPIDetailForShowPricing(ExportPIRWU oExportPIRWU)
        {
            Tuple<List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>> tuple = new Tuple<List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>>(new List<ExportPIRWU>(), new List<CellRowSpan>(), new List<ExportPIRWU>(), new List<CellRowSpan>(), new List<ExportPIRWU>());
            List<ExportPIRWU> oExportPIRWUDetails = new List<ExportPIRWU>();
            try
            {
                string sFabricIDs = oExportPIRWU.FabricID.ToString();
                string sConstruction = oExportPIRWU.Construction;


                if (!string.IsNullOrEmpty(sFabricIDs) || oExportPIRWU.ExportPIDetailID > 0 || !string.IsNullOrEmpty(sConstruction))
                {
                    string sSQL = "Select * from View_ExportPIReport_WU Where ISNULL(FabricID,0)<>0";
                    if (!string.IsNullOrEmpty(sFabricIDs))
                        sSQL += " And FabricID IN (" + sFabricIDs + ")";

                    if (!string.IsNullOrEmpty(sConstruction))
                        sSQL += " And  Construction Like '%" + sConstruction.Trim() + "%'";

                    if (oExportPIRWU.ExportPIDetailID > 0)
                        sSQL += " And FabricID IN (Select FabricID from View_ExportPIReport Where ExportPIDetailID=" + oExportPIRWU.ExportPIDetailID + ")";


                    sSQL += " Order By FabricID";
                    oExportPIRWUDetails = ExportPIRWU.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oExportPIRWUDetails.Any() && oExportPIRWUDetails.First().ExportPIDetailID > 0)
                    {

                        #region Find Avg Unit Price For Each Fabric
                        var oFabicAvgUnitPrices = oExportPIRWUDetails.GroupBy(x => x.FabricID).Select(grp => new ExportPIRWU
                        {
                            FabricID = grp.Key,
                            UnitPrice_Avg = Math.Round(grp.Sum(p => p.Qty * p.UnitPrice) / grp.Sum(p => p.Qty), 6),
                        }).ToList();

                        var oTExportPIRWUDs = new List<ExportPIRWU>();
                        oExportPIRWUDetails.ForEach(x =>
                        {
                            x.UnitPrice_Avg = oFabicAvgUnitPrices.Where(m => m.FabricID == x.FabricID).First().UnitPrice_Avg;
                            oTExportPIRWUDs.Add(x);
                        });
                        var oCellRowSpansFabric = GenerateSpan(oTExportPIRWUDs, true, false);

                        #endregion

                        #region Find Avg Unit Price For Construction

                        var oExportPIRWUDetailConstructions = oExportPIRWUDetails.OrderBy(x => x.Construction).ToList();
                        oFabicAvgUnitPrices = oExportPIRWUDetailConstructions.GroupBy(x => x.Construction).Select(grp => new ExportPIRWU
                        {
                            Construction = grp.Key,
                            UnitPrice_Avg = Math.Round(grp.Sum(p => p.Qty * p.UnitPrice) / grp.Sum(p => p.Qty), 6),
                        }).ToList();

                        oTExportPIRWUDs = new List<ExportPIRWU>();
                        oExportPIRWUDetailConstructions.ForEach(x =>
                        {
                            x.UnitPrice_Avg = oFabicAvgUnitPrices.Where(m => m.Construction == x.Construction).First().UnitPrice_Avg;
                            oTExportPIRWUDs.Add(x);
                        });
                        var oCellRowSpansConstruction = GenerateSpan(oTExportPIRWUDs, false, false);

                        #endregion

                        #region Construction, Unit Price Group
                        var oConstructionGroups = oExportPIRWUDetails.GroupBy(x => new { x.Construction, x.UnitPrice }).Select(grp => new ExportPIRWU
                        {
                            Construction = grp.Key.Construction,
                            UnitPrice = grp.Key.UnitPrice,
                            Qty = grp.Count()
                        }).OrderBy(x => x.Construction).ToList();

                        #endregion
                        tuple = new Tuple<List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>, List<CellRowSpan>, List<ExportPIRWU>>(oExportPIRWUDetails, oCellRowSpansFabric, oExportPIRWUDetailConstructions, oCellRowSpansConstruction, oConstructionGroups);
                    }
                }
            }
            catch (Exception ex)
            {
                oExportPIRWUDetails = new List<ExportPIRWU>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(tuple);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<CellRowSpan> GenerateSpan(List<ExportPIRWU> oExportPIRWUDs, bool IsFabric, bool IsConstructionGrp)
        {
            var oTExportPIRWUDs = new List<ExportPIRWU>();
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[,] mergerCell2D = new int[1, 2];
            int[] rowIndex = new int[15];
            int[] rowSpan = new int[15];

            while (oExportPIRWUDs.Count() > 0)
            {
                if (IsFabric)
                {
                    oTExportPIRWUDs = oExportPIRWUDs.Where(x => x.FabricID == oExportPIRWUDs.FirstOrDefault().FabricID).ToList();
                    oExportPIRWUDs.RemoveAll(x => x.FabricID == oTExportPIRWUDs.FirstOrDefault().FabricID);
                }
                else
                {
                    oTExportPIRWUDs = oExportPIRWUDs.Where(x => x.Construction == oExportPIRWUDs.FirstOrDefault().Construction).ToList();
                    oExportPIRWUDs.RemoveAll(x => x.Construction == oTExportPIRWUDs.FirstOrDefault().Construction);
                }
                rowIndex[0] = rowIndex[0] + rowSpan[0]; 
                rowSpan[0] = oTExportPIRWUDs.Count(); 
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("Span", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        [HttpPost]
        public JsonResult PO_Assign(ExportPIRWU oExportPIRWU)
        {
            oExportPIRWU.RemoveNulls();
            List<FabricSCReport> oFabricSCReports = new List<FabricSCReport>();
            try
            {
                //string sSQL = "select * from View_FabricSalesContractReport WHERE FabricID=" + oExportPIRWU.FabricID+ " and FabricSalesContractDetailID not in (Select OrderSheetDetailID from ExportPIDetail where isnull(OrderSheetDetailID,0)>0)";
                string sSQL = "select * from View_FabricSalesContractReport WHERE FabricID=" + oExportPIRWU.FabricID + " and isnull(Qty_PI,0)<Qty and OrderType in (" + (int)EnumFabricRequestType.Buffer + ","  +(int)EnumFabricRequestType.Bulk + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.SampleFOC + ")";
                oFabricSCReports = FabricSCReport.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportPIRWU = new ExportPIRWU();
                _oExportPIRWU.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult POAssign(ExportPIRWU oExportPIRWU)
        {
            _oExportPIRWU = new ExportPIRWU();
            try
            {
                _oExportPIRWU = _oExportPIRWU.SetPO(oExportPIRWU, (int)Session[SessionInfo.currentUserID]);
                //_oExportPIRWU = _oExportPIRWU.Get(oExportPIRWU.ExportPIDetailID, (int)Session[SessionInfo.currentUserID]);
            }
            catch(Exception ex)
            {
                _oExportPIRWU.ErrorMessage = ex.Message;
            }
            return Json(_oExportPIRWU);
        }

        #endregion
    }
}
