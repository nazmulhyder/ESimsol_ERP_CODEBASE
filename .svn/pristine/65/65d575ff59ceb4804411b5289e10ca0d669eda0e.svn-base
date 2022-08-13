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
    public class CommentsHistoryService : MarshalByRefObject, ICommentsHistoryService
    {
        #region Private functions and declaration
        private CommentsHistory MapObject(NullHandler oReader)
        {
            CommentsHistory oCommentsHistory = new CommentsHistory();
            oCommentsHistory.CommentsHistoryID = oReader.GetInt32("CommentsHistoryID");
            oCommentsHistory.ModuleID = oReader.GetInt32("ModuleID");
            oCommentsHistory.CommentsBy = oReader.GetString("CommentsBy");
            oCommentsHistory.ModuleObjID = oReader.GetInt32("ModuleObjID");
            oCommentsHistory.CommentsText = oReader.GetString("CommentsText");
            oCommentsHistory.CommentsDateTime = oReader.GetDateTime("CommentsDateTime");
            return oCommentsHistory;
        }
        private CommentsHistory CreateObject(NullHandler oReader)
        {
            CommentsHistory oCommentsHistory = new CommentsHistory();
            oCommentsHistory = MapObject(oReader);
            return oCommentsHistory;
        }
        private List<CommentsHistory> CreateObjects(IDataReader oReader)
        {
            List<CommentsHistory> oCommentsHistory = new List<CommentsHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommentsHistory oItem = CreateObject(oHandler);
                oCommentsHistory.Add(oItem);
            }
            return oCommentsHistory;
        }

        #endregion

        #region Interface implementation
        public CommentsHistory Save(CommentsHistory oCommentsHistory, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCommentsHistory.CommentsHistoryID <= 0)
                {
                    reader = CommentsHistoryDA.InsertUpdate(tc, oCommentsHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = CommentsHistoryDA.InsertUpdate(tc, oCommentsHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommentsHistory = new CommentsHistory();
                    oCommentsHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCommentsHistory.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oCommentsHistory;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CommentsHistory oCommentsHistory = new CommentsHistory();
                oCommentsHistory.CommentsHistoryID = id;                
                CommentsHistoryDA.Delete(tc, oCommentsHistory, EnumDBOperation.Delete, nUserId);
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
        public List<CommentsHistory> Gets(int nUserID)
        {
            List<CommentsHistory> oCommentsHistorys = new List<CommentsHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommentsHistoryDA.Gets(tc);
                oCommentsHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCommentsHistorys = new List<CommentsHistory>();
                CommentsHistory oCommentsHistory = new CommentsHistory();
                oCommentsHistory.ErrorMessage = e.Message.Split('~')[0];
                oCommentsHistorys.Add(oCommentsHistory);
                #endregion
            }
            return oCommentsHistorys;
        }

        public List<CommentsHistory> Gets(string sSQL, int nUserID)
        {
            List<CommentsHistory> oCommentsHistorys = new List<CommentsHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommentsHistoryDA.Gets(tc, sSQL);
                oCommentsHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCommentsHistorys = new List<CommentsHistory>();
                CommentsHistory oCommentsHistory = new CommentsHistory();
                oCommentsHistory.ErrorMessage = e.Message.Split('~')[0];
                oCommentsHistorys.Add(oCommentsHistory);
                #endregion
            }
            return oCommentsHistorys;
        }
        public CommentsHistory Get(int id, int nUserId)
        {
            CommentsHistory oCommentsHistory = new CommentsHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CommentsHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommentsHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCommentsHistory.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oCommentsHistory;
        }

        #endregion
    }
}
