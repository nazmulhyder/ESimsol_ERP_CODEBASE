using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region StorePermission
    public class StorePermission : BusinessObject
    {
        public StorePermission()
        {
            StorePermissionID = 0;
            UserID = 0;
            ModuleName = EnumModuleName.None;
            ModuleNameInt = 0;
            WorkingUnitID = 0;
            StoreType = EnumStoreType.None;
            StoreTypeInt = 0;
            Remarks = "";
            WorkingUnitCode = "";
            LocationName = "";
            OperationUnitName = "";
            ErrorMessage = "";
            ModuleNameObjs = new List<EnumObject>();
            WorkingUnits=new List<WorkingUnit>();
            StorePermissions = new List<StorePermission>();
            UserName = "";
        }

        #region Properties
        public int StorePermissionID { get; set; }
        public int UserID { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int ModuleNameInt { get; set; }
        public int WorkingUnitID { get; set; }
        public EnumStoreType StoreType { get; set; }
        public int StoreTypeInt { get; set; }
        public string Remarks { get; set; }
        public string WorkingUnitCode { get; set; }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string UserName { get; set; }
        public List<EnumObject> ModuleNameObjs { get; set; }
        public List<WorkingUnit> WorkingUnits { get; set; }
        public List<StorePermission> StorePermissions { get; set; }
        public string WorkingUnitName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName + "]";
            }
        }
        public string ModuleNameST
        {
            get
            {
                return EnumObject.jGet(this.ModuleName);
            }
        }
        public string StoreTypeST
        {
            get
            {
                return this.StoreType.ToString();
            }
        }
        #endregion

        #region Functions
        public StorePermission Get(int id, int nUserID)
        {
            return StorePermission.Service.Get(id, nUserID);
        }
        public List<StorePermission> Save(int nUserID)
        {
            return StorePermission.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return StorePermission.Service.Delete(id, nUserID);
        }
        public static List<StorePermission> Gets(int nUserID)
        {
            return StorePermission.Service.Gets(nUserID);
        }
        public static List<StorePermission> Gets(string sSQL, int nUserID)
        {
            return StorePermission.Service.Gets(sSQL, nUserID);
        }
        public static List<StorePermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            return StorePermission.Service.GetsByUser(nPermittedUserID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IStorePermissionService Service
        {
            get { return (IStorePermissionService)Services.Factory.CreateService(typeof(IStorePermissionService)); }
        }
        #endregion
    }
    #endregion

    #region IStorePermission interface
    public interface IStorePermissionService
    {
        StorePermission Get(int id, int nUserID);
        List<StorePermission> Gets(int nUserID);
        string Delete(int id, int nUserID);
        List<StorePermission> Save(StorePermission oStorePermission, int nUserID);
        List<StorePermission> Gets(string sSQL, int nUserID);
        List<StorePermission> GetsByUser(int nPermittedUserID, int nUserID);
    }
    #endregion
}
