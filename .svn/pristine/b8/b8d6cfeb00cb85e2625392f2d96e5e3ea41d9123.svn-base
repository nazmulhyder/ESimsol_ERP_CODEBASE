using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{

    #region ProductionExecution

    public class ProductionExecution : BusinessObject
    {
        public ProductionExecution()
        {
            ProductionExecutionID = 0;
            ProductionProcedureID = 0;
            ProductionStepID = 0;
            ExecutionQty = 0;
            YetToExecution = 0;
            ProductID = 0;
            ExecutionDate = DateTime.Now;
            StepName = "";
            StepShortName = "";
            ProductCode = "";
            ProductName = "";
            ProductionSheetID = 0;
            Sequence = 0;
            ProductionStepType = EnumProductionStepType.Regular;
            ProductionStepTypeInInt = 0;
            ErrorMessage = "";
            MeasurementUnits = new List<MeasurementUnit>();
            PETransactions = new List<PETransaction>();

        }

        #region Properties
        public int ProductionExecutionID { get; set; }
        public int ProductionStepID { get; set; }
        public double ExecutionQty { get; set; }
        public double YetToExecution { get; set; }
        public int  ProductID { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string StepName { get; set; }
        public string StepShortName { get; set; }
        public string ProductCode { get; set; }
         public string ProductName { get; set; }
        public int ProductionProcedureID { get; set; }
        public int ProductionSheetID { get; set; }
        public int ContractorID { get; set; }
        public int Sequence { get; set; }
        public EnumProductionStepType ProductionStepType { get; set; }
        public int ProductionStepTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived ProductionExecution
        public string ExecutionDateInString
        {
            get
            {
                return this.ExecutionDate.ToString("dd MMM yyyy");   
            }
        }

        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<PETransaction> PETransactions { get; set; }
        public List<ProductionExecution> ProductionExecutions { get; set; }
        public Company Company { get; set; }
        
        #endregion

        #region Functions

        public static List<ProductionExecution> Gets(long nUserID)
        {
            return ProductionExecution.Service.Gets(nUserID);
        }
        public static List<ProductionExecution> Gets(int nPSID, long nUserID)
        {
            return ProductionExecution.Service.Gets(nPSID, nUserID);
        }
        public static List<ProductionExecution> Gets(string sSQL, long nUserID)
        {
            return ProductionExecution.Service.Gets(sSQL, nUserID);
        }
        public ProductionExecution Get(int id, long nUserID)
        {
            return ProductionExecution.Service.Get(id, nUserID);
        }
        public ProductionExecution Save(long nUserID)
        {
            return ProductionExecution.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ProductionExecution.Service.Delete(id, nUserID);
        }
        #endregion

        #region Non DB Function
     
        #endregion

        #region ServiceFactory
        internal static IProductionExecutionService Service
        {
            get { return (IProductionExecutionService)Services.Factory.CreateService(typeof(IProductionExecutionService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionExecution interface

    public interface IProductionExecutionService
    {
        ProductionExecution Get(int id, Int64 nUserID);
        List<ProductionExecution> Gets(Int64 nUserID);
        List<ProductionExecution> Gets(int nPSID, Int64 nUserID);      
        List<ProductionExecution> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ProductionExecution Save(ProductionExecution oProductionExecution, Int64 nUserID);

      
    }
    #endregion
  
}
