using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductRate

    public class ProductRate: BusinessObject 
    {
        public ProductRate()
        {
            ProductRateID = 0;
            ProductID = 0;
            Rate = 0;
            ActivationDate = DateTime.MinValue;
            SaleSchemeID = 0;
        }

        #region Properties
        public int ProductRateID { get; set; }
        public int ProductID { get; set; }
        public double Rate { get; set; }
        public DateTime ActivationDate { get; set; }
        public int SaleSchemeID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ActivationDateString
        {
            get
            {
                return this.ActivationDate.ToString("dd MMM yyyy");
            }
        }
        public string RateString
        {
            get
            {
                if (this.Rate == 0) return "--";
                return Math.Round(this.Rate, 2).ToString();
            }
        }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ProductRate> ProductRates { get; set; }
        #endregion

        #region Functions
        public static List<ProductRate> Gets(long nUserID)
        {
            return ProductRate.Service.Gets(nUserID);
        }
        public static List<ProductRate> Gets(int ProductID, Int64 nUserID)
        {
            return ProductRate.Service.Gets(ProductID, nUserID);
        }
        public ProductRate Get(int nId, long nUserID)
        {
            return ProductRate.Service.Get(nId, nUserID);
        }
        public ProductRate Save(long nUserID)
        {
            return ProductRate.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ProductRate.Service.Delete(nId, nUserID);
        }
        #endregion

        internal static IProductRateService Service
        {
            get { return (IProductRateService)Services.Factory.CreateService(typeof(IProductRateService)); }
        }
    }
    #endregion

    #region IProductRateService

    public interface IProductRateService
    {
        List<ProductRate> Gets(long nUserID);
        List<ProductRate> Gets(int ProductID, long nUserID);
        ProductRate Get(int ProductRateID, long nUserID);
        string Delete(int id, long nUserID);
        ProductRate Save(ProductRate oProductRate, long nUserID);
    }
    #endregion
}