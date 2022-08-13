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


    #region PackageTemplate
    
    public class PackageTemplate : BusinessObject
    {
        public PackageTemplate()
        {
            PackageTemplateID = 0;
            PackageNo = "";
            PackageName = "";
            Note = "";
            BUID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int PackageTemplateID { get; set; }
         
        public string PackageNo { get; set; }
         
        public string PackageName { get; set; }
         
        public string Note { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<PackageTemplateDetail> PackageTemplateDetails { get; set; }
         
        public List<PackageTemplate> PackageTemplateList { get; set; }
         
        public Company Company { get; set; }

        #endregion

        #region Functions

        public static List<PackageTemplate> Gets(long nUserID)
        {
            return PackageTemplate.Service.Gets(nUserID);        
        }

        public PackageTemplate Get(int id, long nUserID)
        {
                       return PackageTemplate.Service.Get(id, nUserID);        
        }

        public PackageTemplate Save(long nUserID)
        {
            return PackageTemplate.Service.Save(this, nUserID);        
        }


        public static List<PackageTemplate> Gets(string sSQL, long nUserID)
        {
            return PackageTemplate.Service.Gets(sSQL, nUserID);        
        }
        public string Delete(int id, long nUserID)
        {
            return PackageTemplate.Service.Delete(id, nUserID);        
        }


        #endregion

        #region ServiceFactory
        internal static IPackageTemplateService Service
        {
            get { return (IPackageTemplateService)Services.Factory.CreateService(typeof(IPackageTemplateService)); }
        }

        #endregion
    }
    #endregion

    #region IPackageTemplate interface
     
    public interface IPackageTemplateService
    {
         
        PackageTemplate Get(int id, Int64 nUserID);
         
        List<PackageTemplate> Gets(Int64 nUserID);
         
        List<PackageTemplate> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        PackageTemplate Save(PackageTemplate oPackageTemplate, Int64 nUserID);


    }
    #endregion 

}
