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


    #region CostSheetPackage
    
    public class CostSheetPackage : BusinessObject
    {
        public CostSheetPackage()
        {
            CostSheetPackageID = 0;
            CostSheetID = 0;
            PackageName = "";
            Price = 0;
            Remark = "";
            CostSheetPackageDetails = new List<CostSheetPackageDetail>();
            ErrorMessage = "";
        }

        #region Properties
         
        public int CostSheetPackageID { get; set; }
         
        public int CostSheetID { get; set; }
         
        public double Price { get; set; }
         
        public string PackageName { get; set; }
         
        public string Remark { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<CostSheetPackageDetail> CostSheetPackageDetails { get; set; }
         
        public List<CostSheetPackage> CostSheetPackageList { get; set; }
         
        public Company Company { get; set; }

        #endregion

        #region Functions

        public static List<CostSheetPackage> Gets( long nUserID)
        {
            return CostSheetPackage.Service.Gets( nUserID);
        }

        public static List<CostSheetPackage> Gets(int id, long nUserID)
        {
            return CostSheetPackage.Service.Gets(id, nUserID);
        }


        public CostSheetPackage Get(int id, long nUserID)
        {
            return CostSheetPackage.Service.Get(id, nUserID);
        }

        public CostSheetPackage Save(long nUserID)
        {
            return CostSheetPackage.Service.Save(this, nUserID);
        }
        public static List<CostSheetPackage> Gets(string sSQL, long nUserID)
        {           
            return CostSheetPackage.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CostSheetPackage.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICostSheetPackageService Service
        {
            get { return (ICostSheetPackageService)Services.Factory.CreateService(typeof(ICostSheetPackageService)); }
        }
        #endregion
    }
    #endregion

    #region ICostSheetPackage interface
     
    public interface ICostSheetPackageService
    {
         
        CostSheetPackage Get(int id, Int64 nUserID);
         
        List<CostSheetPackage> Gets(Int64 nUserID);
         
        List<CostSheetPackage> Gets(int id, Int64 nUserID);

         
        List<CostSheetPackage> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        CostSheetPackage Save(CostSheetPackage oCostSheetPackage, Int64 nUserID);


    }
    #endregion 
    
  
}
