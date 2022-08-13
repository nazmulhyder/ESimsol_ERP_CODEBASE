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
    public class VoucherMappingService : MarshalByRefObject, IVoucherMappingService
    {
        #region Private functions and declaration
        private VoucherMapping MapObject(NullHandler oReader)
        {
            VoucherMapping oVoucherMapping = new VoucherMapping();
            oVoucherMapping.VoucherMappingID = oReader.GetInt32("VoucherMappingID");
            oVoucherMapping.TableName = oReader.GetString("TableName");
            oVoucherMapping.PKColumnName = oReader.GetString("PKColumnName");
            oVoucherMapping.PKValue = oReader.GetInt32("PKValue");
            oVoucherMapping.VoucherSetup = (EnumVoucherSetup)oReader.GetInt32("VoucherSetup");
            oVoucherMapping.VoucherSetupInt = oReader.GetInt32("VoucherSetup");
            oVoucherMapping.VoucherID = oReader.GetInt32("VoucherID");
            return oVoucherMapping;
        }

        private VoucherMapping CreateObject(NullHandler oReader)
        {
            VoucherMapping oVoucherMapping = new VoucherMapping();
            oVoucherMapping = MapObject(oReader);
            return oVoucherMapping;
        }

        private List<VoucherMapping> CreateObjects(IDataReader oReader)
        {
            List<VoucherMapping> oVoucherMapping = new List<VoucherMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherMapping oItem = CreateObject(oHandler);
                oVoucherMapping.Add(oItem);
            }
            return oVoucherMapping;
        }

        #endregion

        #region Interface implementation
        public VoucherMappingService() { }

        public VoucherMapping Save(VoucherMapping oVoucherMapping, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);              
                #region VoucherMapping
                IDataReader reader;
                if (oVoucherMapping.VoucherMappingID <= 0)
                {
                 //   AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "VoucherMapping", EnumRoleOperationType.Add);
                    reader = VoucherMappingDA.InsertUpdate(tc, oVoucherMapping, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                   /// AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "VoucherMapping", EnumRoleOperationType.Edit);
                    reader = VoucherMappingDA.InsertUpdate(tc, oVoucherMapping, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherMapping = new VoucherMapping();
                    oVoucherMapping = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oVoucherMapping.ErrorMessage = Message;

                #endregion
            }
            return oVoucherMapping;
        }

        public string Delete(int nVoucherMappingID, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherMapping oVoucherMapping = new VoucherMapping();
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "VoucherMapping", EnumRoleOperationType.Delete);
                oVoucherMapping.VoucherMappingID = nVoucherMappingID;
                VoucherMappingDA.Delete(tc, oVoucherMapping, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public VoucherMapping Get(int id, int nUserId)
        {
            VoucherMapping oAccountHead = new VoucherMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherMappingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherMapping", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherMapping> Gets(int nUserID)
        {
            List<VoucherMapping> oVoucherMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherMappingDA.Gets(tc);
                oVoucherMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherMapping", e);
                #endregion
            }

            return oVoucherMapping;
        }

        public List<VoucherMapping> Gets(string sSQL, int nUserID)
        {
            List<VoucherMapping> oVoucherMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherMappingDA.Gets(tc, sSQL);
                oVoucherMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherMapping", e);
                #endregion
            }

            return oVoucherMapping;
        }
        #endregion
    }
}
