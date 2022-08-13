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
    public class ImportFormatService : MarshalByRefObject, IImportFormatService
    {
        #region Private functions and declaration
        private ImportFormat MapObject(NullHandler oReader)
        {
            ImportFormat oImportFormat = new ImportFormat();
            oImportFormat.ImportFormatID = oReader.GetInt32("ImportFormatID");
            oImportFormat.ImportFormatType = (EnumImportFormatType)oReader.GetInt16("ImportFormatType");
            oImportFormat.AttatchmentName = oReader.GetString("AttatchmentName");
            oImportFormat.AttatchFile = oReader.GetBytes("AttatchFile");
            oImportFormat.FileType = oReader.GetString("FileType");
            oImportFormat.Remarks = oReader.GetString("Remarks");
            
            return oImportFormat;
        }

        private ImportFormat CreateObject(NullHandler oReader)
        {
            ImportFormat oImportFormat = new ImportFormat();
            oImportFormat = MapObject(oReader);
            return oImportFormat;
        }

        private List<ImportFormat> CreateObjects(IDataReader oReader)
        {
            List<ImportFormat> oImportFormats = new List<ImportFormat>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportFormat oItem = CreateObject(oHandler);
                oImportFormats.Add(oItem);
            }
            return oImportFormats;
        }

        #endregion

        #region Interface implementation
        public ImportFormatService() { }

        public ImportFormat Save(ImportFormat oImportFormat, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                ImportFormat oTempImportFormat = new ImportFormat();
                oTempImportFormat.AttatchFile = oImportFormat.AttatchFile;
                oImportFormat.AttatchFile = null;

                if (oImportFormat.ImportFormatID <= 0)
                {
                    reader = ImportFormatDA.InsertUpdate(tc, oImportFormat, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportFormatDA.InsertUpdate(tc, oImportFormat, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportFormat = new ImportFormat();
                    oImportFormat = CreateObject(oReader);
                }
                reader.Close();

                oTempImportFormat.ImportFormatID = oImportFormat.ImportFormatID;
                if (oTempImportFormat.AttatchFile != null)
                {
                    ImportFormatDA.UpdatePhoto(tc, oTempImportFormat);
                    oImportFormat.AttatchFile = oTempImportFormat.AttatchFile;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportFormat.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ImportFormat. Because of " + e.Message, e);
                #endregion
            }

            return oImportFormat;
        }



        public string Delete(int nImportFormatID, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportFormat oImportFormat = new ImportFormat();
                oImportFormat.ImportFormatID = nImportFormatID;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ImportFormat, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "ImportFormat", nImportFormatID);
                ImportFormatDA.Delete(tc, oImportFormat, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public List<ImportFormat> Gets(string sSQL, int nUserID)
        {
            List<ImportFormat> oImportFormats = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportFormatDA.Gets(tc, sSQL);
                oImportFormats = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportFormat", e);
                #endregion
            }

            return oImportFormats;
        }        

        public List<ImportFormat> Gets(int nUserId)
        {
            List<ImportFormat> oImportFormats = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportFormatDA.Gets(tc);
                oImportFormats = CreateObjects(reader);
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

            return oImportFormats;
        }

        public ImportFormat Get(int id, int nUserId)
        {
            ImportFormat oImportFormat = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportFormatDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportFormat = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportFormat;
        }
        public ImportFormat GetByType(EnumImportFormatType eIFT, int nUserId)
        {
            ImportFormat oImportFormat = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportFormatDA.GetByType(tc, eIFT);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportFormat = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportFormat;
        }
        public ImportFormat GetWithAttachFile(int id, int nUserId)
        {
            ImportFormat oImportFormat = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportFormatDA.GetWithAttachFile(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportFormat = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oImportFormat;
        }

        #endregion
    }
}
