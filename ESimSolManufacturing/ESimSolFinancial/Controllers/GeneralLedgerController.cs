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
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSolFinancial.Hubs;


namespace ESimSolFinancial.Controllers
{
    public class GeneralLedgerController : Controller
    {
        #region Declaration
        List<SP_GeneralLedger> _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
        SP_GeneralLedger _oSP_GeneralLedger = new SP_GeneralLedger();
        CostCenterBreakdown _oCostCenterBreakdown = new CostCenterBreakdown();
        VoucherBillBreakDown _oVoucherBillBreakDown = new VoucherBillBreakDown();
        #endregion


        #region New Version

        #region GL
        private bool ValidateInputForGeneralLedger(SP_GeneralLedger oSP_GeneralLedger)
        {
            _oSP_GeneralLedger = new SP_GeneralLedger();
            if (oSP_GeneralLedger.AccountHeadID <= 0)
            {
                _oSP_GeneralLedger.ErrorMessage = "Please select an account head";
                return false;
            }
            if (oSP_GeneralLedger.EndDate < oSP_GeneralLedger.StartDate)
            {
                _oSP_GeneralLedger.ErrorMessage = "End Date must be grater/equal then start date";
                return false;
            }
            return true;
        }
        #region Get Data According to Configuration
        private List<SP_GeneralLedger> GetsListAccordingToConfig(SP_GeneralLedger oSP_GeneralLedger, List<ACConfig> oACConfigs)
        {
            if (oACConfigs == null && oACConfigs.Count <= 0) { return oSP_GeneralLedger.SP_GeneralLedgerList; }
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            List<SP_GeneralLedger> oNewSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();

            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherBillTransaction> oSubLedgerBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            List<VoucherCheque> oSLVoucherCheques = new List<VoucherCheque>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<VOReference> oVOReferences = new List<VOReference>();
            //List<VoucherReference> oVoucherReferences = new List<VoucherReference>();
            string sSQL = "";
            if (oSP_GeneralLedger.IsApproved == true)
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VOReference AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND ISNULL(TT.ApprovedBy,0) != 0 AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVOReferences= VOReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VOReference AS TT WHERE TT.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.TransactionDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))";
                oVOReferences= VOReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


