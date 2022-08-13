using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region LCTransfer
    
    public class LCTransfer : BusinessObject
    {
        public LCTransfer()
        {
            LCTransferID = 0;
            MasterLCID = 0;
            RefNo = "";
            LCTransferStatus = EnumLCTransferStatus.Initialize;
            TransferIssueDate = DateTime.Now;
            ProductionFactoryID = 0;
            BuyerID = 0;
            CommissionFavorOf = 0;
            CommissionAccountID = 0;
            TransferNo = "";
            TransferDate = DateTime.Now;
            TransferAmount = 0;
            CommissionAmount = 0;
            ApprovedBy = 0;
            Note = "";
            IsCommissionCollect = false;
            ProductionFactoryName = "";
            BuyerName = "";
            LCFavorOfName = "";
            AccountNo = "";
            BranchName = "";
            BankName = "";
            MasterLCNo = "";
            LCStatus = EnumLCStatus.None;
            LCType = EnumLCType.At_Sight;
            LCTransferQty = 0;
            LCValue = 0;
            YetToTransferValue = 0;
            YetToInvoiceAmount = 0;
            ApprovedByName = "";
            BankNameAccountNo = "";
            LCDate = DateTime.Now;
            FactoryAddress = "";
            FactoryPhone = "";
            FactoryOrigin = "";
            FactoryEmail = "";
            BankAddress = "";
            FactoryBranchID = 0;
            FactoryBankBranchName = "";
            FactoryBankName = "";
            FactorBankAddress = "";
            SwiftCode = "";
            LCTransferStatusInInt = 0;
            LCTransferLogID = 0;
            LCTranferActionType = EnumLCTranferActionType.None;
            ActionTypeExtra = "";            
            ErrorMessage = "";
            BUID = 0;
            //DocumentTypes = new List<DocumentType>();
            ActualTransferAmount = 0;
        }

        #region Properties
        public int LCTransferID { get; set; }
        public int LCTransferLogID { get; set; }
        public int MasterLCID { get; set; }
        public string RefNo { get; set; }
        public DateTime TransferIssueDate { get; set; }
        public int ProductionFactoryID { get; set; }
        public int BuyerID { get; set; }
        public int CommissionFavorOf { get; set; }
        public int CommissionAccountID { get; set; }
        public string TransferNo { get; set; }
        public DateTime TransferDate { get; set; }
        public double TransferAmount { get; set; }
        public double CommissionAmount { get; set; }
        public int ApprovedBy { get; set; }
        public int FactoryBranchID { get; set; }
        public string Note { get; set; }
        public bool IsCommissionCollect { get; set; }
        public string ProductionFactoryName { get; set; }
        public string BankNameAccountNo  { get; set; }
        public string BuyerName { get; set; }
        public string LCFavorOfName { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string BankName { get; set; }
        public string MasterLCNo { get; set; }
        public EnumLCStatus LCStatus { get; set; }
        public EnumLCType LCType   {get;set;}
        public double LCValue { get; set; }
        public double LCTransferQty { get; set; }
        public double YetToTransferValue { get; set; }
        public double YetToInvoiceAmount { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime LCDate{ get; set; }
        public string BankAddress { get; set; }
        public string FactoryAddress { get; set; }
        public string FactoryOrigin { get; set; }
        public string FactoryPhone { get; set; }
        public string FactoryEmail { get; set; }
        public string ActionTypeExtra { get; set; }
        public string FactoryBankBranchName { get; set; }
        public string FactoryBankName { get; set; }
        public string FactorBankAddress { get; set; }         
        public string SwiftCode { get; set; }         
        public EnumLCTransferStatus LCTransferStatus { get; set; }         
        public EnumLCTranferActionType LCTranferActionType { get; set; }
        public double ActualTransferAmount { get; set; }
        public int LCTransferStatusInInt { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<LCTransferDetail> LCTransferDetails {get;set;}
        public List<LCTransfer> LCTransfers{ get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<BankBranch> BankBranches { get; set; }
        public List<BusinessSession> BussinessSessions { get; set; }
         
        public ClientOperationSetting ClientOperationSetting { get; set; }
        public List<Company> Companies { get; set; }
        public List<CommercialInvoice> CommercialInvoices { get; set; }
        //public List<DocumentType> DocumentTypes { get; set; }
        public List<ReportLayout> ReportLayouts { get; set; }
        public MasterLC MasterLC { get; set; }

        public string LCTransferStatusInString
        {
            get
            {
                return this.LCTransferStatus.ToString();
            }
        }
        public string LCStatusInString
        {
            get
            {
                return this.LCStatus.ToString();
            }
        }


        public string TransferIssueDateInString
        {
            get
            {
                return this.TransferIssueDate.ToString("dd MMM yyyy");
            }
        }

        public string TransferDateInString
        {
            get
            {
                return this.TransferDate.ToString("dd MMM yyyy");
            }
        }

        public string LCDateInString
        {
            get
            {
                return this.LCDate.ToString("dd MMM yyyy");
            }
        }
        public string LCTypeInString
        {
            get
            {
                return this.LCType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<LCTransfer> Gets(long nUserID)
        {
            return LCTransfer.Service.Gets( nUserID);
        }

        public static List<LCTransfer> Gets(string sSQL, long nUserID)
        {
            return LCTransfer.Service.Gets(sSQL, nUserID);
        }

        public static List<LCTransfer> Gets(int id, long nUserID) //id is Master LC ID
        {
            return LCTransfer.Service.Gets(id, nUserID);
        }
        public LCTransfer GetLog(int id, long nUserID)
        {
            return LCTransfer.Service.GetLog(id, nUserID);
        }
        public LCTransfer Get(int id, long nUserID)
        {           
            return LCTransfer.Service.Get(id, nUserID);
        }

        public LCTransfer Save(long nUserID)
        {           
            return LCTransfer.Service.Save(this, nUserID);
        }
        public LCTransfer AcceptLCTransferRevise(long nUserID)
        {
            return LCTransfer.Service.AcceptLCTransferRevise(this, nUserID);
        }
        public LCTransfer ChangeStatus(long nUserID)
        {
            return LCTransfer.Service.ChangeStatus(this, nUserID);
        }
        public LCTransfer UpdateTransferNoDate(long nUserID)
        {
            return LCTransfer.Service.UpdateTransferNoDate(this, nUserID);
        }
        public string Delete(int nLCTransferID, long nUserID)
        {
            return LCTransfer.Service.Delete(nLCTransferID, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ILCTransferService Service
        {
            get { return (ILCTransferService)Services.Factory.CreateService(typeof(ILCTransferService)); }
        }

        #endregion
    }
    #endregion

    #region ILCTransfer interface
     
    public interface ILCTransferService
    {         
        LCTransfer Get(int id, Int64 nUserID);         
        LCTransfer GetLog(int id, Int64 nUserID);
        List<LCTransfer> Gets(Int64 nUserID);
        List<LCTransfer> Gets(string sSQL, Int64 nUserID);
        List<LCTransfer> Gets(int id, Int64 nUserID);
        LCTransfer AcceptLCTransferRevise(LCTransfer oLCTransfer, Int64 nUserID);
        LCTransfer ChangeStatus(LCTransfer oLCTransfer, Int64 nUserID);
        LCTransfer Save(LCTransfer oLCTransfer, Int64 nUserID);
        LCTransfer UpdateTransferNoDate(LCTransfer oLCTransfer, Int64 nUserID);
        string Delete(int nLCTransferID, Int64 nUserID);
    }
    #endregion
}
