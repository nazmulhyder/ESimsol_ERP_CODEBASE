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
    #region FabricSizingPlanDetail
    public class FabricSizingPlanDetail : BusinessObject
    {
        public FabricSizingPlanDetail()
        {
            FSPDID = 0;
            FabricSizingPlanID = 0;
            FMID = 0;
            FabricMachineTypeID = 0;
            SizingBeamNo = 0;
            Qty = 0;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            ErrorMessage = "";
            FabricMachineTypeName = "";
            BeamName = "";
            FabricModelName = "";
        }

        #region Property
        public int FSPDID { get; set; }
        public int FabricSizingPlanID { get; set; }
        public int FMID { get; set; }
        public int FabricMachineTypeID { get; set; }
        public int SizingBeamNo { get; set; }
        public double Qty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string FabricMachineTypeName { get; set; }
        public string FabricModelName { get; set; }
        public string BeamName { get; set; }
        
        #endregion

        #region Functions
        public static List<FabricSizingPlanDetail> Gets(int nFabricSizingPlanID, long nUserID)
        {
            return FabricSizingPlanDetail.Service.Gets(nFabricSizingPlanID, nUserID);
        }
        public static List<FabricSizingPlanDetail> Gets(string sSQL, long nUserID)
        {
            return FabricSizingPlanDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricSizingPlanDetail Get(int id, long nUserID)
        {
            return FabricSizingPlanDetail.Service.Get(id, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FabricSizingPlanDetail.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricSizingPlanDetailService Service
        {
            get { return (IFabricSizingPlanDetailService)Services.Factory.CreateService(typeof(IFabricSizingPlanDetailService)); }
        }
        #endregion

        public List<FabricSizingPlanDetail> FabricSizingPlanDetails { get; set; }
    }
    #endregion

    #region IFabricSizingPlanDetail interface
    public interface IFabricSizingPlanDetailService
    {
        FabricSizingPlanDetail Get(int id, Int64 nUserID);
        List<FabricSizingPlanDetail> Gets(int nFabricSizingPlanID, Int64 nUserID);
        List<FabricSizingPlanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(FabricSizingPlanDetail oFabricSizingPlanDetail, Int64 nUserID);

    }
    #endregion
}
