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
    public class DataCollectionSetupService : MarshalByRefObject, IDataCollectionSetupService
    {
        #region Private functions and declaration
        private DataCollectionSetup MapObject(NullHandler oReader)
        {
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            oDataCollectionSetup.DataCollectionSetupID = oReader.GetInt32("DataCollectionSetupID");
            oDataCollectionSetup.DataReferenceType = (EnumDataReferenceType)oReader.GetInt32("DataReferenceType");
            oDataCollectionSetup.DataReferenceTypeInInt = oReader.GetInt32("DataReferenceType");
            oDataCollectionSetup.DataReferenceID = oReader.GetInt32("DataReferenceID");
            oDataCollectionSetup.DataSetupType = (EnumDataSetupType)oReader.GetInt32("DataSetupType");
            oDataCollectionSetup.DataSetupTypeInInt = oReader.GetInt32("DataSetupType");
            oDataCollectionSetup.DataGenerateType = (EnumDataGenerateType)oReader.GetInt32("DataGenerateType");
            oDataCollectionSetup.DataGenerateTypeInInt = oReader.GetInt32("DataGenerateType");
            oDataCollectionSetup.QueryForValue = oReader.GetString("QueryForValue");
            oDataCollectionSetup.ReferenceValueFields = oReader.GetString("ReferenceValueFields");
            oDataCollectionSetup.FixedText = oReader.GetString("FixedText");
            oDataCollectionSetup.Note = oReader.GetString("Note");
            return oDataCollectionSetup;
        }

        private DataCollectionSetup CreateObject(NullHandler oReader)
        {
            DataCollectionSetup oDataCollectionSetup = new DataCollectionSetup();
            oDataCollectionSetup = MapObject(oReader);
            return oDataCollectionSetup;
        }

        private List<DataCollectionSetup> CreateObjects(IDataReader oReader)
        {
            List<DataCollectionSetup> oDataCollectionSetup = new List<DataCollectionSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DataCollectionSetup oItem = CreateObject(oHandler);
                oDataCollectionSetup.Add(oItem);
            }
            return oDataCollectionSetup;
        }

        #endregion

        #region Interface implementation
        public DataCollectionSetupService() { }

        public DataCollectionSetup Save(DataCollectionSetup oDataCollectionSetup, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<DataCollectionSetup> _oDataCollectionSetups = new List<DataCollectionSetup>();
            oDataCollectionSetup.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DataCollectionSetupDA.InsertUpdate(tc, oDataCollectionSetup, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDataCollectionSetup = new DataCollectionSetup();
                    oDataCollectionSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDataCollectionSetup.ErrorMessage = e.Message;
                #endregion
            }
            return oDataCollectionSetup;
        }
        
        public DataCollectionSetup Get(int nDataCollectionSetupID, Int64 nUserId)
        {
            DataCollectionSetup oAccountHead = new DataCollectionSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DataCollectionSetupDA.Get(tc, nDataCollectionSetupID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DataCollectionSetup", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DataCollectionSetup> Gets(int nDataReferenceID, int nDataReferenceType, Int64 nUserID)
        {
            List<DataCollectionSetup> oDataCollectionSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DataCollectionSetupDA.Gets(tc, nDataReferenceID, nDataReferenceType);
                oDataCollectionSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataCollectionSetup", e);
                #endregion
            }

            return oDataCollectionSetup;
        }

        public List<DataCollectionSetup> GetsByIntegrationSetup(int nIntegrationSetupID, int nDataReferenceType, Int64 nUserID)
        {
            List<DataCollectionSetup> oDataCollectionSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DataCollectionSetupDA.GetsByIntegrationSetup(tc, nIntegrationSetupID, nDataReferenceType);
                oDataCollectionSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataCollectionSetup", e);
                #endregion
            }

            return oDataCollectionSetup;
        }

        public List<DataCollectionSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<DataCollectionSetup> oDataCollectionSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DataCollectionSetupDA.Gets(tc, sSQL);
                oDataCollectionSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataCollectionSetup", e);
                #endregion
            }

            return oDataCollectionSetup;
        }
        #endregion
    }
}
