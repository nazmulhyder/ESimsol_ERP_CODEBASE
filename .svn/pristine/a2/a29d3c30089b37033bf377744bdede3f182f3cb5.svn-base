using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region GUProductionTracingUnitDetail
    
    public class GUProductionTracingUnitDetail : BusinessObject
    {
        public GUProductionTracingUnitDetail()
        {
            GUProductionTracingUnitDetailID = 0;
            GUProductionTracingUnitID = 0;
            GUProductionProcedureID = 0;
            ProductionStepID = 0;
            ExecutionQty = 0;
            YetToExecutionQty = 0;
            ExecutionStartDate = DateTime.Now;
            StepName = "";
            Sequence = 0;
            ErrorMessage = "";
            PTUTransections = new List<PTUTransection>();
        }

        #region Properties
         
        public int GUProductionTracingUnitDetailID { get; set; }
         
        public int GUProductionTracingUnitID { get; set; }

        public int GUProductionProcedureID { get; set; }
         
        public int ProductionStepID { get; set; }
         
        public double ExecutionQty { get; set; }
         
        public double YetToExecutionQty { get; set; }
         
        public DateTime ExecutionStartDate { get; set; }
         
        public int Sequence { get; set; }
         
        public string StepName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PTUTransection> PTUTransections { get; set; }
        #endregion


        #region Function
        public static List<GUProductionTracingUnitDetail> Gets(long nUserID)
        {
            return GUProductionTracingUnitDetail.Service.Gets(nUserID);
        }

        public static List<GUProductionTracingUnitDetail> Gets(int PTUID, long nUserID)
        {
            return GUProductionTracingUnitDetail.Service.Gets(PTUID, nUserID);
        }
        public static List<GUProductionTracingUnitDetail> Gets(string sql, long nUserID)
        {
            return GUProductionTracingUnitDetail.Service.Gets(sql, nUserID);
        }

        public static List<GUProductionTracingUnitDetail> GetsByOrderRecap( int nOrderRecapID, long nUserID)
        {
            return GUProductionTracingUnitDetail.Service.GetsByOrderRecap(nOrderRecapID, nUserID);
        }
        public static List<GUProductionTracingUnitDetail> Gets_byPOIDs(string sPOIDs, long nUserID)
        {
            return GUProductionTracingUnitDetail.Service.Gets_byPOIDs(sPOIDs, nUserID);
        }
        
        #endregion


        #region ServiceFactory

    
        internal static IGUProductionTracingUnitDetailService Service
        {
            get { return (IGUProductionTracingUnitDetailService)Services.Factory.CreateService(typeof(IGUProductionTracingUnitDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IGUProductionTracingUnitDetail interface
     
    public interface IGUProductionTracingUnitDetailService
    {
         
        List<GUProductionTracingUnitDetail> Gets(Int64 nUserID);
        List<GUProductionTracingUnitDetail> Gets(int PUID, Int64 nUserID);
        List<GUProductionTracingUnitDetail> Gets(string sql, Int64 nUserID);
         
        List<GUProductionTracingUnitDetail> GetsByOrderRecap(int nOrderRecapID, Int64 nUserID);
         
        List<GUProductionTracingUnitDetail> Gets_byPOIDs(string sPOIDs, Int64 nUserID);
        
    }
    #endregion
}
