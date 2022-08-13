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
    public class DevelopmentRecapHistoryService : MarshalByRefObject, IDevelopmentRecapHistoryService
    {
        #region Private functions and declaration
        private DevelopmentRecapHistory MapObject(NullHandler oReader)
        {
            DevelopmentRecapHistory oDevelopmentRecapHistory = new DevelopmentRecapHistory();
            oDevelopmentRecapHistory.DevelopmentRecapHistoryID = oReader.GetInt32("DevelopmentRecapHistoryID");
            oDevelopmentRecapHistory.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopmentRecapHistory.CurrentStatus = (EnumDevelopmentStatus)oReader.GetInt32("CurrentStatus");
            oDevelopmentRecapHistory.PreviousStatus = (EnumDevelopmentStatus)oReader.GetInt32("PreviousStatus");
            oDevelopmentRecapHistory.OperationDate = oReader.GetDateTime("OperationDate");
            oDevelopmentRecapHistory.Note = oReader.GetString("Note");
            oDevelopmentRecapHistory.MarchandiserName = oReader.GetString("MarchandiserName");
            return oDevelopmentRecapHistory;
        }

        private DevelopmentRecapHistory CreateObject(NullHandler oReader)
        {
            DevelopmentRecapHistory oDevelopmentRecapHistory = new DevelopmentRecapHistory();
            oDevelopmentRecapHistory = MapObject(oReader);
            return oDevelopmentRecapHistory;
        }

        private List<DevelopmentRecapHistory> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentRecapHistory> oDevelopmentRecapHistory = new List<DevelopmentRecapHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentRecapHistory oItem = CreateObject(oHandler);
                oDevelopmentRecapHistory.Add(oItem);
            }
            return oDevelopmentRecapHistory;
        }

        #endregion

        #region Interface implementation
        public DevelopmentRecapHistoryService() { }

        public DevelopmentRecapHistory Save(DevelopmentRecapHistory oDevelopmentRecapHistory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDevelopmentRecapHistory.DevelopmentRecapHistoryID <= 0)
                {
                    reader = DevelopmentRecapHistoryDA.InsertUpdate(tc, oDevelopmentRecapHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DevelopmentRecapHistoryDA.InsertUpdate(tc, oDevelopmentRecapHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecapHistory = new DevelopmentRecapHistory();
                    oDevelopmentRecapHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DevelopmentRecapHistory. Because of " + e.Message, e);
                #endregion
            }
            return oDevelopmentRecapHistory;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DevelopmentRecapHistory oDevelopmentRecapHistory = new DevelopmentRecapHistory();
                oDevelopmentRecapHistory.DevelopmentRecapHistoryID = id;
                DevelopmentRecapHistoryDA.Delete(tc, oDevelopmentRecapHistory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DevelopmentRecapHistory. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public DevelopmentRecapHistory Get(int id, Int64 nUserId)
        {
            DevelopmentRecapHistory oAccountHead = new DevelopmentRecapHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentRecapHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DevelopmentRecapHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DevelopmentRecapHistory> Gets(Int64 nUserID)
        {
            List<DevelopmentRecapHistory> oDevelopmentRecapHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapHistoryDA.Gets(tc);
                oDevelopmentRecapHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapHistory", e);
                #endregion
            }

            return oDevelopmentRecapHistory;
        }

        public List<DevelopmentRecapHistory> GetsDevelopmentRecapHistotry(int nDevelopmentRecapID, int nCurrentStatus, Int64 nUserID)
        {
            List<DevelopmentRecapHistory> oDevelopmentRecapHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapHistoryDA.GetsDevelopmentRecapHistotry(tc, nDevelopmentRecapID, nCurrentStatus);
                oDevelopmentRecapHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapHistory", e);
                #endregion
            }

            return oDevelopmentRecapHistory;
        }
        #endregion
    }
}
