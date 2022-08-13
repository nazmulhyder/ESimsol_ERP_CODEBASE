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
    public class MasterLCMappingService : MarshalByRefObject, IMasterLCMappingService
    {
        #region Private functions and declaration
        private MasterLCMapping MapObject(NullHandler oReader)
        {
            MasterLCMapping oMasterLCMapping = new MasterLCMapping();
            oMasterLCMapping.MasterLCMappingID = oReader.GetInt32("MasterLCMappingID");
            oMasterLCMapping.ExportLCID = oReader.GetInt32("ExportLCID");
            oMasterLCMapping.MasterLCID = oReader.GetInt32("MasterLCID");
            oMasterLCMapping.ContractorID = oReader.GetInt32("ContractorID");
            oMasterLCMapping.MasterLCNo = oReader.GetString("MasterLCNo");
            oMasterLCMapping.MasterLCDate = oReader.GetDateTime("MasterLCDate");
            oMasterLCMapping.MasterLCType = (EnumMasterLCType)oReader.GetInt32("MasterLCType");
            oMasterLCMapping.MasterLCTypeInInt = oReader.GetInt32("MasterLCType");
            return oMasterLCMapping;
        }
        private MasterLCMapping CreateObject(NullHandler oReader)
        {
            MasterLCMapping oMasterLCMapping = new MasterLCMapping();
            oMasterLCMapping = MapObject(oReader);
            return oMasterLCMapping;
        }
        private List<MasterLCMapping> CreateObjects(IDataReader oReader)
        {
            List<MasterLCMapping> oMasterLCMapping = new List<MasterLCMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterLCMapping oItem = CreateObject(oHandler);
                oMasterLCMapping.Add(oItem);
            }
            return oMasterLCMapping;
        }
        #endregion

        #region Interface implementation
        public MasterLCMapping Save(MasterLCMapping oMasterLCMapping, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMasterLCMapping.MasterLCMappingID <= 0)
                {
                    reader = MasterLCMappingDA.InsertUpdate(tc, oMasterLCMapping, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MasterLCMappingDA.InsertUpdate(tc, oMasterLCMapping, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLCMapping = new MasterLCMapping();
                    oMasterLCMapping = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMasterLCMapping = new MasterLCMapping();
                oMasterLCMapping.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMasterLCMapping;
        }
        public MasterLCMapping SaveWithMasterLC(MasterLCMapping oMasterLCMapping, Int64 nUserId)
        {
            MasterLC oMasterLC = new MasterLC();
            oMasterLC = oMasterLCMapping.MasterLCObj;
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
                oMasterLCMapping.MasterLCID = oMasterLC.MasterLCID;
                if (oMasterLCMapping.MasterLCMappingID <= 0)
                {
                    reader = MasterLCMappingDA.InsertUpdate(tc, oMasterLCMapping, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MasterLCMappingDA.InsertUpdate(tc, oMasterLCMapping, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLCMapping = new MasterLCMapping();
                    oMasterLCMapping = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMasterLCMapping = new MasterLCMapping();
                oMasterLCMapping.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMasterLCMapping;
        }
        public string Delete(MasterLCMapping oMasterLCMapping, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                MasterLCMappingDA.Delete(tc, oMasterLCMapping, EnumDBOperation.Delete, nUserId);
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
        public MasterLCMapping Get(int id, Int64 nUserId)
        {
            MasterLCMapping oAccountHead = new MasterLCMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MasterLCMappingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MasterLCMapping", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MasterLCMapping> Gets(int nELCID, Int64 nUserID)
        {
            List<MasterLCMapping> oMasterLCMappings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MasterLCMappingDA.Gets(tc, nELCID);
                oMasterLCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCMapping", e);
                #endregion
            }
            return oMasterLCMappings;
        }
        public List<MasterLCMapping> Gets(string sSQL, Int64 nUserID)
        {
            List<MasterLCMapping> oMasterLCMappings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MasterLCMappingDA.Gets(tc, sSQL);
                oMasterLCMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCMapping", e);
                #endregion
            }
            return oMasterLCMappings;
        }
        #endregion
    }
}
