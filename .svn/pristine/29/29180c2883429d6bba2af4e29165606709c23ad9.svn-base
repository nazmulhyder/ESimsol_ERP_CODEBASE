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
    public class ACConfigService : MarshalByRefObject, IACConfigService
    {
        #region Private functions and declaration
        private ACConfig MapObject(NullHandler oReader)
        {
            ACConfig oACConfig = new ACConfig();
            oACConfig.ACConfigID = oReader.GetInt32("ACConfigID");
            oACConfig.ConfigureType = (EnumConfigureType)oReader.GetInt32("ConfigureType");
            oACConfig.ConfigureTypeInInt = (int)oReader.GetInt32("ConfigureType");
            oACConfig.ConfigureValueType = (EnumConfigureValueType)oReader.GetInt32("ConfigureValueType");
            oACConfig.ConfigureValueTypeInInt = (int)oReader.GetInt32("ConfigureValueType");
            oACConfig.ConfigureValue = oReader.GetString("ConfigureValue");
            return oACConfig;
        }
        private ACConfig CreateObject(NullHandler oReader)
        {
            ACConfig oACConfig = new ACConfig();
            oACConfig = MapObject(oReader);
            return oACConfig;
        }
        private List<ACConfig> CreateObjects(IDataReader oReader)
        {
            List<ACConfig> oACConfig = new List<ACConfig>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ACConfig oItem = CreateObject(oHandler);
                oACConfig.Add(oItem);
            }
            return oACConfig;
        }
        #endregion

        #region Interface implementation
        public ACConfig Save(ACConfig oACConfig, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<ACConfig> oACConfigs = new List<ACConfig>();
                oACConfigs = oACConfig.ACConfigs;
                tc = TransactionContext.Begin(true);
                if (oACConfigs != null)
                {
                    foreach (ACConfig oItem in oACConfigs)
                    {
                        oItem.ConfigureType = (EnumConfigureType)oItem.ConfigureTypeInInt;
                        oItem.ConfigureValueType = (EnumConfigureValueType)oItem.ConfigureValueTypeInInt;                        
                        IDataReader reader;
                        if (oItem.ACConfigID <= 0)
                        {
                            reader = ACConfigDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = ACConfigDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oACConfig = new ACConfig();
                            oACConfig = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save ACConfig. Because of " + e.Message, e);
                #endregion
            }
            return oACConfig;
        }

        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ACConfig oACConfig = new ACConfig();
                oACConfig.ACConfigID = id;
                ACConfigDA.Delete(tc, oACConfig, EnumDBOperation.Delete, nUserId);
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

        public ACConfig Get(int id, int nUserId)
        {
            ACConfig oAccountHead = new ACConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ACConfigDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ACConfig", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ACConfig> Gets(int nUserID)
        {
            List<ACConfig> oACConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACConfigDA.Gets(tc);
                oACConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ACConfig", e);
                #endregion
            }
            return oACConfigs;
        }
        public List<ACConfig> Gets(string sSQL, int nUserID)
        {
            List<ACConfig> oACConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ACConfigDA.Gets(tc, sSQL);
                oACConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ACConfig", e);
                #endregion
            }

            return oACConfigs;
        }
        #endregion
    }
}
