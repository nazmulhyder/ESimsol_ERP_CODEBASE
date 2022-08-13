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
    public class CommercialInvoiceHistoryService : MarshalByRefObject, ICommercialInvoiceHistoryService
    {
        #region Private functions and declaration
        private CommercialInvoiceHistory MapObject(NullHandler oReader)
        {
            CommercialInvoiceHistory oCommercialInvoiceHistory = new CommercialInvoiceHistory();
            oCommercialInvoiceHistory.CommercialInvoiceHistoryID = oReader.GetInt32("CommercialInvoiceHistoryID");
            oCommercialInvoiceHistory.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oCommercialInvoiceHistory.PreviousStatus = (EnumCommercialInvoiceStatus)oReader.GetInt32("PreviousStatus");
            oCommercialInvoiceHistory.CurrentStatus = (EnumCommercialInvoiceStatus)oReader.GetInt32("CurrentStatus");
            oCommercialInvoiceHistory.Note = oReader.GetString("Note");
            oCommercialInvoiceHistory.OperateBy = oReader.GetInt32("OperateBy");
            oCommercialInvoiceHistory.OperateByName = oReader.GetString("OperateByName");
            oCommercialInvoiceHistory.InVoiceNo = oReader.GetString("InVoiceNo");
            oCommercialInvoiceHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            
            return oCommercialInvoiceHistory;
        }

        private CommercialInvoiceHistory CreateObject(NullHandler oReader)
        {
            CommercialInvoiceHistory oCommercialInvoiceHistory = new CommercialInvoiceHistory();
            oCommercialInvoiceHistory = MapObject(oReader);
            return oCommercialInvoiceHistory;
        }

        private List<CommercialInvoiceHistory> CreateObjects(IDataReader oReader)
        {
            List<CommercialInvoiceHistory> oCommercialInvoiceHistory = new List<CommercialInvoiceHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommercialInvoiceHistory oItem = CreateObject(oHandler);
                oCommercialInvoiceHistory.Add(oItem);
            }
            return oCommercialInvoiceHistory;
        }

        #endregion

        #region Interface implementation
        public CommercialInvoiceHistoryService() { }
        public CommercialInvoiceHistory Get(int CommercialInvoiceHistoryID, Int64 nUserId)
        {
            CommercialInvoiceHistory oAccountHead = new CommercialInvoiceHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommercialInvoiceHistoryDA.Get(tc, CommercialInvoiceHistoryID);
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
                throw new ServiceException("Failed to Get CommercialInvoiceHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CommercialInvoiceHistory> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<CommercialInvoiceHistory> oCommercialInvoiceHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceHistoryDA.Gets(LabDipOrderID, tc);
                oCommercialInvoiceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoiceHistory", e);
                #endregion
            }

            return oCommercialInvoiceHistory;
        }

        public List<CommercialInvoiceHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<CommercialInvoiceHistory> oCommercialInvoiceHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceHistoryDA.Gets(tc, sSQL);
                oCommercialInvoiceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoiceHistory", e);
                #endregion
            }

            return oCommercialInvoiceHistory;
        }
        #endregion
    }
}
