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
    #region HumanInteractionAgent
    
    public class HumanInteractionAgent : BusinessObject
    {
        public HumanInteractionAgent()
        {
            HIAID = 0;
            HIASetupID = 0;
            MessageBodyText = "";
            ProcessDateTime = DateTime.Now;
            OrginType = "";
            UserID = 0;
            IsRead = false;
            ReadDatetime = DateTime.Now;
            OperationObjectID = 0;
            LinkReference = "";
            Parameter = "";
            SetupName = "";
            OperationName = "";
            OperationValue = "";
            HIASetupType = EnumHIASetupType.Manual;
            OrderStepID = 0;
			TimeEventType = EnumTimeEventType.ON_Date;
            TimeCounter = 0;
            ErrorMessage = "";
            View_Hight = 0;
            View_Wight = 0;
        }

        #region Properties
         
        public int HIAID { get; set; }
         
        public int HIASetupID { get; set; }
         
        public string MessageBodyText { get; set; }
         
        public DateTime ProcessDateTime { get; set; }
         
        public string OrginType { get; set; }
         
        public int UserID { get; set; }
         
        public bool IsRead { get; set; }

        public DateTime ReadDatetime { get; set; }

        public int OperationObjectID { get; set; }
        public int View_Hight { get; set; }
        public int View_Wight { get; set; }
        public string LinkReference { get; set; }
                  
        public string Parameter { get; set; }
         
        public string SetupName { get; set; }

        public string OperationName { get; set; }

        public string OperationValue { get; set; }
        public EnumHIASetupType HIASetupType { get; set; }
         public int OrderStepID {get;set;}
         public EnumTimeEventType TimeEventType  {get;set;}
         public int TimeCounter { get; set; }
        public string ErrorMessage { get; set; }


        #region Derived Property
        public string ProcessDateTimeInString
        {
            get
            {
                return this.ProcessDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string ReadDatetimeInString
        {
            get
            {
                return this.ReadDatetime.ToString("dd MMM yyyy HH:mm");
            }
        }       
        #endregion
        #endregion


        #region Functions
        public static List<HumanInteractionAgent> Gets(bool bIsAll, long nUserID)
        {
            return HumanInteractionAgent.Service.Gets(bIsAll, nUserID);
        }
        public HumanInteractionAgent Get(int id, long nUserID)
        {
            return HumanInteractionAgent.Service.Get(id, nUserID);
        }

        public HumanInteractionAgent Save(long nUserID)
        {
            return HumanInteractionAgent.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return HumanInteractionAgent.Service.Delete(id, nUserID);
        }

        public int GetHIA_NotificationCount(long nUserID)
        {
            return HumanInteractionAgent.Service.GetHIA_NotificationCount( nUserID);
        }

        public HumanInteractionAgent UpdateRead(long nUserID)
        {
            return HumanInteractionAgent.Service.UpdateRead(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IHumanInteractionAgentService Service
        {
            get { return (IHumanInteractionAgentService)Services.Factory.CreateService(typeof(IHumanInteractionAgentService)); }
        }
        #endregion
    }
    #endregion

    #region IHumanInteractionAgentinterface
     
    public interface IHumanInteractionAgentService
    {
         
        HumanInteractionAgent Get(int id, Int64 nUserID);       
         
        List<HumanInteractionAgent> Gets(bool bIsAll, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        int GetHIA_NotificationCount(Int64 nUserID);
         
        HumanInteractionAgent Save(HumanInteractionAgent oHumanInteractionAgent, Int64 nUserID);
         
        HumanInteractionAgent UpdateRead(HumanInteractionAgent oHumanInteractionAgent, Int64 nUserID);

    }
    #endregion
}
