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
using System.Threading;
using ESimSolFinancial.Hubs;

namespace ESimSolFinancial.Controllers
{
    public class GeneralJournalController : Controller
    {
        #region Declaration
        SP_GeneralJournal _oSP_GeneralJournal = new SP_GeneralJournal();
        List<SP_GeneralJournal> _oSP_GeneralJournals = new List<SP_GeneralJournal>();
        #endregion

        #region New Version
        #region GJ
        #region Make SQL
        private string GetSQL(SP_GeneralJournal oGeneralJournal)
        {
            string sVoucherNo = "";
            double nFromVoucherAmount = 0;
            double nToVoucherAmount = 0;
            string sAccountHeadName = "";
            string sHeadWiseNarration = "";
            string sVoucherNarration = "";
            string sVoucherBillNo = "";
            double nFromBillAmount = 0;
            double nToBillAmount = 0;
            string sCostCenterName = "";
            double nFromCostCenterAmount = 0;
            double nToCostCenterAmount = 0;
            double nUserID = 0;
            
            string sParams = oGeneralJournal.Narration;

            if (sParams != null)
            {
                if (sParams != "")
                {
                    sVoucherNo = Convert.ToString(sParams.Split('~')[0]);
                    nFromVoucherAmount = Convert.ToDouble(sParams.Split('~')[1]);
                    nToVoucherAmount = Convert.ToDouble(sParams.Split('~')[2]);
                    sAccountHeadName = Convert.ToString(sParams.Split('~')[3]);
                    sHeadWiseNarration = Convert.ToString(sParams.Split('~')[4]);
                    sVoucherNarration = Convert.ToString(sParams.Split('~')[5]);
                    sVoucherBillNo = Convert.ToString(sParams.Split('~')[6]);
                    nFromBillAmount = Convert.ToDouble(sParams.Split('~')[7]);
                    nToBillAmount = Convert.ToDouble(sParams.Split('~')[8]);
                    sCostCenterName = Convert.ToString(sParams.Split('~')[9]);
                    nFromCostCenterAmount = Convert.ToDouble(sParams.Split('~')[10]);
                    nToCostCenterAmount = Convert.ToDouble(sParams.Split('~')[11]);
                    nUserID = Convert.ToDouble(sParams.Split('~')[12]);
                }
            }
            string sReturn1 = "SELECT VD.VoucherDetailID, VD.VoucherID, VD.AccountHeadID, VD.IsDebit, VD.Narration FROM View_VoucherDetail AS VD ";
            string sReturn = "";



            #region AccountHead
            if (oGeneralJournal.AccountHeadID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.AccountHeadID =" + oGeneralJournal.AccountHeadID.ToString();
            }
            #endregion

            #region BusinessUnit
            if (oGeneralJournal.BusinessUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.BUID =" + oGeneralJournal.BusinessUnitID.ToString();
            }
            #endregion

            #region Voucher Type
            if (oGeneralJournal.VoucherTypeID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.VoucherTypeID = " + oGeneralJournal.VoucherTypeID.ToString();
            }
            #endregion

            #region Authorized By
            if (oGeneralJournal.IsApproved)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(VD.AuthorizedBy,0) !=0 ";
            }
            #endregion

            #region Voucher Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)) ";
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
                    if (nFromVoucherAmount > 0 && nToVoucherAmount > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.Amount BETWEEN " + nFromVoucherAmount + " AND " + nToVoucherAmount + " ";
                    }
                    #endregion

                    #region AccountHeadName
                    if (sAccountHeadName != null && sAccountHeadName != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.AccountHeadID IN (SELECT AccountHeadID FROM View_ChartsOfAccount WHERE AccountHeadName LIKE '%" + sAccountHeadName + "%')  ";
                    }
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
                    if (nFromBillAmount > 0 && nToBillAmount > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT VBT.VoucherDetailID FROM View_VoucherBillTransaction AS VBT WHERE VBT.Amount BETWEEN " + nFromBillAmount + " AND " + nToBillAmount + " ) ";
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
                    if (nFromCostCenterAmount > 0 && nToCostCenterAmount>0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherDetailID IN (SELECT CCT.VoucherDetailID FROM View_CostCenterTransaction AS CCT WHERE CCT.Amount BETWEEN " + nFromCostCenterAmount + " AND " + nToCostCenterAmount + " ) ";
                    }
                    #endregion

                    #region UserID
                    if (nUserID != 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " VD.VoucherID IN (SELECT HH.VoucherID FROM Voucher AS HH WHERE HH.DBUserID =" + nUserID + ")";
                    }
                    #endregion
                }
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #region Get GJ According to config
        private List<SP_GeneralJournal> GetsListAccordingToConfig(SP_GeneralJournal oSP_GeneralJournal, List<ACConfig> oACConfigs)
        {

            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];

