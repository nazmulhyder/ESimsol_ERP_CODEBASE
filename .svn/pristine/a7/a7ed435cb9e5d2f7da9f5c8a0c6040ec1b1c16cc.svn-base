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
    #region FabricPlanDetail
    public class FabricPlanDetail : BusinessObject
    {
        public FabricPlanDetail()
        {
            FabricPlanDetailID = 0;
            FabricPlanID = 0;
            ColNo = 0;
            EndsCount = 0;
            SLNo = 0;
            RepeatNo = 0;
            ErrorMessage = "";
        }

        #region Property
        public int FabricPlanDetailID { get; set; }
        public int FabricPlanID { get; set; }
        public int ColNo { get; set; }
        public int EndsCount { get; set; }
        public int SLNo { get; set; }
        public int RepeatNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static List<FabricPlanDetail> Gets(int nFabricPlanID, long nUserID)
        {
            return FabricPlanDetail.Service.Gets(nFabricPlanID,nUserID);
        }
        public static List<FabricPlanDetail> Gets(string sSQL, long nUserID)
        {
            return FabricPlanDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricPlanDetail Get(int id, long nUserID)
        {
            return FabricPlanDetail.Service.Get(id, nUserID);
        }
     
        public static List<FabricPlanDetail> SaveAll(List<FabricPlanDetail> oFabricPlanDetails, long nUserID)
        {
            return FabricPlanDetail.Service.SaveAll(oFabricPlanDetails, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricPlanDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricPlanDetailService Service
        {
            get { return (IFabricPlanDetailService)Services.Factory.CreateService(typeof(IFabricPlanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricPlanDetail interface
    public interface IFabricPlanDetailService
    {
        FabricPlanDetail Get(int id, Int64 nUserID);
        List<FabricPlanDetail> Gets(int nFabricPlanID,Int64 nUserID);
        List<FabricPlanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        List<FabricPlanDetail> SaveAll(List<FabricPlanDetail> oFabricPlanDetails, Int64 nUserID);
    }
    #endregion
}
