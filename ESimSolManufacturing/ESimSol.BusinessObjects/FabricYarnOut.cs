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
    #region FabricYarnOut
    public class FabricYarnOut : BusinessObject
    {
        public FabricYarnOut()
        {
            FPBID = 0;
            ProductID = 0;
            ProductName = "";
            LotID = 0;
            LotNo = "";
            CurrentStock = 0; //Lot Balance
            Measured = 0;
            ConsumeQty = 0;
            Qty = 0;

            TriggerParentID = 0;
            ITransactionID = 0;
            FabricYarnOuts = new List<FabricYarnOut>();
            ErrorMessage = "";
        }

        #region Properties
        public int FPBID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public double CurrentStock { get; set; }
        public double Measured { get; set; }
        public double ConsumeQty { get; set; }
        public double Qty { get; set; }
        public string ErrorMessage { get; set; }

        public int TriggerParentID { get; set; }
        public int ITransactionID { get; set; }

        public List<FabricYarnOut> FabricYarnOuts { get; set; }
        //public FabricBatch FabricProductionBatch { get; set; }
        #endregion

        #region Functions

        public static List<FabricYarnOut> Gets(int nFPBID, long nUserID)
        {
            return FabricYarnOut.Service.Gets(nFPBID, nUserID);
        }
        public FabricYarnOut Save(long nUserID)
        {
            return FabricYarnOut.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricYarnOutService Service
        {
            get { return (IFabricYarnOutService)Services.Factory.CreateService(typeof(IFabricYarnOutService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricYarnOut interface

    public interface IFabricYarnOutService
    {
        List<FabricYarnOut> Gets(int nFPBID, long nUserID);
        FabricYarnOut Save(FabricYarnOut oFabricYarnOut, long nUserID);
    }
    #endregion
}