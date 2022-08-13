using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region BUPermission
    public class BUPermission : BusinessObject
    {
        public BUPermission()
        {
            BUPermissionID = 0;
            UserID = 0;            
            BUID = 0;            
            Remarks = "";
            BUCode = "";
            BUName = "";            
            ErrorMessage = "";            
            BusinessUnits=new List<BusinessUnit>();
            BUPermissions = new List<BUPermission>();
            UserName = "";
            BUWiseShiftID = 0;
            ShiftID = 0;
            ShiftName = "";
        }

        #region Properties
        public int BUPermissionID { get; set; }
        public int UserID { get; set; }        
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public int BUWiseShiftID { get; set; }  
        public int ShiftID { get; set; }  
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string UserName { get; set; }        
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<BUPermission> BUPermissions { get; set; }
        public string ShiftName { get; set; }    
        #endregion

        #region Functions
        public BUPermission Get(int id, int nUserID)
        {
            return BUPermission.Service.Get(id, nUserID);
        }
        public BUPermission Save(int nUserID)
        {
            return BUPermission.Service.Save(this, nUserID);
        }
        public BUPermission SaveBUWiseShift(int nUserID)
        {
            return BUPermission.Service.SaveBUWiseShift(this, nUserID);
        }
        public string DeleteBUWiseShift(int id, int nUserID)
        {
            return BUPermission.Service.DeleteBUWiseShift(id, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BUPermission.Service.Delete(id, nUserID);
        }
        public static List<BUPermission> Gets(int nUserID)
        {
            return BUPermission.Service.Gets(nUserID);
        }
        public static List<BUPermission> Gets(string sSQL, int nUserID)
        {
            return BUPermission.Service.Gets(sSQL, nUserID);
        }
        public static List<BUPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            return BUPermission.Service.GetsByUser(nPermittedUserID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBUPermissionService Service
        {
            get { return (IBUPermissionService)Services.Factory.CreateService(typeof(IBUPermissionService)); }
        }
        #endregion
    }
    #endregion

    #region IBUPermission interface
    public interface IBUPermissionService
    {
        BUPermission Get(int id, int nUserID);
        List<BUPermission> Gets(int nUserID);
        string Delete(int id, int nUserID);
        string DeleteBUWiseShift(int id, int nUserID);
        BUPermission Save(BUPermission oBUPermission, int nUserID);
        BUPermission SaveBUWiseShift(BUPermission oBUPermission, int nUserID);
        List<BUPermission> Gets(string sSQL, int nUserID);
        List<BUPermission> GetsByUser(int nPermittedUserID, int nUserID);
    }
    #endregion
}
