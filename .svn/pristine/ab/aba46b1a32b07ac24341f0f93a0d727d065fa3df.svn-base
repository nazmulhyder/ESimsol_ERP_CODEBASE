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
    #region Export_LDBP
    public class Export_LDBP : BusinessObject
    {
        public Export_LDBP()
        {
            Export_LDBPID = 0;
            RefNo = "";
            BankAccountID = 0;
            LetterIssueDate = DateTime.Now;
            RequestBy = 0;
            ApprovedBy = 0;
            Note = "";
            AccountNo = "";
            BankName = "";
            BranchName = "";
            BranchAddress = "";
            RequestByName = "";
            ApprovedByName = "";
            ErrorMessage = "";
            Export_LDBPDetails = new List<Export_LDBPDetail>();
            BankAccounts = new List<BankAccount>();
            Users = new List<User>();
            BUID = 0;
           
        }

        #region Properties
        public int Export_LDBPID { get; set; }
        public string RefNo { get; set; }
        public int BankAccountID { get; set; }
        public DateTime LetterIssueDate { get; set; }
        public int RequestBy { get; set; }
        public int ApprovedBy { get; set; }
        public string Note { get; set; }
        public string AccountNo { get; set; }
        public int BankID { get; set; }
        public int BankBranchID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string RequestByName { get; set; }
        public string ApprovedByName { get; set; }
        
        public string ErrorMessage { get; set; }
         
        public bool CurrencyType{ get; set; }
         
         public EnumLCBillEvent Status { get; set; }
         
         public int StatusInt { get; set; }
         
         public int BUID { get; set; }
        #endregion

        #region Derived Property
         private string _sStatusSt = "";
         public string StatusSt
         {
             get
             {
                 if(this.StatusInt<=(int)EnumLCBillEvent.BOEinHand)
                 {
                     _sStatusSt = "Initialize";
                 }
                 else
                 {
                     _sStatusSt = this.Status.ToString();
                 }
                 return _sStatusSt;
             }
         }
         public int SelectedOption { get; set; }
         public DateTime LetterIssueDate_end { get; set; }
        
        public List<Export_LDBPDetail> Export_LDBPDetails { get; set; }
        
    
        
        public List<BankAccount> BankAccounts { get; set; }
        public List<BankBranch> BankBranchs { get; set; }
        public List<User> Users { get; set; }
        public string LetterIssueDateInSt
        {
            get
            {
                return this.LetterIssueDate.ToString("dd MMM yyyy");
            }
        }
        public string BankBranchName
        {
            get
            {
                return this.BankName + "["+this.BranchName+"]";
            }
        }
        public string BankBranchAccount
        {
            get
            {
                return this.BankName + "[" + this.BranchName + "] "+ this.AccountNo;
            }
        }
        public string CurrencyTypeST
        {
            get
            {
                if (this.CurrencyType)
                {
                    return "Foreign";
                }
                else
                {
                     return "Local";
                }


            }
        }   
        #endregion

        #region Functions

        public Export_LDBP Get(int nId, Int64 nUserID)
        {
            return Export_LDBP.Service.Get(nId, nUserID);
        }
        public static List<Export_LDBP> Gets( Int64 nUserID)
        {
            return Export_LDBP.Service.Gets( nUserID);
        }
        public static List<Export_LDBP> Gets(string sSQL,Int64 nUserID)
        {
            return Export_LDBP.Service.Gets( sSQL,nUserID);
        }
        public static List<Export_LDBP> WaitForApproval( int nBUID,Int64 nUserID)
        {
            return Export_LDBP.Service.WaitForApproval(nBUID,nUserID);
        }
        public Export_LDBP Save(Int64 nUserID)
        {
            return Export_LDBP.Service.Save(this, nUserID);
        }
        public Export_LDBP Cancel_Request(Int64 nUserID)
        {
            return Export_LDBP.Service.Cancel_Request(this, nUserID);
        }
        public Export_LDBP Approved(Int64 nUserID)
        {
            return Export_LDBP.Service.Approved(this, nUserID);
        }

        public string Delete(Int64 nUserID)
        {
            return Export_LDBP.Service.Delete(this, nUserID);
        }



        #endregion

        #region ServiceFactory

     
        internal static IExport_LDBPService Service
        {
            get { return (IExport_LDBPService)Services.Factory.CreateService(typeof(IExport_LDBPService)); }
        }

        #endregion
    }
    #endregion

    #region IExport_LDBP interface
    public interface IExport_LDBPService
    {
        Export_LDBP Get(int id, Int64 nUserID);
        List<Export_LDBP> WaitForApproval(int nBUID,Int64 nUserID);
        List<Export_LDBP> Gets(Int64 nUserID);
        List<Export_LDBP> Gets(string sSQL, Int64 nUserID);
        string Delete(Export_LDBP oExport_LDBP, Int64 nUserID);
        Export_LDBP Save(Export_LDBP oExport_LDBP, Int64 nUserID);
        Export_LDBP Cancel_Request(Export_LDBP oExport_LDBP, Int64 nUserID);
        Export_LDBP Approved(Export_LDBP oExport_LDBP, Int64 nUserID);
    }
    
    #endregion
}
