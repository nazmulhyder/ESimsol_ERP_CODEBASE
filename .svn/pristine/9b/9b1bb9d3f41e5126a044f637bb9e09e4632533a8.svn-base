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
    public class ImportCnfService : MarshalByRefObject, IImportCnfService
    {
        #region Private functions and declaration
        private ImportCnf MapObject(NullHandler oReader)
        {
            ImportCnf oImportCnf = new ImportCnf();
            oImportCnf.ImportCnfID = oReader.GetInt32("ImportCnfID");
            oImportCnf.FileNo = oReader.GetString("FileNo");
            oImportCnf.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportCnf.ContractorID = oReader.GetInt32("ContractorID");
            oImportCnf.ContractorName = oReader.GetString("ContractorName");
            oImportCnf.SendDate = oReader.GetDateTime("SendDate");
            oImportCnf.Note = oReader.GetString("Note");

          
            return oImportCnf;
        }
        private ImportCnf CreateObject(NullHandler oReader)
        {
            ImportCnf oImportCnf = new ImportCnf();
            oImportCnf = MapObject(oReader);
            return oImportCnf;
        }
        private List<ImportCnf> CreateObjects(IDataReader oReader)
        {
            List<ImportCnf> oImportCnf = new List<ImportCnf>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportCnf oItem = CreateObject(oHandler);
                oImportCnf.Add(oItem);
            }
            return oImportCnf;
        }

        #endregion

        #region Interface implementation
        public ImportCnf Save(ImportCnf oImportCnf, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportCnf.ImportCnfID <= 0)
                {
                    reader = ImportCnfDA.InsertUpdate(tc, oImportCnf, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportCnfDA.InsertUpdate(tc, oImportCnf, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportCnf = new ImportCnf();
                    oImportCnf = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportCnf.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportCnf;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportCnf oImportCnf = new ImportCnf();
                oImportCnf.ImportCnfID = id;
                //DBTableReferenceDA.HasReference(tc, "ImportCnf", id);
                ImportCnfDA.Delete(tc, oImportCnf, EnumDBOperation.Delete, nUserId);
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
        public List<ImportCnf> Gets(Int64 nUserID)
        {
            List<ImportCnf> oImportCnfs = new List<ImportCnf>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportCnfDA.Gets(tc);
                oImportCnfs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportCnfs = new List<ImportCnf>();
                ImportCnf oImportCnf = new ImportCnf();
                oImportCnf.ErrorMessage = e.Message.Split('~')[0];
                oImportCnfs.Add(oImportCnf);
                #endregion
            }
            return oImportCnfs;
        }
        public ImportCnf Get(int id, Int64 nUserId)
        {
            ImportCnf oImportCnf = new ImportCnf();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportCnfDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportCnf = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportCnf.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportCnf;
        }
        public ImportCnf GetBy(int nImportInvoiceID, Int64 nUserId)
        {
            ImportCnf oImportCnf = new ImportCnf();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportCnfDA.GetBy(tc, nImportInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportCnf = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportCnf.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportCnf;
        }
      
        #endregion
    }
}
