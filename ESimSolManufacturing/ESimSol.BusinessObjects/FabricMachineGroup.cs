using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FabricMachineGroup
    public class FabricMachineGroup
    {
        public FabricMachineGroup()
        {
            FabricMachineGroupID = 0;
            Name = "";
            Note = "";
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";

        }
        #region Property
        public int FabricMachineGroupID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion
        #region Derived Property
        public string LastUpdateDateTimeInString
        {
            get
            {
                return LastUpdateDateTime.ToString("dd MMM yyyy");
            }
        }
        
        #endregion


        #region Functions
        public static List<FabricMachineGroup> Gets(long nUserID)
        {
            return FabricMachineGroup.Service.Gets(nUserID);
        }
        public static List<FabricMachineGroup> Gets(string sSQL, long nUserID)
        {
            return FabricMachineGroup.Service.Gets(sSQL, nUserID);
        }
        public FabricMachineGroup Get(int id, long nUserID)
        {
            return FabricMachineGroup.Service.Get(id, nUserID);
        }
        public FabricMachineGroup Save(long nUserID)
        {
            return FabricMachineGroup.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricMachineGroup.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricMachineGroupService Service
        {
            get { return (IFabricMachineGroupService)Services.Factory.CreateService(typeof(IFabricMachineGroupService)); }
        }
        #endregion
    }
    #endregion
    #region IFabricMachineGroup interface
    public interface IFabricMachineGroupService
    {
        FabricMachineGroup Get(int id, Int64 nUserID);
        List<FabricMachineGroup> Gets(Int64 nUserID);
        List<FabricMachineGroup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricMachineGroup Save(FabricMachineGroup oFabricMachineGroup, Int64 nUserID);
    }
    #endregion
}
