using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ProductCategory
    public class ProductCategory : BusinessObject
    {
        public ProductCategory()
        {
            ProductCategoryID = 0;
            ProductCategoryName = "";
            ParentCategoryID = 0;
            Note = "";
            IsLastLayer = false;
            ErrorMessage = "";
            BusinessUnitID = 0;
            DrAccountHeadID = 0;
            DrAccountHeadName = "";
            ParentCategoryName = "";
            CrAccountHeadID = 0;
            CrAccountHeadName = "";
        }

        #region Properties
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public int ParentCategoryID { get; set; }
        public string Note { get; set; }
        public bool IsLastLayer { get; set; }
        public bool IsApplyGroup { get; set; }
        public int DrAccountHeadID { get; set; }
        public string DrAccountHeadName { get; set; }
        public int CrAccountHeadID { get; set; }
        public string CrAccountHeadName { get; set; }
        public string ErrorMessage { get; set; }
        public string ParentCategoryName { get; set; }
        public List<ProductCategory> ProductCategorys { get; set; }
        public ProductSetup ProductSetup { get; set; } 
     
        #endregion

        #region Derived Property
        public int BusinessUnitID { get; set; }
        public bool IsApplyCategory { get; set; }
        public bool ApplyGroup_IsShow { get; set; }
        public bool ApplyProductType_IsShow { get; set; }/// for Account Effece
        public bool ApplyProperty_IsShow { get; set; }
        public bool ApplyPlantNo_IsShow { get; set; }
        public IEnumerable<ProductCategory> ChildCategorys { get; set; }
        public List<BUWiseProductCategory> BUWiseProductCategories { get; set; }
        public List<BUWiseProductCategory> ParentBUWiseProductCategories { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        #endregion
        #region Function
        public static List<ProductCategory> GetsByParentID(int nParentID, int nUserID)
        {
            return ProductCategory.Service.GetsByParentID(nParentID,  nUserID);
        }
        public ProductCategory IUD(int id, int nDBOperation, int nUserID)
        {
            return ProductCategory.Service.IUD(this, nDBOperation, nUserID);
        }
        public ProductCategory Get(int id,  int nUserID)
        {
            return ProductCategory.Service.Get(id,  nUserID);
        }
        public ProductCategory Update_AccountHead(int nUserID)
        {
            return ProductCategory.Service.Update_AccountHead(this, nUserID);
        }
        public static List<ProductCategory> Gets( int nUserID)
        {
            return ProductCategory.Service.Gets( nUserID);
        }
        public static List<ProductCategory> BUWiseGets(int nBUID, int nUserID)
        {
            return ProductCategory.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<ProductCategory> GetsBUWiseLastLayer(int nBUID, int nUserID)
        {
            return ProductCategory.Service.GetsBUWiseLastLayer(nBUID, nUserID);
        }
        public static List<ProductCategory> GetsLastLayer(int nUserID)
        {
            return ProductCategory.Service.GetsLastLayer(nUserID);
        }
        public static List<ProductCategory> Gets(string sSQL, int nUserID)
        {
            return ProductCategory.Service.Gets(sSQL, nUserID);
        }
        public static List<ProductCategory> GetsForTree(string sTableName, string sPK, string sPCID, bool IsParent, bool IsChild, string IDs, bool IsAll, int nUserID)
        {
            return ProductCategory.Service.GetsForTree(sTableName, sPK, sPCID, IsParent, IsChild, IDs, IsAll, nUserID);
        }
    
     
        #endregion

        #region NonDB Members
        #region Get parent
        public static List<ProductCategory> SubGroupitem(List<ProductCategory> oProductCategorys, int nParentID)
        {
            List<ProductCategory> oSubGroup = new List<ProductCategory>();
            foreach (ProductCategory oItem in oProductCategorys)
            {
                if (oItem.ParentCategoryID == nParentID)
                {
                    oSubGroup.Add(oItem);
                }
            }
            return oSubGroup;
        }
        #endregion
        #endregion


        #region ServiceFactory
        internal static IProductCategoryService Service
        {
            get { return (IProductCategoryService)Services.Factory.CreateService(typeof(IProductCategoryService)); }
        }
        #endregion
    }
    #endregion

    #region IProductCategory interface
    public interface IProductCategoryService
    {
        ProductCategory Get(int id, int nUserID);
        List<ProductCategory> Gets(int nUserID);
        List<ProductCategory> BUWiseGets(int nBUID, int nUserID);
        List<ProductCategory> GetsBUWiseLastLayer(int nBUID, int nUserID);
        List<ProductCategory> GetsLastLayer(int nUserID);
        List<ProductCategory> GetsByParentID(int nPrentID,int nUserID);
        ProductCategory IUD(ProductCategory oProductCategory, int nDBOperation, int nUserID);
        ProductCategory Update_AccountHead(ProductCategory oProductCategory, int nUserID);
        
        List<ProductCategory> Gets(string sSQL, int nUserID);
        [OperationContract]
        List<ProductCategory> GetsForTree(string sTableName, string sPK, string sPCID, bool IsParent, bool IsChild, string IDs, bool IsAll, Int64 nUserID);

      
    }
    #endregion

    #region TProductCategory
    public class TProductCategory
    {
        public TProductCategory()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;            
            Description = "";           
            IsLastLayer = false;
            AssetTypeInString = "";
            IsActive = true;
            productId = 0;
            DrAccountHeadID = 0;
            DrAccountHeadName = "";
            CrAccountHeadID = 0;
            CrAccountHeadName = "";

        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string Description { get; set; }
        public bool IsLastLayer { get; set; }
        public string AssetTypeInString { get; set; }
        public bool IsActive { get; set; }
        public int productId { get; set; }
        public int DrAccountHeadID { get; set; }
        public string DrAccountHeadName { get; set; }
        public int CrAccountHeadID { get; set; }
        public bool IsApplyGroup { get; set; }
        public bool IsApplyCategory { get; set; }
        public bool ApplyGroup_IsShow { get; set; }
        public bool ApplyProductType_IsShow { get; set; }/// for Account Effece
        public bool ApplyProperty_IsShow { get; set; }
        public bool ApplyPlantNo_IsShow { get; set; }
        public string CrAccountHeadName { get; set; }
        public IEnumerable<TProductCategory> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}