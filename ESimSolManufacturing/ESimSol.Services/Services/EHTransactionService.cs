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
    public class EHTransactionService : MarshalByRefObject, IEHTransactionService
    {
        #region Private functions and declaration
        private EHTransaction MapObject(NullHandler oReader)
        {
            EHTransaction oEHTransaction = new EHTransaction();
            oEHTransaction.EHTransactionID = oReader.GetInt32("EHTransactionID");
            oEHTransaction.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oEHTransaction.ExpenditureType = (EnumExpenditureType)oReader.GetInt32("ExpenditureType");
            oEHTransaction.ExpenditureTypeInt = oReader.GetInt32("ExpenditureType");
            oEHTransaction.RefObjectID = oReader.GetInt32("RefObjectID");
            oEHTransaction.CurrencyID = oReader.GetInt32("CurrencyID");
            oEHTransaction.Amount = oReader.GetDouble("Amount");
            oEHTransaction.CCRate = oReader.GetDouble("CCRate");
            oEHTransaction.AmountBC = oReader.GetDouble("AmountBC");
            oEHTransaction.Remarks = oReader.GetString("Remarks");
            oEHTransaction.AccountHeadName = oReader.GetString("AccountHeadName");
            oEHTransaction.CSymbol = oReader.GetString("CSymbol");
            return oEHTransaction;
        }

        private EHTransaction CreateObject(NullHandler oReader)
        {
            EHTransaction oEHTransaction = MapObject(oReader);
            return oEHTransaction;
        }

        private List<EHTransaction> CreateObjects(IDataReader oReader)
        {
            List<EHTransaction> oEHTransaction = new List<EHTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EHTransaction oItem = CreateObject(oHandler);
                oEHTransaction.Add(oItem);
            }
            return oEHTransaction;
        }

        #endregion

        #region Interface implementation
        public EHTransactionService() { }

        public EHTransaction Save(EHTransaction oEHT, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEHT.EHTransactionID <= 0)
                {
                    reader = EHTransactionDA.InsertUpdate(tc, oEHT, (int)EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = EHTransactionDA.InsertUpdate(tc, oEHT, (int)EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEHT = new EHTransaction();
                    oEHT = CreateObject(oReader);
                }                                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEHT.ErrorMessage = e.Message;
                oEHT.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oEHT;
        }
        public string Delete(EHTransaction oEHT, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EHTransactionDA.Delete(tc, oEHT, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public EHTransaction Get(int nEHTID, Int64 nUserId)
        {
            EHTransaction oEHTransaction = new EHTransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EHTransactionDA.Get(nEHTID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEHTransaction = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EHTransaction", e);
                oEHTransaction.ErrorMessage = e.Message;
                #endregion
            }

            return oEHTransaction;
        }
        public List<EHTransaction> Gets(Int64 nUserID)
        {
            List<EHTransaction> oEHTransaction = new List<EHTransaction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EHTransactionDA.Gets(tc);
                oEHTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EHTransaction", e);
                #endregion
            }
            return oEHTransaction;
        }

        public List<EHTransaction> Gets(int nRefObjectID, EnumExpenditureType eExpenditureType, Int64 nUserID)
        {
            List<EHTransaction> oEHTransaction = new List<EHTransaction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EHTransactionDA.Gets(tc, nRefObjectID, eExpenditureType);
                oEHTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EHTransaction", e);
                #endregion
            }
            return oEHTransaction;
        }
        public List<EHTransaction> Gets(string sSQL, Int64 nUserID)
        {
            List<EHTransaction> oEHTransaction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EHTransactionDA.Gets(sSQL, tc);
                oEHTransaction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EHTransaction", e);
                #endregion
            }
            return oEHTransaction;
        }
        #endregion
    }
}
