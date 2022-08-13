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
    #region UserWiseContractorConfigureDetail
    public class UserWiseContractorConfigureDetail : BusinessObject
    {
        public UserWiseContractorConfigureDetail()
        {
            UserWiseContractorConfigureDetailID = 0;
            UserWiseContractorConfigureID = 0;
            StyleType = EnumTSType.Sweater;
          
            ErrorMessage = "";
        }

        #region Properties

        public int UserWiseContractorConfigureDetailID { get; set; }

        public int UserWiseContractorConfigureID { get; set; }

        public EnumTSType StyleType { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string StyleTypeInString
        {
            get
            {
                return StyleType.ToString();
            }
        }

        #endregion

        #region Functions
        public static List<UserWiseContractorConfigureDetail> Gets(int UserWiseContractorConfigureID, long nUserID)
        {
            return UserWiseContractorConfigureDetail.Service.Gets(UserWiseContractorConfigureID, nUserID);
        }
        public static List<UserWiseContractorConfigureDetail> Gets(string sSQL, long nUserID)
        {
            return UserWiseContractorConfigureDetail.Service.Gets(sSQL, nUserID);
        }
        public UserWiseContractorConfigureDetail Get(int UserWiseContractorConfigureDetailID, long nUserID)
        {
            return UserWiseContractorConfigureDetail.Service.Get(UserWiseContractorConfigureDetailID, nUserID);
        }
             #endregion

        #region ServiceFactory

        internal static IUserWiseContractorConfigureDetailService Service
        {
            get { return (IUserWiseContractorConfigureDetailService)Services.Factory.CreateService(typeof(IUserWiseContractorConfigureDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IUserWiseContractorConfigureDetail interface

    public interface IUserWiseContractorConfigureDetailService
    {

        UserWiseContractorConfigureDetail Get(int UserWiseContractorConfigureDetailID, Int64 nUserID);

        List<UserWiseContractorConfigureDetail> Gets(int UserWiseContractorConfigureID, Int64 nUserID);

        List<UserWiseContractorConfigureDetail> Gets(string sSQL, Int64 nUserID);

      
    }
    #endregion
    
}
