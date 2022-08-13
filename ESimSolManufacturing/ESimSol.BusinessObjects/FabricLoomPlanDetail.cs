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
    #region FabricLoomPlanDetail
    public class FabricLoomPlanDetail : BusinessObject
    {
        public FabricLoomPlanDetail()
        {
            FLPDID = 0;
            FLPID = 0;
            FBPBeamID = 0;
            Construction = "";
            FEONo = "";
            BatchNo = "";
            BeamNo = "";
            BeamID = 0;
            BuyerName = "";
            Qty = 0;
            IsDrawing = 0;
            ErrorMessage = "";
        }

        #region Property
        public int FLPDID { get; set; }
        public int FLPID { get; set; }
        public int FBPBeamID { get; set; }
        public string Construction { get; set; }
        public string FEONo { get; set; }
        public string BatchNo { get; set; }
        public string BuyerName { get; set; }
        public int BeamID { get; set; }
        public string BeamNo { get; set; }
        public double Qty { get; set; }
        public int IsDrawing { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int FEOID { get; set; }
        public int FEOSID { get; set; }
        public EnumFabricSpeType FSpcType { get; set; }
        public double QtyInMtr
        {
            get
            {
                return Global.GetMeter(this.Qty, 2);
            }
        }
        public string IsDrawingSt
        {
            get
            {
                if (IsDrawing == (int)EnumFabricBatchState.DrawingIn) return "Drawing";
                else if (IsDrawing == (int)EnumFabricBatchState.LeasingIn) return "Leasing";
                return "";
            }
        }
        #endregion

        #region Functions
        public static List<FabricLoomPlanDetail> Gets(int nFabricLoomPlanID, long nUserID)
        {
            return FabricLoomPlanDetail.Service.Gets(nFabricLoomPlanID, nUserID);
        }
        public static List<FabricLoomPlanDetail> Gets(string sSQL, long nUserID)
        {
            return FabricLoomPlanDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricLoomPlanDetail Get(int id, long nUserID)
        {
            return FabricLoomPlanDetail.Service.Get(id, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricLoomPlanDetail.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricLoomPlanDetailService Service
        {
            get { return (IFabricLoomPlanDetailService)Services.Factory.CreateService(typeof(IFabricLoomPlanDetailService)); }
        }
        #endregion

        public List<FabricLoomPlanDetail> FabricLoomPlanDetails { get; set; }
    }
    #endregion

    #region IFabricLoomPlanDetail interface
    public interface IFabricLoomPlanDetailService
    {
        FabricLoomPlanDetail Get(int id, Int64 nUserID);
        List<FabricLoomPlanDetail> Gets(int nFabricLoomPlanID, Int64 nUserID);
        List<FabricLoomPlanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}
