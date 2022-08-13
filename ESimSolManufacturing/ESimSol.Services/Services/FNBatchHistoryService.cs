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
    public class FNBatchHistoryService : MarshalByRefObject, IFNBatchHistoryService
    {
        #region Private functions and declaration
        private FNBatchHistory MapObject(NullHandler oReader)
        {
            FNBatchHistory oFNBatchHistory = new FNBatchHistory();
            oFNBatchHistory.FNBatchHistoryID = oReader.GetInt32("FNBatchHistoryID");
            oFNBatchHistory.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchHistory.PreviousStatus = oReader.GetInt32("PreviousStatus");
            oFNBatchHistory.CurrentStatus = oReader.GetInt32("CurrentStatus");
            return oFNBatchHistory;
        }

        private FNBatchHistory CreateObject(NullHandler oReader)
        {
            FNBatchHistory oFNBatchHistory = MapObject(oReader);
            return oFNBatchHistory;
        }

        private List<FNBatchHistory> CreateObjects(IDataReader oReader)
        {
            List<FNBatchHistory> oFNBatchHistory = new List<FNBatchHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchHistory oItem = CreateObject(oHandler);
                oFNBatchHistory.Add(oItem);
            }
            return oFNBatchHistory;
        }

        #endregion

        #region Interface implementation
        public FNBatchHistoryService() { }


        public FNBatchHistory Get(int nFNBatchHistoryID, Int64 nUserId)
        {
            FNBatchHistory oFNBatchHistory = new FNBatchHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchHistoryDA.Get(tc, nFNBatchHistoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchHistory = new FNBatchHistory();
                oFNBatchHistory.ErrorMessage = ex.Message;
                #endregion
            }

            return oFNBatchHistory;
        }

        public List<FNBatchHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatchHistory> oFNBatchHistorys = new List<FNBatchHistory>();
            FNBatchHistory oFNBatchHistory = new FNBatchHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchHistoryDA.Gets(tc, sSQL);
                oFNBatchHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchHistory.ErrorMessage = ex.Message;
                oFNBatchHistorys.Add(oFNBatchHistory);
                #endregion
            }

            return oFNBatchHistorys;
        }

        #endregion
    }
}
