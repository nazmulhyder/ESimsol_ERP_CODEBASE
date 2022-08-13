using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ProductionCost
    
    public class ProductionCost : BusinessObject
    {

        #region Properties
        
        public int RouteSheetID { get; set; }
        
        public int RSYarnID { get; set; }
        
        public int LotID { get; set; }
        
        public double YarnQtyInLBS { get; set; }
        
        public double YarnUnitPrice { get; set; }
        
        public double DyesQty { get; set; }
        
        public double DyesPrice { get; set; }
        
        public double ChemicalQty { get; set; }
        
        public double ChemicalPrice { get; set; }
        
        public DateTime ProductionDate { get; set; }
        
        public string RouteSheetNo { get; set; }
        
        public string BuyerName { get; set; }
        
        public string OrderNo { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public double TYarnValue { get; set; }
        public double TDyesValue { get; set; }
        public double TChemicalValue { get; set; }
        public double TValue { get; set; }
        public double TDCValue { get; set; }
        public string PDateInString { get; set; }


        public double TotalYarnValue
        {
            get
            {
                return this.YarnQtyInLBS * this.YarnUnitPrice;
            }
        }
        public double TotalDyesValue
        {
            get
            {
                return this.DyesPrice;
            }
        }
        public double TotalChemicalValue
        {
            get
            {
                return this.ChemicalPrice;
            }
        }
        public double TotalDyesChemicalValue
        {
            get
            {
                return TotalDyesValue + TotalChemicalValue;
            }
        }
        public double TotalValue
        {
            get
            {
                return TotalYarnValue + TotalDyesValue + TotalChemicalValue;
            }
        }

        //public string TotalYarnValueInString
        //{
        //    get
        //    {
        //        return Global.MillionFormat(TotalYarnValue) ;
        //    }
        //}
        //public string TotalDyesValueInString
        //{
        //    get
        //    {
        //        return Global.MillionFormat(TotalDyesValue);
        //    }
        //}
        //public string TotalChemicalValueInString
        //{
        //    get
        //    {
        //        return Global.MillionFormat(TotalChemicalValue);
        //    }
        //}
        //public string TotalValueInString
        //{
        //    get
        //    {
        //        return Global.MillionFormat(TotalValue);
        //    }
        //}

        public string ProductionDateInString
        {
            get
            {
                if (ProductionDate != DateTime.MinValue)
                {
                    return ProductionDate.ToString("dd MMM yyyy HH:mm");
                }
                else
                {
                    return "";
                }

            }
        }
        #endregion



        #region Functions

        public static List<ProductionCost> Gets(DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, long nUserID)
        {
            return ProductionCost.Service.Gets(startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IProductionCostService Service
        {
            get { return (IProductionCostService)Services.Factory.CreateService(typeof(IProductionCostService)); }
        }

        #endregion
    }
    #endregion



    #region IProductionCost interface
    
    public interface IProductionCostService
    {

        
        List<ProductionCost> Gets(DateTime startDate, DateTime EndDate,string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, long nUserID);

    }
    #endregion
}