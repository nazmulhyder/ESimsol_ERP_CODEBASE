using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
  public class FabricInHouseChallan
    {
        public FabricInHouseChallan()
        {
            ChallanID = 0;
            ChallanNo = string.Empty;
            ChallanDate = DateTime.MinValue;
            ContractorID = 0;
            VehicleNo = string.Empty;
            GatePassNo = string.Empty;
            DeliveredBy = 0;
            DEOID = 0;
            DEONo = string.Empty;
            FEOID = 0;
            FEONo = string.Empty;
            DeliveryOrderID = 0;
            DeliveryOrderNo = string.Empty;
            ContractorName = string.Empty;
            DeliveredByName = string.Empty;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            TxtUnit = 0;
            IsALLRcv = false;
        }
        #region Properties
        public int ChallanID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int ContractorID { get; set; }
        public string VehicleNo  { get; set; }
        public string GatePassNo { get; set; }
        public int DeliveredBy { get; set; }
        public int DEOID { get; set; }
        public string DEONo  { get; set; }
        public int FEOID { get; set; }
        public string FEONo { get; set; }
        public int DeliveryOrderID { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string ContractorName { get; set; }
        public string DeliveredByName { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int TxtUnit { get; set; }
        public bool IsALLRcv { get; set; } 

        #endregion

        #region dreiverd Properties
        public string ChallanDateStr { get { return (this.ChallanDate == DateTime.MinValue) ? "" : this.ChallanDate.ToString("dd MMM yyyy"); } }
        public string DeliveredUnit { 
            get {
                if (this.TxtUnit ==1)
                {
                    return "Dyeing Unit";
                }
                if (this.TxtUnit == 2)
                {
                    return "Spinning Unit";
                }
                else 
                    return "Weaving Unit";
                //return (this.DEOID > 0) ? "Dyeing Unit" : "Spinning Unit"; 
            
            }
        
        }
        
        #endregion

        #region Functions
        public static List<FabricInHouseChallan> Gets(string sFEONo, string sChallanNo, string BuyerIDs, bool bIsDate, DateTime dtFrom, DateTime dtTo, bool IsAll, int nUnit, long nUserID)
        {
            return FabricInHouseChallan.Service.Gets(sFEONo, sChallanNo, BuyerIDs, bIsDate, dtFrom, dtTo, IsAll, nUnit, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricInHouseChallanService Service
        {
            get { return (IFabricInHouseChallanService)Services.Factory.CreateService(typeof(IFabricInHouseChallanService)); }
        }

        #endregion
    }
 
  #region  IWeavingInHouseChallan interface
  public interface IFabricInHouseChallanService
  {
      List<FabricInHouseChallan> Gets(string sFEONo, string sChallanNo, string BuyerIDs, bool bIsDate, DateTime dtFrom, DateTime dtTo, bool IsAll, int nUnit, Int64 nUserID);
  }
  #endregion
}
