using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{


    public class MasterLCHistoryService : MarshalByRefObject, IMasterLCHistoryService
    {
        #region Private functions and declaration
        private MasterLCHistory MapObject(NullHandler oReader)
        {
            MasterLCHistory oMasterLCHistory = new MasterLCHistory();

            oMasterLCHistory.MasterLCHistoryID = oReader.GetInt32("MasterLCHistoryID");
            oMasterLCHistory.MasterLCID = oReader.GetInt32("MasterLCID");
            oMasterLCHistory.PreviousStatus = (EnumLCStatus)oReader.GetInt32("PreviousStatus");
            oMasterLCHistory.CurrentStatus = (EnumLCStatus)oReader.GetInt32("CurrentStatus");
            oMasterLCHistory.OperateBy = oReader.GetInt32("OperateBy");
            oMasterLCHistory.Note = oReader.GetString("Note");
            oMasterLCHistory.OperateByName = oReader.GetString("OperateByName");
            oMasterLCHistory.MasterLCNo = oReader.GetString("MasterLCNo");
            oMasterLCHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            return oMasterLCHistory;
        }

        private MasterLCHistory CreateObject(NullHandler oReader)
        {
            MasterLCHistory oMasterLCHistory = new MasterLCHistory();
            oMasterLCHistory = MapObject(oReader);
            return oMasterLCHistory;
        }

        private List<MasterLCHistory> CreateObjects(IDataReader oReader)
        {
            List<MasterLCHistory> oMasterLCHistory = new List<MasterLCHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterLCHistory oItem = CreateObject(oHandler);
                oMasterLCHistory.Add(oItem);
            }
            return oMasterLCHistory;
        }

        #endregion

        #region Interface implementation
        public MasterLCHistoryService() { }

        public MasterLCHistory Save(MasterLCHistory oMasterLCHistory, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<MasterLCHistory> _oMasterLCHistorys = new List<MasterLCHistory>();
            oMasterLCHistory.ErrorMessage = "";

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMasterLCHistory.MasterLCHistoryID <= 0)
                {

                    reader = MasterLCHistoryDA.InsertUpdate(tc, oMasterLCHistory, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = MasterLCHistoryDA.InsertUpdate(tc, oMasterLCHistory, EnumDBOperation.Update, nUserID, "");
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLCHistory = new MasterLCHistory();
                    oMasterLCHistory = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMasterLCHistory.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save MasterLCHistory. Because of " + e.Message, e);
                #endregion
            }
            return oMasterLCHistory;
        }


        public MasterLCHistory Get(int id, Int64 nUserId)
        {
            MasterLCHistory oAccountHead = new MasterLCHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MasterLCHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MasterLCHistory> Gets(int MasterLCID, Int64 nUserID)
        {
            List<MasterLCHistory> oMasterLCHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCHistoryDA.Gets(MasterLCID, tc);
                oMasterLCHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCHistory", e);
                #endregion
            }

            return oMasterLCHistory;
        }

        public List<MasterLCHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<MasterLCHistory> oMasterLCHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCHistoryDA.Gets(tc, sSQL);
                oMasterLCHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCHistory", e);
                #endregion
            }

            return oMasterLCHistory;
        }
        #endregion
    }
  
}
