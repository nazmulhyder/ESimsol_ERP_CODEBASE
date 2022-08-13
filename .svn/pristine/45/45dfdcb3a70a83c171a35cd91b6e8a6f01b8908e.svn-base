using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects.ReportingObject
{
    public class rptFabricTransferWeavingToFinishing
    {
        public rptFabricTransferWeavingToFinishing()
        {
            FTNDate = DateTime.MinValue;
            BuyerName = string.Empty;
            FEONo = string.Empty;
            ProcessName = string.Empty;
            WarpLot = string.Empty;
            WeftLot = string.Empty;
            FTNNo = string.Empty;
            DeliveryQty = 0;
            PartyChallan = string.Empty;
        }
        #region Properties
        public DateTime FTNDate { get; set; }
        public string BuyerName { get; set; }
        public string FEONo { get; set; }
        public string Construction { get; set; }
        public string WarpLot { get; set; }
        public string WeftLot { get; set; }
        public string ProcessName { get; set; }
        public string FTNNo { get; set; }
        public double DeliveryQty { get; set; }
        public string PartyChallan { get; set; }
        public double ReceivedQty { get; set; }
        public double ProcessLossGain { get; set; }
        public string Remarks { get; set; }
        #endregion
        #region Derived Properties
        public double DeliveryQtyMeter { get { return Global.GetMeter(this.DeliveryQty, 2); } }
        public string FTNDateStr { get { return (this.FTNDate==DateTime.MinValue)?"":this.FTNDate.ToString("dd MMM yyyy"); } }
        #endregion
        #region Functions

        public static List<rptFabricTransferWeavingToFinishing> Gets(DateTime FromDate ,DateTime ToDate,String SFEOIDs ,string sBuyerIDs, long nUserID)
        {
            return rptFabricTransferWeavingToFinishing.Service.Gets(FromDate,ToDate,SFEOIDs,sBuyerIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IrptFabricTransferWeavingToFinishingService Service
        {
            get { return (IrptFabricTransferWeavingToFinishingService)Services.Factory.CreateService(typeof(IrptFabricTransferWeavingToFinishingService)); }
        }
        #endregion
    }
    #region ISUMachine interface

    public interface IrptFabricTransferWeavingToFinishingService
    {
        List<rptFabricTransferWeavingToFinishing> Gets(DateTime FromDate, DateTime ToDate, String SFEOIDs, string sBuyerIDs, long nUserID);
    }
    #endregion
}
