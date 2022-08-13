using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductSort
    
    public class ProductSort : BusinessObject
    {
        public ProductSort()
        {
            ProductSortID = 0;
            ErrorMessage = "";
            ProductCode = "";
            ProductName = "";
            SortType = 0;
            ProductNameBulk = "";
            Qty_Grace = 0;
            DUDyeingTypeMappings = new List<DUDyeingTypeMapping>();
            Value = 0;
        }

        #region Properties
        public int ProductSortID { get; set; }
        public string ProductCode { get; set; }
        public int ProductID { get; set; }
        public int ProductID_Bulk { get; set; }
        public int SortType { get; set; }
        public double Qty_Grace { get; set; }
        public string ErrorMessage { get; set; }
        public double Value { get; set; }
        
        #region Derived Property
        public string ProductName { get; set; }
        public string ProductNameBulk { get; set; }
        public List<DUDyeingTypeMapping> DUDyeingTypeMappings { get; set; }
        #endregion

        #endregion

        #region Functions
        public static List<ProductSort> Gets(long nUserID)
        {
            return ProductSort.Service.Gets(nUserID);
        }
        public ProductSort Get(int id, long nUserID)
        {
            return ProductSort.Service.Get(id, nUserID);
        }
        public ProductSort GetBy(int nProductID, long nUserID)
        {
            return ProductSort.Service.GetBy(nProductID, nUserID);
        }
        public ProductSort Save(long nUserID)
        {
            return ProductSort.Service.Save(this, nUserID);
        }
     
        public string Delete(long nUserID)
        {
            return ProductSort.Service.Delete(this, nUserID);
        }
        public static List<ProductSort> GetsBy(int nProductID_Bulk,long nUserID)
        {
            return ProductSort.Service.GetsBy(nProductID_Bulk,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProductSortService Service
        {
            get { return (IProductSortService)Services.Factory.CreateService(typeof(IProductSortService)); }
        }
        #endregion
    }
    #endregion


    #region IProductSort interface
    
    public interface IProductSortService
    {
        
        ProductSort Get(int id, Int64 nUserID);
        ProductSort GetBy(int nProductID, Int64 nUserID);
        List<ProductSort> GetsBy(int nProductID_Bulk,Int64 nUserID);
        List<ProductSort> Gets(Int64 nUserID);
        string Delete(ProductSort oProductSort, Int64 nUserID);
        ProductSort Save(ProductSort oProductSort, Int64 nUserID);
       
    }
    #endregion
}