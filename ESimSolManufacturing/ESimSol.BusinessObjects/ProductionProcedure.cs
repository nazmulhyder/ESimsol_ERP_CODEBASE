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
    #region ProductionProcedure
    
    public class ProductionProcedure : BusinessObject
    {
        public ProductionProcedure()
        {
            ProductionProcedureID = 0;
            ProductionSheetID = 0;
            ProductionStepID = 0;
            Sequence = 0;
            Remarks = "";
            StepName = "";
            Measurement = "";
            ThickNess = "";
            ProductionStepType = EnumProductionStepType.Regular;
            ErrorMessage = "";
        }

        #region Properties
         
        public int ProductionProcedureID { get; set; }
         
        public int ProductionSheetID { get; set; }
         
        public int ProductionStepID { get; set; }
         
        public int Sequence { get; set; }

        public string Remarks { get; set; }
         
        public string StepName { get; set; }
        public string Measurement { get; set; }
        public string ThickNess { get; set; }
        public EnumProductionStepType ProductionStepType { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductionStepTypeInString
        {
            get
            {
                return this.ProductionStepType.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<ProductionProcedure> Gets(long nUserID)
        {
            return ProductionProcedure.Service.Gets( nUserID);
        }
        public static List<ProductionProcedure> Gets(string sSQL, long nUserID)
        {
            return ProductionProcedure.Service.Gets(sSQL, nUserID);
        }
        public static List<ProductionProcedure> Gets(int nProductionSheetID, long nUserID)
        {
            return ProductionProcedure.Service.Gets(nProductionSheetID, nUserID);
        }
      
        public ProductionProcedure Get(int nProductionOrderID, long nUserID)
        {
            return ProductionProcedure.Service.Get(nProductionOrderID, nUserID);
        }

        public static List<ProductionProcedure> Save(ProductionSheet oProductionSheet, long nUserID)
        {
            
            return ProductionProcedure.Service.Save(oProductionSheet, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
               return ProductionProcedure.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IProductionProcedureService Service
        {
            get { return (IProductionProcedureService)Services.Factory.CreateService(typeof(IProductionProcedureService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionProcedure interface
     
    public interface IProductionProcedureService
    {
        ProductionProcedure Get(int id, Int64 nUserID);
        List<ProductionProcedure> Gets(Int64 nUserID);
        List<ProductionProcedure> Gets(string sSQL, Int64 nUserID);
        List<ProductionProcedure> Gets(int nProductionSheetID, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        List<ProductionProcedure> Save(ProductionSheet oProductionSheet, Int64 nUserID);
    }
    #endregion
}
