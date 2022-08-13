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
    
    public class ImportPIHistoryService : MarshalByRefObject, IImportPIHistoryService
    {
        #region Private functions and declaration
        private ImportPIHistory MapObject(NullHandler oReader)
        {
            ImportPIHistory oImportPIHistory = new ImportPIHistory();
            oImportPIHistory.ImportPIHistoryID = oReader.GetInt32("ImportPIHistoryID");
            oImportPIHistory.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPIHistory.State = (EnumImportPIState)oReader.GetInt16("State");
            oImportPIHistory.PreviousState = (EnumImportPIState)oReader.GetInt16("PreviousState");
            oImportPIHistory.DateTime = oReader.GetDateTime("DBServerDateTime");
            oImportPIHistory.Note = oReader.GetString("Note");
            oImportPIHistory.NoteSystem = oReader.GetString("NoteSystem");
            oImportPIHistory.DBUserID = oReader.GetInt32("DBUserID");
            oImportPIHistory.UserName = oReader.GetString("UserName");       
       
            return oImportPIHistory;
        }

        private ImportPIHistory CreateObject(NullHandler oReader)
        {
            ImportPIHistory oImportPIHistory = new ImportPIHistory();
            oImportPIHistory=MapObject(oReader);
            return oImportPIHistory;
        }

        private List<ImportPIHistory> CreateObjects(IDataReader oReader)
        {
            List<ImportPIHistory> oImportPIHistorys = new List<ImportPIHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIHistory oItem = CreateObject(oHandler);
                oImportPIHistorys.Add(oItem);
            }
            return oImportPIHistorys;
        }
        #endregion

        #region Interface implementation
        public ImportPIHistoryService() { }
     
        public ImportPIHistory Get(int id, Int64 nUserId)
        {
            ImportPIHistory oImportPIHistory = new ImportPIHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPIHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPIHistory", e);
                #endregion
            }

            return oImportPIHistory;
        }

        public List<ImportPIHistory> Gets(int nImportPIID, Int64 nUserId)
        {
            List<ImportPIHistory> oImportPIHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIHistoryDA.Gets(tc, nImportPIID);
                oImportPIHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIHistorys", e);
                #endregion
            }

            return oImportPIHistorys;
        } 
        #endregion
    }
}
