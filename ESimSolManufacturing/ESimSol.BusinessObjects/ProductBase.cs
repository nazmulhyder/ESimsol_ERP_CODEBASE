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
    #region ProductBase
    [DataContract]
    public class ProductBase : BusinessObject
    {
        public ProductBase()
        {
            ProductBaseID = 0;
            ProductCategoryID = 0;
            ProductCode= "";
            ProductName = "";
            ShortName= "";
            IsActivate=false;
            Note="";
            ManufacturerModelCode = "";
            ProductCategoryName = "";
            ErrorMessage = "";
            PPIs = new List<ProductPropertyInformation>();
            Params = "";
        }

        #region Properties
        public string Params { get; set; }
        public int ProductBaseID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public bool IsActivate { get; set; }
        public string Note { get; set; }
        public string ManufacturerModelCode { get; set; }
        public string ProductCategoryName { get; set; }
     

        
        #endregion

        #region Derived Property
        public string ErrorMessage { get; set; }
      
        public string ActivityinString
        {
            get
            {
                if (this.IsActivate)
                {
                    return "Active";
                }
                else
                {
                    return "InActive";
                }

            }
        }

        public string ProductNameandCode
        {
            get
            {
                if (ProductCode == null)
                {
                    return ProductName.ToString();
                }
                else
                {
                    return ProductName.ToString() + "[" + ProductCode.ToString() + "]";
                    
                }
            }
        }
        public string PropertyIDs { get; set; }
        public string PropertyValueIDs { get; set; }
        public string ProductCategoryIDs { get; set; }
        public List<PropertyValue> PropertyValues { get; set; }
        
        public ProductPropertyInformation ProductPropertyInformation { get; set; }
        public List<ProductPropertyInformation> PPIs { get; set; }
        public List<ProductCategoryProperty> PCPs { get; set; }
        #endregion


        #region Functions
        public static List<ProductBase> Gets(long nUserID)
        {
            return ProductBase.Service.Gets(nUserID);
        }
        public static List<ProductBase> GetsByCategory(int nCategoryID,long nUserID)
        {
            return ProductBase.Service.GetsByCategory(nCategoryID,nUserID);
        }

        public ProductBase Get(int nId, int nUserID)
        {
            return ProductBase.Service.Get(nId, nUserID);
        }


        public ProductBase Save(long nUserID)
        {
            return ProductBase.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ProductBase.Service.Delete(nId, nUserID);
        }
        public static List<ProductBase> Gets(string sSQL,long nUserID)
        {
            return ProductBase.Service.Gets(sSQL,nUserID);
        }
      

        #endregion

        #region ServiceFactory
        internal static IProductBaseService Service
        {
            get { return (IProductBaseService)Services.Factory.CreateService(typeof(IProductBaseService)); }
        }
     
        #endregion
    }
    #endregion
    #region IProductBase interface
    public interface IProductBaseService
    {
        ProductBase Get(int id, Int64 nUserID);
        List<ProductBase> GetsByCategory(int nCategoryID, Int64 nUserID);
        List<ProductBase> Gets(Int64 nUserID);
        ProductBase Save(ProductBase oProductBase, Int64 nUserID);
      //  List<ProductBase> Save(ProductBase oProductBase, Int64 nUserID);
        List<ProductBase> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
    }
    #endregion
}