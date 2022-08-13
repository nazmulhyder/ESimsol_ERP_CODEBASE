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
    #region GeneralWorkingDayDetails

    public class GeneralWorkingDayDetail : BusinessObject
    {
        public GeneralWorkingDayDetail()
        {
            GWDDID = 0;
            GWDID = 0;
            DRPID = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
            BUName = "";
            LocationName = "";
            DepartmentName = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
            DRPIDs = "";
        }

        #region Properties
        public int GWDDID { get; set; }
        public int GWDID { get; set; }
        public int DRPID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string DRPIDs { get; set; }
        #endregion

        #region Functions
        public static List<GeneralWorkingDayDetail> Gets(int id, long nUserID)
        {
            return GeneralWorkingDayDetail.Service.Gets(id, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IGeneralWorkingDayDetailService Service
        {
            get { return (IGeneralWorkingDayDetailService)Services.Factory.CreateService(typeof(IGeneralWorkingDayDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IGeneralWorkingDayDetails interface
    public interface IGeneralWorkingDayDetailService
    {
        List<GeneralWorkingDayDetail> Gets(int id, Int64 nUserID);
      
    }
    #endregion
}
