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
    #region Brand
    
    public class Brand : BusinessObject
    {
        public Brand()
        {
            BrandID = 0;
            BrandName = "";
            BrandCode = "";
            WebAddress= "";
            FaxNo= "";
            EmailAddress= "";
            ShortName = "";
            Remarks = "";
           
        }

        #region Properties
         
        public int BrandID { get; set; }         
        public string BrandName { get; set; }
         
        public string BrandCode { get; set; }
         
        public string WebAddress { get; set; }
         
        public string FaxNo { get; set; }
         
        public string EmailAddress { get; set; }
         
        public string ShortName { get; set; }
      
        public string Remarks { get; set; }
      
         
        public string ErrorMessage { get; set; }        
        public string NameCode
        {
            get
            {
     
                return '['+this.BrandCode+']'+this.BrandName;
            }           
        }

        #endregion

        #region Derived Property        
       public List<BuyerWiseBrand> BuyerWiseBrands { get; set; }
       public List<Contractor> Buyers { get; set; }

        public List<Brand> Brands { get; set;}
        public int BuyerID { get; set; } //Extra field
        #endregion

        #region Functions

        public static List<Brand> Gets(long nUserID)
        {
            return Brand.Service.Gets( nUserID);
        }
     
        public static List<Brand> GetsByName(string sName,  long nUserID)
        {
            return Brand.Service.GetsByName(sName,  nUserID);
        }

    
        public Brand Get(int id, long nUserID)
        {
            return Brand.Service.Get(id, nUserID);
        }
    
        public Brand Save(long nUserID)
        {
            return Brand.Service.Save(this, nUserID);
        }
        public static List<Brand> Gets(string sSQL, long nUserID)
        {
            return Brand.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return Brand.Service.Delete(id, nUserID);
        }

     
   
        #endregion

        #region ServiceFactory
        internal static IBrandService Service
        {
            get { return (IBrandService)Services.Factory.CreateService(typeof(IBrandService)); }
        }

        #endregion
    }
    #endregion

    #region IBrand interface
     
    public interface IBrandService
    {
         
        Brand Get(int id, Int64 nUserID);
         
        List<Brand> Gets(Int64 nUserID);
     
        List<Brand> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        Brand Save(Brand oBrand, Int64 nUserID);

        List<Brand> GetsByName(string sName,  Int64 nUserID);
         
        
    }
    #endregion
}