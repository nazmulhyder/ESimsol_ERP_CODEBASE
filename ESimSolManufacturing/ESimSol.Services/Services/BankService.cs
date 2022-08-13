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
    public class BankService : MarshalByRefObject, IBankService
    {
        #region Private functions and declaration
        private Bank MapObject(NullHandler oReader)
        {
            Bank oBank = new Bank();
            oBank.BankID = oReader.GetInt32("BankID");
            oBank.Code = oReader.GetString("Code");
            oBank.Name= oReader.GetString("Name");
            oBank.ShortName = oReader.GetString("ShortName");
            oBank.IsActive = oReader.GetBoolean("IsActive");
            oBank.FaxNo = oReader.GetString("FaxNo");
            oBank.ChequeSetupID = oReader.GetInt32("ChequeSetupID");
            oBank.ChequeSetupName = oReader.GetString("ChequeSetupName");
            return oBank;
        }

        private Bank CreateObject(NullHandler oReader)
        {
            Bank oBank = new Bank();
            oBank = MapObject(oReader);
            return oBank;
        }

        private List<Bank> CreateObjects(IDataReader oReader)
        {
            List<Bank> oBank = new List<Bank>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Bank oItem = CreateObject(oHandler);
                oBank.Add(oItem);
            }
            return oBank;
        }

        #endregion

        #region Interface implementation
        public BankService() { }
        public Bank Save(Bank oBank, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBank.BankID <= 0)
                {
                    reader = BankDA.InsertUpdate(tc, oBank, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BankDA.InsertUpdate(tc, oBank, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBank = new Bank();
                    oBank = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return oBank;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Bank oBank = new Bank();
                oBank.BankID = id;
                BankDA.Delete(tc, oBank, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public Bank Get(int id, Int64 nUserId)
        {
            Bank oBank = new Bank();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BankDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBank = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Bank", e);
                #endregion
            }
            return oBank;
        }
        public List<Bank> Gets(Int64 nUserID)
        {
            List<Bank> oBanks = new List<Bank>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankDA.Gets(tc);
                oBanks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Bank", e);
                #endregion
            }
            return oBanks;
        }
        public List<Bank> Gets(string sSQL,Int64 nUserID)
        {
            List<Bank> oBanks = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankDA.Gets(tc,sSQL);
                oBanks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Bank", e);
                #endregion
            }
            return oBanks;
        }
        public List<Bank> GetByNegoBank(int nBankID, Int64 nUserID)
        {
            List<Bank> oBanks = new List<Bank>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BankDA.GetByNegoBank(tc, nBankID);
                oBanks = CreateObjects(reader);
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
            return oBanks;
        }
        public List<Bank> GetByCategory(bool bCategory, Int64 nUserID)
        {
            List<Bank> oBanks = new List<Bank>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BankDA.GetByCategory(tc, bCategory);
                oBanks = CreateObjects(reader);
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
            return oBanks;
        }
        #endregion
    }   
}