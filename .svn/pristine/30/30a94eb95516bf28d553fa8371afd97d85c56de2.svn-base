using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
namespace ESimSol.BusinessObjects
{

    #region UserWiseContractorConfigure
    
    public class UserWiseContractorConfigure : BusinessObject
    {
        public UserWiseContractorConfigure()
        {
            UserWiseContractorConfigureID = 0;
            UserID = 0;
            ContractorName = "";
            UserName = "";
            ContractorID = 0;
            StyleTypeIDs = "";
            UserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
            UserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int UserWiseContractorConfigureID { get; set; }
         
        public int UserID { get; set; }
        public string StyleTypeIDs { get; set; }//Extra
         
        public string UserName { get; set; }
         
        public string ContractorName { get; set; }
         
        public int ContractorID { get; set; }
         
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string StyleTypeInString
        {
            get;
            set;
        }
         
        public List<UserWiseContractorConfigure> UserWiseContractorConfigures { get; set; }
        public List<UserWiseContractorConfigureDetail> UserWiseContractorConfigureDetails { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<UserWiseContractorConfigure> Gets(long nUserID)
        {
            return UserWiseContractorConfigure.Service.Gets( nUserID);
        }
        public static List<UserWiseContractorConfigure> GetsByUser(int id, long nUserID)
        {
            return UserWiseContractorConfigure.Service.GetsByUser(id, nUserID);
        }
        public static List<UserWiseContractorConfigure> Gets(string sSQL, long nUserID)
        {
            
            return UserWiseContractorConfigure.Service.Gets(sSQL, nUserID);

        }
        public UserWiseContractorConfigure Get(int id, long nUserID)
        {
            return UserWiseContractorConfigure.Service.Get(id, nUserID);
        }

        public string Save(long nUserID)
        {
            return UserWiseContractorConfigure.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return UserWiseContractorConfigure.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFloor

     
        internal static IUserWiseContractorConfigureService Service
        {
            get { return (IUserWiseContractorConfigureService)Services.Factory.CreateService(typeof(IUserWiseContractorConfigureService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class UserWiseContractorConfigureList : List<UserWiseContractorConfigure>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IUserWiseContractorConfigure interface
     
    public interface IUserWiseContractorConfigureService
    {
         
        UserWiseContractorConfigure Get(int id, Int64 nUserID);
         
        List<UserWiseContractorConfigure> Gets(Int64 nUserID);
         
        List<UserWiseContractorConfigure> GetsByUser(int id, Int64 nUserID);
         
        List<UserWiseContractorConfigure> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        string Save(UserWiseContractorConfigure oUserWiseContractorConfigure, Int64 nUserID);

    }
    #endregion

}
