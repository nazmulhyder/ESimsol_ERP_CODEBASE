using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region BankReconcilationStatement
    public class BankReconcilationStatement : BusinessObject
    {
        public BankReconcilationStatement()
        {
            VoucherID = 0;
            OperationHeadName = "";
            DataType = 0;
            PaymentDate = DateTime.Now;
            RefAmount = 0;
            Narration = "";
            ChequeNo = "";
            PartyName = "";
            ErrorMessage = "";
        }

        #region Property
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public int BUID { get; set; }
        public string OperationHeadName { get; set; }
        public DateTime PaymentDate { get; set; }
        public int DataType { get; set; }
        public double RefAmount { get; set; }
        public string Narration { get; set; }
        public string ChequeNo { get; set; }
        public string PartyName { get; set; }
        public string ErrorMessage { get; set; }
        public string Note { get; set; }
        #endregion

        #region Derived Property
        public List<BankReconcilationStatement> BankReconciliationStatements { get; set; }
        public List<BankReconcilationStatement> BankReconciliationStatementsDataTypeZero { get; set; }
        public List<BankReconcilationStatement> BankReconciliationStatementsDataTypeOne { get; set; }
        public List<BankReconcilationStatement> BankReconciliationStatementsDataTypeTwo { get; set; }
        public List<BankReconcilationStatement> BankReconciliationStatementsDataTypeThree { get; set; }
        public Company Company { get; set; }
        public int SubLedgerID { get; set; }
        public int AccountHeadID { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime BalanceDate { get; set; }
        public string PaymentDateInString
        {
            get
            {
                if (PaymentDate.ToString("dd MMM yyyy") == "01 Jan 0001")
                {
                    return "";
                }
                return PaymentDate.ToString("dd MMM yyyy");
            }
        }
        public string BalanceDateInString
        {
            get
            {
                if (BalanceDate.ToString("dd MMM yyyy") == "01 Jan 0001")
                {
                    return "";
                }
                return BalanceDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public List<BankReconcilationStatement> GetBankReconcilationStatements(Int64 nUserID)
        {
            return BankReconcilationStatement.Service.GetBankReconcilationStatements(this, nUserID);
        }
        public static List<BankReconcilationStatement> Gets(long nUserID)
        {
            return BankReconcilationStatement.Service.Gets(nUserID);
        }
        public static List<BankReconcilationStatement> Gets(string sSQL, long nUserID)
        {
            return BankReconcilationStatement.Service.Gets(sSQL, nUserID);
        }
        public BankReconcilationStatement Get(int id, long nUserID)
        {
            return BankReconcilationStatement.Service.Get(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBankReconcilationStatementService Service
        {
            get { return (IBankReconcilationStatementService)Services.Factory.CreateService(typeof(IBankReconcilationStatementService)); }
        }
        #endregion
    }
    #endregion

    #region IBankReconcilationStatement interface
    public interface IBankReconcilationStatementService
    {
        BankReconcilationStatement Get(int id, Int64 nUserID);
        List<BankReconcilationStatement> Gets(Int64 nUserID);
        List<BankReconcilationStatement> Gets(string sSQL, Int64 nUserID);
        List<BankReconcilationStatement> GetBankReconcilationStatements(BankReconcilationStatement oBankReconcilationStatement, Int64 nUserID);


    }
    #endregion
}
