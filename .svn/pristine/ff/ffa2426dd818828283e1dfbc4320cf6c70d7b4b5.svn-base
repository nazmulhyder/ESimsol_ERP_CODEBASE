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
    #region CostSheet
    
    public class CostSheet : BusinessObject
    {
        public CostSheet()
        {
            CostSheetID = 0;
            FileNo ="";
            TechnicalSheetID = 0;
            StyleNo = "";
            SpecialFinish ="";
            KnittingPatternName = "";
            BuyerName = "";
            BrandName = "";
            ColorRange ="";
            SizeRange ="";
            MerchandiserID = 0;
            CostSheetStatus = EnumCostSheetStatus.Initialized;
            StatusInInt = (int)EnumCostSheetStatus.Initialized;
            CostingDate = DateTime.Now;
            ShipmentDate = DateTime.Now;
            ApproxQty = 0;
            UnitID = 0;
            UnitName = "";
            WeightPerDozen = 0;
            WeightUnitID  = 0;
            WastageInPercent  = 0;
            GG ="";
            FabricDescription ="";
            StyleDescription = "";
            CurrencyID  = 0;
            CurrencySymbol = "$";
            ProcessLoss  = 0;
            FabricWeightPerDozen = 0;
            FabricUnitPrice  = 0;
            FabricCostPerDozen  = 0;
            AccessoriesCostPerDozen  = 0;
            ProductionCostPerDozen  = 0;
            BuyingCommission = 0;
            FOBPricePerPcs  = 0;
            ApprovedBy  = 0;
            ApprovedByName = "";
            CostSheetLogID = 0;
            WeightUnitSymbol = "";
            MerchandiserName = "";
            WeightUnitName = "";
            CostSheetType = EnumCostSheetType.Sweater;
            ApprovedDate = DateTime.Today;
            CostSheetTypeInInt = 0;
            BankingCost = 0;
            GarmentsName = "";
            BUID = 0;
            OfferPricePerPcs = 0;
            YarnCategoryName = "";
            Count = "";
            CMCost = 0;
            ConfirmPricePerPcs = 0;
            DeptName = "";
            FabricCostPerPcs = 0;
            AccessoriesCostPerPcs = 0;
            CMCostPerPcs = 0;
            FOBPricePerDozen = 0;
            OfferPricePerDozen = 0;
            ConfirmPricePerDozen = 0;
            PrintPricePerDozen = 0;
            EmbrodaryPricePerDozen = 0;
            TestPricePerDozen = 0;
            OthersPricePerDozen = 0;
            CourierPricePerDozen = 0;


            PrintPricePerPcs   = 0;
            EmbrodaryPricePerPcs    = 0;
            TestPricePerPcs    = 0;
            OthersPricePerPcs  = 0;
            CourierPricePerPcs = 0;
            OthersCaption = "Ohters";//default
            CourierCaption = "DHL";//default

            RefObjectID = 0;//use for Budget
            PreparedByName = ""; ;
            
            CostSheetActionType = EnumCostSheetActionType.None;
            CostSheetPackages = new List<CostSheetPackage>();
            CostSheetYarnDetails = new List<CostSheetDetail>();
            CostSheetStepDetails = new List<CostSheetDetail>();
            CostSheetCMs = new List<CostSheetCM>();
            ProductionSteps = new List<ProductionStep>();
            
            ErrorMessage = "";
        }

        #region Properties
         
        public int CostSheetID { get; set; }
         
        public int CostSheetLogID { get; set; }
         
        public EnumCostSheetStatus CostSheetStatus { get; set; }
         
        public string FileNo { get; set; }
         
        public EnumCostSheetType CostSheetType { get; set; }
        public DateTime ApprovedDate { get; set; }
         
        public int CostSheetTypeInInt { get; set; }
         
        public string CurrencySymbol { get; set; }
         
        public string UnitSymbol { get; set; }
         
        public string GarmentsName { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public string StyleNo { get; set; }
         
        public string BuyerName { get; set; }
        public string BrandName { get; set; }
         
        public string SpecialFinish { get; set; }

        public string KnittingPatternName { get; set; }
         
        public string ColorRange { get; set; }
         
        public string SizeRange { get; set; }
         
        public int MerchandiserID { get; set; }
         
        public DateTime CostingDate { get; set; }
         
        public DateTime ShipmentDate { get; set; }
        public string DeptName { get; set; }
        public double ApproxQty { get; set; }
        public double CMCost { get; set; }
        public int UnitID { get; set; }
       
        public string UnitName { get; set; }
         
        public int CurrencyID { get; set; }
         
        public string CurrencyName { get; set; }
         
        public double WeightPerDozen { get; set; }
         
        public int WeightUnitID { get; set; }
        public int RefObjectID { get; set; }
        public double WastageInPercent { get; set; }
        
        public string GG { get; set; }
        public string   YarnCategoryName { get; set; }
        public string Count { get; set; }
         
        public string FabricDescription { get; set; }
         
        public string StyleDescription { get; set; }
         
        public string WeightUnitName { get; set; }
         
        public double ProcessLoss { get; set; }
         
        public double FabricWeightPerDozen { get; set; }
         
        public double FabricUnitPrice { get; set; }
         
        public double FabricCostPerDozen { get; set; }
         
        public double AccessoriesCostPerDozen { get; set; }

        public double ProductionCostPerDozen { get; set; }
         
        public double BuyingCommission { get; set; }
        
    
            public double PrintPricePerPcs   { get; set; }
            public double EmbrodaryPricePerPcs    { get; set; }
            public double TestPricePerPcs    { get; set; }
            public double OthersPricePerPcs  { get; set; }
            public double CourierPricePerPcs { get; set; }
            public string OthersCaption { get; set; }
            public string CourierCaption { get; set; }
            public double FabricCostPerPcs { get; set; }
            public double AccessoriesCostPerPcs { get; set; }
            public double CMCostPerPcs { get; set; }
            public double FOBPricePerDozen { get; set; }
            public double OfferPricePerDozen { get; set; }
            public double ConfirmPricePerDozen { get; set; }
            public double PrintPricePerDozen { get; set; }
            public double EmbrodaryPricePerDozen { get; set; }
            public double TestPricePerDozen { get; set; }
            public double OthersPricePerDozen { get; set; }
            public double CourierPricePerDozen { get; set; }
            public string PreparedByName { get; set; }
        public double FOBPricePerPcs { get; set; }
        public double OfferPricePerPcs { get; set; }
         
        public double BankingCost { get; set; }
         
        public int ApprovedBy { get; set; }
         
        public string ApprovedByName { get; set; }
         
        public string WeightUnitSymbol { get; set; }
        public int BUID { get; set; }
        public string MerchandiserName { get; set; }
        public double ConfirmPricePerPcs { get; set; }
         
        public EnumCostSheetActionType CostSheetActionType { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property


        
     
        // public string CMPerPcSt
        //{
        //    get { return Global.MillionFormat(this.CMPerPc); }
        //}
        // public string FinalCMPerPcwithConsideringSt
        // {
        //     get { return Global.MillionFormat(this.FinalCMPerPcwithConsidering); }
        // }
        public List<CostSheet> CostSheetList { get; set; }
         
        public List<CostSheetPackage> CostSheetPackages { get; set; }
        
        public List<ProductionStep> ProductionSteps { get; set; }
        public List<CostSheetHistory> CostSheetHistories { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizeList { get; set; }
        public List<Employee> Employees { get; set; }
         
        public OrderRecap OrderRecap { get; set; }
         
        public List<OrderRecap> OrderRecapList { get; set; }
         
        public List<User> Users { get; set; }
        public List<CostSheetCM> CostSheetCMs { get; set; }
        public List<CostSheetDetail> CostSheetDetails { get; set; }
         
        public List<CostSheetDetail> CostSheetYarnDetails { get; set; }

        public List<CostSheetDetail> CostSheetStepDetails { get; set; }
         
        public List<CostSheetDetail> ActualCostSheetYarnDetails { get; set; }

        public List<CostSheetDetail> CostSheetAccessoriesDetails { get; set; }

        public List<CostSheetDetail> ActualCostSheetAccessoriesDetails { get; set; }
        public List<Company> Companies { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<MeasurementUnit> CountUnits { get; set; }
        public List<MeasurementUnit> WeightUnits { get; set; }
        public TechnicalSheetImage TechnicalSheetImage { get; set; }
      
         
        public Company Company { get; set; }
       
         
        public ApprovalRequest ApprovalRequest { get; set; }
         
        public Contractor Contractor { get; set; }
        
         
        public int StatusInInt { get; set; }
         
        public int PaymentTermInInt { get; set; }
        public int DeliveryTermInInt { get; set; }
        public string ActionTypeExtra { get; set; }
        public string PinCode { get; set; }
        public string sNote { get; set; }

        public string CostSheetStatusInString
        {
            get
            {
                return this.CostSheetStatus.ToString();
            }

        }
            
        public string CostSheetTypeInString
        {
            get
            {
                return this.CostSheetType.ToString();
            }
        }

        public string CostingDateInString
        {
            get
            {
                return this.CostingDate.ToString("dd MMM yyyy");

            }
        }


        public string ShipmentDateInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");

            }
        }
        public string ApprovedDateInString
        {
            get
            {
                if (this.ApprovedDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ApprovedDate.ToString("dd MMM yyyy");
                }

            }
        }
        
        public string FOBPricePerPcsInString
        {
            get
            {
                return this.CurrencySymbol+" "+Global.MillionFormat(this.FOBPricePerPcs);

            }
        }


        public string ProductionCostPerDozenInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.ProductionCostPerDozen);

            }
        }

        public string FabricCostPerDozenInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.FabricCostPerDozen);

            }
        }
        public string AccessoriesCostPerDozenInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.AccessoriesCostPerDozen);

            }
        }

        public string FileNoWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing Report and deshboard
                return this.CostSheetID + "~" + this.FileNo+"~3";
            }
        }
        #endregion

        #region Functions

        public static List<CostSheet> Gets(long nUserID)
        {
            return CostSheet.Service.Gets(nUserID);
        }

        public static List<CostSheet> GetsCostSheetLog(int id, long nUserID) // id is PI ID
        {
            return CostSheet.Service.GetsCostSheetLog(id, nUserID);
        }

        public static List<CostSheet> Gets(string sSQL, long nUserID)
        {
            return CostSheet.Service.Gets(sSQL, nUserID);
        }
        public CostSheet Get(int id, long nUserID)
        {
            return CostSheet.Service.Get(id, nUserID);
        }
      
        

        public CostSheet GetLog(int id, long nUserID) // id is PI Log ID
        {
            return CostSheet.Service.GetLog(id, nUserID);
        }

        public CostSheet Save(long nUserID)
        {
            return CostSheet.Service.Save(this, nUserID);
        }

        public CostSheet AcceptCostSheetRevise(long nUserID)
        {
            return CostSheet.Service.AcceptCostSheetRevise(this, nUserID);
        }
        public CostSheet ChangeStatus(ApprovalRequest oApprovalRequest, long nUserID)
        {
            return CostSheet.Service.ChangeStatus(this, oApprovalRequest, nUserID);
        }

        public string Delete(int nCostSheetID, long nUserID)
        {
            return CostSheet.Service.Delete(nCostSheetID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ICostSheetService Service
        {
            get { return (ICostSheetService)Services.Factory.CreateService(typeof(ICostSheetService)); }
        }

        #endregion
    }
    #endregion

    #region ICostSheet interface
     
    public interface ICostSheetService
    {
         
        CostSheet Get(int id, Int64 nUserID);
       
         
        CostSheet GetLog(int id, Int64 nUserID);
         
        List<CostSheet> Gets(Int64 nUserID);
         
        List<CostSheet> GetsCostSheetLog(int id, Int64 nUserID);
         
        List<CostSheet> Gets(string sSQL, Int64 nUserID);
         
        CostSheet Save(CostSheet oCostSheet, Int64 nUserID);
         
        CostSheet AcceptCostSheetRevise(CostSheet oCostSheet, Int64 nUserID);
         
        CostSheet ChangeStatus(CostSheet oCostSheet, ApprovalRequest oApprovalRequest, Int64 nUserID);
         
        string Delete(int nCostSheetID, Int64 nUserID);
    }
    #endregion
    
    
   
}
