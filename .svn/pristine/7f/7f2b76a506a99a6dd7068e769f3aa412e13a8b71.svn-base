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

    #region AuthorizationRole
    public class AuthorizationRole : BusinessObject
    {
        public AuthorizationRole()
        {
            AuthorizationRoleID =0;
            RoleNo ="";
            ModuleName = EnumModuleName.None;
            ModuleNameInt = 0;
            OperationType = EnumRoleOperationType.None;
            OperationTypeInt = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int AuthorizationRoleID { get; set; }
        public string RoleNo { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int ModuleNameInt { get; set; }
        public EnumRoleOperationType OperationType { get; set; }
        public int OperationTypeInt { get; set; }
        public string Note { get; set; }        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public string ModuleNameST
        {
            get
            {
                return EnumObject.jGet(this.ModuleName);
            }
        }
        public string OperationTypeST
        {
            get
            {
                return EnumObject.jGet(this.OperationType);
            }
        }
        public string RoleName
        {
            get
            {
                return this.ModuleNameST + " " + this.OperationTypeST;
            }
        }        
        public List<AuthorizationRole> AuthorizationRoles { get; set; }
        public List<EnumObject> ModuleNameObjs { get; set; }
        public List<EnumObject> RoleOperationTypeObjs { get; set; } 
        public Company Company { get; set; }
        #endregion

        #region Functions        
        public string CopyAuthorization(int nAssignUserID, int nCopyFromUserID, bool bUserPermission, bool bAuthorizationRule, bool bStorePermission, bool bProductPermission, bool bBUPermission, bool bTimeCardPermission, bool bAutoVoucharPermission, bool bDashBoardPermission, int nUserID)
        {
            return AuthorizationRole.Service.CopyAuthorization(nAssignUserID, nCopyFromUserID, bUserPermission, bAuthorizationRule, bStorePermission, bProductPermission, bBUPermission, bTimeCardPermission, bAutoVoucharPermission, bDashBoardPermission, nUserID);
        }        
        public AuthorizationRole Get(int id, int nUserID)
        {
            return AuthorizationRole.Service.Get(id, nUserID);
        }
        public List<AuthorizationRole> Save(int nUserID)
        {
            return AuthorizationRole.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AuthorizationRole.Service.Delete(id, nUserID);
        }
        public static List<AuthorizationRole> Gets(int nUserID)
        {
            return AuthorizationRole.Service.Gets(nUserID);
        }
        public static List<AuthorizationRole> Gets(string sSQL, int nUserID)
        {
            return AuthorizationRole.Service.Gets(sSQL, nUserID);
        }
        #endregion


        #region ServiceFactory
        internal static IAuthorizationRoleService Service
        {
            get { return (IAuthorizationRoleService)Services.Factory.CreateService(typeof(IAuthorizationRoleService)); }
        }
        #endregion
    }
    #endregion
    
    #region IAuthorizationRole interface
    public interface IAuthorizationRoleService
    {
        AuthorizationRole Get(int id, int nUserID);
        List<AuthorizationRole> Gets(int nUserID);
        List<AuthorizationRole> Gets(string sSQL, int nUserID);

        string Delete(int id, int nUserID);
        string CopyAuthorization(int nAssignUserID, int nCopyFromUserID, bool bUserPermission, bool bAuthorizationRule, bool bStorePermission, bool bProductPermission, bool bBUPermission, bool bTimeCardPermission, bool bAutoVoucharPermission, bool bDashBoardPermission, int nUserID);
        List<AuthorizationRole> Save(AuthorizationRole oAuthorizationRole, int nUserID);        
    }  
    #endregion

    #region DbObject  Class Region
    public class DBObject : BusinessObject
    {
        public DBObject()
        {
            ObjectName = "";
        }
        public string ObjectName { get; set; }
    }

    public class DBObjectColumn : BusinessObject
    {
        public DBObjectColumn()
        {
            ColumnName = "";
        }
        public string ColumnName { get; set; }
    }
    #endregion
  
}
