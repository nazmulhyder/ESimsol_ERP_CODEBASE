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
    #region FabricPlanOrder
    public class FabricPlanOrder : BusinessObject
    {
        public FabricPlanOrder()
        {
            FabricPlanOrderID = 0;
            FabricID = 0;
            RefID = 0;
            ColumnCount = 0;
            RefType = EnumFabricPlanRefType.None;
            FabricPlans = new List<FabricPlan>();
            FabricPlanDetails = new List<FabricPlanDetail>();
            FabricPlanRepeats = new List<FabricPlanRepeat>();
            ErrorMessage = "";
            RefNo = "";
            Weave = 0;
            Reed = 0;
            Pick = 0;
            GSM = 0;
            Warp = "";
            Weft = "";
            Dent = 0;
            Note = "";
            Ratio = "";
            RepeatSize = "";
            WeaveName = "";
            FabricPlanOrderIDFrom = 0;
            FabricDesignName = "";
            WarpWeftType = EnumWarpWeft.None;
        }

        #region Property
        public int FabricPlanOrderID { get; set; }
        public int FabricID { get; set; }
        public int RefID { get; set; }
        public EnumFabricPlanRefType RefType { get; set; }
        public int ColumnCount { get; set; }
        public int Weave { get; set; }
        public int Reed { get; set; }
        public int Pick { get; set; }
        public double GSM { get; set; }
        public string Warp { get; set; }
        public string Weft { get; set; }
        public double Dent { get; set; }
        public string Note { get; set; }
        public string Ratio { get; set; }
        public string RepeatSize { get; set; }
        public string WeaveName { get; set; }
        public string FabricDesignName { get; set; }
        public int FabricPlanOrderIDFrom { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public EnumWarpWeft WarpWeftType { get; set; }
        public string RefNo { get; set; }
        public List<FabricPlan> FabricPlans { get; set; }
        public List<FabricPlanDetail> FabricPlanDetails { get; set; }
        public List<FabricPlanRepeat> FabricPlanRepeats { get; set; }
        public string RefTypeSt
        {
            get
            {
                return this.RefType.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<FabricPlanOrder> Gets(int nFabricPlanID, long nUserID)
        {
            return FabricPlanOrder.Service.Gets(nFabricPlanID,nUserID);
        }
        public static List<FabricPlanOrder> Gets(int nRefID, int nRefType, long nUserID)
        {
            return FabricPlanOrder.Service.Gets(nRefID, nRefType, nUserID);
        }
        public static List<FabricPlanOrder> Gets(string sSQL, long nUserID)
        {
            return FabricPlanOrder.Service.Gets(sSQL, nUserID);
        }
        public FabricPlanOrder Get(int id, long nUserID)
        {
            return FabricPlanOrder.Service.Get(id, nUserID);
        }
        public FabricPlanOrder Save(Int64 nUserID)
        {
            return FabricPlanOrder.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricPlanOrder.Service.Delete(id, nUserID);
        }
        public FabricPlanOrder CopyFabricPlans(long nUserID)
        {
            return FabricPlanOrder.Service.CopyFabricPlans(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricPlanOrderService Service
        {
            get { return (IFabricPlanOrderService)Services.Factory.CreateService(typeof(IFabricPlanOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricPlanOrder interface
    public interface IFabricPlanOrderService
    {
        FabricPlanOrder Get(int id, Int64 nUserID);
        List<FabricPlanOrder> Gets(int nFabricPlanID,Int64 nUserID);
        List<FabricPlanOrder> Gets(int nRefID, int nRefType, Int64 nUserID);
        List<FabricPlanOrder> Gets(string sSQL, Int64 nUserID);
        FabricPlanOrder Save(FabricPlanOrder oFabricPlanOrder, Int64 nUserID);
        FabricPlanOrder CopyFabricPlans(FabricPlanOrder oFabricPlanOrder, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
      
    }
    #endregion
}
