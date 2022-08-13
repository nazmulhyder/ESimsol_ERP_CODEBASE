using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region VPPermission
    public class VPPermission : BusinessObject
    {
        public VPPermission()
        {
            VPPermissionID = 0;
            UserID = 0;
            IntegrationSetupID = 0;
            ErrorMessage = "";
            UserName = "";
            BUName = "";
            BUSName = "";
            BusinessUnitID = 0;
            VPPermissions = new List<VPPermission>();
            IntegrationSetups = new List<IntegrationSetup>();
            VoucherSetupIntegration = EnumVoucherSetup.None;
        }
        #region Properties
        public int VPPermissionID { get; set; }
        public int UserID { get; set; }
        public int IntegrationSetupID { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string UserName { get; set; }
        public int BusinessUnitID { get; set; }
        public string BUName { get; set; }
        public string BUSName { get; set; }
        public List<VPPermission> VPPermissions { get; set; }
        public List<IntegrationSetup> IntegrationSetups { get; set; }
        public EnumVoucherSetup VoucherSetupIntegration { get; set; }
        public string VoucherSetupIntegrationSt
        {
            get
            {
                return EnumObject.jGet(this.VoucherSetupIntegration);
            }
        }

        

        #endregion

        #region Functions
        public VPPermission Get(int id, int nUserID)
        {
            return VPPermission.Service.Get(id, nUserID);
        }
        public VPPermission Save(int nUserID)
        {
            return VPPermission.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return VPPermission.Service.Delete(id, nUserID);
        }
        public static List<VPPermission> Gets(int nUserID)
        {
            return VPPermission.Service.Gets(nUserID);
        }
        public static List<VPPermission> Gets(string sSQL, int nUserID)
        {
            return VPPermission.Service.Gets(sSQL, nUserID);
        }
        public static List<VPPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            return VPPermission.Service.GetsByUser(nPermittedUserID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IVPPermissionService Service
        {
            get { return (IVPPermissionService)Services.Factory.CreateService(typeof(IVPPermissionService)); }
        }
        #endregion
    }
    #endregion

    #region IVPPermission interface
    public interface IVPPermissionService
    {
        VPPermission Get(int id, int nUserID);
        List<VPPermission> Gets(int nUserID);
        string Delete(int id, int nUserID);
        VPPermission Save(VPPermission oVPPermission, int nUserID);
        List<VPPermission> Gets(string sSQL, int nUserID);
        List<VPPermission> GetsByUser(int nPermittedUserID, int nUserID);
    }
    #endregion
}
