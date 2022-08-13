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
    #region FabricChemicalPlan
    public class FabricChemicalPlan : BusinessObject
    {
        public FabricChemicalPlan()
        {
            FabricChemicalPlanID = 0;
            FabricSizingPlanID = 0;
            FBID = 0;
            ProductID = 0;
            Qty = 0;
            ErrorMessage = "";
            BatchNo = "";
            ProductName = "";
            ProductCode = "";
            MUnitName = "";
            MUnitID = 0;
            SizingPlanNo = "";
        }

        #region Property
        public int FabricChemicalPlanID { get; set; }
        public int FabricSizingPlanID { get; set; }
        public int FBID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string SizingPlanNo { get; set; }
        public int MUnitID { get; set; }
        public string MUnitName { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        #endregion

        #region Functions
        public static List<FabricChemicalPlan> Gets(long nUserID)
        {
            return FabricChemicalPlan.Service.Gets(nUserID);
        }
        public static List<FabricChemicalPlan> Gets(string sSQL, long nUserID)
        {
            return FabricChemicalPlan.Service.Gets(sSQL, nUserID);
        }
        public FabricChemicalPlan Get(int id, long nUserID)
        {
            return FabricChemicalPlan.Service.Get(id, nUserID);
        }
        public FabricChemicalPlan Save(long nUserID)
        {
            return FabricChemicalPlan.Service.Save(this, nUserID);
        }
        public List<FabricChemicalPlan> SaveMultiple(List<FabricChemicalPlan> oFabricChemicalPlans, long nUserID)
        {
            return FabricChemicalPlan.Service.SaveMultiple(oFabricChemicalPlans, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricChemicalPlan.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricChemicalPlanService Service
        {
            get { return (IFabricChemicalPlanService)Services.Factory.CreateService(typeof(IFabricChemicalPlanService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricChemicalPlan interface
    public interface IFabricChemicalPlanService
    {
        FabricChemicalPlan Get(int id, Int64 nUserID);
        List<FabricChemicalPlan> Gets(Int64 nUserID);
        List<FabricChemicalPlan> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricChemicalPlan Save(FabricChemicalPlan oFabricChemicalPlan, Int64 nUserID);
        List<FabricChemicalPlan> SaveMultiple(List<FabricChemicalPlan> oFabricChemicalPlans, Int64 nUserID);
    }
    #endregion
}
