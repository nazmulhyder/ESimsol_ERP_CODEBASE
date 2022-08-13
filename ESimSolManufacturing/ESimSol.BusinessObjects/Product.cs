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
    public class Product :BusinessObject 
    {
        public Product()
        {
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            MeasurementUnitID = 0;
            ShortName = "";
            ApplyInventory = true;
            ApplyProperty = false;
            IsApplySizer = false;
            ProductCategoryID = 0;
            ProductCategoryName = "";
            MUnitName = "";
            MUnit = "";
            ErrorMessage = "";
            UnitType = EnumUniteType.None;
            UnitTypeInInt = 0;
            NameCode = "";
            Brand = "";
            ProductType = EnumProductType.None;
            ProductTypeInInt = 0;
            AccountHeadID = 0;
            AccountCode = "";
            AccountHeadName = "";
            IsSerialNoApply = false;
            BUID = 0;
            IsApplyColor = true;
            IsApplySize = true;
            IsApplyMeasurement = true;
            ModelReferenceID = 0;
            ModelReferenceName = "";
            ProductDescription = "";
            FinishGoodsWeight = 0;
            NaliWeight = 0;
            WeigthFor = 0;
            FinishGoodsUnit = 0;
            FGUSymbol = "";
            StockValue = 0;
            ReportingUnitID = 0;
            Activity = true;
            ShortQty = 0;
            ReportingUnitName = "";
            ReportingUnit = "";
            PPIs = new List<ProductPropertyInformation>();
            Params = "";
            PurchasePrice = 0;
            SalePrice = 0;
            QtyType = 0;
            Products = new List<Product>();
            LastUpdateDateTime = DateTime.MinValue;
            LastUpdateBy = 0;
            LastUpdateByName = "";
        }

        #region Properties
        public int ProductID { get; set; }
        public int ProductBaseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MeasurementUnitID { get; set; }
        public string ShortName { get; set; }///Part Name
        public bool ApplyInventory { get; set; }
        public bool ApplyProperty { get; set; }
        public bool IsApplySizer { get; set; }
        public bool IsFixedAsset { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string MUnitName { get; set; }
        public string MUnit { get; set; }
        public string GroupName { get; set; }//Basic Product Name
        public string ErrorMessage { get; set; }
        public EnumUniteType UnitType { get; set; }
        public int UnitTypeInInt { get; set; }
      
        public string AddOne { get; set; }
        public string AddTwo { get; set; }
        public string AddThree { get; set; }
        public string NameCode { get; set; }
        public string Brand { get; set; }
        public  EnumProductType ProductType {get;set;}
        public int  AccountHeadID {get;set;}
        public string  AccountCode {get;set;}
        public string AccountHeadName {get;set;}
        public bool IsSerialNoApply { get; set; }
        public int  ModelReferenceID { get; set; }
        public string ModelReferenceName { get; set; }
        public string ProductDescription { get; set; }
        public double FinishGoodsWeight { get; set; }
        public double NaliWeight { get; set; }
        public double WeigthFor { get; set; }
        public int FinishGoodsUnit { get; set; }
        public string FGUSymbol { get; set; }
        public double StockValue { get; set; }
        public double CurrentStock { get; set; }
        public int   ReportingUnitID { get; set; }
        public bool  Activity { get; set; }
        public double ShortQty { get; set; }
        public string  ReportingUnitName { get; set; }
        public string ReportingUnit { get; set; }
        public string Params { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public int ProductTypeInInt { get; set; }
        public int QtyType { get; set; }//use in Productoin Receipi
        public int LastUpdateBy { get; set; }
        public string LastUpdateByName { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        #endregion

        #region Derived Properties
        public bool IsApplyCategory { get; set; }
        public bool IsApplyGroup { get; set; }
        public bool IsApplyColor { get; set; }
        public bool IsApplySize { get; set; }
        public bool IsApplyMeasurement { get; set; }
        public bool ApplyGroup_IsShow { get; set; }
        public bool ApplyProductType_IsShow { get; set; }/// for Account Effece
        public bool ApplyProperty_IsShow { get; set; }
        public bool ApplyPlantNo_IsShow { get; set; }
        public int BUID { get; set; }
        public int ProductUsagesInInt { get; set; }
        public int ModuleNameInInt { get; set; }
        public string LastUpdateDateTimeInSt
        {
            get
            {
                if (this.LastUpdateDateTime == DateTime.MinValue) return "";
                return LastUpdateDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string ActivityInString {
            get {
                if (this.Activity) { return "Active"; } else { return "In Active"; }
            }
        }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + "[" + this.ProductCode + "]";
            }
        }
        
        private string sFullname;
        public string FullName
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GroupName))
                {
                    sFullname = this.GroupName;
                }
                if (!String.IsNullOrEmpty(this.Brand))
                {
                    if (!String.IsNullOrEmpty(this.sFullname))
                    {
                        sFullname = sFullname + "-" + this.Brand;
                    }
                    else
                    {
                        sFullname = this.Brand;
                    }
                  
                }
                if (!String.IsNullOrEmpty(this.ProductName))
                {
                    sFullname =sFullname+ "-" + this.ProductName;
                }

                if (!String.IsNullOrEmpty(this.ShortName ))
                {
                    sFullname =sFullname+ "-" + this.ShortName;
                }
                return sFullname;

            }
        }
        public string AccountHeadCodeName
        {
            get
            {
                if (this.AccountHeadName!="")
                {
                    return this.AccountHeadName + '[' + this.AccountCode + ']';
                }else{
                    return "";
                }
                
            }
        }
        public List<Product> Products { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<ProductPropertyInformation> PPIs { get; set; }
        //public List<EnumObject> ProductTypeObjs { get; set; }
        public List<UnitConversion> UnitConversions { get; set; }
        #endregion

        #region Functions
        public static List<Product> GetsbyConfigure(string sProductName,  string sParentCategoryIDs,  int nUserID)
        {
            return Product.Service.GetsbyConfigure(sProductName, sParentCategoryIDs,  nUserID);
        }
        public static List<Product> Getsby(int nProductBaseID, int nUserID)
        {
            return Product.Service.Getsby(nProductBaseID, nUserID);
        }
        public static double GetStockQuantity(int nBUID, int nProductID, int nUserID)
        {
            return Product.Service.GetStockQuantity(nBUID, nProductID, nUserID);
        }
        public static List<Product> GetsByPCategory(int nCategoryID, int nUserID)
        {
            return Product.Service.GetsByPCategory(nCategoryID, nUserID);
        }
        public static List<Product> GetsByBU(int nBUID, string sProductName, int nUserID)
        {
            return Product.Service.GetsByBU(nBUID, sProductName, nUserID);
        }
        public static List<Product> GetsbyName(string sProductName, string sProductCategorys,  int nUserID)
        {
            return Product.Service.GetsbyName(sProductName, sProductCategorys,  nUserID);
        }
        public string DeleteGeneralizeProduct(int id, int nUserID)
        {
            return Product.Service.DeleteGeneralizeProduct(id, nUserID);
        }
        public Product Get(int id,int nUserID)
        {
            return Product.Service.Get(id,  nUserID);
        }
        public Product Save(int nUserID)
        {
            return Product.Service.Save(this, nUserID);
        }
        public Product CommitActivity(int id, bool bIsActive, int nUserID)
        {
            return Product.Service.CommitActivity(id, bIsActive, nUserID);
        }
        public Product ProductMarge(int nUserID)
        {
            return Product.Service.ProductMarge(this, nUserID);
        }        
        public string Delete(  int nUserID)
        {
            return Product.Service.Delete(this, nUserID);
        }
        public static List<Product> Gets( int nUserID)
        {
            return Product.Service.Gets( nUserID);
        }
        public static List<Product> Gets(string sSQL, int nUserID)
        {
            return Product.Service.Gets(sSQL, nUserID);
        }
        public static List<Product> ProductGroupChangeSave(string productIds, int ProductCategoryID, int ProductBaseID, int nUserID)
        {
            return Product.Service.ProductGroupChangeSave(productIds, ProductCategoryID, ProductBaseID, nUserID);
        }
        public static List<Product> GetsByCodeOrName(Product oProduct, int nUserID)
        {
            return Product.Service.GetsByCodeOrName(oProduct, nUserID);
        }
        public static List<Product> GetsPermittedProduct(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, int nUserID)
        {
            return Product.Service.GetsPermittedProduct(nBUID, eModuleName, eProductUsages, nUserID);
        }
        public static List<Product> GetsPermittedProductByNameCode(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, string sNameCode, int nUserID)
        {
            return Product.Service.GetsPermittedProductByNameCode(nBUID, eModuleName, eProductUsages, sNameCode, nUserID);
        }
        public static List<Product> Gets_Import(string sNameCode, int nBUID, int nProductType, int nUserID)
        {
            return Product.Service.Gets_Import(sNameCode, nBUID, nProductType, nUserID);
        }
        #endregion

        #region NonDB
        public static string IDInString(List<Product> oProducts)
        {
            string sReturn = "";
            foreach (Product oItem in oProducts)
            {
                sReturn = sReturn + oItem.ProductID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        #endregion

        #region Non DB Function
        public static string IDString(List<Product> ProductList)
        {
            string sPIDs = "";
            foreach (Product oItem in ProductList)
            {
                sPIDs = sPIDs + oItem.ProductID.ToString() + ",";
            }
            if (sPIDs.Length > 0)
                sPIDs = sPIDs.Remove(sPIDs.Length - 1, 1);
            return sPIDs;
        }
        public static List<Product> GetByCategory(List<Product> ProductList,int nPCategoryID)
        {
            List<Product> oProducts = new List<Product>();
            foreach (Product oItem in ProductList)
            {
                if (oItem.ProductCategoryID== nPCategoryID)
                {
                    oProducts.Add(oItem);
                }
            }
            return oProducts;
        }
        #endregion

        #region ServiceFactory
        internal static IProductService Service
        {
            get { return (IProductService)Services.Factory.CreateService(typeof(IProductService)); }
        }
        #endregion
    }
    #endregion

    #region IProduct interface
    public interface IProductService
    {
        Product Get(int id,int nUserID);
        List<Product> GetsbyName(string sProductName, string sProductCategorys,  int nUserID);
        List<Product> Gets( int nUserID);
        double GetStockQuantity(int nBUID, int nProductID, int nUserID);

        string Delete(Product oProduct, int nUserID);
        string DeleteGeneralizeProduct(int id, int nUserID);
        Product Save(Product oProduct, int nUserID);
        Product CommitActivity(int id,bool bIsActive, int nUserID);
        Product ProductMarge(Product oProduct, int nUserID);        
        List<Product> Gets(string sSQL, int nUserID);
        List<Product> ProductGroupChangeSave(string productIds, int ProductCategoryID, int ProductBaseID, int nUserID);
        List<Product> GetsbyConfigure(string sProductName, string sParentCategoryIDs,  int nUserID);
        List<Product> Getsby(int nProductBaseID, int nUserID);
        List<Product> GetsByPCategory(int nCategoryID, int nUserID);
        List<Product> GetsByBU(int nBUID, string sProductName, int nUserID);
        List<Product> GetsByCodeOrName(Product oProduct, int nUserID);
        List<Product> GetsPermittedProduct(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, int nUserID);
        List<Product> GetsPermittedProductByNameCode(int nBUID, EnumModuleName eModuleName, EnumProductUsages eProductUsages, string sNameCode, int nUserID);
        List<Product> Gets_Import(string sNameCode, int nBUID, int nProductType, int nUserID);
    }
    #endregion

    #region Product Tree
    public class ProductTree
    {
        public ProductTree()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            ObjectType = "Root";
            Objectid = 4521456;
            children = new List<ProductTree>();
            IsLastLayer = false;
            PathName = "";
            code = "";
            parentName = "";
            note = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string ObjectType { get; set; }// distinguish object like : ProductCategory, ProductBase, Product etc if Object Product then type will be : "Product", if ProductCategory then type will be : "ProductCategory"
        public int Objectid { get; set; } // for distinguish object id
        public List<ProductTree> children { get; set; }//: an array nodes defines some children nodes
        public bool IsLastLayer { get; set; }
        public string PathName { get; set; }
        public string code { get; set; }
        public string parentName { get; set; }
        public string note { get; set; } 
    }
    #endregion 
}
