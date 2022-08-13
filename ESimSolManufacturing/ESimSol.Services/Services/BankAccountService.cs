using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class BankAccountService : MarshalByRefObject, IBankAccountService
    {
        #region Private functions and declaration
        private BankAccount MapObject(NullHandler oReader)
        {
            BankAccount oBankAccount = new BankAccount();
            oBankAccount.BankAccountID = oReader.GetInt32("BankAccountID");
            oBankAccount.AccountName = oReader.GetString("AccountName");
            oBankAccount.AccountNo = oReader.GetString("AccountNo");
            oBankAccount.BankID = oReader.GetInt32("BankID");
            oBankAccount.BankBranchID = oReader.GetInt32("BankBranchID");
            oBankAccount.AccountType = (EnumBankAccountType)oReader.GetInt32("AccountType");
            oBankAccount.AccountTypeInInt = oReader.GetInt32("AccountType");
            oBankAccount.LimitAmount = oReader.GetDouble("LimitAmount");
            oBankAccount.CurrentLimit = oReader.GetDouble("CurrentLimit");
            oBankAccount.BankAccountName = oReader.GetString("BankAccountName");
            oBankAccount.BankNameBranch = oReader.GetString("BankNameBranch");
            oBankAccount.BankName = oReader.GetString("BankName");
            oBankAccount.BranchName = oReader.GetString("BranchName");
            oBankAccount.BranchAddress = oReader.GetString("BranchAddress");
            oBankAccount.BankShortName = oReader.GetString("BankShortName");
            oBankAccount.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oBankAccount.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oBankAccount.BusinessUnitCode = oReader.GetString("BusinessUnitCode");
            oBankAccount.BusinessUnitNameCode = oReader.GetString("BusinessUnitNameCode");
            oBankAccount.BankNameAccountNo = oReader.GetString("BankNameAccountNo");
            
            return oBankAccount;
        }

        private BankAccount CreateObject(NullHandler oReader)
        {
            BankAccount oBankAccount = new BankAccount();
            oBankAccount = MapObject(oReader);
            return oBankAccount;
        }

        private List<BankAccount> CreateObjects(IDataReader oReader)
        {
            List<BankAccount> oBankAccount = new List<BankAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BankAccount oItem = CreateObject(oHandler);
                oBankAccount.Add(oItem);
            }
            return oBankAccount;
        }

        #endregion

        #region Interface implementation
        public BankAccountService() { }

        public BankAccount Save(BankAccount oBankAccount, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBankAccount.BankAccountID <= 0)
                {
                    reader = BankAccountDA.InsertUpdate(tc, oBankAccount, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = BankAccountDA.InsertUpdate(tc, oBankAccount, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBankAccount = new BankAccount();
                    oBankAccount = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BankAccount. Because of " + e.Message, e);
                #endregion
            }
            return oBankAccount;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BankAccount oBankAccount = new BankAccount();
                oBankAccount.BankAccountID = id;
                BankAccountDA.Delete(tc, oBankAccount, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BankAccount. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BankAccount Get(int id, Int64 nUserId)
        {
            BankAccount oAccountHead = new BankAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankAccountDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oAccountHead;
        }

        public BankAccount GetViaCostCenter(int nCCID, Int64 nUserId)
        {
            BankAccount oAccountHead = new BankAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankAccountDA.GetViaCostCenter(tc, nCCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oAccountHead;
        }

        public BankAccount GetPartyWiseDefultAccount(int nPartyID, long nUserID)
        {
            BankAccount oAccountHead = new BankAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BankAccountDA.GetPartyWiseDefultAccount(tc, nPartyID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BankAccount> GetsByBank(int nBankID, Int64 nUserId)
        {
            List<BankAccount> oBankAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankAccountDA.GetsByBank(tc, nBankID);
                oBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oBankAccount;
        }
        public List<BankAccount> GetsByBankBranch(int nBankBranchID, Int64 nUserID)
        {
            List<BankAccount> oBankAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankAccountDA.GetsByBankBranch(tc, nBankBranchID);
                oBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oBankAccount;
        }
        public List<BankAccount> GetsByDeptAndBU(string sDeptIDs, int BUID, Int64 nUserID)
        {
            List<BankAccount> oBankAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankAccountDA.GetsByDeptAndBU(tc,   sDeptIDs,  BUID);
                oBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oBankAccount;
        }
        public List<BankAccount> Gets(Int64 nUserId)
        {
            List<BankAccount> oBankAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankAccountDA.Gets(tc);
                oBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oBankAccount;
        }
        public List<BankAccount> Gets(string sSQL,Int64 nUserId)
        {
            List<BankAccount> oBankAccount = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankAccountDA.Gets(tc,sSQL);
                oBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                 throw new ServiceException("Failed to Get BankAccount", e);
                #endregion
            }

            return oBankAccount;
        }
        #endregion
    }   
}
