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
    #region ProductionProcedureTemplateDetail
    
    public class ProductionProcedureTemplateDetail : BusinessObject
    {
        public ProductionProcedureTemplateDetail()
        {
            ProductionProcedureTemplateDetailID = 0;
            ProductionProcedureTemplateID = 0;
            ProductionStepID = 0;
            Sequence = 0;
            Remarks = "";
            StepName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ProductionProcedureTemplateDetailID { get; set; }
         
        public int ProductionProcedureTemplateID { get; set; }
         
        public int ProductionStepID { get; set; }
         
        public int Sequence { get; set; }
         
        public string Remarks { get; set; }
         
        public string StepName { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<ProductionProcedureTemplateDetail> Gets(long nUserID)
        {
            return ProductionProcedureTemplateDetail.Service.Gets( nUserID);
        }

        public static List<ProductionProcedureTemplateDetail> Gets(int id, long nUserID)
        {

            return ProductionProcedureTemplateDetail.Service.Gets(id, nUserID);
        }

        public ProductionProcedureTemplateDetail Get(int id, long nUserID)
        {
            return ProductionProcedureTemplateDetail.Service.Get(id, nUserID);
        }

        public ProductionProcedureTemplateDetail Save(long nUserID)
        {
            return ProductionProcedureTemplateDetail.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductionProcedureTemplateDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static IProductionProcedureTemplateDetailService Service
        {
            get { return (IProductionProcedureTemplateDetailService)Services.Factory.CreateService(typeof(IProductionProcedureTemplateDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IProductionProcedureTemplateDetail interface
     
    public interface IProductionProcedureTemplateDetailService
    {
         
        ProductionProcedureTemplateDetail Get(int id, Int64 nUserID);
         
        List<ProductionProcedureTemplateDetail> Gets(Int64 nUserID);
         
        List<ProductionProcedureTemplateDetail> Gets(int id, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ProductionProcedureTemplateDetail Save(ProductionProcedureTemplateDetail oProductionProcedureTemplateDetail, Int64 nUserID);
    }
    #endregion
}
