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
    #region ProductSetup
    
    public class ProductSetup : BusinessObject
    {
        public ProductSetup()
        {
            ProductSetupID = 0;
            ProductCategoryID = 0;
            IsApplyCategory = true;
            IsApplyGroup = false;
            IsApplyProductType = false;
            IsApplyColor = false;
            IsApplySize = false;
            IsApplyMeasurement = false;
            IsApplySizer = false;
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
        public int ProductSetupID { get; set; }
        public int ProductCategoryID { get; set; }
        public bool IsApplyCategory { get; set; }
        public bool IsApplyGroup { get; set; }
        public bool IsApplyProductType { get; set; }/// for Account Effece
        public bool IsApplyProperty { get; set; }
        public bool IsApplyPlantNo { get; set; }
        public bool IsApplyColor { get; set; }
        public bool IsApplySize { get; set; }
        public bool IsApplyMeasurement { get; set; }
        public bool IsApplySizer { get; set; }
        public bool ApplyGroup_IsShow { get; set; }
        public bool ApplyProductType_IsShow { get; set; }/// for Account Effece
        public bool ApplyProperty_IsShow { get; set; }
        public bool ApplyPlantNo_IsShow { get; set; }

        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        
        public List<ProductSetup> ProductSetups { get; set; }
        
        #region Derived Property

        #endregion

        #endregion

        #region Functions
        public static List<ProductSetup> Gets(long nUserID)
        {
            return ProductSetup.Service.Gets(nUserID);
        }

        public ProductSetup Get(int id, long nUserID)
        {
            return ProductSetup.Service.Get(id, nUserID);
        }
        public ProductSetup GetByCategory(int id, long nUserID)
        {
            return ProductSetup.Service.GetByCategory(id, nUserID);
        }

        public ProductSetup Save(long nUserID)
        {
            return ProductSetup.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductSetup.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProductSetupService Service
        {
            get { return (IProductSetupService)Services.Factory.CreateService(typeof(IProductSetupService)); }
        }
        #endregion
    }
    #endregion


    #region IProductSetup interface
    
    public interface IProductSetupService
    {
        
        ProductSetup Get(int id, Int64 nUserID);
        ProductSetup GetByCategory(int id, Int64 nUserID);
        List<ProductSetup> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ProductSetup Save(ProductSetup oProductSetup, Int64 nUserID);
    }
    #endregion
}