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

    #region OrderStep
    
    public class OrderStep : BusinessObject
    {
        public OrderStep()
        {
            OrderStepID = 0;
            OrderStepName = "";
            RequiredDataType = EnumRequiredDataType.Text;
            Sequence = 0;
            RequiredDataTypeInInt = 0;
            IsNotificationSend = true;
            IsActive = true;
            Note = "";
            SubStepName = "";
            StyleType = EnumTSType.Knit;
            TnAStep = EnumTnAStep.None;
            StepType = EnumStepType.Approval;
            ErrorMessage = "";

        }

        #region Properties
         
        public int OrderStepID { get; set; }
        public int ParentID { get; set; }
        public string OrderStepName { get; set; }       
        public string SubStepName { get; set; }   
        public EnumTSType StyleType { get; set; }   
        public EnumStepType StepType { get; set; }   
        public EnumTnAStep TnAStep { get; set; }   
        public string Note { get; set; }
        public EnumRequiredDataType RequiredDataType { get; set; }
        public int RequiredDataTypeInInt { get; set; }
        public List<OrderStep> ChildOrderSteps { get; set; }
        public int Sequence { get; set; }
        public bool IsNotificationSend { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<OrderStep> ChildNodes { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<OrderStep> OrderSteps { get; set; }
         
        //public List<OrderStepDetail> OrderStepDetails { get; set; }
        public Company Company { get; set; }

        public string StyleTypeString
        {
            get
            {

                return EnumObject.jGet(this.StyleType);

            }

        }

        public string StepTypeString
        {
            get
            {

                return EnumObject.jGet(this.StepType);

            }

        }
        public string TnAStepString
        {
            get
            {

                return EnumObject.jGet(this.TnAStep);
              
            }

        }

        public string  RequiredDataTypeInString
        {
           get
           {
             
              return RequiredDataType.ToString();
           }
            
        }
        public string IsNotificationSendInString
        {
            get
            {
                    if (this.IsNotificationSend)
                    {
                        return "Yes";
                    }
                    else
                    {
                        return "No";
                    }
              
            }
        }
        public string IsActiveSt
        {
            get
            {
                if (this.IsActive)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions

        public static List<OrderStep> Gets(long nUserID)
        {
            return OrderStep.Service.Gets( nUserID);
        }
        public static List<OrderStep> Gets(int nStyleType, long nUserID)
        {
            return OrderStep.Service.Gets(nStyleType, nUserID);
        }
        public static List<OrderStep> Gets(string sSQL, long nUserID)
        {
            return OrderStep.Service.Gets(sSQL, nUserID);
        }

        public OrderStep Get(int id, long nUserID)
        {
            return OrderStep.Service.Get(id, nUserID);
        }
        public OrderStep Save(long nUserID)
        {
            return OrderStep.Service.Save(this, nUserID);
        }

        public OrderStep RefreshStepSequence(long nUserID)
        {
            return OrderStep.Service.RefreshStepSequence(this, nUserID);
        }
        public OrderStep ActiveInActive(long nUserID)
        {
            return OrderStep.Service.ActiveInActive(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return OrderStep.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory

        internal static IOrderStepService Service
        {
            get { return (IOrderStepService)Services.Factory.CreateService(typeof(IOrderStepService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class OrderStepList : List<OrderStep>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IOrderStep interface
     
    public interface IOrderStepService
    {    
        OrderStep Get(int id, Int64 nUserID);         
        List<OrderStep> Gets(Int64 nUserID);      
        List<OrderStep> Gets(int nStyleType, Int64 nUserID);      
        List<OrderStep> Gets(string sSQL, Int64 nUserID);         
        string Delete(int id, Int64 nUserID);         
        OrderStep Save(OrderStep oOrderStep, Int64 nUserID);
        OrderStep RefreshStepSequence(OrderStep oOrderStep, Int64 nUserID);
        OrderStep ActiveInActive(OrderStep oOrderStep, Int64 nUserID);
    }
    #endregion
  
}
