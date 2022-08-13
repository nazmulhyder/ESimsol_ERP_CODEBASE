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
    #region DeliveryPlan
    
    public class DeliveryPlan : BusinessObject
    {
        public DeliveryPlan()
        {
           
            DeliveryPlanID = 0;
            DeliveryPlanDate = DateTime.Today;
            DeliveryOrderID  = 0;
            PlanNo = "";
            Sequence = 0;
            Remarks = "";
            BUID = 0;
            ProductNature = EnumProductNature.Hanger;
            DOChallanStatus = "";
            DONo = "";
            RefNo = "";
            BuyerName = "";
            CustomerName = "";
            DeliveryToAddress = "";
            ContractorID = 0;
			BuyerID=0;
            DeliveryToName = "";
            DeliveryPlans = new List<DeliveryPlan>();
           
        }

        #region Properties
        public int DeliveryPlanID { get; set; }
        public DateTime DeliveryPlanDate { get; set; }
        public string DOChallanStatus { get; set; }
        public int DeliveryOrderID { get; set; }

        public string PlanNo { get; set; }

        public int Sequence { get; set; }

        public int BUID { get; set; }

        public string DONo { get; set; }
      
        public string Remarks { get; set; }
        public string RefNo { get; set; }
        public string BuyerName { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryToAddress { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public int ContractorID { get; set; }
		public int 	BuyerID { get; set; }
        public string DeliveryToName { get; set; }
        public string ErrorMessage { get; set; }
        

        #endregion

        #region Derived Property        
        public List<DeliveryPlan> DeliveryPlans { get; set;}
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        public string DeliveryPlanDateInString
        {
            get
            {
                return DeliveryPlanDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public static List<DeliveryPlan> GetsByBUwithDate(int BUID, int nProductNature, DateTime dPlanDate,  int nContractorID, long nUserID)
        {
            return DeliveryPlan.Service.Gets(BUID, nProductNature, dPlanDate, nContractorID, nUserID);
        }
     
       
        public List<DeliveryPlan> Save(long nUserID)
        {
            return DeliveryPlan.Service.Save(this, nUserID);
        }
        public static List<DeliveryPlan> Gets(string sSQL, long nUserID)
        {
            return DeliveryPlan.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return DeliveryPlan.Service.Delete(id, nUserID);
        }
  
        #endregion

        #region ServiceFactory
        internal static IDeliveryPlanService Service
        {
            get { return (IDeliveryPlanService)Services.Factory.CreateService(typeof(IDeliveryPlanService)); }
        }

        #endregion
    }
    #endregion

    #region IDeliveryPlan interface
     
    public interface IDeliveryPlanService
    {
        List<DeliveryPlan> Gets(int BUID, int nProductNature, DateTime dPlanDate, int nContractorID, Int64 nUserID);    
        List<DeliveryPlan> Gets(string sSQL, Int64 nUserID);    
        string Delete(int id, Int64 nUserID);
       List<DeliveryPlan> Save(DeliveryPlan oDeliveryPlan, Int64 nUserID);
     
    }
    #endregion
}