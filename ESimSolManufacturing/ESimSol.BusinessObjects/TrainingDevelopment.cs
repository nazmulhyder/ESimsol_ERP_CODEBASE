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
    #region TrainingDevelopment

    public class TrainingDevelopment : BusinessObject
    {
        public TrainingDevelopment()
        {

            TDID = 0;
            EmployeeID = 0;
            CourseName = "";
            Specification = "";
            Institute = "";
            Vendor = "";
            Country = "";
            State = "";
            Address = "";
            Duration = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            EffectFromDate = DateTime.Now;
            EffectToDate = DateTime.Now;
            Note = "";
            ApproveBy = 0;
            ApprovalNote = "";
            Result = "";
            PassingDate = DateTime.Now;
            ResultNote = "";
            IsCompleted = true;
            IsActive = true;
            InactiveNote = "";
            InactiveDate = DateTime.Now;
            ErrorMessage = "";

        }



        #region Properties

        public int TDID { get; set; }
        public int EmployeeID { get; set; }
        public string CourseName { get; set; }
        public string Specification { get; set; }
        public string Institute { get; set; }
        public string Vendor { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EffectFromDate { get; set; }
        public DateTime EffectToDate { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public string ApprovalNote { get; set; }
        public string Result { get; set; }
        public DateTime PassingDate { get; set; }
        public string ResultNote { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public string InactiveNote { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public List<TrainingDevelopment> oTrainingDevelopments { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string EmployeeTypeName { get; set; }
        public EnumEmployeeWorkigStatus WorkingStatus { get; set; }
        public string WorkingStatusInString { get { return this.WorkingStatus.ToString(); } }
        public string ApproveByName { get; set; }
        public string OfficialInfoInString
        {
            get
            {
                return LocationName + "-" + DepartmentName + "-" + DesignationName;
            }
        }

        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        public string EffectFromDateInString
        {
            get
            {
                return EffectFromDate.ToString("dd MMM yyyy");
            }
        }
        public string EffectToDateInString
        {
            get
            {
                return EffectToDate.ToString("dd MMM yyyy");
            }
        }
        public string PassingDateInString
        {
            get
            {
                return PassingDate.ToString("dd MMM yyyy");
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveDate.ToString("dd MMM yyyy");
            }
        }
        public string TrainingStatusInString
        {
            get
            {
                if (ApproveBy <= 0) return "Initialized";
                else if (ApproveBy > 0 && EffectFromDate > DateTime.Now) return "Upcoming";
                else if (ApproveBy > 0 && EffectFromDate <= DateTime.Now && DateTime.Now <= EffectToDate && IsCompleted == false) return "Running";
                else if (ApproveBy > 0 && EffectToDate <= DateTime.Now && IsCompleted == false) return "Training End";
                else if (IsCompleted == true) return "Completed";
                else if (IsActive == false) return "InActive";
                else return "";
            }
        }
        #endregion

        #region Functions
        public static TrainingDevelopment Get(int Id, long nUserID)
        {
            return TrainingDevelopment.Service.Get(Id, nUserID);
        }
        public static TrainingDevelopment Get(string sSQL, long nUserID)
        {
            return TrainingDevelopment.Service.Get(sSQL, nUserID);
        }
        public static List<TrainingDevelopment> Gets(long nUserID)
        {
            return TrainingDevelopment.Service.Gets(nUserID);
        }

        public static List<TrainingDevelopment> Gets(string sSQL, long nUserID)
        {
            return TrainingDevelopment.Service.Gets(sSQL, nUserID);
        }

        public TrainingDevelopment IUD(int nDBOperation, long nUserID)
        {
            return TrainingDevelopment.Service.IUD(this, nDBOperation, nUserID);
        }
        public static TrainingDevelopment Activite(int EmpID, int nId, bool Active, long nUserID)
        {
            return TrainingDevelopment.Service.Activite(EmpID, nId, Active, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static ITrainingDevelopmentService Service
        {
            get { return (ITrainingDevelopmentService)Services.Factory.CreateService(typeof(ITrainingDevelopmentService)); }
        }

        #endregion
    }
    #endregion

    #region ITrainingDevelopment interface

    public interface ITrainingDevelopmentService
    {
        TrainingDevelopment Get(int id, Int64 nUserID);
        TrainingDevelopment Get(string sSQL, Int64 nUserID);
        List<TrainingDevelopment> Gets(Int64 nUserID);
        List<TrainingDevelopment> Gets(string sSQL, Int64 nUserID);
        TrainingDevelopment IUD(TrainingDevelopment oTrainingDevelopment, int nDBOperation, Int64 nUserID);
        TrainingDevelopment Activite(int EmpID, int nId, bool Active, Int64 nUserID);

    }
    #endregion
}
