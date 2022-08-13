using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportSCDO
    public class ExportSCDO : BusinessObject
    {
        public ExportSCDO()
        {
            ExportSCID = 0;
            PaymentType = EnumPIPaymentType.None;
            PaymentTypeInInt = 0;
            PINo = "";
            PIStatus = EnumPIStatus.Initialized;
            PIStatusInInt = 0;
            IssueDate = DateTime.Now;
            ValidityDate = DateTime.Now;
            AmendmentDate = DateTime.MinValue;
            MKTEmpID = 0;
            CurrencyID = 0;
            TotalQty = 0;
            TotalAmount = 0;
            SCDate = DateTime.Now;
            LCOpenDate = DateTime.Now;
            DeliveryDate = DateTime.Now;
            Note = "";
            ApprovedBy = 0;
            ApprovedDate = DateTime.Now;
            LCID = 0;
            ContractorName = "";
            BuyerName = "";
            DeliveryToName = "";
            ExportLCNo = "";
            ContractorAddress = "";
            ContractorPhone = "";
            ContractorFax = "";
            ContractorEmail = "";
            MKTPName = "";
            MKTPNickName = "";
            Currency = "";
            ErrorMessage = "";
            BranchAddress = "";
            ShipmentDate = DateTime.MinValue;
            Params = "";
            IsApproved = false;
            ColorInfo = "";
            DepthOfShade = "";
            PrepareBy = "";
            DUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
            DUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            DUDeliveryChallans = new List<DUDeliveryChallan>();
            DUReturnChallans = new List<DUReturnChallan>();
            DUReturnChallanDetails = new List<DUReturnChallanDetail>();
            DUClaimOrders = new List<DUClaimOrder>();
            DUClaimOrderDetails = new List<DUClaimOrderDetail>();
            DUDeliveryOrderDetails_Claim = new List<DUDeliveryOrderDetail>();
            DUDeliveryChallanDetails_Claim = new List<DUDeliveryChallanDetail>();
            DUReturnChallanDetails_Claim = new List<DUReturnChallanDetail>();
            ContractorContactPersonName = "";
            BuyerContactPersonName = "";
            AmendmentNo = 0;
            CurrentStatus_LC = 0;
            IsRevisePI = false;
            ContractorID = 0;
            BuyerID = 0;
            POQty = 0;
            Qty_PI = 0;
            Amount_PI = 0;
            Qty_RC = 0;
            Production_ControlBy = 0;
            Delivery_ControlBy = 0;
            AmendmentRequired = false;
            AcceptanceIssue = 0;
            AcceptanceRcvd = 0;
            MaturityDate = "";
            PaymentDate = "";
            Amount_LC = 0;
            BUID = 0;
        }

        #region Properties
      
        public int ExportSCID { get; set; }
        public int ExportPIID { get; set; }
        public EnumPIPaymentType PaymentType { get; set; }
        public int PaymentTypeInInt { get; set; }
        public int ProductionType { get; set; }
        public string PINo { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public int PIStatusInInt { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime SCDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public int ContractorID { get; set; }
        public int BuyerID { get; set; }
        public int MKTEmpID { get; set; }
        public int CurrencyID { get; set; }
        public double TotalQty { get; set; }
        public double POQty { get; set; }
        public double TotalAmount { get; set; }
        public double Qty_PI { get; set; }
        public double Amount_PI { get; set; }
        public double AdjAmount { get; set; }
        public double AdjQty { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int LCID { get; set; }
        public int Production_ControlBy { get; set; }
        public int Delivery_ControlBy { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string DeliveryToName { get; set; }
        public string ExportLCNo { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string MKTPNickName { get; set; }
        public string Currency { get; set; }
        public string Params { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsApproved { get; set; }
        public int ContractorType { get; set; }
        public string ColorInfo { get; set; }
        public string DepthOfShade { get; set; }
        public string PrepareBy { get; set; }
        public string MUName { get; set; }
        public int ReviseNo { get; set; }
        public bool IsRevisePI { get; set; }
        public int BUID { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string ContractorContactPersonName { get; set; }
        public string BuyerContactPersonName { get; set; }
      
        #endregion

        #region Derive Property

        #region PINo_Full
        private string _sPINo_Full = "";
        public string PINo_Full
        {
            get
            {
                if (this.ReviseNo > 0)
                {

                    _sPINo_Full = this.PINo + "R-" + this.ReviseNo;
                }
                else
                {
                    _sPINo_Full = this.PINo;
                }
                return _sPINo_Full;
            }
        }
        #endregion
        #region ELCNo
        private string _sELCNo;
        public string ELCNo
        {
            get
            {
                if (String.IsNullOrEmpty(this.ExportLCNo))
                {

                    _sELCNo = "Waiting For L/C ";
                }
                else if (this.AmendmentNo > 0)
                {
                    _sELCNo = this.ExportLCNo + " A-" + this.AmendmentNo.ToString();
                }
                else
                {
                    _sELCNo = this.ExportLCNo;
                }
                return _sELCNo;
            }
        }
        #endregion

        public List<ExportSCDetailDO> ExportSCDetailDOs { get; set; }
        public List<ExportPIDetail> ExportPIDetails { get; set; }
        public List<DyeingOrderDetail> DyeingOrderDetails { get; set; }
        public List<DUDeliveryChallan> DUDeliveryChallans { get; set; }
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails { get; set; }
        public List<DUDeliveryOrderDetail> DUDeliveryOrderDetails { get; set; }
        public List<DUReturnChallan> DUReturnChallans { get; set; }
        public List<DUReturnChallanDetail> DUReturnChallanDetails { get; set; }
        public List<DUClaimOrder> DUClaimOrders { get; set; }
        public List<DUClaimOrderDetail> DUClaimOrderDetails { get; set; }
        public List<DUDeliveryOrderDetail> DUDeliveryOrderDetails_Claim { get; set; }
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails_Claim { get; set; }
        public List<DUReturnChallanDetail> DUReturnChallanDetails_Claim { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_RC { get; set; }
        public double Qty_DO { get; set; }
        public double Qty_Claim { get; set; }
        public string BranchAddress { get; set; }
        public int CurrentUserId { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ErrorMessage { get; set; }
        public string AmendmentDateST
        {
            get
            {
                if (this.AmendmentDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.AmendmentDate.ToString("dd MMM yyyy");
            }
        }
    

        public string ContractorNameCode
        {
            get
            {
                return this.ContractorName + "[" + this.ContractorID.ToString() + "]";
            }
        }
      
        public string YetToDO
        {
            get
            {
                return Global.MillionFormat(this.TotalQty - this.POQty) ;
            }
        }
     
        public string Amount_PISt { get { return this.Currency+""+Global.MillionFormat(this.Amount_PI); } }

    
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateInString
        {
            get
            {
                return this.ApprovedDate.ToString("dd MMM yyyy");
            }
        }
        public string ExportLCDateSt
        {
            get
            {
                if (this.LCOpenDate == DateTime.MinValue) return "--";

                else return this.LCOpenDate.ToString("dd MMM yyyy");
            }
        }
        public string LCOpenDateSt
        {
            get
            {
                return this.LCOpenDate.ToString("dd MMM yyyy");
            }
        }

        public string DeliveryDateInString
        {
            get
            {
                return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }
        public string LCOpenDateInST
        {
            get
            {
                if (this.LCOpenDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.LCOpenDate.ToString("dd MMM yyyy");
            }
        }
        public string DeliveryDateInST
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.DeliveryDate.ToString("dd MMM yyyy");
            }
        }

        private string sPIStatusSt = "";
        public string PIStatusSt
        {
            get
            {
                
                if (this.PIStatus == EnumPIStatus.PIIssue)
                {
                    sPIStatusSt = "Waiting for L/C";
                }
                else
                {
                    sPIStatusSt = EnumObject.jGet(this.PIStatus);
                }
                return sPIStatusSt;
            }
        }
        private string sExportLCStatusS = "";
        public string ExportLCStatusSt
        {
            get
            {
                if (String.IsNullOrEmpty(this.ExportLCNo))
                {
                    sExportLCStatusS = "Wating For L/C";
                }
                else if (this.AmendmentRequired)
                {
                    sExportLCStatusS = "Amendment Required";
                }
                else
                {
                    sExportLCStatusS = EnumObject.jGet((EnumExportLCStatus)this.CurrentStatus_LC);
                }
                return sExportLCStatusS;
            }
        }
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentType);
            }
        }
       
        public string ShipmentDateInStr
        {
            get
            {
                if (this.ShipmentDate != DateTime.MinValue)
                    return ShipmentDate.ToString("dd MMM yyyy");
                else
                    return "";
            }
        }        
        
        public List<Company> Companys { get; set; }
        public System.Drawing.Image Signature { get; set; }

        #region AmountSt
        public string AmountSt
        {
            get
            {
                //return this.Currency + "" + Global.MillionFormat(Amount,4);)
                return this.Currency + "" + this.TotalAmount.ToString("#,##0.0000");
            }
        }
        #endregion

        public double AcceptanceIssue { get; set; }
        public double AcceptanceRcvd { get; set; }
        public string MaturityDate { get; set; }
        public string PaymentDate { get; set; }
        public double Amount_LC { get; set; }
        #endregion
      
        #region Functions
        public ExportSCDO Get(int nId, long nUserID)
        {
            return ExportSCDO.Service.Get(nId, nUserID);
        }
        public ExportSCDO GetByPI(int nExportPIID, long nUserID)
        {
            return ExportSCDO.Service.GetByPI(nExportPIID, nUserID);
        }
        public static List<ExportSCDO> Gets(Int64 nUserID)
        {
            return ExportSCDO.Service.Gets(nUserID);
        }
        public static List<ExportSCDO> Gets(string sSQL, Int64 nUserID)
        {
            return ExportSCDO.Service.Gets(sSQL, nUserID);
        }
      
    
     
        #endregion

        #region Non DB Functions
     
       

        #endregion

        #region ServiceFactory
        internal static IExportSCDOService Service
        {
            get { return (IExportSCDOService)Services.Factory.CreateService(typeof(IExportSCDOService)); }
        }
        #endregion
    }
    #endregion

    #region IExportSCDO interface
    public interface IExportSCDOService
    {
        ExportSCDO Get(int id, Int64 nUserID);
        ExportSCDO GetByPI(int nExportPIID, Int64 nUserID);
        List<ExportSCDO> Gets(Int64 nUserID);
        List<ExportSCDO> Gets(string sSQL, Int64 nUserID);
       
    }
    #endregion
}