            List<SP_GeneralJournal> oNewSP_GeneralJournals = new List<SP_GeneralJournal>();
            SP_GeneralJournal oNewSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oTempSP_GeneralJournals = new List<SP_GeneralJournal>();

            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, (int)Session[SessionInfo.currentUserID]);

            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<VOReference> oVOReferences = new List<VOReference>();
            List<VoucherCheque> oVoucherCheques = new List<VoucherCheque>();
            string sSQL = "";

            if (oSP_GeneralJournal.VoucherTypeID == 0)
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherCheques = VoucherCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VOReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVOReferences = VOReference.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + oSP_GeneralJournal.VoucherTypeID + ")  AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + oSP_GeneralJournal.VoucherTypeID + ") AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + oSP_GeneralJournal.VoucherTypeID + ") AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + oSP_GeneralJournal.VoucherTypeID + ") AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherCheques = VoucherCheque.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_VOReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.VoucherID IN (SELECT VoucherID FROM Voucher WHERE VoucherTypeID = " + oSP_GeneralJournal.VoucherTypeID + ") AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralJournal.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVOReferences = VOReference.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }


            if (oSP_GeneralJournal.SP_GeneralJournalList.Count > 0)
            {
                int nIndex = 0;
                int nVoucherID = oSP_GeneralJournal.SP_GeneralJournalList[0].VoucherID;
                foreach (SP_GeneralJournal oSPGJ in oSP_GeneralJournal.SP_GeneralJournalList)
                {
                    oNewSP_GeneralJournals.Add(oSPGJ);
                    foreach (ACConfig oItem in oACConfigs)
                    {
                        bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                        #region Account Head Narration
                        if (oItem.ConfigureType == EnumConfigureType.GJAccHeadWiseNarration && bConfigureValue == true)
                        {
                            oNewSP_GeneralJournal = GetNarration(oItem, oSPGJ);
                            oNewSP_GeneralJournal.VoucherDate = oSPGJ.VoucherDate;
                            if (oNewSP_GeneralJournal.IsNullOrNot == false)
                            {
                                oNewSP_GeneralJournals.Add(oNewSP_GeneralJournal);
                            }
                        }
                        #endregion
                        #region Voucher Narration
                        else if (oItem.ConfigureType == EnumConfigureType.GJVoucherNarration && bConfigureValue == true)
                        {
                            oNewSP_GeneralJournal = GetNarration(oItem, oSPGJ);
                            oNewSP_GeneralJournal.VoucherDate = oSPGJ.VoucherDate;
                            if (oNewSP_GeneralJournal.IsNullOrNot == false)
                            {
                                oNewSP_GeneralJournals.Add(oNewSP_GeneralJournal);
                            }
                        }
                        #endregion
                        #region Cost Center Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GJCC && bConfigureValue == true)
                        {
                            if (oCostCenterTransactions.Count > 0)
                            {
                                oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetCC(oCostCenterTransactions, oSPGJ.VoucherDetailID, oSPGJ.VoucherDate, oACConfigs, oVoucherBillTransactions, oVoucherCheques);
                                oNewSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        #endregion
                        #region Voucher Bill Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GJBT && bConfigureValue == true)
                        {
                            if (oVoucherBillTransactions.Count > 0)
                            {
                                oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetBT(oVoucherBillTransactions, oSPGJ.VoucherDetailID, 0, oSPGJ.VoucherDate);
                                oNewSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        #endregion
                        #region VP (IR) Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GJIR && bConfigureValue == true)
                        {
                            if (oVPTransactions.Count > 0)
                            {
                                oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetIR(oVPTransactions, oSPGJ.VoucherDetailID, oSPGJ.VoucherDate);
                                oNewSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        #endregion
                        #region Cheque Reference
                        else if (oItem.ConfigureType == EnumConfigureType.GJVC && bConfigureValue == true)
                        {
                            if (oVoucherCheques.Count > 0)
                            {
                                oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetVC(oVoucherCheques, oSPGJ.VoucherDetailID, 0, oSPGJ.VoucherDate, oSPGJ.IsDebit);
                                oNewSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        #endregion
                        #region Order Reference
                        else if (oItem.ConfigureType == EnumConfigureType.GJOR && bConfigureValue == true)
                        {
                            if (oVOReferences.Count > 0)
                            {
                                oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetOR(oVOReferences, oSPGJ.VoucherDetailID, oSPGJ.VoucherDate);
                                oNewSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        #endregion
                    }
                    nVoucherID = oSP_GeneralJournal.SP_GeneralJournalList[nIndex].VoucherID;
                    nIndex = nIndex + 1;
                }
            }
            return oNewSP_GeneralJournals;
        }

        private List<SP_GeneralJournal> GetCC(List<CostCenterTransaction> oCostCenterTransactions, int VoucherDetailID, DateTime dVoucherDate, List<ACConfig> oACConfigs, List<VoucherBillTransaction> oSubLedgerBillTransactions, List<VoucherCheque> oSLVoucherCheques)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            foreach (CostCenterTransaction oItem in oCostCenterTransactions)
            {
                if (oItem.VoucherDetailID == VoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralJournal = new SP_GeneralJournal();
                    //oSP_GeneralJournal.ConfigTitle = "";
                    oSP_GeneralJournal.ConfigType = EnumConfigureType.GLCC;
                    oSP_GeneralJournal.VoucherDate = dVoucherDate;
                    oSP_GeneralJournal.DebitAmount = oItem.IsDr ? oItem.Amount * oItem.CurrencyConversionRate : 0;
                    oSP_GeneralJournal.CreditAmount = oItem.IsDr ? 0 : oItem.Amount * oItem.CurrencyConversionRate;
                    oSP_GeneralJournal.AccountHeadName = "Subledger : " + oItem.CostCenterName + ", Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralJournals.Add(oSP_GeneralJournal);
                    foreach (ACConfig oACConfig in oACConfigs)
                    {
                        if (oACConfig.ConfigureType == EnumConfigureType.GJBT && oACConfig.ConfigureValue == "1")
                        {
                            if (oSubLedgerBillTransactions.Count > 0)
                            {
                                List<SP_GeneralJournal> oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetBT(oSubLedgerBillTransactions, (int)oItem.VoucherDetailID, oItem.CCTID, dVoucherDate);
                                oSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                        if (oACConfig.ConfigureType == EnumConfigureType.GJVC && oACConfig.ConfigureValue == "1")
                        {
                            if (oSLVoucherCheques.Count > 0)
                            {
                                List<SP_GeneralJournal> oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
                                oTempSP_GeneralJournals = GetVC(oSLVoucherCheques, (int)oItem.VoucherDetailID, oItem.CCTID, dVoucherDate, oItem.IsDr);
                                oSP_GeneralJournals.AddRange(oTempSP_GeneralJournals);
                            }
                        }
                    }

                }
            }
            return oSP_GeneralJournals;
        }
        private List<SP_GeneralJournal> GetBT(List<VoucherBillTransaction> oVoucherBillTransactions, int VoucherDetailID, int nCCTID, DateTime dVoucherDate)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
            {
                if (oItem.VoucherDetailID == VoucherDetailID && oItem.CCTID == nCCTID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralJournal = new SP_GeneralJournal();
                    //oSP_GeneralJournal.ConfigTitle = oItem.CCTID > 0 ? "SL BT " : "BT ";
                    oSP_GeneralJournal.ConfigType = EnumConfigureType.GLBT;
                    oSP_GeneralJournal.VoucherDate = dVoucherDate;
                    oSP_GeneralJournal.DebitAmount = oItem.IsDr ? oItem.Amount * oItem.ConversionRate : 0;
                    oSP_GeneralJournal.CreditAmount = oItem.IsDr ? 0 : oItem.Amount * oItem.ConversionRate;
                    oSP_GeneralJournal.AccountHeadName = (oItem.CCTID > 0 ? "SL Bill : " : "Bill : ") + oItem.ExplanationTransactionTypeInString+", Bill No : " + oItem.BillNo + ", Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + ", Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralJournals.Add(oSP_GeneralJournal);
                }
            }
            return oSP_GeneralJournals;
        }
        private List<SP_GeneralJournal> GetIR(List<VPTransaction> oVPTransactions, int VoucherDetailID, DateTime dVoucherDate)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            foreach (VPTransaction oItem in oVPTransactions)
            {
                if (oItem.VoucherDetailID == VoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralJournal = new SP_GeneralJournal();
                    //oSP_GeneralJournal.ConfigTitle = "IR ";
                    oSP_GeneralJournal.ConfigType = EnumConfigureType.GLIR;
                    oSP_GeneralJournal.VoucherDate = dVoucherDate;
                    oSP_GeneralJournal.DebitAmount = oItem.IsDr ? oItem.Amount * oItem.ConversionRate : 0;
                    oSP_GeneralJournal.CreditAmount = oItem.IsDr ? 0 : oItem.Amount * oItem.ConversionRate;
                    oSP_GeneralJournal.AccountHeadName = "Item :" + oItem.ProductName + ", WU : " + oItem.WorkingUnitName + ", MUnit : " + oItem.MUnitName + ", Qty : " + Global.MillionFormat(oItem.Qty) + ", Unit Price : " + Global.MillionFormat(oItem.UnitPrice) + ", Amount : " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralJournals.Add(oSP_GeneralJournal);
                }
            }
            return oSP_GeneralJournals;
        }
        private List<SP_GeneralJournal> GetVC(List<VoucherCheque> oVoucherCheques, int VoucherDetailID, int nCCTID, DateTime dVoucherDate, bool sIsDr)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            foreach (VoucherCheque oItem in oVoucherCheques)
            {
                if (oItem.VoucherDetailID == VoucherDetailID && oItem.CCTID == nCCTID)
                {
                    oSP_GeneralJournal = new SP_GeneralJournal();
                    //oSP_GeneralJournal.ConfigTitle = oItem.CCTID > 0 ? "SL Cheque" : "Cheque";
                    oSP_GeneralJournal.ConfigType = EnumConfigureType.GLVC;
                    oSP_GeneralJournal.VoucherDate = dVoucherDate;
                    oSP_GeneralJournal.DebitAmount = sIsDr ? oItem.Amount : 0;
                    oSP_GeneralJournal.CreditAmount = sIsDr ? 0 : oItem.Amount;
                    oSP_GeneralJournal.AccountHeadName = (oItem.CCTID > 0 ? "SL Cheque No : " : "Cheque No : ") + oItem.ChequeNo + ", Account No :" + oItem.AccountNo + ", Amount : " + Global.MillionFormat(oItem.Amount);
                    oSP_GeneralJournals.Add(oSP_GeneralJournal);
                }
            }
            return oSP_GeneralJournals;
        }
        private List<SP_GeneralJournal> GetOR(List<VOReference> oVOReferences, int VoucherDetailID, DateTime dVoucherDate)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            foreach (VOReference oItem in oVOReferences)
            {
                if (oItem.VoucherDetailID == VoucherDetailID)
                {
                    string bIsDr = (oItem.IsDebit == true ? " Dr " : " Cr ");
                    oSP_GeneralJournal = new SP_GeneralJournal();
                    //oSP_GeneralJournal.ConfigTitle = "OR ";
                    oSP_GeneralJournal.ConfigType = EnumConfigureType.GLOR;
                    oSP_GeneralJournal.VoucherDate = dVoucherDate;
                    oSP_GeneralJournal.DebitAmount = oItem.IsDebit ? oItem.Amount * oItem.ConversionRate : 0;
                    oSP_GeneralJournal.CreditAmount = oItem.IsDebit ? 0 : oItem.Amount * oItem.ConversionRate;
                    oSP_GeneralJournal.AccountHeadName = (oItem.CCTID > 0 ? "SL Ref No : " : "Ref No : ")+ oItem.RefNo + ", Order No : " + oItem.OrderNo + ", Date : " + oItem.OrderDateSt + ", Amount : " + oItem.AmountInString + bIsDr;
                    oSP_GeneralJournals.Add(oSP_GeneralJournal);
                }
            }
            return oSP_GeneralJournals;
        }
        private SP_GeneralJournal GetNarration(ACConfig oACConfig, SP_GeneralJournal oSP_GeneralJournal)
        {
            SP_GeneralJournal oNewSP_GeneralJournal = new SP_GeneralJournal();
            if (oACConfig.ConfigureType == EnumConfigureType.GJAccHeadWiseNarration)
            {
                //oNewSP_GeneralJournal.ConfigTitle = " Head Wise Narration ";
                oNewSP_GeneralJournal.AccountHeadName = oSP_GeneralJournal.Narration;
                if (oNewSP_GeneralJournal.AccountHeadName == "")
                {
                    oNewSP_GeneralJournal.IsNullOrNot = true;
                }
            }
            else if (oACConfig.ConfigureType == EnumConfigureType.GJVoucherNarration)
            {
                //oNewSP_GeneralJournal.ConfigTitle = " Voucher Narration ";
                oNewSP_GeneralJournal.AccountHeadName = oSP_GeneralJournal.VoucherNarration;
                if (oNewSP_GeneralJournal.AccountHeadName == "")
                {
                    oNewSP_GeneralJournal.IsNullOrNot = true;
                }
            }
            return oNewSP_GeneralJournal;
        }
        #endregion
        #region Month And Date Wise Vouchers
        private List<SP_GeneralJournal> GetVouchers(DateTime dDate, List<SP_GeneralJournal> oSP_GeneralJournals, EnumDisplayMode sEnumDisplayMode)
        {
            List<SP_GeneralJournal> oTempSP_GeneralJournals = new List<SP_GeneralJournal>();
            SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

            foreach (SP_GeneralJournal oItem in oSP_GeneralJournals)
            {
                if (sEnumDisplayMode == EnumDisplayMode.MonthlyView)
                {
                    if (oItem.VoucherDate.ToString("MMM yyyy") == dDate.ToString("MMM yyyy"))
                    {
                        oTempGeneralJournal = NewObject(oItem, EnumDisplayMode.None);
                        oTempSP_GeneralJournals.Add(oTempGeneralJournal);
                    }
                }
                else if (sEnumDisplayMode == EnumDisplayMode.DateView)
                {
                    if (oItem.VoucherDate.ToString("dd MMM yyyy") == dDate.ToString("dd MMM yyyy"))
                    {
                        oTempGeneralJournal = NewObject(oItem, EnumDisplayMode.None);
                        oTempSP_GeneralJournals.Add(oTempGeneralJournal);
                    }
                }
            }
            return oTempSP_GeneralJournals;
        }
        #endregion

        public SP_GeneralJournal NewObject(SP_GeneralJournal oGJ, EnumDisplayMode eEnumDisplayMode)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();

            if (eEnumDisplayMode == EnumDisplayMode.MonthlyView)
            {
                oSP_GeneralJournal.ConfigTitle = oGJ.VoucherDate.ToString("MMM yyyy");
                oSP_GeneralJournal.StartDate = new DateTime(oGJ.VoucherDate.Year, oGJ.VoucherDate.Month, 1);
                oSP_GeneralJournal.EndDate = new DateTime(oGJ.VoucherDate.Year, oGJ.VoucherDate.Month + 1, 1).AddDays(-1);
            }
            else if (eEnumDisplayMode == EnumDisplayMode.DateView)
            {
                oSP_GeneralJournal.ConfigTitle = oGJ.VoucherDate.ToString("dd MMM yyyy");
                oSP_GeneralJournal.StartDate = oGJ.VoucherDate;
                oSP_GeneralJournal.EndDate = oGJ.VoucherDate;
            }
            else
            {
                oSP_GeneralJournal.VoucherDetailID = oGJ.VoucherDetailID;
                oSP_GeneralJournal.VoucherID = oGJ.VoucherID;
                oSP_GeneralJournal.VoucherNo = oGJ.VoucherNo;
                oSP_GeneralJournal.AccountHeadName = oGJ.AccountHeadName;
                oSP_GeneralJournal.VoucherName = oGJ.VoucherName;
                oSP_GeneralJournal.AccountCode = oGJ.AccountCode;
                oSP_GeneralJournal.ConfigTitle = oGJ.ConfigTitle;
            }
            oSP_GeneralJournal.VoucherNarration = oGJ.VoucherNarration;
            oSP_GeneralJournal.VoucherDate = oGJ.VoucherDate;
            oSP_GeneralJournal.DebitAmount = oGJ.DebitAmount;
            oSP_GeneralJournal.CreditAmount = oGJ.CreditAmount;
            oSP_GeneralJournal.SP_GeneralJournalList = oGJ.SP_GeneralJournalList;
            return oSP_GeneralJournal;
        }
        #region Set GJ SessionData
        [HttpPost]
        public ActionResult SetGJSessionData(SP_GeneralJournal oSP_GeneralJournal)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSP_GeneralJournal);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSessionData(SP_GeneralJournal oSP_GeneralJournal)
        {
            List<SP_GeneralJournal> oGeneralJournals = new List<SP_GeneralJournal>();
            try
            {
                oGeneralJournals = (List<SP_GeneralJournal>)Session[SessionInfo.SearchData];
            }
            catch (Exception ex)
            {
                oGeneralJournals = new List<SP_GeneralJournal>();
            }
            var jsonResult = Json(oGeneralJournals, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
        #region GJ Transactions
        public ActionResult ViewGeneralJournal(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                oSP_GeneralJournal = (SP_GeneralJournal)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oSP_GeneralJournal = null;
            }

            if (oSP_GeneralJournal != null)
            {
                Thread.Sleep(1000);
                ProgressHub.SendMessage("Getting Journal Data", 10, (int)Session[SessionInfo.currentUserID]);
                if (oSP_GeneralJournal.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.StartDate = oAccountingSession.StartDate;
                    oSP_GeneralJournal.EndDate = DateTime.Now;
                }



                List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
                if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
                {
                    oSP_GeneralJournals.Add(_oSP_GeneralJournal);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string sjson = serializer.Serialize((object)oSP_GeneralJournals);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Check Authorize Business Unit
                    if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                    #endregion

                    oSP_GeneralJournals = new List<SP_GeneralJournal>();
                    string sSQL = GetSQL(oSP_GeneralJournal);
                    oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                    Thread.Sleep(2000);
                    ProgressHub.SendMessage("Mapping Journal Data", 40, (int)Session[SessionInfo.currentUserID]);

                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;

                    oSP_GeneralJournal.SP_GeneralJournalList = GetsListAccordingToConfig(oSP_GeneralJournal, oSP_GeneralJournal.ACConfigs);

                    Thread.Sleep(3000);
                    ProgressHub.SendMessage("Transfering Journal Data", 70, (int)Session[SessionInfo.currentUserID]);

                    #region Display Mode
                    #region Month
                    if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                    {
                        List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                        SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                        DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");

                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            if (oItem.ConfigType == EnumConfigureType.None)
                            {
                                #region Individual Month
                                if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView)
                                {
                                    if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                    {
                                        oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                        oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                    }
                                }
                                #endregion
                                #region Individual Date
                                else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                                {
                                    if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                    {
                                        oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                        oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                    }
                                }
                                #endregion
                                dLedgerDate = oItem.VoucherDate;
                            }
                        }

                        if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                        {
                            foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                            {
                                oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oSP_GeneralJournal.SP_GeneralJournalList, oSP_GeneralJournal.DisplayMode);
                            }
                        }

                        foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                        {
                            double nDrAmount = 0;
                            double nCrAmount = 0;
                            foreach (SP_GeneralJournal oItemVoucher in oItem.DisplayVouchers)
                            {
                                if (oItemVoucher.VoucherID > 0)
                                {
                                    nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                                    nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                                }
                            }
                            oItem.DebitAmount = nDrAmount;
                            oItem.CreditAmount = nCrAmount;
                        }
                        oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;
                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                            oTempSPGJs = oItem.DisplayVouchers.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                            foreach (SP_GeneralJournal oTempSPGJ in oTempSPGJs)
                            {
                                oItem.DisplayVouchers.Where(x => x.VoucherID == oTempSPGJ.VoucherID && x.VoucherDetailID != oTempSPGJ.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                            }
                        }
                    }
                    #endregion
                    #region Individual Week
                    else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.WeeklyView)
                    {
                        List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                        SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                        DateTime dStartDate = oSP_GeneralJournal.StartDate;
                        int nCountWeek = 1;
                        while (dStartDate <= oSP_GeneralJournal.EndDate.Date)
                        {
                            oTempGeneralJournal = new SP_GeneralJournal();
                            oTempGeneralJournal.StartDate = dStartDate;
                            oTempGeneralJournal.EndDate = dStartDate.AddDays(6);
                            oTempGeneralJournal.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralJournal.StartDate.ToString("MMM") + " " + oTempGeneralJournal.StartDate.ToString("dd") + " - " + oTempGeneralJournal.EndDate.ToString("MMM") + " " + oTempGeneralJournal.EndDate.ToString("dd") + ")";
                            oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                            dStartDate = dStartDate.AddDays(7);
                            nCountWeek++;
                        }

                        foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                        {
                            SP_GeneralJournal oTempGJ = new SP_GeneralJournal();
                            DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                            double nDebitAmount = 0;
                            double nCreditAmount = 0;
                            foreach (SP_GeneralJournal oGJ in oSP_GeneralJournal.SP_GeneralJournalList)
                            {
                                if (oGJ.VoucherDate >= oItem.StartDate && oGJ.VoucherDate <= oItem.EndDate)
                                {
                                    oTempGJ = new SP_GeneralJournal();
                                    oTempGJ.ConfigTitle = oGJ.VoucherDate.ToString("dd MMM yyyy");
                                    oTempGJ.VoucherID = oGJ.VoucherID;
                                    oTempGJ.VoucherNo = oGJ.VoucherNo;
                                    oTempGJ.VoucherDate = oGJ.VoucherDate;
                                    oTempGJ.AccountCode = oGJ.AccountCode;
                                    oTempGJ.AccountHeadName = oGJ.AccountHeadName;
                                    oTempGJ.DebitAmount = oGJ.DebitAmount;
                                    oTempGJ.CreditAmount = oGJ.CreditAmount;
                                    oTempGJ.SP_GeneralJournalList = oGJ.SP_GeneralJournalList;
                                    oItem.DisplayVouchers.Add(oTempGJ);
                                    if (oGJ.VoucherID > 0)
                                    {
                                        nDebitAmount = nDebitAmount + oTempGJ.DebitAmount;
                                        nCreditAmount = nCreditAmount + oTempGJ.CreditAmount;
                                    }
                                }
                            }
                            oItem.DebitAmount = nDebitAmount;
                            oItem.CreditAmount = nCreditAmount;
                        }
                        oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;

                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                            oTempSPGJs = oItem.DisplayVouchers.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                            foreach (SP_GeneralJournal oTempSPGJ in oTempSPGJs)
                            {
                                oItem.DisplayVouchers.Where(x => x.VoucherID == oTempSPGJ.VoucherID && x.VoucherDetailID != oTempSPGJ.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                        oTempSPGJs = oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                        foreach (SP_GeneralJournal oItem in oTempSPGJs)
                        {
                            oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID == oItem.VoucherID && x.VoucherDetailID != oItem.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                        }
                    }
                    #endregion

                    Thread.Sleep(1000);
                    ProgressHub.SendMessage("Loading Journal Data", 100, (int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                oSP_GeneralJournal = new SP_GeneralJournal();
                oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            }

            #region Voucher Types
            VoucherType oVoucherType = new VoucherType();
            List<VoucherType> oVoucherTypes = new List<VoucherType>();
            oVoucherType = new VoucherType();
            oVoucherType.VoucherName = "--Select Voucher Type--";
            oVoucherTypes.Add(oVoucherType);
            oVoucherTypes.AddRange(VoucherType.Gets((int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.VoucherTypes = oVoucherTypes;
            oSP_GeneralJournal.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            #region Configure
            List<ACConfig> oACConfigs = new List<ACConfig>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumConfigureType)))
            {
                if (oItem.id != 0 && oItem.id > 10)
                {
                    ACConfig oACConfig = new ACConfig();
                    oACConfig.ConfigureValue = "1";
                    oACConfig.ConfigureValueType = EnumConfigureValueType.BoolValue;
                    oACConfig.ConfigureType = (EnumConfigureType)oItem.id;
                    oACConfig.ErrorMessage = EnumObject.jGet(oACConfig.ConfigureType);
                    oACConfigs.Add(oACConfig);
                }
            }
            ViewBag.GJConfigs = oACConfigs;
            #endregion
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, nUserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oSP_GeneralJournal.BusinessUnitID, nUserID);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }
            ViewBag.Company = oCompany;
            oUser = new ESimSol.BusinessObjects.User();
            List<ESimSol.BusinessObjects.User> oUsers = new List<ESimSol.BusinessObjects.User>();
            string sUserSQL = "SELECT * FROM View_User AS TT WHERE TT.UserID IN(SELECT DISTINCT V.DBUserID FROM Voucher AS V)";
            oUser.UserName = "-- Select User Name --";
            oUsers.Add(oUser);
            oUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sUserSQL, (int)Session[SessionInfo.currentUserID]));
            foreach (ESimSol.BusinessObjects.User oItem in oUsers)
            {
                oItem.Password = "";
                oItem.ConfirmPassword = "";
            }
            ViewBag.UserList = oUsers;
            this.Session.Remove(SessionInfo.ParamObj);

            this.Session.Remove(SessionInfo.SearchData);
            this.Session.Add(SessionInfo.SearchData, oSP_GeneralJournal.SP_GeneralJournalList);
            oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            return View(oSP_GeneralJournal);
        }
        #endregion

        #region GJ Summary
        public ActionResult ViewGeneralJournalSummary(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            int nUserID = (int)Session[SessionInfo.currentUserID];
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                oSP_GeneralJournal = (SP_GeneralJournal)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oSP_GeneralJournal = null;
            }

            if (oSP_GeneralJournal != null)
            {
                Thread.Sleep(1000);
                ProgressHub.SendMessage("Getting Journal Data", 10, (int)Session[SessionInfo.currentUserID]);
                if (oSP_GeneralJournal.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.StartDate = oAccountingSession.StartDate;
                    oSP_GeneralJournal.EndDate = DateTime.Now;
                }



                List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
                if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
                {
                    oSP_GeneralJournals.Add(_oSP_GeneralJournal);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string sjson = serializer.Serialize((object)oSP_GeneralJournals);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Check Authorize Business Unit
                    if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                    #endregion

                    oSP_GeneralJournals = new List<SP_GeneralJournal>();
                    string sSQL = GetSQL(oSP_GeneralJournal);
                    oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                    Thread.Sleep(2000);
                    ProgressHub.SendMessage("Mapping Journal Data", 40, (int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;

                    oSP_GeneralJournal.SP_GeneralJournalList = GetsListAccordingToConfig(oSP_GeneralJournal, oSP_GeneralJournal.ACConfigs);

                    Thread.Sleep(3000);
                    ProgressHub.SendMessage("Transfering Journal Data", 70, (int)Session[SessionInfo.currentUserID]);

                    #region Display Mode
                    #region Month
                    if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                    {
                        List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                        SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                        DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");

                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            if (oItem.ConfigType == EnumConfigureType.None)
                            {
                                #region Individual Month
                                if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView)
                                {
                                    if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                    {
                                        oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                        oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                    }
                                }
                                #endregion
                                #region Individual Date
                                else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                                {
                                    if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                    {
                                        oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                        oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                    }
                                }
                                #endregion
                                dLedgerDate = oItem.VoucherDate;
                            }
                        }

                        if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                        {
                            foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                            {
                                oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, oSP_GeneralJournal.SP_GeneralJournalList, oSP_GeneralJournal.DisplayMode);
                            }
                        }

                        foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                        {
                            double nDrAmount = 0;
                            double nCrAmount = 0;
                            foreach (SP_GeneralJournal oItemVoucher in oItem.DisplayVouchers)
                            {
                                if (oItemVoucher.VoucherID > 0)
                                {
                                    nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                                    nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                                }
                            }
                            oItem.DebitAmount = nDrAmount;
                            oItem.CreditAmount = nCrAmount;
                        }
                        oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;
                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            oItem.DisplayVouchers = new List<SP_GeneralJournal>();
                        }
                    }
                    #endregion
                    #region Individual Week
                    else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.WeeklyView)
                    {
                        List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                        SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                        DateTime dStartDate = oSP_GeneralJournal.StartDate;
                        int nCountWeek = 1;
                        while (dStartDate <= oSP_GeneralJournal.EndDate.Date)
                        {
                            oTempGeneralJournal = new SP_GeneralJournal();
                            oTempGeneralJournal.StartDate = dStartDate;
                            oTempGeneralJournal.EndDate = dStartDate.AddDays(6);
                            oTempGeneralJournal.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralJournal.StartDate.ToString("MMM") + " " + oTempGeneralJournal.StartDate.ToString("dd") + " - " + oTempGeneralJournal.EndDate.ToString("MMM") + " " + oTempGeneralJournal.EndDate.ToString("dd") + ")";
                            oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                            dStartDate = dStartDate.AddDays(7);
                            nCountWeek++;
                        }

                        foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                        {
                            SP_GeneralJournal oTempGJ = new SP_GeneralJournal();
                            DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                            double nDebitAmount = 0;
                            double nCreditAmount = 0;
                            foreach (SP_GeneralJournal oGJ in oSP_GeneralJournal.SP_GeneralJournalList)
                            {
                                if (oGJ.VoucherDate >= oItem.StartDate && oGJ.VoucherDate <= oItem.EndDate)
                                {
                                    oTempGJ = new SP_GeneralJournal();
                                    oTempGJ.ConfigTitle = oGJ.VoucherDate.ToString("dd MMM yyyy");
                                    oTempGJ.VoucherID = oGJ.VoucherID;
                                    oTempGJ.VoucherNo = oGJ.VoucherNo;
                                    oTempGJ.VoucherDate = oGJ.VoucherDate;
                                    oTempGJ.AccountCode = oGJ.AccountCode;
                                    oTempGJ.AccountHeadName = oGJ.AccountHeadName;
                                    oTempGJ.DebitAmount = oGJ.DebitAmount;
                                    oTempGJ.CreditAmount = oGJ.CreditAmount;
                                    oTempGJ.SP_GeneralJournalList = oGJ.SP_GeneralJournalList;
                                    oItem.DisplayVouchers.Add(oTempGJ);
                                    if (oGJ.VoucherID > 0)
                                    {
                                        nDebitAmount = nDebitAmount + oTempGJ.DebitAmount;
                                        nCreditAmount = nCreditAmount + oTempGJ.CreditAmount;
                                    }
                                }
                            }
                            oItem.DebitAmount = nDebitAmount;
                            oItem.CreditAmount = nCreditAmount;
                        }
                        oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;

                        foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                        {
                            oItem.DisplayVouchers = new List<SP_GeneralJournal>();
                        }
                    }
                    #endregion
                    else
                    {
                        List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                        oTempSPGJs = oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                        foreach (SP_GeneralJournal oItem in oTempSPGJs)
                        {
                            oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID == oItem.VoucherID && x.VoucherDetailID != oItem.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                        }
                    }
                    #endregion

                    Thread.Sleep(1000);
                    ProgressHub.SendMessage("Loading Journal Data", 100, (int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                oSP_GeneralJournal = new SP_GeneralJournal();
                oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            }

            #region Voucher Types
            VoucherType oVoucherType = new VoucherType();
            List<VoucherType> oVoucherTypes = new List<VoucherType>();
            oVoucherType = new VoucherType();
            oVoucherType.VoucherName = "--Select Voucher Type--";
            oVoucherTypes.Add(oVoucherType);
            oVoucherTypes.AddRange(VoucherType.Gets((int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.VoucherTypes = oVoucherTypes;
            oSP_GeneralJournal.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            #region Configure
            List<ACConfig> oACConfigs = new List<ACConfig>();
            foreach (EnumObject oItem in EnumObject.jGets(typeof(EnumConfigureType)))
            {
                if (oItem.id != 0 && oItem.id > 10)
                {
                    ACConfig oACConfig = new ACConfig();
                    oACConfig.ConfigureValue = "1";
                    oACConfig.ConfigureValueType = EnumConfigureValueType.BoolValue;
                    oACConfig.ConfigureType = (EnumConfigureType)oItem.id;
                    oACConfig.ErrorMessage = EnumObject.jGet(oACConfig.ConfigureType);
                    oACConfigs.Add(oACConfig);
                }
            }
            ViewBag.GJConfigs = oACConfigs;
            #endregion
            Company oCompany = new Company();
            oCompany=oCompany.Get(1, nUserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oSP_GeneralJournal.BusinessUnitID, nUserID);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }
            ViewBag.Company = oCompany;
            oUser = new ESimSol.BusinessObjects.User();            
            List<ESimSol.BusinessObjects.User> oUsers = new List<ESimSol.BusinessObjects.User>();
            string sUserSQL = "SELECT * FROM View_User AS TT WHERE TT.UserID IN(SELECT DISTINCT V.DBUserID FROM Voucher AS V)";
            oUser.UserName = "-- Select User Name --";
            oUsers.Add(oUser);
            oUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sUserSQL, (int)Session[SessionInfo.currentUserID]));
            foreach (ESimSol.BusinessObjects.User oItem in oUsers)
            {
                oItem.Password = "";
                oItem.ConfirmPassword = "";
            }
            ViewBag.UserList = oUsers;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Remove(SessionInfo.SearchData);
            this.Session.Add(SessionInfo.SearchData, oSP_GeneralJournal.SP_GeneralJournalList);
            oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            return View(oSP_GeneralJournal);
        }
        #endregion
        public ActionResult PrintGeneralJournalPDF()
        {
            VoucherType oVoucherType = new VoucherType();
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            AccountingSession oAccountingSession = new AccountingSession();
            string TotalDebitAmountInString = "";
            string TotalCreditAmountInString = "";
            try
            {
                oSP_GeneralJournal = (SP_GeneralJournal)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oSP_GeneralJournal = null;
            }

            if (oSP_GeneralJournal != null)
            {
                if (oSP_GeneralJournal.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.StartDate = oAccountingSession.StartDate;
                    oSP_GeneralJournal.EndDate = DateTime.Now;
                }



                List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
                if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
                {
                    oSP_GeneralJournals.Add(_oSP_GeneralJournal);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string sjson = serializer.Serialize((object)oSP_GeneralJournals);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    #region Check Authorize Business Unit
                    if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                    #endregion

                    oSP_GeneralJournals = new List<SP_GeneralJournal>();
                    string sSQL = GetSQL(oSP_GeneralJournal);
                    oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;

                    oSP_GeneralJournal.SP_GeneralJournalList = GetsListAccordingToConfig(oSP_GeneralJournal, oSP_GeneralJournal.ACConfigs);

                    List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                    oTempSPGJs = oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                    foreach (SP_GeneralJournal oItem in oTempSPGJs)
                    {
                        oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID == oItem.VoucherID && x.VoucherDetailID != oItem.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                    }

                    TotalDebitAmountInString = SP_GeneralJournal.GetTotalBalance(true, oSP_GeneralJournal.SP_GeneralJournalList);
                    TotalCreditAmountInString = SP_GeneralJournal.GetTotalBalance(false, oSP_GeneralJournal.SP_GeneralJournalList);

                    oVoucherType = new VoucherType();
                    oVoucherType = oVoucherType.Get(oSP_GeneralJournal.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                    if (oBusinessUnit.BusinessUnitID > 0)
                    {
                        oCompany.Name = oBusinessUnit.Name;
                    }
                    oSP_GeneralJournal.Company = oCompany;
                }
            }
            else
            {
                oSP_GeneralJournal = new SP_GeneralJournal();
                oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            }

            this.Session.Remove(SessionInfo.ParamObj);

            rptGeneralJournals orptGeneralJournal = new rptGeneralJournals();
            byte[] abytes = orptGeneralJournal.PrepareReport(oSP_GeneralJournal, TotalDebitAmountInString, TotalCreditAmountInString, oVoucherType.VoucherName);
            return File(abytes, "application/pdf");

        }

        public void ExportGeneralJournalToExcel()
        {
            #region DataGet
            VoucherType oVoucherType = new VoucherType();
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            AccountingSession oAccountingSession = new AccountingSession();
            string TotalDebitAmountInString = "";
            string TotalCreditAmountInString = "";
            try
            {
                oSP_GeneralJournal = (SP_GeneralJournal)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oSP_GeneralJournal = null;
            }

            if (oSP_GeneralJournal != null)
            {
                if (oSP_GeneralJournal.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.StartDate = oAccountingSession.StartDate;
                    oSP_GeneralJournal.EndDate = DateTime.Now;
                }



                List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
                if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
                {
                    oSP_GeneralJournals.Add(_oSP_GeneralJournal);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;
                }
                else
                {
                    oSP_GeneralJournals = new List<SP_GeneralJournal>();
                    string sSQL = GetSQL(oSP_GeneralJournal);
                    oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;

                    oSP_GeneralJournal.SP_GeneralJournalList = GetsListAccordingToConfig(oSP_GeneralJournal, oSP_GeneralJournal.ACConfigs);

                    List<SP_GeneralJournal> oTempSPGJs = new List<SP_GeneralJournal>();
                    oTempSPGJs = oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID > 0).GroupBy(x => x.VoucherID).Select(g => g.First()).ToList();
                    foreach (SP_GeneralJournal oItem in oTempSPGJs)
                    {
                        oSP_GeneralJournal.SP_GeneralJournalList.Where(x => x.VoucherID == oItem.VoucherID && x.VoucherDetailID != oItem.VoucherDetailID && x.VoucherDetailID > 0).ToList().ForEach(x => { x.VoucherID = 0; x.VoucherNo = ""; x.VoucherName = ""; });
                    }

                    TotalDebitAmountInString = SP_GeneralJournal.GetTotalBalance(true, oSP_GeneralJournal.SP_GeneralJournalList);
                    TotalCreditAmountInString = SP_GeneralJournal.GetTotalBalance(false, oSP_GeneralJournal.SP_GeneralJournalList);

                    oVoucherType = new VoucherType();
                    oVoucherType = oVoucherType.Get(oSP_GeneralJournal.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);
                    Company oCompany = new Company();
                    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                    if (oBusinessUnit.BusinessUnitID > 0)
                    {
                        oCompany.Name = oBusinessUnit.Name;
                    }
                    oSP_GeneralJournal.Company = oCompany;
                }
            }
            else
            {
                oSP_GeneralJournal = new SP_GeneralJournal();
                oSP_GeneralJournal.SP_GeneralJournalList = new List<SP_GeneralJournal>();
            }
            this.Session.Remove(SessionInfo.ParamObj);
            #endregion
            #region Export To Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("General Journal");
                sheet.Name = "General Journal";
                sheet.Column(2).Width = 12;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 25;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                nEndCol = 8;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oSP_GeneralJournal.Company.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = oSP_GeneralJournal.Company.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = oSP_GeneralJournal.Company.Phone + ";  " + oSP_GeneralJournal.Company.Email + ";  " + oSP_GeneralJournal.Company.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; 
                cell.Value = "General Journal"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                nRowIndex = nRowIndex + 2;


                if (oVoucherType.VoucherTypeID > 0)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 2]; cell.Merge = true;
                    cell.Value = "Journal Date : " + _oSP_GeneralJournal.StartDateInString + " --to-- " + _oSP_GeneralJournal.EndDateInString; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Voucher :" + oVoucherType.VoucherName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                    
                    nRowIndex = nRowIndex + 2;

                }
                else
                {
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Journal Date : " + _oSP_GeneralJournal.StartDateInString + " --to-- " + _oSP_GeneralJournal.EndDateInString; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                    nRowIndex = nRowIndex + 2;
                }
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Particulars"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Account Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Voucher"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Debit(" + oSP_GeneralJournal.Company.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Credit(" + oSP_GeneralJournal.Company.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                int nCount = 0;
                //Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (SP_GeneralJournal oItem in oSP_GeneralJournal.SP_GeneralJournalList)
                {
                    if (oItem.VoucherID > 0 && nCount > 1)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;
                    }

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.VoucherDateInString; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    if (oItem.VoucherDetailID > 0)
                    {
                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.VoucherName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        if (oItem.DebitAmount > 0)
                        {
                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }

                        if (oItem.CreditAmount > 0)
                        {
                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        }
                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, 3, nRowIndex, 6]; cell.Merge = true; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    }

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    nCount++;
                    //nDebitClosing = nDebitClosing + oItem.DebitAmount;
                    //nCreditClosing = nCreditClosing + oItem.CreditAmount;
                }

                #endregion

                #region Total
                string sStartCellName = "", sEndCellName = "";
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                sStartCellName = Global.GetExcelCellName(nStartRow + 1, 7);
                sEndCellName = Global.GetExcelCellName(nEndRow, 7);
                cell = sheet.Cells[nRowIndex, 7]; cell.Formula = "SUM(" + sStartCellName + ":" + sEndCellName + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;

                sStartCellName = Global.GetExcelCellName(nStartRow + 1, 8);
                sEndCellName = Global.GetExcelCellName(nEndRow, 8);
                cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCellName + ":" + sEndCellName + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; 
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thick;
                

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                //fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(Color.LightGray);

                //cell = sheet.Cells[nStartRow, nStartCol, nEndRow, nEndCol];
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=General_Journal.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion
        #endregion

     

        public ActionResult PrintGeneralJournal(DateTime date1, DateTime date2, int nVoucherTypeID, string VoucherTypInString, int nDateSearch, int nBusinessUnitID)
        {

            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            oSP_GeneralJournal.StartDate = date1;
            oSP_GeneralJournal.EndDate = date2;
            oSP_GeneralJournal.VoucherTypeID = nVoucherTypeID;
            oSP_GeneralJournal.BusinessUnitID = nBusinessUnitID;
            if (nDateSearch == 1) //EqualTo
            {
                oSP_GeneralJournal.EndDate = oSP_GeneralJournal.StartDate;
            }
            if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
            {
                string sMessage = _oSP_GeneralJournal.ErrorMessage;

                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                string sSQL = GetSQL(oSP_GeneralJournal);
                _oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                oSP_GeneralJournal.GeneralJournalList = _oSP_GeneralJournals;
                string TotalDebitAmountInString = SP_GeneralJournal.GetTotalBalance(true, oSP_GeneralJournal.GeneralJournalList);
                string TotalCreditAmountInString = SP_GeneralJournal.GetTotalBalance(false, oSP_GeneralJournal.GeneralJournalList);
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (nBusinessUnitID > 0)
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                oSP_GeneralJournal.Company = oCompany;
                oSP_GeneralJournal.StartDate = date1;
                oSP_GeneralJournal.EndDate = date2;

                bool bCheckACConfing = false;
                List<ACConfig> oACConfigs = new List<ACConfig>();
                string sSQLACConfig = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GJAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GJVC;
                oACConfigs = ACConfig.Gets(sSQLACConfig, (int)Session[SessionInfo.currentUserID]);
                foreach (ACConfig oItem in oACConfigs)
                {
                    if (oItem.ConfigureType != EnumConfigureType.GJVoucherNarration)
                    {
                        bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                        if (bConfigureValue == true)
                        {
                            bCheckACConfing = true;
                            break;
                        }
                    }
                }
                if (bCheckACConfing == true)
                {
                    oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournal.GeneralJournalList;
                    oSP_GeneralJournal.GeneralJournalList = GetsListAccordingToConfig(oSP_GeneralJournal, oACConfigs);
                }

                rptGeneralJournals orptGeneralJournal = new rptGeneralJournals();
                byte[] abytes = orptGeneralJournal.PrepareReport(oSP_GeneralJournal, TotalDebitAmountInString, TotalCreditAmountInString, VoucherTypInString);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintGeneralJournalInXL(DateTime date1, DateTime date2, int nVoucherTypeID, int nDateSearch, int nBusinessUnitID)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];

            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            DateTime StartDate = date1;
            DateTime EndDate = date2;
            oSP_GeneralJournal.StartDate = date1;
            oSP_GeneralJournal.EndDate = date2;
            oSP_GeneralJournal.VoucherTypeID = nVoucherTypeID;
            oSP_GeneralJournal.BusinessUnitID = nBusinessUnitID;

            if (nDateSearch == 1) // EqualTo
            {
                oSP_GeneralJournal.EndDate = oSP_GeneralJournal.StartDate;
            }

            if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
            {
                string sMessage = _oSP_GeneralJournal.ErrorMessage;
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            else
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                _oSP_GeneralJournals = new List<SP_GeneralJournal>();
                string sSQL = GetSQL(oSP_GeneralJournal);
                _oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                var stream = new MemoryStream();
                var serializer = new XmlSerializer(typeof(List<GeneralJournalXL>));

                //We load the data

                int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
                GeneralJournalXL oGeneralJournalXL = new GeneralJournalXL();
                List<GeneralJournalXL> oGeneralJournalXLs = new List<GeneralJournalXL>();
                foreach (SP_GeneralJournal oItem in _oSP_GeneralJournals)
                {
                    nCount++;
                    oGeneralJournalXL = new GeneralJournalXL();
                    oGeneralJournalXL.SLNo = nCount.ToString();
                    oGeneralJournalXL.VoucherDate = oItem.VoucherDateInString;
                    oGeneralJournalXL.VoucherNo = oItem.VoucherNo;
                    oGeneralJournalXL.AccountCode = oItem.AccountCode;
                    oGeneralJournalXL.AccountHeadName = oItem.AccountHeadName;
                    oGeneralJournalXL.DebitAmount = oItem.DebitAmount;
                    oGeneralJournalXL.CreditAmount = oItem.CreditAmount;
                    oGeneralJournalXLs.Add(oGeneralJournalXL);
                    nTotalDebit = nTotalDebit + oItem.DebitAmount;
                    nTotalCredit = nTotalCredit + oItem.CreditAmount;
                }

                #region Total
                oGeneralJournalXL = new GeneralJournalXL();
                oGeneralJournalXL.SLNo = "";
                oGeneralJournalXL.VoucherDate = "";
                oGeneralJournalXL.VoucherNo = "";
                oGeneralJournalXL.AccountCode = "";
                if (_oSP_GeneralJournals.Count > 0)
                {
                    oGeneralJournalXL.AccountHeadName = "Total :";
                    oGeneralJournalXL.DebitAmount = nTotalDebit;
                    oGeneralJournalXL.CreditAmount = nTotalCredit;
                }
                else
                {
                    oGeneralJournalXL.AccountHeadName = "Sorry,No Data Found.";
                }
                oGeneralJournalXLs.Add(oGeneralJournalXL);
                #endregion

                //We turn it into an XML and save it in the memory
                serializer.Serialize(stream, oGeneralJournalXLs);
                stream.Position = 0;

                //We return the XML from the memory as a .xls file
                return File(stream, "application/vnd.ms-excel", "General Journal.xls");
            }
        }
        private bool ValidateInputForGeneralJournal(SP_GeneralJournal oSP_GeneralJournal)
        {
            _oSP_GeneralJournal = new SP_GeneralJournal();
            if (oSP_GeneralJournal.EndDate < oSP_GeneralJournal.StartDate)
            {
                _oSP_GeneralJournal.ErrorMessage = "End Date must be grater/equal then start date";
                return false;
            }
            return true;
        }

        #region General Journal for Accounts Book
        public ActionResult DetailsBreakdown(string sParams)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];

            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            oSP_GeneralJournal.AccountHeadID = Convert.ToInt32(sParams.Split('~')[0]);
            oSP_GeneralJournal.VoucherTypeID = Convert.ToInt32(sParams.Split('~')[1]);
            oSP_GeneralJournal.StartDate = Convert.ToDateTime(sParams.Split('~')[2]);
            oSP_GeneralJournal.EndDate = Convert.ToDateTime(sParams.Split('~')[3]);


            #region Check Configuration
            bool bACConfing = false;
            oSP_GeneralJournal.DisplayMode = (EnumDisplayMode)oSP_GeneralJournal.DisplayModeInInt;
            List<ACConfig> oACConfigs = new List<ACConfig>();
            string sSQLACConfig = "SELECT * FROM ACConfig WHERE  ConfigureType >= " + (int)EnumConfigureType.GJAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GJVC;
            oACConfigs = ACConfig.Gets(sSQLACConfig, (int)Session[SessionInfo.currentUserID]);
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
            #endregion

            List<SP_GeneralJournal> oSP_GeneralJournals = new List<SP_GeneralJournal>();
            if (!ValidateInputForGeneralJournal(oSP_GeneralJournal))
            {
                oSP_GeneralJournals.Add(_oSP_GeneralJournal);
                oSP_GeneralJournal.SP_GeneralJournalList = oSP_GeneralJournals;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string sjson = serializer.Serialize((object)oSP_GeneralJournals);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            else
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oSP_GeneralJournal.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                _oSP_GeneralJournals = new List<SP_GeneralJournal>();
                string sSQL = GetSQL(oSP_GeneralJournal);
                _oSP_GeneralJournals = SP_GeneralJournal.GetsGeneralJournal(sSQL, (int)Session[SessionInfo.currentUserID]);
                oSP_GeneralJournal.SP_GeneralJournalList = _oSP_GeneralJournals;

                if (bACConfing == true)
                {
                    _oSP_GeneralJournals = GetsListAccordingToConfig(oSP_GeneralJournal, oACConfigs);
                    oSP_GeneralJournal.SP_GeneralJournalList = _oSP_GeneralJournals;
                }

                #region Display Mode
                if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                {
                    List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                    SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                    DateTime dLedgerDate = Convert.ToDateTime("01 Jan 2001");

                    foreach (SP_GeneralJournal oItem in _oSP_GeneralJournals)
                    {
                        if (oItem.ConfigType == EnumConfigureType.None)
                        {
                            #region Individual Month
                            if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView)
                            {
                                if (dLedgerDate.ToString("MMM yyyy") != oItem.VoucherDate.ToString("MMM yyyy"))
                                {
                                    oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                    oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                }
                            }
                            #endregion
                            #region Individual Date
                            else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                            {
                                if (dLedgerDate.ToString("dd MMM yyyy") != oItem.VoucherDate.ToString("dd MMM yyyy"))
                                {
                                    oTempGeneralJournal = NewObject(oItem, oSP_GeneralJournal.DisplayMode);
                                    oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                                }
                            }
                            #endregion
                            dLedgerDate = oItem.VoucherDate;
                        }
                    }

                    if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.MonthlyView || oSP_GeneralJournal.DisplayMode == EnumDisplayMode.DateView)
                    {
                        foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                        {
                            oItem.DisplayVouchers = GetVouchers(oItem.VoucherDate, _oSP_GeneralJournals, oSP_GeneralJournal.DisplayMode);
                        }
                    }

                    foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                    {
                        double nDrAmount = 0;
                        double nCrAmount = 0;
                        foreach (SP_GeneralJournal oItemVoucher in oItem.DisplayVouchers)
                        {
                            nDrAmount = nDrAmount + oItemVoucher.DebitAmount;
                            nCrAmount = nCrAmount + oItemVoucher.CreditAmount;
                        }
                        oItem.DebitAmount = nDrAmount;
                        oItem.CreditAmount = nCrAmount;
                    }
                    oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;
                }
                #region Individual Week
                else if (oSP_GeneralJournal.DisplayMode == EnumDisplayMode.WeeklyView)
                {
                    List<SP_GeneralJournal> oDisplayModeWiseGeneralJournals = new List<SP_GeneralJournal>();
                    SP_GeneralJournal oTempGeneralJournal = new SP_GeneralJournal();

                    DateTime dStartDate = oSP_GeneralJournal.StartDate;
                    int nCountWeek = 1;
                    while (dStartDate <= oSP_GeneralJournal.EndDate.Date)
                    {
                        oTempGeneralJournal = new SP_GeneralJournal();
                        oTempGeneralJournal.WeekStartDate = dStartDate;
                        oTempGeneralJournal.WeekEndDate = dStartDate.AddDays(6);
                        oTempGeneralJournal.ConfigTitle = "Week " + nCountWeek + " : (" + oTempGeneralJournal.WeekStartDate.ToString("MMM") + " " + oTempGeneralJournal.WeekStartDate.ToString("dd") + " - " + oTempGeneralJournal.WeekEndDate.ToString("MMM") + " " + oTempGeneralJournal.WeekEndDate.ToString("dd") + ")";
                        oDisplayModeWiseGeneralJournals.Add(oTempGeneralJournal);
                        dStartDate = dStartDate.AddDays(7);
                        nCountWeek++;
                    }

                    foreach (SP_GeneralJournal oItem in oDisplayModeWiseGeneralJournals)
                    {
                        SP_GeneralJournal oTempGJ = new SP_GeneralJournal();
                        DateTime dEndDate = oItem.VoucherDate.AddDays(7);
                        double nDebitAmount = 0;
                        double nCreditAmount = 0;
                        foreach (SP_GeneralJournal oGJ in _oSP_GeneralJournals)
                        {
                            if (oGJ.VoucherDate >= oItem.WeekStartDate && oGJ.VoucherDate <= oItem.WeekEndDate)
                            {
                                oTempGJ = new SP_GeneralJournal();
                                oTempGJ.ConfigTitle = oGJ.VoucherDate.ToString("dd MMM yyyy");
                                oTempGJ.VoucherID = oGJ.VoucherID;
                                oTempGJ.VoucherNo = oGJ.VoucherNo;
                                oTempGJ.VoucherDate = oGJ.VoucherDate;
                                oTempGJ.AccountCode = oGJ.AccountCode;
                                oTempGJ.AccountHeadName = oGJ.AccountHeadName;
                                oTempGJ.DebitAmount = oGJ.DebitAmount;
                                oTempGJ.CreditAmount = oGJ.CreditAmount;
                                oTempGJ.SP_GeneralJournalList = oGJ.SP_GeneralJournalList;
                                oItem.DisplayVouchers.Add(oTempGJ);
                                nDebitAmount = nDebitAmount + oTempGJ.DebitAmount;
                                nCreditAmount = nCreditAmount + oTempGJ.CreditAmount;
                            }
                        }
                        oItem.DebitAmount = nDebitAmount;
                        oItem.CreditAmount = nCreditAmount;
                    }
                    oSP_GeneralJournal.SP_GeneralJournalList = oDisplayModeWiseGeneralJournals;
                }
                #endregion
                #endregion
                oSP_GeneralJournal.DisplayModes = EnumObject.jGets(typeof(EnumDisplayMode));
                return PartialView(oSP_GeneralJournal);
            }
        }
        #endregion

        
    }
}