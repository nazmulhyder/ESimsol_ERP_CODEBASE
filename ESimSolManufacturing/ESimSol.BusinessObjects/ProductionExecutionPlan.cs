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


    #region ProductionExecutionPlan
    
    public class ProductionExecutionPlan : BusinessObject
    {
        public ProductionExecutionPlan()
        {
            ProductionExecutionPlanID = 0;
            RefNo = "";
            OrderRecapID= 0;
            TechnicalSheetID = 0;
            PlanDate = DateTime.Now;
            ShipmentDate = DateTime.Now;
            FactoryShipmentDate = DateTime.Now;
            ProductionQty = 0;
            PlanExecutionQty = 0;
            Note = "";
            ApproveBy = 0;
            BuyerID =0;
            ProductID = 0;
            BuyerName = "";
            ApproveByName = "";
            ProductName = "";
            RecapNo = "";
            MerchandiserName = "";
            BUID = 0;
            FBUID = 0;
            SMV = 0;
            PlanStatus = EnumPlanStatus.None;
            ProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int ProductionExecutionPlanID { get; set; }
         
        public int OrderRecapID{ get; set; }
         
        public string RefNo { get; set; }

        public int BuyerID { get; set; }
         
        public string BuyerName { get; set; }
         
        public string StyleNo { get; set; }
        
        public string RecapNo { get; set; }
        public int BUID { get; set; }
        public int FBUID { get; set; }
        public EnumPlanStatus PlanStatus { get; set; }
        public DateTime PlanDate { get; set; }
         
        public DateTime ShipmentDate { get; set; }
        public DateTime FactoryShipmentDate { get; set; }
         
        public double ProductionQty { get; set; }
         
        public int ProductID { get; set; }
         
        public int TechnicalSheetID { get; set; }

        public int ApproveBy { get; set; }
         
        public string ApproveByName { get; set; }
         
        public string ProductName { get; set; }
         
        public string MerchandiserName { get; set; }

        public double PlanExecutionQty { get; set; }
        public double RecapQty { get; set; }
        public string Note { get; set; }
        public int PlanStatusInInt { get; set; }
        public double SMV { get; set; }
         public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<User> Users { get; set; }
         
        public List<Employee> MerchandiserList { get; set; }
    
         
        public TechnicalSheetThumbnail TechnicalSheetThumbnail { get; set; }
        public string FactoryShipmentDateSt
        {
            get
            {
                return this.FactoryShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string PlanDateInString
        {
            get
            {
               return PlanDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateInString
        {
            get
            {
               return ShipmentDate.ToString("dd MMM yyyy");
             
            }
        }
   
        public string PlanStatusInString
        {
            get
            {
                return EnumObject.jGet(this.PlanStatus);
            }
        }
        public string RefNoNoWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing deshboard
                return this.ProductionExecutionPlanID + "~" + this.RefNo + "~6";
            }
        }
        public List<ProductionExecutionPlan> ProductionExecutionPlans { get; set; }
         
        public List<ProductionExecutionPlanDetail> ProductionExecutionPlanDetails { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<ProductionExecutionPlan> Gets(long nUserID)
        {
            return ProductionExecutionPlan.Service.Gets( nUserID);
        }

        public static List<ProductionExecutionPlan> Gets(string sSQL, long nUserID)
        {
            return ProductionExecutionPlan.Service.Gets(sSQL, nUserID);
        }

        public ProductionExecutionPlan Get(int id, long nUserID)
        {
            return ProductionExecutionPlan.Service.Get(id, nUserID);
        }
        public ProductionExecutionPlan GetByOrderRecap(int nOrderRecapId, long nUserID)
        {
            return ProductionExecutionPlan.Service.GetByOrderRecap(nOrderRecapId, nUserID);
        }
        public ProductionExecutionPlan Approve(int id, long nUserID)
        {
            return ProductionExecutionPlan.Service.Approve(id, nUserID);
        }
        public ProductionExecutionPlan RequestForRevise(int id, long nUserID)
        {
            return ProductionExecutionPlan.Service.RequestForRevise(id, nUserID);
        }
        public ProductionExecutionPlan Save(long nUserID)
        {
            return ProductionExecutionPlan.Service.Save(this, nUserID);
        }

        public ProductionExecutionPlan AcceptRevise(long nUserID)
        {
            return ProductionExecutionPlan.Service.AcceptRevise(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductionExecutionPlan.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IProductionExecutionPlanService Service
        {
            get { return (IProductionExecutionPlanService)Services.Factory.CreateService(typeof(IProductionExecutionPlanService)); }
        }

        #endregion
    }
    #endregion



    #region IProductionExecutionPlan interface
     
    public interface IProductionExecutionPlanService
    {
         
        ProductionExecutionPlan Get(int id, Int64 nUserID);
        ProductionExecutionPlan GetByOrderRecap(int nOrderRecapId, Int64 nUserID);         
        ProductionExecutionPlan Approve(int id, Int64 nUserID);
        ProductionExecutionPlan RequestForRevise(int id, Int64 nUserID);
        
         
        List<ProductionExecutionPlan> Gets(Int64 nUserID);
         
        List<ProductionExecutionPlan> Gets(string sSQL, Int64 nUserID);

         
        string Delete(int id, Int64 nUserID);
      
         
        ProductionExecutionPlan Save(ProductionExecutionPlan oProductionExecutionPlan, Int64 nUserID);
        ProductionExecutionPlan AcceptRevise(ProductionExecutionPlan oProductionExecutionPlan, Int64 nUserID);
        

    }
    #endregion
    
 
}
