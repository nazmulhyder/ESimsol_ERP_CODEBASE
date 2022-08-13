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


    #region TAPExecution
    
    public class TAPExecution : BusinessObject
    {
        public TAPExecution()
        {
            TAPExecutionID = 0;
            TAPDetailID = 0;
            TAPID  = 0;
            OrderStepID  = 0;
            RequiredDataType = EnumRequiredDataType.Text;
            TAPDetailSequence   = 0;
            OrderStepParentID = 0;
            UpdatedData = "";
            UpdateBy  = 0;
            UpdatedByName = "";
            OrderStepName = "";
            ApprovalPlanDate = DateTime.Now;
            RequiredDataTypeInInt = 0;
            IsDone = false;
		    DoneDate =DateTime.Now;
            BUID = 0;
            JobID = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int TAPExecutionID { get; set; }
         
        public int TAPDetailID { get; set; }
         
        public int TAPID { get; set; }
        public int BUID { get; set; }
         
        public int OrderStepID { get; set; }
         
        public EnumRequiredDataType RequiredDataType { get; set; }
         
        public int TAPDetailSequence { get; set; }
         
        public DateTime ApprovalPlanDate { get; set; }

        public int OrderStepParentID { get; set; }
         
        public string UpdatedData { get; set; }
         
        public int UpdateBy { get; set; }
         
        public double TotalQty { get; set; }
         
        public string UpdatedByName { get; set; }
         
        public string OrderStepName { get; set; }
         
        public int RequiredDataTypeInInt { get; set; }
         public bool  IsDone { get; set; }
         public DateTime DoneDate { get; set; }
         public int JobID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<User> Users { get; set; }
        public TTAPExecution TTAPExecution { get; set; }
        public IEnumerable<TAPExecution> ChildNodes { get; set; }
        public List<TAPExecution> TAPExecutions { get;set;}
        public Company Company { get; set; }
        public string ApprovalPlanDateInString
        {
            get
            {
                return this.ApprovalPlanDate.ToString("dd MMM yyyy");
            }
        }
        public string DoneDateInString
        {
            get
            {
                return this.DoneDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public static List<TAPExecution> Gets(long nUserID)
        {
            return TAPExecution.Service.Gets( nUserID);
        }
        public static List<TAPExecution> GetOrderSteps(int nTAPID, long nUserID)
        {
            return TAPExecution.Service.GetOrderSteps(nTAPID, nUserID);
        }
        public static List<TAPExecution> Gets(string sSQL, long nUserID)
        {
            return TAPExecution.Service.Gets(sSQL, nUserID);
        }
        public static List<TAPExecution> GetsByTaPs(TAP oTAP,  long nUserID)
        {
            return TAPExecution.Service.GetsByTaPs(oTAP, nUserID);
        }
        public TAPExecution Get(int id, long nUserID)
        {
            return TAPExecution.Service.Get(id, nUserID);
        }
     
        public List<TAPExecution> Save(long nUserID)
        {
            return TAPExecution.Service.Save(this, nUserID);
        }
        public TAPExecution Done(long nUserID)
        {
            return TAPExecution.Service.Done(this, nUserID);
        }
        
        public TAPExecution SingleSave(long nUserID)
        {
            return TAPExecution.Service.SingleSave(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return TAPExecution.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ITAPExecutionService Service
        {
            get { return (ITAPExecutionService)Services.Factory.CreateService(typeof(ITAPExecutionService)); }
        }

        #endregion
    }
    #endregion

    #region ITAPExecution interface
     
    public interface ITAPExecutionService
    {
         
        TAPExecution Get(int id, Int64 nUserID);
         
        List<TAPExecution> Gets(Int64 nUserID);
         
        List<TAPExecution> GetOrderSteps(int nTAPID, Int64 nUserID);
        
         
        List<TAPExecution> Gets(string sSQL, Int64 nUserID);
         
        List<TAPExecution> GetsByTaPs(TAP oTAP, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
       
         
        List<TAPExecution> Save(TAPExecution oTAPExecution, Int64 nUserID);
          
       TAPExecution SingleSave(TAPExecution oTAPExecution, Int64 nUserID);
       TAPExecution Done(TAPExecution oTAPExecution, Int64 nUserID);
       

    }
    #endregion

    #region TTAPExecution
    public class TTAPExecution
    {
        public TTAPExecution()
        {
            id = 0;
            text = "";
            TAPExecutionID = 0;
            parentid = 0;
            OrderStepID = 0;
            TAPDetailID = 0;
            UpDatedData = "";
            RequiredDataTypeInInt = 0;
            TAPID = 0;
            IsDone = false;
            DoneDate = DateTime.Now;
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public int TAPDetailID { get; set; }
        public int parentid { get; set; }
        public int TAPExecutionID { get; set; }
        public int OrderStepID { get; set; }
        public int TAPID { get; set; }
        public string UpDatedData { get; set; }
        public int RequiredDataTypeInInt { get; set; }
        public bool IsDone { get; set; }
        public DateTime DoneDate { get; set; }
        public string IsDoneInString
        {
            get
            {
                if(this.IsDone)
                {
                    return "Done";
                }
                else
                {
                    return "Not Done";
                }
            }
        }
       
        public IEnumerable<TTAPExecution> children { get; set; }//: an array nodes defines some children nodes
        public List<TTAPExecution> TTTAPExecutions { get; set; }
    }
    #endregion

}
