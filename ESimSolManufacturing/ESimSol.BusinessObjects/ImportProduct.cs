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
    #region ImportProduct
    
    public class ImportProduct : BusinessObject
    {
        public ImportProduct()
        {
            ImportProductID = 0;
            BUID = 0;
            FileName = "";
            Name = "";
            PrintName = "";
            BUName = "";
            Note = "";
            ErrorMessage = "";
            ProductType = EnumProductNature.Dyeing;
            Note = "";
            ProductTypeInt = 0;
            ImportProductDetails = new List<ImportProductDetail>();
        }

        #region Properties
        public int ImportProductID { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string PrintName { get; set; }
        public EnumProductNature ProductType { get; set; }
        public int ProductTypeInt { get; set; }
        public string Note { get; set; }
        public bool Activity { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
        public List<ImportProductDetail> ImportProductDetails { get; set; }
        public string ActivitySt
        {
            get
            {
                if (this.Activity == true) return "Active";
                else if (this.Activity == false) return "Inactive";
                else return "-";
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<ImportProduct> Gets(long nUserID)
        {
            return ImportProduct.Service.Gets(nUserID);
        }
        public ImportProduct Get(int id, long nUserID)
        {
            return ImportProduct.Service.Get(id, nUserID);
        }
     
        public static List<ImportProduct> GetByBU(int nBUID,long nUserID)
        {
            return ImportProduct.Service.GetByBU(nBUID,nUserID);
        }
        public ImportProduct Save(long nUserID)
        {
            return ImportProduct.Service.Save(this, nUserID);
        }
        public ImportProduct Activate(Int64 nUserID)
        {
            return ImportProduct.Service.Activate(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportProduct.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportProductService Service
        {
            get { return (IImportProductService)Services.Factory.CreateService(typeof(IImportProductService)); }
        }
        #endregion
    }
    #endregion


    #region IImportProduct interface
    
    public interface IImportProductService
    {
        ImportProduct Get(int id, Int64 nUserID);
        List<ImportProduct> GetByBU(int nBUID, Int64 nUserID);
        List<ImportProduct> Gets(Int64 nUserID);
        string Delete(ImportProduct oImportProduct, Int64 nUserID);
        ImportProduct Save(ImportProduct oImportProduct, Int64 nUserID);
        ImportProduct Activate(ImportProduct oImportProduct, Int64 nUserID);
    }
    #endregion
}