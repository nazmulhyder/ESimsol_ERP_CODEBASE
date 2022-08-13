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
    #region GUProductionOrderHistory
    
    public class GUProductionOrderHistory : BusinessObject
    {
        public GUProductionOrderHistory()
        {
            GUProductionOrderHistoryID = 0;
            GUProductionOrderID = 0;
            PreviousStatus = EnumGUProductionOrderStatus.Initialized;
            CurrentStatus = EnumGUProductionOrderStatus.Initialized;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int GUProductionOrderHistoryID { get; set; }
         
        public int GUProductionOrderID { get; set; }
         
        public EnumGUProductionOrderStatus PreviousStatus { get; set; }
         
        public EnumGUProductionOrderStatus CurrentStatus { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        #endregion

        #region Functions

        public static List<GUProductionOrderHistory> Gets(long nUserID)
        {
            return GUProductionOrderHistory.Service.Gets(nUserID);
        }

        public GUProductionOrderHistory Get(int id, long nUserID)
        {
            return GUProductionOrderHistory.Service.Get(id, nUserID);
        }

        public GUProductionOrderHistory Save(long nUserID)
        {
            
            return GUProductionOrderHistory.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {

            return GUProductionOrderHistory.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IGUProductionOrderHistoryService Service
        {
            get { return (IGUProductionOrderHistoryService)Services.Factory.CreateService(typeof(IGUProductionOrderHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IGUProductionOrderHistory interface
     
    public interface IGUProductionOrderHistoryService
    {
         
        GUProductionOrderHistory Get(int id, Int64 nUserID);
         
        List<GUProductionOrderHistory> Gets(Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        GUProductionOrderHistory Save(GUProductionOrderHistory oGUProductionOrderHistory, Int64 nUserID);
    }
    #endregion
}
