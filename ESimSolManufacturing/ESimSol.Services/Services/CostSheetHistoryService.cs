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


    public class CostSheetHistoryService : MarshalByRefObject, ICostSheetHistoryService
    {
        #region Private functions and declaration
        private CostSheetHistory MapObject(NullHandler oReader)
        {
            CostSheetHistory oCostSheetHistory = new CostSheetHistory();

            oCostSheetHistory.CostSheetHistoryID = oReader.GetInt32("CostSheetHistoryID");
            oCostSheetHistory.CostSheetID = oReader.GetInt32("CostSheetID");
            oCostSheetHistory.PreviousStatus = (EnumCostSheetStatus)oReader.GetInt32("PreviousStatus");
            oCostSheetHistory.CurrentStatus = (EnumCostSheetStatus)oReader.GetInt32("CurrentStatus");
            oCostSheetHistory.OperationBy = oReader.GetInt32("OperationBy");
            oCostSheetHistory.Note = oReader.GetString("Note");
            oCostSheetHistory.OperateByName = oReader.GetString("OperateByName");
            oCostSheetHistory.FileNo = oReader.GetString("FileNo");
            oCostSheetHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            return oCostSheetHistory;
        }

        private CostSheetHistory CreateObject(NullHandler oReader)
        {
            CostSheetHistory oCostSheetHistory = new CostSheetHistory();
            oCostSheetHistory = MapObject(oReader);
            return oCostSheetHistory;
        }

        private List<CostSheetHistory> CreateObjects(IDataReader oReader)
        {
            List<CostSheetHistory> oCostSheetHistory = new List<CostSheetHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSheetHistory oItem = CreateObject(oHandler);
                oCostSheetHistory.Add(oItem);
            }
            return oCostSheetHistory;
        }

        #endregion

        #region Interface implementation
        public CostSheetHistoryService() { }

        public CostSheetHistory Save(CostSheetHistory oCostSheetHistory, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<CostSheetHistory> _oCostSheetHistorys = new List<CostSheetHistory>();
            oCostSheetHistory.ErrorMessage = "";

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCostSheetHistory.CostSheetHistoryID <= 0)
                {

                    reader = CostSheetHistoryDA.InsertUpdate(tc, oCostSheetHistory, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = CostSheetHistoryDA.InsertUpdate(tc, oCostSheetHistory, EnumDBOperation.Update, nUserID, "");
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheetHistory = new CostSheetHistory();
                    oCostSheetHistory = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oCostSheetHistory.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save CostSheetHistory. Because of " + e.Message, e);
                #endregion
            }
            return oCostSheetHistory;
        }


        public CostSheetHistory Get(int id, Int64 nUserId)
        {
            CostSheetHistory oAccountHead = new CostSheetHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CostSheetHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CostSheetHistory> Gets(int ProfromaInvoiceID, Int64 nUserID)
        {
            List<CostSheetHistory> oCostSheetHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetHistoryDA.Gets(ProfromaInvoiceID, tc);
                oCostSheetHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetHistory", e);
                #endregion
            }

            return oCostSheetHistory;
        }

        public List<CostSheetHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<CostSheetHistory> oCostSheetHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetHistoryDA.Gets(tc, sSQL);
                oCostSheetHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetHistory", e);
                #endregion
            }

            return oCostSheetHistory;
        }
        #endregion
    }
    
   
}
