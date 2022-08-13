using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region User
    
    public class User : BusinessObject
    {
        public User()
        {
            UserID = 0;
            LogInID = "";
            UserName = "";
            Password = "";
            ConfirmPassword = "";
            OwnerID = 0;
            LoggedOn = false;
            LoggedOnMachine = "";
            CanLogin = false;
            DomainUserName = "";
            EmployeeID = 0;
            LocationID = 0;
            AccountHolderType = EnumAccountHolderType.None;
            AccountHolderTypeInInt = 0;
            EmailAddress = "";
            FinancialUserType = EnumFinancialUserType.None;
            FinancialUserTypeInt = 0;
            LoginMessage = "";
            EmployeeNameCode = "";
            LocationName = "";            
            PasswordChanged = false;
            ErrorMessage = "";
            Menu = new TMenu();
            BrowserName="";
            IPAddress="";
            LogInLocation = "";
            IsLocationBindded = false;
            IsShowLedgerBalance = false;
        }

        #region Properties
        public int UserID { get; set; }        
        public string LogInID { get; set; }        
        public string UserName { get; set; }        
        public string Password { get; set; }        
        public string ConfirmPassword { get; set; }        
        public int OwnerID { get; set; }        
        public bool LoggedOn { get; set; }        
        public string LoggedOnMachine { get; set; }        
        public bool CanLogin { get; set; }        
        public string DomainUserName { get; set; }        
        public Int64 EmployeeID { get; set; }        
        public int LocationID { get; set; }        
        public EnumAccountHolderType AccountHolderType { get; set; }
        public int AccountHolderTypeInInt { get; set; }
        public string EmailAddress { get; set; }
        public EnumFinancialUserType FinancialUserType { get; set; }
        public int FinancialUserTypeInt { get; set; }
        public string LoginMessage { get; set; }        
        public string EmployeeNameCode { get; set; }        
        public string LocationName { get; set; }
        public bool PasswordChanged { get; set; }
        public string ErrorMessage { get; set; }        
        public int ContractorID { get; set; }        
        public EnumContractorType ContractorType { get; set; }        
        public EnumEmployeeDesignationType EmployeeType { get; set; }        
        public int CompanyID { get; set; }
        public bool IsLocationBindded { get; set; }
        public bool IsShowLedgerBalance { get; set; }
        #endregion

        #region Derive Property
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TimesOfLogIn { get; set; }
        public int TimesOfLogOut { get; set; }
        public int TimesOfWrongAttempts { get; set; }
        public string Activity
        {
            get
            {
                if (CanLogin)
                {
                    return "Active";
                }
                else
                {
                    return "Inactive";
                }
            }
        }
        public string IsShowLedgerBalanceSt
        {
            get
            {
                if (IsShowLedgerBalance)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string FinancialUserTypeSt
        {
            get
            {
                return EnumObject.jGet(this.FinancialUserType);
            }
        }

        #region Is SuperUser
        public bool IsSuperUser
        {
            get { return (UserID == -9); }
        }
   
        #endregion

        #region Permission Keys
        private int[] _sKeys;
        
        public int[] Permissions
        {
            get { return _sKeys; }
            set { _sKeys = value; }
        }

        public bool IsPermitted(int permissionKey)
        {
            if (IsSuperUser)
            {
                return true;
            }
            else
            {
                if (this.Permissions == null) { return false; }
                foreach (int Key in this.Permissions)
                {
                    if (Key == permissionKey)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
 
        #endregion

        public TMenu Menu { get; set; }
        public string Keys { get; set; }        
        #endregion

        #region Properties For Action Log // Security Purpose
        public string BrowserName { get; set; }
        public string IPAddress { get; set; }        
        public string LogInLocation { get; set; }
        #endregion

        #region Functions
        public static User EYDLLogin(long nUserID, string sLoginID, string sPassWord, string sBrowser, string sIPAddress, string sLogInLocation)
        {
            return User.Service.EYDLLogin(sLoginID, sPassWord, sBrowser, sIPAddress, sLogInLocation, nUserID);
        }
        public static void LogOut(long nUserID)
        {
            try { User.Service.LogOut(nUserID); }
            catch (Exception e){ throw new Exception(e.Message, e); }
        }
        public static string GetLastLoginID()
        {
            string sLogin = "";
            string sFileSpec = Path.Combine(Environment.CurrentDirectory, "LastLogin.config");

            StreamReader oReader = new StreamReader(sFileSpec);
            sLogin = oReader.ReadToEnd();
            oReader.Close();

            return sLogin;
        }
        public static void SaveLastLoginID(string sLogin)
        {
            string sFileSpec = Path.Combine(Environment.CurrentDirectory, "LastLogin.config");

            StreamWriter oWriter = new StreamWriter(sFileSpec);
            oWriter.Write(sLogin);
            oWriter.Close();
        }
        public static List<User> Gets(long nUserID)
        {
            return User.Service.Gets(nUserID); 
        }
        public static List<User> Gets(string sSQL, long nUserID)
        {
            return User.Service.Gets(sSQL,nUserID); 
        }
        /// <summary>
        /// added by fahim0abir on date : 13 Jul 2015
        /// used to get users including current user
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="sSQL"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static List<User> GetsForProductionExecution(string sSQL, long nUserID)
        {
            return User.Service.GetsForProductionExecution(sSQL, nUserID);
        }
        public static List<User> GetsByLogInID(string sLogInID, long nUserID)
        {
            return User.Service.GetsByLogInID(sLogInID ,nUserID); 
        }
        public User Get(int id, long nUserID)
        {
            return User.Service.Get(id, nUserID); 
        }
        public User ChangePassword(long nUserID)
        {
            return User.Service.ChangePassword(this, nUserID); 

        }
        public User Save(long nUserID)
        {
            return User.Service.Save(this, nUserID);
        }
        public User SaveFromDesktop(string sSelectedMenuKeys,  long nUserID)
        {
            return User.Service.SaveFromDesktop(this, sSelectedMenuKeys, nUserID);
        }
        public bool Delete(int id, long nUserID)
        {
            return User.Service.Delete(id, nUserID);
        }
        public bool ConfirmMenuPermission(int nUID, string sSelectedMenuKeys, int nApplicationType, long nUserID)
        {
            return User.Service.ConfirmMenuPermission(nUID, sSelectedMenuKeys, nApplicationType, nUserID);
        }
        public User UpdateCanlogin(bool canLogin, long nUserID)
        {
            return User.Service.UpdateCanlogin(this.UserID, canLogin, nUserID);
        }
        public bool IsValidPassword(string newPassword)
        {
            bool bPassChange = false;
            bPassChange = !(Global.Decrypt(this.Password) == newPassword || Password == newPassword);
            return !bPassChange;
        }
        public static List<User> GetsBySql(string sSQL, long nUserID)
        {
            return User.Service.GetsBySql(sSQL, nUserID);
        }
        public User GetByLogInID(string sLogInID, long nUserID)
        {
            return User.Service.GetByLogInID(sLogInID, nUserID);
        }

        #region UserActionLog
        public static List<UserActionLogCount> GetUserActionLogs(string sSQL, long nUserID)
        {
            return User.Service.GetUserActionLogs(sSQL, nUserID);
        }
        #endregion

        #region UserAsBuyer
        public User SaveUserAsBuyer(long nUserID)
        {
            return User.Service.SaveUserAsBuyer(this, nUserID);
        }

        public User GetByBuyerID(int id, long nUserID)
        {
            return User.Service.GetByBuyerID(id, nUserID);
        }
        #endregion
        public User ToggleLocationBindded(long nUserID)
        {
            return User.Service.ToggleLocationBindded(this, nUserID);
        }
        public User ToggleShowLedgerBalance(long nUserID)
        {
            return User.Service.ToggleShowLedgerBalance(this, nUserID);
        }
     
        #endregion

        #region NonDB Members
        public bool HasFunctionality(EnumOperationFunctionality eOperationFunctionality,string sObjectName)
        {

            //foreach (AuthorizationUserOEDO item in this.AuthorizationUserOEDOs)
            //{
            //    if (item.OEValue == eOperationFunctionality && item.DBObjectName == sObjectName)
            //        return true;
            //}

            if (((User)Global.CurrentUser).IsSuperUser)
            {
                return true;
            }
            return false;
        }
        public bool HasFunctionality(EnumOperationFunctionality eOperationFunctionality, string sObjectName,int nWorkingUnitID)
        {

            //foreach (AuthorizationUserOEDO item in this.AuthorizationUserOEDOs)
            //{
            //    if (item.OEValue == eOperationFunctionality && item.DBObjectName == sObjectName && item.WorkingUnitID == nWorkingUnitID)
            //        return true;
            //}

            if (((User)Global.CurrentUser).IsSuperUser)
            {
                return true;
            }
            return false;
        }
        
        #endregion

        #region NonDB Members for web
        public bool HasFunctionalityWeb(EnumOperationFunctionality eOperationFunctionality, string sObjectName, List<AuthorizationUserOEDO> oAUOEDOs)
        {

            foreach (AuthorizationUserOEDO item in oAUOEDOs)
            {
                if (item.OEValue == eOperationFunctionality && item.DBObjectName == sObjectName)
                    return true;
            }
            return false;
        }
        public bool HasFunctionalityWeb(EnumOperationFunctionality eOperationFunctionality, string sObjectName, int nWorkingUnitID, List<AuthorizationUserOEDO> oAUOEDOs)
        {

            foreach (AuthorizationUserOEDO item in oAUOEDOs)
            {
                if (item.OEValue == eOperationFunctionality && item.DBObjectName == sObjectName && item.WorkingUnitID == nWorkingUnitID)
                    return true;
            }
            return false;
        }

        #endregion

        #region ServiceFactory

        internal static IUserService Service
        {
            get { return (IUserService)Services.Factory.CreateService(typeof(IUserService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class UserList : List<User>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IUser interface
    
    public interface IUserService
    {   
        //User Login(string loginID, string sPassword, bool IsWebUser);
        ///from 'UserService' class it will return 'ObjectArryayAsObject' but from WCF service it will return 'User' so in BO we will return 'User'
        User EYDLLogin(string sLoginID, string sPassWord,string sBrowser, string sIPAddress, string sLogInLocation, long nUserID);
        void LogOut(long nUserID);
        User Get(int id, long nUserID);
        User ChangePassword(User oUser, long nUserID);
        List<User> Gets(long nUserID);
        List<User> Gets(string sSQL, long nUserID);
        List<User> GetsForProductionExecution(string sSQL, long nUserID);
        List<User> GetsByLogInID(string sLogInID, long nUserID);
        bool Delete(int id, long nUserID);
        User Save(User oUser, long nUserID);
        User SaveFromDesktop(User oUser, string sSelectedMenuKeys, long nUserID);
        bool ConfirmMenuPermission(int nUserID, string sSelectedMenuKeys, int nApplicationType, Int64 nDBUserID);
        User UpdateCanlogin(int nUserID, bool canLogin, Int64 nDBUserID);
        List<User> GetsBySql(string sSQL, long nUserID);
      
        #region UserActionLog
        
        List<UserActionLogCount> GetUserActionLogs(string sSQL, long nUserID);
        #endregion
     
        #region buyer as user
        
        User SaveUserAsBuyer(User oUser, long nUserID);
        
        User GetByBuyerID(int id, long nUserID);
        #endregion
        User GetByLogInID(string sLogInID, long nUserID);
        User ToggleLocationBindded(User oUser, long nUserID);
        User ToggleShowLedgerBalance(User oUser, long nUserID);
    }
    #endregion

}