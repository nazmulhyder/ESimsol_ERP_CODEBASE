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
    public class ImportLCHistryService : MarshalByRefObject, IImportLCHistryService
    {
        #region Private functions and declaration
        private ImportLCHistry MapObject(NullHandler oReader)
        {
            ImportLCHistry oImportLCHistry = new ImportLCHistry();
            oImportLCHistry.ImportLCHistryID= oReader.GetInt32("ImportLCHistryID");
            oImportLCHistry.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLCHistry.CurrentStatus = (EnumLCCurrentStatus)oReader.GetInt16("CurrentStatus");
            oImportLCHistry.OperationDate = oReader.GetDateTime("OperationDate");
            oImportLCHistry.Note = oReader.GetString("Note");
            oImportLCHistry.DBUserID = oReader.GetInt32("DBUserID");
            oImportLCHistry.PrevioustStatus = (EnumLCCurrentStatus)oReader.GetInt16("PrevioustStatus");
            return oImportLCHistry;
        }

        private ImportLCHistry CreateObject(NullHandler oReader)
        {
            ImportLCHistry oImportLCHistry = new ImportLCHistry();
            oImportLCHistry=MapObject(oReader);
            return oImportLCHistry;
        }

        private List<ImportLCHistry> CreateObjects(IDataReader oReader)
        {
            List<ImportLCHistry> oImportLCHistrys = new List<ImportLCHistry>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCHistry oItem = CreateObject(oHandler);
                oImportLCHistrys.Add(oItem);
            }
            return oImportLCHistrys;
        }
        #endregion

        #region Interface implementation
        public ImportLCHistryService() { }

        public ImportLCHistry Save(ImportLCHistry oImportLCHistry, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oImportLCHistry.ImportLCHistryID<=0)
                {
                    oImportLCHistry.ImportLCHistryID=ImportLCHistryDA.GetNewID(tc);
                    ImportLCHistryDA.Insert(tc, oImportLCHistry,nUserID);
                    //ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLCHistry.ImportLCID);
                }
                else
                {
                    ImportLCHistryDA.Update(tc, oImportLCHistry, nUserID);
                   // ImportLCDA.UpdateStatus(tc, oImportLCHistry.CurrentStatus, oImportLCHistry.ImportLCID);
                }
               
                tc.End();
                BusinessObject.Factory.SetObjectState(oImportLCHistry, ObjectState.Saved);
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save ImportLCHistry", e);
                #endregion
            }
            return oImportLCHistry;
        }
        public bool Delete(ImportLCHistry oImportLCHistry, Int64 nUserID)
        {
            ImportLCHistry _oImportLCHistry = new ImportLCHistry();
            //_oImportLCHistry = _oImportLCHistry.Get(oImportLCHistry.InvoiceID, EnumInvoiceEvent.DocReceive_By_Bank);
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //Check deletion validity
                //string MoreParameter = "AND InvoiceEvent > " + Convert.ToInt16(oImportLCHistry.InvoiceEvent).ToString();
                //if (RunSQLDA.ChildCount(tc, "ImportLCHistry", "InvoiceID", oImportLCHistry.InvoiceID, MoreParameter) > 0)
                //{
                //    throw new Exception("Deletion not possible! \nPlease, first check the on other operation.");
                //}              
                //if (oImportLCHistry.ObjectID == _oImportLCHistry.ObjectID)
                //{
                //    InvoiceDA.UpdateActiveForApproval(tc, true, oImportLCHistry.InvoiceID);
                //}
                ImportLCHistryDA.Delete(tc, oImportLCHistry.ImportLCHistryID);
                tc.End();
                return true;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
        }

        public ImportLCHistry Get(int nPLCID, EnumLCCurrentStatus eEvent, Int64 nUserID)
        {
            ImportLCHistry oImportLCHistry = new ImportLCHistry();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCHistryDA.Get(tc,  nPLCID, eEvent);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCHistry = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLCHistry", e);
                #endregion
            }
            return oImportLCHistry;
        }

        public List<ImportLCHistry> Gets(int nPLCID, Int64 nUserID)
        {
            List<ImportLCHistry> oImportLCHistrys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCHistryDA.Gets(tc, nPLCID);
                oImportLCHistrys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCHistrys", e);
                #endregion
            }

            return oImportLCHistrys;
        }
        public List<ImportLCHistry> Gets(int nPLCID, string eStatus, Int64 nUserID)
        {
            List<ImportLCHistry> oImportLCHistrys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCHistryDA.Gets(tc,  nPLCID,  eStatus);
                oImportLCHistrys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCHistrys", e);
                #endregion
            }

            return oImportLCHistrys;
        }
        #endregion
    }
}
