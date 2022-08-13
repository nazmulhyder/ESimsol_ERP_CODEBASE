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
    #region BenefitOnAttendanceEmployeeAssign

    public class BenefitOnAttendanceEmployeeAssign : BusinessObject
    {
        public BenefitOnAttendanceEmployeeAssign()
        {
            BOAEAID = 0;
            BOAEmployeeID = 0;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            ErrorMessage = string.Empty;
        }

        #region Properties
        public int BOAEAID { get; set; }
        public int BOAEmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derive Properties
        public string StartDateStr { get { return this.StartDate.ToString("dd MMM yyyy"); } }
        public string EndDateStr { get { return this.EndDate.ToString("dd MMM yyyy"); } }
        #endregion

        #region Functions
  
        public static BenefitOnAttendanceEmployeeAssign Get(int id, long nUserID)
        {
            return BenefitOnAttendanceEmployeeAssign.Service.Get(id, nUserID);
        }
        public static List<BenefitOnAttendanceEmployeeAssign> Gets(string sSQL, long nUserID)
        {
            return BenefitOnAttendanceEmployeeAssign.Service.Gets(sSQL, nUserID);
        }
        public BenefitOnAttendanceEmployeeAssign IUD(short nDBOperation, long nUserID)
        {
            return BenefitOnAttendanceEmployeeAssign.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceEmployeeAssignService Service
        {
            get { return (IBenefitOnAttendanceEmployeeAssignService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceEmployeeAssignService)); }
        }
        #endregion
    }
    #endregion



    #region IBenefitOnAttendanceEmployeeAssign interface

    public interface IBenefitOnAttendanceEmployeeAssignService
    {
        BenefitOnAttendanceEmployeeAssign Get(int id, Int64 nUserID);
        List<BenefitOnAttendanceEmployeeAssign> Gets(string sSQL, Int64 nUserID);
        BenefitOnAttendanceEmployeeAssign IUD(BenefitOnAttendanceEmployeeAssign oBOAEA, short nDBOperation, Int64 nUserID);
    }
    #endregion
}

