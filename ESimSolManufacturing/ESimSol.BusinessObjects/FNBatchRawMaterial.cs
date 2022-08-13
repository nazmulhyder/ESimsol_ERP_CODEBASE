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
    #region FUProcess

    public class FNBatchRawMaterial : BusinessObject
    {
        public FNBatchRawMaterial()
        {
            FBRMID = 0;
            FNBatchID = 0;
            FabricID = 0;
            LotID = 0;
            Qty = 0;
            ReceiveBy = 0;
            ReceiveByDate = DateTime.Today;
            FNBatchRawMaterials = new List<FNBatchRawMaterial>();
            ErrorMessage = "";
            FNOrderFabricReceiveID = 0;

            //temp properties
            QtyTrIn = 0;
            QtyTrOut = 0;
            QtyReturn = 0;
            QtyCon = 0;
            TotalRcvQty = 0;
        }

        #region Properties

        public int FBRMID { get; set; }
        public int FNBatchID { get; set; }
        public int FabricID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public int ReceiveBy { get; set; }
        public DateTime ReceiveByDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int FNOrderFabricReceiveID { get; set; }

        //temp properties
        public double TotalRcvQty { get; set; }
        public double QtyTrIn { get; set; }
        public double QtyTrOut { get; set; }
        public double QtyReturn { get; set; }
        public double QtyCon { get; set; }
        #endregion

        #region Derived Property
        public string LotNo { get; set; }
        public double Balance { get; set; }
        public string FNBatchNo { get; set; }
        public string ReceiveByName { get; set; }
        public string MeasurementUnitName { get; set; }
        public string ReceiveByDateStr { get { return this.ReceiveByDate.ToString("dd MMM yyyy"); } }

        public List<FNBatchRawMaterial> FNBatchRawMaterials { get; set; }

        #endregion

        #region Functions

        public static FNBatchRawMaterial Get(int nFBRMID, long nUserID)
        {
            return FNBatchRawMaterial.Service.Get(nFBRMID, nUserID);
        }
        public static List<FNBatchRawMaterial> Gets(string sSQL, long nUserID)
        {
            return FNBatchRawMaterial.Service.Gets(sSQL, nUserID);
        }
        public FNBatchRawMaterial FabricOut(long nUserID)
        {
            return FNBatchRawMaterial.Service.FabricOut(this, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IFNBatchRawMaterialService Service
        {
            get { return (IFNBatchRawMaterialService)Services.Factory.CreateService(typeof(IFNBatchRawMaterialService)); }
        }

        #endregion
    }
    #endregion

    #region IFUProcess interface

    public interface IFNBatchRawMaterialService
    {

        FNBatchRawMaterial Get(int nFBRMID, Int64 nUserID);
        List<FNBatchRawMaterial> Gets(string sSQL, Int64 nUserID);
        FNBatchRawMaterial FabricOut(FNBatchRawMaterial oFNBatchRawMaterial, Int64 nUserID);
    }
    #endregion
}
