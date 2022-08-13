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

    #region BuyerWiseBrand
    
    public class BuyerWiseBrand : BusinessObject
    {
        public BuyerWiseBrand()
        {
            
            BuyerWiseBrandID = 0;
            BrandID = 0;
            BrandCode = "";
            BrandName = "";
            BuyerID= 0;
            BuyerName = "";
            BuyerShortName = "";
            BuyerWiseBrands = new List<BuyerWiseBrand>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int BuyerWiseBrandID { get; set; }
         
        public int BrandID { get; set; }
         
        public string BuyerShortName { get; set; }
         
        public int BuyerID { get; set; }
         
        public string BuyerName  { get; set; }

        public string BrandCode { get; set; }

        public string BrandName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public bool IsShortList { get; set; }
         
        public bool IsBuyerBased { get; set; }
     
         
        public List<Contractor> Buyers { get; set; }
         
        public List<BuyerWiseBrand> BuyerWiseBrands { get; set; }
         
        public List<Brand> Brands { get; set; }
        public Company Company { get; set; }
    
        #endregion

        #region Functions

        public static List<BuyerWiseBrand> GetsByBrand(int id,  long nUserID) //Brand ID
        {
            return BuyerWiseBrand.Service.GetsByBrand(id, nUserID);
        }

        public static List<BuyerWiseBrand> GetsByBuyer(int id, long nUserID) //Buyer ID
        {
            return BuyerWiseBrand.Service.GetsByBuyer(id, nUserID);
        }

     
        public BuyerWiseBrand Get(int id, long nUserID)
        {
            return BuyerWiseBrand.Service.Get(id, nUserID);
        }

        public string Save(long nUserID, BuyerWiseBrand oBuyerWiseBrand, bool IsShortList , bool IsBuyerBased)
        {
            return BuyerWiseBrand.Service.Save(oBuyerWiseBrand,IsShortList, IsBuyerBased, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBuyerWiseBrandService Service
        {
            get { return (IBuyerWiseBrandService)Services.Factory.CreateService(typeof(IBuyerWiseBrandService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class BuyerWiseBrandList : List<BuyerWiseBrand>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IBuyerWiseBrand interface
     
    public interface IBuyerWiseBrandService
    {
         
        BuyerWiseBrand Get(int id, Int64 nUserID);

        List<BuyerWiseBrand> GetsByBrand(int id, Int64 nUserID);

        List<BuyerWiseBrand> GetsByBuyer(int id, Int64 nUserID);


        string Save(BuyerWiseBrand oBuyerWiseBrand, bool IsShortList, bool IsBuyerBased, Int64 nUserID);

    }
    #endregion
    
   
}
