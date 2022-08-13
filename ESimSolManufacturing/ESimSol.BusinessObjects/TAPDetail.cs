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
    #region TAPDetail
    public class TAPDetail : BusinessObject
    {
        public TAPDetail()
        {
            TAPDetailID = 0;
            TAPID = 0;
            OrderStepID = 0;
            OrderStepName = "";
            Sequence = 0;
            ApprovalPlanDate = DateTime.Now;
            Remarks = "";
            OrderStepParentID = 0;
            OrderStepSequence = 0;
            ExecutionIsDone = false;
            ExecutionDoneDate = DateTime.Now;
            UpdatedData = "";
            GroupName = "";
            GroupID = 0;
            GroupSequence = 0;
            OrderRecapNo = "";
            TSType = EnumTSType.Sweater;
            TSTypeInt = (int)EnumTSType.Sweater;
            RequiredDataType = EnumRequiredDataType.Text;
            TnAStep = EnumTnAStep.None;
            ChildOrderSteps = new List<TAPDetail>();
            SubmissionDate = Convert.ToDateTime("01 Jan 1870");
            ReqSubmissionDays= 0;
            ReqBuyerApprovalDays =0;
            SubStepName = "";
            StepType = EnumStepType.Approval;
            TnAStepInt = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int TAPDetailID { get; set; }        
        public int TAPID { get; set; }
        public int OrderStepID { get; set; }
        public string OrderStepName { get; set; }
        public EnumTnAStep TnAStep { get; set; }
        public int TnAStepInt { get; set; }
        public int Sequence { get; set; }
        public EnumRequiredDataType RequiredDataType { get; set; }
        public DateTime ApprovalPlanDate { get; set; }
        public int OrderStepParentID { get; set; }
        public int OrderStepSequence { get; set; }
        public bool ExecutionIsDone {get;set;}
        public DateTime ExecutionDoneDate { get; set; }
        public string Remarks { get; set; }
        public bool bIsUp { get; set; }
        public int    GroupID { get; set; }
        public string    GroupName { get; set; }
        public string UpdatedData { get; set; }
        public int GroupSequence { get; set; }
        public string OrderRecapNo { get; set; }
        public EnumTSType TSType { get; set; }
        public int TSTypeInt { get; set; }

        public DateTime SubmissionDate { get; set; }
        public int ReqSubmissionDays { get; set; }
        public int ReqBuyerApprovalDays { get; set; }
        public string SubStepName { get; set; }
        public EnumStepType StepType { get; set; }	
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<TAPDetail> ChildOrderSteps { get; set; }
        public IEnumerable<TAPDetail> ChildNodes { get; set; }
        public List<TAPExecution> TAPExecutions { get; set; }
        public TAPExecution TAPExecution { get; set; }
        public string SubmissionDateInString
        {
            get
            {
                if (this.SubmissionDate == Convert.ToDateTime("01 Jan 1870"))
                {
                    return "";
                }
                else
                {
                    return this.SubmissionDate.ToString("dd MMM yyyy");
                }
            }
        }
        
        public string ApprovalPlanDateInString
        {
            get
            {
                    return this.ApprovalPlanDate.ToString("dd MMM yyyy");
              
            }
        }
      
        public string ExecutionDoneDateInString
        {
            get
            {
                return this.ExecutionDoneDate.ToString("dd MMM yyyy");
            }
        }
        public string SequenceInString
        {
            get
            {
                if (this.OrderStepParentID == 1)
                {
                    return this.Sequence.ToString();
                }
                else
                {
                    return this.OrderStepSequence.ToString();
                }
            }
        }

        public string TestingDateFormat
        {
            get
            {
                return this.ApprovalPlanDate.ToString("MM/dd/yyyy");
            }
        }
        public string TestingNormalDateFormat
        {
            get
            {
                return this.ApprovalPlanDate.ToString("dd/MM/yyyy");
            }
        }
        #endregion

        #region Functions

        public static List<TAPDetail> Gets(int TAPID, long nUserID)
        {
            return TAPDetail.Service.Gets(TAPID, nUserID);
        }

        public static List<TAPDetail> FactoryTAPGets(int TAPID, long nUserID)
        {
            return TAPDetail.Service.FactoryTAPGets(TAPID, nUserID);
        }
        public static List<TAPDetail> Gets(string sSQL, long nUserID)
        {
            return TAPDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<TAPDetail> Gets(long nUserID)
        {
            return TAPDetail.Service.Gets( nUserID);
        }
        public TAPDetail Get(int TAPDetailID, long nUserID)
        {
            return TAPDetail.Service.Get(TAPDetailID, nUserID);
        }
        public TAPDetail Save(long nUserID)
        {
            return TAPDetail.Service.Save(this, nUserID);
        }

        #endregion

        #region ServiceFactory

       
        internal static ITAPDetailService Service
        {
            get { return (ITAPDetailService)Services.Factory.CreateService(typeof(ITAPDetailService)); }
        }

        #endregion
    }
    #endregion

    #region ITAPDetail interface
     
    public interface ITAPDetailService
    {
         
        TAPDetail Get(int TAPDetailID, Int64 nUserID);
         
        List<TAPDetail> Gets(int TAPID, Int64 nUserID);


        List<TAPDetail> FactoryTAPGets(int TAPID, Int64 nUserID);
         
        List<TAPDetail> Gets(string sSQL, Int64 nUserID);
         
        List<TAPDetail> Gets(Int64 nUserID);
         
        TAPDetail Save(TAPDetail oTAPDetail, Int64 nUserID);


    }
    #endregion
   

}
