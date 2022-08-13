using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{

    public class ClientOperationSettingService : MarshalByRefObject, IClientOperationSettingService
    {
        #region Private functions and declaration
        private ClientOperationSetting MapObject(NullHandler oReader)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting.ClientOperationSettingID = oReader.GetInt32("ClientOperationSettingID");
            oClientOperationSetting.OperationType = (EnumOperationType)oReader.GetInt32("OperationType");
            oClientOperationSetting.OperationTypeInInt = oReader.GetInt32("OperationType");
            oClientOperationSetting.DataType = (EnumDataType)oReader.GetInt32("DataType");
            oClientOperationSetting.DataTypeInInt = oReader.GetInt32("DataType");
            oClientOperationSetting.Value = oReader.GetString("Value");
            return oClientOperationSetting;
        }

        private ClientOperationSetting CreateObject(NullHandler oReader)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = MapObject(oReader);
            return oClientOperationSetting;
        }

        private List<ClientOperationSetting> CreateObjects(IDataReader oReader)
        {
            List<ClientOperationSetting> oClientOperationSetting = new List<ClientOperationSetting>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ClientOperationSetting oItem = CreateObject(oHandler);
                oClientOperationSetting.Add(oItem);
            }
            return oClientOperationSetting;
        }

        #endregion

        #region Interface implementation
        public ClientOperationSettingService() { }

        public ClientOperationSetting Save(ClientOperationSetting oClientOperationSetting, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oClientOperationSetting.ClientOperationSettingID <= 0)
                {
                    reader = ClientOperationSettingDA.InsertUpdate(tc, oClientOperationSetting, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ClientOperationSettingDA.InsertUpdate(tc, oClientOperationSetting, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClientOperationSetting = new ClientOperationSetting();
                    oClientOperationSetting = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ClientOperationSetting. Because of " + e.Message, e);
                #endregion
            }
            return oClientOperationSetting;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ClientOperationSetting, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ClientOperationSetting", id);
                oClientOperationSetting.ClientOperationSettingID = id;
                ClientOperationSettingDA.Delete(tc, oClientOperationSetting, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ClientOperationSetting. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ClientOperationSetting Get(int nCOSid, Int64 nUserId)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ClientOperationSettingDA.Get(tc, nCOSid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClientOperationSetting = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ClientOperationSetting", e);
                #endregion
            }

            return oClientOperationSetting;
        }


        public ClientOperationSetting GetByOperationType(int eOperationTypeid, Int64 nUserId)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ClientOperationSettingDA.GetByOperationType(tc, eOperationTypeid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClientOperationSetting = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ClientOperationSetting", e);
                #endregion
            }

            return oClientOperationSetting;
        }

        public List<ClientOperationSetting> Gets(Int64 nUserID)
        {
            List<ClientOperationSetting> oClientOperationSetting = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ClientOperationSettingDA.Gets(tc);
                oClientOperationSetting = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ClientOperationSetting", e);
                #endregion
            }

            return oClientOperationSetting;
        }

        public List<ClientOperationSetting> Gets(string sSQL, Int64 nUserID)
        {
            List<ClientOperationSetting> oClientOperationSetting = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ClientOperationSettingDA.Gets(tc, sSQL);
                oClientOperationSetting = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ClientOperationSetting", e);
                #endregion
            }

            return oClientOperationSetting;
        }
        #endregion
    }
    

}
