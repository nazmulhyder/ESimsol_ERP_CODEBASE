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
    #region ProductionCostReDyeing
    
    public class ProductionCostReDyeing : BusinessObject
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


        
        public int ReDyeRouteSheetID { get; set; }
        
        public int ReDyeRSYarnID { get; set; }
        
        public int ReDyeLotID { get; set; }
        
        public double ReDyeYarnQtyInLBS { get; set; }
        
        public double ReDyeYarnUnitPrice { get; set; }
        
        public double ReDyeDyesQty { get; set; }
        
        public double ReDyeDyesPrice { get; set; }
        
        public double ReDyeChemicalQty { get; set; }
        
        public double ReDyeChemicalPrice { get; set; }
        
        public DateTime ReDyeProductionDate { get; set; }
        
        public string ReDyeRouteSheetNo { get; set; }
        
        public string ReDyeBuyerName { get; set; }
        
        public string ReDyeOrderNo { get; set; }


        
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public double TYarnValue { get; set; }
        public double TDyesValue { get; set; }
        public double TChemicalValue { get; set; }
        public double TValue { get; set; }
        public double TDCValue { get; set; }
        public string PDateInString { get; set; }

        public double ReDyeTYarnValue { get; set; }
        public double ReDyeTDyesValue { get; set; }
        public double ReDyeTChemicalValue { get; set; }
        public double ReDyeTValue { get; set; }
        public double ReDyeTDCValue { get; set; }
        public string ReDyePDateInString { get; set; }

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




        public double ReDyeTotalYarnValue
        {
            get
            {
                return this.ReDyeYarnQtyInLBS * this.ReDyeYarnUnitPrice;
            }
        }
        public double ReDyeTotalDyesValue
        {
            get
            {
                return this.ReDyeDyesPrice;
            }
        }
        public double ReDyeTotalChemicalValue
        {
            get
            {
                return this.ReDyeChemicalPrice;
            }
        }
        public double ReDyeTotalDyesChemicalValue
        {
            get
            {
                return ReDyeTotalDyesValue + ReDyeTotalChemicalValue;
            }
        }
        public double ReDyeTotalValue
        {
            get
            {
                return ReDyeTotalYarnValue + ReDyeTotalDyesValue + ReDyeTotalChemicalValue;
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
        public string ReDyeProductionDateInString
        {
            get
            {
                if (ReDyeProductionDate != DateTime.MinValue)
                {
                    return ReDyeProductionDate.ToString("dd MMM yyyy HH:mm");
                }
                else
                {
                    return "";
                }

            }
        }
        #endregion



        #region Functions

        public static List<ProductionCostReDyeing> Gets(DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, long nUserID)
        {
            return ProductionCostReDyeing.Service.Gets(startDate, EndDate, sBuyerIDs, sRouteSheetNos, sPTUIDs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProductionCostReDyeingService Service
        {
            get { return (IProductionCostReDyeingService)Services.Factory.CreateService(typeof(IProductionCostReDyeingService)); }
        }

        #endregion
    }
    #endregion



    #region IProductionCostReDyeing interface
    
    public interface IProductionCostReDyeingService
    {

        
        List<ProductionCostReDyeing> Gets(DateTime startDate, DateTime EndDate, string sBuyerIDs, string sRouteSheetNos, string sPTUIDs, long nUserID);

    }
    #endregion
}