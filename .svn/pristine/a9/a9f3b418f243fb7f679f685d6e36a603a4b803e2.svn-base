using System;
using System.Linq;
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
    public class FinancialStatementController : Controller
    {

        #region Declaration
        BalanceSheet _oBalanceSheet = new BalanceSheet();
        List<BalanceSheet> _oBalanceSheets = new List<BalanceSheet>();
        SP_GeneralLedger _oSP_GeneralLedger = new SP_GeneralLedger();
        SP_GeneralJournal _oSP_GeneralJournal = new SP_GeneralJournal();
        List<SP_GeneralJournal> _oSP_GeneralJournals = new List<SP_GeneralJournal>();
        List<SP_GeneralLedger> _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
        IncomeStatement _oIncomeStatement = new IncomeStatement();
        List<IncomeStatement> _oIncomeStatements = new List<IncomeStatement>();
        CostCenterBreakdown _oCostCenterBreakdown = new CostCenterBreakdown();
        VoucherBillBreakDown _oVoucherBillBreakDown = new VoucherBillBreakDown();
        string _sErrorMessage = "";
        #endregion

        #region New Version
        #region CC GL According to Config
        private List<CostCenterBreakdown> GetsCCGLAccordingToConfig(CostCenterBreakdown oCostCenterBreakdown, List<ACConfig> oACConfigs)
        {
            List<CostCenterBreakdown> oNewCostCenterBreakdowns = new List<CostCenterBreakdown>();
            CostCenterBreakdown oNewCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oTempCostCenterBreakdowns = new List<CostCenterBreakdown>();


            List<VoucherBillTransaction> oSubLedgerBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherCheque> oSLVoucherCheques = new List<VoucherCheque>();

            string sSQL = "";
            if (oCostCenterBreakdown.IsApproved == true)
            {
                if (oCostCenterBreakdown.AccountHeadID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oCostCenterBreakdown.AccountHeadID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                if (oCostCenterBreakdown.AccountHeadID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oCostCenterBreakdown.AccountHeadID > 0)
                {
                    sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                else
                {
                    sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }


            foreach (CostCenterBreakdown oCCGL in oCostCenterBreakdown.CostCenterBreakdowns)
            {
                oNewCostCenterBreakdowns.Add(oCCGL);
                foreach (ACConfig oItem in oACConfigs)
                {
                    if (oItem.ConfigureValue == "1")
                    {

                        #region Account Head Narration
                        if (oItem.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
                        {
                            oNewCostCenterBreakdown = GetCCGLNarration(EnumConfigureType.GLAccHeadWiseNarration, oCCGL);
                            oNewCostCenterBreakdown.VoucherDate = oCCGL.VoucherDate;
                            if (oNewCostCenterBreakdown.IsNullOrNot == false)
                            {
                                oNewCostCenterBreakdowns.Add(oNewCostCenterBreakdown);
                            }
                        }
                        #endregion
                        #region Voucher Narration
                        else if (oItem.ConfigureType == EnumConfigureType.GLVoucherNarration)
                        {
                            oNewCostCenterBreakdown = GetCCGLNarration(EnumConfigureType.GLVoucherNarration, oCCGL);
                            oNewCostCenterBreakdown.VoucherDate = oCCGL.VoucherDate;
                            if (oNewCostCenterBreakdown.IsNullOrNot == false)
                            {
                                oNewCostCenterBreakdowns.Add(oNewCostCenterBreakdown);
                            }
                        }
                        #endregion

                        #region Voucher Bill Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GLBT)
                        {
                            if (oSubLedgerBillTransactions.Count > 0)
                            {
                                oTempCostCenterBreakdowns = new List<CostCenterBreakdown>();
                                oTempCostCenterBreakdowns = GetCCGLBT(oSubLedgerBillTransactions, oCCGL.VoucherDetailID, oCCGL.CCTID, oCCGL.VoucherDate);
                                oNewCostCenterBreakdowns.AddRange(oTempCostCenterBreakdowns);
                            }
                        }
                        #endregion

                        #region Voucher Cheque
                        else if (oItem.ConfigureType == EnumConfigureType.GLVC)
                        {
                            if (oSLVoucherCheques.Count > 0)
                            {
                                oTempCostCenterBreakdowns = new List<CostCenterBreakdown>();
                                oTempCostCenterBreakdowns = GetCCGLVC(oSLVoucherCheques, oCCGL.VoucherDetailID, oCCGL.CCTID, oCCGL.VoucherDate);
                                oNewCostCenterBreakdowns.AddRange(oTempCostCenterBreakdowns);
                            }
                        }
                        #endregion

                    }
                }
            }
            return oNewCostCenterBreakdowns;
        }
        private List<CostCenterBreakdown> GetCCGLBT(List<VoucherBillTransaction> oVoucherBillTransactions, int nVoucherDetailID, int nCCTID, DateTime dVoucherDate)
        {
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            if (nVoucherDetailID > 0)
            {
                foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
                {
                    if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                    {
                        string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                        oCostCenterBreakdown = new CostCenterBreakdown();
                        oCostCenterBreakdown.ConfigTitle = oItem.CCTID > 0 ? "SL BT " : "BT ";
                        oCostCenterBreakdown.ConfigType = EnumConfigureType.GLBT;
                        oCostCenterBreakdown.VoucherDate = dVoucherDate;
                        oCostCenterBreakdown.VoucherNo = " Bill No : " + oItem.BillNo + " Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                        oCostCenterBreakdowns.Add(oCostCenterBreakdown);
                    }
                }
            }
            return oCostCenterBreakdowns;
        }

        private List<CostCenterBreakdown> GetCCGLVC(List<VoucherCheque> oVoucherCheques, int nVoucherDetailID, int nCCTID, DateTime dVoucherDate)
        {
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            if (nVoucherDetailID > 0)
            {
                foreach (VoucherCheque oItem in oVoucherCheques)
                {
                    if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                    {
                        oCostCenterBreakdown = new CostCenterBreakdown();
                        oCostCenterBreakdown.ConfigTitle = oItem.CCTID > 0 ? "SL Cheque" : "Cheque";
                        oCostCenterBreakdown.ConfigType = EnumConfigureType.GLVC;
                        oCostCenterBreakdown.VoucherDate = dVoucherDate;
                        oCostCenterBreakdown.VoucherNo = " Cheque No :" + oItem.ChequeNo + " Account No :" + oItem.AccountNo + " Amount : " + Global.MillionFormat(oItem.Amount);
                        oCostCenterBreakdowns.Add(oCostCenterBreakdown);
                    }
                }
            }
            return oCostCenterBreakdowns;
        }

        private CostCenterBreakdown GetCCGLNarration(EnumConfigureType eEnumConfigureType, CostCenterBreakdown oCostCenterBreakdown)
        {
            CostCenterBreakdown oNewCostCenterBreakdown = new CostCenterBreakdown();
            if (eEnumConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
            {
                oNewCostCenterBreakdown.ConfigTitle = " Head Wise Narration ";
                oNewCostCenterBreakdown.VoucherNo = oCostCenterBreakdown.Narration;
                if (oNewCostCenterBreakdown.VoucherNo == "")
                {
                    oNewCostCenterBreakdown.IsNullOrNot = true;
                }
            }
            else if (eEnumConfigureType == EnumConfigureType.GLVoucherNarration)
            {
                oNewCostCenterBreakdown.ConfigTitle = " Voucher Narration ";
                oNewCostCenterBreakdown.VoucherNo = oCostCenterBreakdown.VoucherNarration;
                if (oNewCostCenterBreakdown.VoucherNo == "")
                {
                    oNewCostCenterBreakdown.IsNullOrNot = true;
                }
            }
            return oNewCostCenterBreakdown;
        }
        #endregion
        #region Set SubLedger SessionData
        [HttpPost]
        public ActionResult SetCCBSessionData(CostCenterBreakdown oCostCenterBreakdown)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCostCenterBreakdown);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SubLedger Tarnsactions
        public ActionResult ViewSubLedgerTransactions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCostCenterBreakdown = new CostCenterBreakdown();
            AccountingSession oAccountingSession = new AccountingSession();
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            try
            {
                oCostCenterBreakdown = (CostCenterBreakdown)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oCostCenterBreakdown = null;
            }

            if (oCostCenterBreakdown  != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oCostCenterBreakdown.BusinessUnitIDs.Split(',');
                for (int i = 0; i < aBUs.Count(); i++)
                {
                    if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                }
                #endregion

                if (oCostCenterBreakdown.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oCostCenterBreakdown.StartDate = oAccountingSession.StartDate;
                    oCostCenterBreakdown.EndDate = DateTime.Now;
                }
                oCostCenterBreakdowns = CostCenterBreakdown.GetsAccountWiseBreakdown(oCostCenterBreakdown.AccountHeadID, oCostCenterBreakdown.BusinessUnitIDs, oCostCenterBreakdown.CCID, oCostCenterBreakdown.CurrencyID, oCostCenterBreakdown.StartDate, oCostCenterBreakdown.EndDate, oCostCenterBreakdown.IsApproved, (EnumBalanceStatus)oCostCenterBreakdown.BalanceStatusInt, (int)Session[SessionInfo.currentUserID]);
                oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
                oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            }
            else
            {
                oCostCenterBreakdown  = new CostCenterBreakdown ();
                oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oCostCenterBreakdown.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oCostCenterBreakdown.CCID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BalanceStatus = EnumObject.jGets(typeof(EnumBalanceStatus));
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oCostCenterBreakdown);
        }

        [HttpPost]
        public JsonResult GetsSubLedgerTransactions(CostCenterBreakdown oCCB)
        {
            string sjson = "";
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();

            oCostCenterBreakdowns = CostCenterBreakdown.GetsAccountWiseBreakdown(oCCB.AccountHeadID, oCCB.BusinessUnitIDs, oCCB.CCID, oCCB.CurrencyID, oCCB.StartDate, oCCB.EndDate, oCCB.IsApproved, (EnumBalanceStatus)oCCB.BalanceStatusInt, (int)Session[SessionInfo.currentUserID]);
            oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oCostCenterBreakdowns);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintSubLedgerTransactions(string Params)
        {
            int nParameter = Params.Split('~').Count();
            int nAccountHeadID = nParameter >= 1 ? (Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0])) : 0;
            DateTime dStartDate = nParameter >= 2 ? (Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1])) : DateTime.Now;
            DateTime dEndDate = nParameter >= 3 ? (Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2])) : DateTime.Now;
            string sBusinessUnitIDs = nParameter >= 4 ? (Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3]) : "0";
            int nCCID = nParameter >= 5 ? (Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4])) : 0;
            int nCurrencyID = nParameter >= 6 ? (Params.Split('~')[5] == null ? 0 : Params.Split('~')[5] == "" ? 0 : Convert.ToInt32(Params.Split('~')[5])) : 0;
            bool bIsApproved = nParameter >= 7 ? (Params.Split('~')[6] == null ? false : Params.Split('~')[6] == "" ? false : Convert.ToBoolean(Params.Split('~')[6])) : false;
            string sHeader = nParameter >= 8 ? (Params.Split('~')[7] == null ? "" : Params.Split('~')[7]) : "";
            int nBalanceStatus = nParameter >= 9 ? (Params.Split('~')[8] == null ? 0 : Convert.ToInt32(Params.Split('~')[8])) : 0;
            byte[] abytes = null;
            
            #region Check Authorize Business Unit
            string [] aBUs = sBusinessUnitIDs.Split(',');
            for (int i = 0; i < aBUs.Count();i++ )
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }               
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (!string.IsNullOrEmpty(sBusinessUnitIDs) && sBusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + sBusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            //if (nBusinessUnitID > 0)
            //{
            //    BusinessUnit oBusinessUnit = new BusinessUnit();
            //    oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            //    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            //}

            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.GetsAccountWiseBreakdown(nAccountHeadID, sBusinessUnitIDs, nCCID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (EnumBalanceStatus)nBalanceStatus, (int)Session[SessionInfo.currentUserID]);
            foreach (CostCenterBreakdown oItem in oCostCenterBreakdown.CostCenterBreakdowns)
            {
                if (oItem.CCID != 0)
                {
                    oCostCenterBreakdowns.Add(oItem);
                }
                else
                {
                    oCostCenterBreakdown.CCID = oItem.CCID;
                    oCostCenterBreakdown.CCName = oItem.CCName;
                    oCostCenterBreakdown.OpeiningValue = oItem.OpeiningValue;
                    oCostCenterBreakdown.DebitAmount = oItem.DebitAmount;
                    oCostCenterBreakdown.CreditAmount = oItem.CreditAmount;
                    oCostCenterBreakdown.ClosingValue = oItem.ClosingValue;
                    oCostCenterBreakdown.IsDebit = oItem.IsDebit;
                }
            }
            oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;

            rptCostCenterBreakdowns orptCostCenterBreakdowns = new rptCostCenterBreakdowns();
            abytes = orptCostCenterBreakdowns.PrepareReport(oCostCenterBreakdowns, sHeader, oCompany, (EnumBalanceStatus)nBalanceStatus);

            return File(abytes, "application/pdf");
        }

        public void ExportSLTSToExcel(string Params)
        {
            
            #region Dataget
            int nParameter = Params.Split('~').Count();
            int nAccountHeadID = nParameter >= 1 ? (Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0])) : 0;
            DateTime dStartDate = nParameter >= 2 ? (Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1])) : DateTime.Now;
            DateTime dEndDate = nParameter >= 3 ? (Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2])) : DateTime.Now;
            string sBusinessUnitIDs = nParameter >= 4 ? (Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3]) : "0";
            int nCCID = nParameter >= 5 ? (Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4])) : 0;
            int nCurrencyID = nParameter >= 6 ? (Params.Split('~')[5] == null ? 0 : Params.Split('~')[5] == "" ? 0 : Convert.ToInt32(Params.Split('~')[5])) : 0;
            bool bIsApproved = nParameter >= 7 ? (Params.Split('~')[6] == null ? false : Params.Split('~')[6] == "" ? false : Convert.ToBoolean(Params.Split('~')[6])) : false;
            string sHeader = nParameter >= 8 ? (Params.Split('~')[7] == null ? "" : Params.Split('~')[7]) : "";
            int nBalanceStatus = nParameter >= 9 ? (Params.Split('~')[8] == null ? 0 : Convert.ToInt32(Params.Split('~')[8])) : 0;       
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (!string.IsNullOrEmpty(sBusinessUnitIDs) && sBusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + sBusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            //if (nBusinessUnitID > 0)
            //{
            //    BusinessUnit oBusinessUnit = new BusinessUnit();
            //    oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            //    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            //}

            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdowns = CostCenterBreakdown.GetsAccountWiseBreakdown(nAccountHeadID, sBusinessUnitIDs, nCCID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (EnumBalanceStatus)nBalanceStatus, (int)Session[SessionInfo.currentUserID]);
            
            #endregion
            #region Export Excel
           int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
           ExcelRange cell;
           ExcelRange HeaderCell;
           ExcelFill fill;
           OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SubLedger Transactions");
                sheet.Name = "SubLedger Transactions";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 10;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                nEndCol = 9;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Account"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Debit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Credit Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCredit = 0, nDebit = 0, nOpening = 0, nClosing = 0;
                foreach (CostCenterBreakdown oItem in oCostCenterBreakdowns)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ParentHeadCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ParentHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.CCName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OpeiningValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ClosingValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nOpening = nOpening + oItem.OpeiningValue;
                    nDebit = nDebit + oItem.DebitAmount;
                    nCredit = nCredit + oItem.CreditAmount;
                    nClosing = nClosing + oItem.ClosingValue;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nOpening; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nClosing; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SubLedger_Transactions.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }        
        #endregion
        #region SubLedger Ledger

        public ActionResult ViewSubLedgerLedger(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCostCenterBreakdown = new CostCenterBreakdown();
            AccountingSession oAccountingSession = new AccountingSession();
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            try
            {
                oCostCenterBreakdown = (CostCenterBreakdown)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oCostCenterBreakdown = null;
            }

            if (oCostCenterBreakdown != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oCostCenterBreakdown.BusinessUnitIDs.Split(',');
                for (int i = 0; i < aBUs.Count(); i++)
                {
                    if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                }
                #endregion

                if (oCostCenterBreakdown.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oCostCenterBreakdown.StartDate = oAccountingSession.StartDate;
                    oCostCenterBreakdown.EndDate = DateTime.Now;
                }
                oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(oCostCenterBreakdown.BusinessUnitIDs, oCostCenterBreakdown.AccountHeadID, oCostCenterBreakdown.CCID, oCostCenterBreakdown.CurrencyID, oCostCenterBreakdown.StartDate, oCostCenterBreakdown.EndDate, oCostCenterBreakdown.IsApproved, (int)Session[SessionInfo.currentUserID]);
                oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
                oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
                oCostCenterBreakdown.CostCenterBreakdowns = this.GetsCCGLAccordingToConfig(oCostCenterBreakdown, oCostCenterBreakdown.ACConfigs);
            }
            else
            {
                oCostCenterBreakdown = new CostCenterBreakdown();
                oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oCostCenterBreakdown.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oCostCenterBreakdown.CCID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            #region GL Details
            List<EnumObject> oBreakdownTypeObjs = new List<EnumObject>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumBreakdownType)))
            {
                if (oItem.id != 0)
                {
                    oBreakdownTypeObjs.Add(oItem);
                }
            }
            ViewBag.Preferences = oBreakdownTypeObjs;
            #endregion
            #region Configure
            List<ACConfig> oACConfigs = new List<ACConfig>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumConfigureType)))
            {
                if (oItem.id != 0 && oItem.id < 11)
                {
                    ACConfig oACConfig = new ACConfig();
                    oACConfig.ConfigureValue = "1";
                    oACConfig.ConfigureValueType = EnumConfigureValueType.BoolValue;
                    oACConfig.ConfigureType = (EnumConfigureType)oItem.id;
                    oACConfig.ErrorMessage = EnumObject.jGet(oACConfig.ConfigureType);
                    oACConfigs.Add(oACConfig);
                }
            }
            ViewBag.GLConfigs = oACConfigs;
            #endregion

            this.Session.Remove(SessionInfo.SearchData);
            this.Session.Add(SessionInfo.SearchData, oCostCenterBreakdown.CostCenterBreakdowns);
            oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oCostCenterBreakdown);
        }


        [HttpPost]
        public JsonResult GetSessionData(CostCenterBreakdown oCostCenterBreakdown)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            try
            {
                oCostCenterBreakdowns = (List<CostCenterBreakdown>)Session[SessionInfo.SearchData];
            }
            catch (Exception ex)
            {
                oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            }
            var jsonResult = Json(oCostCenterBreakdowns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsSubLedgerLedger(CostCenterBreakdown oCCB)
        {
            string sjson = "";
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdown = oCCB;

            oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(oCostCenterBreakdown.BusinessUnitIDs, oCostCenterBreakdown.AccountHeadID, oCostCenterBreakdown.CCID, oCostCenterBreakdown.CurrencyID, oCostCenterBreakdown.StartDate, oCostCenterBreakdown.EndDate, oCostCenterBreakdown.IsApproved, (int)Session[SessionInfo.currentUserID]);
            oCostCenterBreakdown.CostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            oCostCenterBreakdowns = this.GetsCCGLAccordingToConfig(oCostCenterBreakdown, oCostCenterBreakdown.ACConfigs);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oCostCenterBreakdowns);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintSubLedgerLedger(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCCID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            int nCurrencyID = Params.Split('~')[5] == null ? 0 : Params.Split('~')[5] == "" ? 0 : Convert.ToInt32(Params.Split('~')[5]);
            bool bIsApproved = Params.Split('~')[6] == null ? false : Params.Split('~')[6] == "" ? false : Convert.ToBoolean(Params.Split('~')[6]);
            string sHeader = Params.Split('~')[7] == null ? "" : Params.Split('~')[7];
            byte[] abytes = null;

            #region Check Authorize Business Unit
             string [] aBUs = BusinessUnitIDs.Split(',');
             for (int i = 0; i < aBUs.Count(); i++)
             {
                 if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                 {

                     rptErrorMessage oErrorReport = new rptErrorMessage();
                     byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                     return File(aErrorMessagebytes, "application/pdf");
                 }
             }
            #endregion
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }
            ACCostCenter oACCostCenter=new ACCostCenter();
            oACCostCenter=oACCostCenter.Get(nCCID,(int)Session[SessionInfo.currentUserID]);
            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(nCurrencyID > 0 ? nCurrencyID : 1, (int)Session[SessionInfo.currentUserID]);


            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(BusinessUnitIDs, nAccountHeadID, nCCID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            oCostCenterBreakdown.Company = oCompany;
            oCostCenterBreakdown.Currency = oCurrency;

            string sAddress = "";
            if (oACCostCenter.ReferenceType == EnumReferenceType.Customer || oACCostCenter.ReferenceType == EnumReferenceType.Vendor || oACCostCenter.ReferenceType == EnumReferenceType.Vendor_Foreign)
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(oACCostCenter.ReferenceObjectID, (int)Session[SessionInfo.currentUserID]);
                sAddress = oContractor.Address;
            }
            ClientOperationSetting oClientOperationSetting= new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.Sub_Ledger_Report_Format, (int)Session[SessionInfo.currentUserID]);
            if(Convert.ToInt32(oClientOperationSetting.Value)==(int)EnumClientOperationValueFormat.ColumnWise)
            {
                rptCostCenterLedgerTextWise orptCostCenterLedger = new rptCostCenterLedgerTextWise(); // Name Define  Problem Its Basically Column Wise
                abytes = orptCostCenterLedger.PrepareReport(oCostCenterBreakdown, dStartDate, dEndDate, oACCostCenter.Code, oACCostCenter.Name, oCostCenterBreakdowns[oCostCenterBreakdowns.Count - 1].ClosingValue, bIsApproved, sAddress);                
            }
            else
            {
                rptCostCenterLedger orptCostCenterLedger = new rptCostCenterLedger();
                abytes = orptCostCenterLedger.PrepareReport(oCostCenterBreakdown, dStartDate, dEndDate, oACCostCenter.Code, oACCostCenter.Name, oCostCenterBreakdowns[oCostCenterBreakdowns.Count - 1].ClosingValue, bIsApproved, sAddress);
            }
            return File(abytes, "application/pdf");
        }

        public void ExportSLGLToExcel(string Params)
        {
            #region Data Get
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCCID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            int nCurrencyID = Params.Split('~')[5] == null ? 0 : Params.Split('~')[5] == "" ? 0 : Convert.ToInt32(Params.Split('~')[5]);
            bool bIsApproved = Params.Split('~')[6] == null ? false : Params.Split('~')[6] == "" ? false : Convert.ToBoolean(Params.Split('~')[6]);
            string sHeader = Params.Split('~')[7] == null ? "" : Params.Split('~')[7];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);


            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            ACCostCenter oACCostCenter = new ACCostCenter();
            oACCostCenter = oACCostCenter.Get(nCCID, (int)Session[SessionInfo.currentUserID]);
            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(nCurrencyID > 0 ? nCurrencyID : 1, (int)Session[SessionInfo.currentUserID]);


            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(BusinessUnitIDs, nAccountHeadID, nCCID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            oCostCenterBreakdown.Company = oCompany;
            oCostCenterBreakdown.Currency = oCurrency;
            #endregion
            #region Export To Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            List<ExcelRange> HeaderCells = new List<ExcelRange>();
            List<ExcelRange> ReportCells = new List<ExcelRange>();
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SubLedger Ledger");
                sheet.Name = "SubLedger Ledger";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = "SubLedger Ledger"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = bIsApproved ? "(Approved Only)" : "(UnApproved And Approved)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion
                Double CurrentBalance = 0;
                List<int> ParentHeadIDs = new List<int>();
                ParentHeadIDs = oCostCenterBreakdowns.Select(x => x.ParentHeadID).Distinct().ToList();
                foreach (int nParentHeadID in ParentHeadIDs)
                {
                    //nRowIndex = nRowIndex + 2;
                    string sParentHeadName = "", sParentHeadCode = "";
                    List<CostCenterBreakdown> oCCBs = new List<CostCenterBreakdown>();

                    oCCBs = oCostCenterBreakdowns.Where(x => x.ParentHeadID == nParentHeadID).ToList();

                    CurrentBalance = oCCBs[oCCBs.Count - 1].ClosingValue;
                    sParentHeadCode = oCCBs[0].ParentHeadCode;
                    sParentHeadName = oCCBs[0].ParentHeadName;
                    #region SubLedger Info
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Merge = true;
                    cell.Value = "Sub Ledger Name :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Merge = true;
                    cell.Value = oACCostCenter.Name; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Report Date :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = dStartDate.ToString("dd MMM yyyy") + " -to- " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Merge = true;
                    cell.Value = "Sub Ledger No :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Merge = true;
                    cell.Value = oACCostCenter.Code; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Account :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = sParentHeadName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Merge = true;
                    cell.Value = "Current Balance :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Merge = true;
                    cell.Value = CurrentBalance; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Account Code"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = sParentHeadCode; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Column Header
                    nStartRow = nRowIndex;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Account"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Crdit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    HeaderCells.Add(sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]);
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data
                    int nCount = 0;
                    bool bIsBold = false;
                    //Double nCreditClosing = 0, nDebitClosing = 0;
                    foreach (CostCenterBreakdown oItem in oCCBs)
                    {
                       
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = bIsBold; cell.Style.Numberformat.Format = "General";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3];
                        if (oItem.VoucherID > 0)
                        {
                            cell.Value = oItem.VoucherDateInString;
                        }
                        else
                        {
                            cell.Value = dStartDate.ToString("dd MMM yyyy");
                        }
                        cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        //cell.Style.Numberformat.Format = "d-mmm-yy";

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ParentHeadName; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Description; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ClosingValue; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                        nEndRow = nRowIndex;
                        nRowIndex ++;

                        //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                        //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                    }
                    ReportCells.Add(sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol]);
                    nRowIndex = nRowIndex + 3;
                    #endregion
                }
                //#region Total
                //cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 4]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 5]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //nRowIndex++;
                //#endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                foreach (ExcelRange oItem in HeaderCells)
                {
                    fill = oItem.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.LightGray);
                }

                foreach (ExcelRange oItem in ReportCells)
                {
                    border = oItem.Style.Border; 
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SubLedger_Ledger.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        } 
        #endregion


        #region Set Bill SessionData
        [HttpPost]
        public ActionResult SetVRRSessionData(VoucherRefReport oVoucherRefReport)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVoucherRefReport);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Bill Tarnsactions
        public ActionResult ViewBillTransactions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            oVoucherRefReport = (VoucherRefReport)Session[SessionInfo.ParamObj];

            if (oVoucherRefReport != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oVoucherRefReport.BusinessUnitIDs.Split(',');
                 for (int i = 0; i < aBUs.Count(); i++)
                 {

                     if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                     {
                         rptErrorMessage oErrorReport = new rptErrorMessage();
                         byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                         return File(aErrorMessagebytes, "application/pdf");
                     }
                 }
                #endregion

                if (oVoucherRefReport.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oVoucherRefReport.StartDate = oAccountingSession.StartDate;
                    oVoucherRefReport.EndDate = DateTime.Now;
                }
                oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);
                oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
                oVoucherRefReport.VoucherRefReports = oVoucherRefReports;
            }
            else
            {
                oVoucherRefReport = new VoucherRefReport();
                oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVoucherRefReport.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oVoucherRefReport.CCID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Bill = new VoucherBill().Get(oVoucherRefReport.VoucherBillID, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherRefReport);
        }

        [HttpPost]
        public JsonResult GetsBillTransactions(VoucherRefReport oVRR)
        {
            string sjson = "";
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();

            oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVRR, (int)Session[SessionInfo.currentUserID]);
            oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
            oVoucherRefReport.VoucherRefReports = oVoucherRefReports;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oVoucherRefReports);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintBillTransactions(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" :Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            int nCCID = Params.Split('~')[7] == null ? 0 : Params.Split('~')[7] == "" ? 0 : Convert.ToInt32(Params.Split('~')[7]);
            bool bIsAllBill = Params.Split('~')[8] == null ? false : Params.Split('~')[8] == "" ? false : Convert.ToBoolean(Params.Split('~')[8]);


            #region Check Authorize Business Unit
             string[] aBUs = BusinessUnitIDs.Split(',');
                 for (int i = 0; i < aBUs.Count(); i++)
                 {
                     if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                     {
                         rptErrorMessage oErrorReport = new rptErrorMessage();
                         byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                         return File(aErrorMessagebytes, "application/pdf");
                     }
                }
            #endregion
            byte[] abytes = null;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
           }

            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            oVoucherRefReport.AccountHeadID = nAccountHeadID;
            oVoucherRefReport.BusinessUnitIDs = BusinessUnitIDs;
            oVoucherRefReport.CurrencyID = nCurrencyID;
            oVoucherRefReport.CCID = nCCID;
            oVoucherRefReport.IsApproved = bIsApproved;
            oVoucherRefReport.StartDate = dStartDate;
            oVoucherRefReport.EndDate = dEndDate;
            oVoucherRefReport.IsAllBill = bIsAllBill;
            oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);

            //oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;

            rptVoucherRefReports orptVoucherRefReports = new rptVoucherRefReports();
            abytes = orptVoucherRefReports.PrepareReport(oVoucherRefReports, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportBillTSToExcel(string Params)
        {
            #region Dataget
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            int nCCID = Params.Split('~')[7] == null ? 0 : Params.Split('~')[7] == "" ? 0 : Convert.ToInt32(Params.Split('~')[7]);
            bool bIsAllBill = Params.Split('~')[8] == null ? false : Params.Split('~')[8] == "" ? false : Convert.ToBoolean(Params.Split('~')[8]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            oVoucherRefReport.AccountHeadID = nAccountHeadID;
            oVoucherRefReport.BusinessUnitIDs = BusinessUnitIDs;
            oVoucherRefReport.CurrencyID = nCurrencyID;
            oVoucherRefReport.CCID = nCCID;
            oVoucherRefReport.IsApproved = bIsApproved;
            oVoucherRefReport.StartDate = dStartDate;
            oVoucherRefReport.EndDate = dEndDate;
            oVoucherRefReport.IsAllBill = bIsAllBill;
            oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Bill Transactions");
                sheet.Name = "Bill Transactions";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;

                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 4, nRowIndex + 1, 4].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex + 1, 4]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 5, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 5, nRowIndex, 6]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex+1, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex+1, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 7, nRowIndex + 1, 7].Merge = true;
                cell = sheet.Cells[nRowIndex, 7, nRowIndex + 1, 7]; cell.Value = "Remaining Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 8, nRowIndex + 1, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 8, nRowIndex + 1, 8]; cell.Value = "Due Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 9, nRowIndex + 1, 9].Merge = true;
                cell = sheet.Cells[nRowIndex, 9, nRowIndex + 1, 9]; cell.Value = "Overdue Days"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex+1, 9];
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCredit = 0, nDebit = 0, nClosing = 0;
                foreach (VoucherRefReport oItem in oVoucherRefReports)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.BillDateInString; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BillNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.RemainingBalance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DueDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.OverdueByDays; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nDebit = nDebit + oItem.DebitAmount;
                    nCredit = nCredit + oItem.CreditAmount;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

               
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 10];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 9];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Bill_Transactions.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }        
        #endregion
        #region Bill Ledger
        public ActionResult ViewBillLedger(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            try
            {
                oVoucherRefReport = (VoucherRefReport)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oVoucherRefReport = null;
            }
            if (oVoucherRefReport != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oVoucherRefReport.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (oVoucherRefReport.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oVoucherRefReport.StartDate = oAccountingSession.StartDate;
                    oVoucherRefReport.EndDate = DateTime.Now;
                }
                oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);
                oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
                oVoucherRefReport.VoucherRefReports = oVoucherRefReports;
            }
            else
            {
                oVoucherRefReport = new VoucherRefReport();
                oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Bill = new VoucherBill().Get(oVoucherRefReport.VoucherBillID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Configure
            List<ACConfig> oACConfigs = new List<ACConfig>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumConfigureType)))
            {
                if (oItem.id != 0 && oItem.id < 11)
                {
                    ACConfig oACConfig = new ACConfig();
                    oACConfig.ConfigureValue = "1";
                    oACConfig.ConfigureValueType = EnumConfigureValueType.BoolValue;
                    oACConfig.ConfigureType = (EnumConfigureType)oItem.id;
                    oACConfig.ErrorMessage = EnumObject.jGet(oACConfig.ConfigureType);
                    oACConfigs.Add(oACConfig);
                }
            }
            ViewBag.GLConfigs = oACConfigs;
            #endregion

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherRefReport);
        }

        [HttpPost]
        public JsonResult GetsBillLedger(VoucherRefReport oVRR)
        {
            string sjson = "";
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();

            oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVRR, (int)Session[SessionInfo.currentUserID]);
            oVoucherRefReport.VoucherRefReports = new List<VoucherRefReport>();
            oVoucherRefReport.VoucherRefReports = oVoucherRefReports;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oVoucherRefReports);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintBillLedger(string Params)
        {
            int nVoucherBillID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ?"0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            byte[] abytes = null;

            #region Check Authorize Business Unit
            string[] aBUs = BusinessUnitIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }


            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(nCurrencyID, (int)Session[SessionInfo.currentUserID]);
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill = oVoucherBill.Get(nVoucherBillID, (int)Session[SessionInfo.currentUserID]);


            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            oVoucherRefReport.VoucherBillID = nVoucherBillID;
            oVoucherRefReport.BusinessUnitIDs = BusinessUnitIDs;
            oVoucherRefReport.CurrencyID = nCurrencyID;
            oVoucherRefReport.IsApproved = bIsApproved;
            oVoucherRefReport.StartDate = dStartDate;
            oVoucherRefReport.EndDate = dEndDate;
            oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);

            oVoucherRefReport.VoucherRefReports = oVoucherRefReports;

            rptVoucherRefLedger orptVoucherRefLedger = new rptVoucherRefLedger();
            abytes = orptVoucherRefLedger.PrepareReport(oVoucherRefReports, oCompany, oCurrency, dStartDate, dEndDate, oVoucherBill.BillDateInString, oVoucherBill.BillNo, oVoucherRefReports[oVoucherRefReports.Count - 1].ClosingBalance, bIsApproved);

            return File(abytes, "application/pdf");
        }

        public void ExportBillGLToExcel(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            #region Data Get
            int nVoucherBillID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }



            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(nCurrencyID, (int)Session[SessionInfo.currentUserID]);
            VoucherBill oVoucherBill = new VoucherBill();
            oVoucherBill = oVoucherBill.Get(nVoucherBillID, (int)Session[SessionInfo.currentUserID]);


            List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
            VoucherRefReport oVoucherRefReport = new VoucherRefReport();
            oVoucherRefReport.VoucherBillID = nVoucherBillID;
            oVoucherRefReport.BusinessUnitIDs = BusinessUnitIDs;
            oVoucherRefReport.CurrencyID = nCurrencyID;
            oVoucherRefReport.IsApproved = bIsApproved;
            oVoucherRefReport.StartDate = dStartDate;
            oVoucherRefReport.EndDate = dEndDate;
            oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVoucherRefReport, (int)Session[SessionInfo.currentUserID]);

            oVoucherRefReport.VoucherRefReports = oVoucherRefReports;
            #endregion
            #region Export To Excel
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Bill Ledger");
                sheet.Name = "Bill Ledger";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;


                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Bill Ledger"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = bIsApproved ? "(Approved Only)" : "(UnApproved And Approved)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Bill Info
                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Bill Date :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oVoucherBill.BillDateInString; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Report Date :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 7, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 7, nRowIndex, 8]; cell.Value = dStartDate.ToString("dd MMM yyyy") + " -to- " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Bill No :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oVoucherBill.BillNo; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 7, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 7, nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Current Balance :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oVoucherRefReports[oVoucherRefReports.Count - 1].ClosingBalance; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 7, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 7, nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Crdit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                //Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (VoucherRefReport oItem in oVoucherRefReports)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3];
                    if (oItem.VoucherID > 0)
                    {
                        cell.Value = oItem.VoucherDateSt;
                    }
                    else
                    {
                        cell.Value = dStartDate.ToString("dd MMM yyyy");
                    }
                    cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //cell.Style.Numberformat.Format = "d-mmm-yy";

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ClosingBalance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                    //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                    //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                }

                #endregion

                //#region Total
                //cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 4]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 5]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //nRowIndex++;
                //#endregion

                cell = sheet.Cells[1, 1, nRowIndex, 9];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 8];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Bill_Ledger.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        } 
        #endregion

        #region Set Product SessionData
        [HttpPost]
        public ActionResult SetVPTSessionData(VPTransactionSummary oVPTransactionSummary)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVPTransactionSummary);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Product Tarnsactions
        public ActionResult ViewItemTransactions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            try
            {
                oVPTransactionSummary = (VPTransactionSummary)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oVPTransactionSummary = null;
            }
            if (oVPTransactionSummary != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oVPTransactionSummary.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (oVPTransactionSummary.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oVPTransactionSummary.StartDate = oAccountingSession.StartDate;
                    oVPTransactionSummary.EndDate = DateTime.Now;
                }
                oVPTransactionSummarys = VPTransactionSummary.Gets(oVPTransactionSummary.BusinessUnitIDs, oVPTransactionSummary.AccountHeadID, oVPTransactionSummary.CurrencyID, oVPTransactionSummary.StartDate, oVPTransactionSummary.EndDate, oVPTransactionSummary.IsApproved, (int)Session[SessionInfo.currentUserID]);
                oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
                oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            }
            else
            {
                oVPTransactionSummary = new VPTransactionSummary();
                oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVPTransactionSummary.AccountHeadID, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVPTransactionSummary);
        }

        [HttpPost]
        public JsonResult GetsItemTransactions(VPTransactionSummary oVPT)
        {
            string sjson = "";
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();

            oVPTransactionSummarys = VPTransactionSummary.Gets(oVPT.BusinessUnitIDs, oVPT.AccountHeadID, oVPT.CurrencyID, oVPT.StartDate, oVPT.EndDate, oVPT.IsApproved, (int)Session[SessionInfo.currentUserID]);
            oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
            oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oVPTransactionSummarys);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintItemTransactions(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            byte[] abytes = null;

            #region Check Authorize Business Unit
            string[] aBUs = BusinessUnitIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }


            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummarys = VPTransactionSummary.Gets(BusinessUnitIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            //oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;

            rptVPTransactionSummarys orptVPTransactionSummarys = new rptVPTransactionSummarys();
            abytes = orptVPTransactionSummarys.PrepareReport(oVPTransactionSummarys, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportItemTSToExcel(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            #region Dataget
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummarys = VPTransactionSummary.Gets(BusinessUnitIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Item Transactions");
                sheet.Name = "Item Transactions";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 10;
                sheet.Column(6).Width = 10;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 10;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 10;
                sheet.Column(11).Width = 15;
                sheet.Column(12).Width = 10;
                sheet.Column(13).Width = 15;


                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 13].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 13].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 13].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 13].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 13].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Item"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Opening Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "In Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "In Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Out Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Out Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Closing Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 13];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCredit = 0, nDebit = 0, nClosing = 0, nOpening = 0, nCreditQty = 0, nDebitQty = 0, nClosingQty = 0, nOpeningQty = 0;
                foreach (VPTransactionSummary oItem in oVPTransactionSummarys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ClosingPrice; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OpeiningQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OpeiningValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DebitQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.CreditQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.ClosingQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ClosingValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nOpeningQty = nOpeningQty + oItem.OpeiningQty;
                    nOpening = nOpening + oItem.OpeiningValue;
                    nDebitQty = nDebitQty + oItem.DebitQty;
                    nDebit = nDebit + oItem.DebitAmount;
                    nCreditQty = nCreditQty + oItem.CreditQty;
                    nCredit = nCredit + oItem.CreditAmount;
                    nClosingQty = nClosingQty + oItem.ClosingQty;
                    nClosing = nClosing + oItem.ClosingValue;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nOpeningQty; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nOpening; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                
                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nDebitQty; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nCreditQty; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = nClosingQty; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = nClosing; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "##,##,##0.00;(##,##,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 13];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 13];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                sheet.Cells.AutoFitColumns();
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Item_Transactions.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }        
        #endregion
        #region Product Ledger
        public ActionResult ViewItemLedger(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            oVPTransactionSummary = (VPTransactionSummary)Session[SessionInfo.ParamObj];

            if (oVPTransactionSummary != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oVPTransactionSummary.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (oVPTransactionSummary.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oVPTransactionSummary.StartDate = oAccountingSession.StartDate;
                    oVPTransactionSummary.EndDate = DateTime.Now;
                }
                oVPTransactionSummarys = VPTransactionSummary.GetsForProduct(oVPTransactionSummary.BusinessUnitIDs, oVPTransactionSummary.AccountHeadID, oVPTransactionSummary.ProductID, oVPTransactionSummary.CurrencyID, oVPTransactionSummary.StartDate, oVPTransactionSummary.EndDate, oVPTransactionSummary.IsApproved, (int)Session[SessionInfo.currentUserID]);
                oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
                oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            }
            else
            {
                oVPTransactionSummary = new VPTransactionSummary();
                oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oVPTransactionSummary.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Item = new Product().Get(oVPTransactionSummary.ProductID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Configure
            List<ACConfig> oACConfigs = new List<ACConfig>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumConfigureType)))
            {
                if (oItem.id != 0 && oItem.id < 11)
                {
                    ACConfig oACConfig = new ACConfig();
                    oACConfig.ConfigureValue = "1";
                    oACConfig.ConfigureValueType = EnumConfigureValueType.BoolValue;
                    oACConfig.ConfigureType = (EnumConfigureType)oItem.id;                    
                    oACConfig.ErrorMessage = EnumObject.jGet(oACConfig.ConfigureType);
                    oACConfigs.Add(oACConfig);
                }
            }
            ViewBag.GLConfigs = oACConfigs;
            #endregion

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVPTransactionSummary);
        }

        [HttpPost]
        public JsonResult GetsItemLedger(VPTransactionSummary oVPT)
        {
            string sjson = "";
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();

            oVPTransactionSummarys = VPTransactionSummary.GetsForProduct(oVPT.BusinessUnitIDs, oVPT.AccountHeadID, oVPT.ProductID, oVPT.CurrencyID, oVPT.StartDate, oVPT.EndDate, oVPT.IsApproved, (int)Session[SessionInfo.currentUserID]);
            oVPTransactionSummary.VPTransactionSummarys = new List<VPTransactionSummary>();
            oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oVPTransactionSummarys);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintItemLedger(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            int nProductID = Params.Split('~')[7] == null ? 0 : Params.Split('~')[7] == "" ? 0 : Convert.ToInt32(Params.Split('~')[7]);
            byte[] abytes = null;

            #region Check Authorize Business Unit

            string[] aBUs = BusinessUnitIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }



            Currency oCurrency=new Currency();
            oCurrency=oCurrency.Get(nCurrencyID,(int)Session[SessionInfo.currentUserID]);
            Product oProduct=new Product();
            oProduct=oProduct.Get(nProductID,(int)Session[SessionInfo.currentUserID]);


            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummarys = VPTransactionSummary.GetsForProduct(BusinessUnitIDs, nAccountHeadID, nProductID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            oVPTransactionSummary.Company=oCompany;
            oVPTransactionSummary.Currency=oCurrency;

            rptProductLedger orptProductLedger = new rptProductLedger();
            abytes = orptProductLedger.PrepareReport(oVPTransactionSummary, dStartDate, dEndDate, oProduct.ProductCode, oProduct.ProductName, oVPTransactionSummarys[oVPTransactionSummarys.Count - 1].ClosingValue, bIsApproved);

            return File(abytes, "application/pdf");
        }

        public void ExportItemGLToExcel(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            #region Data Get
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string BusinessUnitIDs = Params.Split('~')[3] == null ? "0" : Params.Split('~')[3] == "" ? "0" : Params.Split('~')[3];
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            int nProductID = Params.Split('~')[7] == null ? 0 : Params.Split('~')[7] == "" ? 0 : Convert.ToInt32(Params.Split('~')[7]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            Currency oCurrency = new Currency();
            oCurrency = oCurrency.Get(nCurrencyID, (int)Session[SessionInfo.currentUserID]);
            Product oProduct = new Product();
            oProduct = oProduct.Get(nProductID, (int)Session[SessionInfo.currentUserID]);

            List<VPTransactionSummary> oVPTransactionSummarys = new List<VPTransactionSummary>();
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummarys = VPTransactionSummary.GetsForProduct(BusinessUnitIDs, nAccountHeadID, nProductID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;
            oVPTransactionSummary.Company = oCompany;
            oVPTransactionSummary.Currency = oCurrency;
            #endregion
            #region Export To Excel
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Item Ledger");
                sheet.Name = "Item Ledger";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 10;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 10;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 10;
                sheet.Column(11).Width = 20;


                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Item Ledger"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = bIsApproved ? "(Approved Only)" : "(UnApproved And Approved)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Item Info
                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Item Name :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oProduct.ProductName; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 6, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 6, nRowIndex, 8]; cell.Value = "Report Date :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 9, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = dStartDate.ToString("dd MMM yyyy") + " -to- " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Item Code :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oProduct.ProductCode; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 6, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 6, nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 9, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;

                sheet.Cells[nRowIndex, 2, nRowIndex, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Current Balance :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4, nRowIndex, 5]; cell.Value = oVPTransactionSummarys[oVPTransactionSummarys.Count - 1].ClosingValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                sheet.Cells[nRowIndex, 6, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 6, nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                sheet.Cells[nRowIndex, 9, nRowIndex, 11].Merge = true;
                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.None; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "In Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "In Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Out Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Out Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Closing Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Closing Value"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 11];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                //Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (VPTransactionSummary oItem in oVPTransactionSummarys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3];
                    if (oItem.VoucherID > 0)
                    {
                        cell.Value = oItem.VoucherDateInString;
                    }
                    else
                    {
                        cell.Value = dStartDate.ToString("dd MMM yyyy");
                    }
                    cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //cell.Style.Numberformat.Format = "d-mmm-yy";

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DebitQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.CreditQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ClosingQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ClosingValue; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                    //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                    //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                }

                #endregion

                //#region Total
                //cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 4]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[nRowIndex, 5]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                //border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //nRowIndex++;
                //#endregion

                cell = sheet.Cells[1, 1, nRowIndex, 12];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 11];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Item_Ledger.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        } 
        #endregion

        #region Opening BreakDown
        #region Set Opening SessionData
        [HttpPost]
        public ActionResult SetAHOBSessionData(AHOpeningBreakdown oAHOpeningBreakdown)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oAHOpeningBreakdown);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AHOpeningBreakdowns
        public ActionResult ViewAHOpeningBreakdowns(int menuid)
        {

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            List<AHOpeningBreakdown> oAHOpeningBreakdowns = new List<AHOpeningBreakdown>();
            oAHOpeningBreakdown = (AHOpeningBreakdown)Session[SessionInfo.ParamObj];

            if (oAHOpeningBreakdown != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oAHOpeningBreakdown.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (oAHOpeningBreakdown.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oAHOpeningBreakdown.StartDate = oAccountingSession.StartDate;
                }
                oAHOpeningBreakdowns = AHOpeningBreakdown.Gets(oAHOpeningBreakdown.BusinessUnitID, oAHOpeningBreakdown.AccountHeadID, oAHOpeningBreakdown.CurrencyID, oAHOpeningBreakdown.StartDate, oAHOpeningBreakdown.IsApproved, (int)Session[SessionInfo.currentUserID]);
                oAHOpeningBreakdown.AHOpeningBreakdowns = new List<AHOpeningBreakdown>();
                oAHOpeningBreakdown.AHOpeningBreakdowns = oAHOpeningBreakdowns;
            }
            else
            {
                oAHOpeningBreakdown = new AHOpeningBreakdown();
                oAHOpeningBreakdown.AHOpeningBreakdowns = new List<AHOpeningBreakdown>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oAHOpeningBreakdown.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oAHOpeningBreakdown);
        }

        [HttpPost]
        public JsonResult GetsAHOpeningBreakdowns(AHOpeningBreakdown oAHOB)
        {
            string sjson = "";
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            List<AHOpeningBreakdown> oAHOpeningBreakdowns = new List<AHOpeningBreakdown>();

            oAHOpeningBreakdowns = AHOpeningBreakdown.Gets(oAHOB.BusinessUnitID, oAHOB.AccountHeadID, oAHOB.CurrencyID, oAHOB.StartDate, oAHOB.IsApproved, (int)Session[SessionInfo.currentUserID]);
            oAHOpeningBreakdown.AHOpeningBreakdowns = new List<AHOpeningBreakdown>();
            oAHOpeningBreakdown.AHOpeningBreakdowns = oAHOpeningBreakdowns;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oAHOpeningBreakdowns);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintAHOpeningBreakdowns(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            int nBusinessUnitID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            int nCurrencyID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            bool bIsApproved = Params.Split('~')[4] == null ? false : Params.Split('~')[4] == "" ? false : Convert.ToBoolean(Params.Split('~')[4]);
            string sHeader = Params.Split('~')[5] == null ? "" : Params.Split('~')[5];
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<AHOpeningBreakdown> oAHOpeningBreakdowns = new List<AHOpeningBreakdown>();
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            oAHOpeningBreakdowns = AHOpeningBreakdown.Gets(nBusinessUnitID, nAccountHeadID, nCurrencyID, dStartDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            //oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;

            rptAHOpeningBreakdowns orptAHOpeningBreakdowns = new rptAHOpeningBreakdowns();
            abytes = orptAHOpeningBreakdowns.PrepareReport(oAHOpeningBreakdowns, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportAHOBToExcel(string Params)
        {


            #region Dataget
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            int nBusinessUnitID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            int nCurrencyID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            bool bIsApproved = Params.Split('~')[4] == null ? false : Params.Split('~')[4] == "" ? false : Convert.ToBoolean(Params.Split('~')[4]);
            string sHeader = Params.Split('~')[5] == null ? "" : Params.Split('~')[5];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<AHOpeningBreakdown> oAHOpeningBreakdowns = new List<AHOpeningBreakdown>();
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            oAHOpeningBreakdowns = AHOpeningBreakdown.Gets(nBusinessUnitID, nAccountHeadID, nCurrencyID, dStartDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("AccountHead Opening BreakDown");
                sheet.Name = "AccountHead Opening BreakDown";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                nEndCol = 7;


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "BreakDown Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCredit = 0, nDebit = 0, nClosing = 0, nOpening = 0, nCreditQty = 0, nDebitQty = 0, nClosingQty = 0, nOpeningQty = 0;
                foreach (AHOpeningBreakdown oItem in oAHOpeningBreakdowns)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.BreakdownName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ClosingAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nOpening = nOpening + oItem.OpeningAmount;
                    nDebit = nDebit + oItem.DebitAmount;
                    nCredit = nCredit + oItem.CreditAmount;
                    nClosing = nClosing + oItem.ClosingAmount;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpening; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosing; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AccountHead_Opening_BreakDown.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Set CC Opening SessionData
        [HttpPost]
        public ActionResult SetCCOBSessionData(CCOpeningBreakdown oCCOpeningBreakdown)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCCOpeningBreakdown);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CCOpeningBreakdown
        public ActionResult ViewCCOpeningBreakdowns(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            List<CCOpeningBreakdown> oCCOpeningBreakdowns = new List<CCOpeningBreakdown>();
            oCCOpeningBreakdown = (CCOpeningBreakdown)Session[SessionInfo.ParamObj];

            if (oCCOpeningBreakdown != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oCCOpeningBreakdown.BusinessUnitIDs.Split(',');
                for (int i = 0; i < aBUs.Count(); i++)
                {
                    if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                }
                #endregion

                if (oCCOpeningBreakdown.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oCCOpeningBreakdown.StartDate = oAccountingSession.StartDate;
                }
                oCCOpeningBreakdowns = CCOpeningBreakdown.Gets(oCCOpeningBreakdown.BusinessUnitIDs, oCCOpeningBreakdown.CCID, oCCOpeningBreakdown.AccountHeadID, oCCOpeningBreakdown.CurrencyID, oCCOpeningBreakdown.StartDate, oCCOpeningBreakdown.IsApproved, (int)Session[SessionInfo.currentUserID]);
                oCCOpeningBreakdown.CCOpeningBreakdowns = new List<CCOpeningBreakdown>();
                oCCOpeningBreakdown.CCOpeningBreakdowns = oCCOpeningBreakdowns;
            }
            else
            {
                oCCOpeningBreakdown = new CCOpeningBreakdown();
                oCCOpeningBreakdown.CCOpeningBreakdowns = new List<CCOpeningBreakdown>();
            }
            ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.COA = new ChartsOfAccount().Get(oCCOpeningBreakdown.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CC = new ACCostCenter().Get(oCCOpeningBreakdown.CCID, (int)Session[SessionInfo.currentUserID]);
            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oCCOpeningBreakdown);
        }

        [HttpPost]
        public JsonResult GetsCCOpeningBreakdowns(CCOpeningBreakdown oCCOB)
        {
            string sjson = "";
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            List<CCOpeningBreakdown> oCCOpeningBreakdowns = new List<CCOpeningBreakdown>();

            oCCOpeningBreakdowns = CCOpeningBreakdown.Gets(oCCOB.BusinessUnitIDs, oCCOB.CCID, oCCOB.AccountHeadID, oCCOB.CurrencyID, oCCOB.StartDate, oCCOB.IsApproved, (int)Session[SessionInfo.currentUserID]);
            oCCOpeningBreakdown.CCOpeningBreakdowns = new List<CCOpeningBreakdown>();
            oCCOpeningBreakdown.CCOpeningBreakdowns = oCCOpeningBreakdowns;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize(oCCOpeningBreakdowns);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintCCOpeningBreakdowns(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            string BusinessUnitIDs = Params.Split('~')[2] == null ? "0" : Params.Split('~')[2] == "" ? "0" : Params.Split('~')[2];
            int nCurrencyID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            bool bIsApproved = Params.Split('~')[4] == null ? false : Params.Split('~')[4] == "" ? false : Convert.ToBoolean(Params.Split('~')[4]);
            string sHeader = Params.Split('~')[5] == null ? "" : Params.Split('~')[5];
            int nCCID = Params.Split('~')[6] == null ? 0 : Params.Split('~')[6] == "" ? 0 : Convert.ToInt32(Params.Split('~')[6]);
            byte[] abytes = null;

            #region Check Authorize Business Unit
            string[] aBUs = BusinessUnitIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }

            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }

            List<CCOpeningBreakdown> oCCOpeningBreakdowns = new List<CCOpeningBreakdown>();
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            oCCOpeningBreakdowns = CCOpeningBreakdown.Gets(BusinessUnitIDs, nCCID, nAccountHeadID, nCurrencyID, dStartDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            //oVPTransactionSummary.VPTransactionSummarys = oVPTransactionSummarys;

            rptCCOpeningBreakdowns orptCCOpeningBreakdowns = new rptCCOpeningBreakdowns();
            abytes = orptCCOpeningBreakdowns.PrepareReport(oCCOpeningBreakdowns, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportCCOBToExcel(string Params)
        {


            #region Dataget
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            string BusinessUnitIDs = Params.Split('~')[2] == null ? "0" : Params.Split('~')[2] == "" ? "0" : Params.Split('~')[2];
            int nCurrencyID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            bool bIsApproved = Params.Split('~')[4] == null ? false : Params.Split('~')[4] == "" ? false : Convert.ToBoolean(Params.Split('~')[4]);
            string sHeader = Params.Split('~')[5] == null ? "" : Params.Split('~')[5];
            int nCCID = Params.Split('~')[6] == null ? 0 : Params.Split('~')[6] == "" ? 0 : Convert.ToInt32(Params.Split('~')[6]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(BusinessUnitIDs) && BusinessUnitIDs != "0")//not empty or not group accounts
            {
                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + BusinessUnitIDs + ")";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oBusinessUnits.Count > 1)
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.ShortName));
                }
                else
                {
                    oCompany.Name = string.Join(",", oBusinessUnits.Select(x => x.Name));
                }
            }
            
            List<CCOpeningBreakdown> oCCOpeningBreakdowns = new List<CCOpeningBreakdown>();
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            oCCOpeningBreakdowns = CCOpeningBreakdown.Gets(BusinessUnitIDs, nCCID, nAccountHeadID, nCurrencyID, dStartDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SubLedger Opening BreakDown");
                sheet.Name = "SubLedger Opening BreakDown";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                nEndCol = 7;


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = cell.Value = "[All Amount Display in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "BreakDown Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Value"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCredit = 0, nDebit = 0, nClosing = 0, nOpening = 0, nCreditQty = 0, nDebitQty = 0, nClosingQty = 0, nOpeningQty = 0;
                foreach (CCOpeningBreakdown oItem in oCCOpeningBreakdowns)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.BreakdownName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ClosingAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    nOpening = nOpening + oItem.OpeningAmount;
                    nDebit = nDebit + oItem.DebitAmount;
                    nCredit = nCredit + oItem.CreditAmount;
                    nClosing = nClosing + oItem.ClosingAmount;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpening; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nDebit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = nCredit; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosing; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SubLedger_Opening_BreakDown.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        #endregion
        
        #region Sales Profit
        #region Set Sales Profit SessionData
        [HttpPost]
        public ActionResult SetSPSessionData(SalesProfit oSalesProfit)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSalesProfit);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Sales Profit

        private List<SalesProfit> GetIncomes(List<SalesProfit> oSalesProfits)
        {
            List<SalesProfit> oTempSalesProfits = new List<SalesProfit>();
            foreach (SalesProfit oItem in oSalesProfits)
            {
                if (oItem.ComponentID == 5)
                {
                    oTempSalesProfits.Add(oItem);
                }
            }
            return oTempSalesProfits;
        }
        private List<SalesProfit> GetExpenses(List<SalesProfit> oSalesProfits)
        {
            List<SalesProfit> oTempSalesProfits = new List<SalesProfit>();
            foreach (SalesProfit oItem in oSalesProfits)
            {
                if (oItem.ComponentID == 6)
                {
                    oTempSalesProfits.Add(oItem);
                }
            }
            return oTempSalesProfits;
        }
        public ActionResult ViewSalesProfit(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];
            AccountingSession oAccountingSession = new AccountingSession();
            SalesProfit oSalesProfit = new SalesProfit();
            List<SalesProfit> oSalesProfits = new List<SalesProfit>();
            
            try
            {
                oSalesProfit = (SalesProfit)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oSalesProfit = null;
            }

            if (oSalesProfit != null)
            {
                if (oSalesProfit.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(nUserID);
                    oSalesProfit.StartDate = oAccountingSession.StartDate;
                    oSalesProfit.EndDate = DateTime.Now;
                }
                oSalesProfits = SalesProfit.Gets(oSalesProfit.OrderID, oSalesProfit.StartDate, oSalesProfit.EndDate, nUserID);
                oSalesProfit.Incomes = new List<SalesProfit>();
                oSalesProfit.Incomes = GetIncomes(oSalesProfits);
                oSalesProfit.Expenses = new List<SalesProfit>();
                oSalesProfit.Expenses = GetExpenses(oSalesProfits);
            }
            else
            {
                oSalesProfit = new SalesProfit();
                oSalesProfit.Incomes = new List<SalesProfit>();
                oSalesProfit.Expenses = new List<SalesProfit>();
            }
            //ViewBag.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            //ViewBag.COA = new ChartsOfAccount().Get(oSalesProfit.AccountHeadID, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            VOrder oSaleOrder=new VOrder();
            oSaleOrder=oSaleOrder.Get(oSalesProfit.OrderID, nUserID); 
            ViewBag.SO = oSaleOrder;
            ViewBag.Company = new Company().Get(1, nUserID);
            ViewBag.BU = new BusinessUnit().Get(oSaleOrder.BUID, nUserID);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oSalesProfit);
        }

        public ActionResult PrintSalesProfit(string Params)
        {
            int nOrderID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = "";
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            VOrder oSaleOrder = new VOrder();
            oSaleOrder = oSaleOrder.Get(nOrderID, (int)Session[SessionInfo.currentUserID]);
            if (oSaleOrder.BUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oSaleOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<SalesProfit> oSalesProfits = new List<SalesProfit>();
            SalesProfit oSalesProfit = new SalesProfit();
            oSalesProfits = SalesProfit.Gets(nOrderID, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);
            oSalesProfit.Incomes = new List<SalesProfit>();
            oSalesProfit.Incomes = GetIncomes(oSalesProfits);
            oSalesProfit.Expenses = new List<SalesProfit>();
            oSalesProfit.Expenses = GetExpenses(oSalesProfits);
            
            rptSalesProfits orptSalesProfits = new rptSalesProfits();
            abytes = orptSalesProfits.PrepareReport(oSalesProfit, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportSalesProfitToExcel(string Params)
        {
           
            #region Dataget
            int nOrderID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = "";
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            
            VOrder oSaleOrder = new VOrder();
            oSaleOrder = oSaleOrder.Get(nOrderID, (int)Session[SessionInfo.currentUserID]);
            if (oSaleOrder.BUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oSaleOrder.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<SalesProfit> oSalesProfits = new List<SalesProfit>();
            SalesProfit oSalesProfit = new SalesProfit();
            oSalesProfits = SalesProfit.Gets(nOrderID, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);
            oSalesProfit.Incomes = new List<SalesProfit>();
            oSalesProfit.Incomes = GetIncomes(oSalesProfits);
            oSalesProfit.Expenses = new List<SalesProfit>();
            oSalesProfit.Expenses = GetExpenses(oSalesProfits);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nIncomeStartRow = 2, nIncomeEndRow = 0, nIncomeStartCol = 2, nIncomeEndCol = 0, nExpenseStartRow = 2, nExpenseEndRow = 0, nExpenseStartCol = 2, nExpenseEndCol = 0;
            ExcelRange cell;
            ExcelRange IncomeHeaderCell;
            ExcelRange ExpenseHeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            double nTotalIncomeAmount = 0;
            double nTotalExpensesAmount = 0;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sales Profit");
                sheet.Name = "Sales Profit";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 20;
                nIncomeEndCol = 8;
                nExpenseEndCol = 8;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nIncomeStartCol, nRowIndex, nIncomeEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                                
                cell = sheet.Cells[nRowIndex, nIncomeStartCol, nRowIndex, nIncomeEndCol]; cell.Merge = true; 
                cell.Value = "Sales Profit"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nIncomeStartCol, nRowIndex, nIncomeEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nIncomeStartCol, nRowIndex, nIncomeEndCol]; cell.Merge = true; 
                cell.Value = "[All Amounts shown in " + oCompany.CurrencyName + "]"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Income
                #region Column Header
                nIncomeStartRow = nRowIndex; nIncomeEndRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sales No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Sales Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Account Head"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Voucher Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                IncomeHeaderCell = sheet.Cells[nRowIndex, nIncomeStartCol, nRowIndex, nIncomeEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;                
                foreach (SalesProfit oItem in oSalesProfit.Incomes)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.VoucherDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nTotalIncomeAmount = nTotalIncomeAmount + oItem.Amount;
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nIncomeEndRow = nRowIndex;
                    nRowIndex++;

                    
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total Income :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalIncomeAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                nRowIndex = nRowIndex + 4;
                #endregion
                
               

                #endregion

                #region Expense
                #region Column Header
                nExpenseStartRow = nRowIndex; nExpenseEndRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Sale No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Sale Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Account"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Voucher Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                ExpenseHeaderCell = sheet.Cells[nRowIndex, nExpenseStartCol, nRowIndex, nExpenseEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                nCount = 0;                
                foreach (SalesProfit oItem in oSalesProfit.Expenses)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.VoucherDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nTotalExpensesAmount = nTotalExpensesAmount + oItem.Amount;
                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nExpenseEndRow = nRowIndex;
                    nRowIndex++;
                    
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total Expense :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nTotalExpensesAmount; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 3;
                #endregion


                #region Net Profit
                double nNetProfitAmount = (nTotalIncomeAmount - nTotalExpensesAmount);
                if (nNetProfitAmount < 0)
                {
                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Net Loss :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (nNetProfitAmount * (-1)); cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;
                }
                else
                {
                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Net Profit :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = nNetProfitAmount; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;
                }
                
                #endregion


                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 9];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = IncomeHeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nIncomeStartRow, nIncomeStartCol, nIncomeEndRow, nIncomeEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                fill = ExpenseHeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nExpenseStartRow, nExpenseStartCol, nExpenseEndRow, nExpenseEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Sales_Profit.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        #endregion

        #region Changes in Equity
        #region Set Changes SessionData
        [HttpPost]
        public ActionResult SetCIESessionData(SP_ChangesInEquity oSP_ChangesInEquity)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSP_ChangesInEquity);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Changes
        public ActionResult ViewChangesInEquity(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            SP_ChangesInEquity oSP_ChangesInEquity = new SP_ChangesInEquity();
            List<SP_ChangesInEquity> oSP_ChangesInEquitys = new List<SP_ChangesInEquity>();
            oSP_ChangesInEquity = (SP_ChangesInEquity)Session[SessionInfo.ParamObj];

            if (oSP_ChangesInEquity != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oSP_ChangesInEquity.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oAccountingSession = oAccountingSession.Get(oSP_ChangesInEquity.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);
                oSP_ChangesInEquity.SessionName = oAccountingSession.SessionName;
                oSP_ChangesInEquity.StartDate = oAccountingSession.StartDate;
                oSP_ChangesInEquity.EndDate = oAccountingSession.EndDate;

                AccountingSession oPreviousSession = new AccountingSession();
                oPreviousSession = oPreviousSession.GetSessionByDate(oAccountingSession.StartDate.AddDays(-1), (int)Session[SessionInfo.currentUserID]);
                oSP_ChangesInEquity.PreviousSessionName =oPreviousSession.AccountingSessionID != oAccountingSession.AccountingSessionID ? oPreviousSession.SessionName : "";

                oSP_ChangesInEquitys = SP_ChangesInEquity.Gets(oSP_ChangesInEquity.AccountingSessionID, oSP_ChangesInEquity.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oSP_ChangesInEquity.SP_ChangesInEquitys = new List<SP_ChangesInEquity>();
                oSP_ChangesInEquity.SP_ChangesInEquitys = oSP_ChangesInEquitys;
            }
            else
            {
                oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                AccountingSession oPreviousSession = new AccountingSession();
                oPreviousSession = oPreviousSession.GetSessionByDate(oAccountingSession.StartDate.AddDays(-1), (int)Session[SessionInfo.currentUserID]);
                

                oSP_ChangesInEquity = new SP_ChangesInEquity();
                oSP_ChangesInEquity.SessionName = oAccountingSession.SessionName;
                oSP_ChangesInEquity.StartDate = oAccountingSession.StartDate;
                oSP_ChangesInEquity.EndDate = oAccountingSession.EndDate;
                oSP_ChangesInEquity.AccountingSessionID = oAccountingSession.AccountingSessionID;
                oSP_ChangesInEquity.PreviousSessionName = oPreviousSession.AccountingSessionID != oAccountingSession.AccountingSessionID ? oPreviousSession.SessionName : "";

                oSP_ChangesInEquitys = SP_ChangesInEquity.Gets(oSP_ChangesInEquity.AccountingSessionID, oSP_ChangesInEquity.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oSP_ChangesInEquity.SP_ChangesInEquitys = new List<SP_ChangesInEquity>();
                oSP_ChangesInEquity.SP_ChangesInEquitys = oSP_ChangesInEquitys;
            }
            ViewBag.Sessions = AccountingSession.GetsAccountingYears((int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oSP_ChangesInEquity.BusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(oSP_ChangesInEquity.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            ViewBag.Company = oCompany;

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oSP_ChangesInEquity);
        }


        public ActionResult PrintChangesInEquity(string Params)
        {
            int nAccountingSessionID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nBusinessUnitID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            AccountingSession oPreviousSession = new AccountingSession();
            oPreviousSession = oPreviousSession.GetSessionByDate(oAccountingSession.StartDate.AddDays(-1), (int)Session[SessionInfo.currentUserID]);

            List<SP_ChangesInEquity> oSP_ChangesInEquitys = new List<SP_ChangesInEquity>();
            SP_ChangesInEquity oSP_ChangesInEquity = new SP_ChangesInEquity();
            oSP_ChangesInEquity.AccountingSessionID = nAccountingSessionID;
            oSP_ChangesInEquity.BusinessUnitID = nBusinessUnitID;
            oSP_ChangesInEquity.StartDate = oAccountingSession.StartDate;
            oSP_ChangesInEquity.EndDate = oAccountingSession.EndDate;
            oSP_ChangesInEquity.SessionName = oAccountingSession.SessionName;
            oSP_ChangesInEquity.PreviousSessionName = oPreviousSession.AccountingSessionID != oAccountingSession.AccountingSessionID ? oPreviousSession.SessionName : "";
            oSP_ChangesInEquitys = SP_ChangesInEquity.Gets(nAccountingSessionID, nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            oSP_ChangesInEquity.SP_ChangesInEquitys = oSP_ChangesInEquitys;

            rptChangesInEquity orptChangesInEquity = new rptChangesInEquity();
            abytes = orptChangesInEquity.PrepareReport(oSP_ChangesInEquity, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportChangesInEquityToExcel(string Params)
        {
            #region Dataget
            int nAccountingSessionID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nBusinessUnitID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(nAccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            AccountingSession oPreviousSession = new AccountingSession();
            oPreviousSession = oPreviousSession.GetSessionByDate(oAccountingSession.StartDate.AddDays(-1), (int)Session[SessionInfo.currentUserID]);

            List<SP_ChangesInEquity> oSP_ChangesInEquitys = new List<SP_ChangesInEquity>();
            SP_ChangesInEquity oSP_ChangesInEquity = new SP_ChangesInEquity();
            oSP_ChangesInEquity.AccountingSessionID = nAccountingSessionID;
            oSP_ChangesInEquity.BusinessUnitID = nBusinessUnitID;
            oSP_ChangesInEquity.StartDate = oAccountingSession.StartDate;
            oSP_ChangesInEquity.EndDate = oAccountingSession.EndDate;
            oSP_ChangesInEquity.SessionName = oAccountingSession.SessionName;
            oSP_ChangesInEquity.PreviousSessionName = oPreviousSession.AccountingSessionID != oAccountingSession.AccountingSessionID ? oPreviousSession.SessionName : "";
            oSP_ChangesInEquitys = SP_ChangesInEquity.Gets(nAccountingSessionID, nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            oSP_ChangesInEquity.SP_ChangesInEquitys = oSP_ChangesInEquitys;

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Changes In Equity");
                sheet.Name = "Changes In Equity";
                sheet.Column(2).Width = 25;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;
                nEndCol = 10;

                #region Report Header
                cell=sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = "Changes In Equity"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.Font.Color.SetColor(Color.Gray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For The Year ended " + oSP_ChangesInEquity.EndDateHeaderSt; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell=sheet.Cells[nRowIndex, 2];
                cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell=sheet.Cells[nRowIndex, 3];
                cell.Value = "Share Capital"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell=sheet.Cells[nRowIndex, 4];
                cell.Value = "Share Premium"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5];
                cell.Value = "Excess of Issue Price over Face Value of GDRs"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6];
                cell.Value = "Capital Reserve on Merger"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7];
                cell.Value = "Revaluation Surplus"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8];
                cell.Value = "Fair Value Gain on Investment"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9];
                cell.Value = "Retained Earnings"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10];
                cell.Value = "Total"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                bool bIsBold = false;
                foreach (SP_ChangesInEquity oItem in oSP_ChangesInEquitys)
                {
                    if (oItem.TransactionType == EnumEquityTransactionType.OperationalProfit)
                    {
                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                        cell.Value = oSP_ChangesInEquity.IncomeSt + " " + oSP_ChangesInEquity.SessionName + ":"; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                        cell.Value = oItem.TransactionTypeSt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = oItem.ShareCapital; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = oItem.SharePremium; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = oItem.ExcessOfIssuePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = oItem.CapitalReserve; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border;  border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = oItem.RevaluationSurplus; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border;  border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = oItem.FairValueGainOnInvestment; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border;  border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = oItem.RetainedEarnings; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border;  border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                    }
                    else if (oItem.TransactionType == EnumEquityTransactionType.OtherIncome)
                    {

                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                        cell.Value = oItem.TransactionTypeSt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = oItem.ShareCapital; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = oItem.SharePremium; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = oItem.ExcessOfIssuePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = oItem.CapitalReserve; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = oItem.RevaluationSurplus; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = oItem.FairValueGainOnInvestment; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = oItem.RetainedEarnings; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                    }
                    else if (oItem.TransactionType == EnumEquityTransactionType.TransactionWithShareholder)
                    {
                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                        cell.Value = oSP_ChangesInEquity.StockSt; cell.Style.Font.Bold = true; cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                        cell.Value = oItem.TransactionTypeSt; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = oItem.ShareCapital; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = oItem.SharePremium; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = oItem.ExcessOfIssuePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = oItem.CapitalReserve; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = oItem.RevaluationSurplus; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = oItem.FairValueGainOnInvestment; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = oItem.RetainedEarnings; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 2];
                        if (oItem.TransactionType == EnumEquityTransactionType.OpeningBalance)
                        {
                            bIsBold = true;
                            cell.Value = oItem.TransactionTypeSt + Environment.NewLine + oSP_ChangesInEquity.StartDateFullSt;
                            
                        }
                        else if (oItem.TransactionType == EnumEquityTransactionType.ClosingBalance)
                        {
                            bIsBold = true;
                            cell.Value = oItem.TransactionTypeSt + Environment.NewLine + oSP_ChangesInEquity.EndDateFullSt;
                            cell.Style.Font.Bold = bIsBold;
                        }
                        else
                        {
                            bIsBold = false;
                            cell.Value = oItem.TransactionTypeSt;
                        }
                        cell.Style.WrapText = true;
                        cell.Style.Font.Bold = bIsBold;
                        
                         cell.Style.Numberformat.Format = "General";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3];
                        cell.Value = oItem.ShareCapital; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4];
                        cell.Value = oItem.SharePremium; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5];
                        cell.Value = oItem.ExcessOfIssuePrice; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6];
                        cell.Value = oItem.CapitalReserve; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7];
                        cell.Value = oItem.RevaluationSurplus; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8];
                        cell.Value = oItem.FairValueGainOnInvestment; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9];
                        cell.Value = oItem.RetainedEarnings; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10];
                        cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = bIsBold;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                    }

                }
                cell = sheet.Cells[nRowIndex, 2]; cell.Style.WrapText = true;
                cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style= ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style= ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style= ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10];
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style= ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion



                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Changes_In_Equity.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        #endregion

        #region Acoounting Activity
        #region Set Acoounting Activity SessionData
        [HttpPost]
        public ActionResult SetAASessionData(AccountingActivity oAccountingActivity)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oAccountingActivity);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Acoounting Activity Summary
        public ActionResult ViewActivitySummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingActivity oAccountingActivity = new AccountingActivity();
            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            oAccountingActivity = (AccountingActivity)Session[SessionInfo.ParamObj];

            if (oAccountingActivity != null)
            {

                oAccountingActivitys = AccountingActivity.Gets(0, oAccountingActivity.StartDate, oAccountingActivity.EndDate, (int)Session[SessionInfo.currentUserID]);
                oAccountingActivity.AccountingActivitys = new List<AccountingActivity>();
                oAccountingActivity.AccountingActivitys = oAccountingActivitys;
            }
            else
            {
                oAccountingActivity = new AccountingActivity();
                oAccountingActivitys = AccountingActivity.Gets(0, oAccountingActivity.StartDate, oAccountingActivity.EndDate, (int)Session[SessionInfo.currentUserID]);
                oAccountingActivity.AccountingActivitys = new List<AccountingActivity>();
                oAccountingActivity.AccountingActivitys = oAccountingActivitys;
            }
            
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oAccountingActivity);
        }


        public ActionResult PrintActivitySummary(string Params)
        {
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            
            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivitys = AccountingActivity.Gets(0, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);

            rptAccountingActivitySummarys orptAccountingActivitySummarys = new rptAccountingActivitySummarys();
            abytes = orptAccountingActivitySummarys.PrepareReport(oAccountingActivitys, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportActivitySummaryToExcel(string Params)
        {


            #region Dataget
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);


            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivitys = AccountingActivity.Gets(0, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("User Activity Summary");
                sheet.Name = "User Activity Summary";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 10;
                sheet.Column(5).Width = 10;
                sheet.Column(6).Width = 10;
                sheet.Column(7).Width = 10;
                nEndCol = 7;


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "User"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Added"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Edited"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Approved"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;



                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nApproved = 0, nEdited = 0, nTotal = 0, nAdded = 0;
                foreach (AccountingActivity oItem in oAccountingActivitys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.UserName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Added; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Edited; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Approved; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Total; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                string sStartCell = "", sEndCell = "";
                #region Total
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                sStartCell = Global.GetExcelCellName(nStartRow + 1, 4);
                sEndCell = Global.GetExcelCellName(nEndRow, 4);
                cell = sheet.Cells[nRowIndex, 4]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 5);
                sEndCell = Global.GetExcelCellName(nEndRow, 5);
                cell = sheet.Cells[nRowIndex, 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                sEndCell = Global.GetExcelCellName(nEndRow, 6);
                cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 7);
                sEndCell = Global.GetExcelCellName(nEndRow, 7);
                cell = sheet.Cells[nRowIndex, 7]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=User_Activity_Summary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Acoounting Activity BreakDown
        public ActionResult ViewActivityBreakDown(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingActivity oAccountingActivity = new AccountingActivity();
            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            oAccountingActivity = (AccountingActivity)Session[SessionInfo.ParamObj];

            if (oAccountingActivity != null)
            {

                oAccountingActivitys = AccountingActivity.Gets(oAccountingActivity.UserID, oAccountingActivity.StartDate, oAccountingActivity.EndDate, (int)Session[SessionInfo.currentUserID]);
                oAccountingActivity.AccountingActivitys = new List<AccountingActivity>();
                oAccountingActivity.AccountingActivitys = oAccountingActivitys;
            }
            else
            {
                oAccountingActivity = new AccountingActivity();
                oAccountingActivity.AccountingActivitys = new List<AccountingActivity>();
            }
            List<VoucherHistory> oVoucherHistorys = new List<VoucherHistory>();
            #region User
            if (oAccountingActivity.UserID != 0)
            {
                string sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.UserID=" + oAccountingActivity.UserID;
                oVoucherHistorys = VoucherHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            oVoucherHistorys = oVoucherHistorys.Count > 0 ? oVoucherHistorys.GroupBy(x => x.UserID).Select(vh => vh.First()).ToList() : new List<VoucherHistory>();
            ViewBag.User = oVoucherHistorys.Count > 0 ? oVoucherHistorys[0] : new VoucherHistory();
            #endregion
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oAccountingActivity);
        }


        public ActionResult PrintActivityBreakDown(string Params)
        {
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);


            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivitys = AccountingActivity.Gets(nUserID, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);

            rptAccountingActivityBreakDowns orptAccountingActivityBreakDowns = new rptAccountingActivityBreakDowns();
            abytes = orptAccountingActivityBreakDowns.PrepareReport(oAccountingActivitys, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportActivityBreakDownToExcel(string Params)
        {


            #region Dataget
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);


            List<AccountingActivity> oAccountingActivitys = new List<AccountingActivity>();
            AccountingActivity oAccountingActivity = new AccountingActivity();
            oAccountingActivitys = AccountingActivity.Gets(nUserID, dStartDate, dEndDate, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("User Activity BreakDown");
                sheet.Name = "User Activity BreakDown";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                nEndCol = 7;


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Voucher"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Added"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Edited"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Approved"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;



                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nApproved = 0, nEdited = 0, nTotal = 0, nAdded = 0;
                foreach (AccountingActivity oItem in oAccountingActivitys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.VoucherName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.Added; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Edited; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.Approved; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Total; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                string sStartCell = "", sEndCell = "";
                #region Total
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                sStartCell = Global.GetExcelCellName(nStartRow + 1, 4);
                sEndCell = Global.GetExcelCellName(nEndRow, 4);
                cell = sheet.Cells[nRowIndex, 4]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 5);
                sEndCell = Global.GetExcelCellName(nEndRow, 5);
                cell = sheet.Cells[nRowIndex, 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                sEndCell = Global.GetExcelCellName(nEndRow, 6);
                cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sStartCell = Global.GetExcelCellName(nStartRow + 1, 7);
                sEndCell = Global.GetExcelCellName(nEndRow, 7);
                cell = sheet.Cells[nRowIndex, 7]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=User_Activity_BreakDown.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Voucher Activity
        #region Set Voucher History SessionData
        [HttpPost]
        public ActionResult SetVHSessionData(VoucherHistory oVoucherHistory)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oVoucherHistory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ViewVoucherActivity(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            VoucherHistory oVoucherHistory = new VoucherHistory();
            List<VoucherHistory> oVoucherHistorys = new List<VoucherHistory>();
            oVoucherHistory = (VoucherHistory)Session[SessionInfo.ParamObj];

            if (oVoucherHistory != null)
            {
                oVoucherHistorys = VoucherHistory.Gets(oVoucherHistory, (int)Session[SessionInfo.currentUserID]);
                oVoucherHistory.VoucherHistorys = new List<VoucherHistory>();
                oVoucherHistory.VoucherHistorys = oVoucherHistorys;
            }
            else
            {
                oVoucherHistory = new VoucherHistory();
                oVoucherHistory.VoucherHistorys = new List<VoucherHistory>();
            }
            
            #region User
            List<VoucherHistory> oVHs = new List<VoucherHistory>();
            if (oVoucherHistory.UserID != 0)
            {
                sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.UserID=" + oVoucherHistory.UserID;
                oVHs = VoucherHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            oVHs = oVHs.Count > 0 ? oVHs.GroupBy(x => x.UserID).Select(vh => vh.First()).ToList() : new List<VoucherHistory>();
            ViewBag.User = oVHs.Count > 0 ? oVHs[0] : new VoucherHistory();
            #endregion
            #region VoucherType
            oVHs = new List<VoucherHistory>();
            if (oVoucherHistory.VoucherTypeID > 0)
            {
                sSQL = "SELECT * FROM View_VoucherHistory AS VH WHERE VH.VoucherTypeID=" + oVoucherHistory.VoucherTypeID;
                oVHs = VoucherHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            oVHs = oVHs.Count > 0 ? oVHs.GroupBy(x => x.UserID).Select(vh => vh.First()).ToList() : new List<VoucherHistory>();
            ViewBag.VoucherType = oVHs.Count > 0 ? oVHs[0] : new VoucherHistory();
            #endregion
            ViewBag.Company = new Company().Get(1, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oVoucherHistory);
        }


        public ActionResult PrintVoucherActivity(string Params)
        {
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            int nVoucherTypeID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            VoucherHistory oVoucherHistory = new VoucherHistory();
            oVoucherHistory.StartDate = dStartDate;
            oVoucherHistory.EndDate = dEndDate;
            oVoucherHistory.UserID = nUserID;
            oVoucherHistory.VoucherTypeID = nVoucherTypeID;

            List<VoucherHistory> oVoucherHistorys = new List<VoucherHistory>();
            oVoucherHistorys = VoucherHistory.Gets(oVoucherHistory, (int)Session[SessionInfo.currentUserID]);

            rptVoucherActivitys orptVoucherActivitys = new rptVoucherActivitys();
            abytes = orptVoucherActivitys.PrepareReport(oVoucherHistorys, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }


        public void ExportVoucherActivityToExcel(string Params)
        {


            #region Dataget
            int nUserID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            string sHeader = Params.Split('~')[3] == null ? "" : Params.Split('~')[3];
            int nVoucherTypeID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            VoucherHistory oVoucherHistory = new VoucherHistory();
            oVoucherHistory.StartDate = dStartDate;
            oVoucherHistory.EndDate = dEndDate;
            oVoucherHistory.UserID = nUserID;
            oVoucherHistory.VoucherTypeID = nVoucherTypeID;

            List<VoucherHistory> oVoucherHistorys = new List<VoucherHistory>();
            oVoucherHistorys = VoucherHistory.Gets(oVoucherHistory, (int)Session[SessionInfo.currentUserID]);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Voucher Activity");
                sheet.Name = "Voucher Activity";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 25;
                sheet.Column(4).Width = 35;
                sheet.Column(5).Width = 60;
               
                nEndCol = 5;


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;


                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Voucher"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Narration"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                HeaderCell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol];
                nRowIndex++;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nApproved = 0, nEdited = 0, nTotal = 0, nAdded = 0;
                foreach (VoucherHistory oItem in oVoucherHistorys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.VoucherAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Voucher_Activity.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        [HttpPost]
        public JsonResult GetsVHs(VoucherHistory oVoucherHistory)
        {
            List<VoucherHistory> oVoucherHistorys = new List<VoucherHistory>();
            string sSQL="SELECT * FROM View_VoucherHistory AS VH WHERE VH.UserName LIKE '%"+oVoucherHistory.UserName+"%'";
            oVoucherHistorys = VoucherHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            oVoucherHistorys = oVoucherHistorys.GroupBy(x => x.UserID).Select(vh => vh.First()).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region SP_SuspenAccountLogl
        public ActionResult ViewSuspendAccountLog(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            _oSP_GeneralJournal = new SP_GeneralJournal();
            _oSP_GeneralJournals = new List<SP_GeneralJournal>();
            _oSP_GeneralJournals = SP_GeneralJournal.Gets_SuspendALog("", nCompanyID, (int)Session[SessionInfo.currentUserID]);
            double nCuAmount = 0;
            foreach (SP_GeneralJournal oItem in _oSP_GeneralJournals)
            {
                oItem.CreditAmount = Math.Abs(oItem.CreditAmount);
                if (oItem.IsDebit)
                {
                    nCuAmount = nCuAmount + oItem.CreditAmount;
                }
                else
                {
                    nCuAmount = nCuAmount - oItem.CreditAmount;
                }
                oItem.CurrentBalanceSt = Global.MillionFormat(nCuAmount);
            }

            return View(_oSP_GeneralJournals);
        }
        #endregion
   
        #region Profit Loss Account
        public ActionResult ViewProfitLossAccounts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oIncomeStatement = new IncomeStatement();
            _oIncomeStatements = new List<IncomeStatement>();
            _oIncomeStatement.Revenues = IncomeStatement.GetStatements(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.Expenses = IncomeStatement.GetStatements(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.AccountingSessions = AccountingSession.GetRunningFreezeAccountingYear((int)Session[SessionInfo.currentUserID]);
            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            return View(_oIncomeStatement);
        }

        public ActionResult PrepareProfitLossAccount(bool flag, int sid, double ts)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            _oIncomeStatement = new IncomeStatement();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(sid, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatements = IncomeStatement.Gets(0, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, (int)EnumAccountType.Ledger, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.Revenues = IncomeStatement.GetStatements(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.Expenses = IncomeStatement.GetStatements(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oIncomeStatements);

            if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
            }
            else
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
            }
            _oIncomeStatement.SessionDate = oAccountingSession.StartDate.ToString("dd MMM yyyy") + " -to- " + oAccountingSession.EndDate.ToString("dd MMM yyyy");
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oIncomeStatement.Company = oCompany;

            rptIncomeStatement oReport = new rptIncomeStatement();
            if (flag == true)
            {
                byte[] abytes = oReport.PrepareReportDescribe(_oIncomeStatement);
                return File(abytes, "application/pdf");
            }
            else
            {
                byte[] abytes = oReport.PrepareReportShort(_oIncomeStatement, new BusinessUnit());
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrepareProfitLossAccountInXL(bool flag, int sid, double ts)
        {
            _oIncomeStatement = new IncomeStatement();
            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.Get(sid, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatements = IncomeStatement.Gets(0, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, (int)EnumAccountType.Ledger, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.Revenues = IncomeStatement.GetStatements(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.Expenses = IncomeStatement.GetStatements(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oIncomeStatements);

            var stream = new MemoryStream();
            if (flag == true)
            {
                #region Income Statement Descriptive
                var serializer = new XmlSerializer(typeof(List<IncomeStatementXL>));
                //We load the data           
                IncomeStatementXL oIncomeStatementXL = new IncomeStatementXL();
                List<IncomeStatementXL> oIncomeStatementXLs = new List<IncomeStatementXL>();

                #region Revenues
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "Revenues";
                oIncomeStatementXLs.Add(oIncomeStatementXL);
                foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
                {
                    oIncomeStatementXL = new IncomeStatementXL();
                    if (oItem.AccountType == EnumAccountType.Group)
                    {
                        oIncomeStatementXL.Group = oItem.AccountHeadName;
                        oIncomeStatementXL.SubGroup = "";
                        oIncomeStatementXL.Ledger = "";
                        oIncomeStatementXL.LedgerBalance = 0.00;
                        oIncomeStatementXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        oIncomeStatementXL.Group = "";
                        oIncomeStatementXL.SubGroup = oItem.AccountHeadName;
                        oIncomeStatementXL.Ledger = "";
                        oIncomeStatementXL.LedgerBalance = 0.00;
                        oIncomeStatementXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.Ledger)
                    {
                        oIncomeStatementXL.Group = "";
                        oIncomeStatementXL.SubGroup = "";
                        oIncomeStatementXL.Ledger = oItem.AccountHeadName;
                        oIncomeStatementXL.LedgerBalance = oItem.LedgerBalance;
                        oIncomeStatementXL.GroupBalance = 0.00;
                    }
                    oIncomeStatementXLs.Add(oIncomeStatementXL);
                }

                #region Total Assets
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "";
                oIncomeStatementXL.SubGroup = "";
                oIncomeStatementXL.Ledger = "Total Revenues=";
                oIncomeStatementXL.LedgerBalance = 0.00;
                oIncomeStatementXL.GroupBalance = _oIncomeStatement.TotalRevenues;
                oIncomeStatementXLs.Add(oIncomeStatementXL);
                #endregion

                #endregion

                #region Blank
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "";
                oIncomeStatementXL.SubGroup = "";
                oIncomeStatementXL.Ledger = "";
                oIncomeStatementXL.LedgerBalance = 0;
                oIncomeStatementXL.GroupBalance = 0;
                oIncomeStatementXLs.Add(oIncomeStatementXL);

                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "";
                oIncomeStatementXL.SubGroup = "";
                oIncomeStatementXL.Ledger = "";
                oIncomeStatementXL.LedgerBalance = 0;
                oIncomeStatementXL.GroupBalance = 0;
                oIncomeStatementXLs.Add(oIncomeStatementXL);
                #endregion

                #region Expenses
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "Expenses ";
                oIncomeStatementXLs.Add(oIncomeStatementXL);
                foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
                {
                    oIncomeStatementXL = new IncomeStatementXL();
                    if (oItem.AccountType == EnumAccountType.Group)
                    {
                        oIncomeStatementXL.Group = oItem.AccountHeadName;
                        oIncomeStatementXL.SubGroup = "";
                        oIncomeStatementXL.Ledger = "";
                        oIncomeStatementXL.LedgerBalance = 0;
                        oIncomeStatementXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        oIncomeStatementXL.Group = "";
                        oIncomeStatementXL.SubGroup = oItem.AccountHeadName;
                        oIncomeStatementXL.Ledger = "";
                        oIncomeStatementXL.LedgerBalance = 0;
                        oIncomeStatementXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.Ledger)
                    {
                        oIncomeStatementXL.Group = "";
                        oIncomeStatementXL.SubGroup = "";
                        oIncomeStatementXL.Ledger = oItem.AccountHeadName;
                        oIncomeStatementXL.LedgerBalance = oItem.LedgerBalance;
                        oIncomeStatementXL.GroupBalance = 0;
                    }
                    oIncomeStatementXLs.Add(oIncomeStatementXL);
                }

                #region Total Liabilitys
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "";
                oIncomeStatementXL.SubGroup = "";
                oIncomeStatementXL.Ledger = "Total Expenses=";
                oIncomeStatementXL.LedgerBalance = 0;
                oIncomeStatementXL.GroupBalance = _oIncomeStatement.TotalExpenses;
                oIncomeStatementXLs.Add(oIncomeStatementXL);
                #endregion

                #endregion

                #region Blank
                oIncomeStatementXL = new IncomeStatementXL();
                oIncomeStatementXL.Group = "";
                oIncomeStatementXL.SubGroup = "";
                oIncomeStatementXL.Ledger = "";
                oIncomeStatementXL.LedgerBalance = 0;
                oIncomeStatementXL.GroupBalance = 0;
                oIncomeStatementXLs.Add(oIncomeStatementXL);

                #endregion

                #region Net Income or Loss
                if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
                {
                    oIncomeStatementXL = new IncomeStatementXL();
                    oIncomeStatementXL.Group = " Net Income =";
                    oIncomeStatementXL.SubGroup = (_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses).ToString();
                    oIncomeStatementXL.Ledger = "";
                    oIncomeStatementXL.LedgerBalance = 0.00;
                    oIncomeStatementXL.GroupBalance = 0;
                    oIncomeStatementXLs.Add(oIncomeStatementXL);

                    //_oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
                }
                else
                {
                    oIncomeStatementXL = new IncomeStatementXL();
                    oIncomeStatementXL.Group = " Net Loss =";
                    oIncomeStatementXL.SubGroup = (_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues).ToString();
                    oIncomeStatementXL.Ledger = "";
                    oIncomeStatementXL.LedgerBalance = 0.00;
                    oIncomeStatementXL.GroupBalance = 0;
                    oIncomeStatementXLs.Add(oIncomeStatementXL);
                    // _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
                }
                #endregion


                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oIncomeStatementXLs);
                #endregion

            }
            else
            {
                #region Income Statement Short

                var serializer = new XmlSerializer(typeof(List<IncomeStatementShortXL>));

                //We load the data           
                IncomeStatementShortXL oIncomeStatementShortXL = new IncomeStatementShortXL();
                List<IncomeStatementShortXL> oIncomeStatementShortXLs = new List<IncomeStatementShortXL>();

                #region Revenues
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "Revenues";
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
                {
                    oIncomeStatementShortXL = new IncomeStatementShortXL();
                    if (oItem.AccountType == EnumAccountType.Group)
                    {
                        oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                        oIncomeStatementShortXL.SubGroup = "";
                        oIncomeStatementShortXL.LedgerBalance = 0.00;
                        oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        oIncomeStatementShortXL.Group = "";
                        oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                        oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                        oIncomeStatementShortXL.GroupBalance = 0.00;
                    }

                    oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                }

                #region Total Revenues
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "";
                oIncomeStatementShortXL.SubGroup = "Total Revenues=";
                oIncomeStatementShortXL.LedgerBalance = 0.00;
                oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalRevenues;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                #endregion

                #endregion

                #region Blank
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "";
                oIncomeStatementShortXL.SubGroup = "";
                oIncomeStatementShortXL.LedgerBalance = 0;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);

                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "";
                oIncomeStatementShortXL.SubGroup = "";
                oIncomeStatementShortXL.LedgerBalance = 0;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                #endregion

                #region Expenses
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "Expenses ";
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
                {
                    oIncomeStatementShortXL = new IncomeStatementShortXL();
                    if (oItem.AccountType == EnumAccountType.Group)
                    {
                        oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                        oIncomeStatementShortXL.SubGroup = "";
                        oIncomeStatementShortXL.LedgerBalance = 0.00;
                        oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                    }
                    if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        oIncomeStatementShortXL.Group = "";
                        oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                        oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                        oIncomeStatementShortXL.GroupBalance = 0.00;
                    }

                    oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                }

                #region Total Expenses
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "";
                oIncomeStatementShortXL.SubGroup = "Total Expenses =";
                oIncomeStatementShortXL.LedgerBalance = 0;
                oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalExpenses;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                #endregion

                #endregion

                #region Blank
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "";
                oIncomeStatementShortXL.SubGroup = "";
                oIncomeStatementShortXL.LedgerBalance = 0;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                #endregion

                #region Net Income or Loss
                if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
                {
                    oIncomeStatementShortXL = new IncomeStatementShortXL();
                    oIncomeStatementShortXL.Group = " Net Income =";
                    oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses).ToString();
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = 0;
                    oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                    //_oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
                }
                else
                {
                    oIncomeStatementShortXL = new IncomeStatementShortXL();
                    oIncomeStatementShortXL.Group = "  Net Loss =";
                    oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues).ToString();
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = 0;
                    oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                    // _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
                }
                #endregion

                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oIncomeStatementShortXLs);
                #endregion

            }
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "BalanceSheet.xls");

        }

        [HttpPost]
        public JsonResult GetProfitLossStatement(AccountingSession oAccountingSession)
        {
            int nBUID = oAccountingSession.BUID;
            _oIncomeStatement = new IncomeStatement();
            oAccountingSession = oAccountingSession.Get(oAccountingSession.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            _oIncomeStatements = IncomeStatement.Gets(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, 0, (int)EnumAccountType.Ledger, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oIncomeStatement.Revenues = IncomeStatement.GetStatements(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.Expenses = IncomeStatement.GetStatements(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oIncomeStatements);
            _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oIncomeStatements);
            _oIncomeStatement.SessionDate = oAccountingSession.StartDate.ToString("dd MMM yyyy") + " to " + oAccountingSession.EndDate.ToString("dd MMM yyyy");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIncomeStatement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitProfitLossAccount(AccountingSession oAccountingSession)
        {
            Voucher oVoucher = new Voucher();
            try
            {
                oVoucher = oVoucher.CommitProfitLossAccounts(oAccountingSession.BUID, oAccountingSession.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Profit Loss Appropriation Accounts
        public ActionResult ViewProfitLossAppropriationAccounts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Voucher oVoucher = new Voucher();
            oVoucher.VoucherDetailLst = new List<VoucherDetail>();
            oVoucher.AccountingSessions = AccountingSession.GetRunningFreezeAccountingYear((int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(oVoucher);
        }

        [HttpPost]
        public JsonResult GetProfitLossAppropriationAccounts(AccountingSession oAccountingSession)
        {
            Voucher oVoucher = new Voucher();
            VoucherDetail oVoucherDetail = new VoucherDetail();
            VoucherDetail oProfitLossAccountTransaction = new VoucherDetail();
            List<VoucherDetail> oVoucherDetails = new List<VoucherDetail>();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            int nBUID = oAccountingSession.BUID;
            oAccountingSession = oAccountingSession.Get(oAccountingSession.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);

            //Find Profit Loss Account Transaction 
            oProfitLossAccountTransaction = oProfitLossAccountTransaction.GetProfitLossAccountTransaction(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, (int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);

            //Get Already P/L Appropriation Commit Voucher
            oVoucher = oVoucher.GetProfitLossAppropriationAccountVoucher(nBUID, oAccountingSession.StartDate, oAccountingSession.EndDate, (int)Session[SessionInfo.currentUserID]);
            if (oVoucher.VoucherID > 0)
            {
                //Add CompanyID
                oVoucherDetails = VoucherDetail.Gets(oVoucher.VoucherID, (int)Session[SessionInfo.currentUserID]);

                #region Reset Profit Loss Account Transaction  Amount If P/L Appropriation Account Already Commit
                foreach (VoucherDetail oTempItem in oVoucherDetails)
                {
                    if (oTempItem.AccountHeadID == 14)
                    {
                        oTempItem.Amount = oProfitLossAccountTransaction.Amount;
                        oTempItem.DebitAmount = oProfitLossAccountTransaction.Amount;
                        oTempItem.CreditAmount = 0.00;
                    }
                }
                #endregion
            }
            else
            {
                #region Set Profit Loss Account Transaction  Amount If P/L Appropriation Account Not Exists For Selected Session
                oVoucherDetail = new VoucherDetail();
                oVoucherDetail.AccountHeadID = oProfitLossAccountTransaction.AccountHeadID;
                oVoucherDetail.AccountHeadCode = oProfitLossAccountTransaction.AccountHeadCode;
                oVoucherDetail.AccountHeadName = oProfitLossAccountTransaction.AccountHeadName;
                oVoucherDetail.IsDebit = true;
                oVoucherDetail.Amount = oProfitLossAccountTransaction.Amount;
                oVoucherDetail.DebitAmount = oProfitLossAccountTransaction.Amount;
                oVoucherDetail.CreditAmount = 0;
                oVoucherDetails.Add(oVoucherDetail);
                #endregion
            }

            #region Get Rest for Reserve & Drawing  Account Head that are not in already commited P/L Appropriation Transaction
            string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID=12 ";//12=Reserve & Drawing (Fixed Account Head)
            oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ChartsOfAccount oItem in oChartsOfAccounts)
            {
                if (!IsExists(oVoucherDetails, oItem.AccountHeadID))
                {
                    if (oItem.AccountHeadID != 13)  //New Add
                    {
                        oVoucherDetail = new VoucherDetail();
                        oVoucherDetail.AccountHeadID = oItem.AccountHeadID;
                        oVoucherDetail.AccountHeadCode = oItem.AccountCode;
                        oVoucherDetail.AccountHeadName = oItem.AccountHeadName;
                        oVoucherDetail.IsDebit = false;
                        oVoucherDetails.Add(oVoucherDetail);
                    }
                }
            }
            #endregion

            #region Get Retained Earning Account Head Amount
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount = oChartsOfAccount.Get(13, (int)Session[SessionInfo.currentUserID]);//13=Retained Earning (Fixed Account Head)
            oVoucherDetail = new VoucherDetail();
            oVoucherDetail.AccountHeadID = oChartsOfAccount.AccountHeadID;
            oVoucherDetail.AccountHeadCode = oChartsOfAccount.AccountCode;
            oVoucherDetail.AccountHeadName = oChartsOfAccount.AccountHeadName;
            oVoucherDetail.IsDebit = false;
            oVoucherDetail.Amount = oProfitLossAccountTransaction.Amount - GetRetainedEarningAmount(oVoucherDetails);
            oVoucherDetail.CreditAmount = oVoucherDetail.Amount;
            oVoucherDetails.Add(oVoucherDetail);
            #endregion

            oVoucher.VoucherDetailLst = oVoucherDetails;
            oVoucher.AccountingSessionID = oAccountingSession.AccountingSessionID;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private double GetRetainedEarningAmount(List<VoucherDetail> oVoucherDetails)
        {
            double nRetainedEarningAmount = 0;
            foreach (VoucherDetail oItem in oVoucherDetails)
            {
                if (oItem.IsDebit == false)
                {
                    nRetainedEarningAmount = nRetainedEarningAmount + oItem.CreditAmount;
                }
            }
            return nRetainedEarningAmount;
        }

        private bool IsExists(List<VoucherDetail> oVoucherDetails, int nAccountHeadID)
        {
            foreach (VoucherDetail oItem in oVoucherDetails)
            {
                if (oItem.AccountHeadID == nAccountHeadID)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult CommitProfitLossAppropriationAccounts(Voucher oVoucher)
        {
            try
            {
                oVoucher.ProfitLossAppropriationAccountsInString = GetProfitLossAppropriationAccountsInString(oVoucher.VoucherDetailLst);
                oVoucher = oVoucher.CommitProfitLossAppropriationAccounts((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oVoucher = new Voucher();
                oVoucher.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVoucher);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetProfitLossAppropriationAccountsInString(List<VoucherDetail> oVoucherDetails)
        {
            //13=0302001001	Retained Earning (Fixed Account Head)
            //14=P/L Appropriation A/C (Fixed Account Head)
            string sProfitLossAppropriationAccounts = "";
            foreach (VoucherDetail oItem in oVoucherDetails)
            {
                if (oItem.CreditAmount > 0)
                {
                    if (oItem.AccountHeadID != 13)//Retained Earning
                    {
                        if (oItem.AccountHeadID != 14)//P/L Appropriation A/C 
                        {
                            sProfitLossAppropriationAccounts = sProfitLossAppropriationAccounts + oItem.AccountHeadID + "," + oItem.CreditAmount.ToString("0.00") + "~";
                        }
                    }
                }
            }
            if (sProfitLossAppropriationAccounts.Length > 0)
            {
                sProfitLossAppropriationAccounts = sProfitLossAppropriationAccounts.Remove(sProfitLossAppropriationAccounts.Length - 1, 1);
            }
            return sProfitLossAppropriationAccounts;
        }

        #endregion

        #region View Cost Break Down
        public ActionResult ViewCostBreakDown(string sParam, double ts)
        {
            _oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            //int nCboDate = Convert.ToInt32(sParam.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            string BUIDs = sParam.Split('~')[3];
            int nAccountHeadID = Convert.ToInt32(sParam.Split('~')[4]);
            int nCurrencyID = Convert.ToInt32(sParam.Split('~')[5]);
            bool bIsApproved = Convert.ToBoolean(sParam.Split('~')[6]); //Converted into Boolean
            //if (nCboDate == 1) //if select date criteria is Equal 
            //{
            //    dStartDate = dEndDate;
            //}


            #region Check Authorize Business Unit
            string[] aBUs = BUIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }
            #endregion

            _oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.Gets(BUIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            foreach (CostCenterBreakdown oItem in _oCostCenterBreakdown.CostCenterBreakdowns)
            {
                if (oItem.CCID != 0)
                {
                    oCostCenterBreakdowns.Add(oItem);
                }
                else
                {
                    _oCostCenterBreakdown.CCID = oItem.CCID;
                    _oCostCenterBreakdown.CCName = oItem.CCName;
                    _oCostCenterBreakdown.OpeiningValue = oItem.OpeiningValue;
                    _oCostCenterBreakdown.DebitAmount = oItem.DebitAmount;
                    _oCostCenterBreakdown.CreditAmount = oItem.CreditAmount;
                    _oCostCenterBreakdown.ClosingValue = oItem.ClosingValue;
                    _oCostCenterBreakdown.IsDebit = oItem.IsDebit;
                }
            }
            _oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            return PartialView(_oCostCenterBreakdown);
        }

        public ActionResult ViewCostCenterDetails(string sParams, double tsv)
        {
            _oCostCenterBreakdown = new CostCenterBreakdown();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            string BUIDs = sParams.Split('~')[3];
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[5]);
            bool bApprovedBy = Convert.ToBoolean(sParams.Split('~')[6]);
            int nCostCenterID = Convert.ToInt32(sParams.Split('~')[8]);
            #region Check Authorize Business Unit
            string[] aBUs = BUIDs.Split(',');
            for (int i = 0; i < aBUs.Count(); i++)
            {
                if (!BusinessUnit.IsPermittedBU(Convert.ToInt32(aBUs[i]), (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }
            #endregion

            _oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(BUIDs, nAccountHeadID, nCostCenterID, nCurrencyID, dStartDate, dEndDate, bApprovedBy, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oCostCenterBreakdown);
        }

        [HttpPost]
        public JsonResult GetsVoucherByDate(CostCenterBreakdown oCostCenterBreakdown)
        {
            _oCostCenterBreakdown = new CostCenterBreakdown();
            _oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(oCostCenterBreakdown.BusinessUnitIDs, oCostCenterBreakdown.AccountHeadID, oCostCenterBreakdown.CCID, oCostCenterBreakdown.CurrencyID, oCostCenterBreakdown.StartDate, oCostCenterBreakdown.EndDate, oCostCenterBreakdown.IsApproved, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCostCenterBreakdown);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintCostBreakDownXL(string sParams)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            string BUIDs = sParams.Split('~')[3];
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[5]);
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[6]); //Converted into Boolean
            if (nCboDate == 1) //if select date criteria is Equal 
            {
                dStartDate = dEndDate;
            }
            oCostCenterBreakdowns = CostCenterBreakdown.Gets(BUIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.currentUserID]);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<CostCenterBreakdownXL>));

            int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
            CostCenterBreakdownXL oCostCenterBreakdownXL = new CostCenterBreakdownXL();
            List<CostCenterBreakdownXL> oCostCenterBreakdownXLs = new List<CostCenterBreakdownXL>();
            foreach (CostCenterBreakdown oItem in oCostCenterBreakdowns)
            {
                nCount++;
                oCostCenterBreakdownXL = new CostCenterBreakdownXL();
                oCostCenterBreakdownXL.SLNo = nCount.ToString();
                oCostCenterBreakdownXL.CCCode = oItem.CCCode;
                oCostCenterBreakdownXL.CCName = oItem.CCName;
                oCostCenterBreakdownXL.OpeningValueDebitCredit = oItem.IsDrOpenInString;
                oCostCenterBreakdownXL.OpeningValue = oItem.OpeiningValue;
                oCostCenterBreakdownXL.DebitAmount = oItem.DebitAmount;
                oCostCenterBreakdownXL.CreditAmount = oItem.CreditAmount;
                //oCostCenterBreakdownXL.ClosingValueDebitCredit = oItem.IsDrClosingInString;
                oCostCenterBreakdownXL.ClosingValue = oItem.ClosingValue;

                oCostCenterBreakdownXLs.Add(oCostCenterBreakdownXL);
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
            }

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oCostCenterBreakdownXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Sub Ledger Breakdown.xls");
        }
        public ActionResult PrintCostCenterDetailsXL(string sParams)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            _oCostCenterBreakdown = new CostCenterBreakdown();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            string BUIDs = sParams.Split('~')[3];
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[5]);
            bool bApprovedBy = Convert.ToBoolean(sParams.Split('~')[6]);
            int nCostCenterID = Convert.ToInt32(sParams.Split('~')[7]);

            oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(BUIDs, nAccountHeadID, nCostCenterID, nCurrencyID, dStartDate, dEndDate, bApprovedBy, (int)Session[SessionInfo.currentUserID]);


            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<CostCenterDetailXL>));

            int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
            CostCenterDetailXL oCostCenterDetailXL = new CostCenterDetailXL();
            List<CostCenterDetailXL> oCostCenterDetailXLs = new List<CostCenterDetailXL>();
            foreach (CostCenterBreakdown oItem in oCostCenterBreakdowns)
            {
                nCount++;
                oCostCenterDetailXL = new CostCenterDetailXL();
                oCostCenterDetailXL.SLNo = nCount.ToString();
                oCostCenterDetailXL.VoucherDate = oItem.VoucherDateInString;
                oCostCenterDetailXL.VoucherNo = oItem.VoucherNo;
                oCostCenterDetailXL.DebitAmount = oItem.DebitAmount;
                oCostCenterDetailXL.CreditAmount = oItem.CreditAmount;
                //oCostCenterDetailXL.ClosingValueDebitCredit = oItem.IsDrClosingInString;
                oCostCenterDetailXL.ClosingValue = oItem.ClosingValue;
                oCostCenterDetailXL.Particulars = oItem.AccountHeadName;
                oCostCenterDetailXLs.Add(oCostCenterDetailXL);
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
            }

            #region Total
            oCostCenterDetailXL = new CostCenterDetailXL();
            oCostCenterDetailXL.SLNo = "";
            oCostCenterDetailXL.VoucherDate = "";
            oCostCenterDetailXL.VoucherNo = "";
            oCostCenterDetailXL.Particulars = "Total :";
            oCostCenterDetailXL.DebitAmount = nTotalDebit;
            oCostCenterDetailXL.CreditAmount = nTotalCredit;
            oCostCenterDetailXL.ClosingValueDebitCredit = "";
            oCostCenterDetailXL.ClosingValue = 0;
            oCostCenterDetailXLs.Add(oCostCenterDetailXL);
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oCostCenterDetailXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "View Sub Ledger Details.xls");
        }
        #endregion

        #region View Voucher Bill Break Down
        public ActionResult ViewVoucherBillBreakDown(string sParam, double ts)
        {
            _oVoucherBillBreakDown = new VoucherBillBreakDown();
            List<VoucherBillBreakDown> oVoucherBillBreakDowns = new List<VoucherBillBreakDown>();
            int nCboDate = Convert.ToInt32(sParam.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sParam.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParam.Split('~')[4]);
            bool bIsApproved = Convert.ToBoolean(sParam.Split('~')[5]); //Converted into Boolean
            if (nCboDate == 1) //if select date criteria is Equal 
            {
                dStartDate = dEndDate;
            }
            _oVoucherBillBreakDown.VoucherBillBreakDowns = VoucherBillBreakDown.Gets(nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);

            foreach (VoucherBillBreakDown oItem in _oVoucherBillBreakDown.VoucherBillBreakDowns)
            {
                if (oItem.VoucherBillID != 0)
                {
                    oVoucherBillBreakDowns.Add(oItem);
                }
                else
                {
                    _oVoucherBillBreakDown.VoucherBillID = oItem.VoucherBillID;
                    _oVoucherBillBreakDown.BillNo = oItem.BillNo;
                    _oVoucherBillBreakDown.OpeningValue = oItem.OpeningValue;
                    _oVoucherBillBreakDown.DebitAmount = oItem.DebitAmount;
                    _oVoucherBillBreakDown.CreditAmount = oItem.CreditAmount;
                    _oVoucherBillBreakDown.ClosingValue = oItem.ClosingValue;
                    _oVoucherBillBreakDown.IsDrOpen = oItem.IsDrOpen;
                    _oVoucherBillBreakDown.IsDrClosing = oItem.IsDrClosing;
                }
            }
            _oVoucherBillBreakDown.VoucherBillBreakDowns = oVoucherBillBreakDowns;
            return PartialView(_oVoucherBillBreakDown);
        }

        public ActionResult ViewVoucherBillDetails(string sParams, double tsv)
        {
            _oVoucherBillBreakDown = new VoucherBillBreakDown();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool nApprovedBy = Convert.ToBoolean(sParams.Split('~')[5]);
            int nVoucherBillID = Convert.ToInt32(sParams.Split('~')[7]);
            _oVoucherBillBreakDown.VoucherBillBreakDowns = VoucherBillBreakDown.GetsForVoucherBill(nAccountHeadID, nVoucherBillID, nCurrencyID, dStartDate, dEndDate, nApprovedBy, (int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oVoucherBillBreakDown);
        }

        [HttpPost]
        public JsonResult GetsVoucherByDateVB(VoucherBillBreakDown oVoucherBillBreakDown)
        {
            _oVoucherBillBreakDown = new VoucherBillBreakDown();
            _oVoucherBillBreakDown.VoucherBillBreakDowns = VoucherBillBreakDown.GetsForVoucherBill(oVoucherBillBreakDown.AccountHeadID, oVoucherBillBreakDown.VoucherBillID, oVoucherBillBreakDown.CurrencyID, oVoucherBillBreakDown.StartDate, oVoucherBillBreakDown.EndDate, oVoucherBillBreakDown.IsApproved, (int)Session[SessionInfo.CurrentCompanyID], (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBillBreakDown);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
    }
}