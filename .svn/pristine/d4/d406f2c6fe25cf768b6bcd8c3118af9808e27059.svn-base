using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class CandidateUserService : MarshalByRefObject, ICandidateUserService
    {
        #region Private functions and declaration
        private CandidateUser MapObject(NullHandler oReader)
        {
            CandidateUser oCandidateUser = new CandidateUser();
            oCandidateUser.UserID = oReader.GetInt32("UserID");
            oCandidateUser.LogInID = oReader.GetString("LogInID");
            oCandidateUser.UserName = oReader.GetString("UserName");
            oCandidateUser.Password = oReader.GetString("Password");
            oCandidateUser.CandidateID = oReader.GetInt32("CandidateID");
            //derive
            oCandidateUser.CandidateName = oReader.GetString("CandidateName");
            oCandidateUser.CandidateCode = oReader.GetString("CandidateCode");

            return oCandidateUser;

        }

        private CandidateUser CreateObject(NullHandler oReader)
        {
            CandidateUser oCandidateUser = MapObject(oReader);
            return oCandidateUser;
        }

        private List<CandidateUser> CreateObjects(IDataReader oReader)
        {
            List<CandidateUser> oCandidateUser = new List<CandidateUser>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CandidateUser oItem = CreateObject(oHandler);
                oCandidateUser.Add(oItem);
            }
            return oCandidateUser;
        }

        #endregion

        #region Interface implementation
        public CandidateUserService() { }

        public ObjectArryay CandidateUserLogin(string sLoginID, string sPassWord, Int64 nUserID)
        {
            ObjectArryay objReturn;
            CandidateUser oCandidateUser = null;
            TransactionContext tc = null;
            byte Operation = 0;
            int nTempUserID = 0;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = CandidateUserDA.GetForLogIn(tc,sLoginID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateUser = CreateObject(oReader);

                }

                //Check LoginUser Exist
                if (oCandidateUser == null)
                {
                    oCandidateUser = new CandidateUser();
                    oCandidateUser.ErrorMessage = "Invalid Login-ID, Enter valid Login-ID";
                }
                else
                {
                    nTempUserID = oCandidateUser.UserID;
                    Operation = 1;//for successful loging                    
                    //check is the password is valid                    
                    if (Global.Decrypt(oCandidateUser.Password) != sPassWord)
                    {
                        Operation = 3;//for wrong password                                                
                        oCandidateUser = new CandidateUser();
                        oCandidateUser.ErrorMessage = "Invalid password, try again...";
                    }
                    //if (oCandidateUser.UserID != 0 && oCandidateUser.CanLogin == false)
                    //{
                    //    Operation = 4;//restricted user                        
                    //    oUser = new User();
                    //    oUser.LoginMessage = "You are not authorised to Login the system";
                    //}
                    oCandidateUser.Password = "";
                }
                reader.Close();
                if (Operation != 0)
                {
                    //string sSQL = "INSERT INTO UserActionLog VALUES((SELECT ISNULL(MAX(UserActionLogID),0)+1 FROM UserActionLog)," +
                    //              nTempUserID + ",GETDATE()," + Operation.ToString() + ",'" + sLoginID + "','" + sPassWord + "','" + sBrowser + "','"+sIPAddress+"','"+sLogInLocation+"')";
                    string sSQL = "INSERT INTO UserActionLog VALUES((SELECT ISNULL(MAX(UserActionLogID),0)+1 FROM UserActionLog)," +
                                  nTempUserID + ",GETDATE()," + Operation.ToString() + ")";
                    RunSQLDA.RunSQL(tc, sSQL);
                }

                //if (oCandidateUser != null && oCandidateUser.UserID != 0)
                //{
                //    int[] PermissionKeys = null;

                //    int Count = 0;
                //    Count = UserDA.CountPermissionkey(tc, oUser.UserID);
                //    PermissionKeys = new int[Count];

                //    reader = UserDA.GetPermissionKeys(tc, oUser.UserID);
                //    oReader = new NullHandler(reader);
                //    Count = 0;
                //    while (reader.Read())
                //    {
                //        PermissionKeys[Count] = oReader.GetInt32("MenuID");
                //        Count = Count + 1;
                //    }
                //    reader.Close();
                //    oUser.Permissions = PermissionKeys;
                //}
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message, e);

                oCandidateUser.ErrorMessage = e.Message;
                #endregion
            }
            objReturn = new ObjectArryay(oCandidateUser, oCandidateUser.UserID);
            return objReturn;
        }

        public CandidateUser IUD(CandidateUser oCandidateUser, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CandidateUserDA.IUD(tc, oCandidateUser, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCandidateUser = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCandidateUser.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCandidateUser.UserID = 0;
                #endregion
            }
            return oCandidateUser;
        }


        public CandidateUser Get(int nBMMID, Int64 nUserId)
        {
            CandidateUser oCandidateUser = new CandidateUser();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateUserDA.Get(nBMMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateUser = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateUser", e);
                oCandidateUser.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateUser;
        }

        public CandidateUser Get(string sSql, Int64 nUserId)
        {
            CandidateUser oCandidateUser = new CandidateUser();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CandidateUserDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateUser = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get CandidateUser", e);
                oCandidateUser.ErrorMessage = e.Message;
                #endregion
            }

            return oCandidateUser;
        }

        public List<CandidateUser> Gets(Int64 nUserID)
        {
            List<CandidateUser> oCandidateUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateUserDA.Gets(tc);
                oCandidateUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateUser", e);
                #endregion
            }
            return oCandidateUser;
        }

        public List<CandidateUser> Gets(string sSQL, Int64 nUserID)
        {
            List<CandidateUser> oCandidateUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CandidateUserDA.Gets(sSQL, tc);
                oCandidateUser = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_CandidateUser", e);
                #endregion
            }
            return oCandidateUser;
        }
        public CandidateUser ChangePassword(CandidateUser oCandidateUser, Int64 nCandidateUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                oCandidateUser.Password = Global.Encrypt(oCandidateUser.Password);
                CandidateUserDA.ChangePassword(tc, oCandidateUser);

                IDataReader reader = CandidateUserDA.Get(oCandidateUser.UserID,tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCandidateUser = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CandidateUser", e);
                #endregion
            }

            return oCandidateUser;
        }
        #endregion
 
    }
}
