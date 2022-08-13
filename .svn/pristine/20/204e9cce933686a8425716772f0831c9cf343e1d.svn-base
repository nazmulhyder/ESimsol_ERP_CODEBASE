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
    #region ExportSC
    public class ExportSC : BusinessObject
    {
        public ExportSC()
        {
            ExportSCID = 0;
            PaymentType = EnumPIPaymentType.None;
            PaymentTypeInInt = 0;
            PINo = "";
            PIStatus = EnumPIStatus.Initialized;
            PIStatusInInt = 0;
            IssueDate = DateTime.Now;
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
            MotherBuyerID = 0;
            DeliveryToID = 0;
            ContractorContactPerson = 0;
            ExportLCNo = "";
            ContractorAddress = "";
            ContractorPhone = "";
            ContractorFax = "";
            ContractorEmail = "";
            MKTPName = "";
            MKTPNickName = "";
            Currency = "";
            ErrorMessage = "";            
            ExportSCDetails = new List<ExportSCDetail>();
            ExportSCActionInInt = 0;
            BranchAddress = "";
            ExportSCLogID = 0;
            ShipmentDate = DateTime.MinValue;
            Params = "";
            IsRevisePI = false;
            RateUnit = 1;
            ProductNature = EnumProductNature.Dyeing;
            ColorInfo = "";
            DepthOfShade = "";
            YarnCount = "";
            AmendmentNo = 0;
            YetToProductionOrderQty = 0;
            AmendmentRequired = false;
            MotherBuyerName = "";
            PIType = EnumPIType.Open;
			BankName = "";
			AccountName = "";
            BankBranchID = 0;
            BranchName = "";
            BankAccountNo = "";
            PTUUnit2s = new List<PTUUnit2>();
            RateAdjConID =0;
            QtyAdjConID =0;
            DicChargeAdjConID = 0;
            AdjManualy = 0;
            IsClose = false;

        }

        #region Properties
      
        public int ExportSCID { get; set; }
        public int ExportPIID { get; set; }
        public int ExportSCLogID { get; set; }
        public int BUID { get; set; }
        public EnumPIPaymentType PaymentType { get; set; }
        public int PaymentTypeInInt { get; set; }
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
        public double TotalAmount { get; set; }
        public double Qty_PI { get; set; }
        public double Amount_PI { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public int RateUnit { get; set; }
        public int BankBranchID { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }

        /// <summary>
        /// PI Qty and Rate adjusted amount
        /// </summary>
        public double AdjAmount { get; set; }
        public double SampleInvoiceAdjAmount { get; set; }
        public double AdjManualy { get; set; }
        public double AdjQty { get; set; }
        public DateTime LCOpenDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int LCID { get; set; }
        public int Production_ControlBy { get; set; }
        public int Delivery_ControlBy { get; set; }
        public double YetToProductionOrderQty { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string DeliveryToName { get; set; }
        public string ExportLCNo { get; set; }
        public string ContractorAddress { get; set; }
        public string MotherBuyerName { get; set; }
        public int  MotherBuyerID{ get; set; }
		public int DeliveryToID { get; set; }
		public int ContractorContactPerson{ get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string MKTPNickName { get; set; }
        public string Currency { get; set; }
        public string Params { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsRevisePI { get; set; }
        public int ContractorType { get; set; }
        public string ColorInfo { get; set; }
        public string DepthOfShade { get; set; }
        public string YarnCount { get; set; }
        public string ApprovedName { get; set; }
        public EnumPIType PIType { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public int RateAdjConID { get; set; }
        public int QtyAdjConID { get; set; }
        public int DicChargeAdjConID { get; set; }
        public bool IsClose { get; set; }
        #endregion

        #region Derive Property
      
        public int ExportSCActionInInt { get; set; }
        public List<PTUUnit2> PTUUnit2s { get; set; }
        public List<ExportSCDetail> ExportSCDetails { get; set; }
        
        public string BranchAddress { get; set; }
        public Company Company { get; set; }
        public int CurrentUserId { get; set; }
        public int ReviseNo { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ErrorMessage { get; set; }

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
                else if(this.AmendmentNo>0)
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
        #region IsRevisePI
        private string _sIsRevisePI;
        public string IsRevisePISt
        {
            get
            {
                if (this.IsRevisePI)
                {

                    _sIsRevisePI = "Not prepared for Production";
                }
                else
                {
                    _sIsRevisePI = "Approved";
                }
                return _sIsRevisePI;
            }
        }
        #endregion

        #region AmountToBeAdjusted 
        /// <summary>
        /// Amoung Sample invoice and PI adj amount whichever is higher
        /// </summary>
        private double _nAmountToBeAdjusted;
        public double AmountToBeAdjusted
        {
            get
            {
                _nAmountToBeAdjusted = this.AdjAmount + this.SampleInvoiceAdjAmount + this.AdjManualy;
              
                return _nAmountToBeAdjusted;
            }
        }
        #endregion

        public string IsCloseSt
        {
            get
            {
                if (this.IsClose == true) return "Close";
                else if (this.IsClose == false) return "Running";
                else return "-";
            }
        }

        public string PITypeInString
        {
            get
            {
                return this.PIType.ToString();
            }
        }
        public string ContractorNameCode
        {
            get
            {
                return this.ContractorName + "[" + this.ContractorID.ToString() + "]";
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
        public string ValidityDateInString
        {
            get
            {
                return this.ValidityDate.ToString("dd MMM yyyy");
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
        public string AmendmentDateST
        {
            get
            {
                if (this.AmendmentDate == DateTime.MinValue) return DateTime.Now.ToString("dd MMM yyyy");

                else return this.AmendmentDate.ToString("dd MMM yyyy");
            }
        }
        private string sPIStatusSt = "";
        public string PIStatusSt
        {
            get
            {
                 sPIStatusSt = EnumObject.jGet(this.PIStatus); 
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

        
        #endregion
      
        #region Functions
      
        public static List<ExportSC> Gets(Int64 nUserID)
        {
            return ExportSC.Service.Gets(nUserID);
        }
        public static List<ExportSC> Gets(string sSQL, Int64 nUserID)
        {
            return ExportSC.Service.Gets(sSQL, nUserID);
        }
    
        public static List<ExportSC> GetsLog(int nExportSCID, Int64 nUserID)
        {
            return ExportSC.Service.GetsLog(nExportSCID, nUserID);
        }
        public ExportSC Get(int id, Int64 nUserID)
        {
            return ExportSC.Service.Get(id, nUserID);
        }
        public ExportSC GetPI(int id, Int64 nUserID)
        {
            return ExportSC.Service.GetPI(id, nUserID);
        }
        public ExportSC Get(string sPINo, int nTextTileUnit, Int64 nUserID)
        {
            return ExportSC.Service.Get(sPINo, nTextTileUnit, nUserID);
        }
        public ExportSC Save(Int64 nUserID)
        {
            return ExportSC.Service.Save(this, nUserID);
        }
        public ExportSC SaveLog(Int64 nUserID)
        {
            return ExportSC.Service.SaveLog(this, nUserID);
        }
        public ExportSC AcceptRevise(Int64 nUserID)
        {
            return ExportSC.Service.AcceptRevise(this, nUserID);
        }
        public ExportSC Approved(Int64 nUserID)
        {
            return ExportSC.Service.Approved(this, nUserID);
        }

        public ExportSC ApproveSalesContract(Int64 nUserID)
        {
            return ExportSC.Service.ApproveSalesContract(this, nUserID);
        }
        public ExportSC UpdateExportSC(Int64 nUserID)
        {
            return ExportSC.Service.UpdateExportSC(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportSC.Service.Delete(this, nUserID);
        }
        public ExportSC Save_UP(Int64 nUserID)
        {
            return ExportSC.Service.Save_UP(this, nUserID);
        }
        public static List<ExportSC> GetsByBU(int nBUID, int nProductNature, Int64 nUserID)
        {
            return ExportSC.Service.GetsByBU(nBUID, nProductNature, nUserID);
        }
        public ExportSC OrderClose(Int64 nUserID)
        {
            return ExportSC.Service.OrderClose(this, nUserID);
        }
        public ExportSC UpdateBuyer(Int64 nUserID)
        {
            return ExportSC.Service.UpdateBuyer(this, nUserID);
        }
        public ExportSC ExportPIToPIOrderTransfer(int nExportPIID_TO, int nExportPIID_From,Int64 nUserID)
        {
            return ExportSC.Service.ExportPIToPIOrderTransfer( nExportPIID_TO,  nExportPIID_From, nUserID);
        }
        #endregion

        #region Non DB Functions
     
        public static string IDInString(List<ExportSC> oExportSCs)
        {
            string sReturn = "";
            foreach (ExportSC oItem in oExportSCs)
            {
                sReturn = sReturn + oItem.ExportSCID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        #endregion

        #region ServiceFactory
        internal static IExportSCService Service
        {
            get { return (IExportSCService)Services.Factory.CreateService(typeof(IExportSCService)); }
        }
        #endregion
    }
    #endregion

    #region IExportSC interface
    public interface IExportSCService
    {
        ExportSC Get(int id, Int64 nUserID);
        ExportSC Get(string sPINo, int nTextTileUnit, Int64 nUserID);
        ExportSC GetPI(int id, Int64 nUserID);
        List<ExportSC> Gets(Int64 nUserID);
        List<ExportSC> Gets(string sSQL, Int64 nUserID);
        List<ExportSC> GetsLog(int nExportSCID, Int64 nUserID);
        List<ExportSC> Gets(int nContractorID, string sLCIDs, Int64 nUserID);
        List<ExportSC> Gets(int nContractorID, string sLCIDs, bool bPaymentType, Int64 nUserID);
        List<ExportSC> GetsByBU(int nBUID,int nProductNature, Int64 nUserID);
        ExportSC Save(ExportSC oExportSC, Int64 nUserID);
        ExportSC Approved(ExportSC oExportSC, Int64 nUserID);
        ExportSC ApproveSalesContract(ExportSC oExportSC, Int64 nUserID);
        ExportSC UpdateExportSC(ExportSC oExportSC, Int64 nUserID);
        
        string Delete(ExportSC oExportSC, Int64 nUserID);
        List<ExportSC> GetsByPIIDs(string sIDs, Int64 nUserID);
        ExportSC SaveLog(ExportSC oExportSC, Int64 nUserID);
        ExportSC AcceptRevise(ExportSC oExportSC, Int64 nUserID);
        ExportSC Save_UP(ExportSC oExportSC, Int64 nUserID);
        ExportSC OrderClose(ExportSC oExportSC, Int64 nUserID);
        ExportSC UpdateBuyer(ExportSC oExportSC, Int64 nUserID);
        ExportSC ExportPIToPIOrderTransfer(int nExportPIID_TO, int nExportPIID_From, Int64 nUserID);
        
    }
    #endregion
}
