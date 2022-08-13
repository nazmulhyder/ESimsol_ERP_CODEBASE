using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region StyleBudgetDetail
    
    public class StyleBudgetDetail : BusinessObject
    {
        public StyleBudgetDetail()
        {
            StyleBudgetDetailID = 0;
            StyleBudgetID = 0;
            StyleBudgetLogDetailID = 0;
            StyleBudgetLogID = 0;
            MaterialType = EnumCostSheetMeterialType.None;
            MaterialID = 0;
            ProductCode = "";
            ProductName = "";
            Ply = "";
            MaterialMarketPrice = 0;
            UsePercentage = 0;
            EstimatedCost = 0;
            MaterialTypeInInt = 0;
            ActualGarmentsWeight =0;
            ActualProcessLoss = 0;
            Description = "";
            Width = "";
            Consumption = 0;
            UnitName = "";
            Sequence = 0;
            UnitID = 0;
            UnitSymbol = "";
            WastagePercentPerMaterial = 0;
            DyeingCost	= 0;
			LycraCost	= 0;
			AOPCost	= 0;
			KnittingCost	= 0;
            WashCost = 0;
            YarnDyeingCost = 0;
            SuedeCost  = 0;
            FinishingCost = 0;
            BrushingCost = 0;
            RateUnit = 1;
            ErrorMessage = "";
        }

        #region Properties

        public int StyleBudgetDetailID { get; set; }
         
        public int StyleBudgetID { get; set; }
         
        public EnumCostSheetMeterialType MaterialType { get; set; }
         
        public int MaterialID { get; set; }
         
        public string ProductCode { get; set; }
         
        public string Ply { get; set; }
         
        public double MaterialMarketPrice { get; set; }
         
        public double UsePercentage { get; set; }
         
        public double EstimatedCost { get; set; }
         
        public string UnitName { get; set; }
         
        public string ProductName { get; set; }
         
        public int StyleBudgetLogDetailID { get; set; }
         
        public int StyleBudgetLogID { get; set; }
         
         public string Description {get;set;}
         
         public string  Width {get;set;}
         
        public double Consumption { get; set; }
        public int RateUnit { get; set; }
        public int MaterialTypeInInt{ get; set; }
        public double YarnDyeingCost { get; set; }
        public double SuedeCost { get; set; }
        public double FinishingCost { get; set; }
        public double BrushingCost { get; set; }
        public int Sequence { get; set; }
        public int UnitID {get;set;}
        public string UnitSymbol { get; set; }
         
        public double ActualGarmentsWeight {get;set;}
         
        public double ActualProcessLoss { get; set; }
         
        public double WastagePercentPerMaterial { get; set; }
        public double  DyeingCost	{ get; set; }
        public double LycraCost { get; set; }
        public double AOPCost { get; set; }
        public double KnittingCost { get; set; }
        public double WashCost { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int DeptInInt { get; set; }
        public List<StyleBudgetDetail> StyleBudgetDetails { get; set; }
        
        public string EstimatedCostInString
        {
            get
            {
                return Global.MillionFormat(this.EstimatedCost) + " /Dozen";
            }
        }

        public double CostPerDzn
        {
            get
            {
                if (this.MaterialType == EnumCostSheetMeterialType.Accessories && this.Consumption > 0 && this.MaterialMarketPrice > 0)
                {
                    return this.Consumption * this.MaterialMarketPrice/this.RateUnit;
                }else
                {
                    return 0;
                }
            }
        }
        public double FabricCost
        {
            get
            {
                return this.MaterialMarketPrice + this.KnittingCost + this.DyeingCost + this.LycraCost + this.AOPCost + this.WashCost + this.YarnDyeingCost + this.SuedeCost + this.FinishingCost + this.BrushingCost;
            }
        }
        public double ConsumptionPerPc
        {
            get
            {
                if(this.Consumption>0)
                {
                    return this.Consumption / 12;
                }
                else
                {
                    return 0;
                }
            }
        }
        public double CostPerPcs
        {
            get
            {
                if (this.MaterialType == EnumCostSheetMeterialType.Accessories && this.Consumption > 0 && this.MaterialMarketPrice > 0)
                {
                    return (this.Consumption * (this.MaterialMarketPrice/this.RateUnit))/12;
                }
                else if (this.EstimatedCost > 0)
                {
                    return this.EstimatedCost/12;
                }
                else
                {
                    return 0;
                }
            }
        }
        //only use for accesoreis
        public double EstimatedCostPerPc
        {
            get
            {
                return this.MaterialType == EnumCostSheetMeterialType.Accessories ? this.EstimatedCost / 12 : 0;
            }
        }
        #endregion

        #region Functions

        public static List<StyleBudgetDetail> Gets(int StyleBudgetID, long nUserID)
        {
            return StyleBudgetDetail.Service.Gets(StyleBudgetID, nUserID);
        }

        public static List<StyleBudgetDetail> GetActualSheet(int SaleOrderID, long nUserID)
        {
            return StyleBudgetDetail.Service.GetActualSheet(SaleOrderID, nUserID);
        }
        public static List<StyleBudgetDetail> GetsStyleBudgetLog(int StyleBudgetLogID, long nUserID)
        {
            return StyleBudgetDetail.Service.GetsStyleBudgetLog(StyleBudgetLogID, nUserID);
        }

        public static List<StyleBudgetDetail> Gets(string sSQL, long nUserID)
        {
            return StyleBudgetDetail.Service.Gets(sSQL, nUserID);
        }
        public StyleBudgetDetail Get(int StyleBudgetDetailID, long nUserID)
        {
            return StyleBudgetDetail.Service.Get(StyleBudgetDetailID, nUserID);
        }

        public StyleBudgetDetail Save(long nUserID)
        {
            return StyleBudgetDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IStyleBudgetDetailService Service
        {
            get { return (IStyleBudgetDetailService)Services.Factory.CreateService(typeof(IStyleBudgetDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IStyleBudgetDetail interface
     
    public interface IStyleBudgetDetailService
    {
         
        StyleBudgetDetail Get(int StyleBudgetDetailID, Int64 nUserID);
         
        List<StyleBudgetDetail> Gets(int StyleBudgetID, Int64 nUserID);
         
        List<StyleBudgetDetail> GetActualSheet(int SaleOrderID, Int64 nUserID);
        
         
        List<StyleBudgetDetail> GetsStyleBudgetLog(int StyleBudgetLogID, Int64 nUserID);
         
        List<StyleBudgetDetail> Gets(string sSQL, Int64 nUserID);
         
        StyleBudgetDetail Save(StyleBudgetDetail oStyleBudgetDetail, Int64 nUserID);


    }
    #endregion
    

}
