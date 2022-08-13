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
    public class AccountHeadConfigureService : MarshalByRefObject, IAccountHeadConfigureService
    {
        #region Private functions and declaration
        private AccountHeadConfigure MapObject(NullHandler oReader)
        {
            AccountHeadConfigure oAccountHeadConfigure = new AccountHeadConfigure();
            oAccountHeadConfigure.AccountHeadConfigureID = oReader.GetInt32("AccountHeadConfigureID");
            oAccountHeadConfigure.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAccountHeadConfigure.ReferenceObjectID = oReader.GetInt32("ReferenceObjectID");
            oAccountHeadConfigure.ReferenceObjectType = (EnumVoucherExplanationType)oReader.GetInt32("ReferenceObjectType");
            oAccountHeadConfigure.Name = oReader.GetString("Name");
            oAccountHeadConfigure.CostCenterDescription = oReader.GetString("CostCenterDescription");
            oAccountHeadConfigure.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oAccountHeadConfigure.AccountHeadName = oReader.GetString("AccountHeadName");
            oAccountHeadConfigure.AccountPathName = oReader.GetString("AccountPathName");
            return oAccountHeadConfigure;
        }

        private AccountHeadConfigure CreateObject(NullHandler oReader)
        {
            AccountHeadConfigure oAccountHeadConfigure = new AccountHeadConfigure();
            oAccountHeadConfigure = MapObject(oReader);
            return oAccountHeadConfigure;
        }
        private List<AccountHeadConfigure> CreateObjects(IDataReader oReader)
        {
            List<AccountHeadConfigure> oAccountHeadConfigure = new List<AccountHeadConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AccountHeadConfigure oItem = CreateObject(oHandler);
                oAccountHeadConfigure.Add(oItem);
            }
            return oAccountHeadConfigure;
        }

        #endregion

        #region Interface implementation
        public List<AccountHeadConfigure> Gets(int nUserID)
        {
            List<AccountHeadConfigure> oAccountHeadConfigure = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountHeadConfigureDA.Gets(tc);
                oAccountHeadConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountHeadConfigure", e);
                #endregion
            }
            return oAccountHeadConfigure;
        }
        public List<AccountHeadConfigure> Gets(int nExplationType, int nAccountHeadID, int nUserID)
        {
            List<AccountHeadConfigure> oAccountHeadConfigure = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountHeadConfigureDA.Gets(tc, nExplationType, nAccountHeadID);
                oAccountHeadConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountHeadConfigure", e);
                #endregion
            }
            return oAccountHeadConfigure;
        }
        public List<AccountHeadConfigure> Gets(string sSQL, int nUserID)
        {
            List<AccountHeadConfigure> oAccountHeadConfigure = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AccountHeadConfigureDA.Gets(tc, sSQL);
                oAccountHeadConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountHeadConfigure", e);
                #endregion
            }

            return oAccountHeadConfigure;
        }
        public AccountHeadConfigure Get(int id, int nUserId)
        {
            AccountHeadConfigure oAccountHead = new AccountHeadConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = AccountHeadConfigureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get AccountHeadConfigure", e);
                #endregion
            }
            return oAccountHead;
        }
        public AccountHeadConfigure Save(AccountHeadConfigure oAccountHeadConfigure, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAccountHeadConfigure.AccountHeadConfigureID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountHeadConfigure, EnumRoleOperationType.Add);
                    reader = AccountHeadConfigureDA.InsertUpdate(tc, oAccountHeadConfigure, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AccountHeadConfigure, EnumRoleOperationType.Edit);
                    reader = AccountHeadConfigureDA.InsertUpdate(tc, oAccountHeadConfigure, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHeadConfigure = new AccountHeadConfigure();
                    oAccountHeadConfigure = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save AccountHeadConfigure. Because of " + e.Message, e);
                #endregion
            }
            return oAccountHeadConfigure;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AccountHeadConfigure oAccountHeadConfigure = new AccountHeadConfigure();
                oAccountHeadConfigure.AccountHeadConfigureID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AccountHeadConfigure, EnumRoleOperationType.Delete);//Delete check
                AccountHeadConfigureDA.Delete(tc, oAccountHeadConfigure, EnumDBOperation.Delete, nUserId);
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
        #endregion
    }
}
