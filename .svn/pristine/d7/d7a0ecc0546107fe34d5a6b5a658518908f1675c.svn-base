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
using System.Linq;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class TrialBalance_CategorizedController : Controller
    {
        #region Declaration
        TrialBalance_Categorized _oTrialBalance_Categorized = new TrialBalance_Categorized();
        List<TrialBalance_Categorized> _oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
        TTrialBalance_Categorized _oTTrialBalance_Categorized = new TTrialBalance_Categorized();
        List<TTrialBalance_Categorized> _oTTrialBalance_Categorizeds = new List<TTrialBalance_Categorized>();

        List<SP_GeneralLedger> _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
        SP_GeneralLedger _oSP_GeneralLedger = new SP_GeneralLedger();
        #endregion

        #region Functions
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

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

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

            string sReturn1 = "SELECT VD.VoucherID, VD.AccountHeadID FROM View_VoucherDetail AS VD ";
            string sReturn = "";

            #region BusinessUnit
            if (oGeneralLedger.BusinessUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.BUID =" + oGeneralLedger.BusinessUnitID.ToString();
            }
            #endregion

            if (oGeneralLedger.IsApproved)
            {
                Global.TagSQL(ref sReturn);
                if (oCompany.BaseCurrencyID == oGeneralLedger.CurrencyID)
                {
                    sReturn = sReturn + "  VD.AccountHeadID = " + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                else
                {
                    sReturn = sReturn + "  VD.CurrencyID= " + oGeneralLedger.CurrencyID + " AND VD.AccountHeadID=" + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE V.AuthorizedBy!=0 AND  CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
            }
            else
            {
                Global.TagSQL(ref sReturn);
                if (oCompany.BaseCurrencyID == oGeneralLedger.CurrencyID)
                {
                    sReturn = sReturn + "  VD.AccountHeadID = " + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106))) ";
                }
                else
                {
                    sReturn = sReturn + "  VD.CurrencyID = " + oGeneralLedger.CurrencyID + " AND VD.AccountHeadID=" + oGeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE CONVERT(DATE,CONVERT(VARCHAR(12),V.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oGeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                }
            }

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
                    if (sAccountHeadName != null && sAccountHeadName != "")
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " AccountHeadID IN (SELECT AccountHeadID FROM COA_ChartsOfAccount WHERE AccountHeadName LIKE '%" + sAccountHeadName + "%') AND AccountHeadID != " + oGeneralLedger.AccountHeadID + "  ";
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
        #region GL According to Config
        private List<SP_GeneralLedger> GetsListAccordingToConfig(SP_GeneralLedger oSP_GeneralLedger, List<ACConfig> oACConfigs)
        {
            List<SP_GeneralLedger> oNewSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
           

            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherBillTransaction> oSubLedgerBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherCheque> oSLVoucherCheques = new List<VoucherCheque>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            //List<VoucherReference> oVoucherReferences = new List<VoucherReference>();
            string sSQL = "";
            if (oSP_GeneralLedger.IsApproved == true)
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //sSQL = "SELECT * FROM View_VoucherReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                //oVoucherReferences = VoucherReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherCheque AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSLVoucherCheques = VoucherCheque.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //sSQL = "SELECT * FROM View_VoucherReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                //oVoucherReferences = VoucherReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            

            foreach (SP_GeneralLedger oSPGL in oSP_GeneralLedger.SP_GeneralLedgerList)
            {
                oNewSP_GeneralLedgers.Add(oSPGL);
                foreach (ACConfig oItem in oACConfigs)
                {
                    if (oItem.ConfigureValue == "1")
                    {

                        #region Account Head Narration
                        if (oItem.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
                        {
                            oNewSP_GeneralLedger = GetNarration(EnumConfigureType.GLAccHeadWiseNarration, oSPGL);
                            oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                            if (oNewSP_GeneralLedger.IsNullOrNot == false)
                            {
                                oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                            }
                        }
                        #endregion
                        #region Voucher Narration
                        else if (oItem.ConfigureType == EnumConfigureType.GLVoucherNarration)
                        {
                            oNewSP_GeneralLedger = GetNarration(EnumConfigureType.GLVoucherNarration, oSPGL);
                            oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                            if (oNewSP_GeneralLedger.IsNullOrNot == false)
                            {
                                oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                            }
                        }
                        #endregion
                        #region Cost Center Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GLCC)
                        {
                            if (oCostCenterTransactions.Count > 0)
                            {
                                oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                                oTempSP_GeneralLedgers = GetCC(oCostCenterTransactions, oSPGL.VoucherDetailID, oSPGL.VoucherDate, oACConfigs, oSubLedgerBillTransactions, oSLVoucherCheques);
                                oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);

                            }
                        }
                        #endregion
                        #region Voucher Bill Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GLBT)
                        {
                            if (oVoucherBillTransactions.Count > 0)
                            {
                                oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                                oTempSP_GeneralLedgers = GetBT(oVoucherBillTransactions, oSPGL.VoucherDetailID, 0, oSPGL.VoucherDate);
                                oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                            }
                        }
                        #endregion
                        #region VP (IR) Transaction
                        else if (oItem.ConfigureType == EnumConfigureType.GLIR)
                        {
                            if (oVPTransactions.Count > 0)
                            {
                                oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                                oTempSP_GeneralLedgers = GetIR(oVPTransactions, oSPGL.VoucherDetailID, oSPGL.VoucherDate);
                                oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                            }
                        }
                        #endregion
                        #region Voucher Reference
                        //else if (oItem.ConfigureType == EnumConfigureType.GLVR && bConfigureValue == true)
                        //{
                        //    if (oVoucherReferences.Count > 0)
                        //    {
                        //        oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                        //        oTempSP_GeneralLedgers = GetVR(oVoucherReferences, oSPGL.VoucherDetailID, oCompany, oSPGL.VoucherDate);
                        //        oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        //    }
                        //}
                        #endregion
                    }
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
                    oSP_GeneralLedger.ConfigTitle = "CC ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLCC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Name : " + oItem.CostCenterName + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
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
                    oSP_GeneralLedger.ConfigTitle = oItem.CCTID > 0 ? "SL BT " : "BT ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLBT;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Bill No : " + oItem.BillNo + " Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
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
                    oSP_GeneralLedger.ConfigTitle = "IR ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLIR;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Product Name :" + oItem.ProductName + " WU : " + oItem.WorkingUnitName + " MUnit : " + oItem.MUnitName + " Qty : " + Global.MillionFormat(oItem.Qty) + " Unit Price : " + Global.MillionFormat(oItem.UnitPrice) + " Amount : " + Global.MillionFormat(oItem.Amount) + bIsDr;
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
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID==nCCTID)
                {
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    oSP_GeneralLedger.ConfigTitle = oItem.CCTID > 0 ? "SL Cheque" : "Cheque";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLVC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Cheque No :" + oItem.ChequeNo + " Account No :" + oItem.AccountNo + " Amount : " + Global.MillionFormat(oItem.Amount);
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private SP_GeneralLedger GetNarration(EnumConfigureType eEnumConfigureType, SP_GeneralLedger oSP_GeneralLedger)
        {
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            if (eEnumConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
            {
                oNewSP_GeneralLedger.ConfigTitle = " Head Wise Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.Narration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
            }
            else if (eEnumConfigureType == EnumConfigureType.GLVoucherNarration)
            {
                oNewSP_GeneralLedger.ConfigTitle = " Voucher Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.VoucherNarration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
            }
            return oNewSP_GeneralLedger;
        }
        #endregion

        #region CC GL According to Config
        private List<CostCenterBreakdown> GetsCCGLAccordingToConfig(CostCenterBreakdown oCostCenterBreakdown, List<ACConfig> oACConfigs)
        {
            List<CostCenterBreakdown> oNewCostCenterBreakdowns = new List<CostCenterBreakdown>();
            CostCenterBreakdown oNewCostCenterBreakdown = new CostCenterBreakdown();
            List<CostCenterBreakdown> oTempCostCenterBreakdowns = new List<CostCenterBreakdown>();


            List<VoucherBillTransaction> oSubLedgerBillTransactions = new List<VoucherBillTransaction>();
            string sSQL = "";
            if (oCostCenterBreakdown.IsApproved == true)
            {
                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.CCTID!=0 AND TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oCostCenterBreakdown.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oCostCenterBreakdown.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oSubLedgerBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
                                oTempCostCenterBreakdowns = GetCCGLBT(oSubLedgerBillTransactions, oCCGL.VoucherDetailID, oCCGL.CCID, oCCGL.VoucherDate);
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
            foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID && oItem.CCTID == nCCTID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oCostCenterBreakdown = new CostCenterBreakdown();
                    oCostCenterBreakdown.ConfigTitle = oItem.CCTID > 0 ? "SL BT " : "BT ";
                    oCostCenterBreakdown.ConfigType = EnumConfigureType.GLBT;
                    oCostCenterBreakdown.VoucherDate = dVoucherDate;
                    oCostCenterBreakdown.VoucherNo = " Bill No : " + oItem.BillNo + "Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oCostCenterBreakdowns.Add(oCostCenterBreakdown);
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
        #endregion

        #region New Version
        #region Set TBC SessionData
        [HttpPost]
        public ActionResult SetTBCSessionData(TrialBalance_Categorized oTrialBalance_Categorized)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oTrialBalance_Categorized);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ViewComponentTBCs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                oTrialBalance_Categorized = (TrialBalance_Categorized)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oTrialBalance_Categorized = null;
            }
            if (oTrialBalance_Categorized != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oTrialBalance_Categorized.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }
            else
            {
                oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ParentAccountHeadID = 0;
                oTrialBalance_Categorized.AccountType = EnumAccountType.None;
                oTrialBalance_Categorized.IsApproved = true;
                oTrialBalance_Categorized.CurrencyID = oCompany.BaseCurrencyID;
                oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.StartDate = oAccountingSession.StartDate;
                oTrialBalance_Categorized.EndDate = oAccountingSession.EndDate;
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }



            ViewBag.Company = oCompany;
            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oTrialBalance_Categorized.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oTrialBalance_Categorized);
        }
        public ActionResult ViewSegmentTBCs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            try
            {
                oTrialBalance_Categorized = (TrialBalance_Categorized)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                oTrialBalance_Categorized = null;
            }
            if (oTrialBalance_Categorized != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oTrialBalance_Categorized.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }
            else
            {
                oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ParentAccountHeadID = 0;
                oTrialBalance_Categorized.AccountType = EnumAccountType.None;
                oTrialBalance_Categorized.IsApproved = true;
                oTrialBalance_Categorized.CurrencyID = oCompany.BaseCurrencyID;
                oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.StartDate = oAccountingSession.StartDate;
                oTrialBalance_Categorized.EndDate = oAccountingSession.EndDate;
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }



            ViewBag.Company = oCompany;
            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oTrialBalance_Categorized.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oTrialBalance_Categorized);
        }
        public ActionResult ViewGroupTBCs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oTrialBalance_Categorized = (TrialBalance_Categorized)Session[SessionInfo.ParamObj];
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oTrialBalance_Categorized != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oTrialBalance_Categorized.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }
            else
            {
                oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ParentAccountHeadID = 0;
                oTrialBalance_Categorized.AccountType = EnumAccountType.None;
                oTrialBalance_Categorized.IsApproved = true;
                oTrialBalance_Categorized.CurrencyID = oCompany.BaseCurrencyID;
                oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.StartDate = oAccountingSession.StartDate;
                oTrialBalance_Categorized.EndDate = oAccountingSession.EndDate;
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }



            ViewBag.Company = oCompany;
            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oTrialBalance_Categorized.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oTrialBalance_Categorized);
        }
        public ActionResult ViewSubGroupTBCs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oTrialBalance_Categorized = (TrialBalance_Categorized)Session[SessionInfo.ParamObj];
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oTrialBalance_Categorized != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(oTrialBalance_Categorized.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }
            else
            {
                oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ParentAccountHeadID = 0;
                oTrialBalance_Categorized.AccountType = EnumAccountType.None;
                oTrialBalance_Categorized.IsApproved = true;
                oTrialBalance_Categorized.CurrencyID = oCompany.BaseCurrencyID;
                oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.StartDate = oAccountingSession.StartDate;
                oTrialBalance_Categorized.EndDate = oAccountingSession.EndDate;
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }



            ViewBag.Company = oCompany;
            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oTrialBalance_Categorized.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oTrialBalance_Categorized);
        }
        public ActionResult ViewLedgerTBCs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();
            TrialBalance_Categorized oTrialBalance_Categorized = new TrialBalance_Categorized();
            oTrialBalance_Categorized = (TrialBalance_Categorized)Session[SessionInfo.ParamObj];
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oTrialBalance_Categorized != null)
            {
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }
            else
            {
                oTrialBalance_Categorized = new TrialBalance_Categorized();
                oTrialBalance_Categorized.ParentAccountHeadID = 0;
                oTrialBalance_Categorized.AccountType = EnumAccountType.None;
                oTrialBalance_Categorized.IsApproved = true;
                oTrialBalance_Categorized.CurrencyID = oCompany.BaseCurrencyID;
                oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.StartDate = oAccountingSession.StartDate;
                oTrialBalance_Categorized.EndDate = oAccountingSession.EndDate;
                oTBCs = TrialBalance_Categorized.Gets(oTrialBalance_Categorized.AccountHeadID, oTrialBalance_Categorized.StartDate, oTrialBalance_Categorized.EndDate, oTrialBalance_Categorized.IsApproved, oTrialBalance_Categorized.CurrencyID, oTrialBalance_Categorized.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorized.TrialBalance_Categorizeds = oTBCs;
            }



            ViewBag.Company = oCompany;
            ViewBag.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COA = new ChartsOfAccount().Get(oTrialBalance_Categorized.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(oTrialBalance_Categorized);
        }


        public ActionResult PrintTBCs(string Params)
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
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            List<TrialBalance_Categorized> oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
            oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(nAccountHeadID, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptTrialBalance_Categorizeds oReport = new rptTrialBalance_Categorizeds();
            abytes = oReport.PrepareReport(oTrialBalance_Categorizeds, sHeader, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportTBCsToExcel(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            #region Data get
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];

            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<TrialBalance_Categorized> oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
            oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(nAccountHeadID, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            #region Excel Export
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trial Balance");
                sheet.Name = "Trial Balance";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 50;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 30;
                sheet.Column(7).Width = 30;
                sheet.Column(8).Width = 30;


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
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nStartRow = nRowIndex;
                sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3].Merge = true;
                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Account Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 4, nRowIndex + 1, 4].Merge = true;
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "AccountHead Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 5, nRowIndex + 1, 5].Merge = true;
                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 6, nRowIndex + 1, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, 7, nRowIndex, 8].Merge = true;
                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex+1, 7]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex+1, 8]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 8];
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Report Data
                int nCount = 0;
                Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (TrialBalance_Categorized oItem in oTrialBalance_Categorizeds)
                {
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = ++nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DebitClosingBalance; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.CreditClosingBalance; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    nDebitClosing = nDebitClosing + oItem.DebitClosingBalance;
                    nCreditClosing = nCreditClosing + oItem.CreditClosingBalance;
                }
                nEndRow = nRowIndex-1;
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = oTrialBalance_Categorizeds.Sum(x=>x.DebitAmount); cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = oTrialBalance_Categorizeds.Sum(x => x.CreditAmount); cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 9];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[nStartRow, 2, nEndRow, 8];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Trial_Balance.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }


        public void ExportTBCsToExcel_MonthWise(string Params)
        {
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            #region Data get
            int nAccountHeadID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            int nCurrencyID = Params.Split('~')[4] == null ? 0 : Params.Split('~')[4] == "" ? 0 : Convert.ToInt32(Params.Split('~')[4]);
            bool bIsApproved = Params.Split('~')[5] == null ? false : Params.Split('~')[5] == "" ? false : Convert.ToBoolean(Params.Split('~')[5]);
            string sHeader = Params.Split('~')[6] == null ? "" : Params.Split('~')[6];


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<TrialBalance_Categorized> oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
            oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(nAccountHeadID, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nStartColumn = 2, nEndColumn = 0, nColumnIndex = 0, nCount = 0, nTotalMonths = (dEndDate.Year - dStartDate.Year) * 12 + dEndDate.Month - dStartDate.Month;
            
            DateTime [] oDateTime_Array= new DateTime[nTotalMonths+1];
            List<TrialBalance_Categorized>[] oTrialBalance_Categorizeds_Array = new List<TrialBalance_Categorized>[nTotalMonths+1];
            
            #region Get Month Wise Data
            for (int i = 0; i <= nTotalMonths; i++)
            {
                DateTime oStartDate_temp = new DateTime(dStartDate.Year, dStartDate.Month, 1);
                if(i==0) oStartDate_temp = dStartDate;
                
                oStartDate_temp = oStartDate_temp.AddMonths(i);
                oDateTime_Array[i] = oStartDate_temp;

                DateTime oEndDate_temp = oStartDate_temp.AddMonths(1).AddDays(-1);
                if (i == nTotalMonths) { oEndDate_temp = dEndDate; oDateTime_Array[i] = oEndDate_temp; }

                List<TrialBalance_Categorized> oTrialBalance_Categorizeds_Temp = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorizeds_Temp = TrialBalance_Categorized.Gets(nAccountHeadID, oStartDate_temp, oEndDate_temp, bIsApproved, nCurrencyID, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oTrialBalance_Categorizeds_Array[i] = oTrialBalance_Categorizeds_Temp;
            }
            #endregion

            #endregion

            #region Excel Export
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trial Balance");
                sheet.Name = "Trial Balance";

                nCount = nStartColumn;
                sheet.Column(nCount++).Width = 10;
                sheet.Column(nCount++).Width = 20;
                sheet.Column(nCount++).Width = 50;

                for (int i = 0; i <= nTotalMonths; i++) 
                {
                    sheet.Column(nCount++).Width = 30;
                    sheet.Column(nCount++).Width = 30;
                }

                sheet.Column(nCount++).Width = 30;
                sheet.Column(nCount++).Width = 30;
                sheet.Column(nCount++).Width = 30;
                sheet.Column(nCount++).Width = 30;
                nEndColumn = nCount;

                #region Report Header
                sheet.Cells[nRowIndex, nStartColumn, nRowIndex, nEndColumn].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, nStartColumn, nRowIndex, nEndColumn].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, nStartColumn, nRowIndex, nEndColumn].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, nStartColumn, nRowIndex, nEndColumn].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                nColumnIndex = nStartColumn;
                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex++].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex-1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex++].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex-1]; cell.Value = "Account Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex++].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex-1]; cell.Value = "AccountHead Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                for (int i = 0; i <= nTotalMonths; i++)
                {
                    int nStart_Column = nColumnIndex;
                    sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, ++nColumnIndex].Merge = true; //dStartDate.AddMonths(i).ToString("dd MMM yyyy");
                    cell = sheet.Cells[nRowIndex, nColumnIndex - 1]; cell.Value = oDateTime_Array[i].ToString("MMM yyyy"); cell.Style.Font.Bold = true; //cell.Value = oDateTime_Array[i].ToString("dd MMM yyyy");
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nColumnIndex++;

                    cell = sheet.Cells[nRowIndex + 1, nStart_Column++]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex + 1, nStart_Column]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }

                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex++].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex - 1]; cell.Value = "Debit Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex + 1, nColumnIndex++].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex - 1]; cell.Value = "Credit Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, nColumnIndex+1].Merge = true;
                cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "Closing"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex + 1, nColumnIndex]; cell.Value = "Debit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex + 1, ++nColumnIndex]; cell.Value = "Credit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                HeaderCell = sheet.Cells[nRowIndex, nStartColumn, nRowIndex + 1, nEndColumn-1];
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Report Data
                int nCount_Data = 0;
                Double nCreditClosing = 0, nDebitClosing = 0;
                foreach (TrialBalance_Categorized oItem in oTrialBalance_Categorizeds)
                {
                    nColumnIndex = nStartColumn;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = ++nCount_Data; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    for (int i = 0; i <= nTotalMonths; i++)
                    {
                        cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds_Array[i].Where(x => x.AccountHeadID == oItem.AccountHeadID).Select(x => x.DebitAmount).FirstOrDefault(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds_Array[i].Where(x => x.AccountHeadID == oItem.AccountHeadID).Select(x => x.CreditAmount).FirstOrDefault(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    }
                   
                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.DebitAmount; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.CreditAmount; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.DebitClosingBalance; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oItem.CreditClosingBalance; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nRowIndex++;

                    nDebitClosing = nDebitClosing + oItem.DebitClosingBalance;
                    nCreditClosing = nCreditClosing + oItem.CreditClosingBalance;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, nStartColumn+2]; cell.Value = "Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                nColumnIndex = nStartColumn + 3;
                for (int i = 0; i <= nTotalMonths; i++)
                {
                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds_Array[i].Sum(x => x.DebitAmount); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds_Array[i].Sum(x => x.CreditAmount); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                }

                cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds.Sum(x => x.DebitAmount); cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = oTrialBalance_Categorizeds.Sum(x => x.CreditAmount); cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = nDebitClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, nColumnIndex++]; cell.Value = nCreditClosing; cell.Style.Font.Bold = true;
                border = cell.Style.Border; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndColumn];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                fill = HeaderCell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);

                cell = sheet.Cells[8, nStartColumn, nRowIndex, nEndColumn - 1];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Trial_Balance.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        #endregion

        public ActionResult ViewTrialBalance_Categorized(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Company oCompany = new Company();
            List<TrialBalance_Categorized> oTBCs = new List<TrialBalance_Categorized>();
            AccountingSession oAccountingSession = new AccountingSession();

            oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
            oTBCs = TrialBalance_Categorized.Gets(0, oAccountingSession.StartDate, oAccountingSession.EndDate, true, 1, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.StartDate = oAccountingSession.StartDateString;
            ViewBag.EndDate = oAccountingSession.EndDateTimeString;
            ViewBag.Company = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

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
            ViewBag.Level = 1;
            return View(oTBCs);
        }

        [HttpPost]
        public JsonResult Gets(TrialBalance_Categorized oTBC)
        {
            string sjson = "";
            #region TB
            if (oTBC.Level <= 4)
            {
                _oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();

                _oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(oTBC.AccountHeadID, oTBC.StartDate, oTBC.EndDate, true, 1, oTBC.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize((object)_oTrialBalance_Categorizeds);
            }
            #endregion
            #region Monthly GL
            else if (oTBC.Level == 5)
            {
                List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();

                oGLMonthlySummarys = GLMonthlySummary.Gets(oTBC.AccountHeadID, oTBC.StartDate, oTBC.EndDate, 1, true, oTBC.BusinessUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize((object)oGLMonthlySummarys);
            }
            #endregion
            #region GL
            else if (oTBC.Level == 6)
            {
                List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
                oSP_GeneralLedger.CurrencyID = 1;
                oSP_GeneralLedger.IsApproved = true;
                oSP_GeneralLedger.ACConfigs = oTBC.ACConfigs;
                oSP_GeneralLedger.StartDate = oTBC.StartDate;
                oSP_GeneralLedger.EndDate = oTBC.EndDate;
                oSP_GeneralLedger.AccountHeadID = oTBC.AccountHeadID;
                oSP_GeneralLedger.BusinessUnitID = oTBC.BusinessUnitID;
                string sSQL = GetSQL(oSP_GeneralLedger);

                oSP_GeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oSP_GeneralLedger.AccountHeadID, oSP_GeneralLedger.StartDate, oSP_GeneralLedger.EndDate, oSP_GeneralLedger.CurrencyID, oSP_GeneralLedger.IsApproved, oSP_GeneralLedger.BusinessUnitIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oSP_GeneralLedger.SP_GeneralLedgerList = oSP_GeneralLedgers;
                if (oSP_GeneralLedger.ACConfigs.Count > 0)
                {
                    oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                    oSP_GeneralLedgers = GetsListAccordingToConfig(oSP_GeneralLedger, oSP_GeneralLedger.ACConfigs);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize((object)oSP_GeneralLedgers);
            }
            #endregion
            #region BreakDown Transactions
            else if (oTBC.Level == 7)
            {
                if (oTBC.OptionID == 1)
                {
                    List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
                    CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
                    oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.Gets(oTBC.BusinessUnitID.ToString(), oTBC.AccountHeadID, 1, oTBC.StartDate, oTBC.EndDate, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize((object)oCostCenterBreakdowns);
                }
            }
            #endregion
            #region BreakDown Ledger
            else if (oTBC.Level == 8)
            {
                if (oTBC.OptionID == 1)
                {
                    CostCenterBreakdown oCostCenterBreakdown=new CostCenterBreakdown();

                    
                    List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();

                    oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(oTBC.BusinessUnitID.ToString(), oTBC.AccountHeadID, oTBC.CCID, 1, oTBC.StartDate, oTBC.EndDate, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oCostCenterBreakdown.IsApproved=true;
                    oCostCenterBreakdown.AccountHeadID = oTBC.AccountHeadID;
                    oCostCenterBreakdown.StartDate = oTBC.StartDate;
                    oCostCenterBreakdown.EndDate = oTBC.EndDate;
                    oCostCenterBreakdown.CostCenterBreakdowns=oCostCenterBreakdowns;

                    if (oTBC.ACConfigs.Count > 0)
                    {
                        oCostCenterBreakdowns = new List<CostCenterBreakdown>();
                        oCostCenterBreakdowns = this.GetsCCGLAccordingToConfig(oCostCenterBreakdown, oTBC.ACConfigs);
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize((object)oCostCenterBreakdowns);
                }
            }
            #endregion
            #region CC BT
            else if (oTBC.Level == 9)
            {
                if (oTBC.OptionID == 1 && oTBC.CCOptionID == 5)
                {
                    List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
                    VoucherRefReport oVoucherRefReport = new VoucherRefReport();
                    oVoucherRefReport.AccountHeadID = oTBC.AccountHeadID;
                    oVoucherRefReport.StartDate = oTBC.StartDate;
                    oVoucherRefReport.EndDate = oTBC.EndDate;
                    oVoucherRefReport.CCID = oTBC.CCID;
                    oVoucherRefReport.BusinessUnitID = oTBC.BusinessUnitID;
                    oVoucherRefReport.VoucherBillID = oTBC.VoucherBillID;
                    oVoucherRefReport.IsApproved = true;
                    oVoucherRefReport.CurrencyID = 1;
                    oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVoucherRefReport, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize((object)oVoucherRefReports);
                }
            }
            #endregion
            #region CC B GL
            else if (oTBC.Level == 10)
            {
                if (oTBC.OptionID == 1 && oTBC.CCOptionID == 5)
                {
                    List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
                    VoucherRefReport oVoucherRefReport = new VoucherRefReport();
                    oVoucherRefReport.AccountHeadID = oTBC.AccountHeadID;
                    oVoucherRefReport.StartDate = oTBC.StartDate;
                    oVoucherRefReport.EndDate = oTBC.EndDate;
                    oVoucherRefReport.CCID = oTBC.CCID;
                    oVoucherRefReport.BusinessUnitID = oTBC.BusinessUnitID;
                    oVoucherRefReport.VoucherBillID = oTBC.VoucherBillID;
                    oVoucherRefReport.IsApproved = true;
                    oVoucherRefReport.CurrencyID = 1;
                    oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVoucherRefReport, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    sjson = serializer.Serialize((object)oVoucherRefReports);
                }
            }
            #endregion
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                //img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrintTrialBalance_Categorized(string Params)
        {
            int nLevel = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            int nOptionID = Params.Split('~')[1] == null ? 0 : Params.Split('~')[1] == "" ? 0 : Convert.ToInt32(Params.Split('~')[1]);
            int nCCOptionID = Params.Split('~')[2] == null ? 0 : Params.Split('~')[2] == "" ? 0 : Convert.ToInt32(Params.Split('~')[2]);
            int nAccountHeadID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            DateTime dStartDate = Params.Split('~')[4] == null ? DateTime.Now : Params.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[4]);
            DateTime dEndDate = Params.Split('~')[5] == null ? DateTime.Now : Params.Split('~')[5] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[5]);
            int nBusinessUnitID = Params.Split('~')[6] == null ? 0 : Params.Split('~')[6] == "" ? 0 : Convert.ToInt32(Params.Split('~')[6]);
            int nCCID = Params.Split('~')[7] == null ? 0 : Params.Split('~')[7] == "" ? 0 : Convert.ToInt32(Params.Split('~')[7]);
            int nVoucherBillID = Params.Split('~')[8] == null ? 0 : Params.Split('~')[8] == "" ? 0 : Convert.ToInt32(Params.Split('~')[8]);
            string sHeader = Params.Split('~')[9] == null ? "" : Params.Split('~')[9];
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
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (nLevel >= 1 && nLevel <= 5)
            {
                List<TrialBalance_Categorized> oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();
                oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(nAccountHeadID, dStartDate, dEndDate, true, 1, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptTrialBalance_Categorizeds oReport = new rptTrialBalance_Categorizeds();
                abytes = oReport.PrepareReport(oTrialBalance_Categorizeds, sHeader, oCompany);
            }
            else if (nLevel == 6)
            {
                List<GLMonthlySummary> oGLMonthlySummarys = new List<GLMonthlySummary>();

                oGLMonthlySummarys = GLMonthlySummary.Gets(nAccountHeadID, dStartDate, dEndDate, 1, true, nBusinessUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptGLMonthlySummarys oReport = new rptGLMonthlySummarys();
                abytes = oReport.PrepareReport(oGLMonthlySummarys, sHeader, oCompany);
            }
            else if (nLevel == 7)
            {
                List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                SP_GeneralLedger oGeneralLedger = new SP_GeneralLedger();
                ChartsOfAccount oCOA = new ChartsOfAccount();
                oGeneralLedger.AccountHeadID = nAccountHeadID;
                oGeneralLedger.StartDate = dStartDate;
                oGeneralLedger.EndDate = dEndDate;
                oGeneralLedger.CurrencyID = 1;
                oGeneralLedger.IsApproved = true;
                string sSQL = GetSQL(oGeneralLedger);
                _oSP_GeneralLedgers = SP_GeneralLedger.GetsGeneralLedger(oGeneralLedger.AccountHeadID, oGeneralLedger.StartDate, oGeneralLedger.EndDate, oGeneralLedger.CurrencyID, oGeneralLedger.IsApproved, nBusinessUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
                oGeneralLedger.SP_GeneralLedgerList = _oSP_GeneralLedgers;
                oCOA = oCOA.Get(nAccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string AccountCode = oCOA.AccountCode;
                string AccountHeadName = oCOA.AccountHeadName;
                double CurrentBalance = _oSP_GeneralLedgers[_oSP_GeneralLedgers.Count - 1].CurrentBalance;
                oGeneralLedger.Company = oCompany;
                oGeneralLedger.Currency = new Currency().Get(oGeneralLedger.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);


                rptGeneralLedgersTwo orptGeneralLedgers = new rptGeneralLedgersTwo();
                abytes = orptGeneralLedgers.PrepareReport(oGeneralLedger, dStartDate, dEndDate, AccountCode, AccountHeadName, CurrentBalance, true);
            }
            else if (nLevel == 8)
            {
                if (nOptionID == 1)
                {
                    List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
                    CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
                    oCostCenterBreakdown.CostCenterBreakdowns = CostCenterBreakdown.Gets(nBusinessUnitID.ToString(), nAccountHeadID, 1, dStartDate, dEndDate, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
                    abytes = orptCostCenterBreakdowns.PrepareReport(oCostCenterBreakdowns, sHeader, oCompany, EnumBalanceStatus.None);
                }

            }
            else if (nLevel == 9)
            {
                if (nOptionID == 1)
                {
                    List<CostCenterBreakdown> oCostCenterBreakdowns = new List<CostCenterBreakdown>();
                    CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
                    ACCostCenter oACCostCenter = new ACCostCenter();
                    Currency oCurrency = new Currency();
                    oCostCenterBreakdowns = CostCenterBreakdown.GetsForCostCenter(nBusinessUnitID.ToString(), nAccountHeadID, nCCID, 1, dStartDate, dEndDate, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCostCenterBreakdown.CostCenterBreakdowns = oCostCenterBreakdowns;
                    oACCostCenter = oACCostCenter.Get(nCCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    string sAddress = "";
                    if (oACCostCenter.ReferenceType == EnumReferenceType.Customer || oACCostCenter.ReferenceType == EnumReferenceType.Vendor || oACCostCenter.ReferenceType == EnumReferenceType.Vendor_Foreign)
                    {
                        Contractor oContractor = new Contractor();
                        oContractor = oContractor.Get(oACCostCenter.ReferenceObjectID, (int)Session[SessionInfo.currentUserID]);
                        sAddress = oContractor.Address;
                    }
                    string CCCode = oACCostCenter.Code;
                    string CCName = oACCostCenter.Name;
                    double CurrentBalance = oCostCenterBreakdowns[oCostCenterBreakdowns.Count - 1].ClosingValue;
                    oCostCenterBreakdown.Company = oCompany;
                    oCostCenterBreakdown.Currency = oCurrency.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    rptCostCenterLedger orptCostCenterLedgers = new rptCostCenterLedger();
                    abytes = orptCostCenterLedgers.PrepareReport(oCostCenterBreakdown, dStartDate, dEndDate, CCCode, CCName, CurrentBalance, true, sAddress);
                }
            }
            else if (nLevel == 10)
            {
                if (nOptionID == 1 && nCCOptionID == 5)
                {
                    List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
                    VoucherRefReport oVoucherRefReport = new VoucherRefReport();
                    oVoucherRefReport.AccountHeadID = nAccountHeadID;
                    oVoucherRefReport.StartDate = dStartDate;
                    oVoucherRefReport.EndDate = dEndDate;
                    oVoucherRefReport.CCID = nCCID;
                    oVoucherRefReport.BusinessUnitID = nBusinessUnitID;
                    oVoucherRefReport.VoucherBillID = nVoucherBillID;
                    oVoucherRefReport.IsApproved = true;
                    oVoucherRefReport.CurrencyID = 1;
                    oVoucherRefReports = VoucherRefReport.GetsVoucherBillBreakDown(oVoucherRefReport, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    rptVoucherRefReports orptVoucherRefReports = new rptVoucherRefReports();
                    abytes = orptVoucherRefReports.PrepareReport(oVoucherRefReports, sHeader, oCompany);
                }

            }
            else if (nLevel == 11)
            {
                if (nOptionID == 1 && nCCOptionID==5)
                {
                    List<VoucherRefReport> oVoucherRefReports = new List<VoucherRefReport>();
                    VoucherRefReport oVoucherRefReport = new VoucherRefReport();
                    oVoucherRefReport.AccountHeadID = nAccountHeadID;
                    oVoucherRefReport.StartDate = dStartDate;
                    oVoucherRefReport.EndDate = dEndDate;
                    oVoucherRefReport.CCID = nCCID;
                    oVoucherRefReport.BusinessUnitID = nBusinessUnitID;
                    oVoucherRefReport.VoucherBillID = nVoucherBillID;
                    oVoucherRefReport.IsApproved = true;
                    oVoucherRefReport.CurrencyID = 1;

                    Currency oCurrency = new Currency();
                    VoucherBill oVoucherBill=new VoucherBill();
                    oVoucherBill=oVoucherBill.Get(oVoucherRefReport.VoucherBillID, ((User)Session[SessionInfo.CurrentUser]).UserID);;
                    oCurrency = oCurrency.Get(oVoucherRefReport.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oVoucherRefReports = VoucherRefReport.GetsVoucherBillDetails(oVoucherRefReport, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    rptVoucherRefLedger orptVoucherRefLedger = new rptVoucherRefLedger();
                    abytes = orptVoucherRefLedger.PrepareReport(oVoucherRefReports, oCompany, oCurrency, dStartDate, dEndDate, oVoucherBill.BillDateInString, oVoucherBill.BillNo, oVoucherRefReports[oVoucherRefReports.Count - 1].ClosingBalance, oVoucherRefReport.IsApproved);
                }
            }
            return File(abytes, "application/pdf");
        }

        #region PrintTrialBalanceInXL
        public ActionResult PrintTrialBalanceInXL(string sTempString)
        {
            _oTrialBalance_Categorizeds = new List<TrialBalance_Categorized>();

            DateTime dStartDate = Convert.ToDateTime(sTempString.Split('~')[0]);
            DateTime dEndDate = Convert.ToDateTime(sTempString.Split('~')[1]);
            int nAccountType = Convert.ToInt32(sTempString.Split('~')[2]);
            int nAccountHeadID = Convert.ToInt32(sTempString.Split('~')[3]);
            int nBusinessUnitID = Convert.ToInt32(sTempString.Split('~')[4]);


            _oTrialBalance_Categorizeds = TrialBalance_Categorized.Gets(nAccountHeadID, dStartDate, dEndDate, true, 1, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var stream = new MemoryStream();
            //var serializer = new XmlSerializer(typeof(List<TrialBalance_CategorizedXL>));

            ////We load the data

            //int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0; double nTotalOpeningBalance = 0; double nTotalClosingBalance = 0;
            //TrialBalance_CategorizedXL oTrialBalance_CategorizedXL = new TrialBalance_CategorizedXL();
            //List<TrialBalance_CategorizedXL> oTrialBalance_CategorizedXLs = new List<TrialBalance_CategorizedXL>();
            //foreach (TrialBalance_Categorized oItem in _oTrialBalance_Categorizeds)
            //{
            //    nCount++;
            //    oTrialBalance_CategorizedXL = new TrialBalance_CategorizedXL();
            //    oTrialBalance_CategorizedXL.SLNo = nCount.ToString();
            //    oTrialBalance_CategorizedXL.AccountCode = oItem.AccountCode;
            //    oTrialBalance_CategorizedXL.AccountHeadName = oItem.AccountHeadName;
            //    oTrialBalance_CategorizedXL.ComponentType = oItem.ComponentType.ToString();
            //    oTrialBalance_CategorizedXL.OpeningBalance = oItem.OpenningBalance;
            //    oTrialBalance_CategorizedXL.DebitAmount = oItem.DebitAmount;
            //    oTrialBalance_CategorizedXL.CreditAmount = oItem.CreditAmount;
            //    oTrialBalance_CategorizedXL.ClosingBalance = oItem.ClosingBalance;
            //    oTrialBalance_CategorizedXLs.Add(oTrialBalance_CategorizedXL);
            //    nTotalOpeningBalance = nTotalOpeningBalance + oItem.OpenningBalance;
            //    nTotalDebit = nTotalDebit + oItem.DebitAmount;
            //    nTotalCredit = nTotalCredit + oItem.CreditAmount;
            //    nTotalClosingBalance = nTotalClosingBalance + oItem.ClosingBalance;

            //}

            #region Total
            //oTrialBalance_CategorizedXL = new TrialBalance_CategorizedXL();
            //oTrialBalance_CategorizedXL.SLNo = "";
            //oTrialBalance_CategorizedXL.AccountCode = "";
            //oTrialBalance_CategorizedXL.AccountHeadName = "";
            //oTrialBalance_CategorizedXL.ComponentType = "Total";
            //oTrialBalance_CategorizedXL.OpeningBalance = nTotalOpeningBalance;
            //oTrialBalance_CategorizedXL.DebitAmount = nTotalDebit;
            //oTrialBalance_CategorizedXL.CreditAmount = nTotalCredit;
            //oTrialBalance_CategorizedXL.ClosingBalance = nTotalClosingBalance;
            //oTrialBalance_CategorizedXLs.Add(oTrialBalance_CategorizedXL);

            #endregion
            //We turn it into an XML and save it in the memory
            //serializer.Serialize(stream, oTrialBalance_CategorizedXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Trial Balance.xls");
        }
        #endregion



    }
}