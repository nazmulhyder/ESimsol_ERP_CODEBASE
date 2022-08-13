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


    public class AuthorizationRoleMappingService : MarshalByRefObject, IAuthorizationRoleMappingService
    {
        #region Private functions and declaration
        private AuthorizationRoleMapping MapObject(NullHandler oReader)
        {
            AuthorizationRoleMapping oAuthorizationRoleMapping = new AuthorizationRoleMapping();
            oAuthorizationRoleMapping.AuthorizationRoleMappingID = oReader.GetInt32("AuthorizationRoleMappingID");
            oAuthorizationRoleMapping.AuthorizationRoleID = oReader.GetInt32("AuthorizationRoleID");
            oAuthorizationRoleMapping.UserID = oReader.GetInt32("UserID");
            oAuthorizationRoleMapping.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oAuthorizationRoleMapping.ModuleNameInt = oReader.GetInt32("ModuleName");
            oAuthorizationRoleMapping.OperationType = (EnumRoleOperationType)oReader.GetInt32("OperationType");
            oAuthorizationRoleMapping.OperationTypeInt =oReader.GetInt32("OperationType");            
            return oAuthorizationRoleMapping;
        }

        private AuthorizationRoleMapping CreateObject(NullHandler oReader)
        {
            AuthorizationRoleMapping oAuthorizationRoleMapping = new AuthorizationRoleMapping();
            oAuthorizationRoleMapping = MapObject(oReader);
            return oAuthorizationRoleMapping;
        }

        private List<AuthorizationRoleMapping> CreateObjects(IDataReader oReader)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AuthorizationRoleMapping oItem = CreateObject(oHandler);
                oAuthorizationRoleMapping.Add(oItem);
            }
            return oAuthorizationRoleMapping;
        }

        #endregion

        #region Interface implementation
        public string Save(AuthorizationRoleMapping oAuthorizationRoleMapping, bool IsShortList, bool IsUserBased, int nUserID)
        {
            TransactionContext tc = null;
            AuthorizationRoleMapping oNewAuthorizationRoleMapping = new AuthorizationRoleMapping();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = oAuthorizationRoleMapping.AuthorizationRoleMappings;
            string sIds = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (IsShortList == true)
                {
                    foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
                    {
                        if (oItem.AuthorizationRoleMappingID <= 0)
                        {
                            reader = AuthorizationRoleMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = AuthorizationRoleMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
                    {
                        if (oItem.AuthorizationRoleMappingID <= 0)
                        {
                            reader = AuthorizationRoleMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = AuthorizationRoleMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);

                        if (reader.Read())
                        {
                            oNewAuthorizationRoleMapping = new AuthorizationRoleMapping();
                            oNewAuthorizationRoleMapping = CreateObject(oReader);
                        }
                        reader.Close();
                        if (IsUserBased == true)
                        {
                            sIds = sIds + oNewAuthorizationRoleMapping.AuthorizationRoleID + ",";
                        }
                        else
                        {
                            sIds = sIds + oNewAuthorizationRoleMapping.UserID + ",";
                        }
                    }
                    if (sIds.Length > 0)
                    {
                        sIds = sIds.Remove(sIds.Length - 1, 1);
                    }
                    AuthorizationRoleMappingDA.Delete(tc, oNewAuthorizationRoleMapping, EnumDBOperation.Delete, nUserID, IsUserBased, sIds);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save AuthorizationRoleMapping. Because of " + e.Message, e);
                #endregion
            }
            return "Succefully Saved";
        }
        
        public AuthorizationRoleMapping Get(int id, int nUserId)
        {
            AuthorizationRoleMapping oAccountHead = new AuthorizationRoleMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AuthorizationRoleMappingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get AuthorizationRoleMapping", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<AuthorizationRoleMapping> GetsByRole(int id, int nUserID)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationRoleMappingDA.GetsByRole(tc, id);
                oAuthorizationRoleMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AuthorizationRoleMapping", e);
                #endregion
            }

            return oAuthorizationRoleMappings;
        }

        public List<AuthorizationRoleMapping> GetsByUser(int id, int nUserID)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationRoleMappingDA.GetsByUser(tc, id);
                oAuthorizationRoleMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AuthorizationRoleMapping", e);
                #endregion
            }

            return oAuthorizationRoleMappings;
        }

        public List<AuthorizationRoleMapping> DisallowMappingRole(AuthorizationRoleMapping oAuthorizationRoleMapping, int nUserID)
        {            
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                AuthorizationRoleMappingDA.DisallowMappingRole(tc, oAuthorizationRoleMapping.AuthorizationRoleMappingIDs);
                reader = AuthorizationRoleMappingDA.GetsByUser(tc, oAuthorizationRoleMapping.UserID);
                oAuthorizationRoleMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oAuthorizationRoleMapping = new AuthorizationRoleMapping();
                oAuthorizationRoleMapping.ErrorMessage = e.Message;
                oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
                oAuthorizationRoleMappings.Add(oAuthorizationRoleMapping);
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get AuthorizationRoleMapping", e);
                #endregion
            }

            return oAuthorizationRoleMappings;
        }
        
        public List<AuthorizationRoleMapping> GetsByModuleAndUser(string sModuleNames, int id, int nUserID)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AuthorizationRoleMappingDA.GetsByModuleAndUser(tc, sModuleNames, id);
                oAuthorizationRoleMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AuthorizationRoleMapping", e);
                #endregion
            }
            return oAuthorizationRoleMappings;
        }

        #endregion
    }   
    
    
}
