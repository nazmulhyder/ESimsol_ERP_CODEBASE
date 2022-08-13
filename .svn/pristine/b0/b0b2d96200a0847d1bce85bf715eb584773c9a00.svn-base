using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{

    #region DashBoardPermission
    public class DBPermission : BusinessObject
    {
        public DBPermission()
        {
            DBPermissionID = 0;
            UserID = 0;
            DashBoardType = EnumDashBoardType.None;
            DashBoardTypeInt = 0;
            Remarks = "";
            ErrorMessage = "";
            UserName = "";
            DBPermissions = new List<DBPermission>();
            DashBoardTypeObjs = new List<EnumObject>();
        }
        #region Properties
        public int DBPermissionID { get; set; }
        public int UserID { get; set; }
        public EnumDashBoardType DashBoardType { get; set; }
        public int DashBoardTypeInt { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string UserName { get; set; }
        public List<EnumObject> DashBoardTypeObjs { get; set; }
        public List<DBPermission> DBPermissions { get; set; }
        public string DashBoardTypeST
        {
            get
            {
                return EnumObject.jGet(this.DashBoardType);
            }
        }
        #endregion

        #region Functions
        public DBPermission Get(int id, int nUserID)
        {
            return DBPermission.Service.Get(id, nUserID);
        }
        public DBPermission Save(int nUserID)
        {
            return DBPermission.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return DBPermission.Service.Delete(id, nUserID);
        }
        public static List<DBPermission> Gets(int nUserID)
        {
            return DBPermission.Service.Gets(nUserID);
        }
        public static List<DBPermission> Gets(string sSQL, int nUserID)
        {
            return DBPermission.Service.Gets(sSQL, nUserID);
        }
        public static List<DBPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            return DBPermission.Service.GetsByUser(nPermittedUserID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDBPermissionService Service
        {
            get { return (IDBPermissionService)Services.Factory.CreateService(typeof(IDBPermissionService)); }
        }
        #endregion
    }
    #endregion

    #region IDBPermissionService interface
    public interface IDBPermissionService
    {
        DBPermission Get(int id, int nUserID);
        List<DBPermission> Gets(int nUserID);
        string Delete(int id, int nUserID);
        DBPermission Save(DBPermission oProductPermission, int nUserID);
        List<DBPermission> Gets(string sSQL, int nUserID);
        List<DBPermission> GetsByUser(int nPermittedUserID, int nUserID);
    }
    #endregion
}
