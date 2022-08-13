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
    #region ChequeHistory
    
    public class ChequeHistory : BusinessObject
    {
        
        public ChequeHistory()
        {
            ChequeHistoryID = 0;
            ChequeID = 0;
            PreviousStatus = EnumChequeStatus.Initiate;
            CurrentStatus = EnumChequeStatus.Initiate;
            Note = "";
            ChangeLog = "";
            OperationBy = 0;
            OperationDateTime = DateTime.Now;

            OperationByName = "";
            
            ErrorMessage = "";
        }

        #region Properties
        
        public int ChequeHistoryID { get; set; }
        
        public int ChequeID { get; set; }
        
        public EnumChequeStatus PreviousStatus { get; set; }
        
        public EnumChequeStatus CurrentStatus { get; set; }
        
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
        public static List<ChequeHistory> Gets(int nCurrentUserID)
        {
            return ChequeHistory.Service.Gets(nCurrentUserID);
        }
        public ChequeHistory Get(int nChequeID, int nStatus, int nCurrentUserID)
        {
            return ChequeHistory.Service.Get(nChequeID, nStatus, nCurrentUserID);
        }
        public static List<ChequeHistory> Gets(int nChequeID, int nCurrentUserID)
        {
            return ChequeHistory.Service.Gets(nChequeID, nCurrentUserID);
        }
        public static List<ChequeHistory> Gets(string sSQL, int nCurrentUserID)
        {
            return ChequeHistory.Service.Gets(sSQL, nCurrentUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IChequeHistoryService Service
        {
            get { return (IChequeHistoryService)Services.Factory.CreateService(typeof(IChequeHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IChequeHistory interface
    
    public interface IChequeHistoryService
    {
        
        List<ChequeHistory> Gets(int nCurrentUserID);
        
        ChequeHistory Get(int nChequeID, int nStatus, int nCurrentUserID);
        
        List<ChequeHistory> Gets(int nChequeID, int nCurrentUserID);
        
        List<ChequeHistory> Gets(string sSQL, int nCurrentUserID);
    }
    #endregion
}