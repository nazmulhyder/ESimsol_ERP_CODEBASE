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
    public class BatchProcessPlannedDateService : MarshalByRefObject, IBatchProcessPlannedDateService
    {
        #region Private functions and declaration
        private BatchProcessPlannedDate MapObject(NullHandler oReader)
        {
            BatchProcessPlannedDate oBatchProcessPlannedDate = new BatchProcessPlannedDate();
            oBatchProcessPlannedDate.BatchProcessPlannedDateID = oReader.GetInt32("BatchProcessPlannedDateID");
            oBatchProcessPlannedDate.FNBatchCardID = oReader.GetInt32("FNBatchCardID");
            oBatchProcessPlannedDate.FNTreatmentProcessID = oReader.GetInt32("FNTreatmentProcessID");
            oBatchProcessPlannedDate.FNBatchID = oReader.GetInt32("FNBatchID");
            oBatchProcessPlannedDate.PlannedDate = oReader.GetDateTime("PlannedDate");
            oBatchProcessPlannedDate.FNProcess = oReader.GetString("FNProcess");
            oBatchProcessPlannedDate.Code = oReader.GetString("Code");
            return oBatchProcessPlannedDate;
        }
        private BatchProcessPlannedDate CreateObject(NullHandler oReader)
        {
            BatchProcessPlannedDate oBatchProcessPlannedDate = new BatchProcessPlannedDate();
            oBatchProcessPlannedDate = MapObject(oReader);
            return oBatchProcessPlannedDate;
        }
        private List<BatchProcessPlannedDate> CreateObjects(IDataReader oReader)
        {
            List<BatchProcessPlannedDate> oBatchProcessPlannedDate = new List<BatchProcessPlannedDate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BatchProcessPlannedDate oItem = CreateObject(oHandler);
                oBatchProcessPlannedDate.Add(oItem);
            }
            return oBatchProcessPlannedDate;
        }

        #endregion

        #region Interface implementation
        public BatchProcessPlannedDateService() { }

        public BatchProcessPlannedDate Save(BatchProcessPlannedDate oBatchProcessPlannedDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBatchProcessPlannedDate.BatchProcessPlannedDateID <= 0)
                {
                    reader = BatchProcessPlannedDateDA.InsertUpdate(tc, oBatchProcessPlannedDate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BatchProcessPlannedDateDA.InsertUpdate(tc, oBatchProcessPlannedDate, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBatchProcessPlannedDate = new BatchProcessPlannedDate();
                    oBatchProcessPlannedDate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BatchProcessPlannedDate. Because of " + e.Message, e);
                #endregion
            }
            return oBatchProcessPlannedDate;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BatchProcessPlannedDate oBatchProcessPlannedDate = new BatchProcessPlannedDate();
                oBatchProcessPlannedDate.BatchProcessPlannedDateID = id;
                BatchProcessPlannedDateDA.Delete(tc, oBatchProcessPlannedDate, EnumDBOperation.Delete, nUserId);
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

        public BatchProcessPlannedDate Get(int id, Int64 nUserId)
        {
            BatchProcessPlannedDate oBatchProcessPlannedDate = new BatchProcessPlannedDate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BatchProcessPlannedDateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBatchProcessPlannedDate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BatchProcessPlannedDate", e);
                #endregion
            }
            return oBatchProcessPlannedDate;
        }
        public List<BatchProcessPlannedDate> Gets(Int64 nUserID)
        {
            List<BatchProcessPlannedDate> oBatchProcessPlannedDates = new List<BatchProcessPlannedDate>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BatchProcessPlannedDateDA.Gets(tc);
                oBatchProcessPlannedDates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BatchProcessPlannedDate", e);
                #endregion
            }
            return oBatchProcessPlannedDates;
        }
        public List<BatchProcessPlannedDate> Gets(string sSQL,Int64 nUserID)
        {
            List<BatchProcessPlannedDate> oBatchProcessPlannedDates = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BatchProcessPlannedDateDA.Gets(tc,sSQL);
                oBatchProcessPlannedDates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BatchProcessPlannedDate", e);
                #endregion
            }
            return oBatchProcessPlannedDates;
        }
        #endregion
    }   
}