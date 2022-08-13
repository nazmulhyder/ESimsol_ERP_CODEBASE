using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class FabricBatchQCFault : BusinessObject
    {
        public FabricBatchQCFault()
        {
            FBQCFaultID = 0;
            FBQCDetailID = 0;
            FPFID = 0;
            FaultPoint = 0;
            NoOfFault = 0;
            FPFName = "";
            ErrorMessage = "";
            FaultDate = DateTime.MinValue;
            FabricFaultType = EnumFabricFaultType.None;
        }

        #region Properties
        public int FBQCFaultID { get; set; }
        public int FBQCDetailID { get; set; }
        public int FPFID { get; set; }
        public int FaultPoint { get; set; }
        public int NoOfFault { get; set; }
        public string FPFName { get; set; }
        public string ErrorMessage { get; set; }
        public string Remarks { get; set; }
        public   DateTime FaultDate { get; set; }
        public EnumFabricFaultType FabricFaultType { get; set; }
        public   string FaultDateStr { get { return this.FaultDate.ToString("dd MMM yyy"); } }
        public int FaultTotal { get { return this.FaultPoint * this.NoOfFault; } }
        public string FabricFaultTypeSt
        {
            get
            {
                return EnumObject.jGet(this.FabricFaultType);
            }
        }
        #endregion

        #region Functions
        public static List<FabricBatchQCFault> Gets(long nUserID)
        {
            return FabricBatchQCFault.Service.Gets(nUserID);
        }
        public static List<FabricBatchQCFault> Gets(string sSQL, long nUserID)
        {
            return FabricBatchQCFault.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchQCFault Save(long nUserID)
        {
            return FabricBatchQCFault.Service.Save(this, nUserID);
        }
        public FabricBatchQCFault Get(int nFBQCFaultID, long nUserID)
        {
            return FabricBatchQCFault.Service.Get(nFBQCFaultID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchQCFault.Service.Delete(nId, nUserID);
        }
        public static List<FabricBatchQCFault> SaveMultipleFNBatchQCFault(List<FabricBatchQCFault> oFabricBatchQCFaults, long nUserID)
        {
            return FabricBatchQCFault.Service.SaveMultipleFabricBatchQCFault(oFabricBatchQCFaults, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricBatchQCFaultService Service
        {
            get { return (IFabricBatchQCFaultService)Services.Factory.CreateService(typeof(IFabricBatchQCFaultService)); }
        }
        #endregion
    }
    #region IFabricBatchQCFault interface
    public interface IFabricBatchQCFaultService
    {
        List<FabricBatchQCFault> Gets(long nUserID);
        List<FabricBatchQCFault> Gets(string sSQL, long nUserID);
        FabricBatchQCFault Save(FabricBatchQCFault oFabricBatchQCFault, long nUserID);
        FabricBatchQCFault Get(int nFBQCFaultID, long nUserID);
        string Delete(int id, long nUserID);
        List<FabricBatchQCFault> SaveMultipleFabricBatchQCFault(List<FabricBatchQCFault> oFabricBatchQCFaults, Int64 nUserID);
    }
    #endregion

}