            foreach (SP_GeneralLedger oSPGL in oSP_GeneralLedger.SP_GeneralLedgerList)
            {
                oNewSP_GeneralLedgers.Add(oSPGL);
                foreach (ACConfig oItem in oACConfigs)
                {
                    bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                    #region Account Head Narration
                    if (oItem.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration && bConfigureValue == true)
                    {
                        oNewSP_GeneralLedger = GetNarration(oItem, oSPGL);
                        oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                        if (oNewSP_GeneralLedger.IsNullOrNot == false)
                        {
                            oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                        }
                    }
                    #endregion
                    #region Voucher Narration
                    else if (oItem.ConfigureType == EnumConfigureType.GLVoucherNarration && bConfigureValue == true)
                    {
                        oNewSP_GeneralLedger = GetNarration(oItem, oSPGL);
                        oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                        if (oNewSP_GeneralLedger.IsNullOrNot == false)
                        {
                            oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                        }
                    }
                    #endregion
                    #region Cost Center Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLCC && bConfigureValue == true)
                    {
                        if (oCostCenterTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetCC(oCostCenterTransactions, (int)oSPGL.VoucherDetailID, oSPGL.VoucherDate, oACConfigs, oSubLedgerBillTransactions, oSLVoucherCheques);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region Voucher Bill Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLBT && bConfigureValue == true)
                    {
                        if (oVoucherBillTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetBT(oVoucherBillTransactions, (int)oSPGL.VoucherDetailID, 0, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region VP (IR) Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLIR && bConfigureValue == true)
                    {
                        if (oVPTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetIR(oVPTransactions, (int)oSPGL.VoucherDetailID, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region Voucher Cheque
                    else if (oItem.ConfigureType == EnumConfigureType.GLVC && bConfigureValue == true)
                    {
                        if (oVoucherCheques.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetVC(oVoucherCheques, (int)oSPGL.VoucherDetailID, 0, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region Order Reference
                    else if (oItem.ConfigureType == EnumConfigureType.GLOR && bConfigureValue == true)
                    {
                        if (oVOReferences.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetOR(oVOReferences, (int)oSPGL.VoucherDetailID, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                }
            }
            return oNewSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetCC(List<CostCenterTransaction> oCostCenterTransactions, int nVoucherDetailID, DateTime dVoucherDate, List<ACConfig> oACConfigs, List<VoucherBillTransaction> oSubLedgerBillTransactions, List<VoucherCheque> oSLVoucherCheques)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (CostCenterTransaction oItem in oCostCenterTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    //oSP_GeneralLedger.ConfigTitle = "CC ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLCC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = "Subledger : " + oItem.CostCenterName + ", Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                    foreach (ACConfig oACConfig in oACConfigs)
                    {
                        if (oACConfig.ConfigureType == EnumConfigureType.GLBT && oACConfig.ConfigureValue == "1")
                        {
                            if (oSubLedgerBillTransactions.Count > 0)
                            {
                                List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                                oTempSP_GeneralLedgers = GetBT(oSubLedgerBillTransactions, (int)oItem.VoucherDetailID, oItem.CCTID, dVoucherDate);
                                oSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                            }
                        }
                        if (oACConfig.ConfigureType == EnumConfigureType.GLVC && oACConfig.ConfigureValue == "1")
                        {
                            if (oSLVoucherCheques.Count > 0)
                            {
                                List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                                oTempSP_GeneralLedgers = GetVC(oSLVoucherCheques, (int)oItem.VoucherDetailID, oItem.CCTID, dVoucherDate);
                                oSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                            }
                        }
                    }

                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetBT(List<VoucherBillTransaction> oVoucherBillTransactions, int nVoucherDetailID, int nCCTID, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    //oSP_GeneralLedger.ConfigTitle = oItem.CCTID > 0 ? "SL BT " : "BT ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLBT;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = (oItem.CCTID > 0 ? "SL Bill : " : "Bill : ") + oItem.ExplanationTransactionTypeInString + ", Bill No : " + oItem.BillNo + ", Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + ", Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetIR(List<VPTransaction> oVPTransactions, int nVoucherDetailID, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VPTransaction oItem in oVPTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    //oSP_GeneralLedger.ConfigTitle = "IR ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLIR;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = "Item :" + oItem.ProductName + ", WU : " + oItem.WorkingUnitName + ", MUnit : " + oItem.MUnitName + ", Qty : " + Global.MillionFormat(oItem.Qty) + ", Unit Price : " + Global.MillionFormat(oItem.UnitPrice) + ", Amount : " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetVC(List<VoucherCheque> oVoucherCheques, int nVoucherDetailID, int nCCTID, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VoucherCheque oItem in oVoucherCheques)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    //oSP_GeneralLedger.ConfigTitle = oItem.CCTID > 0 ? "SL Cheque" : "Cheque";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLVC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = (oItem.CCTID > 0 ? "SL Cheque No : " : "Cheque No : ") + oItem.ChequeNo + ", Account No :" + oItem.AccountNo + ", Amount : " + Global.MillionFormat(oItem.Amount);
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetOR(List<VOReference> oVOReferences, int nVoucherDetailID, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VOReference oItem in oVOReferences)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDebit == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();                    
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLOR;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = (oItem.CCTID > 0 ? "SL Ref No : " : "Ref No : ")+ oItem.RefNo + ", Order No : " + oItem.OrderNo + ", Date : " + oItem.OrderDateSt + ", Amount : " + oItem.AmountInCurrencySt + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private SP_GeneralLedger GetNarration(ACConfig oACConfig, SP_GeneralLedger oSP_GeneralLedger)
        {
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            if (oACConfig.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
            {
                //oNewSP_GeneralLedger.ConfigTitle = " Head Wise Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.Narration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
                oNewSP_GeneralLedger.ConfigType = EnumConfigureType.GLAccHeadWiseNarration;
            }
            else if (oACConfig.ConfigureType == EnumConfigureType.GLVoucherNarration)
            {
                //oNewSP_GeneralLedger.ConfigTitle = " Voucher Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.VoucherNarration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
                oNewSP_GeneralLedger.ConfigType = EnumConfigureType.GLVoucherNarration;
            }
            return oNewSP_GeneralLedger;
        }
        #endregion
        public SP_GeneralLedger NewObject(SP_GeneralLedger oGL, double nOpenningBalance, EnumDisplayMode eEnumDisplayMode, int nComponentType)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();

            if (eEnumDisplayMode == EnumDisplayMode.MonthlyView)
            {
                oSP_GeneralLedger.ConfigTitle = oGL.VoucherDate.ToString("MMM yyyy");
                oSP_GeneralLedger.OpenningBalance = nOpenningBalance;
                oSP_GeneralLedger.StartDate = new DateTime(oGL.VoucherDate.Year, oGL.VoucherDate.Month, 1);
                if (oGL.VoucherDate.Month == 12)
                {
                    oSP_GeneralLedger.EndDate = (new DateTime(oGL.VoucherDate.Year+1, 1, 1)).AddDays(-1);
                }
                else
                {
                    oSP_GeneralLedger.EndDate = (new DateTime(oGL.VoucherDate.Year, oGL.VoucherDate.Month + 1, 1)).AddDays(-1);
                }
            }
            else if (eEnumDisplayMode == EnumDisplayMode.DateView)
            {
                oSP_GeneralLedger.ConfigTitle = oGL.VoucherDate.ToString("dd MMM yyyy");
                oSP_GeneralLedger.OpenningBalance = nOpenningBalance;
                oSP_GeneralLedger.StartDate = oGL.VoucherDate;
                oSP_GeneralLedger.EndDate = oGL.VoucherDate;
            }
            else
            {
                oSP_GeneralLedger.VoucherID = oGL.VoucherID;
                oSP_GeneralLedger.VoucherNo = oGL.VoucherNo;
                oSP_GeneralLedger.OpenningBalance = oGL.OpenningBalance;
                oSP_GeneralLedger.Particulars = oGL.Particulars;
                oSP_GeneralLedger.CurrentBalance = oGL.CurrentBalance;
                oSP_GeneralLedger.ConfigTitle = oGL.ConfigTitle;
            }
            oSP_GeneralLedger.VoucherDate = oGL.VoucherDate;
            oSP_GeneralLedger.DebitAmount = oGL.DebitAmount;
            oSP_GeneralLedger.CreditAmount = oGL.CreditAmount;
            oSP_GeneralLedger.SP_GeneralLedgerList = oGL.SP_GeneralLedgerList;
            return oSP_GeneralLedger;
        }
        #region Month And Date Wise Vouchers
        private List<SP_GeneralLedger> GetVouchers(DateTime dDate, List<SP_GeneralLedger> oSP_GeneralLedgers, EnumDisplayMode sEnumDisplayMode, int nComponentType)
        {
            List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

            foreach (SP_GeneralLedger oItem in oSP_GeneralLedgers)
            {
                if (sEnumDisplayMode == EnumDisplayMode.MonthlyView)
                {
                    if (oItem.VoucherDate.ToString("MMM yyyy") == dDate.ToString("MMM yyyy"))
                    {
                        oTempGeneralLedger = NewObject(oItem, 0.00, EnumDisplayMode.None, nComponentType);
                        oTempSP_GeneralLedgers.Add(oTempGeneralLedger);
                    }
                }
                else if (sEnumDisplayMode == EnumDisplayMode.DateView)
                {
                    if (oItem.VoucherDate.ToString("dd MMM yyyy") == dDate.ToString("dd MMM yyyy"))
                    {
                        oTempGeneralLedger = NewObject(oItem, 0.00, EnumDisplayMode.None, nComponentType);
                        oTempSP_GeneralLedgers.Add(oTempGeneralLedger);
                    }
                }
            }
            return oTempSP_GeneralLedgers;
        }
        #endregion
        private double GetCurrentBalance(double nOpenningBalance, double nDebitAmount, double nCreditAmount, int nComponentType)
        {
            double nCurrentBalance = 0;
            if (nComponentType == (int)EnumComponentType.Asset || nComponentType == (int)EnumComponentType.Expenditure)
            {
                nCurrentBalance = nOpenningBalance + nDebitAmount - nCreditAmount;
            }
            else
            {
                nCurrentBalance = nOpenningBalance + nCreditAmount - nDebitAmount;
            }
            return nCurrentBalance;
        }
        [HttpPost]
        public ActionResult SetSessionData(SP_GeneralLedger oSP_GeneralLedger)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSP_GeneralLedger);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Transactions
       
        [HttpPost]
        public JsonResult GetSessionData(SP_GeneralLedger oSP_GeneralLedger)
        {
            List<SP_GeneralLedger> oGeneralLedgers = new List<SP_GeneralLedger>();
            try
            {
                oGeneralLedgers = (List<SP_GeneralLedger>)Session[SessionInfo.SearchData];
            }
            catch (Exception ex)
            {
                oGeneralLedgers = new List<SP_GeneralLedger>();
            }
            var jsonResult = Json(oGeneralLedgers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ViewGeneralLedger(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            bool bACConfing = false;
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            AccountingSession oAccountingSession = new AccountingSession();
            SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oGeneralLedgers = new List<SP_GeneralLedger>();
            try
            {
                oGeneralLedger = (SP_GeneralLedger)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oGeneralLedger = null;
            }

            if (oGeneralLedger != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oGeneralLedger.BusinessUnitIDs.Split(',');
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

                Thread.Sleep(1000);
                ProgressHub.SendMessage("Getting Ledger Data", 10, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
                if (oGeneralLedger.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oGeneralLedger.StartDate = oAccountingSession.StartDate;
                    oGeneralLedger.EndDate = DateTime.Now;
                }
                oChartsOfAccount = oChartsOfAccount.Get(oGeneralLedger.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //string sSQL = GetSQL(oGeneralLedger);
                oGeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oGeneralLedger.AccountHeadID, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oGeneralLedger.CurrencyID, oGeneralLedger.IsApproved, oGeneralLedger.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                Thread.Sleep(2000);
                ProgressHub.SendMessage("Mapping Ledger Data", 40, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                oGeneralLedger.SP_GeneralLedgerList = oGeneralLedgers;
                oGeneralLedger.SP_GeneralLedgerList = this.GetsListAccordingToConfig(oGeneralLedger, oGeneralLedger.ACConfigs);
                Thread.Sleep(3000);
                ProgressHub.SendMessage("Transfering Ledger Data", 70, ((User)Session[SessionInfo.CurrentUser]).UserID);
                

                #region Display Mode
                if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");
                    double nOpeningBalance = 0;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        if (oItem.ConfigType == EnumConfigureType.None)
                        {
                            #region Individual Month
                            if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
                            {
                                if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            #region Individual Date
                            else if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                            {
                                if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            dLedgerDate = oItem.VoucherDate;
                            nOpeningBalance = oItem.CurrentBalance;
                        }
                    }


                    if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                    {
                        foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                        {
                            oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oGeneralLedger.SP_GeneralLedgerList, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                        }
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        double nDrAmount = 0;
                        double nCrAmount = 0;
                        foreach (SP_GeneralLedger oItemVoucher in oItem.DisplayVouchers)
                        {
                            nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                            nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                        }
                        oItem.DebitAmount = nDrAmount;
                        oItem.CreditAmount = nCrAmount;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, nDrAmount, nCrAmount, oChartsOfAccount.ComponentID);
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #region Individual Week
                else if (oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dStartDate = oGeneralLedger.StartDate;
                    int nCountWeek = 1;
                    while (dStartDate <= oGeneralLedger.EndDate.Date)
                    {
                        oTempGeneralLedger = new SP_GeneralLedger();
                        oTempGeneralLedger.StartDate = dStartDate;
                        oTempGeneralLedger.EndDate = dStartDate.AddDays(6);
                        oTempGeneralLedger.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralLedger.StartDate.ToString("MMM") + " " + oTempGeneralLedger.StartDate.ToString("dd") + " - " + oTempGeneralLedger.EndDate.ToString("MMM") + " " + oTempGeneralLedger.EndDate.ToString("dd") + ")";
                        oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                        dStartDate = dStartDate.AddDays(7);
                        nCountWeek++;
                    }

                    double nOpeningBalance = 0;
                    int nCount = 1;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        SP_GeneralLedger oTempGL = new SP_GeneralLedger();
                        DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                        double nDebitAmount = 0;
                        double nCreditAmount = 0;
                        foreach (SP_GeneralLedger oGL in oGeneralLedger.SP_GeneralLedgerList)
                        {

                            if (oGL.VoucherDate >= oItem.StartDate && oGL.VoucherDate <= oItem.EndDate)
                            {
                                oTempGL = new SP_GeneralLedger();
                                oTempGL.ConfigTitle = oGL.VoucherDate.ToString("dd MMM yyyy");
                                oTempGL.VoucherID = oGL.VoucherID;
                                oTempGL.VoucherNo = oGL.VoucherNo;
                                oTempGL.Particulars = oGL.Particulars;
                                oTempGL.OpenningBalance = oGL.OpenningBalance;
                                oTempGL.VoucherDate = oGL.VoucherDate;
                                oTempGL.DebitAmount = oGL.DebitAmount;
                                oTempGL.CreditAmount = oGL.CreditAmount;
                                oTempGL.CurrentBalance = oGL.CurrentBalance;
                                oTempGL.SP_GeneralLedgerList = oGL.SP_GeneralLedgerList;
                                oItem.DisplayVouchers.Add(oTempGL);
                                nDebitAmount = nDebitAmount + oTempGL.DebitAmount;
                                nCreditAmount = nCreditAmount + oTempGL.CreditAmount;
                            }
                        }
                        oItem.DebitAmount = nDebitAmount;
                        oItem.CreditAmount = nCreditAmount;
                        oItem.OpenningBalance = nOpeningBalance;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, oItem.DebitAmount, oItem.CreditAmount, oChartsOfAccount.ComponentID);
                        nOpeningBalance = oItem.CurrentBalance;
                        nCount++;
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #endregion
                #endregion
                                
                Thread.Sleep(1000);
                ProgressHub.SendMessage("Loading Ledger Data", 100, ((User)Session[SessionInfo.CurrentUser]).UserID);



            }
            else
            {
                oGeneralLedger = new SP_GeneralLedger();
            }


            oGeneralLedger.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));
            oGeneralLedger.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.COA = oChartsOfAccount;

            #region Business Unit
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;
            #endregion
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
            ViewBag.Company = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            this.Session.Remove(SessionInfo.ParamObj);

            this.Session.Remove(SessionInfo.SearchData);
            this.Session.Add(SessionInfo.SearchData, oGeneralLedger.SP_GeneralLedgerList);
            oGeneralLedger.SP_GeneralLedgerList = new List<SP_GeneralLedger>();

            return View(oGeneralLedger);
        }





        #endregion

        #region GL Summary
        public ActionResult ViewGeneralLedgerSummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            AccountingSession oAccountingSession = new AccountingSession();
            SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oGeneralLedgers = new List<SP_GeneralLedger>();
            try
            {
                oGeneralLedger = (SP_GeneralLedger)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oGeneralLedger = null;
            }
            if (oGeneralLedger != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oGeneralLedger.BusinessUnitIDs.Split(',');
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

                Thread.Sleep(1000);
                ProgressHub.SendMessage("Getting Ledger Data", 10, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oGeneralLedger.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oGeneralLedger.StartDate = oAccountingSession.StartDate;
                    oGeneralLedger.EndDate = DateTime.Now;
                }
                oChartsOfAccount = oChartsOfAccount.Get(oGeneralLedger.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //string sSQL = GetSQL(oGeneralLedger);
                oGeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oGeneralLedger.AccountHeadID, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oGeneralLedger.CurrencyID, oGeneralLedger.IsApproved, oGeneralLedger.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                Thread.Sleep(2000);
                ProgressHub.SendMessage("Mapping Ledger Data", 40, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGeneralLedger.SP_GeneralLedgerList = oGeneralLedgers;
                oGeneralLedger.SP_GeneralLedgerList = this.GetsListAccordingToConfig(oGeneralLedger, oGeneralLedger.ACConfigs);

                Thread.Sleep(3000);
                ProgressHub.SendMessage("Transfering Ledger Data", 70, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Display Mode
                if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");
                    double nOpeningBalance = 0;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        if (oItem.ConfigType == EnumConfigureType.None)
                        {
                            #region Individual Month
                            if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
                            {
                                if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            #region Individual Date
                            else if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                            {
                                if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            dLedgerDate = oItem.VoucherDate;
                            nOpeningBalance = oItem.CurrentBalance;
                        }
                    }


                    if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                    {
                        foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                        {
                            oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oGeneralLedger.SP_GeneralLedgerList, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                        }
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        double nDrAmount = 0;
                        double nCrAmount = 0;
                        foreach (SP_GeneralLedger oItemVoucher in oItem.DisplayVouchers)
                        {
                            if (oItemVoucher.VoucherID > 0)
                            {
                                nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                                nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                            }
                        }
                        oItem.DebitAmount = nDrAmount;
                        oItem.CreditAmount = nCrAmount;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, nDrAmount, nCrAmount, oChartsOfAccount.ComponentID);
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #region Individual Week
                else if (oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dStartDate = oGeneralLedger.StartDate;
                    int nCountWeek = 1;
                    while (dStartDate <= oGeneralLedger.EndDate.Date)
                    {
                        oTempGeneralLedger = new SP_GeneralLedger();
                        oTempGeneralLedger.StartDate = dStartDate;
                        oTempGeneralLedger.EndDate = dStartDate.AddDays(6);
                        oTempGeneralLedger.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralLedger.StartDate.ToString("MMM") + " " + oTempGeneralLedger.StartDate.ToString("dd") + " - " + oTempGeneralLedger.EndDate.ToString("MMM") + " " + oTempGeneralLedger.EndDate.ToString("dd") + ")";
                        oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                        dStartDate = dStartDate.AddDays(7);
                        nCountWeek++;
                    }

                    double nOpeningBalance = 0;
                    int nCount = 1;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        SP_GeneralLedger oTempGL = new SP_GeneralLedger();
                        DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                        double nDebitAmount = 0;
                        double nCreditAmount = 0;
                        foreach (SP_GeneralLedger oGL in oGeneralLedger.SP_GeneralLedgerList)
                        {

                            if (oGL.VoucherDate >= oItem.StartDate && oGL.VoucherDate <= oItem.EndDate)
                            {
                                oTempGL = new SP_GeneralLedger();
                                oTempGL.ConfigTitle = oGL.VoucherDate.ToString("dd MMM yyyy");
                                oTempGL.VoucherID = oGL.VoucherID;
                                oTempGL.VoucherNo = oGL.VoucherNo;
                                oTempGL.Particulars = oGL.Particulars;
                                oTempGL.OpenningBalance = oGL.OpenningBalance;
                                oTempGL.VoucherDate = oGL.VoucherDate;
                                oTempGL.DebitAmount = oGL.DebitAmount;
                                oTempGL.CreditAmount = oGL.CreditAmount;
                                oTempGL.CurrentBalance = oGL.CurrentBalance;
                                oTempGL.SP_GeneralLedgerList = oGL.SP_GeneralLedgerList;
                                oItem.DisplayVouchers.Add(oTempGL);
                                if (oTempGL.VoucherID > 0)
                                {
                                    nDebitAmount = nDebitAmount + oTempGL.DebitAmount;
                                    nCreditAmount = nCreditAmount + oTempGL.CreditAmount;
                                }
                            }
                        }
                        oItem.DebitAmount = nDebitAmount;
                        oItem.CreditAmount = nCreditAmount;
                        oItem.OpenningBalance = nOpeningBalance;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, oItem.DebitAmount, oItem.CreditAmount, oChartsOfAccount.ComponentID);
                        nOpeningBalance = oItem.CurrentBalance;
                        nCount++;
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #endregion
                #endregion

                Thread.Sleep(1000);
                ProgressHub.SendMessage("Loading Ledger Data", 100, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oGeneralLedger = new SP_GeneralLedger();
            }

            oGeneralLedger.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));
            oGeneralLedger.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.COA = oChartsOfAccount;

            #region Business Unit
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;
            #endregion
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
            ViewBag.Company = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oGeneralLedger);
        }
        #endregion
        public ActionResult PrintGeneralLedger()
        {
           
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            AccountingSession oAccountingSession = new AccountingSession();
            SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oGeneralLedgers = new List<SP_GeneralLedger>();
            try
            {
                oGeneralLedger = (SP_GeneralLedger)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oGeneralLedger = null;
            }
            if (oGeneralLedger != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oGeneralLedger.BusinessUnitIDs.Split(',');
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

                if (oGeneralLedger.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oGeneralLedger.StartDate = oAccountingSession.StartDate;
                    oGeneralLedger.EndDate = DateTime.Now;
                }
                oChartsOfAccount = oChartsOfAccount.Get(oGeneralLedger.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //string sSQL = GetSQL(oGeneralLedger);
                oGeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oGeneralLedger.AccountHeadID, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oGeneralLedger.CurrencyID, oGeneralLedger.IsApproved, oGeneralLedger.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGeneralLedger.SP_GeneralLedgerList = oGeneralLedgers;
                oGeneralLedger.SP_GeneralLedgerList = this.GetsListAccordingToConfig(oGeneralLedger, oGeneralLedger.ACConfigs);

                #region Display Mode
                if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");
                    double nOpeningBalance = 0;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        if (oItem.ConfigType == EnumConfigureType.None)
                        {
                            #region Individual Month
                            if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
                            {
                                if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            #region Individual Date
                            else if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                            {
                                if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            dLedgerDate = oItem.VoucherDate;
                            nOpeningBalance = oItem.CurrentBalance;
                        }
                    }


                    if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                    {
                        foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                        {
                            oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oGeneralLedger.SP_GeneralLedgerList, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                        }
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        double nDrAmount = 0;
                        double nCrAmount = 0;
                        foreach (SP_GeneralLedger oItemVoucher in oItem.DisplayVouchers)
                        {
                            if (oItemVoucher.VoucherID > 0)
                            {
                                nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                                nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                            }
                        }
                        oItem.DebitAmount = nDrAmount;
                        oItem.CreditAmount = nCrAmount;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, nDrAmount, nCrAmount, oChartsOfAccount.ComponentID);
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #region Individual Week
                else if (oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dStartDate = oGeneralLedger.StartDate;
                    int nCountWeek = 1;
                    while (dStartDate <= oGeneralLedger.EndDate.Date)
                    {
                        oTempGeneralLedger = new SP_GeneralLedger();
                        oTempGeneralLedger.StartDate = dStartDate;
                        oTempGeneralLedger.EndDate = dStartDate.AddDays(6);
                        oTempGeneralLedger.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralLedger.StartDate.ToString("MMM") + " " + oTempGeneralLedger.StartDate.ToString("dd") + " - " + oTempGeneralLedger.EndDate.ToString("MMM") + " " + oTempGeneralLedger.EndDate.ToString("dd") + ")";
                        oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                        dStartDate = dStartDate.AddDays(7);
                        nCountWeek++;
                    }

                    double nOpeningBalance = 0;
                    int nCount = 1;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        SP_GeneralLedger oTempGL = new SP_GeneralLedger();
                        DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                        double nDebitAmount = 0;
                        double nCreditAmount = 0;
                        foreach (SP_GeneralLedger oGL in oGeneralLedger.SP_GeneralLedgerList)
                        {

                            if (oGL.VoucherDate >= oItem.StartDate && oGL.VoucherDate <= oItem.EndDate)
                            {
                                oTempGL = new SP_GeneralLedger();
                                oTempGL.ConfigTitle = oGL.VoucherDate.ToString("dd MMM yyyy");
                                oTempGL.VoucherID = oGL.VoucherID;
                                oTempGL.VoucherNo = oGL.VoucherNo;
                                oTempGL.Particulars = oGL.Particulars;
                                oTempGL.OpenningBalance = oGL.OpenningBalance;
                                oTempGL.VoucherDate = oGL.VoucherDate;
                                oTempGL.DebitAmount = oGL.DebitAmount;
                                oTempGL.CreditAmount = oGL.CreditAmount;
                                oTempGL.CurrentBalance = oGL.CurrentBalance;
                                oTempGL.SP_GeneralLedgerList = oGL.SP_GeneralLedgerList;
                                oItem.DisplayVouchers.Add(oTempGL);
                                if (oTempGL.VoucherID > 0)
                                {
                                    nDebitAmount = nDebitAmount + oTempGL.DebitAmount;
                                    nCreditAmount = nCreditAmount + oTempGL.CreditAmount;
                                }
                            }
                        }
                        oItem.DebitAmount = nDebitAmount;
                        oItem.CreditAmount = nCreditAmount;
                        oItem.OpenningBalance = nOpeningBalance;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, oItem.DebitAmount, oItem.CreditAmount, oChartsOfAccount.ComponentID);
                        nOpeningBalance = oItem.CurrentBalance;
                        nCount++;
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #endregion
                #endregion

                Company oCompany = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
             
                if (!string.IsNullOrEmpty(oGeneralLedger.BusinessUnitIDs) && oGeneralLedger.BusinessUnitIDs!="0")//not empty or not group accounts
                {
                    List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                    string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN ("+oGeneralLedger.BusinessUnitIDs+")";
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

                Currency oCurrency = new Currency().Get(oGeneralLedger.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCurrency.CurrencyID <= 0) { oCurrency = oCurrency.Get(oCompany.BaseCurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID); }

                oGeneralLedger.Company = oCompany;
                oGeneralLedger.Currency = oCurrency;
            }
            else
            {
                oGeneralLedger = new SP_GeneralLedger();
            }
           
     

            this.Session.Remove(SessionInfo.ParamObj);
            if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView || oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView || oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
            {
                int nCount = oGeneralLedger.SP_GeneralLedgerList.Count();
                Double nCurrentBalance = 0;
                if (nCount > 0)
                {
                    nCurrentBalance = oGeneralLedger.SP_GeneralLedgerList[nCount - 1].CurrentBalance;
                }
                
                rptGeneralLedgersDateWeekMonthView orptGeneralLedgers = new rptGeneralLedgersDateWeekMonthView();
                byte[] abytes = orptGeneralLedgers.PrepareReport(oGeneralLedger, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oChartsOfAccount.AccountCode, oChartsOfAccount.AccountHeadName, nCurrentBalance, oGeneralLedger.IsApproved);
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SP_GeneralLedger> oTemp = new List<SP_GeneralLedger>();
                oTemp = oGeneralLedger.SP_GeneralLedgerList.Where(x => x.VoucherID > 0).OrderBy(x => x.VoucherDate).ToList();
                Double nCurrentBalance = 0;
                if (oTemp.Count > 0)
                {
                    nCurrentBalance = oTemp[oTemp.Count - 1].CurrentBalance;
                }


                rptGeneralLedgersTwo orptGeneralLedgers = new rptGeneralLedgersTwo();
                byte[] abytes = orptGeneralLedgers.PrepareReport(oGeneralLedger, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oChartsOfAccount.AccountCode, oChartsOfAccount.AccountHeadName, nCurrentBalance, oGeneralLedger.IsApproved);
                return File(abytes, "application/pdf");   
            }
        }
        public void ExportGLToExcel()
        {
            
            #region Data Get
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            AccountingSession oAccountingSession = new AccountingSession();
            SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oGeneralLedgers = new List<SP_GeneralLedger>();
            oGeneralLedger = (SP_GeneralLedger)Session[SessionInfo.ParamObj];

            if (oGeneralLedger != null)
            {
                if (oGeneralLedger.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oGeneralLedger.StartDate = oAccountingSession.StartDate;
                    oGeneralLedger.EndDate = DateTime.Now;
                }
                oChartsOfAccount = oChartsOfAccount.Get(oGeneralLedger.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oGeneralLedger.AccountHeadID, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oGeneralLedger.CurrencyID, oGeneralLedger.IsApproved, oGeneralLedger.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGeneralLedger.SP_GeneralLedgerList = oGeneralLedgers;
                oGeneralLedger.SP_GeneralLedgerList = this.GetsListAccordingToConfig(oGeneralLedger, oGeneralLedger.ACConfigs);
                #region Display Mode
                if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");
                    double nOpeningBalance = 0;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        if (oItem.ConfigType == EnumConfigureType.None)
                        {
                            #region Individual Month
                            if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
                            {
                                if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            #region Individual Date
                            else if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                            {
                                if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                {
                                    oTempGeneralLedger = NewObject(oItem, nOpeningBalance, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                                    oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                                }
                            }
                            #endregion
                            dLedgerDate = oItem.VoucherDate;
                            nOpeningBalance = oItem.CurrentBalance;
                        }
                    }


                    if (oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView || oGeneralLedger.DisplayMode == EnumDisplayMode.DateView)
                    {
                        foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                        {
                            oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oGeneralLedger.SP_GeneralLedgerList, oGeneralLedger.DisplayMode, oChartsOfAccount.ComponentID);
                        }
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        double nDrAmount = 0;
                        double nCrAmount = 0;
                        foreach (SP_GeneralLedger oItemVoucher in oItem.DisplayVouchers)
                        {
                            if (oItemVoucher.VoucherID > 0)
                            {
                                nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                                nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                            }
                        }
                        oItem.DebitAmount = nDrAmount;
                        oItem.CreditAmount = nCrAmount;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, nDrAmount, nCrAmount, oChartsOfAccount.ComponentID);
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #region Individual Week
                else if (oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView)
                {
                    List<SP_GeneralLedger> oDisplayModeWiseGeneralLedgers = new List<SP_GeneralLedger>();
                    SP_GeneralLedger oTempGeneralLedger = new SP_GeneralLedger();

                    DateTime dStartDate = oGeneralLedger.StartDate;
                    int nCountWeek = 1;
                    while (dStartDate <= oGeneralLedger.EndDate.Date)
                    {
                        oTempGeneralLedger = new SP_GeneralLedger();
                        oTempGeneralLedger.StartDate = dStartDate;
                        oTempGeneralLedger.EndDate = dStartDate.AddDays(6);
                        oTempGeneralLedger.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralLedger.StartDate.ToString("MMM") + " " + oTempGeneralLedger.StartDate.ToString("dd") + " - " + oTempGeneralLedger.EndDate.ToString("MMM") + " " + oTempGeneralLedger.EndDate.ToString("dd") + ")";
                        oDisplayModeWiseGeneralLedgers.Add(oTempGeneralLedger);
                        dStartDate = dStartDate.AddDays(7);
                        nCountWeek++;
                    }

                    double nOpeningBalance = 0;
                    int nCount = 1;
                    if (oGeneralLedger.SP_GeneralLedgerList.Count > 0)
                    {
                        nOpeningBalance = oGeneralLedger.SP_GeneralLedgerList[0].CurrentBalance;
                    }

                    foreach (SP_GeneralLedger oItem in oDisplayModeWiseGeneralLedgers)
                    {
                        SP_GeneralLedger oTempGL = new SP_GeneralLedger();
                        DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                        double nDebitAmount = 0;
                        double nCreditAmount = 0;
                        foreach (SP_GeneralLedger oGL in oGeneralLedger.SP_GeneralLedgerList)
                        {

                            if (oGL.VoucherDate >= oItem.StartDate && oGL.VoucherDate <= oItem.EndDate)
                            {
                                oTempGL = new SP_GeneralLedger();
                                oTempGL.ConfigTitle = oGL.VoucherDate.ToString("dd MMM yyyy");
                                oTempGL.VoucherID = oGL.VoucherID;
                                oTempGL.VoucherNo = oGL.VoucherNo;
                                oTempGL.Particulars = oGL.Particulars;
                                oTempGL.OpenningBalance = oGL.OpenningBalance;
                                oTempGL.VoucherDate = oGL.VoucherDate;
                                oTempGL.DebitAmount = oGL.DebitAmount;
                                oTempGL.CreditAmount = oGL.CreditAmount;
                                oTempGL.CurrentBalance = oGL.CurrentBalance;
                                oTempGL.SP_GeneralLedgerList = oGL.SP_GeneralLedgerList;
                                oItem.DisplayVouchers.Add(oTempGL);
                                if (oTempGL.VoucherID > 0)
                                {
                                    nDebitAmount = nDebitAmount + oTempGL.DebitAmount;
                                    nCreditAmount = nCreditAmount + oTempGL.CreditAmount;
                                }
                            }
                        }
                        oItem.DebitAmount = nDebitAmount;
                        oItem.CreditAmount = nCreditAmount;
                        oItem.OpenningBalance = nOpeningBalance;
                        oItem.CurrentBalance = GetCurrentBalance(oItem.OpenningBalance, oItem.DebitAmount, oItem.CreditAmount, oChartsOfAccount.ComponentID);
                        nOpeningBalance = oItem.CurrentBalance;
                        nCount++;
                    }
                    oGeneralLedger.SP_GeneralLedgerList = oDisplayModeWiseGeneralLedgers;
                }
                #endregion
                #endregion
                Company oCompany = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (oGeneralLedger.BusinessUnitID > 0)
                //{
                //    BusinessUnit oBusinessUnit = new BusinessUnit();
                //    oBusinessUnit = oBusinessUnit.Get(oGeneralLedger.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                //    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                //}
                if (!string.IsNullOrEmpty(oGeneralLedger.BusinessUnitIDs) && oGeneralLedger.BusinessUnitIDs != "0")//not empty or not group accounts
                {
                    List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                    string sSQL = "SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (" + oGeneralLedger.BusinessUnitIDs + ")";
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


                Currency oCurrency = new Currency().Get(oGeneralLedger.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCurrency.CurrencyID <= 0) { oCurrency = oCurrency.Get(oCompany.BaseCurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID); }

                oGeneralLedger.Company = oCompany;
                oGeneralLedger.Currency = oCurrency;
            }
            else
            {
                oGeneralLedger = new SP_GeneralLedger();
            }
            
            this.Session.Remove(SessionInfo.ParamObj);
            #endregion

            if (oGeneralLedger.DisplayMode == EnumDisplayMode.DateView || oGeneralLedger.DisplayMode == EnumDisplayMode.WeeklyView || oGeneralLedger.DisplayMode == EnumDisplayMode.MonthlyView)
            {
                #region Export To Excel
                int nCount = oGeneralLedger.SP_GeneralLedgerList.Count();
                Double nCurrentBalance = 0;
                if (nCount > 0)
                {
                    nCurrentBalance = oGeneralLedger.SP_GeneralLedgerList[nCount - 1].CurrentBalance;
                }
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("General Ledger");
                    sheet.Name = "General Ledger";
                    sheet.Column(2).Width = 20;
                    sheet.Column(3).Width = 20;
                    sheet.Column(4).Width = 20;
                    sheet.Column(5).Width = 20;
                    sheet.Column(6).Width = 20;
                    nEndCol = 6;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Phone + ";  " + oGeneralLedger.Company.Email + ";  " + oGeneralLedger.Company.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "General Ledger"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.IsApproved ? "(Approved Only)" : "(UnApproved And Approved)"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;
                    #endregion

                    #region AccountHead Info
                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Ledger Name :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Merge = true;
                    cell.Value = oChartsOfAccount.AccountHeadName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4];
                    cell.Value = "Report Date :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 6]; cell.Merge = true;
                    cell.Value = oGeneralLedger.StartDateInString + " to " + oGeneralLedger.EndDateInString; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Ledger No :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Merge = true;
                    cell.Value = oChartsOfAccount.AccountCode; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 6]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Current Balance :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Merge = true;
                    cell.Value = nCurrentBalance; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[nRowIndex, 4];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 6]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    nRowIndex = nRowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartRow = nRowIndex;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Crdit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data
                    nCount = 0;
                    //Double nCreditClosing = 0, nDebitClosing = 0;
                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        nCount++;

                        if (oGeneralLedger.ACConfigs != null && oGeneralLedger.ACConfigs.Count > 0)
                        {
                            if (oItem.VoucherID > 0 && nCount > 1)
                            {
                                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true; cell.Style.WrapText = true;
                                cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                                nRowIndex++;
                            }
                        }

                        cell = sheet.Cells[nRowIndex, 2];
                        cell.Value = oItem.VoucherDateInString;
                        cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OpenningBalance; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CurrentBalance; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
               
                        nEndRow = nRowIndex;
                        nRowIndex++;

                        //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                        //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                    }

                    #endregion

                    #region Total
                    string sStartCell = "", sEndCell = "";

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 5);
                    sEndCell = Global.GetExcelCellName(nEndRow, 5);
                    cell = sheet.Cells[nRowIndex, 4]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                    cell = sheet.Cells[nRowIndex, 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=General_Ledger.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else
            {

                #region Export To Excel

                List<SP_GeneralLedger> oTemp = new List<SP_GeneralLedger>();
                oTemp = oGeneralLedger.SP_GeneralLedgerList.Where(x => x.VoucherID > 0).OrderBy(x => x.VoucherDate).ToList();
                Double nCurrentBalance = 0;
                if (oTemp.Count > 0)
                {
                    nCurrentBalance = oTemp[oTemp.Count - 1].CurrentBalance;
                }

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("General Ledger");
                    sheet.Name = "General Ledger";
                    sheet.Column(2).Width = 18;
                    sheet.Column(3).Width = 20;
                    sheet.Column(4).Width = 50;
                    sheet.Column(5).Width = 20;
                    sheet.Column(6).Width = 20;
                    sheet.Column(7).Width = 20;
                    nEndCol = 7;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.Company.Phone + ";  " + oGeneralLedger.Company.Email + ";  " + oGeneralLedger.Company.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "General Ledger"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oGeneralLedger.IsApproved ? "(Approved Only)" : "(UnApproved And Approved)"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;
                    #endregion

                    #region AccountHead Info
                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Ledger Name :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3, nRowIndex, 4]; cell.Merge = true;
                    cell.Value = oChartsOfAccount.AccountHeadName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5];
                    cell.Value = "Report Date :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oGeneralLedger.StartDateInString + " to " + oGeneralLedger.EndDateInString; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Ledger No :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3, nRowIndex, 4]; cell.Merge = true;
                    cell.Value = oChartsOfAccount.AccountCode; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2];
                    cell.Value = "Current Balance :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3, nRowIndex, 4]; cell.Merge = true;
                    cell.Value = nCurrentBalance; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[nRowIndex, 5];
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.None;

                    nRowIndex = nRowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartRow = nRowIndex;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Crdit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                    HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data
                    int nCount = 0;
                    //Double nCreditClosing = 0, nDebitClosing = 0;
                    foreach (SP_GeneralLedger oItem in oGeneralLedger.SP_GeneralLedgerList)
                    {
                        nCount++;

                        if (oGeneralLedger.ACConfigs != null && oGeneralLedger.ACConfigs.Count > 0)
                        {
                            if (oItem.VoucherID > 0 && nCount > 1)
                            {
                                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true; cell.Style.WrapText = true;
                                cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                                nRowIndex++;
                            }
                        }

                        if (oItem.VoucherID > 0 || nCount == 1)
                        {
                            cell = sheet.Cells[nRowIndex, 2];
                            if (nCount == 1)
                            {
                                cell.Value = oGeneralLedger.StartDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                cell.Value = oItem.VoucherDateInString;
                            }
                            cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            //cell.Style.Numberformat.Format = "d-mmm-yy";

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = this.GetParticualrAsTable(oItem.Particulars); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.CurrentBalance; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }
                        else if (nCount > 1)
                        {
                            cell = sheet.Cells[nRowIndex, 2];
                            cell.Value = oItem.VoucherDateInString;
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[nRowIndex, 3, nRowIndex, 7]; cell.Merge = true; cell.Style.WrapText = true;
                            cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }



                        nEndRow = nRowIndex;
                        nRowIndex++;

                        //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                        //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                    }

                    #endregion

                    #region Total
                    string sStartCell = "", sEndCell = "";

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 5);
                    sEndCell = Global.GetExcelCellName(nEndRow, 5);
                    cell = sheet.Cells[nRowIndex, 5]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=General_Ledger.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private string GetParticualrAsTable(string sParticulars)
        {
            string sReturn = "";
            if (sParticulars != null)
            {                
                string[] aParticulars = sParticulars.Split(';');
                for (var i = 0; i < aParticulars.Length; i++)
                {
                    if (aParticulars.Length == (i + 1))
                    {
                        sReturn = sReturn + aParticulars[i];
                    }
                    else
                    {
                        sReturn = sReturn + aParticulars[i] + "\n";
                    }
                }
            }
            else
            {
                sReturn = "";
            }
            return sReturn;
        }
        #endregion

        #region Monthly Summary
        [HttpPost]
        public ActionResult SetMonthlyGLSessionData(GLMonthlySummary oGLMonthlySummary)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oGLMonthlySummary);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewGLMonthlySummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            AccountingSession oAccountingSession = new AccountingSession();
            GLMonthlySummary oGLMonthlySummary = new GLMonthlySummary();
            List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();
            try
            {
                oGLMonthlySummary = (GLMonthlySummary)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oGLMonthlySummary = null;
            }

            if (oGLMonthlySummary != null)
            {
                #region Check Authorize Business Unit
                string[] aBUs = oGLMonthlySummary.BusinessUnitIDs.Split(',');
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

                if (oGLMonthlySummary.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oGLMonthlySummary.StartDate = oAccountingSession.StartDate;
                    //oGLMonthlySummary.EndDate = DateTime.Now;
                }
                oGLMonthlySummary.StartDate = new DateTime(oGLMonthlySummary.StartDate.Year, oGLMonthlySummary.StartDate.Month, 1);
                oGLMonthlySummary.EndDate = new DateTime(oGLMonthlySummary.EndDate.AddMonths(1).Year, oGLMonthlySummary.EndDate.AddMonths(1).Month, 1).AddDays(-1);
                oGLMonthlySummarys = GLMonthlySummary.Gets(oGLMonthlySummary.AccountHeadID, oGLMonthlySummary.StartDate, oGLMonthlySummary.EndDate, oGLMonthlySummary.CurrencyID, oGLMonthlySummary.IsApproved, oGLMonthlySummary.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGLMonthlySummary.GLMonthlySummarys = new List<GLMonthlySummary>();
                oGLMonthlySummary.GLMonthlySummarys = oGLMonthlySummarys;
            }
            else
            {
                oGLMonthlySummary = new GLMonthlySummary();
                oGLMonthlySummary.GLMonthlySummarys = new List<GLMonthlySummary>();
            }

            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oGLMonthlySummary.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Company = new Company().Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            #region Business Unit
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;
            #endregion


            this.Session.Remove(SessionInfo.ParamObj);
            return View(oGLMonthlySummary);
        }

        [HttpPost]
        public JsonResult GetsGLMonthlySummary(GLMonthlySummary oGLMonthlySummary)
        {
            string sjson = "";


            List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();

            oGLMonthlySummarys = GLMonthlySummary.Gets(oGLMonthlySummary.AccountHeadID, oGLMonthlySummary.StartDate, oGLMonthlySummary.EndDate, oGLMonthlySummary.CurrencyID, oGLMonthlySummary.IsApproved, oGLMonthlySummary.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oGLMonthlySummary.GLMonthlySummarys = new List<GLMonthlySummary>();
            oGLMonthlySummary.GLMonthlySummarys = oGLMonthlySummarys;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            sjson = serializer.Serialize((object)oGLMonthlySummary);

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintGLMonthlySummary(string Params)
        {
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
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
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();

            oGLMonthlySummarys = GLMonthlySummary.Gets(nAccountHeadID, dStartDate, dEndDate, nCurrencyID, bIsApproved, nBusinessUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptGLMonthlySummarys oReport = new rptGLMonthlySummarys();
            abytes = oReport.PrepareReport(oGLMonthlySummarys, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportGLMSToExcel(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (nBusinessUnitID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }


            List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();

            oGLMonthlySummarys = GLMonthlySummary.Gets(nAccountHeadID, dStartDate, dEndDate, nCurrencyID, bIsApproved, nBusinessUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Monthly Summary");
                sheet.Name = "Monthly Summary";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 30;


                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 4, nRowIndex, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Transaction Summary"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex + 1, 4]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex + 1, 5]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 6, nRowIndex + 1, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 6];
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (GLMonthlySummary oItem in oGLMonthlySummarys)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.NameOfMonth; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ClosingAmount; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    nDebitClosing = nDebitClosing + oItem.DebitAmount;
                    nCreditClosing = nCreditClosing + oItem.CreditAmount;
                }
                nEndRow = nRowIndex - 1;
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 7];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 6];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Monthly_Summary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion
        #endregion

        public ActionResult GeneralLedgerCostCenterWise(int id, DateTime date1, DateTime date2, int DateType)
        {
            string sDateRange = "";
            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.AccountHeadID = id;
            oSP_GeneralLedger.StartDate = date1;
            oSP_GeneralLedger.EndDate = date2;
            if (DateType == 1)
            {
                oSP_GeneralLedger.EndDate = oSP_GeneralLedger.StartDate;
                sDateRange = "Date on " + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy");
            }
            else
            {
                sDateRange = oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + " to " + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy");
            }
            if (!ValidateInputForGeneralLedger(oSP_GeneralLedger))
            {
                string sMessage = _oSP_GeneralLedger.ErrorMessage;
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                ChartsOfAccount oCoA = new ChartsOfAccount();
                oCoA = oCoA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oSP_GeneralLedger.CompanyID = (int)Session[SessionInfo.CurrentCompanyID];
                oSP_GeneralLedgers = SP_GeneralLedger.GetsForReport(oSP_GeneralLedger, ((User)Session[SessionInfo.CurrentUser]).UserID);
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                if (oSP_GeneralLedgers.Count > 0)
                {
                    oSP_GeneralLedgers[0].AccountCode = oCoA.AccountHeadName;
                }
                rptVoucherReportReport_Statistics orptGeneralLedgers = new rptVoucherReportReport_Statistics();
                byte[] abytes = orptGeneralLedgers.PrepareReport_CostcenterWises(oSP_GeneralLedgers, oCompany, "Cost Breakup of Ledger", sDateRange);
                return File(abytes, "application/pdf");
            }
        }

        #region GeneralLedgerInXL
        public ActionResult GeneralLedgerInXL(int id, DateTime date1, DateTime date2, int nCurrencyID, bool IsApproved)
        {
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.AccountHeadID = id;
            oSP_GeneralLedger.StartDate = date1;
            oSP_GeneralLedger.EndDate = date2;
            oSP_GeneralLedger.CurrencyID = nCurrencyID;
            oSP_GeneralLedger.IsApproved = IsApproved;
            if (!ValidateInputForGeneralLedger(oSP_GeneralLedger))
            {
                string sMessage = _oSP_GeneralLedger.ErrorMessage;
                //return this.ViewPdf("", "rptGLedger", _oSP_GeneralLedger);
                //return this.ViewPdf("", "rptGLedger", _oSP_GeneralLedger, 40, 40, 40, 40, false);
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
                string sSQL = GetSQL(oSP_GeneralLedger);
                _oSP_GeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oSP_GeneralLedger.AccountHeadID, oSP_GeneralLedger.StartDate, oSP_GeneralLedger.EndDate, oSP_GeneralLedger.CurrencyID, oSP_GeneralLedger.IsApproved, "0", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oSP_GeneralLedgers.Count > 0)
                {
                    oGeneralLedger.Particulars = "Opening Balance";
                    oGeneralLedger.IsOpenningBalance = true;
                    oGeneralLedger.CurrentBalance = _oSP_GeneralLedgers[0].OpenningBalance;
                    oSP_GeneralLedgers.Add(oGeneralLedger);
                    oSP_GeneralLedgers.AddRange(_oSP_GeneralLedgers);
                }
                var stream = new MemoryStream();
                var serializer = new XmlSerializer(typeof(List<GeneralLedgerXL>));

                //We load the data

                int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
                GeneralLedgerXL oGeneralLedgerXL = new GeneralLedgerXL();
                List<GeneralLedgerXL> oGeneralLedgerXLs = new List<GeneralLedgerXL>();
                foreach (SP_GeneralLedger oItem in _oSP_GeneralLedgers)
                {
                    nCount++;
                    oGeneralLedgerXL = new GeneralLedgerXL();
                    oGeneralLedgerXL.SLNo = nCount.ToString();
                    oGeneralLedgerXL.VoucherDate = oItem.VoucherDateInString;
                    oGeneralLedgerXL.VoucherNo = oItem.VoucherNo;
                    oGeneralLedgerXL.Particulars = oItem.Particulars;
                    oGeneralLedgerXL.DebitAmount = oItem.DebitAmount;
                    oGeneralLedgerXL.CreditAmount = oItem.CreditAmount;
                    oGeneralLedgerXL.CurrentBalance = oItem.CurrentBalance;
                    oGeneralLedgerXLs.Add(oGeneralLedgerXL);
                    nTotalDebit = nTotalDebit + oItem.DebitAmount;
                    nTotalCredit = nTotalCredit + oItem.CreditAmount;
                }

                #region Total
                oGeneralLedgerXL = new GeneralLedgerXL();
                oGeneralLedgerXL.SLNo = "";
                oGeneralLedgerXL.VoucherDate = "";
                oGeneralLedgerXL.VoucherNo = "";
                oGeneralLedgerXL.Particulars = "Total :";
                oGeneralLedgerXL.DebitAmount = nTotalDebit;
                oGeneralLedgerXL.CreditAmount = nTotalCredit;
                oGeneralLedgerXL.CurrentBalance = 0;
                oGeneralLedgerXLs.Add(oGeneralLedgerXL);
                #endregion

                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oGeneralLedgerXLs);
                stream.Position = 0;

                //We return the XML from the memory as a .xls file
                return File(stream, "application/vnd.ms-excel", "General Ledger.xls");
            }
        }
        public ActionResult PrintGeneralLedgerInXL(string sParams)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.AccountHeadID = Convert.ToInt32(sParams.Split('~')[0]);
            //int nDateSearch = Convert.ToInt32(sParams.Split('~')[1]);
            oSP_GeneralLedger.StartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            oSP_GeneralLedger.EndDate = Convert.ToDateTime(sParams.Split('~')[3]);
            oSP_GeneralLedger.CurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            oSP_GeneralLedger.IsApproved = Convert.ToBoolean(sParams.Split('~')[5]);
            oSP_GeneralLedger.Narration = Convert.ToString(sParams.Split('~')[6]);
            oSP_GeneralLedger.BusinessUnitID = Convert.ToInt32(sParams.Split('~')[7]);
            //if (nDateSearch == 1) // EqualTo
            //{
            //    oSP_GeneralLedger.EndDate = oSP_GeneralLedger.StartDate;
            //}
            bool bACConfing = false;
            List<ACConfig> oACConfigs = new List<ACConfig>();
            string sSQLACConfig = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GLAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GLVC;
            oACConfigs = ACConfig.Gets(sSQLACConfig, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (ACConfig oItem in oACConfigs)
            {
                if (oItem.ConfigureType != EnumConfigureType.GLVoucherNarration)
                {
                    bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                    if (bConfigureValue == true)
                    {
                        bACConfing = true;
                        break;
                    }
                }
            }


            if (!ValidateInputForGeneralLedger(oSP_GeneralLedger))
            {
                string sMessage = _oSP_GeneralLedger.ErrorMessage;
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                string sSQL = GetSQL(oSP_GeneralLedger);
                _oSP_GeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oSP_GeneralLedger.AccountHeadID, oSP_GeneralLedger.StartDate, oSP_GeneralLedger.EndDate, oSP_GeneralLedger.CurrencyID, oSP_GeneralLedger.IsApproved, ((int)Session[SessionInfo.CurrentCompanyID]).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
                var stream = new MemoryStream();
                var serializer = new XmlSerializer(typeof(List<GeneralLedgerXL>));

                oSP_GeneralLedger.SP_GeneralLedgerList = _oSP_GeneralLedgers;
                if (bACConfing == true)
                {
                    _oSP_GeneralLedgers = GetsListAccordingToConfig(oSP_GeneralLedger, oACConfigs);
                }

                //We load the data

                int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
                GeneralLedgerXL oGeneralLedgerXL = new GeneralLedgerXL();
                List<GeneralLedgerXL> oGeneralLedgerXLs = new List<GeneralLedgerXL>();
                foreach (SP_GeneralLedger oItem in _oSP_GeneralLedgers)
                {
                    nCount++;
                    oGeneralLedgerXL = new GeneralLedgerXL();
                    oGeneralLedgerXL.SLNo = nCount.ToString();
                    oGeneralLedgerXL.VoucherDate = oItem.VoucherDateInString;
                    oGeneralLedgerXL.VoucherNo = oItem.VoucherNo;
                    oGeneralLedgerXL.Particulars = oItem.Particulars;
                    oGeneralLedgerXL.DebitAmount = oItem.DebitAmount;
                    oGeneralLedgerXL.CreditAmount = oItem.CreditAmount;
                    oGeneralLedgerXL.CurrentBalance = oItem.CurrentBalance;
                    oGeneralLedgerXLs.Add(oGeneralLedgerXL);
                    nTotalDebit = nTotalDebit + oItem.DebitAmount;
                    nTotalCredit = nTotalCredit + oItem.CreditAmount;
                }

                #region Total
                oGeneralLedgerXL = new GeneralLedgerXL();
                oGeneralLedgerXL.SLNo = "";
                oGeneralLedgerXL.VoucherDate = "";
                oGeneralLedgerXL.VoucherNo = "";
                oGeneralLedgerXL.Particulars = "Total :";
                oGeneralLedgerXL.DebitAmount = nTotalDebit;
                oGeneralLedgerXL.CreditAmount = nTotalCredit;
                oGeneralLedgerXL.CurrentBalance = 0;
                oGeneralLedgerXLs.Add(oGeneralLedgerXL);
                #endregion

                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oGeneralLedgerXLs);
                stream.Position = 0;

                //We return the XML from the memory as a .xls file
                return File(stream, "application/vnd.ms-excel", "General Ledger.xls");
            }
        }
        #endregion

        #region View Cost Break Down
        public ActionResult ViewCostBreakDown(string sParam, double ts)
        {
            _oCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            int nCboDate = Convert.ToInt32(sParam.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            string BUIDs = sParam.Split('~')[3];
            int nAccountHeadID = Convert.ToInt32(sParam.Split('~')[4]);
            int nCurrencyID = Convert.ToInt32(sParam.Split('~')[5]);
            bool bIsApproved = Convert.ToBoolean(sParam.Split('~')[6]); //Converted into Boolean
            if (nCboDate == 1) //if select date criteria is Equal 
            {
                dStartDate = dEndDate;
            }


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

            _oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.Gets(BUIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, ((User)Session[SessionInfo.CurrentUser]).UserID);

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
                    //_oCostCenterBreakdown.IsDrClosing = oItem.IsDrClosing;
                }
            }
            _oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
            return PartialView(_oCostCenterBreakdown);
        }

        public ActionResult PrintCostCenterDetailsXL(string sParams)
        {
            string BUIDs = sParams.Split('~')[0];
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]);
            int nCCID = Convert.ToInt32(sParams.Split('~')[6]);

            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(BUIDs, nAccountHeadID, nCCID, nCurrencyID, dStartDate, dEndDate, bIsApproved, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<CostCenterDetailXL>));

            int nCount = 0;
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
            }
            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oCostCenterDetailXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "View Sub Ledger Details.xls");
        }

        public ActionResult PrintCostBreakDownXL(string sParams)
        {
            string BUIDs = sParams.Split('~')[0];
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]);

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


            List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
            oCostCenterBreakdowns = CostCenterBreakdown.Gets(BUIDs, nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, ((User)Session[SessionInfo.CurrentUser]).UserID);

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
            return File(stream, "application/vnd.ms-excel", "View Cost Breakdown.xls");
        }

        #endregion

        #region View Voucher Bill Break Down
        public ActionResult ViewVoucherBillBreakDown(string sParams, double ts)
        {
            _oVoucherBillBreakDown = new VoucherBillBreakDown();
            List<VoucherBillBreakDown> oVoucherBillBreakDowns = new List<VoucherBillBreakDown>();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]); //Converted into Boolean
            if (nCboDate == 1) //if select date criteria is Equal 
            {
                dStartDate = dEndDate;
            }
            _oVoucherBillBreakDown.VoucherBillBreakDowns = VoucherBillBreakDown.Gets(nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);

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
        public ActionResult PrintVoucherBillBreakDownXL(string sParams)
        {
            _oVoucherBillBreakDown = new VoucherBillBreakDown();
            List<VoucherBillBreakDown> oVoucherBillBreakDowns = new List<VoucherBillBreakDown>();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool bIsApproved = Convert.ToBoolean(sParams.Split('~')[5]); //Converted into Boolean
            if (nCboDate == 1) //if select date criteria is Equal 
            {
                dStartDate = dEndDate;
            }
            _oVoucherBillBreakDown.VoucherBillBreakDowns = VoucherBillBreakDown.Gets(nAccountHeadID, nCurrencyID, dStartDate, dEndDate, bIsApproved, (int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<VoucherBillBreakDownXL>));

            //We load the data

            int nCount = 0;
            VoucherBillBreakDownXL oVoucherBillBreakDownXL = new VoucherBillBreakDownXL();
            List<VoucherBillBreakDownXL> oVoucherBillBreakDownXLs = new List<VoucherBillBreakDownXL>();
            foreach (VoucherBillBreakDown oItem in _oVoucherBillBreakDown.VoucherBillBreakDowns)
            {
                nCount++;
                oVoucherBillBreakDownXL = new VoucherBillBreakDownXL();
                oVoucherBillBreakDownXL.SLNo = nCount.ToString();
                oVoucherBillBreakDownXL.BillNo = oItem.BillNo;
                oVoucherBillBreakDownXL.OpeningValueDebitCredit = oItem.IsDrOpenInString;
                oVoucherBillBreakDownXL.OpeningValue = oItem.OpeningValue;
                oVoucherBillBreakDownXL.DebitAmount = oItem.DebitAmount;
                oVoucherBillBreakDownXL.CreditAmount = oItem.CreditAmount;
                oVoucherBillBreakDownXL.ClosingValueDebitCredit = oItem.IsDrClosingInString;
                oVoucherBillBreakDownXL.ClosingValue = oItem.ClosingValue;
                oVoucherBillBreakDownXLs.Add(oVoucherBillBreakDownXL);
            }

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oVoucherBillBreakDownXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Voucher Bill Breakdown.xls");
        }

        public ActionResult PrintVoucherBillDetailsXL(string sParams)
        {
            List<VoucherBillBreakDown> oVoucherBillBreakDowns = new List<VoucherBillBreakDown>();
            int nCboDate = Convert.ToInt32(sParams.Split('~')[0]); //0:None; 1:Equlat to; 2:Between
            DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sParams.Split('~')[3]);
            int nCurrencyID = Convert.ToInt32(sParams.Split('~')[4]);
            bool bApprovedBy = Convert.ToBoolean(sParams.Split('~')[5]);
            int nVoucherBillID = Convert.ToInt32(sParams.Split('~')[6]);
            oVoucherBillBreakDowns = VoucherBillBreakDown.GetsForVoucherBill(nAccountHeadID, nVoucherBillID, nCurrencyID, dStartDate, dEndDate, bApprovedBy, (int)Session[SessionInfo.CurrentCompanyID], ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<VoucherBillDetailXL>));

            int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0; double nTotalClosingValue = 0;
            VoucherBillDetailXL oVoucherBillDetailXL = new VoucherBillDetailXL();
            List<VoucherBillDetailXL> oVoucherBillDetailXLs = new List<VoucherBillDetailXL>();
            foreach (VoucherBillBreakDown oItem in oVoucherBillBreakDowns)
            {
                nCount++;
                oVoucherBillDetailXL = new VoucherBillDetailXL();
                oVoucherBillDetailXL.SLNo = nCount.ToString();
                oVoucherBillDetailXL.VoucherDate = oItem.VoucherDateInString;
                oVoucherBillDetailXL.VoucherNo = oItem.VoucherNo;
                oVoucherBillDetailXL.Particulars = oItem.AccountHeadName;
                oVoucherBillDetailXL.DebitAmount = oItem.DebitAmount;
                oVoucherBillDetailXL.CreditAmount = oItem.CreditAmount;
                oVoucherBillDetailXL.ClosingValueDebitCredit = oItem.IsDrClosingInString;
                oVoucherBillDetailXL.ClosingValue = oItem.ClosingValue;
                oVoucherBillDetailXLs.Add(oVoucherBillDetailXL);
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
                nTotalClosingValue = nTotalClosingValue + oItem.ClosingValue;
            }

            #region Total
            oVoucherBillDetailXL = new VoucherBillDetailXL();
            oVoucherBillDetailXL.SLNo = "";
            oVoucherBillDetailXL.VoucherDate = "";
            oVoucherBillDetailXL.VoucherNo = "";
            oVoucherBillDetailXL.Particulars = "Total :";
            oVoucherBillDetailXL.DebitAmount = nTotalDebit;
            oVoucherBillDetailXL.CreditAmount = nTotalCredit;
            oVoucherBillDetailXL.ClosingValueDebitCredit = "";
            oVoucherBillDetailXL.ClosingValue = nTotalClosingValue;
            oVoucherBillDetailXLs.Add(oVoucherBillDetailXL);
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oVoucherBillDetailXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Voucher Bill Details.xls");
        }
        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {
            _oSP_GeneralLedger = new SP_GeneralLedger();
            _oSP_GeneralLedger.CompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            return PartialView(_oSP_GeneralLedger);
        }
        #endregion
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Make SQL
        private string GetSQL(SP_GeneralLedger oGeneralLedger)
        {

            string sVoucherNo = "";
            double nFromVoucherAmount = 0;
            double nToVoucherAmount = 0;
            string sAccountHeadName = "";
            int nVoucherType = 0;
            string sHeadWiseNarration = "";
            string sVoucherNarration = "";
            string sVoucherBillNo = "";
            double nFromBillAmount = 0;
            double nToBillAmount = 0;
            string sCostCenterName = "";
            double nFromCostCenterAmount = 0;
            double nToCostCenterAmount = 0;
            int nVoucherAmountCompareOperatorType = 0;
            int nBillAmountCompareOperatorType = 0;
            int nCostCenterAmountCompareOperatorType = 0;

            string sParams = oGeneralLedger.Narration;

            if (sParams != null)
            {
                if (sParams != "")
                {
                    sVoucherNo = Convert.ToString(sParams.Split('~')[0]);
                    nFromVoucherAmount = Convert.ToDouble(sParams.Split('~')[1]);
                    nToVoucherAmount = Convert.ToDouble(sParams.Split('~')[2]);
                    sAccountHeadName = Convert.ToString(sParams.Split('~')[3]);
                    nVoucherType = Convert.ToInt32(sParams.Split('~')[4]);
                    sHeadWiseNarration = Convert.ToString(sParams.Split('~')[5]);
                    sVoucherNarration = Convert.ToString(sParams.Split('~')[6]);
                    sVoucherBillNo = Convert.ToString(sParams.Split('~')[7]);
                    nFromBillAmount = Convert.ToDouble(sParams.Split('~')[8]);
                    nToBillAmount = Convert.ToDouble(sParams.Split('~')[9]);
                    sCostCenterName = Convert.ToString(sParams.Split('~')[10]);
                    nFromCostCenterAmount = Convert.ToDouble(sParams.Split('~')[11]);
                    nToCostCenterAmount = Convert.ToDouble(sParams.Split('~')[12]);

                    nVoucherAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[13]);
                    nBillAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[14]);
                    nCostCenterAmountCompareOperatorType = Convert.ToInt32(sParams.Split('~')[15]);
                }
            }

            #region Company Info
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            string sReturn1 = "SELECT VD.VoucherID, VD.AccountHeadID FROM View_VoucherDetail AS VD ";
            string sReturn = "";

            //#region Company
            //Global.TagSQL(ref sReturn);
            //sReturn = sReturn + " VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.CompanyID="+ nCompanyID +") ";
            //#endregion

            #region AccountHead
            if (oGeneralLedger.AccountHeadID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.AccountHeadID =" + oGeneralLedger.AccountHeadID.ToString();
            }
            #endregion

            #region BusinessUnit
            if (oGeneralLedger.BusinessUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.BUID =" + oGeneralLedger.BusinessUnitID.ToString();
            }
            #endregion

            #region Authorized By
            if (oGeneralLedger.IsApproved)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(VD.AuthorizedBy,0) !=0 ";
            }
            #endregion

            #region Currency
            if (oCompany.BaseCurrencyID != oGeneralLedger.CurrencyID)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.CurrencyID = " + oGeneralLedger.CurrencyID.ToString();
            }
            #endregion

            #region Voucher Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)) ";
            #endregion

            if (sParams != null)
            {
                if (sParams != "")
                {
                    #region Voucher No
                    if (sVoucherNo != null && sVoucherNo != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherNo LIKE '%" + sVoucherNo + "%')  ";
                    }
                    #endregion

                    #region Voucher Amount
                    if (nVoucherAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.Amount = " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.Amount != " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.Amount > " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.Amount < " + nFromVoucherAmount;
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.Amount BETWEEN " + nFromVoucherAmount + " AND " + nToVoucherAmount + " ";
                        }
                        else if (nVoucherAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.Amount NOT BETWEEN " + nFromVoucherAmount + " AND " + nToVoucherAmount + " ";
                        }
                    }
                    #endregion

                    #region Voucher Type
                    if (nVoucherType > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + nVoucherType + " ) ";
                    }
                    #endregion

                    #region Revarse AccountHeadName
                    //if (sAccountHeadName != null && sAccountHeadName != "")
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + " AccountHeadID IN (SELECT AccountHeadID FROM COA_ChartsOfAccount WHERE AccountHeadName LIKE '%" + sAccountHeadName + "%') AND AccountHeadID != "+oGeneralLedger.AccountHeadID+"  ";
                    //}
                    #endregion

                    #region Head Wise Narration
                    if (sHeadWiseNarration != null && sHeadWiseNarration != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.Narration LIKE '%" + sHeadWiseNarration + "%' ";
                    }
                    #endregion

                    #region Voucher Narration
                    if (sVoucherNarration != null && sVoucherNarration != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT VoucherID FROM Voucher AS V WHERE V.Narration LIKE '%" + sVoucherNarration + "%') ";
                    }
                    #endregion

                    #region BillNo
                    if (sVoucherBillNo != null && sVoucherBillNo != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.BillNo LIKE '%" + sVoucherBillNo + "%' ) ";
                    }
                    #endregion

                    #region Bill Amount
                    if (nBillAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount = " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount != " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount > " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount < " + nFromBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount BETWEEN " + nFromBillAmount + " AND " + nToBillAmount + " ) ";
                        }
                        else if (nBillAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount NOT BETWEEN " + nFromBillAmount + " AND " + nToBillAmount + ") ";
                        }
                    }
                    #endregion

                    #region CostCenterName
                    if (sCostCenterName != null && sCostCenterName != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.CostCenterName LIKE '%" + sCostCenterName + "%' ) ";
                    }
                    #endregion

                    #region Cost Center Amount
                    if (nCostCenterAmountCompareOperatorType != (int)EnumCompareOperator.None)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.EqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount = " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount != " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount > " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount < " + nFromCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount BETWEEN " + nFromCostCenterAmount + " AND " + nToCostCenterAmount + " ) ";
                        }
                        else if (nCostCenterAmountCompareOperatorType == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount NOT BETWEEN " + nFromCostCenterAmount + " AND " + nToCostCenterAmount + ") ";
                        }
                    }
                    #endregion
                }
            }
            sReturn = sReturn1 + sReturn + "  GROUP BY VoucherID, AccountHeadID ";
            return sReturn;
        }
        #endregion


        [HttpPost]

        public JsonResult GetText(SP_GeneralLedger oGeneralLedger)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
            }
            oSP_GeneralLedger.ErrorMessage = "All finished!";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSP_GeneralLedger);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}