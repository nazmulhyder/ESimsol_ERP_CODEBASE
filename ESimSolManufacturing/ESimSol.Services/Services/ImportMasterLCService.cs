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
    public class ImportMasterLCService : MarshalByRefObject, IImportMasterLCService
    {
        #region Private functions and declaration
        private ImportMasterLC MapObject(NullHandler oReader)
        {
            ImportMasterLC oImportMasterLC = new ImportMasterLC();
            oImportMasterLC.ImportMasterLCID = oReader.GetInt32("ImportMasterLCID");
            oImportMasterLC.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportMasterLC.MasterLCID = oReader.GetInt32("MasterLCID");
            oImportMasterLC.MasterLCDate = oReader.GetDateTime("MasterLCDate");
            oImportMasterLC.MasterLCNo = oReader.GetString("MasterLCNo");
            
            return oImportMasterLC;
        }
        private ImportMasterLC CreateObject(NullHandler oReader)
        {
            ImportMasterLC oImportMasterLC = new ImportMasterLC();
            oImportMasterLC = MapObject(oReader);
            return oImportMasterLC;
        }
        private List<ImportMasterLC> CreateObjects(IDataReader oReader)
        {
            List<ImportMasterLC> oImportMasterLC = new List<ImportMasterLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportMasterLC oItem = CreateObject(oHandler);
                oImportMasterLC.Add(oItem);
            }
            return oImportMasterLC;
        }

        #endregion

        #region Interface implementation
        public ImportMasterLC Save(ImportMasterLC oImportMasterLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportMasterLC.ImportMasterLCID <= 0)
                {
                    reader = ImportMasterLCDA.InsertUpdate(tc, oImportMasterLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportMasterLCDA.InsertUpdate(tc, oImportMasterLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportMasterLC = new ImportMasterLC();
                    oImportMasterLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportMasterLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportMasterLC;
        }
        public ImportMasterLC SaveWithMasterLC(ImportMasterLC oImportMasterLC, Int64 nUserId)
        {
            MasterLC oMasterLC = new MasterLC();
            oMasterLC = oImportMasterLC.MasterLCObj;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader readerMLC;
                if (oMasterLC.MasterLCID <= 0)
                {
                    readerMLC = MasterLCDA.InsertUpdate(tc, oMasterLC, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    readerMLC = MasterLCDA.InsertUpdate(tc, oMasterLC, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReaderMlc = new NullHandler(readerMLC);
                if (readerMLC.Read())
                {
                    oMasterLC.MasterLCID = oReaderMlc.GetInt32("MasterLCID");
                }
                readerMLC.Close();

                IDataReader reader;
                oImportMasterLC.MasterLCID = oMasterLC.MasterLCID;
                if (oImportMasterLC.ImportMasterLCID <= 0)
                {
                    reader = ImportMasterLCDA.InsertUpdate(tc, oImportMasterLC, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ImportMasterLCDA.InsertUpdate(tc, oImportMasterLC, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportMasterLC = new ImportMasterLC();
                    oImportMasterLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportMasterLC = new ImportMasterLC();
                oImportMasterLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportMasterLC;
        }
        public string Delete(ImportMasterLC oImportMasterLC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportMasterLCDA.Delete(tc, oImportMasterLC, EnumDBOperation.Delete, nUserId);
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
        public List<ImportMasterLC> Gets(Int64 nUserID)
        {
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportMasterLCDA.Gets(tc);
                oImportMasterLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportMasterLCs = new List<ImportMasterLC>();
                ImportMasterLC oImportMasterLC = new ImportMasterLC();
                oImportMasterLC.ErrorMessage = e.Message.Split('~')[0];
                oImportMasterLCs.Add(oImportMasterLC);
                #endregion
            }
            return oImportMasterLCs;
        }
        public ImportMasterLC Get(int id, Int64 nUserId)
        {
            ImportMasterLC oImportMasterLC = new ImportMasterLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportMasterLCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportMasterLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportMasterLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportMasterLC;
        }
        public List<ImportMasterLC> Gets(int nImportLCID, Int64 nUserID)
        {
            List<ImportMasterLC> oImportMasterLCs = new List<ImportMasterLC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportMasterLCDA.Gets(tc, nImportLCID);
                oImportMasterLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportMasterLCs = new List<ImportMasterLC>();
                ImportMasterLC oImportMasterLC = new ImportMasterLC();
                oImportMasterLC.ErrorMessage = e.Message.Split('~')[0];
                oImportMasterLCs.Add(oImportMasterLC);
                #endregion
            }
            return oImportMasterLCs;
        }
        #endregion
    }
}
