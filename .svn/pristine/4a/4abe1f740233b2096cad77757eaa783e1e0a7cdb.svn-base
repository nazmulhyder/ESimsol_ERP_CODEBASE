using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.BusinessObjects
{
    #region FabricDeliverySchedule
    
    public class FabricDeliverySchedule : BusinessObject
    {

        #region  Constructor
        public FabricDeliverySchedule()
        {
            FabricDeliveryScheduleID = 0;
            DeliveryOrderNameID = 0;
            DeliveryDate = DateTime.Now;
            FabricSalesContractID = 0;
            Qty = 0;
            Note = "";
            Name = "";
            DeliveryAddress = "";
            Note = "";
            DeliveryToName="";
            ErrorMessage = "";
            IsOwn = false;
            IsFoc = false;
            DeliveryToID = 0;
            IsGrey = false;
        }
        #endregion

        #region Properties
      
        public int FabricDeliveryScheduleID { get; set; }
        public int DeliveryOrderNameID { get; set; }
        public int FabricSalesContractID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public double Qty { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsOwn { get; set; }
        public bool IsFoc { get; set; }
        public bool IsGrey { get; set; }
        public int DeliveryToID { get; set; }
        public string DeliveryToName { get; set; }
        public string ErrorMessage { get; set; }
     
        
        #endregion

        #region Derived Property
        public string DeliveryDateSt
        {
            get
            {
              
                if (this.DeliveryDate == DateTime.MinValue)
                {
                    return DateTime.Now.ToString("dd MMM yyyy");
                }
                else
                {
                    return this.DeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public FabricDeliverySchedule Get(int nFabricDeliveryScheduleID, Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.Get(nFabricDeliveryScheduleID, nUserID);
        }
        public FabricDeliverySchedule Save(Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.Save(this, nUserID);
        }
      
        public string Delete(Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.Delete(this, nUserID);
        }
     
        public static List<FabricDeliverySchedule> Gets(Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.Gets(nUserID);
        }
        public static List<FabricDeliverySchedule> Gets(string sSQL, Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.Gets(sSQL,nUserID);
        }
        public static List<FabricDeliverySchedule> GetsFSCID(int nFSCID, Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.GetsFSCID(nFSCID, nUserID);
        }
        public static List<FabricDeliverySchedule> GetsFSCIDLog(int nFSCID, Int64 nUserID)
        {
            return FabricDeliverySchedule.Service.GetsFSCIDLog(nFSCID, nUserID);
        }

     
  
        #endregion

        #region ServiceFactory
      
        internal static IFabricDeliveryScheduleService Service
        {
            get { return (IFabricDeliveryScheduleService)Services.Factory.CreateService(typeof(IFabricDeliveryScheduleService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricDeliverySchedule interface
    
    public interface IFabricDeliveryScheduleService
    {
    
        FabricDeliverySchedule Get(int id, Int64 nUserID);
        List<FabricDeliverySchedule> GetsFSCID(int nFSCID, Int64 nUserID);
        List<FabricDeliverySchedule> GetsFSCIDLog(int nFSCID, Int64 nUserID);
        List<FabricDeliverySchedule> Gets(Int64 nUserID);
        List<FabricDeliverySchedule> Gets(string sSQL, Int64 nUserID);
        FabricDeliverySchedule Save(FabricDeliverySchedule oFabricDeliverySchedule, Int64 nUserID);
        string Delete(FabricDeliverySchedule oFabricDeliverySchedule, Int64 nUserID);
      
    }
    #endregion
}
