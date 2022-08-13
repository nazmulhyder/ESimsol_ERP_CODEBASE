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
    public class ImportLC_RequestService : MarshalByRefObject, IImportLC_RequestService
    {
        #region Private functions and declaration
        private ImportLC_Request MapObject(NullHandler oReader)
        {
            ImportLC_Request oImportLC_Request = new ImportLC_Request();
            oImportLC_Request.ImportLC_RequestID= oReader.GetInt32("ImportLC_RequestID");
            oImportLC_Request.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLC_Request.Clause = oReader.GetString("Clause");
            oImportLC_Request.Text = oReader.GetString("Clause");
            return oImportLC_Request;
        }

        private ImportLC_Request CreateObject(NullHandler oReader)
        {
            ImportLC_Request oImportLC_Request = new ImportLC_Request();
            oImportLC_Request=MapObject(oReader);
            return oImportLC_Request;
        }

        private List<ImportLC_Request> CreateObjects(IDataReader oReader)
        {
            List<ImportLC_Request> oImportLC_Requests = new List<ImportLC_Request>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLC_Request oItem = CreateObject(oHandler);
                oImportLC_Requests.Add(oItem);
            }
            return oImportLC_Requests;
        }
        #endregion

        #region Interface implementation
        public ImportLC_RequestService() { }


        public ImportLC_Request Save(List<ImportLC_Request> oImportLC_RequestForLCs, Int64 nUserId)
        {
            ImportLC_Request oImportLC_Request = new ImportLC_Request();
            EnumLCCurrentStatus eLCCurrentStatus = EnumLCCurrentStatus.None;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                eLCCurrentStatus = (EnumLCCurrentStatus)ImportLCHistryDA.GetImportLCStatusStatus(tc, oImportLC_RequestForLCs[0].ImportLCID, EnumLCCurrentStatus.LC_Open);

                if (eLCCurrentStatus == EnumLCCurrentStatus.LC_Open)
                {
                    throw new Exception("Already LC is Open! \nPlease, You can change it after amendment");
                }
                string sClauseIDs = "";
                foreach (ImportLC_Request item in oImportLC_RequestForLCs)
                {
                    if (item.ImportLC_RequestID <= 0)
                    {
                        item.ImportLC_RequestID = ImportLC_RequestDA.GetNewID(tc);
                        ImportLC_RequestDA.Insert(tc, item);
                    }
                    else
                    {
                        ImportLC_RequestDA.Update(tc, item);
                    }
                    sClauseIDs = sClauseIDs + item.ClauseID.ToString() + ",";
                    if (sClauseIDs.Length > 0)
                    {
                        sClauseIDs = sClauseIDs.Remove(sClauseIDs.Length - 1, 1);
                    }
                }

                if (oImportLC_RequestForLCs.Count > 0)
                {
                    ImportLC_RequestDA.DeleteByImportLC_RLC(tc, oImportLC_RequestForLCs[0].ImportLCID, sClauseIDs);
                }

                eLCCurrentStatus = (EnumLCCurrentStatus)ImportLCHistryDA.GetImportLCStatusStatus(tc, oImportLC_RequestForLCs[0].ImportLCID, EnumLCCurrentStatus.ReqForLC);

                ImportLCHistry oImportLCHistry = new ImportLCHistry();
                oImportLCHistry.ImportLCID = oImportLC_RequestForLCs[0].ImportLCID;
               // oImportLCHistry.PrevioustStatus =(EnumLCCurrentStatus)ImportLCDA.GetImportLCCurrentStatus(tc,oImportLC_RequestForLCs[0].ImportLCID);
                oImportLCHistry.CurrentStatus = EnumLCCurrentStatus.ReqForLC;
                if (eLCCurrentStatus != EnumLCCurrentStatus.ReqForLC)
                {
                    oImportLCHistry.ImportLCHistryID= ImportLCHistryDA.GetNewID(tc);
                    ImportLCHistryDA.Insert(tc, oImportLCHistry, nUserId);
                   // ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLC_RequestForLCs[0].ImportLCID);
                }
                else
                {
                    ImportLCHistryDA.UpdateByImportLCID(tc, oImportLCHistry, nUserId);
                   // ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLC_RequestForLCs[0].ImportLCID);
                }

                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC_Request = new  ImportLC_Request();
                oImportLC_Request.ErrorMessage = e.Message;
                #endregion
            }

            return oImportLC_Request;
        }

        public List<ImportLC_Request> GetsBySQL(string sSQL, Int64 nUserId)
        {
            List<ImportLC_Request> oImportLC_Requests = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_RequestDA.GetsBySQL(tc, sSQL);
                oImportLC_Requests = CreateObjects(reader);
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

            return oImportLC_Requests;
        }

        public List<ImportLC_Request> GetsByImportLCID(int nImportLCID, Int64 nUserId)
        {
            List<ImportLC_Request> oImportLC_Requests = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_RequestDA.GetsByImportLCID(tc, nImportLCID);
                oImportLC_Requests = CreateObjects(reader);
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

            return oImportLC_Requests;
        }

        public List<ImportLC_Request> Gets(int nImportLCID, Int64 nUserId)
        {
            List<ImportLC_Request> oImportLC_Requests = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLC_RequestDA.Gets(tc, nImportLCID);
                oImportLC_Requests = CreateObjects(reader);
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

            return oImportLC_Requests;
        }

        #endregion
    }
   
   
}
