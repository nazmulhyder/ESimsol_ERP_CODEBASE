using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    
    #region CostSheetPackageDetail
    
    public class CostSheetPackageDetail : BusinessObject
    {
        public CostSheetPackageDetail()
        {
            CostSheetPackageDetailID = 0;
            CostSheetPackageID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            Description = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int CostSheetPackageDetailID { get; set; }
         
        public int CostSheetPackageID { get; set; }
         
        public int ProductID { get; set; }
         
        public string ProductCode { get; set; }
         
        public string ProductName { get; set; }
         
        public string Description { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion


        #region Functions

        public static List<CostSheetPackageDetail> Gets(long nUserID)
        {
            return CostSheetPackageDetail.Service.Gets( nUserID);
        }

        public CostSheetPackageDetail Get(int id, long nUserID)
        {
            return CostSheetPackageDetail.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return CostSheetPackageDetail.Service.Delete(id, nUserID);
        }

        public static List<CostSheetPackageDetail> GetsByCostSheetID(int id, long nUserID)
        {
            return CostSheetPackageDetail.Service.GetsByCostSheetID(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ICostSheetPackageDetailService Service
        {
            get { return (ICostSheetPackageDetailService)Services.Factory.CreateService(typeof(ICostSheetPackageDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ICostSheetPackageDetail interface
     
    public interface ICostSheetPackageDetailService
    {
         
        CostSheetPackageDetail Get(int id, Int64 nUserID);
         
        List<CostSheetPackageDetail> Gets(Int64 nUserID);
         
        List<CostSheetPackageDetail> GetsByCostSheetID(int id, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion

  
}
