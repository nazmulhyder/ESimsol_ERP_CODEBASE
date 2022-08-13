using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{

    #region FabricBatchProductionColor
    public class FabricBatchProductionColor : BusinessObject
    {
        public FabricBatchProductionColor()
        {
            FBPColorID = 0;
            FBPBID = 0;
            Note = "";
            Qty = 0;
            Name = "";
            FabricBatchProductionColors = new List<FabricBatchProductionColor>();
            ErrorMessage = "";
        }

        #region Properties
        public string Note { get; set; }
        public int FBPBID { get; set; }
        public int FBPColorID { get; set; }
        public string Name { get; set; }
        public double Qty { get; set; }

        public string ErrorMessage { get; set; }
       
        public List<FabricBatchProductionColor> FabricBatchProductionColors { get; set; }
        public FabricBatch FabricProductionBatch { get; set; }

        #endregion

        #region Derived Properties
        public FabricBatch FabricBatch { get; set; }
        public double QtyInM
        {
            get
            {
                return Global.GetMeter(this.Qty,2);
            }
        }
        #endregion

        #region Functions

        public static List<FabricBatchProductionColor> Gets(int nFBPBID, long nUserID)
        {
            return FabricBatchProductionColor.Service.Gets(nFBPBID, nUserID);
        }
        public FabricBatchProductionColor Save(long nUserID)
        {
            return FabricBatchProductionColor.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchProductionColor.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchProductionColorService Service
        {
            get { return (IFabricBatchProductionColorService)Services.Factory.CreateService(typeof(IFabricBatchProductionColorService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchProductionColor interface

    public interface IFabricBatchProductionColorService
    {
        List<FabricBatchProductionColor> Gets(int nFBPBID, long nUserID);
        FabricBatchProductionColor Save(FabricBatchProductionColor oFabricBatchProductionColor, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
    

}
