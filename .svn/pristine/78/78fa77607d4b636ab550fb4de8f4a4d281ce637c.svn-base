using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{ 

    #region ProductCategoryPropertyInformation
    
    public class ProductCategoryProperty : BusinessObject
    {
        public ProductCategoryProperty()
        {
            PCPID = 0;
            ProductCategoryID = 0;
            Note = "";
            ErrorMessage = "";
            PropertyName = "";
            PropertyID = 0;
            IsMandatory = false;
        }

        #region Properties
        
        public int PCPID { get; set; }
        
        public int ProductCategoryID { get; set; }
        
        public int PropertyID { get; set; }
        
        public bool IsMandatory { get; set; } 
        
        public string Note { get; set; }        
        #endregion

        #region Derived Property
        public List<ProductCategoryProperty> PCPIs { get; set; }
        public string SelecetedProduct { get; set; }
        public List<Property> Properties { get; set; }
        
        public List<PropertyValue> PropertyValueList { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string PropertyName { get; set; }       
        
        public string ProductCategoryName { get; set; }
        #endregion

        #region Functions

        public static List<ProductCategoryProperty> Gets(int nPCID, long nUserID)
        {
            return ProductCategoryProperty.Service.Gets(nPCID, nUserID);
        }

        public static List<ProductCategoryProperty> Gets(string sSQL,long nUserID)
        {
            return ProductCategoryProperty.Service.Gets(sSQL, nUserID);
        }

        public ProductCategoryProperty Get(int id, long nUserID)
        {
            return ProductCategoryProperty.Service.Get(id, nUserID);
        }

        public ProductCategoryProperty IUD(int nDBOperation,long nUserID)
        {
            return ProductCategoryProperty.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductCategoryPropertyService Service
        {
            get { return (IProductCategoryPropertyService)Services.Factory.CreateService(typeof(IProductCategoryPropertyService)); }
        }
        #endregion
    }
    #endregion

    #region IProductCategoryPropertyInformation interface
    
    public interface IProductCategoryPropertyService
    {
        
        ProductCategoryProperty Get(int id, Int64 nUserID);
        
        List<ProductCategoryProperty> Gets(int nPCID,Int64 nUserID);
        
        List<ProductCategoryProperty> Gets(string sSQL, Int64 nUserID);
        
        ProductCategoryProperty IUD(ProductCategoryProperty oProductCategoryPropertyInformation, int nDBoperation,Int64 nUserID);
    }
    #endregion
}