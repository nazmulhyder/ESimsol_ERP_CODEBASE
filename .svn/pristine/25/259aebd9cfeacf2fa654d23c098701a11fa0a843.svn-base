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
    public class SubledgerRefConfigService : MarshalByRefObject, ISubledgerRefConfigService
    {
        #region Private functions and declaration
        private SubledgerRefConfig MapObject(NullHandler oReader)
        {
            SubledgerRefConfig oSubledgerRefConfig = new SubledgerRefConfig();
            oSubledgerRefConfig.SubledgerRefConfigID = oReader.GetInt32("SubledgerRefConfigID");
            oSubledgerRefConfig.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oSubledgerRefConfig.SubledgerID = oReader.GetInt32("SubledgerID");
            oSubledgerRefConfig.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oSubledgerRefConfig.IsOrderRefApply = oReader.GetBoolean("IsOrderRefApply");
            oSubledgerRefConfig.AccountHeadName = oReader.GetString("AccountHeadName");
            oSubledgerRefConfig.AccountCode = oReader.GetString("AccountCode");
            oSubledgerRefConfig.SubledgerName = oReader.GetString("SubledgerName");
            oSubledgerRefConfig.SubledgerCode = oReader.GetString("SubledgerCode");
            return oSubledgerRefConfig;
        }
        private SubledgerRefConfig CreateObject(NullHandler oReader)
        {
            SubledgerRefConfig oSubledgerRefConfig = new SubledgerRefConfig();
            oSubledgerRefConfig = MapObject(oReader);
            return oSubledgerRefConfig;
        }
        private List<SubledgerRefConfig> CreateObjects(IDataReader oReader)
        {
            List<SubledgerRefConfig> oSubledgerRefConfig = new List<SubledgerRefConfig>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SubledgerRefConfig oItem = CreateObject(oHandler);
                oSubledgerRefConfig.Add(oItem);
            }
            return oSubledgerRefConfig;
        }
        #endregion

        #region Interface implementation

        public SubledgerRefConfig Save(SubledgerRefConfig oSubledgerRefConfig, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SubledgerRefConfigDA.InsertUpdate(tc, oSubledgerRefConfig, EnumDBOperation.Insert, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSubledgerRefConfig.ErrorMessage = e.Message;
                #endregion
            }
            return oSubledgerRefConfig;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SubledgerRefConfig oSubledgerRefConfig = new SubledgerRefConfig();
                oSubledgerRefConfig.SubledgerRefConfigID = id;
                //SubledgerRefConfigDA.Delete(tc, oSubledgerRefConfig, EnumDBOperation.Delete, nUserId);
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
            return "Data delete successfully";
        }

        public SubledgerRefConfig Get(int nAccountHeadID, int nUserId)
        {
            SubledgerRefConfig oAccountHead = new SubledgerRefConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SubledgerRefConfigDA.Get(tc, nAccountHeadID);
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
                throw new ServiceException("Failed to Get SubledgerRefConfig", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<SubledgerRefConfig> Gets(int nSubLedgerID, int nUserID)
        {
            List<SubledgerRefConfig> oSubledgerRefConfigs = new List<SubledgerRefConfig>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SubledgerRefConfigDA.Gets(tc, nSubLedgerID);
                oSubledgerRefConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SubledgerRefConfig", e);
                #endregion
            }

            return oSubledgerRefConfigs;
        }
        public List<SubledgerRefConfig> Gets(string sSQL, int nUserID)
        {
            List<SubledgerRefConfig> oSubledgerRefConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SubledgerRefConfigDA.Gets(tc, sSQL);
                oSubledgerRefConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SubledgerRefConfig", e);
                #endregion
            }

            return oSubledgerRefConfigs;
        }
        #endregion
    }
}
