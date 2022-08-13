using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core;
using ICS.Core.Utility;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region Cheque
    
    public class Cheque : BusinessObject
    {
        public Cheque()
        {
            ChequeID = 0;
            ChequeBookID = 0;
            ChequeStatus = EnumChequeStatus.Initiate;
            PaymentType = EnumPaymentType.None;
            ChequeNo = "";
            ChequeDate = DateTime.MinValue;
            PayTo = 0;
            IssueFigureID = 0;
            Amount = 0;
            VoucherReference = "";
            Note = "";
            RegisterPrint = false;
            BankAccountID = 0;
            BankBranchID = 0;
            BankID = 0;
            BusinessUnitID = 0;
            BookCodePartOne = "";
            BookCodePartTwo = "";
            ChequeSetupID = 0;
            AccountNo = "";
            BankName = "";
            BankShortName = "";
            BankBranchName = "";
            BusinessUnitName = "";
            Selected = false;
            ContractorName = "";
            ChequeIssueTo = "";
            SecondLineIssueTo = "";
            OperationBy = 0;
            OperationDateTime = DateTime.MinValue;
            OperationByName = "";
            CommitType = "";
            ContractorPhone = "";
            ContractorAddress = "";
            SerialNo = 0;
            Cheques = new List<Cheque>();
            ReceivedCheques = new List<ReceivedCheque>();
            Setup = EnumVoucherSetup.None;
            ErrorMessage = "";
        }

        #region Properties        
        public int ChequeID { get; set; }        
        public int ChequeBookID { get; set; }        
        public EnumChequeStatus ChequeStatus { get; set; }        
        public EnumPaymentType PaymentType { get; set; }        
        public string ChequeNo { get; set; }        
        public DateTime ChequeDate { get; set; }        
        public int PayTo { get; set; }        
        public int IssueFigureID { get; set; }        
        public double Amount { get; set; }        
        public string VoucherReference { get; set; }        
        public string Note { get; set; }
        public bool RegisterPrint { get; set; }
        public int BankAccountID { get; set; }        
        public int BankBranchID { get; set; }        
        public int BankID { get; set; }        
        public int BusinessUnitID { get; set; }        
        public string BookCodePartOne { get; set; }        
        public string BookCodePartTwo { get; set; }
        public int ChequeSetupID { get; set; }        
        public string AccountNo { get; set; }        
        public string BankName { get; set; }        
        public string BankShortName { get; set; }        
        public string BankBranchName { get; set; }        
        public string BusinessUnitName { get; set; }        
        public bool Selected { get; set; }        
        public string ContractorName { get; set; }        
        public string ChequeIssueTo { get; set; }        
        public string SecondLineIssueTo { get; set; }
        public string ContractorPhone { get; set; }        
        public string ContractorAddress { get; set; }        
        public int OperationBy { get; set; }        
        public DateTime OperationDateTime { get; set; }        
        public string OperationByName { get; set; }        
        public string CommitType { get; set; }        
        public int SerialNo { get; set; }
        public EnumVoucherSetup Setup { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public List<Voucher> Vouchers { get; set; }
        public string BookCode { get { return this.BookCodePartOne + "-" + this.BookCodePartTwo; } }
        public string ChequeDateInString { get { return (this.ChequeDate == DateTime.MinValue) ? "" : this.ChequeDate.ToString("dd MMM yyyy"); } }
        public string ChequeStatusInString { get { return EnumObject.jGet(this.ChequeStatus); } }
        public int ChequeStatusInInt { get { return (int)this.ChequeStatus; } }
        public string PaymentTypeInString { get { return EnumObject.jGet(this.PaymentType); } }
        public string AmountInString { get { return (this.Amount == 0) ? "-" : Global.MillionFormat(this.Amount); } }
        public string AmountInWord { get { return Global.TakaWords(this.Amount); } }
        public string OperationDateTimeInString { get { return (this.OperationDateTime == DateTime.MinValue) ? "" : this.OperationDateTime.ToString("dd MMM yyyy hh:mm tt"); } }

        public List<EnumObject> PaymentTypes { get; set; }
        public List<EnumObject> ChequeStatuses { get; set; }
        public List<Cheque> Cheques { get; set; }
        public Contractor Contractor { get; set; }
        public List<ReceivedCheque> ReceivedCheques { get; set; }
        public string RegisterPrintSt
        {
            get
            {
                if (this.RegisterPrint)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions
        public static List<Cheque> Gets(int nCurrentUserID)
        {
            return Cheque.Service.Gets(nCurrentUserID);
        }
        public Cheque Get(int id, int nCurrentUserID)
        {
            return Cheque.Service.Get(id, nCurrentUserID);
        }
        public Cheque Save(int nCurrentUserID)
        {
            return Cheque.Service.Save(this, nCurrentUserID);
        }
        public DataSet ChequeTacker(string sSQL, int nCurrentUserID)
        {
            return Cheque.Service.ChequeTacker(sSQL, nCurrentUserID);
        }
        public Cheque UpdateChequeStatus(ChequeHistory oChequeHistory, int nCurrentUserID)
        {
            return Cheque.Service.UpdateChequeStatus(this, oChequeHistory, nCurrentUserID);
        }
        public static List<Cheque> UpdateChequeStatus(List<ChequeHistory> oChequeHistorys, int nCurrentUserID)
        {
            return Cheque.Service.UpdateChequeStatus(oChequeHistorys, nCurrentUserID);
        }
        public string Delete(int id, int nCurrentUserID)
        {
            return Cheque.Service.Delete(id, nCurrentUserID);
        }
        public static List<Cheque> Gets(string sSQL, int nCurrentUserID)
        {
            return Cheque.Service.Gets(sSQL, nCurrentUserID);
        }
        public static List<Cheque> Gets(int nChequeBookID, int nCurrentUserID)
        {
            return Cheque.Service.Gets(nChequeBookID, nCurrentUserID);
        }
        public static List<Cheque> Gets(int nChequeBookID, EnumChequeStatus eSealed, int nCurrentUserID)
        {
            return Cheque.Service.Gets(nChequeBookID, (int)eSealed, nCurrentUserID);
        }
        public static List<Cheque> GetsByChequeNo(string sChequeNo, int nCurrentUserID)
        {
            return Cheque.Service.GetsByChequeNo(sChequeNo, nCurrentUserID);
        }
        public static string ConfirmRegisterPrint(List<Cheque> oCheques, int nCurrentUserID)
        {
            return Cheque.Service.ConfirmRegisterPrint(oCheques, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IChequeService Service
        {
            get { return (IChequeService)Services.Factory.CreateService(typeof(IChequeService)); }
        }
        #endregion
    }
    #endregion

    #region ICheque interface
    
    public interface IChequeService
    {   
        List<Cheque> GetsByChequeNo(string sChequeNo, int nCurrentUserID);        
        Cheque Get(int id, int nCurrentUserID);        
        List<Cheque> Gets(int nCurrentUserID);        
        List<Cheque> Gets(string sSQL, int nCurrentUserID);        
        List<Cheque> Gets(int nChequeBookID, int nCurrentUserID);        
        List<Cheque> Gets(int nChequeBookID, int eSealed, int nCurrentUserID);        
        string Delete(int id, int nCurrentUserID);        
        Cheque Save(Cheque oCheque, int nCurrentUserID);        
        DataSet ChequeTacker(string sSQL, int nCurrentUserID);        
        Cheque UpdateChequeStatus(Cheque oCheque, ChequeHistory oChequeHistory, int nCurrentUserID);        
        List<Cheque> UpdateChequeStatus(List<ChequeHistory> oChequeHistorys, int nCurrentUserID);
        string ConfirmRegisterPrint(List<Cheque> oCheques, int nCurrentUserID);
    }
    #endregion
}