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
    #region EmployeeConfirmation

    public class EmployeeConfirmation : BusinessObject
    {
        public EmployeeConfirmation()
        {
            ECID = 0;
            EmployeeCategory = EnumEmployeeCategory.None;
            EmployeeID = 0;
            EndDate = DateTime.Now;
            StartDate = DateTime.Now;
            Note = "";
            MotherEmployeeID = 0;
            ErrorMessage = "";
            EmployeeName="";
            EmployeeCode = "";
            IsConfirm = false;
            DBServerDateTime = DateTime.Now;
            EmployeeConfirmations = new List<EmployeeConfirmation>();
            EmployeeOfficials = new List<EmployeeOfficial>();
        }

        #region Properties
        public int ECID { get; set; }
        public EnumEmployeeCategory EmployeeCategory { get; set; }
        public int EmployeeID { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string Note { get; set; }
        public int MotherEmployeeID { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsConfirm { get; set; }
        public List<EmployeeConfirmation> EmployeeConfirmations { get; set; }
        public List<EmployeeOfficial> EmployeeOfficials { get; set; }
        #endregion

        #region Derived Property
        public string YettoConfirmInString
        {
            get
            {
                if (DateTime.Now < this.EndDate)
                { DateDifference dateDifference = new DateDifference(DateTime.Now, this.EndDate); return dateDifference.ToString(); }
                else { return "-"; }
            }
        }
        public int EmployeeCategoryInt { get; set; }

        public string EmployeeCategoryInString
        {
            get
            {
                return EmployeeCategory.ToString();
            }
        }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DBServerDateTimeInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyy");
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyy");
            }
        }
        #endregion

        #region Functions
        public static EmployeeConfirmation Get(int Id, long nUserID)
        {
            return EmployeeConfirmation.Service.Get(Id, nUserID);
        }

        public static EmployeeConfirmation Get(string sSQL, long nUserID)
        {
            return EmployeeConfirmation.Service.Get(sSQL, nUserID);
        }

        public static List<EmployeeConfirmation> Gets(long nUserID)
        {
            return EmployeeConfirmation.Service.Gets(nUserID);
        }

        public static List<EmployeeConfirmation> Gets(string sSQL, long nUserID)
        {
            return EmployeeConfirmation.Service.Gets(sSQL, nUserID);
        }

        public EmployeeConfirmation IUD(int nDBOperation, long nUserID)
        {
            return EmployeeConfirmation.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeConfirmationService Service
        {
            get { return (IEmployeeConfirmationService)Services.Factory.CreateService(typeof(IEmployeeConfirmationService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeConfirmation interface

    public interface IEmployeeConfirmationService
    {
        EmployeeConfirmation Get(int id, Int64 nUserID);
        EmployeeConfirmation Get(string sSQL, Int64 nUserID);
        List<EmployeeConfirmation> Gets(Int64 nUserID);
        List<EmployeeConfirmation> Gets(string sSQL, Int64 nUserID);
        EmployeeConfirmation IUD(EmployeeConfirmation oEmployeeConfirmation, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
