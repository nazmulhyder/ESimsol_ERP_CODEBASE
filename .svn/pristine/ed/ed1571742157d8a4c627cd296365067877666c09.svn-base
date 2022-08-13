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
    #region GUProductionProcedure
    
    public class GUProductionProcedure : BusinessObject
    {
        public GUProductionProcedure()
        {
            GUProductionProcedureID = 0;
            GUProductionOrderID = 0;
            ProductionStepID = 0;
            Sequence = 0;
            Remarks = "";
            StepName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int GUProductionProcedureID { get; set; }
         
        public int GUProductionOrderID { get; set; }
         
        public int ProductionStepID { get; set; }
         
        public int Sequence { get; set; }
         
        public string Remarks { get; set; }
         
        public string StepName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<GUProductionProcedure> Gets(long nUserID)
        {
            return GUProductionProcedure.Service.Gets( nUserID);
        }

        public static List<GUProductionProcedure> Gets(int nGUProductionOrderID, long nUserID)
        {
            return GUProductionProcedure.Service.Gets(nGUProductionOrderID, nUserID);
        }

        public static List<GUProductionProcedure> GetsbyOrderRecap(int nOrderRecapID, long nUserID)
        {
           return GUProductionProcedure.Service.GetsbyOrderRecap(nOrderRecapID, nUserID);
        }
        public static List<GUProductionProcedure> Gets_byPOIDs(string sPOIDs, long nUserID)
        {
           return GUProductionProcedure.Service.Gets_byPOIDs(sPOIDs, nUserID);
        }
        public GUProductionProcedure Get(int nGUProductionOrderID, long nUserID)
        {
            return GUProductionProcedure.Service.Get(nGUProductionOrderID, nUserID);
        }

        public static List<GUProductionProcedure> Save(GUProductionOrder oGUProductionOrder, long nUserID)
        {
            
            return GUProductionProcedure.Service.Save(oGUProductionOrder, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
               return GUProductionProcedure.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IGUProductionProcedureService Service
        {
            get { return (IGUProductionProcedureService)Services.Factory.CreateService(typeof(IGUProductionProcedureService)); }
        }

        #endregion
    }
    #endregion

    #region IGUProductionProcedure interface
     
    public interface IGUProductionProcedureService
    {
         
        GUProductionProcedure Get(int id, Int64 nUserID);
         
        List<GUProductionProcedure> Gets(Int64 nUserID);
         
        List<GUProductionProcedure> Gets(int nGUProductionOrderID, Int64 nUserID);
         
        List<GUProductionProcedure> GetsbyOrderRecap(int nOrderRecapID, Int64 nUserID);
         
        List<GUProductionProcedure> Gets_byPOIDs(string sPOIDs, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        List<GUProductionProcedure> Save(GUProductionOrder oGUProductionOrder, Int64 nUserID);
    }
    #endregion
}
