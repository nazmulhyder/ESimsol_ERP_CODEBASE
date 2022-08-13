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

    #region CostSheetDetail
    
    public class CostSheetDetail : BusinessObject
    {
        public CostSheetDetail()
        {
            CostSheetDetailID = 0;
            CostSheetID = 0;
            CostSheetLogDetailID = 0;
            CostSheetLogID = 0;
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

        public int CostSheetDetailID { get; set; }
         
        public int CostSheetID { get; set; }
         
        public EnumCostSheetMeterialType MaterialType { get; set; }
         
        public int MaterialID { get; set; }
         
        public string ProductCode { get; set; }
         
        public string Ply { get; set; }
         
        public double MaterialMarketPrice { get; set; }
         
        public double UsePercentage { get; set; }
         
        public double EstimatedCost { get; set; }
         
        public string UnitName { get; set; }
         
        public string ProductName { get; set; }
         
        public int CostSheetLogDetailID { get; set; }
         
        public int CostSheetLogID { get; set; }
         
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
        public List<CostSheetDetail> CostSheetDetails { get; set; }
        
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
                if(this.MaterialType == EnumCostSheetMeterialType.Accessories && this.Consumption>0 && this.MaterialMarketPrice>0)
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
                return this.MaterialType==EnumCostSheetMeterialType.Accessories? this.EstimatedCost / 12:0;
            }
        }
        #endregion

        #region Functions

        public static List<CostSheetDetail> Gets(int CostSheetID, long nUserID)
        {
            return CostSheetDetail.Service.Gets(CostSheetID, nUserID);
        }

        public static List<CostSheetDetail> GetActualSheet(int SaleOrderID, long nUserID)
        {
            return CostSheetDetail.Service.GetActualSheet(SaleOrderID, nUserID);
        }
        public static List<CostSheetDetail> GetsCostSheetLog(int CostSheetLogID, long nUserID)
        {
            return CostSheetDetail.Service.GetsCostSheetLog(CostSheetLogID, nUserID);
        }

        public static List<CostSheetDetail> Gets(string sSQL, long nUserID)
        {
            return CostSheetDetail.Service.Gets(sSQL, nUserID);
        }
        public CostSheetDetail Get(int CostSheetDetailID, long nUserID)
        {
            return CostSheetDetail.Service.Get(CostSheetDetailID, nUserID);
        }

        public CostSheetDetail Save(long nUserID)
        {
            return CostSheetDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ICostSheetDetailService Service
        {
            get { return (ICostSheetDetailService)Services.Factory.CreateService(typeof(ICostSheetDetailService)); }
        }

        #endregion
    }
    #endregion

    #region ICostSheetDetail interface
     
    public interface ICostSheetDetailService
    {
         
        CostSheetDetail Get(int CostSheetDetailID, Int64 nUserID);
         
        List<CostSheetDetail> Gets(int CostSheetID, Int64 nUserID);
         
        List<CostSheetDetail> GetActualSheet(int SaleOrderID, Int64 nUserID);
        
         
        List<CostSheetDetail> GetsCostSheetLog(int CostSheetLogID, Int64 nUserID);
         
        List<CostSheetDetail> Gets(string sSQL, Int64 nUserID);
         
        CostSheetDetail Save(CostSheetDetail oCostSheetDetail, Int64 nUserID);


    }
    #endregion
    

}
