using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region CandidateUser

    public class CandidateUser : BusinessObject
    {
        public CandidateUser()
        {
            UserID = 0;
            LogInID = "";
            UserName = "";
            Password = "";
            CandidateID = 0;
            CandidateName = "";
            CandidateCode = "";
            ErrorMessage = "";
            //wcfSessionid = Guid.Empty;

        }

        #region Properties

        public int UserID { get; set; }

        public string LogInID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Int64 CandidateID { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derive Property

        public string CandidateName { get; set; }

        public string CandidateCode { get; set; }
        #endregion
        //for WCF Server Session      
        public long nUserID { get; set; }

        #region Functions

        //public static CandidateUser CandidateUserLogin(long nUserID, string loginID, string password)
        //{
        //    try
        //    {
        //        CandidateUser oCandidateUser = new CandidateUser();
        //        //_oCurrentCandidateUser = (CandidateUser)ICSWCFServiceClient.CallMethod(ServiceType, "Login", loginID, password);
        //        //ObjectArryay tempReturn = ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "EYDLLogin", loginID, password, sBrowser, sIPAddress, sLogInLocation);
        //        ObjectArryay tempReturn = ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "CandidateUserLogin", loginID, password);
        //        oCandidateUser = (CandidateUser)tempReturn[0];
        //        oCandidateUser.wcfSessionid = (Guid)tempReturn[1];
        //        Global.CurrentSession = (Guid)tempReturn[1];
        //        if (oCandidateUser.UserID == 0)
        //        {
        //            throw new Exception(oCandidateUser.ErrorMessage);
        //        }
        //        return oCandidateUser;

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message, e);
        //    }
        //}

        //public static void LogOut(long nUserID)
        //{
        //    try
        //    {
        //        ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "LogOut");
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message, e);
        //    }
        //}


        public static CandidateUser Get(int id, long nUserID)
        {
            return CandidateUser.Service.Get(id, nUserID);
        }

        public static CandidateUser Get(string sSQL, long nUserID)
        {
            return CandidateUser.Service.Get(sSQL, nUserID);
        }

        public static List<CandidateUser> Gets(long nUserID)
        {
            return CandidateUser.Service.Gets(nUserID);
        }

        public static List<CandidateUser> Gets(string sSQL, long nUserID)
        {
            return CandidateUser.Service.Gets(sSQL, nUserID);
        }

        public CandidateUser IUD(int nDBOperation, long nUserID)
        {
            return CandidateUser.Service.IUD(this, nDBOperation, nUserID);
        }

        //public CandidateUser ChangePassword(long nUserID)
        //{
        //    return CandidateUser.Service.ChangePassword(this, nUserID);
        //}

        #endregion

        #region ServiceFactory
        internal static ICandidateUserService Service
        {
            get { return (ICandidateUserService)Services.Factory.CreateService(typeof(ICandidateUserService)); }
        }

        #endregion
    }
    #endregion


    #region ICandidateUser interface

    public interface ICandidateUserService
    {
        ObjectArryay CandidateUserLogin(string sLoginID, string sPassWord, Int64 nUserID);

        CandidateUser Get(int id, Int64 nUserID);

        CandidateUser Get(string sSQL, Int64 nUserID);

        List<CandidateUser> Gets(Int64 nUserID);

        List<CandidateUser> Gets(string sSQL, Int64 nUserID);

        CandidateUser IUD(CandidateUser oCandidateUser, int nDBOperation, Int64 nUserID);


    }
    #endregion

}