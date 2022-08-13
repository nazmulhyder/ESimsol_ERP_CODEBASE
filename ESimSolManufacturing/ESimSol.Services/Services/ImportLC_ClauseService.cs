using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class ImportLC_ClauseService : MarshalByRefObject, IImportLC_ClauseService
    {
        #region Private functions and declaration
        private ImportLC_Clause MapObject(NullHandler oReader)
        {
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            oImportLC_Clause.ImportLC_ClauseID= oReader.GetInt32("ImportLC_ClauseID");
            oImportLC_Clause.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLC_Clause.Clause = oReader.GetString("Clause");
            oImportLC_Clause.Caption = oReader.GetString("Caption");
            return oImportLC_Clause;
        }

        private ImportLC_Clause CreateObject(NullHandler oReader)
        {
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            oImportLC_Clause=MapObject(oReader);
            return oImportLC_Clause;
        }

        private List<ImportLC_Clause> CreateObjects(IDataReader oReader)
        {
            List<ImportLC_Clause> oImportLC_Clauses = new List<ImportLC_Clause>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLC_Clause oItem = CreateObject(oHandler);
                oImportLC_Clauses.Add(oItem);
            }
            return oImportLC_Clauses;
        }
        #endregion

        #region Interface implementation
        public ImportLC_ClauseService() { }


        public ImportLC_Clause Save(List<ImportLC_Clause> oImportLC_ClauseForLCs, Int64 nUserId)
        {
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            EnumLCCurrentStatus eLCCurrentStatus = EnumLCCurrentStatus.None;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                eLCCurrentStatus = (EnumLCCurrentStatus)ImportLCHistryDA.GetImportLCStatusStatus(tc, oImportLC_ClauseForLCs[0].ImportLCID, EnumLCCurrentStatus.LC_Open);

                if (eLCCurrentStatus == EnumLCCurrentStatus.LC_Open)
                {
                    throw new Exception("Already LC is Open! \nPlease, You can change it after amendment");
                }
                string sClauseIDs = "";
                foreach (ImportLC_Clause item in oImportLC_ClauseForLCs)
                {
                    if (item.ImportLC_ClauseID <= 0)
                    {
                        item.ImportLC_ClauseID = ImportLC_ClauseDA.GetNewID(tc);
                        ImportLC_ClauseDA.Insert(tc, item);
                    }
                    else
                    {
                        ImportLC_ClauseDA.Update(tc, item);
                    }
                    sClauseIDs = sClauseIDs + item.ClauseID.ToString() + ",";
                    if (sClauseIDs.Length > 0)
                    {
                        sClauseIDs = sClauseIDs.Remove(sClauseIDs.Length - 1, 1);
                    }
                }

                if (oImportLC_ClauseForLCs.Count > 0)
                {
                    ImportLC_ClauseDA.DeleteByImportLC_RLC(tc, oImportLC_ClauseForLCs[0].ImportLCID, sClauseIDs);
                }

                eLCCurrentStatus = (EnumLCCurrentStatus)ImportLCHistryDA.GetImportLCStatusStatus(tc, oImportLC_ClauseForLCs[0].ImportLCID, EnumLCCurrentStatus.ReqForLC);

                ImportLCHistry oImportLCHistry = new ImportLCHistry();
                oImportLCHistry.ImportLCID = oImportLC_ClauseForLCs[0].ImportLCID;
               // oImportLCHistry.PrevioustStatus =(EnumLCCurrentStatus)ImportLCDA.GetImportLCCurrentStatus(tc,oImportLC_ClauseForLCs[0].ImportLCID);
                oImportLCHistry.CurrentStatus = EnumLCCurrentStatus.ReqForLC;
                if (eLCCurrentStatus != EnumLCCurrentStatus.ReqForLC)
                {
                    oImportLCHistry.ImportLCHistryID= ImportLCHistryDA.GetNewID(tc);
                    ImportLCHistryDA.Insert(tc, oImportLCHistry, nUserId);
                   // ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLC_ClauseForLCs[0].ImportLCID);
                }
                else
                {
                    ImportLCHistryDA.UpdateByImportLCID(tc, oImportLCHistry, nUserId);
                   // ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLC_ClauseForLCs[0].ImportLCID);
                }

                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC_Clause = new  ImportLC_Clause();
                oImportLC_Clause.ErrorMessage = e.Message;
                #endregion
            }

            return oImportLC_Clause;
        }
        public List<ImportLC_Clause> SaveAll(List<ImportLC_Clause> oImportLC_Clauses,ImportLC oImportLC, Int64 nUserID)
        {
            ImportLC_Clause oImportLC_Clause = new ImportLC_Clause();
            List<ImportLC_Clause> oImportLC_Clauses_Return = new List<ImportLC_Clause>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportLC_ClauseDA.DeleteClause(tc, oImportLC.ImportLCID, oImportLC.ImportLCLogID, oImportLC.LCCurrentStatusInt);
                if (oImportLC_Clauses != null && oImportLC_Clauses.Count>0)
                {
                    ImportLC_ClauseDA.DeleteClause(tc, oImportLC_Clauses[0].ImportLCID, oImportLC_Clauses[0].ImportLCLogID, oImportLC_Clauses[0].LCCurrentStatusInt);
                    foreach (ImportLC_Clause oItem in oImportLC_Clauses)
                    {
                        IDataReader reader;
                        if (oItem.ImportLC_ClauseID <= 0)
                        {
                            reader = ImportLC_ClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            reader = ImportLC_ClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oImportLC_Clause = new ImportLC_Clause();
                            oImportLC_Clause = CreateObject(oReader);
                            oImportLC_Clauses_Return.Add(oImportLC_Clause);
                        }
                        reader.Close();
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC_Clause = new ImportLC_Clause();
                oImportLC_Clause.ErrorMessage = e.Message.Split('~')[0];
                oImportLC_Clauses_Return.Add(oImportLC_Clause);

                #endregion
            }
            return oImportLC_Clauses_Return;
        }

        public List<ImportLC_Clause> GetsBySQL(string sSQL, Int64 nUserId)
        {
            List<ImportLC_Clause> oImportLC_Clauses = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_ClauseDA.GetsBySQL(tc, sSQL);
                oImportLC_Clauses = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportLC_Clauses;
        }

        public List<ImportLC_Clause> GetsByImportLCID(int nImportLCID, Int64 nUserId)
        {
            List<ImportLC_Clause> oImportLC_Clauses = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_ClauseDA.GetsByImportLCID(tc, nImportLCID);
                oImportLC_Clauses = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportLC_Clauses;
        }

        public List<ImportLC_Clause> Gets(int nImportLCID, int nImportLCLogID, int nLCCurrentStatus, Int64 nUserId)
        {
            List<ImportLC_Clause> oImportLC_Clauses = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_ClauseDA.Gets(tc,  nImportLCID,  nImportLCLogID,  nLCCurrentStatus);
                oImportLC_Clauses = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportLC_Clauses;
        }

        #endregion
    }
   
   
}
