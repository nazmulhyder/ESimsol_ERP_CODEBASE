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
    public class ImportSetupService : MarshalByRefObject, IImportSetupService
    {
        #region Private functions and declaration
        private ImportSetup MapObject(NullHandler oReader)
        {
            ImportSetup oImportSetup = new ImportSetup();
            oImportSetup.ImportSetupID = oReader.GetInt32("ImportSetupID");
            oImportSetup.BUID = oReader.GetInt32("BUID");
            oImportSetup.IsApplyPO = oReader.GetBoolean("IsApplyPO");
            oImportSetup.IsApplyTT = oReader.GetBoolean("IsApplyTT");
            oImportSetup.IsApplyMasterLC = oReader.GetBoolean("IsApplyMasterLC");
            oImportSetup.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportSetup.Activity = oReader.GetBoolean("Activity");
            oImportSetup.BUName = oReader.GetString("BUName");
            oImportSetup.Note = oReader.GetString("Note");
            oImportSetup.CoverNoteNumber = oReader.GetString("CoverNoteNumber");
            oImportSetup.Currency = oReader.GetString("Currency");
            oImportSetup.CurrencyName = oReader.GetString("CurrencyName");
            oImportSetup.IsFreightRate = oReader.GetBoolean("IsFreightRate");
            oImportSetup.IsApplyRateOn = oReader.GetBoolean("IsApplyRateOn");
            oImportSetup.FileType = (EnumImportFileType)oReader.GetInt32("FileType");
            oImportSetup.DaysCalculateOn = (EnumImportDateCalBy)oReader.GetInt32("DaysCalculateOn");
            oImportSetup.FileTypeInt = oReader.GetInt32("FileType");
            oImportSetup.DaysCalculateOnInt = oReader.GetInt32("DaysCalculateOn");
            oImportSetup.ShipmentDay = oReader.GetInt32("ShipmentDay");
            oImportSetup.ExpireDay = oReader.GetInt32("ExpireDay");
            oImportSetup.CoverNoteDate = oReader.GetDateTime("CoverNoteDate");

            
            return oImportSetup;
        }

        private ImportSetup CreateObject(NullHandler oReader)
        {
            ImportSetup oImportSetup = new ImportSetup();
            oImportSetup = MapObject(oReader);
            return oImportSetup;
        }

        private List<ImportSetup> CreateObjects(IDataReader oReader)
        {
            List<ImportSetup> oImportSetups = new List<ImportSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportSetup oItem = CreateObject(oHandler);
                oImportSetups.Add(oItem);
            }
            return oImportSetups;
        }

        #endregion

        #region Interface implementation
        public ImportSetupService() { }


        public ImportSetup Save(ImportSetup oImportSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region ImportSetup
                IDataReader reader;
                if (oImportSetup.ImportSetupID <= 0)
                {
                    reader = ImportSetupDA.InsertUpdate(tc, oImportSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ImportSetupDA.InsertUpdate(tc, oImportSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportSetup = new ImportSetup();
                    oImportSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportSetup = new ImportSetup();
                oImportSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportSetup;
        }
      
        public String Delete(ImportSetup oImportSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportSetupDA.Delete(tc, oImportSetup, EnumDBOperation.Delete, nUserID);
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
        public ImportSetup Get(int id, Int64 nUserId)
        {
            ImportSetup oImportSetup = new ImportSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportSetup;
        }
        public ImportSetup GetByBU(int nBUID, Int64 nUserId)
        {
            ImportSetup oImportSetup = new ImportSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportSetupDA.GetByBU(tc, nBUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportSetup;
        }
     

        public List<ImportSetup> Gets(Int64 nUserId)
        {
            List<ImportSetup> oImportSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportSetupDA.Gets(tc);
                oImportSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportSetups;
        }
        public List<ImportSetup> Gets(int nBUID,Int64 nUserId)
        {
            List<ImportSetup> oImportSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportSetupDA.Gets(tc, nBUID);
                oImportSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportSetups;
        }

        public ImportSetup Activate(ImportSetup oImportSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportSetupDA.Activate(tc, oImportSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportSetup = new ImportSetup();
                oImportSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportSetup;
        }
    

        #endregion
    }
}