using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{

    #region OperationCategorySetup
    public class OperationCategorySetup : BusinessObject
    {
        #region  Constructor
        public OperationCategorySetup()
        {
            OperationCategorySetupID = 0;
            StatementSetupID = 0;
            CategorySetupName = "";
            DebitCredit = EnumDebitCredit.None;
            Note = "";
            CounLedgerGroup = 0;            
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int OperationCategorySetupID { get; set; }
        public int StatementSetupID { get; set; }
        public string CategorySetupName { get; set; }
        public EnumDebitCredit DebitCredit { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; } 
        #endregion

        #region Derived Properties
        public List<LedgerGroupSetup> LedgerGroupSetups { get; set; }
        public int CounLedgerGroup { get; set; }
        public string DebitCreditInST
        {
            get
            {
                return this.DebitCredit.ToString();
            }
        }
        public bool IsDebit
        {
            get
            {
                if (this.DebitCredit == EnumDebitCredit.Debit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Functions
        public OperationCategorySetup Get(int id, int nUserID)
        {
            return OperationCategorySetup.Service.Get(id, nUserID);
        }
        public OperationCategorySetup Save(int nUserID)
        {
            return OperationCategorySetup.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return OperationCategorySetup.Service.Delete(id, nUserID);
        }
        public static List<OperationCategorySetup> Gets(int nStatementSetupID, int nUserID)
        {
            return OperationCategorySetup.Service.Gets(nStatementSetupID, nUserID);
        }
        #endregion

       
        #region ServiceFactory
        internal static IOperationCategorySetupService Service
        {
            get { return (IOperationCategorySetupService)Services.Factory.CreateService(typeof(IOperationCategorySetupService)); }
        }
        #endregion
    }
    #endregion


    #region IOperationCategorySetup interface
    public interface IOperationCategorySetupService
    {
        OperationCategorySetup Get(int nID, int nUserID);
        List<OperationCategorySetup> Gets(int nStatementSetupID, int nUserID);
        string Delete(int id, int nUserID);
        OperationCategorySetup Save(OperationCategorySetup oOperationCategorySetup, int nUserID);
    }
    #endregion
   
}
