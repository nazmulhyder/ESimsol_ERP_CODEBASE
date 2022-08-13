using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ReceivedChequeHistory
    
    public class ReceivedChequeHistory : BusinessObject
    {
        
        public ReceivedChequeHistory()
        {
            ReceivedChequeHistoryID = 0;
            ReceivedChequeID = 0;
            PreviousStatus = EnumReceivedChequeStatus.Initiate;
            CurrentStatus = EnumReceivedChequeStatus.Initiate;
            Note = "";
            ChangeLog = "";
            OperationBy = 0;
            OperationDateTime = DateTime.Now;

            OperationByName = "";
            
            ErrorMessage = "";
        }

        #region Properties
        
        public int ReceivedChequeHistoryID { get; set; }
        
        public int ReceivedChequeID { get; set; }
        
        public EnumReceivedChequeStatus PreviousStatus { get; set; }
        
        public EnumReceivedChequeStatus CurrentStatus { get; set; }
        
        public string Note { get; set; }
        
        public string ChangeLog { get; set; }
        
        public int OperationBy { get; set; }
        
        public DateTime OperationDateTime { get; set; }

        
        public string OperationByName { get; set; }
        
        
        public string PrintSetupName { get; set; }
        
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derive Property
        public string CurrentStatusInString { get { return EnumObject.jGet(this.CurrentStatus); } }
        public string PreviousStatusInString { get { return EnumObject.jGet(this.PreviousStatus); } }
        public string OperationDateTimeInString { get { return this.OperationDateTime == DateTime.MinValue ? "" : this.OperationDateTime.ToString("dd MMM yyyy hh:mm tt"); } }
        #endregion

        #region Functions
        public static List<ReceivedChequeHistory> Gets(int nCurrentUserID)
        {
            return ReceivedChequeHistory.Service.Gets(nCurrentUserID);
        }
        public ReceivedChequeHistory Get(int nReceivedChequeID, int nStatus, int nCurrentUserID)
        {
            return ReceivedChequeHistory.Service.Get(nReceivedChequeID, nStatus, nCurrentUserID);
        }
        public static List<ReceivedChequeHistory> Gets(int nReceivedChequeID, int nCurrentUserID)
        {
            return ReceivedChequeHistory.Service.Gets(nReceivedChequeID, nCurrentUserID);
        }
        public static List<ReceivedChequeHistory> Gets(string sSQL, int nCurrentUserID)
        {
            return ReceivedChequeHistory.Service.Gets(sSQL, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IReceivedChequeHistoryService Service
        {
            get { return (IReceivedChequeHistoryService)Services.Factory.CreateService(typeof(IReceivedChequeHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IReceivedChequeHistory interface
    
    public interface IReceivedChequeHistoryService
    {
        
        List<ReceivedChequeHistory> Gets(int nCurrentUserID);
        
        ReceivedChequeHistory Get(int nReceivedChequeID, int nStatus, int nCurrentUserID);
        
        List<ReceivedChequeHistory> Gets(int nReceivedChequeID, int nCurrentUserID);
        
        List<ReceivedChequeHistory> Gets(string sSQL, int nCurrentUserID);
    }
    #endregion
}