using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class ImportLCCodeService : MarshalByRefObject, IImportLCCodeService
    {
        #region Private functions and declaration
        private ImportLCCode MapObject(NullHandler oReader)
        {
            ImportLCCode oImportLCCodeDetail = new ImportLCCode();
            oImportLCCodeDetail.ImportLCCodeID = oReader.GetInt32("ImportLCCodeID");
            oImportLCCodeDetail.LCCode = oReader.GetString("LCCode");
            oImportLCCodeDetail.LCNature = oReader.GetString("LCNature");           
            oImportLCCodeDetail.Remarks = oReader.GetString("Remarks");
            oImportLCCodeDetail.DBUserID = oReader.GetInt32("DBUserID");
            oImportLCCodeDetail.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            
            return oImportLCCodeDetail;
        }

        private List<ImportLCCode> CreateObjects(IDataReader oReader)
        {
            List<ImportLCCode> oImportLCCodes = new List<ImportLCCode>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCCode oType = CreateObject(oHandler);
                oImportLCCodes.Add(oType);
            }
            return oImportLCCodes;
        }

        private ImportLCCode CreateObject(NullHandler oReader)
        {
            ImportLCCode oImportLCCode = new ImportLCCode();
            oImportLCCode = MapObject(oReader);
            return oImportLCCode;
        }
        #endregion

        #region Interface implementation
        public ImportLCCodeService() { }

        public List<ImportLCCode> Gets(Int64 nUserID)
        {
            List<ImportLCCode> oImportLCCode = new List<ImportLCCode>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCCodeDA.Gets(tc);
                oImportLCCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import LC Codes", e);
                #endregion
            }

            return oImportLCCode;
        }
     
        public ImportLCCode Get(int id, Int64 nUserId)
        {
            ImportLCCode oImportLCCode = new ImportLCCode();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCCodeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCCode = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Import LC Code", e);
                #endregion
            }

            return oImportLCCode;
        }

        public ImportLCCode Save(ImportLCCode oImportLCCode, Int64 nUserID)
        {
            TransactionContext tc = null;
            oImportLCCode.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportLCCode.ImportLCCodeID <= 0)
                {
                    reader = ImportLCCodeDA.InsertUpdate(tc, oImportLCCode, EnumDBOperation.Insert, nUserID); //
                }
                else
                {
                    reader = ImportLCCodeDA.InsertUpdate(tc, oImportLCCode, EnumDBOperation.Update, nUserID);//
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCCode = new ImportLCCode();
                    oImportLCCode = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportLCCode = new ImportLCCode();
                oImportLCCode.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oImportLCCode;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportLCCode oImportLCCode = new ImportLCCode();
                oImportLCCode.ImportLCCodeID = id;
                ImportLCCodeDA.Delete(tc, oImportLCCode, EnumDBOperation.Delete, nUserId);
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
            return "Data delete successfully";
        }
        #endregion
    }
}
