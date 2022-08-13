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
    #region HIASetup
    
    public class HIASetup : BusinessObject
    {
        public HIASetup()
        {
            HIASetupID = 0;
            HIASetupType = EnumHIASetupType.Manual;
            SetupName = "";
            DBTable = "";
            KeyColumn = "";
            FileNumberColumn = "";
            SenderColumn = "";
            ReceiverColumn = "";
            WhereClause = "";
            MessageBodyText = "";
            Activity = true;
            LinkReference = "";
            Parameter = "";
            OrderStepID  = 0;
            TimeEventType = EnumTimeEventType.ON_Date;
            TimeEventTypeInInt = 0;
            TimeCounter = 0;
            HIASetupTypeInInt = 0;
            BUID = 0;
            OperationName = "";
            OperationValue = "";
            ErrorMessage = "";
        }

        #region Properties

        public int HIASetupID { get; set; }
        public int HIASetupTypeInInt { get; set; }
        public EnumHIASetupType HIASetupType { get; set; }
        public int OrderStepID { get; set; }

        public EnumTimeEventType TimeEventType { get; set; }
        public int TimeEventTypeInInt { get; set; }
        public int TimeCounter { get; set; }
        public string SetupName { get; set; }
         
        public string DBTable { get; set; }
         
        public string KeyColumn { get; set; }
         
        public string FileNumberColumn { get; set; }
         
        public string SenderColumn { get; set; }
         
        public string ReceiverColumn { get; set; }
         
        public string WhereClause { get; set; }
         
        public string MessageBodyText { get; set; }
         
        public string LinkReference { get; set; }
         
        public bool Activity { get; set; }
         
        public string Parameter { get; set; }
        public string OperationName { get; set; }
        public string OperationValue { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public List<HIAUserAssign> HIAUserAssigns { get; set; }
        public  string TimeCounterInString
        {
            get
            {
                return this.TimeCounter.ToString()+" Days";
            }
        }
             
        public string TimeEventTypeInString
        {
            get
            {
                return this.TimeEventType.ToString();
            }
        }
        
        #endregion 

        #region Functions
        public static List<HIASetup> Gets(int buid, long nUserID)
        {
            return HIASetup.Service.Gets(buid, nUserID);
        }
        public static List<HIASetup> GetsByOrderStep(int id, long nUserID)
        {
            return HIASetup.Service.GetsByOrderStep(id, nUserID);
        }
        public static List<HIASetup> GetsByOrderStepBUWise(int id, int buid, long nUserID)
        {
            return HIASetup.Service.GetsByOrderStepBUWise(id, buid, nUserID);
        }
        public HIASetup Get(int id, long nUserID)
        {
            return HIASetup.Service.Get(id, nUserID);
        }

        public HIASetup Save(long nUserID)
        {
            return HIASetup.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {

            return HIASetup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IHIASetupService Service
        {
            get { return (IHIASetupService)Services.Factory.CreateService(typeof(IHIASetupService)); }
        }
        #endregion
    }
    #endregion

    #region IHIASetupinterface
     
    public interface IHIASetupService
    {
        HIASetup Get(int id, Int64 nUserID);
        List<HIASetup> Gets(int buid, Int64 nUserID);
        List<HIASetup> GetsByOrderStep(int id,long nUserID);
        List<HIASetup> GetsByOrderStepBUWise(int id, int buid, long nUserID);
        string Delete(int id, Int64 nUserID);
        HIASetup Save(HIASetup oHIASetup, Int64 nUserID);
    }
    #endregion
}
