using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.BusinessObjects
{
    #region Voucher
    public class Voucher : BusinessObject
    {
        public Voucher()
        {
            VoucherID = 0;
            BUID = 0;
            VoucherTypeID = 0;
            VoucherNo = "";
            Narration = "";
            ReferenceNote = "";
            VoucherDate = DateTime.Today;
            AuthorizedBy = 0;
            LastUpdatedDate = DateTime.Now;
            BUCode = "";
            BUName = "";
            BUShortName = "";
            OperationType = EnumVoucherOperationType.None;
            VoucherAmount = 0;
            NewVoucherNo = "";
            ErrorMessage = "";
            AccountingSessionID = 0;
            ProfitLossAppropriationAccountsInString = "";
            VDObjs = new List<VDObj>();
            VoucherDetailLst = new List<VoucherDetail>();
            NumberMethodInInt = 0;
            CurrentSession = 0;
            BatchID = 0;
            TaxEffect = EnumTaxEffect.No;
            TaxEffectInt = 0;
            CurrencyID = 0;
            CRate = 0;
            TotalAmount = 0;
            VoucherAmount = 0;
            PreparedByName = "";
            DBServerDate = DateTime.Now;
            BUIDCodeNames = "";
            BaseCurrencyID = 0;
            BaseCUSymbol = "";
            VoucherBatchNO = "";
            CounterVoucherID = 0;
            VoucherCategory = EnumVoucherCategory.None;
            BusinessUnit = new BusinessUnit();
            IsPrint = false;
        }

        #region Properties
        public long VoucherID { get; set; }
        public int BUID { get; set; }
        public int VoucherTypeID { get; set; }
        public string VoucherNo { get; set; }
        public string Narration { get; set; }
        public string ReferenceNote { get; set; }
        public string Address { get; set; }
        public DateTime VoucherDate { get; set; }
        public int AuthorizedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public EnumVoucherOperationType OperationType { get; set; }
        public bool Selected { get; set; }
        public string ErrorMessage { get; set; }
        public double VoucherAmount { get; set; }
        public int PrintCount { get; set; }
        public int BatchID { get; set; }
        public EnumTaxEffect TaxEffect { get; set; }
        public int TaxEffectInt { get; set; }        
        public DateTime SearchStartDate { get; set; }
        public DateTime SearchEndDate { get; set; }
        public int CurrentSession { get; set; }
        public int CurrencyID { get; set; }
        public string CUSymbol { get; set; }
        public double CRate { get; set; }
        public double TotalAmount { get; set; }
        public string AuthorizedByName { get; set; }
        public string PreparedByName { get; set; }
        public DateTime DBServerDate { get; set; }
        public string BUIDCodeNames { get; set; }
        public int BaseCurrencyID { get; set; }
        public string BaseCUSymbol { get; set; }
        public string VoucherBatchNO { get; set; }
        public int CounterVoucherID { get; set; }
        public EnumVoucherCategory VoucherCategory { get; set; }
        public bool IsPrint { get; set; }
        #endregion

        #region Derive Property
        #region Derive Property Man
        public string NewVoucherNo { get; set; }
        public string BUShortNameCode
        {
            get { return this.BUShortName + " [" + this.BUCode + "]"; }
        }
        public string VoucherDateInString
        {
            get { return this.VoucherDate.ToString("dd MMM yyyy"); }
        }
        public string DBServerDateInString
        {
            get { return this.DBServerDate.ToString("dd MMM yyyy hh:mm tt"); }
        }
        public string VoucherDateAsString
        {
            get { return this.VoucherDate.ToString("dd MMM yyyy"); }
        }
        public string VoucherAmountInString
        {
            get
            {
                return Global.MillionFormat(VoucherAmount);
            }
        }
        public AccountingSession RunningAccountingYear { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public string VoucherName { get; set; }
        public EnumNumberMethod NumberMethod { get; set; }
        public int NumberMethodInInt { get; set; }        
        public List<VoucherDetail> VoucherDetailLst { get; set; }
        public List<VDObj> VDObjs { get; set; }
        public List<VDRptObj> VDRptObjs { get; set; }
        public List<AccountingSession> AccountingSessions { get; set; }
        public int AccountingSessionID { get; set; }
        public List<Voucher> VoucherList { get; set; }
        public List<VoucherType> VoucherTypeList { get; set; }
        public List<Currency> LstCurrency { get; set; }
        public List<User> UserList { get; set; }
        public string ProfitLossAppropriationAccountsInString { get; set; }
        public string TotalDebitAmounts { get; set; }
        public string TotalCreditAmounts { get; set; }
        #endregion
        
        #region Derive Property AutoVoucherCreate
        public VoucherMapping VoucherMapping { get; set; }
        public string TableName { get; set; }
        public string ManualDataEntry { get; set; }
        public int PKValue { get; set; }
        #endregion
        #endregion

        #region NON DB Functions
        public string TotalDebitAmount(List<VoucherDetail> VoucherDetailLst)
        {
            double TDA = 0.00;
            foreach (VoucherDetail oVoucherDetail in VoucherDetailLst)
            {
                if (oVoucherDetail.IsDebit)
                {
                    TDA = TDA + oVoucherDetail.Amount;
                }
            }
            return Global.MillionFormat(TDA);
        }
        public string TotalDebitAmountInString(List<VoucherDetail> VoucherDetailLst)
        {
            double TDA = 0.00;
            foreach (VoucherDetail oVoucherDetail in VoucherDetailLst)
            {
                if (oVoucherDetail.IsDebit)
                {
                    TDA = TDA + oVoucherDetail.Amount;
                }
            }
            return Global.MillionFormat(TDA);
        }
        public string TotalCreditAmount(List<VoucherDetail> VoucherDetailLst)
        {
            double TCA = 0.00;
            foreach (VoucherDetail oVoucherDetail in VoucherDetailLst)
            {
                if (!oVoucherDetail.IsDebit)
                {
                    TCA = TCA + oVoucherDetail.Amount;
                }
            }
            return Global.MillionFormat(TCA);
        }

        public string TotalCreditAmountInString(List<VoucherDetail> VoucherDetailLst)
        {
            double TCA = 0.00;
            foreach (VoucherDetail oVoucherDetail in VoucherDetailLst)
            {
                if (!oVoucherDetail.IsDebit)
                {
                    TCA = TCA + oVoucherDetail.Amount;
                }
            }
            return Global.MillionFormat(TCA);
        }
        public string VoucherIDWithNo
        {
            get
            {
                return this.VoucherNo + "~" + this.VoucherID;
            }
        }
        #endregion

        #region Functions
        public static List<Voucher> Gets(int nUserID)
        {
            return Voucher.Service.Gets(nUserID);
        }
        public static List<Voucher> GetsByBatch(int nVoucherBatchID, int nUserID)
        {
            return Voucher.Service.GetsByBatch(nVoucherBatchID, nUserID);
        }
        public static List<Voucher> GetsByBatchForApprove(int nVoucherBatchID, int nUserID)
        {
            return Voucher.Service.GetsByBatchForApprove(nVoucherBatchID, nUserID);
        }
        public static List<Voucher> GetsWaitForApproval(int nUserID)
        {
            return Voucher.Service.GetsWaitForApproval(nUserID);
        }
        public static List<Voucher> Gets(string sSQL, int nUserID)
        {
            return Voucher.Service.Gets(sSQL, nUserID);
        }
        public static List<Voucher> GetsByVoucherType(int nVoucherTypeID, int nUserID)
        {
            return Voucher.Service.GetsByVoucherType(nVoucherTypeID, nUserID);
        }
        public Voucher Get(long id,int nUserID)
        {
            return Voucher.Service.Get(id, nUserID);
        }
        public Voucher GetLastNarration(int nUserID)
        {
            return Voucher.Service.LastNarration(nUserID);
        }
        public Voucher GetProfitLossAppropriationAccountVoucher(int nBUID, DateTime dStartDate, DateTime dEndDate,int nUserID)
        {
            return Voucher.Service.GetProfitLossAppropriationAccountVoucher(nBUID, dStartDate, dEndDate, nUserID);
        }
        public Voucher Save(int nUserID)
        {
            return Voucher.Service.Save(this, nUserID);
        }
        public Voucher UpdatePrintCount(int nUserID)
        {
            return Voucher.Service.UpdatePrintCount(this,nUserID);
        }
        public Voucher ApprovedVoucher(int nUserID)
        {
            return Voucher.Service.ApprovedVoucher(this,nUserID);
        }
        public List<Voucher> ApprovedVouchers(int nUserID)
        {
            return Voucher.Service.ApprovedVouchers(this, nUserID);
        }
        public List<Voucher> CommitInventoryEffect(List<Voucher> oVouchers, int nUserID)
        {
            return Voucher.Service.CommitInventoryEffect(oVouchers, nUserID);
        }
        public Voucher CommitVoucherNo(int nBUID, int nVoucherTypeID,  DateTime dVoucherDate, int nUserID)
        {
            return Voucher.Service.CommitVoucherNo(nBUID, nVoucherTypeID, dVoucherDate, nUserID);
        }
        public Voucher GetMaxDate(int nVType, int nBUID, int nUserID)
        {
            return Voucher.Service.GetMaxDate(nVType, nBUID, nUserID);
        }
        public Voucher CommitProfitLossAccounts(int nBUID, int nSessionID, int nUserID)
        {
            return Voucher.Service.CommitProfitLossAccounts(nBUID, nSessionID, nUserID);
        }
        public Voucher CommitProfitLossAppropriationAccounts(int nUserID)
        {
            return Voucher.Service.CommitProfitLossAppropriationAccounts(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Voucher.Service.Delete(id, nUserID);
        }
        public Voucher UnApprovedVoucher(Int64 id, int nUserID)
        {
            return Voucher.Service.UnApprovedVoucher(id, nUserID);
        }
        public static List<Voucher> GetsAutoVoucher(IntegrationSetup oIntegrationSetup, object oPerameterObject, bool bUserPerameterObject, Int64 nUserId)
        {
            return Voucher.Service.GetsAutoVoucher(oIntegrationSetup, oPerameterObject, bUserPerameterObject, nUserId);
        }
        public static List<Voucher> CommitAutoVoucher(List<Voucher> oParamVouchers, Int64 nUserId)
        {
            return Voucher.Service.CommitAutoVoucher(null, false, oParamVouchers, nUserId);
        }
        #endregion

        #region Non DB Functions
        public static List<Voucher> MapVoucherExplanationObject(List<Voucher> oVouchers)
        {            
            VDObj oVDObj = new VDObj();
            List<VDObj> oVDObjs = new List<VDObj>();            
            List<CostCenterTransaction> oTempCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VoucherBillTransaction> oTempVoucherBillTransactions = new List<VoucherBillTransaction>();            
            List<VoucherCheque> oTempVoucherCheques = new List<VoucherCheque>();            
            List<VPTransaction> oTempVPTransactions = new List<VPTransaction>();
            List<VOReference> oTempVOReferences = new List<VOReference>();

            foreach (Voucher oVoucher in oVouchers)
            {
                oVDObjs = new List<VDObj>();
                oVoucher.VDObjs = new List<VDObj>();
                foreach (VoucherDetail oItem in oVoucher.VoucherDetailLst)
                {
                    oTempCostCenterTransactions = new List<CostCenterTransaction>();
                    oTempVoucherBillTransactions = new List<VoucherBillTransaction>();
                    oTempVoucherCheques = new List<VoucherCheque>();
                    oTempVPTransactions = new List<VPTransaction>();
                    oTempCostCenterTransactions = oItem.CCTs;
                    oTempVoucherBillTransactions = oItem.VoucherBillTrs;
                    oTempVoucherCheques = oItem.VoucherCheques;
                    oTempVPTransactions = oItem.VPTransactions;
                    oTempVOReferences = oItem.VOReferences;

                    #region Voucher Details
                    oVDObj = new VDObj();
                    oVDObj.VDObjID = oItem.VoucherDetailID;
                    oVDObj.BUID = oItem.BUID;
                    oVDObj.AID = oItem.AreaID;
                    oVDObj.ZID = oItem.ZoneID;
                    oVDObj.SID = oItem.SiteID;
                    oVDObj.PID = oItem.ProductID;
                    oVDObj.DptID = oItem.DeptID;
                    oVDObj.AHID = oItem.AccountHeadID;
                    oVDObj.CCID = oItem.CostCenterID;
                    oVDObj.CID = oItem.CurrencyID;
                    oVDObj.CAmount = oItem.AmountInCurrency;
                    oVDObj.CRate = oItem.ConversionRate;
                    oVDObj.Amount = oItem.Amount;
                    oVDObj.IsDr = oItem.IsDebit;
                    if (oItem.IsDebit) { oVDObj.DR_CR = "Debit"; } else { oVDObj.DR_CR = "Credit"; }
                    oVDObj.Detail = oItem.Narration;
                    oVDObj.AHCode = oItem.AccountHeadCode;
                    oVDObj.AHName = oItem.AccountHeadName;                    
                    oVDObj.ACode = oItem.AreaCode;
                    oVDObj.AName = oItem.AreaName;
                    oVDObj.ASName = oItem.AreaShortName;
                    oVDObj.ZCode = oItem.ZoneCode;
                    oVDObj.ZName = oItem.ZoneName;
                    oVDObj.ZSName = oItem.ZoneShortName;
                    oVDObj.SCode = oItem.SiteCode;
                    oVDObj.SName = oItem.SiteName;
                    oVDObj.SSName = oItem.SiteShortName;
                    oVDObj.PCode = oItem.PCode;
                    oVDObj.PName = oItem.PName;
                    oVDObj.PSName = oItem.PShortName;
                    oVDObj.DCode = oItem.DeptCode;
                    oVDObj.DName = oItem.DeptName;
                    oVDObj.DSName = oItem.DeptShortName;
                    oVDObj.CCCode = oItem.CCCode;
                    oVDObj.CCName = oItem.CCName;
                    oVDObj.DrAmount = oItem.DebitAmount;
                    oVDObj.CrAmount = oItem.CreditAmount;
                    oVDObj.BCDrAmount = oItem.BCDebitAmount;
                    oVDObj.BCCrAmount = oItem.BCCreditAmount;
                    oVDObj.IsAEfct = oItem.IsAreaEffect;
                    oVDObj.IsZEfct = oItem.IsZoneEffect;
                    oVDObj.IsSEfct = oItem.IsSiteEffect;
                    oVDObj.IsCCAply = oItem.IsCostCenterApply;
                    oVDObj.IsBTAply = oItem.IsBillRefApply;
                    oVDObj.IsChkAply = oItem.IsChequeApply;
                    oVDObj.IsIRAply = oItem.IsInventoryApply;
                    oVDObj.IsPaidChk = oItem.IsPaymentCheque;
                    oVDObjs.Add(oVDObj);
                    #endregion

                    #region CostCenterTransaction
                    int nCCCategoryID = 0;
                    foreach (CostCenterTransaction oCostCenterTransaction in oTempCostCenterTransactions)
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = oCostCenterTransaction.CCTID;
                        oVDObj.ObjType = EnumBreakdownType.CostCenter;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.CostCenter;
                        oVDObj.CCID = oCostCenterTransaction.CCID;
                        oVDObj.CCName = oCostCenterTransaction.CostCenterName;
                        oVDObj.CCCode = oCostCenterTransaction.CostCenterCode;
                        oVDObj.CID = oCostCenterTransaction.CurrencyID;
                        oVDObj.IsBTAply = oCostCenterTransaction.IsBillRefApply;
                        oVDObj.IsOrderaAply = oCostCenterTransaction.IsOrderRefApply;
                        oVDObj.CAmount = oCostCenterTransaction.Amount;
                        oVDObj.CRate = oCostCenterTransaction.CurrencyConversionRate;
                        oVDObj.Amount = oCostCenterTransaction.Amount;
                        oVDObj.CFormat = "Subledger : " + oCostCenterTransaction.CostCenterName;
                        oVDObj.CName = oItem.CUName;
                        oVDObj.CSymbol = oItem.CUSymbol;
                        oVDObj.Detail = oCostCenterTransaction.Description;
                        oVDObj.IsDr = oCostCenterTransaction.IsDr;
                        oVDObj.DR_CR = "Subledger";
                        oVDObj.DrAmount = 0.00;
                        oVDObj.CrAmount = 0.00;
                        oVDObjs.Add(oVDObj);
                        nCCCategoryID = oCostCenterTransaction.CCCategoryID;

                        #region Map Subledger Bill
                        if (oCostCenterTransaction.IsBillRefApply)
                        {
                            List<VoucherBillTransaction> oVBTransactions = new List<VoucherBillTransaction>();
                            oVBTransactions = oCostCenterTransaction.VBTransactions;
                            if (oVBTransactions!=null && oVBTransactions.Count > 0)
                            {
                                foreach (VoucherBillTransaction oVoucherBillTransaction in oVBTransactions)
                                {
                                    oVDObj = new VDObj();
                                    oVDObj.VDObjID = oVoucherBillTransaction.VoucherBillTransactionID;
                                    oVDObj.ObjType = EnumBreakdownType.SubledgerBill;
                                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.SubledgerBill;
                                    oVDObj.BillID = oVoucherBillTransaction.VoucherBillID;
                                    oVDObj.TrType = oVoucherBillTransaction.TrType;
                                    oVDObj.TrType = oVoucherBillTransaction.TrType;
                                    oVDObj.TrTypeInt = (int)oVoucherBillTransaction.TrType;
                                    oVDObj.TrTypeStr = oVoucherBillTransaction.TrType.ToString();
                                    oVDObj.BillNo = oVoucherBillTransaction.BillNo;
                                    oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                                    oVDObj.BillAmount = oVoucherBillTransaction.BillAmount;
                                    oVDObj.CID = oVoucherBillTransaction.CurrencyID;
                                    oVDObj.CAmount = oVoucherBillTransaction.Amount;
                                    oVDObj.CRate = oVoucherBillTransaction.ConversionRate;
                                    oVDObj.Amount = oVoucherBillTransaction.Amount;
                                    oVDObj.CFormat = "SL Bill : " + oVoucherBillTransaction.ExplanationTransactionTypeInString + "  " + oVoucherBillTransaction.BillNo + " @ " + oVoucherBillTransaction.BillDate.ToString("dd MMM yyyy");
                                    oVDObj.CName = oItem.CUName;
                                    oVDObj.CSymbol = oItem.CUSymbol;
                                    oVDObj.Detail = "";
                                    oVDObj.IsDr = oVoucherBillTransaction.IsDr;
                                    oVDObj.DR_CR = "SL Bill";
                                    oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                                    oVDObj.CCID = oVoucherBillTransaction.CCID;
                                    oVDObj.CCName = oVoucherBillTransaction.CostCenterName;
                                    oVDObj.CCCode = oVoucherBillTransaction.CostCenterCode;
                                    oVDObjs.Add(oVDObj);
                                }
                            }
                        }
                        #endregion

                        #region Map Subledger Order Ref
                        if (oCostCenterTransaction.IsOrderRefApply)
                        {
                            List<VOReference> oVOReferences = new List<VOReference>();
                            oVOReferences = oCostCenterTransaction.VOReferences;
                            if (oVOReferences != null && oVOReferences.Count > 0)
                            {
                                foreach (VOReference oVOReference in oVOReferences)
                                {
                                    oVDObj = new VDObj();
                                    oVDObj.AHID = oItem.AccountHeadID;
                                    oVDObj.AHCode = oItem.AccountHeadCode;
                                    oVDObj.AHName = oItem.AccountHeadName;
                                    oVDObj.VDObjID = oVOReference.VOReferenceID;
                                    oVDObj.ObjType = EnumBreakdownType.SLOrderReference;
                                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.SLOrderReference;
                                    oVDObj.OrderID = oVOReference.OrderID;
                                    oVDObj.RefNo = oVOReference.RefNo;
                                    oVDObj.OrderNo = oVOReference.OrderNo;
                                    oVDObj.ORemarks = oVOReference.Remarks;
                                    oVDObj.CID = oVOReference.CurrencyID;
                                    oVDObj.CAmount = oVOReference.AmountInCurrency;
                                    oVDObj.CRate = oVOReference.ConversionRate;
                                    oVDObj.Amount = oVOReference.Amount;
                                    oVDObj.CFormat = "SLOrder Ref : " + (oVOReference.RefNo != "" ? (oVOReference.RefNo + ", " + oVOReference.OrderNo) : "N/A") + "," + oVOReference.Remarks + ", " + oItem.CUSymbol + " " + Global.MillionFormat(oItem.AmountInCurrency) + " @ " + Global.MillionFormat(oItem.ConversionRate);
                                    oVDObj.CName = oItem.CUName;
                                    oVDObj.CSymbol = oItem.CUSymbol;
                                    oVDObj.IsDr = oVOReference.IsDebit;
                                    oVDObj.DR_CR = "SLOrder Ref";
                                    oVDObj.CCID = oVOReference.SubledgerID;
                                    oVDObjs.Add(oVDObj);
                                }
                            }
                        }
                        #endregion

                        #region Map Subledger Cheque
                        if (oCostCenterTransaction.IsChequeApply)
                        {
                            List<VoucherCheque> oVCheques = new List<VoucherCheque>();
                            oVCheques = oCostCenterTransaction.VoucherCheques;
                            if (oVCheques.Count > 0)
                            {
                                foreach (VoucherCheque oVCheque in oVCheques)
                                {
                                    oVDObj = new VDObj();
                                    oVDObj.VDObjID = oVCheque.VoucherChequeID;
                                    oVDObj.ObjType = EnumBreakdownType.SubledgerCheque;
                                    oVDObj.ObjTypeInt = (int)EnumBreakdownType.SubledgerCheque;
                                    oVDObj.CAmount = oVCheque.Amount;
                                    oVDObj.Amount = oVCheque.Amount;
                                    oVDObj.CFormat = "SL Cheque : " + oVCheque.ChequeNo + " @ " + oVCheque.ChequeDate.ToString("dd MMM yyyy") + " On " + oVCheque.BankName;
                                    oVDObj.Detail = "";
                                    oVDObj.IsDr = oItem.IsDebit;
                                    oVDObj.DR_CR = "SL Cheque";
                                    oVDObj.ChequeDate = oVCheque.TransactionDate;
                                    oVDObj.ChequeID = oVCheque.ChequeID;
                                    oVDObj.ChequeType = oVCheque.ChequeType;
                                    oVDObj.ChequeNo = oVCheque.ChequeNo;
                                    oVDObj.BankName = oVCheque.BankName;
                                    oVDObj.BranchName = oVCheque.BranchName;
                                    oVDObj.AccountNo = oVCheque.AccountNo;
                                    oVDObj.CCID = oVCheque.CCID;
                                    oVDObj.CCName = oVCheque.CostCenterName;
                                    oVDObj.CCCode = oVCheque.CostCenterCode;
                                    oVDObjs.Add(oVDObj);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region VoucherBillTransaction
                    foreach (VoucherBillTransaction oVoucherBillTransaction in oTempVoucherBillTransactions)
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = oVoucherBillTransaction.VoucherBillTransactionID;
                        oVDObj.ObjType = EnumBreakdownType.BillReference;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.BillReference;
                        oVDObj.BillID = oVoucherBillTransaction.VoucherBillID;
                        oVDObj.TrType = oVoucherBillTransaction.TrType;
                        oVDObj.TrType = oVoucherBillTransaction.TrType;
                        oVDObj.TrTypeInt = (int)oVoucherBillTransaction.TrType;
                        oVDObj.TrTypeStr = oVoucherBillTransaction.TrType.ToString();
                        oVDObj.BillNo = oVoucherBillTransaction.BillNo;
                        oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                        oVDObj.BillAmount = oVoucherBillTransaction.BillAmount;
                        oVDObj.CID = oVoucherBillTransaction.CurrencyID;
                        oVDObj.CAmount = oVoucherBillTransaction.Amount;
                        oVDObj.CRate = oVoucherBillTransaction.ConversionRate;
                        oVDObj.Amount = oVoucherBillTransaction.Amount;
                        oVDObj.CFormat = "Bill : " + oVoucherBillTransaction.ExplanationTransactionTypeInString + "  " + oVoucherBillTransaction.BillNo + " @ " + oVoucherBillTransaction.BillDate.ToString("dd MMM yyyy");
                        oVDObj.CName = oItem.CUName;
                        oVDObj.CSymbol = oItem.CUSymbol;
                        oVDObj.Detail = "";
                        oVDObj.IsDr = oVoucherBillTransaction.IsDr;
                        oVDObj.DR_CR = "Bill";
                        oVDObj.BillDate = oVoucherBillTransaction.BillDate;
                        oVDObj.CCID = oVoucherBillTransaction.CCID;
                        oVDObj.CCName = oVoucherBillTransaction.CostCenterName;
                        oVDObj.CCCode = oVoucherBillTransaction.CostCenterCode;
                        oVDObjs.Add(oVDObj);
                    }
                    #endregion

                    #region VoucherCheque
                    foreach (VoucherCheque oVoucherCheque in oTempVoucherCheques)
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = oVoucherCheque.VoucherChequeID;
                        oVDObj.ObjType = EnumBreakdownType.ChequeReference;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.ChequeReference;
                        oVDObj.CAmount = oVoucherCheque.Amount;
                        oVDObj.Amount = oVoucherCheque.Amount;
                        oVDObj.CFormat = "Cheque : " + oVoucherCheque.ChequeNo + " @ " + oVoucherCheque.ChequeDate.ToString("dd MMM yyyy") + " On " + oVoucherCheque.BankName;
                        oVDObj.Detail = "";
                        oVDObj.IsDr = oItem.IsDebit;
                        oVDObj.DR_CR = "Cheque";
                        oVDObj.ChequeDate = oVoucherCheque.TransactionDate;
                        oVDObj.ChequeID = oVoucherCheque.ChequeID;
                        oVDObj.ChequeType = oVoucherCheque.ChequeType;
                        oVDObj.ChequeNo = oVoucherCheque.ChequeNo;
                        oVDObj.BankName = oVoucherCheque.BankName;
                        oVDObj.BranchName = oVoucherCheque.BranchName;
                        oVDObj.AccountNo = oVoucherCheque.AccountNo;
                        oVDObjs.Add(oVDObj);
                    }
                    #endregion

                    #region VPTransaction
                    foreach (VPTransaction oVPTransaction in oTempVPTransactions)
                    {
                        oVDObj = new VDObj();
                        oVDObj.VDObjID = oVPTransaction.VPTransactionID;
                        oVDObj.ObjType = EnumBreakdownType.InventoryReference;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.InventoryReference;
                        oVDObj.PID = oVPTransaction.ProductID;
                        oVDObj.PName = oVPTransaction.ProductName;
                        oVDObj.PCode = oVPTransaction.ProductCode;
                        oVDObj.CID = oVPTransaction.CurrencyID;
                        oVDObj.CAmount = oVPTransaction.Amount;
                        oVDObj.CRate = oVPTransaction.ConversionRate;
                        oVDObj.Amount = oVPTransaction.Amount;
                        oVDObj.CFormat = "Inventory : " + oVPTransaction.ProductName + ", " + oVPTransaction.Description + "," + oVPTransaction.WorkingUnitName + ", " + oVPTransaction.QtyInString + ", " + oVPTransaction.MUnitName + " @ " + oItem.CUSymbol + " " + oVPTransaction.UnitPriceInString;
                        oVDObj.CName = oItem.CUName;
                        oVDObj.CSymbol = oItem.CUSymbol;
                        oVDObj.IsDr = oVPTransaction.IsDr;
                        oVDObj.DR_CR = "Inventory";
                        oVDObj.WUID = oVPTransaction.WorkingUnitID;
                        oVDObj.WUName = oVPTransaction.WorkingUnitName;
                        oVDObj.MUID = oVPTransaction.MUnitID;
                        oVDObj.MUName = oVPTransaction.MUnitName;
                        oVDObj.Qty = oVPTransaction.Qty;
                        oVDObj.UPrice = oVPTransaction.UnitPrice;
                        oVDObjs.Add(oVDObj);
                    }
                    #endregion

                    #region Map Order Ref
                    foreach (VOReference oVOReference in oTempVOReferences)
                    {
                        oVDObj = new VDObj();
                        oVDObj.AHID = oItem.AccountHeadID;
                        oVDObj.AHCode = oItem.AccountHeadCode;
                        oVDObj.AHName = oItem.AccountHeadName;
                        oVDObj.VDObjID = oVOReference.VOReferenceID;
                        oVDObj.ObjType = EnumBreakdownType.OrderReference;
                        oVDObj.ObjTypeInt = (int)EnumBreakdownType.OrderReference;
                        oVDObj.OrderID = oVOReference.OrderID;
                        oVDObj.RefNo = oVOReference.RefNo;
                        oVDObj.OrderNo = oVOReference.OrderNo;
                        oVDObj.ORemarks = oVOReference.Remarks;
                        oVDObj.CID = oVOReference.CurrencyID;
                        oVDObj.CAmount = oVOReference.AmountInCurrency;
                        oVDObj.CRate = oVOReference.ConversionRate;
                        oVDObj.Amount = oVOReference.Amount;
                        oVDObj.CFormat = "Order Ref : " + (oVOReference.RefNo != "" ? (oVOReference.RefNo + ", " + oVOReference.OrderNo) : "N/A") + "," + oVOReference.Remarks + ", " + oItem.CUSymbol + " " + Global.MillionFormat(oVOReference.AmountInCurrency) + " @ " + Global.MillionFormat(oItem.ConversionRate);
                        oVDObj.CName = oItem.CUName;
                        oVDObj.CSymbol = oItem.CUSymbol;
                        oVDObj.IsDr = oVOReference.IsDebit;
                        oVDObj.DR_CR = "Order Ref";
                        oVDObj.CCID = oVOReference.SubledgerID;
                        oVDObjs.Add(oVDObj);
                    }
                    #endregion
                }
                oVoucher.VDObjs = oVDObjs;
            }
            return oVouchers;
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherService Service
        {
            get { return (IVoucherService)Services.Factory.CreateService(typeof(IVoucherService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucher interface
    public interface IVoucherService
    {
        Voucher LastNarration(int nUserID);
        Voucher Get(long id, int nUserID);
        Voucher GetProfitLossAppropriationAccountVoucher(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID);
        Voucher GetMaxDate(int nVType, int nBUID, int nUserID);
        List<Voucher> GetsByBatch(int nVoucherBatchID, int nUserID);
        List<Voucher> GetsByBatchForApprove(int nVoucherBatchID, int nUserID);
        List<Voucher> Gets(int nUserID);
        List<Voucher> GetsWaitForApproval(int nUserID);
        List<Voucher> Gets(string sSQL, int nUserID);
        List<Voucher> GetsByVoucherType(int nVoucherTypeID, int nUserID);
        string Delete(int id,int nUserID);
        Voucher UnApprovedVoucher(Int64 id, int nUserID);
        Voucher Save(Voucher oVoucher, int nUserID);
        Voucher UpdatePrintCount(Voucher oVoucher, int nUserID);
        Voucher ApprovedVoucher(Voucher oVoucher, int nUserID);
        List<Voucher> ApprovedVouchers(Voucher oVoucher, int nUserID);
        List<Voucher> CommitInventoryEffect(List<Voucher> oVouchers, int nUserID);
        Voucher CommitVoucherNo(int nBUID, int nVoucherTypeID, DateTime dVoucherDate, int nUserID);
        Voucher CommitProfitLossAccounts(int nBUID, int nSessionID, int nUserID);
        Voucher CommitProfitLossAppropriationAccounts(Voucher oVoucher, int nUserID);
        List<Voucher> GetsAutoVoucher(IntegrationSetup oIntegrationSetup, object oPerameterObject, bool bUserPerameterObject, Int64 nUserId);
        List<Voucher> CommitAutoVoucher(TransactionContext tc, bool bUsetc, List<Voucher> oParamVouchers, Int64 nUserId);
    }
    #endregion

    #region Temp Object
    public class TVoucher
    {
        public TVoucher()
        {
            VoucherID = 0;
            BUID = 0;
            VoucherTypeID = 0;
            VoucherNo = "";
            Narration = "";
            ReferenceNote = "";
            VoucherDate = DateTime.Now;
            AuthorizedBy = 0;
            BatchID = 0;
            TaxEffect = EnumTaxEffect.No;
            TaxEffectInt = 0;
            CurrentSession = 0;
            CurrencyID = 0;
            CUSymbol = "";
            CRate = 0;
            VDObjs = new List<VDObj>();
        }
        public int VoucherID { get; set; }
        public int BUID { get; set; }
        public int VoucherTypeID { get; set; }
        public string VoucherNo { get; set; }
        public string Narration { get; set; }
        public string ReferenceNote { get; set; }
        public DateTime VoucherDate { get; set; }
        public int AuthorizedBy { get; set; }
        public int BatchID { get; set; }
        public EnumTaxEffect TaxEffect { get; set; }
        public int TaxEffectInt { get; set; }                
        public int CurrentSession { get; set; }
        public int CurrencyID { get; set; }
        public string CUSymbol { get; set; }
        public double CRate { get; set; }
        public List<VDObj> VDObjs { get; set; }
    }
    #endregion
}