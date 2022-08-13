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
    
    public class ExportBillHistoryService : MarshalByRefObject, IExportBillHistoryService
    {
        #region Private functions and declaration
        private ExportBillHistory MapObject(NullHandler oReader)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            oExportBillHistory.ExportBillHistoryID = oReader.GetInt32("ExportBillHistoryID");
            oExportBillHistory.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillHistory.State = (EnumLCBillEvent)oReader.GetInt16("State");
            oExportBillHistory.PreviousState = (EnumLCBillEvent)oReader.GetInt16("PreviousState");
            oExportBillHistory.DateTime = oReader.GetDateTime("DBServerDateTime");
            oExportBillHistory.Note = oReader.GetString("Note");
            oExportBillHistory.NoteSystem = oReader.GetString("NoteSystem");
            oExportBillHistory.DBUserID = oReader.GetInt32("DBUserID");
            oExportBillHistory.UserName = oReader.GetString("UserName");       
            //oExportBillHistory.CntDBUserID = oReader.GetInt32("CntDBUserID");
            //oExportBillHistory.CntDBDateTime = oReader.GetDateTime("CntDBDateTime");
            return oExportBillHistory;
        }

        private ExportBillHistory CreateObject(NullHandler oReader)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();
            oExportBillHistory=MapObject(oReader);
            return oExportBillHistory;
        }

        private List<ExportBillHistory> CreateObjects(IDataReader oReader)
        {
            List<ExportBillHistory> oExportBillHistorys = new List<ExportBillHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillHistory oItem = CreateObject(oHandler);
                oExportBillHistorys.Add(oItem);
            }
            return oExportBillHistorys;
        }
        #endregion

        #region Interface implementation
        public ExportBillHistoryService() { }
     
        public string Delete(ExportBillHistory oExportBillHistory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ///checked validation
                //Export_LDBPDetailDA.UpdateState(tc, (int)EnumLCBillEvent.BillCancel, oExportBillHistory.ExportBillID);
                
                ExportBillHistoryDA.Delete(tc, oExportBillHistory);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return "Deletion not possible" + e.Message;
                #endregion
            }

        }

        public ExportBillHistory Get(int id, Int64 nUserId)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillHistory", e);
                #endregion
            }

            return oExportBillHistory;
        }
        public ExportBillHistory Getby(int nLCBillID, int eEvent, Int64 nUserId)
        {
            ExportBillHistory oExportBillHistory = new ExportBillHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillHistoryDA.Getby(tc, nLCBillID, eEvent);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportBillHistory", e);
                #endregion
            }

            return oExportBillHistory;
        }

        public List<ExportBillHistory> Gets(int nExportBillID, Int64 nUserId)
        {
            List<ExportBillHistory> oExportBillHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillHistoryDA.Gets(tc, nExportBillID);
                oExportBillHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillHistorys", e);
                #endregion
            }

            return oExportBillHistorys;
        } 
        #endregion
    }
}
