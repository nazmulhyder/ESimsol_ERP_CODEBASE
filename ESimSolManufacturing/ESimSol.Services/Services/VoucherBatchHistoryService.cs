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
    public class VoucherBatchHistoryService : MarshalByRefObject, IVoucherBatchHistoryService
    {
        #region Private functions and declaration
        private VoucherBatchHistory MapObject(NullHandler oReader)
        {
            VoucherBatchHistory oVoucherBatchHistory = new VoucherBatchHistory();
            oVoucherBatchHistory.VoucherBatchHistoryID = oReader.GetInt32("VoucherBatchHistoryID");
            oVoucherBatchHistory.VoucherBatchID = oReader.GetInt32("VoucherBatchID");
            oVoucherBatchHistory.BatchNO = oReader.GetString("BatchNO");
            oVoucherBatchHistory.CreateBy = oReader.GetInt32("CreateBy");
            oVoucherBatchHistory.CreateDate = oReader.GetDateTime("CreateDate");
            oVoucherBatchHistory.PreviousBatchStatus = (EnumVoucherBatchStatus)oReader.GetInt16("PreviousBatchStatus");
            oVoucherBatchHistory.CurrentBatchStatus = (EnumVoucherBatchStatus)oReader.GetInt16("CurrentBatchStatus");
            oVoucherBatchHistory.RequestTo = oReader.GetInt32("RequestTo");
            oVoucherBatchHistory.RequestDate = oReader.GetDateTime("RequestDate");
            oVoucherBatchHistory.CreateByName = oReader.GetString("CreateByName");
            oVoucherBatchHistory.RequestToName = oReader.GetString("RequestToName");
            oVoucherBatchHistory.VoucherCount = oReader.GetInt32("VoucherCount");
            oVoucherBatchHistory.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            
            return oVoucherBatchHistory;
        }

        private VoucherBatchHistory CreateObject(NullHandler oReader)
        {
            VoucherBatchHistory oVoucherBatchHistory = new VoucherBatchHistory();
            oVoucherBatchHistory = MapObject(oReader);
            return oVoucherBatchHistory;
        }

        private List<VoucherBatchHistory> CreateObjects(IDataReader oReader)
        {
            List<VoucherBatchHistory> oVoucherBatchHistory = new List<VoucherBatchHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherBatchHistory oItem = CreateObject(oHandler);
                oVoucherBatchHistory.Add(oItem);
            }
            return oVoucherBatchHistory;
        }

        #endregion

        #region Interface implementation
        public VoucherBatchHistoryService() { }

       
        public VoucherBatchHistory Get(int id, int nUserId)
        {
            VoucherBatchHistory oAccountHead = new VoucherBatchHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherBatchHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherBatchHistory", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<VoucherBatchHistory> Gets(int nUserID)
        {
            List<VoucherBatchHistory> oVoucherBatchHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBatchHistoryDA.Gets(tc);
                oVoucherBatchHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatchHistory", e);
                #endregion
            }

            return oVoucherBatchHistory;
        }

        public List<VoucherBatchHistory> GetsByBatchID(int nVoucherBatchID, int nUserID)
        {
            List<VoucherBatchHistory> oVoucherBatchHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBatchHistoryDA.GetsByBatchID(tc, nVoucherBatchID);
                oVoucherBatchHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatchHistory", e);
                #endregion
            }

            return oVoucherBatchHistory;
        }
        
        public List<VoucherBatchHistory> Gets(string sSQL,int nUserID)
        {
            List<VoucherBatchHistory> oVoucherBatchHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from VoucherBatchHistory where VoucherBatchHistoryID in (1,2,80,272,347,370,60,45)";
                    }
                reader = VoucherBatchHistoryDA.Gets(tc, sSQL);
                oVoucherBatchHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatchHistory", e);
                #endregion
            }

            return oVoucherBatchHistory;
        }

       
        #endregion
    }   
}