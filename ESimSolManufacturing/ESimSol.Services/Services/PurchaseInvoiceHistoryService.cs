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
    public class PurchaseInvoiceHistoryService : MarshalByRefObject, IPurchaseInvoiceHistoryService
    {
        #region Private functions and declaration
        private PurchaseInvoiceHistory MapObject(NullHandler oReader)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();
            oPurchaseInvoiceHistory.PurchaseInvoiceHistoryID = oReader.GetInt32("PurchaseInvoiceHistoryID");
            oPurchaseInvoiceHistory.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oPurchaseInvoiceHistory.CurrentStatus = (EnumPurchaseInvoiceEvent)oReader.GetInt16("CurrentStatus");
            oPurchaseInvoiceHistory.CurrentStatusInt = oReader.GetInt16("CurrentStatus");
            oPurchaseInvoiceHistory.PrevoiusStatus = (EnumPurchaseInvoiceEvent)oReader.GetInt16("PrevoiusStatus");
            oPurchaseInvoiceHistory.PrevoiusStatusInt = oReader.GetInt16("PrevoiusStatus");
            oPurchaseInvoiceHistory.Note = oReader.GetString("Note");
            oPurchaseInvoiceHistory.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oPurchaseInvoiceHistory.DBUserID = oReader.GetInt32("DBUserID");          
            return oPurchaseInvoiceHistory;
        }

        private PurchaseInvoiceHistory CreateObject(NullHandler oReader)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();
            oPurchaseInvoiceHistory=MapObject(oReader);
            return oPurchaseInvoiceHistory;
        }

        private List<PurchaseInvoiceHistory> CreateObjects(IDataReader oReader)
        {
            List<PurchaseInvoiceHistory> oPurchaseInvoiceHistorys = new List<PurchaseInvoiceHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseInvoiceHistory oItem = CreateObject(oHandler);
                oPurchaseInvoiceHistorys.Add(oItem);
            }
            return oPurchaseInvoiceHistorys;
        }
        #endregion

        #region Interface implementation
        public PurchaseInvoiceHistoryService() { }
        public PurchaseInvoiceHistory Save(PurchaseInvoiceHistory oPurchaseInvoiceHistory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oPurchaseInvoiceHistory.PurchaseInvoiceHistoryID <= 0)
                {
                    PurchaseInvoiceHistoryDA.Insert(tc, oPurchaseInvoiceHistory, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    PurchaseInvoiceHistoryDA.Insert(tc, oPurchaseInvoiceHistory, EnumDBOperation.Update, nUserId);                 

                }
                IDataReader reader = PurchaseInvoiceHistoryDA.Get(tc, oPurchaseInvoiceHistory.PurchaseInvoiceHistoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PurchaseInvoiceHistory", e);
                #endregion
            }
            return oPurchaseInvoiceHistory;
        }
    
      
        public string Delete(PurchaseInvoiceHistory oPurchaseInvoiceHistory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ///checked validation
              //  PurchaseInvoiceDA.UpdateBillState_Discount(tc, (int)oPurchaseInvoiceHistory.BillEvent, 0, oPurchaseInvoiceHistory.PurchaseInvoiceID);
                PurchaseInvoiceHistoryDA.Delete(tc, oPurchaseInvoiceHistory);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return "Deletion not possible" + e.Message;
                #endregion
            }

        }

        public PurchaseInvoiceHistory Get(int id, Int64 nUserId)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseInvoiceHistory", e);
                #endregion
            }

            return oPurchaseInvoiceHistory;
        }
        public PurchaseInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nInvID, Int64 nUserId)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceHistoryDA.Get(tc,  nInvoiceStatus,  nBankStatus,  nInvID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseInvoiceHistory", e);
                #endregion
            }

            return oPurchaseInvoiceHistory;
        }
        public PurchaseInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, Int64 nUserId)
        {
            PurchaseInvoiceHistory oPurchaseInvoiceHistory = new PurchaseInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseInvoiceHistoryDA.GetbyPurchaseInvoice(tc, nPurchaseInvoiceID, eEvent);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseInvoiceHistory", e);
                #endregion
            }

            return oPurchaseInvoiceHistory;
        }
        public List<PurchaseInvoiceHistory> Gets(Int64 nUserId)
        {
            List<PurchaseInvoiceHistory> oPurchaseInvoiceHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceHistoryDA.Gets(tc);
                oPurchaseInvoiceHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoiceHistorys", e);
                #endregion
            }

            return oPurchaseInvoiceHistorys;
        }
        public List<PurchaseInvoiceHistory> Gets(int nPurchaseInvoiceID, Int64 nUserId)
        {
            List<PurchaseInvoiceHistory> oPurchaseInvoiceHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseInvoiceHistoryDA.Gets(tc,nPurchaseInvoiceID);
                oPurchaseInvoiceHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseInvoiceHistorys", e);
                #endregion
            }

            return oPurchaseInvoiceHistorys;
        } 
        #endregion
    }
}
