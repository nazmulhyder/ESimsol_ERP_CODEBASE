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
    #region FabricPlanningDetail
    public class FabricPlanningDetail : BusinessObject
    {
        public FabricPlanningDetail()
        {
            FabricPlanningDetailID = 0;
            FabricPlanningID = 0;
            ColNo = 0;
            EndsCount = 0;
            SLNo = 0;
            RepeatNo = 0;
            ErrorMessage = "";
        }

        #region Property
        public int FabricPlanningDetailID { get; set; }
        public int FabricPlanningID { get; set; }
        public int ColNo { get; set; }
        public int EndsCount { get; set; }
        public int SLNo { get; set; }
        public int RepeatNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions
        public static List<FabricPlanningDetail> Gets(long nUserID)
        {
            return FabricPlanningDetail.Service.Gets(nUserID);
        }
        public static List<FabricPlanningDetail> Gets(string sSQL, long nUserID)
        {
            return FabricPlanningDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricPlanningDetail Get(int id, long nUserID)
        {
            return FabricPlanningDetail.Service.Get(id, nUserID);
        }
        public FabricPlanningDetail Save(long nUserID)
        {
            return FabricPlanningDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricPlanningDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricPlanningDetailService Service
        {
            get { return (IFabricPlanningDetailService)Services.Factory.CreateService(typeof(IFabricPlanningDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricPlanningDetail interface
    public interface IFabricPlanningDetailService
    {
        FabricPlanningDetail Get(int id, Int64 nUserID);
        List<FabricPlanningDetail> Gets(Int64 nUserID);
        List<FabricPlanningDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricPlanningDetail Save(FabricPlanningDetail oFabricPlanningDetail, Int64 nUserID);
    }
    #endregion
}
