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
    #region LocationBind
    public class LB_Location
    {
        public LB_Location()
        {
            LB_LocationID = 0;
            LB_IPV4 = string.Empty;
            LB_KnownName = string.Empty;
            LB_LocationNote = string.Empty;
            LB_Is_Classified = false;
            LB_ClassificationDate = DateTime.Now;
            LB_ClasifiedBy = 0;
            LB_FirstHitDate = DateTime.Now;
            ExpireDateTime = DateTime.MinValue;
            LB_FirstHitBy = 0;
        }

        #region Properties
        public int LB_LocationID { get; set; }
        public string LB_IPV4 { get; set; }
        public string LB_KnownName { get; set; }
        public string LB_LocationNote { get; set; }
        public bool LB_Is_Classified { get; set; }
        public DateTime LB_ClassificationDate { get; set; }
        public int LB_ClasifiedBy { get; set; }
        public DateTime LB_FirstHitDate { get; set; }
        public int LB_FirstHitBy { get; set; }
        public DateTime ExpireDateTime { get; set; }
        #endregion

        #region Derive Properties
        public string LB_Is_ClassifiedStr { get { return (this.LB_Is_Classified) ? "Yes" : "No"; } }
        public string LB_ClassificationDateStr { get { return (this.LB_ClassificationDate!=DateTime.MinValue)? this.LB_ClassificationDate.ToString("dd MMM yyyy hh:mm tt"):""; } }
        public int LB_ClasifiedByStr { get; set; }
        public string LB_FirstHitDateStr { get { return (this.LB_FirstHitDate != DateTime.MinValue) ? this.LB_FirstHitDate.ToString("dd MMM yyyy hh:mm tt") : ""; } }
        public int LB_FirstHitByStr { get; set; }
        public string ExpireDateTimeStr { get { return (this.ExpireDateTime != DateTime.MinValue) ? this.ExpireDateTime.ToString("dd MMM yyyy hh:mm tt") : ""; } }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions

        public static LB_Location Get(int nID, int nUserID)
        {
            return LB_Location.Service.Get(nID, nUserID);
        }
        public static List<LB_Location> Gets(string sSQL, int nUserID)
        {
            return LB_Location.Service.Gets(sSQL, nUserID);
        }
        public LB_Location IUD(short nDBOperation, int nUserID)
        {
            return LB_Location.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILB_LocationService Service
        {
            get { return (ILB_LocationService)Services.Factory.CreateService(typeof(ILB_LocationService)); }
        }
        #endregion
    }

    #region ILB_Location interface
    public interface ILB_LocationService
    {
        LB_Location Get(int nID, int nUserID);
        List<LB_Location> Gets(string sSQL, int nUserID);
        LB_Location IUD(LB_Location oLB_Location, short nDBOperation, int nUserID);
    }
    #endregion
    #endregion

    #region LB_UserLocationMap
    public class LB_UserLocationMap
    {
        public LB_UserLocationMap()
        {
            LB_UserLocationMapID = 0;
            LB_UserID = 0;
            LB_LB_LocationID = 0;
            LB_ExpireDateTime = DateTime.MinValue;
            ErrorMessage = string.Empty;
            LB_UserLocationMaps = new List<LB_UserLocationMap>();
            Params = string.Empty;
        }

        #region Properties
        public int LB_UserLocationMapID { get; set; }
        public int LB_UserID { get; set; }
        public int LB_LB_LocationID { get; set; }
        public DateTime LB_ExpireDateTime { get; set; }

        #endregion

        #region Derive Properties
        public bool HasMultiLocation { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public string LB_IPV4 { get; set; }
        public string LB_KnownName { get; set; }
        public string LogInID { get; set; }
        public string UserName { get; set; }
        public string LB_ExpireDateTimeStr { get { return (this.LB_ExpireDateTime != DateTime.MinValue) ? this.LB_ExpireDateTime.ToString("dd MMM yyyy hh:mm tt") : ""; } }
        public List<LB_UserLocationMap> LB_UserLocationMaps { get; set; }
        #endregion

        #region Functions
        public static LB_UserLocationMap Get(int nID, int nUserID)
        {
            return LB_UserLocationMap.Service.Get(nID, nUserID);
        }
        public static List<LB_UserLocationMap> Gets(string sSQL, int nUserID)
        {
            return LB_UserLocationMap.Service.Gets(sSQL, nUserID);
        }
        //public static List<LB_UserLocationMap> Gets(int nLB_LocationID, DateTime dStartDate, DateTime dEndDate, int nUserID)
        //{
        //    return LB_UserLocationMap.Service.Gets(nLB_LocationID, dStartDate, dEndDate, nUserID);
        //}
        public LB_UserLocationMap IUD(short nDBOperation, int nUserID)
        {
            return LB_UserLocationMap.Service.IUD(this, nDBOperation, nUserID);
        }
        public LB_UserLocationMap Save(int nUserID)
        {
            return LB_UserLocationMap.Service.Save(this,nUserID);
        }
        public string Delete(int nUserID)
        {
            return LB_UserLocationMap.Service.Delete(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static ILB_UserLocationMapService Service
        {
            get { return (ILB_UserLocationMapService)Services.Factory.CreateService(typeof(ILB_UserLocationMapService)); }
        }
        #endregion
    }

    #region ILB_Location interface
    public interface ILB_UserLocationMapService
    {
        LB_UserLocationMap Get(int nID, int nUserID);
        List<LB_UserLocationMap> Gets(string sSQL, int nUserID);
        //List<LB_UserLocationMap> Gets(int nAccountsBookSetupID, DateTime dStartDate, DateTime dEndDate, int nUserID);
        LB_UserLocationMap IUD(LB_UserLocationMap oLB_UserLocationMap, short nDBOperation, int nUserID);
        LB_UserLocationMap Save(LB_UserLocationMap oLB_UserLocationMap, int nUserID);
        string Delete(LB_UserLocationMap oLB_UserLocationMap, int nUserID);
       
    }
    #endregion
    #endregion

}
