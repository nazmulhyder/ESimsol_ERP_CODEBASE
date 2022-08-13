using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductionSheet
    public class ProductionSheet : BusinessObject
    {
        public ProductionSheet()
        {
            ProductionSheetID = 0;
            SheetNo = "";
            PTUUnit2ID = 0;
            IssueDate = DateTime.Now;
            ProductionOrderID = 0;
            ProductID = 0;
            Note = "";
            ProductCode = "";
            ProductName = "";
            PONo = "";
            BUID  = 0;
            ModelReferenceID = 0;
            Quantity = 0;
            FGWeight = 0;
            NaliWeight = 0;
            WeightFor = 0;
            FGWeightUnitID = 0;
            RecipeID = 0;
            RecipeName = "";
            ColorName = "";
            ProdOrderQty = 0;
            ExportSCQty = 0;
            YetToSheetQty = 0;
            ExportPINo = "";
            ContractorID = 0;
            ContractorName = "";
            ModelReferencenName = "";
            SheetStatus = EnumProductionSheetStatus.Initialize;
            RawMaterialStatus = EnumRawMaterialStatus.YetToRawMaterialOut;
            QCStatus = EnumQCStatus.Yet_To_QC;
            ApprovedBy = 0;
            ApprovedByName = "";
            UnitID = 0;
            UnitSymbol = "";
            LastExecutionStepName = "";
            LastExecutionStepQty = 0;
            QCQty = 0;
            MachineID = 0;
            MachineName = "";
            PerCartonFGQty = 0;
            FGWeightUnitSymbol = "";
            Cavity = 0;
            BuyerName = "";
            ProductDescription = "";
            ProductSize = "";
            Measurement = "";
            ProductNature = EnumProductNature.Hanger;
            ProductNatureInInt = 0;
            OrderColorName = "";
            ErrorMessage = "";
            YetToPlanQty = 0;
            ProductionStartBy = 0;
            ProductionStartByName = "";
            ProductionStartDate = DateTime.Now;
            ProductionSheets = new List<ProductionSheet>();
            ProductionProcedures = new List<ProductionProcedure>();
            ProductionRecipes = new List<ProductionRecipe>();
            ProductionSteps = new List<ProductionStep>();
            BusinessUnit = new BusinessUnit();
            ProductionExecutions = new List<ProductionExecution>();
            UnitConversions = new List<UnitConversion>();
        }

        #region Properties        
        public int ProductionSheetID { get; set; }
        public string SheetNo { get; set; }
        public int PTUUnit2ID { get; set; }
        public DateTime IssueDate { get; set; }
        public int ProductionOrderID { get; set; }
        public int ProductID { get; set; }
        public string Note { get; set; }
        public double Quantity { get; set; }      
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PONo { get; set; }
        public int    BUID   { get; set; }
        public int   ModelReferenceID  { get; set; }
        public int ContractorID { get; set; }
        public double  FGWeight  { get; set; }
        public double NaliWeight  { get; set; }
        public double WeightFor { get; set; }
        public int  FGWeightUnitID  { get; set; }
        public int   RecipeID  { get; set; }
        public string   RecipeName  { get; set; }
        public string    ColorName  { get; set; }
        public string Measurement { get; set; }
        public double ProdOrderQty { get; set; }
        public double ExportSCQty { get; set; }
        public double   YetToSheetQty  { get; set; }
        public string  ExportPINo  { get; set; }
        public string ContractorName { get; set; }
        public string ModelReferencenName { get; set; }
        public EnumProductionSheetStatus SheetStatus { get; set; }
        public EnumRawMaterialStatus RawMaterialStatus { get; set; }
        public EnumQCStatus QCStatus { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public int SheetStatusInInt { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public int  UnitID { get; set; }
        public string UnitSymbol { get; set; }
        public string  LastExecutionStepName { get; set; }
        public double LastExecutionStepQty { get; set; }
        public double QCQty { get; set; }
        public int MachineID { get; set; }
        public double PerCartonFGQty { get; set; }
        public string MachineName { get; set; }
        public string FGWeightUnitSymbol { get; set; }
        public int Cavity { get; set; }
        public string BuyerName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSize { get; set; }
        public string OrderColorName { get; set; }
        public double YetToPlanQty { get; set; }
        public List<ProductionStep> ProductionSteps { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public int RawMaterialStatusInInt { get; set; }
        public int  ProductionStartBy { get; set; }
       public string   ProductionStartByName { get; set; }
       public DateTime ProductionStartDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<ProductionSheet> ProductionSheets { get; set; }
        public Company Company { get; set; }
        public List<ProductionProcedure> ProductionProcedures { get; set; }
        public List<UnitConversion> UnitConversions { get; set; }
        public List<ProductionRecipe> ProductionRecipes { get; set; }
        public List<ProductionExecution> ProductionExecutions { get; set; }
        public double YetToQCQty
        {
            get
            {
                return Math.Round((this.LastExecutionStepQty - this.QCQty),2);
            }
        }
        public string SheetStatusInString
        {
            get
            {
                return this.SheetStatus.ToString();
            }
        }
        public string RawMaterialStatusInString
        {
            get
            {
                return this.RawMaterialStatus.ToString();
            }
        }
        public string QCStatusInString
        {
            get
            {
                return this.QCStatus.ToString();
            }
        }
        public string IssueDateInString
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }


        public string ProductionStartDateSt
		{
			get
            {
                if (this.ProductionStartDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return ProductionStartDate.ToString("dd MMM yyyy hh:mm tt");
                }
			}
		}
        public double TotalWeight
        {
            get
            {
                return (this.FGWeight + this.NaliWeight);
            }

        }
        public string QuantityInString
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Quantity)+" "+this.UnitSymbol;
            }

        }
        
     
        #endregion

        #region Functions

        public static List<ProductionSheet> Gets(long nUserID)
        {
            return ProductionSheet.Service.Gets(nUserID);
        }
        public static List<ProductionSheet> BUWiseGets(int nBUID,int nProductNature, long nUserID)
        {
            return ProductionSheet.Service.BUWiseGets(nBUID,nProductNature, nUserID);
        }
        public static List<ProductionSheet> Gets(string sSQL, Int64 nUserID)
        {
            return ProductionSheet.Service.Gets(sSQL, nUserID);
        }
        public ProductionSheet Get(int nId, long nUserID)
        {
            return ProductionSheet.Service.Get(nId,nUserID);
        }
               
        public ProductionSheet Save(long nUserID)
        {
            return ProductionSheet.Service.Save(this, nUserID);
        }
        public ProductionSheet ChangeRawMaterial(long nUserID)
        {
            return ProductionSheet.Service.ChangeRawMaterial(this, nUserID);
        }
        public ProductionSheet Approve(long nUserID)
        {
            return ProductionSheet.Service.Approve(this, nUserID);
        }

        public ProductionSheet ProductionStart(long nUserID)
        {
            return ProductionSheet.Service.ProductionStart(this, nUserID);
        }
        public ProductionSheet ProductionUndo(long nUserID)
        {
            return ProductionSheet.Service.ProductionUndo(this, nUserID);
        }
        public ProductionSheet UndoApproved(long nUserID)
        {
            return ProductionSheet.Service.UndoApproved(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ProductionSheet.Service.Delete(nId, nUserID);
        }        
        #endregion

        #region ServiceFactory
        internal static IProductionSheetService Service
        {
            get { return (IProductionSheetService)Services.Factory.CreateService(typeof(IProductionSheetService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionSheet interface
    
    public interface IProductionSheetService
    {
        ProductionSheet Get(int id, long nUserID);
        List<ProductionSheet> Gets(long nUserID);
        List<ProductionSheet> BUWiseGets(int nBUID, int nProductNature, long nUserID);
        List<ProductionSheet> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        ProductionSheet Save(ProductionSheet oProductionSheet, long nUserID);
        ProductionSheet ChangeRawMaterial(ProductionSheet oProductionSheet, long nUserID);
        ProductionSheet Approve(ProductionSheet oProductionSheet, long nUserID);
        ProductionSheet ProductionStart(ProductionSheet oProductionSheet, long nUserID);
        ProductionSheet ProductionUndo(ProductionSheet oProductionSheet, long nUserID);
        ProductionSheet UndoApproved(ProductionSheet oProductionSheet, long nUserID);
    }
    #endregion
}