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
    #region User Wise Style Configure
    
    public class UserWiseStyleConfigure : BusinessObject
    {
        public UserWiseStyleConfigure()
        {
            UserWiseStyleConfigureID = 0;
            UserID = 0;
            UserName = "";
            TechnicalSheetID = 0;         
            StyleNo = "";
            BuyerName = "";
            BuyerID = 0;
            SessionName = "";
            BusinessSessionID = 0;
            GarmmentsProductName = "";
            DevelopmentStatus = EnumDevelopmentStatus.Initialize;
            YarnCategoryName = "";
            UserWiseStyleConfigures = new List<UserWiseStyleConfigure>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int UserWiseStyleConfigureID { get; set; }
         
        public int UserID { get; set; }
         
        public string UserName { get; set; }
         
        public string StyleNo { get; set; }
         
        public int TechnicalSheetID { get; set; }
         
        public int BuyerID { get; set; }
         
        public int BusinessSessionID { get; set; }
         
        public EnumDevelopmentStatus DevelopmentStatus { get; set; }
         
        public string BuyerName { get; set; }
         
        public string GarmmentsProductName { get; set; }

         
        public string YarnCategoryName { get; set; }
        
         
        public string SessionName { get; set; }


         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
         
        public List<UserWiseStyleConfigure> UserWiseStyleConfigures { get; set; }
         
        public List<Contractor> Contractors { get; set; }
        public Company Company { get; set; }

        public List<BusinessSession> BusinessSessions { get; set; }
        #endregion

        #region Functions

        public static List<UserWiseStyleConfigure> Gets(long nUserID)
        {
            
            return UserWiseStyleConfigure.Service.Gets( nUserID);
        }
        public static List<UserWiseStyleConfigure> GetsByUser(int id, long nUserID)
        {
            return UserWiseStyleConfigure.Service.GetsByUser(id, nUserID);
        }
        public static List<UserWiseStyleConfigure> Gets(string sSQL, long nUserID)
        {
            return UserWiseStyleConfigure.Service.Gets(sSQL, nUserID);
        }
        public UserWiseStyleConfigure Get(int id, long nUserID)
        {
            
            return UserWiseStyleConfigure.Service.Get(id, nUserID);
        }

        public UserWiseStyleConfigure Save(long nUserID)
        {
            
            return UserWiseStyleConfigure.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            
            return UserWiseStyleConfigure.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFloor

 
        internal static IUserWiseStyleConfigureService Service
        {
            get { return (IUserWiseStyleConfigureService)Services.Factory.CreateService(typeof(IUserWiseStyleConfigureService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class UserWiseStyleConfigureList : List<UserWiseStyleConfigure>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IUserWiseStyleConfigure interface
     
    public interface IUserWiseStyleConfigureService
    {
         
        UserWiseStyleConfigure Get(int id, Int64 nUserID);
         
        List<UserWiseStyleConfigure> Gets(Int64 nUserID);
         
        List<UserWiseStyleConfigure> GetsByUser(int id, Int64 nUserID);
         
        List<UserWiseStyleConfigure> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        UserWiseStyleConfigure Save(UserWiseStyleConfigure oUserWiseStyleConfigure, Int64 nUserID);

    }
    #endregion

    

}
