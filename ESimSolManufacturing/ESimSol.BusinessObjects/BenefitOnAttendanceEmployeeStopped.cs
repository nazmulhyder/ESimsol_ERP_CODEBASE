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
    #region BenefitOnAttendanceEmployeeStopped

    public class BenefitOnAttendanceEmployeeStopped : BusinessObject
    {
        public BenefitOnAttendanceEmployeeStopped()
        {
            BOAEmployeeID = 0;
            BOAESID = 0;
            InactiveDate = DateTime.Now;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ErrorMessage = "";
            Params = string.Empty;
        }

        #region Properties
        public int BOAESID { get; set; }
        public int BOAEmployeeID { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsPermanent { get; set; }

        public string Params { get; set; }
        #endregion

        #region Derived Property
        public int EmployeeID { get; set; }
        public int BOAID { get; set; }
        public string StartDateSt
        {
            get
            {
                if (this.StartDate == DateTime.MinValue) return "--";

                else return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateSt
        {
            get
            {
                if (this.EndDate == DateTime.MinValue) return "--";

                else return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string InactiveDateSt
        {
            get
            {
                if (this.InactiveDate == DateTime.MinValue) return "--";

                else return this.InactiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public  BenefitOnAttendanceEmployeeStopped Get(int Id, long nUserID)
        {
            return BenefitOnAttendanceEmployeeStopped.Service.Get(Id, nUserID);
        }
        public BenefitOnAttendanceEmployeeStopped GetBy(int nEmployeeID, int nBOAID, long nUserID)
        {
            return BenefitOnAttendanceEmployeeStopped.Service.GetBy( nEmployeeID,  nBOAID, nUserID);
        }
        public static List<BenefitOnAttendanceEmployeeStopped> Gets(string sSQL, long nUserID)
        {
            return BenefitOnAttendanceEmployeeStopped.Service.Gets(sSQL, nUserID);
        }

        public BenefitOnAttendanceEmployeeStopped IUD(short nDBOperation, long nUserID)
        {
            return BenefitOnAttendanceEmployeeStopped.Service.IUD(this, nDBOperation,  nUserID);
        }
        public static List<BenefitOnAttendanceEmployeeStopped> MultiStopped(List<BenefitOnAttendanceEmployeeStopped> oBOAESs, long nUserID)
        {
            return BenefitOnAttendanceEmployeeStopped.Service.MultiStopped(oBOAESs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceEmployeeStoppedService Service
        {
            get { return (IBenefitOnAttendanceEmployeeStoppedService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceEmployeeStoppedService)); }
        }

        #endregion
    }
    #endregion

    #region IBenefitOnAttendanceEmployeeStopped interface

    public interface IBenefitOnAttendanceEmployeeStoppedService
    {
        BenefitOnAttendanceEmployeeStopped Get(int id, Int64 nUserID);
        BenefitOnAttendanceEmployeeStopped GetBy(int nEmployeeID, int nBOAID, Int64 nUserID);
        List<BenefitOnAttendanceEmployeeStopped> Gets(string sSQL, Int64 nUserID);
        BenefitOnAttendanceEmployeeStopped IUD(BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped, short nDBOperation,  Int64 nUserID);
        List<BenefitOnAttendanceEmployeeStopped> MultiStopped(List<BenefitOnAttendanceEmployeeStopped> oBOAESs, Int64 nUserID);
    }
    #endregion
}
