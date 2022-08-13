using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class InventoryTransactionService : MarshalByRefObject, IInventoryTransactionService
    {
        bool _bActive = false;
        #region Private functions and declaration
        private InventoryTransaction MapObject(NullHandler oReader)
        {
            InventoryTransaction oInventoryTransaction=new InventoryTransaction ();
            oInventoryTransaction.ITransactionID = oReader.GetInt32("ITransactionID");
            oInventoryTransaction.DateTime = oReader.GetDateTime("DateTime");
            oInventoryTransaction.ProductID = oReader.GetInt32("ProductID");
            oInventoryTransaction.Qty = oReader.GetDouble("Quantity");
            oInventoryTransaction.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oInventoryTransaction.ClousingBalance = oReader.GetDouble("ClousingBalance");
            oInventoryTransaction.UnitPrice = oReader.GetDouble("UnitPrice");
            oInventoryTransaction.UserID = oReader.GetInt32("DBUserID");
            oInventoryTransaction.TriggerParentID = oReader.GetInt32("TriggerParentID");
            oInventoryTransaction.TriggerParentType = oReader.GetInt16("TriggerParentType");
            oInventoryTransaction.LotID = oReader.GetInt32("LotID");
            oInventoryTransaction.InOutType = oReader.GetInt16("InOutType");
            oInventoryTransaction.Description = oReader.GetString("Description");
            oInventoryTransaction.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oInventoryTransaction.LocationName = oReader.GetString("LocationName");
            oInventoryTransaction.OperationUnitName = oReader.GetString("OperationUnitName");
            oInventoryTransaction.ProductName = oReader.GetString("ProductName");
            oInventoryTransaction.ProductCode = oReader.GetString("ProductCode");
            oInventoryTransaction.LotNo = oReader.GetString("LotNo");
            oInventoryTransaction.MUnitID = oReader.GetInt32("MeasurementUnitID");
            oInventoryTransaction.CurrencyID = oReader.GetInt32("CurrencyID");
          
            return oInventoryTransaction;
        }
        private InventoryTransaction CreateObject(NullHandler oReader)
        {
            InventoryTransaction oInventoryTransaction = new InventoryTransaction();
            oInventoryTransaction=MapObject(oReader);
            return oInventoryTransaction;
        }
        private List<InventoryTransaction> CreateObjects(IDataReader oReader)
        {
            List<InventoryTransaction> oInventoryTransactions = new List<InventoryTransaction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InventoryTransaction oItem = CreateObject(oHandler);
                oInventoryTransactions.Add(oItem);
            }
            return oInventoryTransactions;
        }
        #endregion

        #region Interface implementation
        public InventoryTransactionService() { }

        #region Execute Non-Query

        #endregion

        #region ExecuteQuery
        
        public List<InventoryTransaction> Gets(int nTriggerParentID, int eEnumTriggerParentsType, int eInOutType, int nProductID, string sStoreIDs, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTransactionDA.Gets(tc,  nTriggerParentID,  eEnumTriggerParentsType,  eInOutType,  nProductID,  sStoreIDs);
                oInventoryTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }
        public List<InventoryTransaction> GetsByInvoice(int nTriggerParentID, int eTriggerParentType, int nProductID, int nInoutType, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTransactionDA.GetsByInvoice(tc,  nTriggerParentID,  eTriggerParentType, nProductID,  nInoutType);
                oInventoryTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }
        public List<InventoryTransaction> GetsBY(int nEnumTriggerParentsType, int eInOutType, DateTime dTranDate, DateTime dTranDateTo, string sWorkingUnitIDs, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                string sInOutType = Convert.ToString((int)eInOutType);              
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTransactionDA.Gets(tc,  nEnumTriggerParentsType,  eInOutType,  dTranDate,  dTranDateTo, sWorkingUnitIDs);
                oInventoryTransactions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }
        public List<InventoryTransaction> Gets(string sSQL, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                _bActive = true;
                reader = InventoryTransactionDA.Gets(tc,  sSQL);
                oInventoryTransactions = CreateObjects(reader);

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }
        public List<InventoryTransaction> GetsByLotID(int nProductID, string sLotIDs, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                _bActive = true;
                reader = InventoryTransactionDA.GetsByLotID(tc, nProductID, sLotIDs);
                oInventoryTransactions = CreateObjects(reader);

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }

        public List<InventoryTransaction> GetsStockByLotID(int nLotID, Int64 nUserID)
        {
            List<InventoryTransaction> oInventoryTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                _bActive = true;
                reader = InventoryTransactionDA.GetsStockByLotID(tc, nLotID);
                oInventoryTransactions = CreateObjects(reader);

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Inventory Transactions", e);
                #endregion
            }
            return oInventoryTransactions;
        }
        #endregion
        #endregion
    }
}