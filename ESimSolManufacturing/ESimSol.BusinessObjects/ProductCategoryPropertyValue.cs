using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{ 

    #region ProductCategoryPropertyValueInformation
    
    public class ProductCategoryPropertyValue : BusinessObject
    {
        public ProductCategoryPropertyValue()
        {
            PCPVID = 0;
            PCPID = 0;
            ErrorMessage = "";
            PropertyValueID = 0;
            Note = "N/A";
        }

        #region Properties
        
        public int PCPVID { get; set; }
        
        public int PCPID { get; set; }
        
        public int PropertyValueID { get; set; } 
        
        public string Note { get; set; }        
        #endregion

        #region Derived Property
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions

        public static List<ProductCategoryPropertyValue> Gets(int nPCPID, long nUserID)
        {
            return ProductCategoryPropertyValue.Service.Gets(nPCPID, nUserID);
        }

        public static List<ProductCategoryPropertyValue> Gets(string sSQL,long nUserID)
        {
            return ProductCategoryPropertyValue.Service.Gets(sSQL, nUserID);
        }

        public ProductCategoryPropertyValue Get(int id, long nUserID)
        {
            return ProductCategoryPropertyValue.Service.Get(id, nUserID);
        }
        public string Insert(ProductCategoryProperty oPCP, long nUserID)
        {
            return ProductCategoryPropertyValue.Service.Insert(oPCP, nUserID);
        }
       #endregion

        #region ServiceFactory
        internal static IProductCategoryPropertyValueService Service
        {
            get { return (IProductCategoryPropertyValueService)Services.Factory.CreateService(typeof(IProductCategoryPropertyValueService)); }
        }
        #endregion
    }
    #endregion

    #region IProductCategoryPropertyValueInformation interface
    
    public interface IProductCategoryPropertyValueService
    {
        
        ProductCategoryPropertyValue Get(int id, Int64 nUserID);
        
        List<ProductCategoryPropertyValue> Gets(int nPCPID, Int64 nUserID);
        
        List<ProductCategoryPropertyValue> Gets(string sSQL, Int64 nUserID);
        
        string Insert(ProductCategoryProperty oPCP, Int64 nUserID);
     }
    #endregion
}