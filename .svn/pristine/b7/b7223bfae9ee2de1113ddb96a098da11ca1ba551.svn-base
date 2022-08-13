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
    public class ITransactionService : MarshalByRefObject, IITransactionService
    {        
        #region Private functions and declaration
        private ITransaction MapObject(NullHandler oReader)
        {
            ITransaction oITransaction = new ITransaction();
            oITransaction.ITransactionID = oReader.GetInt32("ITransactionID");
            oITransaction.ProductID = oReader.GetInt32("ProductID");
            oITransaction.LotID = oReader.GetInt32("LotID");
            oITransaction.Qty = oReader.GetDouble("Qty");
            oITransaction.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oITransaction.MUnitID = oReader.GetInt32("MUnitID");
            oITransaction.UnitPrice = oReader.GetDouble("UnitPrice");
            oITransaction.CurrencyID = oReader.GetInt32("CurrencyID");
            oITransaction.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oITransaction.Description = oReader.GetString("Description");
            oITransaction.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oITransaction.TriggerParentID = oReader.GetInt32("TriggerParentID");
            oITransaction.TriggerParentType = (EnumTriggerParentsType)oReader.GetInt32("TriggerParentType"); ;
            oITransaction.TransactionTime = oReader.GetDateTime("TransactionTime");
            oITransaction.DBUserID = oReader.GetInt32("DBUserID");
            oITransaction.ProductCode = oReader.GetString("ProductCode");
            oITransaction.ProductName = oReader.GetString("ProductName");
            oITransaction.LotNo = oReader.GetString("LotNo");
            oITransaction.UnitName = oReader.GetString("UnitName");
            oITransaction.USymbol = oReader.GetString("USymbol");
            oITransaction.StoreName = oReader.GetString("StoreName");
            oITransaction.RefNo = oReader.GetString("RefNo");
            oITransaction.UserName = oReader.GetString("UserName");
            oITransaction.AlreadyReturnQty = oReader.GetDouble("AlreadyReturnQty");//don't Delete It used in Raw Material Return Module (call from controller)
            return oITransaction;
        }

        private ITransaction CreateObject(NullHandler oReader)
        {
            ITransaction oITransaction = new ITransaction();
            oITransaction = MapObject(oReader);
            return oITransaction;
        }

        private List<ITransaction> CreateObjects(IDataReader oReader)
        {
            List<ITransaction> oITransactions = new List<ITransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITransaction oItem = CreateObject(oHandler);
                oITransactions.Add(oItem);
            }
            return oITransactions;
        }
        #endregion

        #region Interface implementation
        public ITransactionService() { }

        public ITransaction Get(int nITransactionID, int nUserID)
        {
            ITransaction oITransaction = new ITransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITransactionDA.Get(tc, nITransactionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITransaction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get ITransaction \n" + e.Message, e);
                #endregion
            }
            return oITransaction;
        }



        public ITransaction UpdateTransaction(ITransaction oITransaction, int nUserID)
        {
            ITransaction _oITransaction = new ITransaction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ITransactionDA.UpdateTransaction(tc, oITransaction, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    _oITransaction = new ITransaction();
                    _oITransaction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                _oITransaction = new ITransaction();
                _oITransaction.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return _oITransaction;
        }



        public List<ITransaction> Gets(int nUserID)
        {
            List<ITransaction> oITransactions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITransactionDA.Gets(tc);
                oITransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ITransaction \n" + e.Message, e);
                #endregion
            }

            return oITransactions;
        }
        public List<ITransaction> Gets(string sSQL, int nUserID)
        {
            List<ITransaction> oITransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITransactionDA.Gets(tc, sSQL);
                oITransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ITransactions \n" + e.Message, e);
                #endregion
            }
            return oITransactions;
        }
        #endregion
    }
}
