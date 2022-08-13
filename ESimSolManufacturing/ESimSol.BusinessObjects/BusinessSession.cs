using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region BusinessSession
    
    public class BusinessSession : BusinessObject
    {
        public BusinessSession()
        {
            BusinessSessionID = 0;
            SessionName = "";
            IsActive = false;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int BusinessSessionID { get; set; }
         
        public string SessionName { get; set; }
         
        public bool IsActive { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string IsActiveInString
        {
            get
            {
                if (this.IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "In-Active";
                }
            }
        }
     


        #endregion

        #region Functions

        public static List<BusinessSession> Gets(long nUserID)
        {
            return BusinessSession.Service.Gets(nUserID);
        }
        public static List<BusinessSession> Gets(bool bIsActive, long nUserID)
        {
            return BusinessSession.Service.Gets(bIsActive, nUserID);
        }
        public BusinessSession Get(int id, long nUserID)
        {
            return BusinessSession.Service.Get(id, nUserID);
        }

        public BusinessSession Save(long nUserID)
        {
            return BusinessSession.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return BusinessSession.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IBusinessSessionService Service
        {
            get { return (IBusinessSessionService)Services.Factory.CreateService(typeof(IBusinessSessionService)); }
        }
        #endregion
    }
    #endregion

    #region IBusinessSession interface
     
    public interface IBusinessSessionService
    {
         
        BusinessSession Get(int id, Int64 nUserID);
         
        List<BusinessSession> Gets(Int64 nUserID);
         
        List<BusinessSession> Gets(bool bIsActive, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        BusinessSession Save(BusinessSession oBusinessSession, Int64 nUserID);
    }
    #endregion
}
