using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region VoucherDetail
    public class VoucherDetail : BusinessObject
    {
        public VoucherDetail()
        {
            VoucherDetailID = 0;
            VoucherID = 0;
            BUID = 0;
            AreaID = 0;
            ZoneID = 0;
            SiteID = 0;
            ProductID = 0;
            DeptID = 0;
            AccountHeadID = 0;
            CostCenterID = 0;
            CurrencyID = 0;
            AmountInCurrency = 0;
            ConversionRate = 0;
            Amount = 0;
            IsDebit = true;
            Narration = "";
            AccountHeadCode = "0000000";
            AccountHeadName = "";
            OperationType = EnumVoucherOperationType.None;
            VoucherDate = DateTime.Today;
            VoucherTypeID = 0;
            AuthorizedBy = 0;
            CUName = "";
            CUSymbol = "";            
            AreaCode = "00";
            AreaName = "";
            AreaShortName = "";
            ZoneCode = "00";
            ZoneName = "";
            ZoneShortName = "";
            SiteCode = "0000";
            SiteName = "";
            SiteShortName = "";
            PCode = "00000";
            PName = "";
            PShortName = "";
            DeptCode = "00";
            DeptName = "";
            DeptShortName = "";
            CCCode = "0000";
            CCName = "";
            DebitAmount = 0;
            CreditAmount = 0;
            BCDebitAmount = 0;
            BCCreditAmount = 0;
            IsAreaEffect = false;
            IsZoneEffect = false;
            IsSiteEffect = false;            
            IsCostCenterApply = false;
            IsBillRefApply = false;
            IsChequeApply = false;
            IsInventoryApply = false;
            IsOrderReferenceApply = false;
            IsReferenceApply = false;
            IsPaymentCheque = false;
            VoucherDetails = new List<VoucherDetail>();
            VoucherBillTrs = new List<VoucherBillTransaction>();
            VPTransactions = new List<VPTransaction>();
            CCTs = new List<CostCenterTransaction>();
            VoucherCheques = new List<VoucherCheque>();
            VOReferences = new List<VOReference>();
            LedgerBalance = "";
        }

        #region Properties
        public long VoucherDetailID { get; set; }
        public long VoucherID { get; set; }
        public int BUID { get; set; }
        public int AreaID { get; set; }
        public int ZoneID { get; set; }
        public int SiteID { get; set; }
        public int ProductID { get; set; }
        public int DeptID { get; set; }
        public int AccountHeadID { get; set; }
        public int CostCenterID { get; set; }
        public int CurrencyID { get; set; }
        public double AmountInCurrency { get; set; }
        public double ConversionRate { get; set; }
        public double Amount { get; set; }
        public bool IsDebit { get; set; }
        public string Narration { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; }
        public EnumVoucherOperationType OperationType { get; set; }
        public DateTime VoucherDate { get; set; }
        public int VoucherTypeID { get; set; }
        public int AuthorizedBy { get; set; }
        public string CUName { get; set; }
        public string CUSymbol { get; set; }        
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string AreaShortName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string ZoneShortName { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SiteShortName { get; set; }
        public string PCode { get; set; }
        public string PName { get; set; }
        public string PShortName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string DeptShortName { get; set; }
        public string CCCode { get; set; }
        public string CCName { get; set; }
        public bool IsAreaEffect { get; set; }
        public bool IsZoneEffect { get; set; }
        public bool IsSiteEffect { get; set; }
        public bool IsCostCenterApply { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsChequeApply { get; set; }
        public bool IsInventoryApply { get; set; }
        public bool IsOrderReferenceApply { get; set; }
        public bool IsReferenceApply { get; set; }
        public bool IsPaymentCheque { get; set; }
        public string LedgerBalance { get; set; } 
        #endregion

        #region Derive Properties
        public List<VoucherDetail> VoucherDetails { get; set; }
        public List<VoucherBillTransaction> VoucherBillTrs { get; set; }
        public List<VPTransaction> VPTransactions { get; set; }
        public List<CostCenterTransaction> CCTs { get; set; }
        public List<VoucherCheque> VoucherCheques { get; set; }
        public List<VOReference> VOReferences { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double BCDebitAmount { get; set; }
        public double BCCreditAmount { get; set; }
        public string IsDebitSt
        {
            get
            {
                if (this.IsDebit) { return "Dr"; }
                else { return "Cr"; }
            }
        }
        public string AccountCode
        {
            get
            {
                return this.AreaCode + "." + this.ZoneCode + "." + this.SiteCode + "." + this.PCode + "." + this.DeptCode + "." + this.AccountHeadCode;
            }
        }
        #endregion

        #region Functions
        public static List<VoucherDetail> Gets(int nUserID)
        {
            return VoucherDetail.Service.Gets(nUserID);
        }
        public VoucherDetail Get(int id, int nUserID)
        {
            return VoucherDetail.Service.Get(id, nUserID);
        }
        public VoucherDetail GetProfitLossAccountTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nCompanyID, int nUserID)
        {
            return VoucherDetail.Service.GetProfitLossAccountTransaction(nBUID, dStartDate, dEndDate, nCompanyID, nUserID);
        }
        public VoucherDetail GetDividendTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            return VoucherDetail.Service.GetDividendTransaction(nBUID, dStartDate, dEndDate, nUserID);
        }
        public VoucherDetail GetRetaindEarningTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        {
            return VoucherDetail.Service.GetRetaindEarningTransaction(nBUID, dStartDate, dEndDate, nUserID);
        }
        public VoucherDetail Save(int nUserID)
        {
            return VoucherDetail.Service.Save(this, nUserID);
        }
        public static List<VoucherDetail> Gets(long nVoucherId, int nUserID)
        {
            return VoucherDetail.Service.Gets(nVoucherId, nUserID);
        }
        public static List<VoucherDetail> Gets(string sSQL, int nUserId)
        {
            return VoucherDetail.Service.Gets(sSQL, nUserId);
        }
        public static double GetCurrentBalance(int nAccountHeadID, bool bIsDebit, DateTime dStartDate,DateTime dEndDate, int nUserId)
        {
           return VoucherDetail.Service.GetCurrentBalance(nAccountHeadID,bIsDebit,dStartDate,dEndDate,nUserId);
        }
        #endregion

        #region ServiceFactory
        internal static IVoucherDetailService Service
        {
            get { return (IVoucherDetailService)Services.Factory.CreateService(typeof(IVoucherDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IVoucherDetail interface
    public interface IVoucherDetailService
    {
        VoucherDetail Get(int id, int nUserID);
        VoucherDetail GetProfitLossAccountTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nCompanyID, int nUserID);
        VoucherDetail GetDividendTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID);
        VoucherDetail GetRetaindEarningTransaction(int nBUID, DateTime dStartDate, DateTime dEndDate, int nUserID);
        List<VoucherDetail> Gets(int nUserID);
        List<VoucherDetail> Gets(long nVoucherId, int nUserID);
        List<VoucherDetail> Gets(string sSQL, int nUserId);
        VoucherDetail Save(VoucherDetail oVoucherDetail, int nUserID);

        double GetCurrentBalance(int nAccountHeadID, bool bIsDebit, DateTime dStartDate, DateTime dEndDate, int nUserId);
    }
    #endregion

    #region VDObj
    public class VDObj
    {
        public VDObj()
        {
            VDObjID = 0;
            DR_CR = "";
            BUID = 0;
            AID = 0;
            ZID = 0;
            SID = 0;
            PID = 0;
            DptID = 0;
            AHID = 0;
            CID = 0;
            CAmount = 0;
            CRate = 0;
            Amount = 0;
            IsDr = false;
            Detail = "";
            AHCode = "0000000";
            AHName = "";
            CName = "";
            CSymbol = "";            
            ACode = "00";
            AName = "";
            ASName = "";
            ZCode = "00";
            ZName = "";
            ZSName = "";
            SCode = "0000";
            SName = "";
            SSName = "";
            PCode = "00000";
            PName = "";
            PSName = "";
            DCode = "00";
            DName = "";
            DSName = "";
            DrAmount = 0;
            CrAmount = 0;
            BCDrAmount = 0;
            BCCrAmount = 0;
            IsAEfct = false;
            IsZEfct = false;
            IsSEfct = false;

            //for CostCenter Transaction
            CCID = 0;
            CCCode = "";
            CCName = "";

            //For Bill Reference 
            BillID = 0;
            BillDate = DateTime.Now;
            TrType = EnumVoucherBillTrType.None;
            TrTypeInt = 0;
            TrTypeStr = "None";
            BillNo = "";
            BillAmount = 0;

            //Cheque Reference
            ChequeID = 0;
            ChequeNo = "";
            ChequeType = EnumChequeType.None;
            ChequeDate = DateTime.Now;
            BankName = "Bank";
            BranchName = "Branch";
            AccountNo = "Account No";

            //Inventory Reference
            WUID = 0;
            WUName = "";
            MUID = 0;
            MUName = "";
            Qty = 0;
            UPrice = 0;

            //Order Reference 
            OrderID = 0;
            RefNo = "";
            OrderNo = "";            
            ORemarks = "";

            //Common Property
            IsCCAply = false;
            IsBTAply = false;
            IsChkAply = false;
            IsIRAply = false;
            IsOrderaAply = false;
            IsPaidChk = false;
            ObjType = EnumBreakdownType.VoucherDetail;
            ObjTypeInt = 0;
            CFormat = "";
        }
        public long VDObjID { get; set; }//VoucherDetailID, CCTransactionID, VoucherBillTrID            
        public string DR_CR { get; set; }
        public int BUID { get; set; }//AccHeadID, CCID, VoucherBillID, ProductID
        public int AID { get; set; }//AreaID
        public int ZID { get; set; }//zoneid
        public int SID { get; set; }//siteid
        public int PID { get; set; } //productID
        public int DptID { get; set; }//DeptID
        public int AHID { get; set; }//AccountHeadID         
        public int CID { get; set; }//CurrencyID 
        public double CAmount { get; set; }//AmountInCurrency 
        public double CRate { get; set; }//ConversionRate 
        public double Amount { get; set; }
        public double AmountBC { get; set; } // for Subledger Decimal point fraction 
        public bool IsDr { get; set; }
        public string Detail { get; set; } //Narrations 
        public string AHCode { get; set; }// AccountHead Code
        public string AHName { get; set; }//AccHead Name        
        public string CName { get; set; }//CUName 
        public string CSymbol { get; set; }//CUSymbol        
        public string ACode { get; set; } //AreaCode   
        public string AName { get; set; } //AreaName    
        public string ASName { get; set; } //AreaShortName    
        public string ZCode { get; set; } //ZoneCode     
        public string ZName { get; set; } //ZoneName     
        public string ZSName { get; set; } //ZoneShortName     
        public string SCode { get; set; } //SiteCode      
        public string SName { get; set; } //SiteName      
        public string SSName { get; set; } //SiteShortName     
        public string PCode { get; set; } //PCode       
        public string PName { get; set; } //PName       
        public string PSName { get; set; } //PShortName     
        public string DCode { get; set; } //DeptCode        
        public string DName { get; set; } //DeptName        
        public string DSName { get; set; } //DeptShortName           
        public double DrAmount { get; set; } //DebitAmount 
        public double CrAmount { get; set; }//CreditAmount 
        public double BCDrAmount { get; set; } //BCDebitAmount 
        public double BCCrAmount { get; set; }//BCCreditAmount 
        public bool IsAEfct { get; set; } //IsAreaEffect
        public bool IsZEfct { get; set; }//IsZoneEffect
        public bool IsSEfct { get; set; }//IsSiteEffect        

        //for CostCenter Transaction
        public int CCID { get; set; } //CostCenterID
        public string CCCode { get; set; } //CCCode  
        public string CCName { get; set; } //CCName  

        //For Bill Reference 
        public int BillID { get; set; } //VoucherBillID
        public DateTime BillDate { get; set; } //VoucherBill Transaction Date
        public EnumVoucherBillTrType TrType { get; set; }//Voucher Bill TrType
        public int TrTypeInt { get; set; }//Voucher Bill TrType in Int
        public string TrTypeStr { get; set; }//Voucher Bill TrType in string
        public string BillNo { get; set; }
        public double BillAmount { get; set; }

        //Cheque Reference
        public int ChequeID { get; set; }
        public string ChequeNo { get; set; }
        public EnumChequeType ChequeType { get; set; } 
        public DateTime ChequeDate { get; set; }
        public string BankName { get; set; }        
        public string BranchName { get; set; }
        public string AccountNo { get; set; }               
       
        //Inventory Reference
        public int WUID { get; set; }//Inventory Working Unit
        public string WUName { get; set; }//WUnitStr
        public int MUID { get; set; }//Inventory MeasurementUnit
        public string MUName { get; set; }//MUnitStr
        public double Qty { get; set; }// Inventory Qty
        public double UPrice { get; set; }// Inventory Unit Price
        
        //Order Reference
        public int OrderID { get; set; } //OrderID
        public string RefNo { get; set; } //Order RefNo
        public string OrderNo { get; set; } // Order Order No
        public string ORemarks { get; set; }//Order Remarks
        
        //Common Property
        public bool IsCCAply { get; set; }//IsCostCenterApply AccountHead Wise
        public bool IsBTAply { get; set; }//IsBillReferenceApply AccountHead Wise        
        public bool IsChkAply { get; set; }//IsCheque Apply Voucher Type Wise
        public bool IsIRAply { get; set; }//IsInventoryReferenceApply AccountHead Wise      
        public bool IsOrderaAply { get; set; }//IsOrderReferenceApply AccountHead Wise      
        public bool IsPaidChk { get; set; }//Is only use for Cheque Reference
        public EnumBreakdownType ObjType { get; set; }//EnumBreakdownType
        public int ObjTypeInt { get; set; }//EnumBreakdownType int Value
        public string CFormat { get; set; }

        //Derived Property
        public string AccountCode
        {
            get
            {
                return this.ACode + "." + this.ZCode + "." + this.SCode + "." + this.PCode + "." + this.DCode + "." + this.AHCode;
            }
        }

        public string BillDateStr
        {
            get
            {
                if (this.BillDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.BillDate.ToString("dd MMM yyyy");
                }
            }
        }

        public string ChequeDateStr
        {
            get
            {
                if (this.ChequeDate== DateTime.MinValue)
                {
                    return "Date";
                }
                else
                {
                    return this.DR_CR.Split('-')[0] == "SL Cheque" || this.DR_CR.Split('-')[0] == "Cheque" ? this.ChequeDate.ToString("dd MMM yyyy") : "Date";
                }
            }
        }


    }

    #endregion

    #region VDRptObj
    public class VDRptObj
    {
        public VDRptObj()
        {
            VDObjID = 0;
            VoucherRecordType = EnumVoucherRecordType.None;
            AHID = 0;
            CAmount = 0;
            CRate = 0;
            Amount = 0;
            IsDr = false;
            DetailText = "";
            Code = "";
            DrAmount = 0;
            CrAmount = 0;
            BCDrAmount = 0;
            BCCrAmount = 0;
        }
        public long VDObjID { get; set; }//VoucherDetailID, CCTransactionID, VoucherBillTrID                    
        public EnumVoucherRecordType VoucherRecordType { get; set; }
        public int AHID { get; set; }
        public int CCID { get; set; }
        public double CAmount { get; set; }//AmountInCurrency 
        public double CRate { get; set; }//ConversionRate 
        public double Amount { get; set; }
        public bool IsDr { get; set; }
        public string DetailText { get; set; } //Narrations 
        public string Code { get; set; }// AccountHead Code        
        public double DrAmount { get; set; } //DebitAmount 
        public double CrAmount { get; set; }//CreditAmount 
        public double BCDrAmount { get; set; } //BCDebitAmount 
        public double BCCrAmount { get; set; }//BCCreditAmount 
        public string Note { get; set; }// Voucher Narration  
    }
    #endregion
}
