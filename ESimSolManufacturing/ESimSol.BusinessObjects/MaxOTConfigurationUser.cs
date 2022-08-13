using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region MaxOTConfigurationUser
    public class MaxOTConfigurationUser
    {
        public MaxOTConfigurationUser()
        {
            MOCUID = 0;
            MOCID = 0;
            UserID = 0;
            IsShortList = false;
            IsUserBased = false;
            MaxOTConfigurationUsers = new List<MaxOTConfigurationUser>();
            ErrorMessage = "";
        }

        #region Properties
        public int MOCUID { get; set; }
        public int MOCID { get; set; }
        public int UserID { get; set; }
        public bool IsShortList { get; set; }
        public bool IsUserBased { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        public List<MaxOTConfigurationUser> MaxOTConfigurationUsers { get; set; }
        #region Functions


        public static List<MaxOTConfigurationUser> Gets(string sSQL, long nUserID)
        {
            return MaxOTConfigurationUser.Service.Gets(sSQL, nUserID);
        }
        public static MaxOTConfigurationUser Get(string sSQL, long nUserID)
        {
            return MaxOTConfigurationUser.Service.Get(sSQL, nUserID);
        }
        public string IUD(bool IsShortList, bool IsUserBased, long nUserID)
        {
            return MaxOTConfigurationUser.Service.IUD(this, IsShortList, IsUserBased, nUserID);
        }
        public static List<MaxOTConfigurationUser> GetsUser(long nUserID, int id)
        {
            return MaxOTConfigurationUser.Service.GetsUser(nUserID, id);
        }
 
        #endregion

        #region ServiceFactory

        internal static IMaxOTConfigurationUserService Service
        {
            get { return (IMaxOTConfigurationUserService)Services.Factory.CreateService(typeof(IMaxOTConfigurationUserService)); }
        }
        #endregion
    }
    #endregion

    #region IMaxOTConfigurationUser interface

    public interface IMaxOTConfigurationUserService
    {
        List<MaxOTConfigurationUser> GetsUser(long nUserID, int id);
        List<MaxOTConfigurationUser> Gets(string sSQL, Int64 nUserID);
        MaxOTConfigurationUser Get(string sSQL, Int64 nUserID);
        string IUD(MaxOTConfigurationUser oMaxOTConfigurationUser, bool IsShortList, bool IsUserBased, Int64 nUserID);
       
      
    }
    #endregion
}

