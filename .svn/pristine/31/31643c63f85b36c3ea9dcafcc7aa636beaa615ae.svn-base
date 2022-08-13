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
    #region FabricSalesContract
    public class FabricSalesContract : BusinessObject
    {
        public FabricSalesContract()
        {
            FabricSalesContractID = 0;
            PaymentType = EnumPIPaymentType.None;
            PaymentTypeInt = 0;
            SCNo = "";
            CurrentStatus = EnumFabricPOStatus.Initialized;
            CurrentStatusInt = 0;
            SCDate = DateTime.Now;
            //ValidityDate = SCDate.AddDays(7);
            ContractorID = 0;
            BuyerID = 0;
            MktAccountID = 0;
            CurrencyID = 2;
            LightSourceID = 0;
            LightSourceIDTwo = 0;
            Amount = 0;
            Qty = 0;
            IsInHouse = true;
            LCTermID = 8;
            Note = "";
            ApproveBy = 0;
            ApprovedDate = DateTime.MinValue;
            BUID = 0;
            ContractorName = "";
            BuyerName = "";            
            Con_Address = "";
            //ContractorPhone = "";
            //ContractorFax = "";
            //ContractorEmail = "";
            MKTPName = "";
            //MKTPNickName = "";
            Currency = "";
            LCTermsName = "";
            ErrorMessage = "";            
            FabricSalesContractDetails = new List<FabricSalesContractDetail>();
            FabricSalesContractNotes = new List<FabricSalesContractNote>();
            //FabricSalesContractLogID = 0;
            Params = "";
            //IsCreateReviseNo = false;
            //BuyerCPersonName = "";
            SCNoFull = "";
            AttCount = "";
            IsRevise = false;
            FabricDesignID = 0;
            FabricDesignName="";
            ExportPIID = 0;
            IsPrint = false;
            IsOpenPI = false;
            FabricSalesContractLogID = 0;
            OrderTypeSt = "";
            LCNo = "";
            LCDate = DateTime.MinValue;
            CheckedBy = 0;
            CheckedDate = DateTime.MinValue;
            CheckedByName = "";
        }

        #region Properties
        public int FabricSalesContractID { get; set; }
        public int FabricSalesContractLogID { get; set; }
        public EnumPIPaymentType PaymentType { get; set; }
        public int PaymentTypeInt { get; set; }
        public int OrderType { get; set; }
        public string SCNo { get; set; }
        public EnumFabricPOStatus CurrentStatus { get; set; }
        public int CurrentStatusInt { get; set; }
        public DateTime SCDate { get; set; }
        public int ContractorID { get; set; }
        public int BuyerID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int ContactPersonnelID_Buyer { get; set; }
        public int MktAccountID { get; set; }
        public int MktGroupID { get; set; }
        public int CurrencyID { get; set; }
        public int LightSourceID { get; set; }
        public int LightSourceIDTwo { get; set; }
        public int PaymentInstruction { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public bool IsInHouse { get; set; }
        public int LCTermID { get; set; }
        public string EndUse { get; set; }
        public string LightSourceName { get; set; }
        public string LightSourceNameTwo { get; set; }
        public string GarmentWash { get; set; }
        public string QualityParameters { get; set; }
        public string Note { get; set; }
        public string Emirzing { get; set; }
        public string QtyTolarance { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ContractorName { get; set; }
        public string BuyerName { get; set; }
        public string Con_Address { get; set; }
        public string BuyerAddress { get; set; }
        //public string ContractorPhone { get; set; }
        //public string ContractorFax { get; set; }
        //public string ContractorEmail { get; set; }
        public string MKTPName { get; set; }
        public string MKTGroupName { get; set; }
        //public string MKTPNickName { get; set; }
        public string Currency { get; set; }
        public string PreapeByName { get; set; }
        public int PreapeBy { get; set; }
        public string PreapeByDesignation { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveByDesignation { get; set; }
        public string CheckByDesignation { get; set; }
        public string LCTermsName { get; set; }
        public string Params { get; set; }
        //public int ContractorType { get; set; }
        public int BUID { get; set; }
        public int ExportPIID { get; set; }
        public int ReviseNo { get; set; }
        //public string BuyerCPersonName { get; set; }
        public bool IsRevise { get; set; }
        public bool IsPrint { get; set; }
        public bool IsOpenPI { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public string OrderTypeSt { get; set; }
        public string AttCount { get; set; }

        //public DateTime FabricReceiveDate { get; set; }
        //public int FabricReceiveBy { get; set; }
        public int CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public string CheckedByName { get; set; }
        
 
        #endregion

        #region Derive Property
        public System.Drawing.Image ConImage { get; set; }
        public List<FabricSalesContractDetail> FabricSalesContractDetails { get; set; }
        public List<FabricSalesContractNote> FabricSalesContractNotes { get; set; }
        public string LCNo { get; set; }
        public DateTime LCDate { get; set; }
        public string PINo { get; set; }
        public string SCNoFull { get; set; }
        //public int CurrentUserId { get; set; }
        public string ErrorMessage { get; set; }
        public string ContractorNameCode
        {
            get
            {
                return this.ContractorName + "[" + this.ContractorID.ToString() + "]";
            }
        }
        public string SCDateSt
        {
            get
            {
                return this.SCDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateSt
        {
            get
            {

                if (this.ApprovedDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string LCDateSt
        {
            get
            {
                if (LCDate == DateTime.MinValue) return "";
                return this.LCDate.ToString("dd MMM yyyy");
            }
        }
    
        private string sCurrentStatusSt = "";
        public string CurrentStatusSt
        {
            get
            {
               sCurrentStatusSt = EnumObject.jGet(this.CurrentStatus);
                return sCurrentStatusSt;
            }
        }
        
        public string PaymentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PaymentType);
            }
        }

        //public string FabricReceiveDateStr
        //{
        //    get
        //    {
        //        return (this.FabricReceiveDate == DateTime.MinValue) ? "" : this.FabricReceiveDate.ToString("dd MMM yyyy");
        //    }
        //}
      
        public System.Drawing.Image Signature { get; set; }
        public System.Drawing.Image PreparedBySignature { get; set; }
        public System.Drawing.Image ApprovedBySignature { get; set; }
        public System.Drawing.Image CheckBySignature { get; set; }

        #region AmountSt
        public string AmountSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount);
                //return this.Currency + "" + this.Amount.ToString("#,##0.0000");
            }
        }
        #endregion

        
        #endregion
      
        #region Functions

        public static List<FabricSalesContract> Gets(Int64 nUserID)
        {
            return FabricSalesContract.Service.Gets(nUserID);
        }
        public static List<FabricSalesContract> Gets(string sSQL, Int64 nUserID)
        {
            return FabricSalesContract.Service.Gets(sSQL, nUserID);
        }
        public static List<FabricSalesContract> GetsReport(string sSQL, Int64 nUserID)
        {
            return FabricSalesContract.Service.GetsReport(sSQL, nUserID);
        }
      
        public static List<FabricSalesContract> GetsLog(int nFabricSalesContractID, Int64 nUserID)
        {
            return FabricSalesContract.Service.GetsLog(nFabricSalesContractID, nUserID);
        }
        public FabricSalesContract Get(int id, Int64 nUserID)
        {
            return FabricSalesContract.Service.Get(id, nUserID);
        }
        public FabricSalesContract GetByLog(int nLogid, Int64 nUserID)
        {
            return FabricSalesContract.Service.GetByLog(nLogid, nUserID);
        }
        public string UpdateBySQL(string sSQL, Int64 nUserID)
        {
            return FabricSalesContract.Service.UpdateBySQL(sSQL, nUserID);
        }
        public FabricSalesContract Save(Int64 nUserID)
        {
            return FabricSalesContract.Service.Save(this, nUserID);
        }
        public FabricSalesContract SaveLog(Int64 nUserID)
        {
            return FabricSalesContract.Service.SaveLog(this, nUserID);
        }
        public FabricSalesContract SaveRevise(Int64 nUserID)
        {
            return FabricSalesContract.Service.SaveRevise(this, nUserID);
        }
        public FabricSalesContract Save_FSCNote(Int64 nUserID)
        {
            return FabricSalesContract.Service.Save_FSCNote(this, nUserID);
        }
        public FabricSalesContract Approved(Int64 nUserID)
        {
            return FabricSalesContract.Service.Approved(this, nUserID);
        }
        public FabricSalesContract UpdateInfo(Int64 nUserID)
        {
            return FabricSalesContract.Service.UpdateInfo(this, nUserID);
        }
        public FabricSalesContract Copy(Int64 nUserID)
        {
            return FabricSalesContract.Service.Copy(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return FabricSalesContract.Service.Delete(this, nUserID);
        }
        public static List<FabricSalesContract> Gets_DistinctItem(string sSQL, Int64 nUserID)
        {
            return FabricSalesContract.Service.Gets_DistinctItem(sSQL, nUserID);
        }
        public static List<FabricSalesContract> GetsByPI(int nPIID, Int64 nUserID)
        {
            return FabricSalesContract.Service.GetsByPI(nPIID,nUserID);
        }
        public static FabricSalesContract ReceiveFabric(int nFSCID, DateTime dtReceive, Int64 nUserID)
        {
            return FabricSalesContract.Service.ReceiveFabric(nFSCID, dtReceive, nUserID);
        }
        public FabricSalesContract Cancel(long nUserID)
        {
            return FabricSalesContract.Service.Cancel(this, nUserID);
        }
        public FabricSalesContract Check(long nUserID)
        {
            return FabricSalesContract.Service.Check(this, nUserID);
        }
        public FabricSalesContract SaveSampleInvoice(Int64 nUserID)
        {
            return FabricSalesContract.Service.SaveSampleInvoice(this, nUserID);
        }
        public FabricSalesContract UpdateReviseNo(int id, int ReviseNo, int nUserID)
        {
            return FabricSalesContract.Service.UpdateReviseNo(id, ReviseNo, nUserID);
        }
        #endregion

        #region Non DB Functions
     
     
        public static string IDInString(List<FabricSalesContract> oFabricSalesContracts)
        {
            string sReturn = "";
            foreach (FabricSalesContract oItem in oFabricSalesContracts)
            {
                sReturn = sReturn + oItem.FabricSalesContractID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        #endregion

        #region ServiceFactory
        internal static IFabricSalesContractService Service
        {
            get { return (IFabricSalesContractService)Services.Factory.CreateService(typeof(IFabricSalesContractService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSalesContract interface
    public interface IFabricSalesContractService
    {
        FabricSalesContract Get(int id, Int64 nUserID);
        FabricSalesContract GetByLog(int id, Int64 nUserID);
        List<FabricSalesContract> Gets(Int64 nUserID);
        List<FabricSalesContract> GetsByPI(int nPIID,Int64 nUserID);
        List<FabricSalesContract> Gets(string sSQL, Int64 nUserID);
        List<FabricSalesContract> GetsReport(string sSQL, Int64 nUserID);
        string UpdateBySQL(string sSQL, Int64 nUserID);
        List<FabricSalesContract> GetsLog(int nFabricSalesContractID, Int64 nUserID);
        FabricSalesContract Save(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract SaveLog(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract SaveRevise(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract Approved(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract Save_FSCNote(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract UpdateInfo(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract Copy(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        string Delete(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        List<FabricSalesContract> Gets_DistinctItem(string sSQL, Int64 nUserID);
        FabricSalesContract Cancel(FabricSalesContract oFabricSalesContract, long nUserID);
        FabricSalesContract Check(FabricSalesContract oFabricSalesContract, long nUserID);
        FabricSalesContract ReceiveFabric(int nFSCID, DateTime dtReceive, Int64 nUserID);
        FabricSalesContract SaveSampleInvoice(FabricSalesContract oFabricSalesContract, Int64 nUserID);
        FabricSalesContract UpdateReviseNo(int id, int ReviseNo, int nUserID);
        
    }
    #endregion
}
