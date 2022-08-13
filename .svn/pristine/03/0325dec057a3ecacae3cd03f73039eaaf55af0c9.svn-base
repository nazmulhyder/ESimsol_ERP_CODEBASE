using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region AuthorizationRoleMapping
    public class AuthorizationRoleMapping : BusinessObject
    {
        public AuthorizationRoleMapping()
        {
            AuthorizationRoleMappingID = 0;
            AuthorizationRoleID = 0;
            UserID= 0;
            ModuleName = EnumModuleName.None;
            ModuleNameInt = 0;
            OperationType = EnumRoleOperationType.None;
            OperationTypeInt = 0;
            AuthorizationRoleMappingIDs = "";
            ErrorMessage = "";
        }

        #region Properties
        public int AuthorizationRoleMappingID { get; set; }
        public int AuthorizationRoleID { get; set; }
        public int UserID { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int ModuleNameInt { get; set; }
        public EnumRoleOperationType OperationType { get; set; }
        public int OperationTypeInt { get; set; }
        public string AuthorizationRoleMappingIDs { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ModuleNameST
        {
            get
            {
                //return EnumObject.jGet(this.ModuleName);
                return this.ModuleName.ToString();
            }
        }
        public string OperationTypeST
        {
            get
            {
                //return EnumObject.jGet(this.OperationType);
                return this.OperationType.ToString();

            }
        }
        public string RoleName
        {
            get
            {                
                return this.ModuleNameST + " " + this.OperationTypeST;

            }
        }
        public bool IsShortList { get; set; }
        public bool IsUserBased { get; set; }        
        public List<User> Users { get; set; }
        public List<AuthorizationRoleMapping> AuthorizationRoleMappings { get; set; }
        public List<AuthorizationRole> AuthorizationRoles { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static List<AuthorizationRoleMapping> GetsByRole(int id, int nUserID)
        {
            return AuthorizationRoleMapping.Service.GetsByRole(id, nUserID);
        }
        public static List<AuthorizationRoleMapping> GetsByUser(int id, int nUserID)
        {
            return AuthorizationRoleMapping.Service.GetsByUser(id, nUserID);
        }
        public static List<AuthorizationRoleMapping> GetsByModuleAndUser(string sModuleNames, int id, int nUserID)
        {
            return AuthorizationRoleMapping.Service.GetsByModuleAndUser(sModuleNames, id, nUserID);
        }
        public AuthorizationRoleMapping Get(int id, int nUserID)
        {
            return AuthorizationRoleMapping.Service.Get(id, nUserID);
        }
        public string Save(bool IsShortList , bool IsUserBased, int nUserID)
        {
            return AuthorizationRoleMapping.Service.Save(this, IsShortList, IsUserBased, nUserID);
        }
        public static List<AuthorizationRoleMapping> DisallowMappingRole(AuthorizationRoleMapping oAuthorizationRoleMapping, int nUserID)
        {
            return AuthorizationRoleMapping.Service.DisallowMappingRole(oAuthorizationRoleMapping, nUserID);
        }

        #region Non DB Functions
        public static bool HasPermission(EnumRoleOperationType eRoleOperationType, EnumModuleName eModuleName, List<AuthorizationRoleMapping> oAuthorizationRoleMappings, int nUserID)
        {
            if (nUserID == -9) { return true; }
            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.ModuleName == eModuleName && oItem.OperationType == eRoleOperationType)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region ServiceFactory
        internal static IAuthorizationRoleMappingService Service
        {
            get { return (IAuthorizationRoleMappingService)Services.Factory.CreateService(typeof(IAuthorizationRoleMappingService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class AuthorizationRoleMappingList : List<AuthorizationRoleMapping>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IAuthorizationRoleMapping interface
    public interface IAuthorizationRoleMappingService
    {
        AuthorizationRoleMapping Get(int id, int nUserID);
        List<AuthorizationRoleMapping> GetsByRole( int id, int nUserID);
        List<AuthorizationRoleMapping> GetsByUser(int id, int nUserID);
        List<AuthorizationRoleMapping> GetsByModuleAndUser(string sModuleNames, int id, int nUserID);
        string Save(AuthorizationRoleMapping oAuthorizationRoleMapping, bool IsShortList, bool IsUserBased, int nUserID);
        List<AuthorizationRoleMapping> DisallowMappingRole(AuthorizationRoleMapping oAuthorizationRoleMapping, int nUserID);
    }
    #endregion
    
   
}
