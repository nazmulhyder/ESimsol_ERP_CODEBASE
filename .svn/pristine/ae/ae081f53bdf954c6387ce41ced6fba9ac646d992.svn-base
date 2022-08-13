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

    #region FabricBatchProductionBreakage
    public class FabricBatchProductionBreakage : BusinessObject
    {
        public FabricBatchProductionBreakage()
        {
            FBPBreakageID = 0;
            FBLDetailID = 0;
            FBreakageID = 0;
            DurationInMin = 0;
            Note = "";
            NoOfBreakage = 0;
            FabricBreakageName = "";
            FBreakageWeavingProcess = EnumWeavingProcess.Warping;
            FabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();
            ErrorMessage = "";
        }

        #region Properties
        public EnumWeavingProcess FBreakageWeavingProcess { get; set; }
        public int FBreakageID { get; set; }
        public string FabricBreakageName { get; set; }
        public double DurationInMin { get; set; }
        public string Note { get; set; }
        public int FBLDetailID { get; set; }
        public int FBPBreakageID { get; set; }
        
        public int NoOfBreakage { get; set; }
        public string ErrorMessage { get; set; }

        
        public List<FabricBatchProductionBreakage> FabricBatchProductionBreakages { get; set; }
        public FabricBatch FabricProductionBatch { get; set; }

  

        #endregion
        #region Derived Properties
        public FabricBatch FabricBatch { get; set; }
        public string BatchNo { get; set; }
        public string BeamNo { get; set; }
        public string Shift { get; set; }
        public DateTime ShiftDate { get; set; }

        #endregion

        #region Functions

        public static List<FabricBatchProductionBreakage> Gets(int nFBPBID, long nUserID)
        {
            return FabricBatchProductionBreakage.Service.Gets(nFBPBID, nUserID);
        }
        public FabricBatchProductionBreakage Save(long nUserID)
        {
            return FabricBatchProductionBreakage.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchProductionBreakage.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionBreakageService Service
        {
            get { return (IFabricBatchProductionBreakageService)Services.Factory.CreateService(typeof(IFabricBatchProductionBreakageService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchProductionBreakage interface

    public interface IFabricBatchProductionBreakageService
    {
        List<FabricBatchProductionBreakage> Gets(int nFBPBID, long nUserID);
        FabricBatchProductionBreakage Save(FabricBatchProductionBreakage oFabricBatchProductionBreakage, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
    

}
