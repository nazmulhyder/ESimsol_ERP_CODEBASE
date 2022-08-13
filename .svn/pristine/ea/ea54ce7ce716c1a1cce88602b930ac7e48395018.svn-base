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
    #region Feature
    
    public class Feature : BusinessObject
    {
        public Feature()
        {
            FeatureID = 0;
            FeatureCode="";
            Remarks = "";
            FeatureName  = "";
            FeatureType = EnumFeatureType.None;
            CurrencyID = 0;
            VehicleModelID = 0;
            UnitPrice = 0;
            VatAmount = 0;
            Price = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int FeatureID { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
        public EnumFeatureType  FeatureType {get;set;}
        public int CurrencyID { get; set; }
        public int VehicleModelID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string ModelShortName { get; set; }
        public string ModelNo { get; set; }
        public double UnitPrice { get; set; }
        public double VatAmount { get; set; }
        public double Price { get; set; }
        public string Remarks { get; set; }
        public int FeatureTypeInInt { get; set; }
        public string Param { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<Feature> ColorCategories { get; set; }
        public string FeatureTypeST
        {
            get
            {
                return EnumObject.jGet(this.FeatureType);
            }
        }
        public string PriceST
        {
            get
            {
                if (this.Price>0)
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.Price);
                }
                else
                {
                    return "";
                }
            }
        }
     
        #endregion

        #region Functions
        
        public static List<Feature> Gets(long nUserID)
        {
            return Feature.Service.Gets( nUserID);
        }

        public static List<Feature> GetsbyFeatureNameWithType(string sFeatureName, string sFTypes, long nUserID)
        {
            return Feature.Service.GetsbyFeatureNameWithType(sFeatureName,sFTypes, nUserID);
        }

     

        public Feature Get(int id, long nUserID)
        {
            return Feature.Service.Get(id, nUserID);
        }
        public static List<Feature> Gets(string sSQL, long nUserID)
        {           
            return Feature.Service.Gets(sSQL, nUserID);
        }
        public Feature Save(long nUserID)
        {
            
            return Feature.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Feature.Service.Delete(id, nUserID);
        }
        
        #endregion

        #region ServiceFactory

        internal static IFeatureService Service
        {
            get { return (IFeatureService)Services.Factory.CreateService(typeof(IFeatureService)); }
        }


        #endregion
    }
    #endregion

    #region IFeature interface
     
    public interface IFeatureService
    {
         
        Feature Get(int id, Int64 nUserID);
         
        List<Feature> Gets(Int64 nUserID);
         
        List<Feature> Gets(string sSQL, Int64 nUserID);

        List<Feature> GetsbyFeatureNameWithType(string sFeatureName, string sFTypes, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        Feature Save(Feature oFeature, Int64 nUserID);
    }
    #endregion
}
