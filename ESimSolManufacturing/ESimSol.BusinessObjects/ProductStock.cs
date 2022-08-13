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
    #region Product    
    public class ProductStock
    {
        public ProductStock()
        {
            ProductID = 0;
            BUID = 0;
            ProductCode = "";
            ProductName = "";
            MeasurementUnitID = 0;
            ShortName = "";            
            ProductCategoryID = 0;
            ProductCategoryName = "";
            MUnitName = "";
            MUnit = "";
            CurrentStock = 0;
            StockValue = 0;            
            ShortQty = 0;
            IsReport = false;
            ErrorMessage = ""; 
        }

        #region Properties
        public int ProductID { get; set; }
        public int BUID { get; set; }        
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MeasurementUnitID { get; set; }
        public string ShortName { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string MUnitName { get; set; }
        public string MUnit { get; set; }        
        public double ShortQty { get; set; }
        public double CurrentStock { get; set; }        
        public double StockValue { get; set; }
        public bool IsReport { get; set; }
        public string ErrorMessage { get; set; }                
        #endregion

        #region Derived Properties
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }                
        #endregion

        #region Functions        
        public static List<ProductStock> Gets(ProductStock oProductStock, int nUserID)
        {
            return ProductStock.Service.Gets(oProductStock, nUserID);
        }
        public ProductStock SetReorderLevel(int nUserID)
        {
            return ProductStock.Service.SetReorderLevel(this, nUserID);
        }           
        #endregion

        
        #region ServiceFactory
        internal static IProductStockService Service
        {
            get { return (IProductStockService)Services.Factory.CreateService(typeof(IProductStockService)); }
        }
        #endregion
    }
    #endregion

    #region IProductStock interface
    public interface IProductStockService
    {
        ProductStock SetReorderLevel(ProductStock oProductStock, int nUserID);
        List<ProductStock> Gets(ProductStock oProductStock, int nUserID);     
    }
    #endregion
}
