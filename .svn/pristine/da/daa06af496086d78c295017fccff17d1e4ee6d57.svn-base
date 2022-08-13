using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ExportBillHistory
    [DataContract]
    public class ExportBillHistory : BusinessObject
    {

        #region  Constructor
        public ExportBillHistory()
        {
            ExportBillHistoryID = 0;
            ExportBillID = 0;
            State = EnumLCBillEvent.BOEinHand;
            Note="";
            ErrorMsg = "";
            DBUserID = 0;
            DateTime = DateTime.Now;
        }
        #endregion

        #region Properties
        [DataMember]
        public int ExportBillHistoryID { get; set; }
        [DataMember]
        public int ExportBillID { get; set; }
        [DataMember]
        public EnumLCBillEvent State { get; set; }
        [DataMember]
        public EnumLCBillEvent PreviousState { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string NoteSystem { get; set; }
        [DataMember]
        public int DBUserID { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
        
        #endregion
        #region Derived Properties
        [DataMember]
        public string UserName { get; set; }

        public List<ExportBillHistory> ExportBillHistorys { get; set; }
        public Company Company { get; set; }

        public string DateTimeInString { get{return DateTime.ToString("dd MMM yyyy hh:mm tt");} }
        public string StateInString { get { return State.ToString(); } }
        public string PreviousStateInString { get { return PreviousState.ToString(); } }

       
         #endregion

        #region Functions
        public ExportBillHistory Get(int nId, Int64 nUserID)
        {
            return ExportBillHistory.Service.Get(nId, nUserID);
        }
        public ExportBillHistory Getby(int nId,int eEvent, Int64 nUserID)
        {
            return ExportBillHistory.Service.Getby(nId,eEvent, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportBillHistory.Service.Delete(this, nUserID);
        }
  
        
        #region  Collection Functions

        public static List<ExportBillHistory> Gets(int nExportBillID, Int64 nUserID)
        {
            return ExportBillHistory.Service.Gets(nExportBillID, nUserID);
        }
     

        #region Non DB Members
        public ExportBillHistory GetHistoryByEvent(EnumLCBillEvent eEvent, List<ExportBillHistory> oExportBillHistorys)
        {
            foreach (ExportBillHistory oitem in oExportBillHistorys)
            {
                if (oitem.State == eEvent) return oitem;
            }
            ExportBillHistory oReturn = new ExportBillHistory();
            oReturn.State = eEvent;
            return oReturn;
        }

        #endregion
        #endregion
        #endregion

        #region ServiceFactory

        internal static IExportBillHistoryService Service
        {
            get { return (IExportBillHistoryService)Services.Factory.CreateService(typeof(IExportBillHistoryService)); }
        }

        #endregion
    }
    #endregion

    

    #region IExportBillHistory interface
    [ServiceContract]
    public interface IExportBillHistoryService
    {
        ExportBillHistory Get(int id, Int64 nUserID);
        ExportBillHistory Getby(int nLCBillID, int eEvent, Int64 nUserID);
        List<ExportBillHistory> Gets(int nExportBillID, Int64 nUserID);
        string Delete(ExportBillHistory oExportBillHistory, Int64 nUserID);
        
    }
    #endregion
}
