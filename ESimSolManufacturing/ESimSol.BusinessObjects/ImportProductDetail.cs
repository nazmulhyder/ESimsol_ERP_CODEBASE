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
    #region ImportProductDetail
    
    public class ImportProductDetail : BusinessObject
    {
        public ImportProductDetail()
        {
            ImportProductID = 0;
            ImportProductDetailID = 0;
            ProductCategoryName = "";
            ProductCategoryID = 0;
            ProductCategoryName = "";
            ErrorMessage = "";
          
        }

        #region Properties
        public int ImportProductID { get; set; }
        public int ImportProductDetailID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ErrorMessage { get; set; }
        
        #region Derived Property
       
        #endregion

        #endregion

        #region Functions
        public static List<ImportProductDetail> Gets(int nIPID, long nUserID)
        {
            return ImportProductDetail.Service.Gets( nIPID,nUserID);
        }
        public ImportProductDetail Get(int id, long nUserID)
        {
            return ImportProductDetail.Service.Get(id, nUserID);
        }
    
    
        public string Delete(long nUserID)
        {
            return ImportProductDetail.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportProductDetailService Service
        {
            get { return (IImportProductDetailService)Services.Factory.CreateService(typeof(IImportProductDetailService)); }
        }
        #endregion
    }
    #endregion


    #region IImportProductDetail interface
    
    public interface IImportProductDetailService
    {
        ImportProductDetail Get(int id, Int64 nUserID);
        List<ImportProductDetail> Gets(int nIPID,Int64 nUserID);
        string Delete(ImportProductDetail oImportProductDetail, Int64 nUserID);
     
     
    }
    #endregion
}