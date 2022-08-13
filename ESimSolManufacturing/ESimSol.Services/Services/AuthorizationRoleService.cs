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
    public class AuthorizationRoleService : MarshalByRefObject, IAuthorizationRoleService
    {
        #region Private functions and declaration
        private AuthorizationRole MapObject(NullHandler oReader)
        {
            AuthorizationRole oAuthorizationRole = new AuthorizationRole();
            oAuthorizationRole.AuthorizationRoleID = oReader.GetInt32("AuthorizationRoleID");
            oAuthorizationRole.RoleNo = oReader.GetString("RoleNo");
            oAuthorizationRole.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oAuthorizationRole.ModuleNameInt = oReader.GetInt32("ModuleName");
            oAuthorizationRole.OperationType = (EnumRoleOperationType)oReader.GetInt32("OperationType");
            oAuthorizationRole.OperationTypeInt = oReader.GetInt32("OperationType");            
            oAuthorizationRole.Note = oReader.GetString("Note");
            return oAuthorizationRole;
        }

        private AuthorizationRole CreateObject(NullHandler oReader)
        {
            AuthorizationRole oAuthorizationRole = new AuthorizationRole();
            oAuthorizationRole = MapObject(oReader);
            return oAuthorizationRole;
        }

        private List<AuthorizationRole> CreateObjects(IDataReader oReader)
        {
            List<AuthorizationRole> oAuthorizationRole = new List<AuthorizationRole>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AuthorizationRole oItem = CreateObject(oHandler);
                oAuthorizationRole.Add(oItem);
            }
            return oAuthorizationRole;
        }

        #endregion
        
        #region Interface implementation
        public List<AuthorizationRole> Save(AuthorizationRole oAuthorizationRole, int nUserID)
        {
            TransactionContext tc = null;
            List<AuthorizationRole> oAuthorizationRoles = new List<AuthorizationRole>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (AuthorizationRole oItem in oAuthorizationRole.AuthorizationRoles)
                {
                    if (oItem.AuthorizationRoleID <= 0)
                    {

                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AuthorizationRole, EnumRoleOperationType.Add);
                        reader = AuthorizationRoleDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.AuthorizationRole, EnumRoleOperationType.Edit);
                        reader = AuthorizationRoleDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oAuthorizationRole = new AuthorizationRole();
                        oAuthorizationRole = CreateObject(oReader);
                    }
                    oAuthorizationRoles.Add(oAuthorizationRole);
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAuthorizationRoles = new List<AuthorizationRole>();
                oAuthorizationRole = new AuthorizationRole();
                oAuthorizationRole.ErrorMessage = e.Message.Split('!')[0];
                oAuthorizationRoles.Add(oAuthorizationRole);

                #endregion
            }
            return oAuthorizationRoles;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRole oAuthorizationRole = new AuthorizationRole();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.AuthorizationRole, EnumRoleOperationType.Delete);
                oAuthorizationRole.AuthorizationRoleID = id;
                AuthorizationRoleDA.Delete(tc, oAuthorizationRole, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete AuthorizationRole. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
        public string CopyAuthorization(int nAssignUserID, int nCopyFromUserID, bool bUserPermission, bool bAuthorizationRule, bool bStorePermission, bool bProductPermission, bool bBUPermission, bool bTimeCardPermission, bool bAutoVoucharPermission, bool bDashBoardPermission, int nUserID)
        {
            TransactionContext tc = null;
            int newUserID = nUserID;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRole oAuthorizationRole = new AuthorizationRole();
                AuthorizationRoleDA.CheckUserPermission(tc, newUserID, EnumModuleName.AuthorizationRole, EnumRoleOperationType.Add);
                AuthorizationRoleDA.CopyAuthorization(tc, nAssignUserID, nCopyFromUserID, bUserPermission, bAuthorizationRule, bStorePermission, bProductPermission, bBUPermission, bTimeCardPermission, bAutoVoucharPermission, bDashBoardPermission, newUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Commit sucessfully";
        }
        public AuthorizationRole Get(int id, int nUserId)
        {
            AuthorizationRole oAccountHead = new AuthorizationRole();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AuthorizationRoleDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get AuthorizationRole", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<AuthorizationRole> Gets(int nUserID)
        {
            List<AuthorizationRole> oAuthorizationRole = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationRoleDA.Gets(tc);
                oAuthorizationRole = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AuthorizationRole", e);
                #endregion
            }

            return oAuthorizationRole;
        }
        public List<AuthorizationRole> Gets(string sSQL, int nUserID)
        {
            List<AuthorizationRole> oAuthorizationRole = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationRoleDA.Gets(tc, sSQL);
                oAuthorizationRole = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AuthorizationRole", e);
                #endregion
            }

            return oAuthorizationRole;
        }
        #endregion

        
    }
}
