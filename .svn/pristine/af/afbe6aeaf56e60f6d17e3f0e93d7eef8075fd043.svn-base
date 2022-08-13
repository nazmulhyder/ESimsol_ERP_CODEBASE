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

    #region FabricBatchRawMaterial
    public class FabricBatchRawMaterial : BusinessObject
    {
        public FabricBatchRawMaterial()
        {
            FBID = 0;
            FBRMID = 0;
            ProductID = 0;
            LotID = 0;
            LotNo = "";
            Qty = 0;
            ProductCode = "";
            ProductName = "";
            WeavingProcess = EnumWeavingProcess.Warping;
            ReceiveBy = 0;
            ReceiveByName = "";
            OnlyLotNo = "";
            ColorName = "";
             ReceiveByDate = DateTime.MinValue;
            FabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            IsChemicalOut = false;
            ErrorMessage = "";
            FabricChemicalPlanID = 0;
            ChemicalPlanQty = 0;
        }

        #region Properties
  
        public EnumWeavingProcess WeavingProcess { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int FBRMID { get; set; }
        public int FBID { get; set; }
        public string ReceiveByName { get; set; }
        public string OnlyLotNo { get; set; }
        public double Qty { get; set; }
        public double ChemicalPlanQty { get; set; }
        public string ErrorMessage { get; set; }
        public string ColorName { get; set; }
        public int FabricChemicalPlanID { get; set; }
        public string ProductCode { get; set; }
        public int ReceiveBy { get; set; }
       
        public bool IsChemicalOut { get; set; }
        
        public DateTime ReceiveByDate { get; set; }
        public List<FabricBatchRawMaterial> FabricBatchRawMaterials { get; set; }
        public FabricBatch FabricProductionBatch { get; set; }
        public string ReceiveByDateInString
        {
            get
            {
                if(this.ReceiveByDate==DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ReceiveByDate.ToString("dd MMM yyyy");
                }
                
            }
        }

        #endregion

        #region Derived Properties
        public FabricBatch FabricBatch { get; set; }
        public string UnitName { get; set; }
        public string StoreName { get; set; }
        #endregion

        #region Functions

        public static List<FabricBatchRawMaterial> Gets(int nFBID, EnumWeavingProcess nWevinProcess, long nUserID)
        {
            return FabricBatchRawMaterial.Service.Gets(nFBID, (int)nWevinProcess, nUserID);
        }
        public static List<FabricBatchRawMaterial> Gets(string sSQL, long nUserID)
        {
            return FabricBatchRawMaterial.Service.Gets(sSQL, nUserID);
        }
        public FabricBatchRawMaterial Save(long nUserID)
        {
            return FabricBatchRawMaterial.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricBatchRawMaterial.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricBatchRawMaterialService Service
        {
            get { return (IFabricBatchRawMaterialService)Services.Factory.CreateService(typeof(IFabricBatchRawMaterialService)); }
        }
        #endregion
    }
    #endregion


    #region IFabricBatchRawMaterial interface

    public interface IFabricBatchRawMaterialService
    {
        List<FabricBatchRawMaterial> Gets(int nFBID, int nWevinProcess, long nUserID);
        List<FabricBatchRawMaterial> Gets(string sSQL, long nUserID);
        FabricBatchRawMaterial Save(FabricBatchRawMaterial oFabricBatchRawMaterial, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
    

}
