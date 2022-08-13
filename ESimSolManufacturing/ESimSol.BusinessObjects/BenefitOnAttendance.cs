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
    #region BenefitOnAttendance

    public class BenefitOnAttendance : BusinessObject
    {
        public BenefitOnAttendance()
        {

            BOAID=0;
            Name="";
            BenefitOn= EnumBenefitOnAttendance.None;
            StartTime=DateTime.Now;
            EndTime=DateTime.Now;
            TolarenceInMinute=0;
            OTInMinute=0;
            OTDistributePerPresence=0;
            IsFullWorkingHourOT = false;
            SalaryHeadID=0;
            AllowanceOn=EnumPayrollApplyOn.None;
            IsPercent=true;
            Value=0;
            LeaveHeadID=0;
            LeaveAmount=0;
            HolidayID = 0;
            IsContinous=true;
            BenefitStartDate = DateTime.Now;
            BenefitEndDate= DateTime.Now;
            ApproveBy=0;
            ApproveDate= DateTime.Now;
            InactiveDate = DateTime.Now;
            InactiveBy = 0;
            CreateDate = DateTime.Now;
            ErrorMessage = "";
            SalaryHeadName = "";
            LeaveHeadName = "";
            ApproveByName = "";
            nID = 0;
            Params = "";
        }

        #region Properties
        public int BOAID { get; set; }
        public string Name { get; set; }
        public EnumBenefitOnAttendance BenefitOn { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TolarenceInMinute{ get; set; }
        public int OTInMinute{ get; set; }
        public int OTDistributePerPresence{ get; set; }
        public bool IsFullWorkingHourOT { get; set; }
        public int SalaryHeadID{ get; set; }
        public EnumPayrollApplyOn AllowanceOn { get; set; }
        public bool IsPercent{ get; set; }
        public double Value{ get; set; }
        public int LeaveHeadID{ get; set; }
        public int  LeaveAmount{ get; set; }
        public int HolidayID { get; set; }
        public bool IsContinous{ get; set; }
        public DateTime BenefitStartDate{ get; set; }
        public DateTime BenefitEndDate{ get; set; }
        public int ApproveBy{ get; set; }
        public DateTime ApproveDate{ get; set; }
        public DateTime InactiveDate { get; set; }
        public int InactiveBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ErrorMessage { get; set; }
        public string CurrencySymbol { get; set; }

        public string Params { get; set; }

        #endregion

        #region Derived Property
        public string EncryptedID { get; set; }
        public string ActivityInString { get { if (InactiveBy > 0)return "Inactive"; else return "Active"; } }
        public string OvertimeST
        {
            get
            {
                if (this.OTInMinute > 0 && !this.IsFullWorkingHourOT)
                    return "OT " + (this.OTInMinute / 60).ToString() + " hr and distribute " + (this.OTDistributePerPresence/60).ToString()+" per day";
                else if (this.OTInMinute <= 0 && this.IsFullWorkingHourOT)
                    return "Full Working Hour";
                else
                    return "";
            }
        }
        public string SalaryST
        {
            get
            {
                string S = "";
                if (this.SalaryHeadID > 0)
                {
                    S = this.SalaryHeadName + ", ";

                    if (this.IsPercent)
                    {
                        S += this.Value + "% of" + this.AllowanceOnInString;
                    }
                    else
                    {
                        S += this.CurrencySymbol+ this.Value + "(fixed)";
                    }
                    return S;
                }
                return S;
            }
        }
        public string LeaveST
        {
            get
            {
                if (this.LeaveHeadID > 0)
                    return this.LeaveAmount+" " + this.LeaveHeadName;
                else
                    return "";
            }
        }

        public string StartTimeInString
        {
            get
            {
                if (StartTime.ToString("HH:mm") != "00:00")
                    return StartTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string EndTimeInString
        {
            get
            {
                if (EndTime.ToString("HH:mm") != "00:00")
                    return EndTime.ToString("HH:mm");
                else
                    return "-";
            }
        }
        public string BenefitStartDateInString
        {
            get
            {
                return BenefitStartDate.ToString("dd MMM yyyy");
            }
        }
        public string BenefitEndDateInString
        {
            get
            {
                return BenefitEndDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (this.ApproveBy > 0)
                { return ApproveDate.ToString("dd MMM yyyy"); }
                else return "";
                
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveDate.ToString("dd MMM yyyy");
            }
        }
        public string CreateDateInString
        {
            get
            {
                return CreateDate.ToString("dd MMM yyyy");
            }
        }
        public int BenefitOnInt { get; set; }
        public string BenefitOnInString
        {
            get
            {
                return this.BenefitOn.ToString();
            }
        }
        public int AllowanceOnInt { get; set; }
        public string AllowanceOnInString
        {
            get
            {
                return this.AllowanceOn.ToString();
            }
        }
        public int nID { get; set; }
        public Company Company { get; set; }
        public string SalaryHeadName { get; set; }
        public string LeaveHeadName { get; set; }
        public string ApproveByName { get; set; }
        public List<BenefitOnAttendance> BOAs { get; set; }
        public List<BenefitOnAttendanceEmployee> BOAEmployees { get; set; }
        #endregion

        #region Functions
        public static BenefitOnAttendance Get(int Id, long nUserID)
        {
            return BenefitOnAttendance.Service.Get(Id, nUserID);
        }
        public static BenefitOnAttendance Get(string sSQL, long nUserID)
        {
            return BenefitOnAttendance.Service.Get(sSQL, nUserID);
        }
        public static List<BenefitOnAttendance> Gets(long nUserID)
        {
            return BenefitOnAttendance.Service.Gets(nUserID);
        }

        public static List<BenefitOnAttendance> Gets(string sSQL, long nUserID)
        {
            return BenefitOnAttendance.Service.Gets(sSQL, nUserID);
        }

        public BenefitOnAttendance IUD(int nDBOperation, long nUserID)
        {
            return BenefitOnAttendance.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBenefitOnAttendanceService Service
        {
            get { return (IBenefitOnAttendanceService)Services.Factory.CreateService(typeof(IBenefitOnAttendanceService)); }
        }

        #endregion
    }
    #endregion

    #region IBenefitOnAttendance interface

    public interface IBenefitOnAttendanceService
    {
        BenefitOnAttendance Get(int id, Int64 nUserID);
        BenefitOnAttendance Get(string sSQL, Int64 nUserID);
        List<BenefitOnAttendance> Gets(Int64 nUserID);
        List<BenefitOnAttendance> Gets(string sSQL, Int64 nUserID);
        BenefitOnAttendance IUD(BenefitOnAttendance oBenefitOnAttendance, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
