using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region FabricTransferableLot
    public class FabricTransferableLot : BusinessObject
    {
        #region  Constructor
        public FabricTransferableLot()
        {
            FabricTransferableLotID = 0;
            LotNo = "";
            Qty = 0;
            Product = "";
            RollQty = 0;
        }
        #endregion

        #region Properties

        public int FabricTransferableLotID { get; set; }
        public int LotID { get; set; }
        public int WorkingUnitID { get; set; }
        public double Qty { get; set; }
        public string LotNo { get; set; }
        public string Product { get; set; }
        public string WUName { get; set; }
        public string OrderType { get; set; }
        public string ErrorMessage { get; set; }
        public int WorkingUnitID_Recd { get; set; }
        public int RollQty { get; set; }
        #endregion

        #region Functions
        public static List<FabricTransferableLot> Gets(long nUserID)
        {
            return FabricTransferableLot.Service.Gets(nUserID);
        }
        public static List<FabricTransferableLot> SendToRequsition(List<FabricTransferableLot> oItems, long nUserID)
        {
            return FabricTransferableLot.Service.SendToRequsition(oItems, nUserID);
        }
        public string Delete(long nUserID)
        {
            return FabricTransferableLot.Service.Delete(this, nUserID);
        }
        public string TransferToStore(long nUserID)
        {
            return FabricTransferableLot.Service.TransferToStore(this, nUserID);
        }
        public string LotAdjustment(long nUserID)
        {
            return FabricTransferableLot.Service.LotAdjustment(this, nUserID);
        }


        #endregion

        #region Non DB Function

        #endregion
        #region ServiceFactory


        internal static IFabricTransferableLotService Service
        {
            get { return (IFabricTransferableLotService)Services.Factory.CreateService(typeof(IFabricTransferableLotService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricTransferableLot interface
    public interface IFabricTransferableLotService
    {
        List<FabricTransferableLot> Gets(Int64 nUserID);
        List<FabricTransferableLot> SendToRequsition(List<FabricTransferableLot> oFabricTransferableLot, long nUserID);
        string Delete(FabricTransferableLot oFabricTransferableLot, Int64 nUserID);
        string TransferToStore(FabricTransferableLot oFabricTransferableLot, Int64 nUserID);
        string LotAdjustment(FabricTransferableLot oFabricTransferableLot, Int64 nUserID);
    }

    #endregion

}