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
    public class GUProductionOrderHistoryService : MarshalByRefObject, IGUProductionOrderHistoryService
    {
        #region Private functions and declaration
        private GUProductionOrderHistory MapObject(NullHandler oReader)
        {
            GUProductionOrderHistory oGUProductionOrderHistory = new GUProductionOrderHistory();
            oGUProductionOrderHistory.GUProductionOrderHistoryID = oReader.GetInt32("GUProductionOrderHistoryID");
            oGUProductionOrderHistory.GUProductionOrderID = oReader.GetInt32("GUProductionOrderID");
            oGUProductionOrderHistory.PreviousStatus = (EnumGUProductionOrderStatus)oReader.GetInt32("PreviousStatus");
            oGUProductionOrderHistory.CurrentStatus = (EnumGUProductionOrderStatus)oReader.GetInt32("CurrentStatus");
            oGUProductionOrderHistory.Note = oReader.GetString("Note");
            return oGUProductionOrderHistory;
        }

        private GUProductionOrderHistory CreateObject(NullHandler oReader)
        {
            GUProductionOrderHistory oGUProductionOrderHistory = new GUProductionOrderHistory();
            oGUProductionOrderHistory = MapObject(oReader);
            return oGUProductionOrderHistory;
        }

        private List<GUProductionOrderHistory> CreateObjects(IDataReader oReader)
        {
            List<GUProductionOrderHistory> oGUProductionOrderHistory = new List<GUProductionOrderHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionOrderHistory oItem = CreateObject(oHandler);
                oGUProductionOrderHistory.Add(oItem);
            }
            return oGUProductionOrderHistory;
        }

        #endregion

        #region Interface implementation
        public GUProductionOrderHistoryService() { }

        public GUProductionOrderHistory Save(GUProductionOrderHistory oGUProductionOrderHistory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGUProductionOrderHistory.GUProductionOrderHistoryID <= 0)
                {
                    reader = GUProductionOrderHistoryDA.InsertUpdate(tc, oGUProductionOrderHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GUProductionOrderHistoryDA.InsertUpdate(tc, oGUProductionOrderHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrderHistory = new GUProductionOrderHistory();
                    oGUProductionOrderHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionOrderHistory = new GUProductionOrderHistory();
                oGUProductionOrderHistory.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GUProductionOrderHistory. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionOrderHistory;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUProductionOrderHistory oGUProductionOrderHistory = new GUProductionOrderHistory();
                oGUProductionOrderHistory.GUProductionOrderHistoryID = id;
                GUProductionOrderHistoryDA.Delete(tc, oGUProductionOrderHistory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GUProductionOrderHistory. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GUProductionOrderHistory Get(int id, Int64 nUserId)
        {
            GUProductionOrderHistory oAccountHead = new GUProductionOrderHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionOrderHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GUProductionOrderHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GUProductionOrderHistory> Gets(Int64 nUserID)
        {
            List<GUProductionOrderHistory> oGUProductionOrderHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderHistoryDA.Gets(tc);
                oGUProductionOrderHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrderHistory", e);
                #endregion
            }

            return oGUProductionOrderHistory;
        }
        #endregion
    }
}
