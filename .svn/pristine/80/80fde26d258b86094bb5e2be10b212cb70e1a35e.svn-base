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
    #region ProductionStep
    
    public class ProductionStep : BusinessObject
    {
        public ProductionStep()
        {
            ProductionStepID = 0;
            StepName = "";
            ProductionStepType = EnumProductionStepType.Regular;
            ProductionStepTypeInt = 0;
            Note = "";
            ShortName = "";
            ErrorMessage = "";
            ProductionStepTypeObjs = new List<EnumObject>();
            ProductionStepList = new List<ProductionStep>();
        }

        #region Properties
        public int ProductionStepID { get; set; }         
        public string StepName { get; set; }
        public string ShortName { get; set; }
        public EnumProductionStepType ProductionStepType { get; set; }
        public int ProductionStepTypeInt { get; set; }
        public string Note { get; set; }         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductionStepTypeSt
        {
            get
            {
                //return ProductionStepTypeObj.GetProductionStepTypeName(this.ProductionStepType);
                return EnumObject.jGet(this.ProductionStepType);
            }
        }
        public bool Selected { get; set; }
        public List<ProductionStep> ProductionStepList { get; set; }
        public List<EnumObject> ProductionStepTypeObjs { get; set; }
        #endregion

        #region Functions

        public static List<ProductionStep> Gets(long nUserID)
        {
            return ProductionStep.Service.Gets(nUserID);
        }

        public static List<ProductionStep> Gets(string sSQL, long nUserID)
        {
            return ProductionStep.Service.Gets(sSQL, nUserID);
        }
        

        public ProductionStep Get(int id, long nUserID)
        {
            return ProductionStep.Service.Get(id, nUserID);
        }

        public ProductionStep Save(long nUserID)
        {
            return ProductionStep.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductionStep.Service.Delete(id, nUserID);
        }        
        #endregion

        #region ServiceFactory
        internal static IProductionStepService Service
        {
            get { return (IProductionStepService)Services.Factory.CreateService(typeof(IProductionStepService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionStep interface
     
    public interface IProductionStepService
    {
         
        ProductionStep Get(int id, Int64 nUserID);        
         
        List<ProductionStep> Gets(Int64 nUserID);
         
        List<ProductionStep> Gets(string sSQL, Int64 nUserID);        
         
        string Delete(int id, Int64 nUserID);
         
        ProductionStep Save(ProductionStep oProductionStep, Int64 nUserID);
    }
    #endregion
}
