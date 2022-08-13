using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region WUSubContract

    public class WUSubContract : BusinessObject
    {
        public WUSubContract()
        {
            WUSubContractID = 0;
            BUID = 0;
            JobNo = "";
            Version = 0;
            ContractDate = DateTime.Today;
            SupplierID = 0;
            ContractPersonID = 0;
            ContractBy = 0;
            ContractStatus = EnumWUSubContractStatus.Initialize;
            ContractStatusInt = 1;
            YarnChallanStatus = EnumWUYarnChallanStatus.YetToChallanStart;
            YarnChallanStatusInt = 1;
            FabricRcvStatus = EnumWUFabricRcvStatus.YetToRcvStart;
            FabricRcvStatusInt = 1;
            PaymentMode = EnumInvoicePaymentMode.None;
            PaymentModeInt = 0;
            SONo = "";
            BuyerID = 0;
            StyleNo = "";
            OrderType = EnumWUOrderType.None;
            OrderTypeInt = 0;
            FabricTypeID = 0;
            CompositionID = 0;
            Construction = "";
            GrayWidth = "";
            GrayPick = 0;
            ReedSpace = "";
            TotalEnds = 0;
            ReedCount = "";
            WeaveDesignID = 0;
            WSCWorkType = EnumWSCWorkType.None;
            WSCWorkTypeInt = 0;
            OrderQty = 0;
            MUnitID = 0;
            Rate = 0;
            RatePerPick = 0;
            TotalAmount = 0;
            CRate = 0;
            CurrencyID = 0;
            Transportation = EnumTransportation.None;
            TransportationInt = 0;
            ProdStartDate = DateTime.Today;
            ProdStartComments = "";
            ProdCompleteDate = DateTime.Today;
            Remarks = "";
            ApprovedBy = 0;
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            BUName = "";
            BUShortName = "";
            SupplierName = "";
            BuyerName = "";
            SupplierCPName = "";
            CUSymbol = "";
            ContractByName = "";
            FabricTypeName = "";
            CompositionCode = "";
            CompositionName = "";
            WeaveDesignName = "";
            MUSymbol = "";
            ApprovedByName = "";
            EntyUserName = "";
            ErrorMessage = "";
            SupplierAddress = "";
            SupplierCPDesignation = "";
            EmpDesignation = "";
            WUSubContractLogID = 0;
            YetToRcvQty = 0;
            BaseCurrencyID = 0;
            BCSymbol = "";
            ReportLayout = EnumReportLayout.None;
            WUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
            WUSubContractTermsConditions = new List<WUSubContractTermsCondition>();
        }

        #region Properties
        public int WUSubContractID { get; set; }
        public int BUID { get; set; }
        public string JobNo { get; set; }
        public int Version { get; set; }
        public DateTime ContractDate { get; set; }
        public int SupplierID { get; set; }
        public int ContractPersonID { get; set; }
        public int ContractBy { get; set; }
        public EnumWUSubContractStatus ContractStatus { get; set; }
        public int ContractStatusInt { get; set; }
        public EnumWUYarnChallanStatus YarnChallanStatus { get; set; }
        public int YarnChallanStatusInt { get; set; }
        public EnumWUFabricRcvStatus FabricRcvStatus { get; set; }
        public int FabricRcvStatusInt { get; set; }
        public EnumInvoicePaymentMode PaymentMode { get; set; }
        public int PaymentModeInt { get; set; }
        public string SONo { get; set; }
        public int BuyerID { get; set; }
        public string StyleNo { get; set; }
        public EnumWUOrderType OrderType { get; set; }
        public int OrderTypeInt { get; set; }
        public int FabricTypeID { get; set; }
        public int CompositionID { get; set; }
        public string Construction { get; set; }
        public string GrayWidth { get; set; }
        public int GrayPick { get; set; }
        public string ReedSpace { get; set; }
        public int TotalEnds { get; set; }
        public string ReedCount { get; set; }
        public int WeaveDesignID { get; set; }
        public EnumWSCWorkType WSCWorkType { get; set; }
        public int WSCWorkTypeInt { get; set; }
        public double OrderQty { get; set; }
        public int MUnitID { get; set; }
        public double Rate { get; set; }
        public double RatePerPick { get; set; }
        public double TotalAmount { get; set; }
        public double CRate { get; set; }
        public int CurrencyID { get; set; }
        public EnumTransportation Transportation { get; set; }
        public int TransportationInt { get; set; }
        public DateTime ProdStartDate { get; set; }
        public string ProdStartComments { get; set; }
        public DateTime ProdCompleteDate { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string SupplierName { get; set; }
        public string BuyerName { get; set; }
        public string SupplierCPName { get; set; }
        public string CUSymbol { get; set; }
        public string ContractByName { get; set; }
        public string FabricTypeName { get; set; }
        public string CompositionCode { get; set; }
        public string CompositionName { get; set; }
        public string WeaveDesignName { get; set; }
        public string MUSymbol { get; set; }
        public string ApprovedByName { get; set; }
        public string EntyUserName { get; set; }
        public string ErrorMessage { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierCPDesignation { get; set; }
        public string EmpDesignation { get; set; }
        public int WUSubContractLogID { get; set; }
        public double YetToRcvQty { get; set; }
        public int BaseCurrencyID { get; set; }
        public string BCSymbol { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public List<WUSubContractYarnConsumption> WUSubContractYarnConsumptions { get; set; }
        public List<WUSubContractTermsCondition> WUSubContractTermsConditions { get; set; }

        #endregion

        #region Derived Property

        public string FullJobNoSt
        {
            get
            {
                if(Version == 0)
                {
                    return this.JobNo.ToString();
                }
                else
                {
                    return this.JobNo.ToString() + "/" + this.Version.ToString();
                }                
            }
        }
        public string OrderQtySt
        {
            get
            {
                return this.OrderQty.ToString("#,##0.00") + " " + this.MUSymbol;
            }
        }
        public string RateSt
        {
            get
            {
                return this.CUSymbol + " " + this.Rate.ToString("#,##0.0000");
            }
        }
        public string TotalAmountSt
        {
            get
            {
                return this.CUSymbol + " " + this.TotalAmount.ToString("#,##0.00");
            }
        }
        public string CRateSt
        {
            get
            {
                return this.BCSymbol + " " + this.CRate.ToString("#,##0.00");
            }
        }
        public string ContractDateSt
        {
            get
            {
                return this.ContractDate.ToString("dd MMM yyyy");
            }
        }
        public string ProdStartDateSt
        {
            get
            {
                return this.ProdStartDate.ToString("dd MMM yyyy");
            }
        }
        public string ProdCompleteDateSt
        {
            get
            {
                return this.ProdCompleteDate.ToString("dd MMM yyyy");
            }
        }
        public string ContractStatusSt
        {
            get
            {
                return EnumObject.jGet(this.ContractStatus);
            }
        }
        public string YarnChallanStatusSt
        {
            get
            {
                return EnumObject.jGet(this.YarnChallanStatus);
            }
        }
        public string FabricRcvStatusSt
        {
            get
            {
                return EnumObject.jGet(this.FabricRcvStatus);
            }
        }
        public string YetToRcvQtySt
        {
            get
            {
                return this.YetToRcvQty.ToString("#,##0.00");
            }
        }

        #endregion

        #region Functions

        public static List<WUSubContract> Gets(int bid, long nUserID)
        {
            return WUSubContract.Service.Gets(bid, nUserID);
        }
        public WUSubContract Get(int nId, long nUserID)
        {
            return WUSubContract.Service.Get(nId, nUserID);
        }
        public WUSubContract GetRevise(int nId, long nUserID)
        {
            return WUSubContract.Service.GetRevise(nId, nUserID);
        }
        public static List<WUSubContract> Get(string sSQL, int nCurrentUserID)
        {
            return WUSubContract.Service.Get(sSQL, nCurrentUserID);
        }
        public static List<WUSubContract> GetsPrint(string sSQL, int nCurrentUserID)
        {
            return WUSubContract.Service.GetsPrint(sSQL, nCurrentUserID);
        }
        public WUSubContract Save(long nUserID)
        {
            return WUSubContract.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return WUSubContract.Service.Delete(nId, nUserID);
        }
        public WUSubContract Approve(long nUserID)
        {
            return WUSubContract.Service.Approve(this, nUserID);
        }
        public WUSubContract FinishYarnChallan(long nUserID)
        {
            return WUSubContract.Service.FinishYarnChallan(this, nUserID);
        }
        public WUSubContract AcceptRevise(long nUserID)
        {
            return WUSubContract.Service.AcceptRevise(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWUSubContractService Service
        {
            get { return (IWUSubContractService)Services.Factory.CreateService(typeof(IWUSubContractService)); }
        }
        #endregion
    }
    #endregion

    #region IWUSubContract interface
    
    public interface IWUSubContractService
    {   
        WUSubContract Get(int id, long nUserID);
        WUSubContract GetRevise(int id, long nUserID);
        List<WUSubContract> Gets(int bid, long nUserID);
        List<WUSubContract> Get(string sSQL, int nCurrentUserID);
        List<WUSubContract> GetsPrint(string sSQL, int nCurrentUserID);
        string Delete(int id, long nUserID);
        WUSubContract Save(WUSubContract oWUSubContract, long nUserID);
        WUSubContract Approve(WUSubContract oWUSubContract, long nUserID);
        WUSubContract FinishYarnChallan(WUSubContract oWUSubContract, long nUserID);
        WUSubContract AcceptRevise(WUSubContract oWUSubContract, long nUserID);
    }

    #endregion
}