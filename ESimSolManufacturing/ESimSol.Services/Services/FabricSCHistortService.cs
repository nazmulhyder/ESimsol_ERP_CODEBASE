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
    public class FabricSCHistoryService : MarshalByRefObject, IFabricSCHistoryService
    {
        #region Private functions and declaration
        private FabricSCHistory MapObject(NullHandler oReader)
        {
            FabricSCHistory oFabricSCHistory = new FabricSCHistory();
            oFabricSCHistory.FabricSCHistoryID = oReader.GetInt32("FabricSCHistoryID");
            oFabricSCHistory.FabricSCID = oReader.GetInt32("FabricSCID");
            oFabricSCHistory.FabricSCDetailID = oReader.GetInt32("FabricSCDetailID");
            oFabricSCHistory.FSCStatus = (EnumFabricPOStatus)oReader.GetInt32("FSCStatus");
            oFabricSCHistory.FSCDStatus = (EnumPOState)oReader.GetInt32("FSCDStatus");
            oFabricSCHistory.FSCStatus_Prv = (EnumFabricPOStatus)oReader.GetInt32("FSCStatus_Prv");
            oFabricSCHistory.FSCDStatus_Prv = (EnumPOState)oReader.GetInt32("FSCDStatus_Prv");
            oFabricSCHistory.Note = oReader.GetString("Note");
            return oFabricSCHistory;
        }
        private FabricSCHistory CreateObject(NullHandler oReader)
        {
            FabricSCHistory oFabricSCHistory = new FabricSCHistory();
            oFabricSCHistory = MapObject(oReader);
            return oFabricSCHistory;
        }
        private List<FabricSCHistory> CreateObjects(IDataReader oReader)
        {
            List<FabricSCHistory> oFabricSCHistory = new List<FabricSCHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSCHistory oItem = CreateObject(oHandler);
                oFabricSCHistory.Add(oItem);
            }
            return oFabricSCHistory;
        }

        #endregion

        #region Interface implementation
        public FabricSCHistory Save(FabricSCHistory oFabricSCHistory, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricSCHistory.FabricSCHistoryID <= 0)
                {
                    reader = FabricSCHistoryDA.InsertUpdate(tc, oFabricSCHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricSCHistoryDA.InsertUpdate(tc, oFabricSCHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCHistory = new FabricSCHistory();
                    oFabricSCHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSCHistory.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSCHistory;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSCHistory oFabricSCHistory = new FabricSCHistory();
                oFabricSCHistory.FabricSCHistoryID = id;                
                FabricSCHistoryDA.Delete(tc, oFabricSCHistory, EnumDBOperation.Delete, nUserId);
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
        public List<FabricSCHistory> Gets(int nFSCID, int nUserID)
        {
            List<FabricSCHistory> oFabricSCHistorys = new List<FabricSCHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSCHistoryDA.Gets(tc, nFSCID);
                oFabricSCHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSCHistorys = new List<FabricSCHistory>();
                FabricSCHistory oFabricSCHistory = new FabricSCHistory();
                oFabricSCHistory.ErrorMessage = e.Message.Split('~')[0];
                oFabricSCHistorys.Add(oFabricSCHistory);
                #endregion
            }
            return oFabricSCHistorys;
        }

        public List<FabricSCHistory> Gets(string sSQL, int nUserID)
        {
            List<FabricSCHistory> oFabricSCHistorys = new List<FabricSCHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSCHistoryDA.Gets(tc, sSQL);
                oFabricSCHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSCHistorys = new List<FabricSCHistory>();
                FabricSCHistory oFabricSCHistory = new FabricSCHistory();
                oFabricSCHistory.ErrorMessage = e.Message.Split('~')[0];
                oFabricSCHistorys.Add(oFabricSCHistory);
                #endregion
            }
            return oFabricSCHistorys;
        }
        public FabricSCHistory Get(int id, int nUserId)
        {
            FabricSCHistory oFabricSCHistory = new FabricSCHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricSCHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSCHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSCHistory.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSCHistory;
        }

        #endregion
    }
}
