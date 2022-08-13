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
    public class ExportPIHistoryService : MarshalByRefObject, IExportPIHistoryService
    {
        #region Private functions and declaration
        private ExportPIHistory MapObject(NullHandler oReader)
        {
            ExportPIHistory oExportPIHistory = new ExportPIHistory();
            oExportPIHistory.ExportPIHistoryID = oReader.GetInt32("ExportPIHistoryID");
            oExportPIHistory.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPIHistory.PINo = oReader.GetString("PINo");
            oExportPIHistory.PreviousStatus = (EnumPIStatus)oReader.GetInt32("PreviousStatus");            
            oExportPIHistory.CurrentStatus = (EnumPIStatus)oReader.GetInt32("CurrentStatus");            
            oExportPIHistory.Note = oReader.GetString("Note");
            oExportPIHistory.OperateBy = oReader.GetInt32("OperateBy");
            oExportPIHistory.OperateByName = oReader.GetString("OperateByName");
            oExportPIHistory.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            return oExportPIHistory;
        }

        private ExportPIHistory CreateObject(NullHandler oReader)
        {
            ExportPIHistory oExportPIHistory = new ExportPIHistory();
            oExportPIHistory = MapObject(oReader);
            return oExportPIHistory;
        }

        private List<ExportPIHistory> CreateObjects(IDataReader oReader)
        {
            List<ExportPIHistory> oExportPIHistory = new List<ExportPIHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIHistory oItem = CreateObject(oHandler);
                oExportPIHistory.Add(oItem);
            }
            return oExportPIHistory;
        }

        #endregion

        #region Interface implementation
        public ExportPIHistoryService() { }

        public ExportPIHistory Save(ExportPIHistory oExportPIHistory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPIHistory.ExportPIHistoryID <= 0)
                {
                    reader = ExportPIHistoryDA.InsertUpdate(tc, oExportPIHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportPIHistoryDA.InsertUpdate(tc, oExportPIHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIHistory = new ExportPIHistory();
                    oExportPIHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ExportPIHistory. Because of " + e.Message, e);
                #endregion
            }
            return oExportPIHistory;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIHistory oExportPIHistory = new ExportPIHistory();
                oExportPIHistory.ExportPIHistoryID = id;
                ExportPIHistoryDA.Delete(tc, oExportPIHistory, EnumDBOperation.Delete, nUserId);
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

        public ExportPIHistory Get(int id, Int64 nUserId)
        {
            ExportPIHistory oExportPIHistory = new ExportPIHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportPIHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPIHistory", e);
                #endregion
            }
            return oExportPIHistory;
        }
                
        public List<ExportPIHistory> Gets(Int64 nUserID)
        {
            List<ExportPIHistory> oExportPIHistorys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIHistoryDA.Gets(tc);
                oExportPIHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIHistory", e);
                #endregion
            }
            return oExportPIHistorys;
        }

        public List<ExportPIHistory> GetsByExportId(int nExportPIID, Int64 nUserID)
        {
            List<ExportPIHistory> oExportPIHistorys = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIHistoryDA.GetsByExportId(tc, nExportPIID);
                oExportPIHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIHistory", e);
                #endregion
            }
            return oExportPIHistorys;
        }        
        #endregion
    }
}
