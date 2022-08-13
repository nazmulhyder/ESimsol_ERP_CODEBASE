using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class EmployeeBankAccountService : MarshalByRefObject, IEmployeeBankAccountService
    {
        #region Private functions and declaration
        private EmployeeBankAccount MapObject(NullHandler oReader)
        {
            EmployeeBankAccount oEBA = new EmployeeBankAccount();
            oEBA.EmployeeBankACID = oReader.GetInt32("EmployeeBankACID");
            oEBA.EmployeeID = oReader.GetInt32("EmployeeID");
            oEBA.BankBranchID = oReader.GetInt32("BankBranchID");
            oEBA.AccountName = oReader.GetString("AccountName");
            oEBA.BankName = oReader.GetString("BankName");
            oEBA.AccountNo = oReader.GetString("AccountNo");
            oEBA.AccountType = (EnumBankAccountType)oReader.GetInt16("AccountType");
            oEBA.Description = oReader.GetString("Description");
            oEBA.IsActive = oReader.GetBoolean("IsActive");
            oEBA.BranchName = oReader.GetString("BankAddress");
            //Derive
            oEBA.BankBranchName = oReader.GetString("BankBranchName");
            return oEBA;
        }

        private EmployeeBankAccount CreateObject(NullHandler oReader)
        {
            EmployeeBankAccount oEmployeeBankAccount = new EmployeeBankAccount();
            oEmployeeBankAccount = MapObject(oReader);
            return oEmployeeBankAccount;
        }

        private List<EmployeeBankAccount> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBankAccount> oEmployeeBankAccount = new List<EmployeeBankAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBankAccount oItem = CreateObject(oHandler);
                oEmployeeBankAccount.Add(oItem);
            }
            return oEmployeeBankAccount;
        }

        #endregion

        #region Interface implementation
        public EmployeeBankAccountService() { }

        public EmployeeBankAccount IUD(EmployeeBankAccount oEBA, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeBankAccountDA.IUD(tc, oEBA, nDBOperation, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEBA = new EmployeeBankAccount();
                    oEBA = CreateObject(oReader);
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
                oEBA.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeBankAccount. Because of " + e.Message, e);
                #endregion
            }
            return oEBA;
        }


        public EmployeeBankAccount Get(int id, Int64 nUserId) //EmployeeBankACID
        {
            EmployeeBankAccount oEBA = new EmployeeBankAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBankAccountDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEBA = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeBankAccount", e);
                #endregion
            }

            return oEBA;
        }

        public List<EmployeeBankAccount> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeBankAccount> oEmployeeBankAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeBankAccountDA.Gets(tc, nEmployeeID);
                oEmployeeBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeBankAccount", e);
                #endregion
            }

            return oEmployeeBankAccount;
        }


        public List<EmployeeBankAccount> Gets(string  sSql, Int64 nUserID)
        {
            List<EmployeeBankAccount> oEmployeeBankAccount = new List<EmployeeBankAccount>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeBankAccountDA.Gets(tc, sSql);
                oEmployeeBankAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeBankAccount", e);
                #endregion
            }

            return oEmployeeBankAccount;
        }

        #endregion
    }
}