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
    
    public class ProductCategoryPropertyInformation : BusinessObject
    {
        public ProductCategoryPropertyInformation()
        {
            PCPID = 0;
            ProductCategoryID = 0;
            PropertyValueID = 0;
            Note = "";
            ErrorMessage = "";
            PropertyValue = "";
            PropertyID = 0;
        }

        #region Properties
        
        public int PCPID { get; set; }
        
        public int ProductCategoryID { get; set; }
        
        public int PropertyValueID { get; set; }
        
        public string Note { get; set; }        
        #endregion

        #region Derived Property
        public List<ProductCategoryPropertyInformation> PCPIs { get; set; }
        public string SelecetedProduct { get; set; }
        public List<PropertyValue> PropertyValueList { get; set; }
        public List<Property> Properties { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string PropertyValue { get; set; }
        
        public int PropertyID { get; set; }        
        
        public string ProductCategoryName { get; set; }
        #endregion

        #region Functions

        public static List<ProductCategoryPropertyInformation> Gets(int nPCID, long nUserID)
        {
            return ProductCategoryPropertyInformation.Service.Gets(nPCID, nUserID);
        }

        public static List<ProductCategoryPropertyInformation> Gets(string sSQL,long nUserID)
        {
            return ProductCategoryPropertyInformation.Service.Gets(sSQL, nUserID);
        }

        public ProductCategoryPropertyInformation Get(int id, long nUserID)
        {
            return ProductCategoryPropertyInformation.Service.Get(id, nUserID);
        }

        public ProductCategoryPropertyInformation IUD(int nDBOperation,long nUserID)
        {
            return ProductCategoryPropertyInformation.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductCategoryPropertyInformationService Service
        {
            get { return (IProductCategoryPropertyInformationService)Services.Factory.CreateService(typeof(IProductCategoryPropertyInformationService)); }
        }
        #endregion
    }
    #endregion

    #region IProductCategoryPropertyInformation interface
    
    public interface IProductCategoryPropertyInformationService
    {
        
        ProductCategoryPropertyInformation Get(int id, Int64 nUserID);
        
        List<ProductCategoryPropertyInformation> Gets(int nPCID,Int64 nUserID);
        
        List<ProductCategoryPropertyInformation> Gets(string sSQL, Int64 nUserID);
        
        ProductCategoryPropertyInformation IUD(ProductCategoryPropertyInformation oProductCategoryPropertyInformation, int nDBoperation,Int64 nUserID);
    }
    #endregion
}