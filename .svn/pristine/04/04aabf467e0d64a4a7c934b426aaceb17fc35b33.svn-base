using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ProductPermission
    public class ProductPermission : BusinessObject
    {
        public ProductPermission()
        {
            ProductPermissionID = 0;
            UserID = 0;
            ModuleName = EnumModuleName.None;
            ModuleNameInt = 0;
            ProductUsages = EnumProductUsages.None;
            ProductUsagesInt = 0;
            ProductCategoryID = 0;
            Remarks = "";
            ProductCategoryName = "";
            ErrorMessage = "";
            UserName = "";
            ModuleNameObjs = new List<EnumObject>();
            ProductPermissions = new List<ProductPermission>();
            ProductUsagesObjs = new List<EnumObject>();
        }
        #region Properties
        public int ProductPermissionID { get; set; }
        public int UserID { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int ModuleNameInt { get; set; }
        public EnumProductUsages ProductUsages { get; set; }
        public int ProductUsagesInt { get; set; }
        public int ProductCategoryID { get; set; }
        public string Remarks { get; set; }
        public string ProductCategoryName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string UserName { get; set; }
        public List<EnumObject> ModuleNameObjs { get; set; }
        public List<EnumObject> ProductUsagesObjs { get; set; }        
        public List<ProductPermission> ProductPermissions { get; set; }        
        public string ModuleNameST
        {
            get
            {
                return EnumObject.jGet(this.ModuleName);
            }
        }
        public string ProductUsagesST
        {
            get
            {
                return EnumObject.jGet(this.ProductUsages);
            }
        }        
        #endregion

        #region Functions
        public ProductPermission Get(int id, int nUserID)
        {
            return ProductPermission.Service.Get(id, nUserID);
        }
        public ProductPermission Save(int nUserID)
        {
            return ProductPermission.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ProductPermission.Service.Delete(id, nUserID);
        }
        public static List<ProductPermission> Gets(int nUserID)
        {
            return ProductPermission.Service.Gets(nUserID);
        }
        public static List<ProductPermission> Gets(string sSQL, int nUserID)
        {
            return ProductPermission.Service.Gets(sSQL, nUserID);
        }
        public static List<ProductPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            return ProductPermission.Service.GetsByUser(nPermittedUserID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductPermissionService Service
        {
            get { return (IProductPermissionService)Services.Factory.CreateService(typeof(IProductPermissionService)); }
        }
        #endregion
    }
    #endregion

    #region IProductPermission interface
    public interface IProductPermissionService
    {
        ProductPermission Get(int id, int nUserID);
        List<ProductPermission> Gets(int nUserID);
        string Delete(int id, int nUserID);
        ProductPermission Save(ProductPermission oProductPermission, int nUserID);
        List<ProductPermission> Gets(string sSQL, int nUserID);
        List<ProductPermission> GetsByUser(int nPermittedUserID, int nUserID);
    }
    #endregion
}
