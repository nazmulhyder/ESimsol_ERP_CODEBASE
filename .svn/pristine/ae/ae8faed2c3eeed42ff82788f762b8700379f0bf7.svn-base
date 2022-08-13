using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ChequeBook
    
    public class ChequeBook : BusinessObject
    {
        public ChequeBook()
        {
            ChequeBookID = 0;
            BankAccountID = 0;
            BookCodePartOne = "";
            BookCodePartTwo = "";
            PageCount = 0;
            FirstChequeNo = "";
            IsActive = false;
            ActivteBy = 0;
            ActivateTime = DateTime.Now;
            Note = "";
            DBServerDateTime = DateTime.Now;            
            BankBranchID = 0;
            BankID = 0;
            BusinessUnitID = 0;
            AccountNo = "";
            AccountType = 0;
            AccountName = "";
            BankName = "";
            BankShortName = "";
            BankBranchName = "";
            BusinessUnitName = "";
            BusinessUnitNameCode = "";
            ChequeSetupName = "";
            Cheques = new List<Cheque>();
            ErrorMessage = "";
        }

        #region Properties
        
        public int ChequeBookID { get; set; }
        
        public int BankAccountID { get; set; }
        
        public string BookCodePartOne { get; set; }
        
        public string BookCodePartTwo { get; set; }
        
        public int PageCount { get; set; }
        
        public string FirstChequeNo { get; set; }
        
        public bool IsActive { get; set; }
        
        public int ActivteBy { get; set; }
        
        public DateTime ActivateTime { get; set; }
        
        public string Note { get; set; }
        
        public DateTime DBServerDateTime { get; set; }
        
        public int BankBranchID { get; set; }
        
        public int BankID { get; set; }
        
        public int BusinessUnitID { get; set; }
        
        public string AccountNo { get; set; }
        
        public int AccountType { get; set; }
        
        public string AccountName { get; set; }
        
        public string BankName { get; set; }
        
        public string BankShortName { get; set; }
        
        public string BankBranchName { get; set; }
        
        public string BusinessUnitName { get; set; }

        public string BusinessUnitNameCode { get; set; }
        
        public string ChequeSetupName { get; set; }

        
        public List<Cheque> Cheques { get; set; }
        
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public string BookCode { get { return this.BookCodePartOne + "-" + this.BookCodePartTwo; } }
        public string DBServerDateTimeInString { get { return (this.DBServerDateTime == DateTime.MinValue) ? "" : this.DBServerDateTime.ToString("dd MMM yyyy"); } }
        
        

        public string IsActiveInString { get { return this.IsActive ? "Active" : "In-active"; } }
        public int IsActiveInInt { get { return this.IsActive ? 1 : 0; } }
        #endregion

        #region Functions
        public static List<ChequeBook> Gets(int nCurrentUserID)
        {
            return ChequeBook.Service.Gets(nCurrentUserID);
        }
        public ChequeBook Get(int id, int nCurrentUserID)
        {
            return ChequeBook.Service.Get(id, nCurrentUserID);
        }
        public ChequeBook Save(int nCurrentUserID)
        {
            return ChequeBook.Service.Save(this, nCurrentUserID);
        }
        public ChequeBook ChequeBookActiveInActive(int nCurrentUserID)
        {
            return ChequeBook.Service.ChequeBookActiveInActive(this, nCurrentUserID);
        }
        public string Delete(int id, int nCurrentUserID)
        {
            return ChequeBook.Service.Delete(id, nCurrentUserID);
        }
        public static List<ChequeBook> Gets(string sSQL, int nCurrentUserID)
        {
            return ChequeBook.Service.Gets(sSQL, nCurrentUserID);
        }
        public static List<ChequeBook> GetsByAccountNo(string sAccountNo, int nCurrentUserID)
        {
            return ChequeBook.Service.GetsByAccountNo(sAccountNo, nCurrentUserID);
        }
        public static List<ChequeBook> Gets(bool bIsActive, int nCurrentUserID)
        {
            return ChequeBook.Service.Gets(bIsActive, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IChequeBookService Service
        {
            get { return (IChequeBookService)Services.Factory.CreateService(typeof(IChequeBookService)); }
        }
        #endregion
    }
    #endregion

    #region IChequeBook interface
    
    public interface IChequeBookService
    {
        
        List<ChequeBook> Gets(bool bIsActive, int nCurrentUserID);
        
        List<ChequeBook> GetsByAccountNo(string sAccountNo, int nCurrentUserID);
        
        ChequeBook Get(int id, int nCurrentUserID);
        
        List<ChequeBook> Gets(int nCurrentUserID);
        
        List<ChequeBook> Gets(string sSQL, int nCurrentUserID);
        
        string Delete(int id, int nCurrentUserID);
        
        ChequeBook Save(ChequeBook oChequeBook, int nCurrentUserID);
        
        ChequeBook ChequeBookActiveInActive(ChequeBook oChequeBook, int nCurrentUserID);
    }
    #endregion
}