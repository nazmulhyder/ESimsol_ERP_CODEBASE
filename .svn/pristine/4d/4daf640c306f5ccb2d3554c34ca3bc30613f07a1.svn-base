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

    public class Account_BalanceService : MarshalByRefObject, IAccount_BalanceService
    {
       #region Private functions and declaration
        private Account_Balance MapObject(NullHandler oReader)
        {
            Account_Balance oAccount_Balance = new Account_Balance();
            oAccount_Balance.Account_Balance_ID = oReader.GetInt32("Account_Balance_ID");
            oAccount_Balance.Bank_ID = oReader.GetInt32("Bank_ID");
            oAccount_Balance.ERQAC = oReader.GetDouble("ERQAC");
            oAccount_Balance.FCAC = oReader.GetDouble("FCAC");
            oAccount_Balance.LTR = oReader.GetDouble("LTR");
            oAccount_Balance.PAD = oReader.GetDouble("PAD");
            oAccount_Balance.Saleable_Amount = oReader.GetDouble("Saleable_Amount");
            oAccount_Balance.Date = oReader.GetDateTime("Date");
            oAccount_Balance.BankName = oReader.GetString("BankName");
            oAccount_Balance.BankNickName = oReader.GetString("BankNickName");
            return oAccount_Balance;
        }

        private Account_Balance CreateObject(NullHandler oReader)
        {
            Account_Balance oAccount_Balance = new Account_Balance();
            oAccount_Balance = MapObject(oReader);
            return oAccount_Balance;
        }

        private List<Account_Balance> CreateObjects(IDataReader oReader)
        {
            List<Account_Balance> oAccount_Balance = new List<Account_Balance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Account_Balance oItem = CreateObject(oHandler);
                oAccount_Balance.Add(oItem);
            }
            return oAccount_Balance;
        }

        #endregion

        #region Interface implementation
        public Account_BalanceService() { }

        public Account_Balance Save(Account_Balance oAccount_Balance, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oAccount_Balance.Account_Balance_ID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Account_Balance, EnumRoleOperationType.Add);
                    oAccount_Balance.Account_Balance_ID = Account_BalanceDA.GetNewID(tc);
                    Account_BalanceDA.Insert(tc, oAccount_Balance);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Account_Balance, EnumRoleOperationType.Edit);
                    Account_BalanceDA.Update(tc, oAccount_Balance);
                }

                //IDataReader reader = Account_BalanceDA.Get(tc, oAccount_Balance.Account_Balance_ID);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oAccount_Balance = CreateObject(oReader);
                //}
                //reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAccount_Balance.ErrorMessage = e.Message;

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Account_Balance", e);
                #endregion
            }
            return oAccount_Balance;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Account_Balance, EnumRoleOperationType.Delete);
                Account_BalanceDA.Delete(tc, id);
                tc.End();                
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //throw new ServiceException(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
            
        }
        public bool UpdateField(string sDBField, object oValue, TypeCode oType, int nUserID)
        {
            //bool bIsExist;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                //bIsExist = Account_BalanceDA.IsExist(tc, sDBField, oValue, oType);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Account_Balance", e);
                #endregion
            }

            return true;
        }
        public List<Account_Balance> Gets(int nUserID)
        {
            List<Account_Balance> oAccount_Balances = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = Account_BalanceDA.Gets(tc);
                oAccount_Balances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Banks", e);
                #endregion
            }

            return oAccount_Balances;
        }
       
        #endregion
    }
   
}
