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

    public class ImportInvoiceHistoryService : MarshalByRefObject, IImportInvoiceHistoryService
    {
        #region Private functions and declaration
        private ImportInvoiceHistory MapObject(NullHandler oReader)
        {
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();
            oImportInvoiceHistory.ImportInvoiceHistoryID = oReader.GetInt32("ImportInvoiceHistoryID");
            oImportInvoiceHistory.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportInvoiceHistory.InvoiceEvent = (EnumInvoiceEvent)oReader.GetInt16("InvoiceEvent");
            oImportInvoiceHistory.InvoiceEventInt = oReader.GetInt16("InvoiceEvent");
            oImportInvoiceHistory.InvoiceBankStatus = (EnumInvoiceBankStatus)oReader.GetInt16("InvoiceBankStatus");
            oImportInvoiceHistory.InvoiceBankStatusInt = oReader.GetInt16("InvoiceBankStatus");
            oImportInvoiceHistory.Note = oReader.GetString("Note");
            oImportInvoiceHistory.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oImportInvoiceHistory.DBUserID = oReader.GetInt32("DBUserID");
            //  oImportInvoiceHistory.EmployeeName = oReader.GetString("EmployeeName");       
            return oImportInvoiceHistory;
        }

        private ImportInvoiceHistory CreateObject(NullHandler oReader)
        {
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();
            oImportInvoiceHistory = MapObject(oReader);
            return oImportInvoiceHistory;
        }

        private List<ImportInvoiceHistory> CreateObjects(IDataReader oReader)
        {
            List<ImportInvoiceHistory> oImportInvoiceHistorys = new List<ImportInvoiceHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportInvoiceHistory oItem = CreateObject(oHandler);
                oImportInvoiceHistorys.Add(oItem);
            }
            return oImportInvoiceHistorys;
        }
        #endregion

        #region Interface implementation
        public ImportInvoiceHistoryService() { }
        public ImportInvoiceHistory Save(ImportInvoiceHistory oImportInvoiceHistory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oImportInvoiceHistory.ImportInvoiceHistoryID <= 0)
                {

                    oImportInvoiceHistory.ImportInvoiceHistoryID = ImportInvoiceHistoryDA.GetNewID(tc);
                    ImportInvoiceHistoryDA.Insert(tc, oImportInvoiceHistory, nUserId);
                    // PurchaseInvoiceDA.UpdateBillState(tc, (int)oImportInvoiceHistory.BillEvent, oImportInvoiceHistory.PurchaseInvoiceID);

                }
                else
                {
                    ImportInvoiceHistoryDA.Update(tc, oImportInvoiceHistory, nUserId);
                    //   PurchaseInvoiceDA.UpdateBillState(tc, (int)oImportInvoiceHistory.BillEvent, oImportInvoiceHistory.PurchaseInvoiceID);

                }
                IDataReader reader = ImportInvoiceHistoryDA.Get(tc, oImportInvoiceHistory.ImportInvoiceHistoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ImportInvoiceHistory", e);
                #endregion
            }
            return oImportInvoiceHistory;
        }
        public ImportInvoiceHistory Save_BOEinCustomerHand(ImportInvoiceHistory oImportInvoiceHistory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oImportInvoiceHistory.ImportInvoiceHistoryID <= 0)
                {

                    oImportInvoiceHistory.ImportInvoiceHistoryID = ImportInvoiceHistoryDA.GetNewID(tc);
                    ImportInvoiceHistoryDA.Insert(tc, oImportInvoiceHistory, nUserId);
                    // PurchaseInvoiceDA.UpdateBillState(tc, (int)oImportInvoiceHistory.BillEvent, oImportInvoiceHistory.PurchaseInvoiceID);
                    //   PurchaseInvoiceDA.UpdateExportLCState(tc, (int)EnumExportLCStatus.OutstandingLC, oImportInvoiceHistory.PurchaseInvoiceID);
                }
                else
                {
                    ImportInvoiceHistoryDA.Update(tc, oImportInvoiceHistory, nUserId);
                    //   PurchaseInvoiceDA.UpdateBillState(tc, (int)oImportInvoiceHistory.BillEvent, oImportInvoiceHistory.PurchaseInvoiceID);
                    // PurchaseInvoiceDA.UpdateExportLCState(tc, (int)EnumExportLCStatus.OutstandingLC, oImportInvoiceHistory.PurchaseInvoiceID);
                }
                IDataReader reader = ImportInvoiceHistoryDA.Get(tc, oImportInvoiceHistory.ImportInvoiceHistoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ImportInvoiceHistory", e);
                #endregion
            }
            return oImportInvoiceHistory;
        }

     
        public ImportInvoiceHistory Get(int id, Int64 nUserId)
        {
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvoiceHistory", e);
                #endregion
            }

            return oImportInvoiceHistory;
        }
        public ImportInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nInvID, Int64 nUserId)
        {
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceHistoryDA.Get(tc, nInvoiceStatus, nBankStatus, nInvID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvoiceHistory", e);
                #endregion
            }

            return oImportInvoiceHistory;
        }
        public ImportInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, Int64 nUserId)
        {
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportInvoiceHistoryDA.GetbyPurchaseInvoice(tc, nPurchaseInvoiceID, eEvent);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportInvoiceHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportInvoiceHistory", e);
                #endregion
            }

            return oImportInvoiceHistory;
        }
        public List<ImportInvoiceHistory> Gets(Int64 nUserId)
        {
            List<ImportInvoiceHistory> oImportInvoiceHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceHistoryDA.Gets(tc);
                oImportInvoiceHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoiceHistorys", e);
                #endregion
            }

            return oImportInvoiceHistorys;
        }
        public List<ImportInvoiceHistory> Gets(int nImportInvoiceID, Int64 nUserId)
        {
            List<ImportInvoiceHistory> oImportInvoiceHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceHistoryDA.Gets(tc, nImportInvoiceID);
                oImportInvoiceHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoiceHistorys", e);
                #endregion
            }

            return oImportInvoiceHistorys;
        }
        public List<ImportInvoiceHistory> GetsByInvoiceEvent(int nImportInvoiceID, string sInvoiceEvent, Int64 nUserId)
        {
            List<ImportInvoiceHistory> oImportInvoiceHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportInvoiceHistoryDA.GetsByInvoiceEvent(tc, nImportInvoiceID, sInvoiceEvent);
                oImportInvoiceHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportInvoiceHistorys", e);
                #endregion
            }

            return oImportInvoiceHistorys;
        }
        #endregion
    }
}
