using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class UserService : MarshalByRefObject, IUserService
    {
        #region Private functions and declaration
        private User MapObject(NullHandler oReader)
        {
            User oUser = new User();
            oUser.UserID = oReader.GetInt32("UserID");
            oUser.LogInID = oReader.GetString("LogInID");
            oUser.UserName = oReader.GetString("UserName");
            oUser.Password = Global.Decrypt(oReader.GetString("Password"));
            oUser.Password = oReader.GetString("Password");
            oUser.OwnerID = oReader.GetInt32("OwnerID");
            oUser.LoggedOn = oReader.GetBoolean("LoggedOn");
            oUser.LoggedOnMachine = oReader.GetString("LoggedOnMachine");
            oUser.CanLogin = oReader.GetBoolean("CanLogin");
            oUser.DomainUserName = oReader.GetString("DomainUserName");
            oUser.EmployeeID = oReader.GetInt32("EmployeeID");
            oUser.LocationID = oReader.GetInt32("LocationID");
            oUser.AccountHolderType = (EnumAccountHolderType)oReader.GetInt32("AccountHolderType");
            oUser.AccountHolderTypeInInt = oReader.GetInt32("AccountHolderType");
            oUser.EmailAddress = oReader.GetString("EmailAddress");
            oUser.FinancialUserType = (EnumFinancialUserType)oReader.GetInt32("FinancialUserType");
            oUser.FinancialUserTypeInt = oReader.GetInt32("FinancialUserType");
            oUser.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oUser.LocationName = oReader.GetString("LocationName");            
            oUser.ContractorID = oReader.GetInt32("ContractorID");
            oUser.EmployeeType = (EnumEmployeeDesignationType)oReader.GetInt32("EmployeeType");
            oUser.ContractorType = (EnumContractorType)oReader.GetInt32("ContractorType");
            oUser.CompanyID = oReader.GetInt32("CompanyID");
            oUser.IsLocationBindded = oReader.GetBoolean("IsLocationBindded");
            oUser.IsShowLedgerBalance = oReader.GetBoolean("IsShowLedgerBalance");
            return oUser;
        }

        private UserActionLogCount MapObjectLog(NullHandler oReader)
        {
            UserActionLogCount oUserActionLogCount = new UserActionLogCount();
            oUserActionLogCount.LogIn = oReader.GetInt32("LogIn");
            oUserActionLogCount.LogOut = oReader.GetInt32("LogOut");
            oUserActionLogCount.WrongPass = oReader.GetInt32("WrongPass");
            return oUserActionLogCount;
        }

        private User CreateObject(NullHandler oReader)
        {
            User oUser = new User();
            oUser = MapObject(oReader);
            return oUser;
        }

        private UserActionLogCount CreateObjectLog(NullHandler oReader)
        {
            UserActionLogCount oUserActionLog = MapObjectLog(oReader);
            return oUserActionLog;
        }

        private List<User> CreateObjects(IDataReader oReader)
        {
            List<User> oUser = new List<User>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                User oItem = CreateObject(oHandler);
                oUser.Add(oItem);
            }
            return oUser;
        }

        private List<UserActionLogCount> CreateObjectsLog(IDataReader oReader)
        {
            List<UserActionLogCount> oUserActionLog = new List<UserActionLogCount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UserActionLogCount oItem = CreateObjectLog(oHandler);
                oUserActionLog.Add(oItem);
            }
            return oUserActionLog;
        }

        #endregion

        #region Interface implementation
        public UserService() { }

        public List<UserActionLogCount> GetUserActionLogs(string sSQL, Int64 nUserID)
        {
            List<UserActionLogCount> oUserActionLog = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.GetUserActionLogs(tc, sSQL);
                oUserActionLog = CreateObjectsLog(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserActionLog", e);
                #endregion
            }
            return oUserActionLog;
        }

        public User Save(User oUser, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oUser.Password = Global.Encrypt(oUser.Password);
                if (oUser.UserID <= 0)
                {
                    reader = UserDA.InsertUpdate(tc, oUser, EnumDBOperation.Insert);
                }
                else
                {
                    reader = UserDA.InsertUpdate(tc, oUser, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = new User();
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUser = new User();
                ExceptionLog.Write(e);
                oUser.ErrorMessage = e.Message.Split('!')[0];
                //throw new ServiceException("Failed to Save User. Because of " + e.Message, e);
                #endregion
            }
            return oUser;
        }

        public User SaveFromDesktop(User oUser, string sSelectedMenuKeys, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oUser.Password = Global.Encrypt(oUser.Password);
                if (oUser.UserID <= 0)
                {
                    reader = UserDA.InsertUpdate(tc, oUser, EnumDBOperation.Insert);
                }
                else
                {
                    reader = UserDA.InsertUpdate(tc, oUser, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = new User();
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                UserDA.ConfirmMenuPermission(tc, oUser.UserID, sSelectedMenuKeys, nUserId, (int)EnumApplicationType.DesktopApplication);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save User. Because of " + e.Message, e);
                #endregion
            }
            return oUser;
        }

        public User ChangePassword(User oUser, Int64 nUserId)
        {            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                oUser.Password = Global.Encrypt(oUser.Password);
                UserDA.ChangePassword(tc, oUser);

                IDataReader reader = UserDA.Get(tc, oUser.UserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = CreateObject(oReader);
                }
                reader.Close();                              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }

        public bool Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                User oUser = new User();
                oUser.UserID = id;
                DBTableReferenceDA.HasReference(tc, "Users", id); //reference check during delete. add by fahim0abir:21/06/15
                UserDA.Delete(tc, oUser, EnumDBOperation.Delete);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete User. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public bool ConfirmMenuPermission(int nUserID, string sSelectedMenuKeys, int nApplicationType, Int64 nCurrentUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                UserDA.ConfirmMenuPermission(tc, nUserID, sSelectedMenuKeys, nCurrentUserId, nApplicationType);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to User Menu configure. Because of " + e.Message, e);
                #endregion
            }
            return true;
        }

        public User Get(int id, Int64 nUserId)
        {
            User oUser=new User();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = CreateObject(oReader);
                }
                reader.Close();

                if (oUser.UserID <= 0) { return oUser; }

                #region GetMenuPermissions
                int[] PermissionKeys = null;

                int Count = 0;
                Count = UserDA.CountPermissionkey(tc, oUser.UserID);
                PermissionKeys = new int[Count];

                reader = UserDA.GetPermissionKeys(tc, oUser.UserID);
                oReader = new NullHandler(reader);
                Count = 0;
                while (reader.Read())
                {
                    PermissionKeys[Count] = oReader.GetInt32("MenuID");
                    Count = Count + 1;
                }
                reader.Close();
                oUser.Permissions = PermissionKeys;

                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }

        public List<User> Gets(Int64 nUserId)
        {
            List<User> oUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.Gets(tc, nUserId);
                oUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }

        public List<User> Gets(string sSQL, Int64 nUserId)
        {
            List<User> oUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.Gets(tc, sSQL, nUserId);
                oUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }
        /// <summary>
        /// added by fahim0abir on date : 13 Jul 2015
        /// used to get users including current user
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="sSQL"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public List<User> GetsForProductionExecution(string sSQL, Int64 nUserId)
        {
            List<User> oUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.GetsForProductionExecution(tc, sSQL, nUserId);
                oUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }
        public User UpdateCanlogin(int id, bool canLogin, Int64 nUserID)
        {
            User oUser = new User();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserDA.UpdateCanLogin(tc, id, canLogin);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oUser = new User();
                oUser.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oUser;
        }
        
        public List<User> GetsByLogInID(string sLogInID, Int64 nUserId)
        {
            List<User> oUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.GetsByLogInID(tc, sLogInID, nUserId);
                oUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }

        public User EYDLLogin(string sLoginID, string sPassWord, string sBrowser, string sIPAddress, string sLogInLocation, Int64 nUserID)
        {
            User oUser = new User();
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;

                #region Get User By Login ID
                reader = UserDA.Get(tc, sLoginID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = CreateObject(oReader);
                    
                }
                reader.Close();
                #endregion
                if (!oUser.CanLogin)
                {
                    throw new Exception("Sorry!!! This user is currently inactive.");
                }
                if (oUser == null || (sLoginID.Trim().ToLower() != oUser.LogInID.Trim().ToLower()) || Global.Decrypt(oUser.Password) != sPassWord)
                {
                    throw new Exception("Invalid Login-ID or Password");
                }
                else
                {
                    #region When user is bindded to the location
                    if (oUser.IsLocationBindded)
                    {

                        //IP address tracking for location binding
                        LB_Location oLB_Location = new LB_Location();
                        oLB_Location.LB_IPV4 = sIPAddress.Trim();
                        oLB_Location.LB_FirstHitDate = DateTime.Now;
                        oLB_Location.LB_FirstHitBy = oUser.UserID;

                        bool hasKnownLocation = LB_LocationDA.HasLocation(tc, oLB_Location.LB_IPV4); ;

                        if (!hasKnownLocation)
                        {
                            reader = LB_LocationDA.IUD(tc, oLB_Location, (short)EnumDBOperation.Insert);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            tc.End();
                            throw new Exception("Cann't login from unknown location.\nA request has been sent to the administrator to varify your location. " );
                        }


                        bool hasLocationPermission = LB_UserLocationMapDA.HasLocationBind(tc, oLB_Location.LB_IPV4, oUser.UserID);
                        if (!hasLocationPermission)
                            throw new Exception("You aren't permitted to login from this location.");
                            
                    }
                    #endregion

                    #region Permission
                    if (tc != null)
                    {
                        int[] PermissionKeys = null;

                        int Count = 0;
                        Count = UserDA.CountPermissionkey(tc, oUser.UserID);
                        PermissionKeys = new int[Count];

                        reader = UserDA.GetPermissionKeys(tc, oUser.UserID);
                        oReader = new NullHandler(reader);
                        Count = 0;
                        while (reader.Read())
                        {
                            PermissionKeys[Count] = oReader.GetInt32("MenuID");
                            Count = Count + 1;
                        }
                        reader.Close();
                        oUser.Permissions = PermissionKeys;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUser = new User();
                oUser.LoginMessage = e.Message;
                #endregion
            }
            return oUser;
        }

        public void LogOut(Int64 nUserId)
        {

            TransactionContext tc = TransactionContext.Begin(true);
            //string sSQL = "INSERT INTO UserActionLog VALUES((SELECT ISNULL(MAX(UserActionLogID),0)+1 FROM UserActionLog)," + nUserId + ",GETDATE(),2,'" + sLoginID + "','" + sPassWord + "','" + sBrowser + "','" + sIPAddress + "','" + sLogInLocation + "')";
            string sSQL = "INSERT INTO UserActionLog VALUES((SELECT ISNULL(MAX(UserActionLogID),0)+1 FROM UserActionLog)," + nUserId + ",GETDATE(),2)";
            RunSQLDA.RunSQL(tc, sSQL);
            tc.End();
            Global.CurrentSession = Guid.Empty;
            //throw new NotImplementedException();
        }

        public List<User> GetsBySql(string sSQL, Int64 nUserId)
        {
            List<User> oUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserDA.GetsBySql(tc, sSQL, nUserId);
                oUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }
        #region Buyer as user
        public User SaveUserAsBuyer(User oUser, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oUser.Password = Global.Encrypt(oUser.Password);
                //Insert or Update decision is taken at DB SP. So user can send EnumDBOperation.Insert or EnumDBOperation.Update any one
                reader = UserDA.InsertUpdateBuyerAsUser(tc, oUser, EnumDBOperation.Insert);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = new User();
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save User. Because of " + e.Message, e);
                #endregion
            }
            return oUser;
        }
        public User GetByBuyerID(int id, Int64 nUserId)
        {
            User oUser = new User();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserDA.GetByBuyerID(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = CreateObject(oReader);
                }
                reader.Close();

                if (oUser.UserID <= 0) { return oUser; }

                #region GetMenuPermissions
                int[] PermissionKeys = null;

                int Count = 0;
                Count = UserDA.CountPermissionkey(tc, oUser.UserID);
                PermissionKeys = new int[Count];

                reader = UserDA.GetPermissionKeys(tc, oUser.UserID);
                oReader = new NullHandler(reader);
                Count = 0;
                while (reader.Read())
                {
                    PermissionKeys[Count] = oReader.GetInt32("MenuID");
                    Count = Count + 1;
                }
                reader.Close();
                oUser.Permissions = PermissionKeys;

                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oUser;
        }
        #endregion


        public User GetByLogInID(string sLogInID, Int64 nUserId)
        {
            User oUser = new User();
            List<User> oUsers = new List<User>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = UserDA.GetByLogInID(tc, sLogInID);
                oUsers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }
            if (oUsers.Count > 0) { oUser = oUsers[0]; }
            return oUser;
        }

        public User ToggleLocationBindded(User oUser, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oUser.Password = Global.Encrypt(oUser.Password);
                reader = UserDA.InsertUpdate(tc, oUser, EnumDBOperation.Approval);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = new User();
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUser = new User();
                ExceptionLog.Write(e);
                oUser.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oUser;
        }

        public User ToggleShowLedgerBalance(User oUser, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                UserDA.ToggleShowLedgerBalance(tc, oUser);
                IDataReader reader;
                reader = UserDA.Get(tc, oUser.UserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUser = new User();
                    oUser = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUser = new User();
                ExceptionLog.Write(e);
                oUser.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oUser;
        }
        #endregion
    }
}