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


    #region PackageTemplateDetail
    
    public class PackageTemplateDetail : BusinessObject
    {
        public PackageTemplateDetail()
        {
            PackageTemplateDetailID = 0;
            PackageTemplateID = 0;
            ProductID = 0;
            ProductCode = "";
            ProductName = "";
            Quantity = "";
          
            ErrorMessage = "";
        }

        #region Properties
         
        public int PackageTemplateDetailID { get; set; }
         
        public int PackageTemplateID { get; set; }
         
        public int ProductID { get; set; }
         
        public string ProductCode { get; set; }
         
        public string ProductName { get; set; }
         
        public string Quantity { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        #endregion

        #region Functions

        public static List<PackageTemplateDetail> Gets(long nUserID)
        {
            return PackageTemplateDetail.Service.Gets( nUserID);
        }
        public PackageTemplateDetail Get(int id, long nUserID)
        {
            return PackageTemplateDetail.Service.Get(id, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PackageTemplateDetail.Service.Delete(id, nUserID);
        }
        public static List<PackageTemplateDetail> Gets(int nONSID, long nUserID)
        {
            return PackageTemplateDetail.Service.Gets(nONSID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPackageTemplateDetailService Service
        {
            get { return (IPackageTemplateDetailService)Services.Factory.CreateService(typeof(IPackageTemplateDetailService)); }
        }


        #endregion
    }
    #endregion

    #region IPackageTemplateDetail interface
     
    public interface IPackageTemplateDetailService
    {
         
        PackageTemplateDetail Get(int id, Int64 nUserID);
         
        List<PackageTemplateDetail> Gets(Int64 nUserID);
         
        List<PackageTemplateDetail> Gets(int nPackageTemplateID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion

}
