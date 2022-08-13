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
    #region GUProductionTracingUnit
    
    public class GUProductionTracingUnit : BusinessObject
    {
        public GUProductionTracingUnit()
        {
            GUProductionTracingUnitID = 0;
            SaleOrderID = 0;
            GUProductionOrderID = 0;
            TechnicalSheetID = 0;
            ColorID = 0;
            SizeID = 0;
            MeasurementUnitID = 0;
            OrderQty = 0;
            StyleNo = "";
            SaleOrderNo = "";
            BuyerID = 0;
            BuyerName = "";
            BuyerContactPerson = "";
            ProductID = 0;
            ProductName = "";
            ColorName = "";
            SizeName = "";
            GUProductionOrderNo = "";
            MeasurementUnitName = "";
            ErrorMessage = "";
            Quantity = 0;
            GUProductionProcedureID = 0;
            ProductionStepID = 0;
            ExecutionQty = 0;
            YetToExecutionQty = 0;
            ExecutionStartDate = DateTime.Now;
            StepName = "";
            GUProductionTracingUnits = new List<GUProductionTracingUnit>();
            TodayExecuteQty = 0;
            PLineConfigureID = 0;
            OperationDate = DateTime.Now;
            ProductionOperationNote = "";
            PreviousStepName = "";
            PreviousStepExecutionQty = 0;
            PreviousStepSequence = 0;
            PriviousStepID = 0;
            ActualWorkingHour = 0;
            UseHelper = 0;
            UseOperator = 0;
            IsUsesValueUpdate = false;
            PTUTransections = new List<PTUTransection>();
            GUProductionTracingUnitDetails = new List<GUProductionTracingUnitDetail>();
            GUProductionOrder = new GUProductionOrder();
            GUProductionProcedures = new List<GUProductionProcedure>();
            PriviousColorSizeRatios = new List<ColorSizeRatio>();
            TodayColorSizeRatios = new List<ColorSizeRatio>();
            YetToColorSizeRatios = new List<ColorSizeRatio>();
            PTUHistoryColorSizeRatios = new List<ColorSizeRatio>();
            TechnicalSheetSizes = new List<TechnicalSheetSize>();

        }

        #region Properties
         
        public int GUProductionTracingUnitID { get; set; }

        public int SaleOrderID { get; set; }
         
        public int GUProductionOrderID { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public string SizeName { get; set; }
         
        public int MeasurementUnitID { get; set; }
        public int PriviousStepID { get; set; }
        public double OrderQty { get; set; }
         
        public string StyleNo { get; set; }
         
        public string SaleOrderNo { get; set; }
         
        public int BuyerID { get; set; }
         
        public string BuyerName { get; set; }
         
        public string BuyerContactPerson { get; set; }
         
        public int ProductID { get; set; }
         
        public string ProductName { get; set; }       
         
        public string ColorName { get; set; }
         
        public string GUProductionOrderNo { get; set; }
         
        public string MeasurementUnitName { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public double Quantity { get; set; }
         
        public int GUProductionProcedureID { get; set; }
         
        public int ProductionStepID { get; set; }
         
        public double ExecutionQty { get; set; }
         
        public double YetToExecutionQty { get; set; }
         
        public DateTime ExecutionStartDate { get; set; }
         
        public string StepName { get; set; }
         
        public double TodayExecuteQty { get; set; }
         
        public string PreviousStepName { get; set; }
         
        public double PreviousStepExecutionQty { get; set; }
         
        public int PreviousStepSequence { get; set; }
        public double ActualWorkingHour { get; set; }
        public int UseHelper { get; set; }
        public int UseOperator { get; set; }        

        #endregion

        #region Derived Property
        public bool IsUsesValueUpdate { get; set; }
        public List<GUProductionTracingUnit> GUProductionTracingUnits { get; set; }
        public List<GUProductionTracingUnitDetail> GUProductionTracingUnitDetails { get; set; }

        public List<GUProductionProcedure> GUProductionProcedures { get; set; }
        public List<ColorSizeRatio> PriviousColorSizeRatios { get; set; }
        public List<ColorSizeRatio> TodayColorSizeRatios { get; set; }
        public List<ColorSizeRatio> YetToColorSizeRatios { get; set; }
        public List<ColorSizeRatio> PTUHistoryColorSizeRatios { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public GUProductionOrder GUProductionOrder { get; set; }
         
        public List<PTUTransection> PTUTransections { get; set; }
         
        public List<Employee> Employees { get; set; }
         
        public List<Contractor> BuyerList { get; set; }
         
        public List<Contractor> SupplierList { get; set; }
         
        public List<Contractor> ProductionFactoryList { get; set; }
        public int CurrentStepSequence { get; set; }
        public int PLineConfigureID { get; set; }
        public DateTime OperationDate { get; set; }
        public string ProductionOperationNote { get; set; }        
        #endregion

        #region Functions
        public static List<GUProductionTracingUnit> CommitProductionExecution(List<PTUTransection> oPTUTransections, long nUserID)
        {
            return GUProductionTracingUnit.Service.CommitProductionExecution(oPTUTransections, nUserID);
        }

        public static List<GUProductionTracingUnit> GetPTUViewDetails(DateTime nOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, long nUserID)
        {
            return GUProductionTracingUnit.Service.GetPTUViewDetails(nOperationDate,nExcutionFatoryId,nGUProductionOrderID, nUserID);
        }
        public static List<GUProductionTracingUnit> GetsByGUProductionOrder(int nGUProductionOrderID, long nUserID)
        {
            return GUProductionTracingUnit.Service.GetsByGUProductionOrder(nGUProductionOrderID, nUserID);
        }

        public static List<GUProductionTracingUnit> GetsPTU(int nGUProductionOrderID, int nProductionStepID, long nUserID)
        {
            return GUProductionTracingUnit.Service.GetsPTU(nGUProductionOrderID,nProductionStepID, nUserID);
        }

        public GUProductionTracingUnit Get(int id, long nUserID)
        {
            return GUProductionTracingUnit.Service.Get(id, nUserID);
            
        }

        public GUProductionTracingUnit Save(long nUserID)
        {
            return GUProductionTracingUnit.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GUProductionTracingUnit.Service.Delete(id, nUserID);
        }

        public static List<GUProductionTracingUnit> Gets(string sSQL, long nUserID)
        {
            return GUProductionTracingUnit.Service.Gets(sSQL, nUserID);
        }
        public static List<GUProductionTracingUnit> GetsByOrderRecap(int nOrderRecapID, long nUserID)
        {
            return GUProductionTracingUnit.Service.GetsByOrderRecap(nOrderRecapID, nUserID);
        }
        public static List<GUProductionTracingUnit> Gets_byPOIDs(string sPOIDs, long nUserID)
        {
            return GUProductionTracingUnit.Service.Gets_byPOIDs(sPOIDs, nUserID);
        }    
        
        #endregion

        #region ServiceFactory
        internal static IGUProductionTracingUnitService Service
        {
            get { return (IGUProductionTracingUnitService)Services.Factory.CreateService(typeof(IGUProductionTracingUnitService)); }
        }


        #endregion
    }
    #endregion

    #region IGUProductionTracingUnit interface
     
    public interface IGUProductionTracingUnitService
    {
         
        GUProductionTracingUnit Get(int id, Int64 nUserID);
         
        List<GUProductionTracingUnit> GetsByGUProductionOrder(int nGUProductionOrderID, Int64 nUserID);
         
        List<GUProductionTracingUnit> GetsPTU(int nGUProductionOrderID, int nProductionStepID, Int64 nUserID);
         
        List<GUProductionTracingUnit> GetPTUViewDetails(DateTime nOperationDate, int nExcutionFatoryId, int nGUProductionOrderID, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        GUProductionTracingUnit Save(GUProductionTracingUnit oGUProductionTracingUnit, Int64 nUserID);
         
        List<GUProductionTracingUnit> CommitProductionExecution(List<PTUTransection> oPTUTransections, Int64 nUserID);
         
        List<GUProductionTracingUnit> Gets(string sSQL, Int64 nUserID);
         
        List<GUProductionTracingUnit> GetsByOrderRecap(int nOrderRecapID, Int64 nUserID);
         
        List<GUProductionTracingUnit> Gets_byPOIDs(string sPOIDs, Int64 nUserID);
        
    }
    #endregion

}
