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
    #region ProductionProcedureTemplate
    
    public class ProductionProcedureTemplate : BusinessObject
    {
        public ProductionProcedureTemplate()
        {
            ProductionProcedureTemplateID = 0;
            TemplateNo = "";
            TemplateName = "";
            Remarks = "";
            ErrorMessage = "";
            ProductionProcedureTemplateDetails = new List<ProductionProcedureTemplateDetail>();
            ProductionProcedureTemplates = new List<ProductionProcedureTemplate>();
            ProductionSteps = new List<ProductionStep>();
        }

        #region Properties
         
        public int ProductionProcedureTemplateID { get; set; }
         
        public string TemplateNo { get; set; }
         
        public string TemplateName { get; set; }
         
        public string Remarks { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<ProductionProcedureTemplateDetail> ProductionProcedureTemplateDetails { get; set; }
        public List<ProductionProcedureTemplate> ProductionProcedureTemplates { get; set; }
        public List<ProductionStep> ProductionSteps { get; set; }
        #endregion

        #region Functions

        public static List<ProductionProcedureTemplate> Gets(long nUserID)
        {
            return ProductionProcedureTemplate.Service.Gets( nUserID);
        }

        public static List<ProductionProcedureTemplate> Gets(string sSQL, long nUserID)
        {
            return ProductionProcedureTemplate.Service.Gets(sSQL, nUserID);
        }

        public ProductionProcedureTemplate Get(int id, long nUserID)
        {
           
            return ProductionProcedureTemplate.Service.Get(id, nUserID);
        }

        public ProductionProcedureTemplate Save(long nUserID)
        {
            return ProductionProcedureTemplate.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductionProcedureTemplate.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IProductionProcedureTemplateService Service
        {
            get { return (IProductionProcedureTemplateService)Services.Factory.CreateService(typeof(IProductionProcedureTemplateService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionProcedureTemplate interface
     
    public interface IProductionProcedureTemplateService
    {
         
        ProductionProcedureTemplate Get(int id, Int64 nUserID);
         
        List<ProductionProcedureTemplate> Gets(Int64 nUserID);
         
        List<ProductionProcedureTemplate> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ProductionProcedureTemplate Save(ProductionProcedureTemplate oProductionProcedureTemplate, Int64 nUserID);
    }
    #endregion
}
