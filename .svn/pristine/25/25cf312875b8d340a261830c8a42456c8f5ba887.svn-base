using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region AccountingSession
    public class AccountingSession : BusinessObject
    {
        public AccountingSession()
        {
            AccountingSessionID=0;            
            SessionType= EnumSessionType.DayEnd;
            SessionCode="";
            SessionName="";
            YearStatus = EnumAccountYearStatus.Initialize;
            StartDate=DateTime.Now;
            EndDate=DateTime.Now;          
            ActivationDateTime = Convert.ToDateTime("1/1/2014 9:00:00 AM");
            LockDateTime = Convert.ToDateTime("1/1/2014 9:00:00 PM");
            ParentSessionID=0;
            SessionHierarchy="";
            WeekLyHolidays = "";            
            SessionID = 0;
            IsDateActivation = false;
            ErrorMessage = "";
            IsLock = false;
            BUID = 0;
        }

        #region Properties
        public int AccountingSessionID { get; set; }        
        public EnumSessionType SessionType { get; set; }
        public string SessionCode { get; set; }
        public string SessionName { get; set; }
        public EnumAccountYearStatus YearStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LockDateTime { get; set; }
        public DateTime ActivationDateTime { get; set; }
        public int ParentSessionID { get; set; }
        public string SessionHierarchy { get; set; }        
        public int SessionID { get; set; }        
        public bool IsDateActivation { get; set; }
        public string WeekLyHolidays { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsLock { get; set; }
        public int PreRunningSessionID { get; set; }        
        public bool IsOpeningTransfer { get; set; }        
        public Double AccountsToTransfer { get; set; }
        public int BUID { get; set; }
        #endregion

        #region Derived Property
        public IEnumerable<AccountingSession> ChildNodes { get; set; }
        public IEnumerable<AccountingSession> SessionList { get; set; }
        public AccountingSession Parent { get; set; }
        public string DisplayNode 
        {
            get 
            {
                if (this.SessionType == EnumSessionType.DayEnd)
                {
                    return SessionName + " (" + this.StartDate.ToString("dd MMM yyyy") + " @LockTime:" + this.LockDateTime.ToString("dd MMM yyyy HH:mm") + ")";
                }
                else
                {
                    return SessionName;                    
                }
                
            }
        }
        public int ParentID { get; set; }
        public string StartDateString
        {
            get { return this.StartDate.ToString("dd MMM yyyy"); }            
        }        
        public string EndDateTimeString 
        {
            get { return this.EndDate.ToString("dd MMM yyyy"); }
        }                
        public string LockDateTimeString
        {
            get { return this.LockDateTime.ToString("HH:mm"); }
        }
        public string LockDateInString
        {
            get { return this.LockDateTime.ToString("dd MMM yyyy HH:mm"); }
        }
        public string ActivationDateTimeString
        {
            get { return this.ActivationDateTime.ToString("HH:mm"); }            
        }         
        #endregion

        #region Functions
        public static List<AccountingSession> GetsTitleSessions(int nUserID)
        {
            return AccountingSession.Service.GetsTitleSessions(nUserID);
        }
        public static List<AccountingSession> Gets(int nUserID)
        {
            return AccountingSession.Service.Gets(nUserID);
        }
        public static List<AccountingSession> Gets(string sSQL, int nUserID)
        {
            return AccountingSession.Service.Gets(sSQL, nUserID);
        }
        public AccountingSession Get(int id, int nUserID)
        {
            return AccountingSession.Service.Get(id,nUserID);
        }
        public AccountingSession GetSessionByDate(DateTime dSessionDate, int nUserID)
        {
            return AccountingSession.Service.GetSessionByDate(dSessionDate, nUserID);
        }
        public AccountingSession Save(int nUserID)
        {
            return AccountingSession.Service.Save(this, nUserID);
        }
        public AccountingSession LockUnLock(int nUserID)
        {
            return AccountingSession.Service.LockUnLock(this,nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return AccountingSession.Service.Delete(id, nUserID);
        }        
        public AccountingSession AccountingYearClose(int nUserID)
        {
            return AccountingSession.Service.AccountingYearClose(this, nUserID);
        }
        public static AccountingSession GetRunningAccountingYear(int nUserID)
        {
            return AccountingSession.Service.GetRunningAccountingYear(nUserID);
        }
        public static AccountingSession GetOpenningAccountingYear(int nUserID)
        {
            return AccountingSession.Service.GetOpenningAccountingYear(nUserID);
        }
        public static List<AccountingSession> GetRunningFreezeAccountingYear(int nUserID)
        {
            return AccountingSession.Service.GetRunningFreezeAccountingYear(nUserID);
        }
        public static List<AccountingSession> GetsAccountingYears(int nUserID)
        {
            return AccountingSession.Service.GetsAccountingYears(nUserID);
        }
        public AccountingSession DeclareNewAccountingYear(int nUserID)
        {
            return AccountingSession.Service.DeclareNewAccountingYear(this, nUserID);
        }
        public static void TransferOpeningBalance(int nNewRunningSessionID, int nPreRunningSessionID, int nBusinessUnitID, int nAccountHeadID, int nSubledgerID, int nUserID)
        {
            AccountingSession.Service.TransferOpeningBalance(nNewRunningSessionID, nPreRunningSessionID, nBusinessUnitID, nAccountHeadID, nSubledgerID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAccountingSessionService Service
        {
            get { return (IAccountingSessionService)Services.Factory.CreateService(typeof(IAccountingSessionService)); }
        }
        #endregion
    }
    #endregion
    
    #region IAccountingSession interface
    public interface IAccountingSessionService
    {
        AccountingSession Get(int id, int nUserID);
        AccountingSession GetSessionByDate(DateTime dSessionDate, int nUserID);
        List<AccountingSession> GetsTitleSessions(int nUserID);
        List<AccountingSession> Gets(int nUserID);
        List<AccountingSession> Gets(string sSQL, int nUserID);
        string Delete(int id,int nUserID);
        AccountingSession Save(AccountingSession oAccountingSession, int nUserID);
        AccountingSession LockUnLock(AccountingSession oAccountingSession, int nUserID);        
        AccountingSession AccountingYearClose(AccountingSession oAccountingSession, int nUserID);
        AccountingSession GetRunningAccountingYear(int nUserID);
        AccountingSession GetOpenningAccountingYear(int nUserID);
        List<AccountingSession> GetsAccountingYears(int nUserID);
        List<AccountingSession> GetRunningFreezeAccountingYear(int nUserID);
        AccountingSession DeclareNewAccountingYear(AccountingSession oAccountingSession,int nUserID);
        void TransferOpeningBalance(int nNewRunningSessionID, int nPreRunningSessionID, int nBusinessUnitID, int nAccountHeadID, int nSubledgerID, int nUserID);
    }
    #endregion

    #region TAccountingSession
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TAccountingSession
    {
        public TAccountingSession()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            parentid = 0;
            SessionType = "";
            SessionCode="";            
            YearStatus = "";
            StartDate = "";
            EndDate = "";
            LockDateTime = "";
            LockDateInString = "";
            ActivationDateTime = "";            
            SessionHierarchy="";            
            ErrorMessage = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string SessionType  { get; set; }
        public string SessionCode { get; set; }        
        public string YearStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LockDateTime { get; set; }
        public string LockDateInString { get; set; }
        public string ActivationDateTime { get; set; }
        public string SessionHierarchy { get; set; }        
        public string ErrorMessage { get; set; }
        public IEnumerable<TAccountingSession> children { get; set; }//: an array nodes defines some children nodes        
        public List<TAccountingSession> TTAccountingSessions { get; set; }
    }
    #endregion
}
