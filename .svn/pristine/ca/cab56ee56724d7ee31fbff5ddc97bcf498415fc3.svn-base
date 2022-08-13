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
    public class ImportLCClauseSetupService : MarshalByRefObject, IImportLCClauseSetupService
    {
        #region Private functions and declaration
        private ImportLCClauseSetup MapObject(NullHandler oReader)
        {
            ImportLCClauseSetup oImportLCClauseSetup = new ImportLCClauseSetup();
            oImportLCClauseSetup.ImportLCClauseSetupID = oReader.GetInt32("ImportLCClauseSetupID");
            oImportLCClauseSetup.BUID = oReader.GetInt32("BUID");
            oImportLCClauseSetup.Activity = oReader.GetBoolean("Activity");
            oImportLCClauseSetup.Clause = oReader.GetString("Clause");
            oImportLCClauseSetup.LCPaymentType = (EnumLCPaymentType)oReader.GetInt16("LCPaymentType");
            oImportLCClauseSetup.LCAppType = (EnumLCAppType)oReader.GetInt16("LCAppType");
            oImportLCClauseSetup.ProductType = (EnumProductNature)oReader.GetInt16("ProductType");
            oImportLCClauseSetup.IsMandatory = oReader.GetBoolean("IsMandatory");

            return oImportLCClauseSetup;
        }

        private ImportLCClauseSetup CreateObject(NullHandler oReader)
        {
            ImportLCClauseSetup oImportLCClause = new ImportLCClauseSetup();
            oImportLCClause = MapObject(oReader);
            return oImportLCClause;
        }

        private List<ImportLCClauseSetup> CreateObjects(IDataReader oReader)
        {
            List<ImportLCClauseSetup> lstImportLCClauses = new List<ImportLCClauseSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCClauseSetup oItem = CreateObject(oHandler);
                lstImportLCClauses.Add(oItem);
            }
            return lstImportLCClauses;
        }
        #endregion

        #region Interface implementation
        public ImportLCClauseSetupService() { }

        public ImportLCClauseSetup Save(ImportLCClauseSetup oImportLCClause, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oImportLCClause.ImportLCClauseSetupID <= 0)
                {
                  reader=  ImportLCClauseSetupDA.InsertUPdate(tc, oImportLCClause, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportLCClauseSetupDA.InsertUPdate(tc, oImportLCClause, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCClause = new ImportLCClauseSetup();
                    oImportLCClause = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLCClause = new ImportLCClauseSetup();
                oImportLCClause.ErrorMessage = e.Message;
                #endregion
            }
            
            return oImportLCClause;
        }
        public string Delete(ImportLCClauseSetup oImportLCClause, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                ImportLCClauseSetupDA.Delete(tc, oImportLCClause,EnumDBOperation.Delete,  nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message;
            }
            return Global.DeleteMessage;
        }
        public ImportLCClauseSetup Get(int id, Int64 nUserID)
        {
            ImportLCClauseSetup oImportLCClause = new ImportLCClauseSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCClauseSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCClause = CreateObject(oReader);
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

            return oImportLCClause;
        }
        public List<ImportLCClauseSetup> Gets(Int64 nUserID)
        {
            List<ImportLCClauseSetup> lstImportLCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCClauseSetupDA.Gets(tc);
                lstImportLCClause = CreateObjects(reader);
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

            return lstImportLCClause;
        }


        public List<ImportLCClauseSetup> GetsActiveImportLCClause(Int64 nUserID)
        {
            List<ImportLCClauseSetup> lstImportLCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCClauseSetupDA.GetsActiveImportLCClauseSetup(tc);
                lstImportLCClause = CreateObjects(reader);
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

            return lstImportLCClause;
        }

        public List<ImportLCClauseSetup> Gets(int nBUID, Int64 nUserID)
        {
            List<ImportLCClauseSetup> lstImportLCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCClauseSetupDA.Gets(tc, nBUID);
                lstImportLCClause = CreateObjects(reader);
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

            return lstImportLCClause;
        }
        public List<ImportLCClauseSetup> GetsWithSQL(string sSQL, Int64 nUserID)
        {
            List<ImportLCClauseSetup> lstImportLCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCClauseSetupDA.GetsWithSQL(tc, sSQL);
                lstImportLCClause = CreateObjects(reader);
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

            return lstImportLCClause;
        }

        public ImportLCClauseSetup IUD(ImportLCClauseSetup oImportLCClauseSetup, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = ImportLCClauseSetupDA.IUD(tc, oImportLCClauseSetup, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oImportLCClauseSetup = new ImportLCClauseSetup();
                        oImportLCClauseSetup = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = ImportLCClauseSetupDA.IUD(tc, oImportLCClauseSetup, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oImportLCClauseSetup.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oImportLCClauseSetup = new ImportLCClauseSetup();
                oImportLCClauseSetup.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oImportLCClauseSetup;
        }

        #endregion
    }


}
