using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region ProductionRecipe
    public class ProductionRecipe : BusinessObject
    {
        public ProductionRecipe()
        {
            ProductionRecipeID = 0;
            ProductionSheetID = 0;
            ProductID = 0;
            QtyInPercent = 0;
            MUnitID = 0;
            RequiredQty = 0;
            Remarks = "";
            ProductCode = "";
            ProductName = "";            
            MUName = "";
            OutQty = 0;
            QtyType = EnumQtyType.None;
            QtyTypeInt = 0;
            ErrorMessage = "";
            StockBalance = 0;
            MUSymbol = "";
            ReportingUnit = "";
            ReportingYetToRMOQty = 0;
            SheetNo = "";
            WUID = 0;
            RequisitionWiseYetToOutQty = 0;
            RequisitionQty = 0;
            ProductBaseID = 0;
            bIsColor = true;
            //extra
            StockUnitName = "";
            BUID = 0;
        }

        #region Property
        public int ProductionRecipeID { get; set; }
        public int ProductionSheetID { get; set; }
        public int ProductID { get; set; }
        public int ProductBaseID { get; set; }
        public double QtyInPercent { get; set; }
        public double RequiredQty { get; set; }
        public double OutQty { get; set; }
        public string Remarks { get; set; }       
        public string ProductCode { get; set; }
        public string ProductName { get; set; }       
        public string MUName { get; set; }
        public int MUnitID { get; set; }
        public EnumQtyType QtyType { get; set; }
        public int QtyTypeInt { get; set; }
        public string SheetNo { get; set; }
        public string ErrorMessage { get; set; }
        public double StockBalance { get; set; }
        public int WUID { get; set; }
        public string MUSymbol { get; set; }
        public string ReportingUnit { get; set; }
        public double ReportingYetToRMOQty { get; set; }
        public double RequisitionWiseYetToOutQty { get; set; }
        public double RequisitionQty { get; set; }
        public int BUID { get; set; }
        #endregion

        #region Derived Property
        public bool bIsColor { get; set; }
        public bool bIsChecked { get; set; }
        public string StockUnitName { get; set; }
        public int RMRequisitionID { get; set; }
        public double CurrentOutQty { get; set; }
    
        public string QtyTypeSt
        {
            get
            {
                return this.QtyType.ToString();
            }
        }
        public string QtySt
        {
            get
            {
                return Global.MillionFormat(this.RequiredQty,4) + this.MUSymbol;
            }
        }

        public double YetToOutQty
        {
            get
            {
                return this.RequiredQty - this.OutQty;
            }
        }

        public double YetToRequisitionQty
        {
            get
            {
                return this.RequiredQty - this.RequisitionQty;
            }
        }

        public string StockBalanceSt
        {
         get
            {
                return Global.MillionFormat(this.StockBalance,4) + this.StockUnitName;
            }
        }

        #endregion

        #region Functions
        public static List<ProductionRecipe> Gets(int nProductionSheetID, long nUserID)
        {
            return ProductionRecipe.Service.Gets(nProductionSheetID, nUserID);
        }
      
        public static List<ProductionRecipe> Gets(string sSQL, long nUserID)
        {
            return ProductionRecipe.Service.Gets(sSQL, nUserID);
        }
        public ProductionRecipe Get(int id, long nUserID)
        {
            return ProductionRecipe.Service.Get(id, nUserID);
        }

        public static List<ProductionRecipe> GetsByWU(int nPSID, int nWUID, int nRMRequisitionID, long nUserID)
        {
            return ProductionRecipe.Service.GetsByWU(nPSID, nWUID, nRMRequisitionID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductionRecipeService Service
        {
            get { return (IProductionRecipeService)Services.Factory.CreateService(typeof(IProductionRecipeService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionRecipe interface
    public interface IProductionRecipeService
    {
        ProductionRecipe Get(int id, Int64 nUserID);
        List<ProductionRecipe> Gets(int nORSID, Int64 nUserID);
     
        List<ProductionRecipe> Gets(string sSQL, Int64 nUserID);

        List<ProductionRecipe> GetsByWU(int nPSID, int nWUID, int nRMRequisitionID, Int64 nUserID);

    }
    #endregion
    
    
}
