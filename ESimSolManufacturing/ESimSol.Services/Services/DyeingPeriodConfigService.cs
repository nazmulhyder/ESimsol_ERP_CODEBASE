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
    public class DyeingPeriodConfigService : MarshalByRefObject, IDyeingPeriodConfigService
    {
        #region Private functions and declaration
        private DyeingPeriodConfig MapObject(NullHandler oReader)
        {
            DyeingPeriodConfig oDyeingPeriodConfig = new DyeingPeriodConfig();
            oDyeingPeriodConfig.DyeingPeriodConfigID = oReader.GetInt32("DyeingPeriodConfigID");
            oDyeingPeriodConfig.ProductID = oReader.GetInt32("ProductID");
            oDyeingPeriodConfig.DyeingCapacityID = oReader.GetInt32("DyeingCapacityID");
            oDyeingPeriodConfig.Remarks = oReader.GetString("Remarks");
            oDyeingPeriodConfig.ReqDyeingPeriod = oReader.GetDouble("ReqDyeingPeriod");
            oDyeingPeriodConfig.ProductName = oReader.GetString("ProductName");
            oDyeingPeriodConfig.ProductCode = oReader.GetString("ProductCode");
            oDyeingPeriodConfig.BaseProductName = oReader.GetString("BaseProductName");
            oDyeingPeriodConfig.DyeingType = (EumDyeingType)oReader.GetInt32("DyeingType");
            return oDyeingPeriodConfig;
        }

        private DyeingPeriodConfig CreateObject(NullHandler oReader)
        {
            DyeingPeriodConfig oDyeingPeriodConfigConfig = new DyeingPeriodConfig();
            oDyeingPeriodConfigConfig = MapObject(oReader);
            return oDyeingPeriodConfigConfig;
        }
        private List<DyeingPeriodConfig> CreateObjects(IDataReader oReaded)
        {
            List<DyeingPeriodConfig> oDyeingPeriodConfigs = new List<DyeingPeriodConfig>();
            NullHandler oHandler = new NullHandler(oReaded);
            while (oReaded.Read())
            {
                DyeingPeriodConfig Item = CreateObject(oHandler);
                oDyeingPeriodConfigs.Add(Item);
            }
            return oDyeingPeriodConfigs;
        }
        #endregion
        #region Interface implementation

        public DyeingPeriodConfig Save(DyeingPeriodConfig oDyeingPeriodConfig, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDyeingPeriodConfig.DyeingPeriodConfigID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DyeingPeriodConfig, EnumRoleOperationType.Add);
                    reader = DyeingPeriodConfigDA.InsertUpdate(tc, oDyeingPeriodConfig, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DyeingPeriodConfig, EnumRoleOperationType.Edit);
                    reader = DyeingPeriodConfigDA.InsertUpdate(tc, oDyeingPeriodConfig, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingPeriodConfig = new DyeingPeriodConfig();
                    oDyeingPeriodConfig = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DyeingPeriodConfig. Because of " + e.Message, e);
                #endregion
            }
            return oDyeingPeriodConfig;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DyeingPeriodConfig oDyeingPeriodConfig = new DyeingPeriodConfig();
                oDyeingPeriodConfig.DyeingPeriodConfigID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.DyeingPeriodConfig, EnumRoleOperationType.Delete);
                DyeingPeriodConfigDA.Delete(tc, oDyeingPeriodConfig, EnumDBOperation.Delete, nUserId);
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
        public DyeingPeriodConfig Get(int id, Int64 nUserId)
        {
            DyeingPeriodConfig oDyeingPeriodConfig = new DyeingPeriodConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DyeingPeriodConfigDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingPeriodConfig = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DyeingPeriodConfig", e);
                #endregion
            }
            return oDyeingPeriodConfig;
        }
        public List<DyeingPeriodConfig> Gets(Int64 nUserID)
        {
            List<DyeingPeriodConfig> oDyeingPeriodConfigs = new List<DyeingPeriodConfig>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingPeriodConfigDA.Gets(tc);
                oDyeingPeriodConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingPeriodConfig", e);
                #endregion
            }
            return oDyeingPeriodConfigs;
        }
        public List<DyeingPeriodConfig> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingPeriodConfig> oDyeingPeriodConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingPeriodConfigDA.Gets(tc, sSQL);
                oDyeingPeriodConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingPeriodConfig", e);
                #endregion
            }
            return oDyeingPeriodConfigs;
        }

        #endregion

    }
}
