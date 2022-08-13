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

    #region EmployeeLeaveLedger

    public class EmployeeLeaveLedger : BusinessObject
    {
        public EmployeeLeaveLedger()
        {
            EmpLeaveLedgerID=0;
            EmployeeID=0;
            ACSID=0;
            ASLID=0;
            LeaveID = 0;
            TotalDay = 0;
            DeferredDay = 0;
            ActivationAfter = EnumRecruitmentEvent.None;
            IsLeaveOnPresence = false;
            PresencePerLeave = 0;
            IsCarryForward = false;
            MaxCarryDays = 0;
            ErrorMessage = "";
            Params = "";
            EmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            IsLWP = false;
            IsComp = true;
            Session = "";
            EnjoyLeave = 0;
        }

        #region Properties

        public string Session { get; set; }
        public int EnjoyLeave { get; set; }
        public int EmpLeaveLedgerID { get; set; }
        public int EmployeeID { get; set; }
        public int ACSID { get; set; }
        public int ASLID { get; set; }
        public int LeaveID { get; set; }
        public double TotalDay { get; set; }
        public int DeferredDay { get; set; }
        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public bool IsLeaveOnPresence { get; set; }
        public bool IsComp { get; set; }
        public int PresencePerLeave { get; set; }
        public bool IsCarryForward { get; set; }
        public int MaxCarryDays { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public string CLDetailsInStr { get; set; }
        public string SLDetailsInStr { get; set; }
        public double Addition { get; set; }
        public double Deduction { get; set; }

        public int FullLeave { get; set; }
        public int HalfLeave { get; set; }
        public int ShortLeave { get; set; }

        public string LeaveName { get; set; }
        public string ActivationAfterInString { get{ return this.ActivationAfter.ToString(); } }
        public int ActivationAfterInInt { get {return (int)this.ActivationAfter;} }

        public double RemainingLeave { get { return this.TotalDay+this.Addition-this.Deduction; } }
        public double YearsToCarry { get { return (this.MaxCarryDays > 0) ? (this.MaxCarryDays / 360) : 0; } }
        public string EarnLeaveStatus { get { return (this.IsLeaveOnPresence) ? "By " + this.PresencePerLeave.ToString() + " days of presence" : ""; } }
        public string CarryForwardStatus { get { return (this.IsCarryForward && this.MaxCarryDays > 0) ? "Max. " + (this.MaxCarryDays / 360).ToString() + " years" : ""; } }
        public string ActivationStatus { get { return ((int)this.ActivationAfter>0) ? this.DeferredDay.ToString() + " days of " + this.ActivationAfter.ToString() : ""; } }

        public int Half { get { if (HalfLeave % 2 == 1) return HalfLeave - 1; else return 0; } }
        public int Short { get { if (ShortLeave % 3 == 1) return ShortLeave - 1; else if (ShortLeave % 3 == 2) return ShortLeave - 2; else return 0; } }

        public double TotalLeave
        { 
            get 
            {
                return this.TotalDay + this.Addition - this.Deduction;
            } 
        }
        public string IsCompSt
        {
            get
            {
                if (this.IsComp)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }

        public double LeaveBalance
        {
            get
            {
                if (this.RemainingLeave > 0)
                {                    
                    //if (HalfLeave % 2 == 1 && (ShortLeave % 3 == 1 || ShortLeave % 3 == 2))
                    //    return RemainingLeave - 2 - FullLeave - Half / 2 - Short / 3;
                    //else if (HalfLeave % 2 == 1 || ShortLeave % 3 == 1 || ShortLeave % 3 == 2)
                    //    return RemainingLeave - 1 - FullLeave - Half / 2 - Short / 3;
                    //else
                    //    return RemainingLeave - FullLeave - Half / 2 - Short / 3;
                    double dBalance = this.FullLeave;
                    if (this.HalfLeave>0)
                    {
                        dBalance = dBalance + Convert.ToDouble(this.HalfLeave) /2;
                    }
                    if (this.ShortLeave > 0)
                    {
                        dBalance = dBalance + Convert.ToDouble(this.ShortLeave) / 4;
                    }
                    return this.RemainingLeave-dBalance;
                }
                else
                {
                    return 0;
                }
            }
        }


        public string HalfLeaveInStr { get { if (HalfLeave % 2 == 1) return ", 1 H"; else return ""; } }
        public string ShortLeaveInStr { get { if (ShortLeave % 3 == 1) return ", 2 S"; else if (ShortLeave % 3 == 2) return ", 1 S"; else return ""; } }

        //public string LeaveBalanceInStr { get { return (this.LeaveBalance > 0) ? LeaveBalance.ToString() + " F" + HalfLeaveInStr + ShortLeaveInStr : "0"; } }
        public string LeaveBalanceInStr { get { return this.LeaveBalance.ToString(); } }
        public double Enjoyed { get { return ((this.FullLeave * 1) + (this.HalfLeave * 0.5) + (this.ShortLeave * 0.25)); } }
        public double Balance { get { return (this.TotalLeave - this.Enjoyed); } }

        public string TransferLeaveInStr 
        { 
            get 
            { 
                if (this.Addition > 0 || this.Deduction > 0) 
                { 
                    return "[+]: " + this.Addition.ToString() + ",    [-]: " + this.Deduction.ToString(); 
                } 
                else 
                { 
                    return "--"; 
                }
            } 
        }

        public bool IsLWP { get; set; }

        public string LeaveNameWithBalance { get { return this.LeaveName + " [" + this.LeaveBalanceInStr + "]"; } }
        public string LeaveWithBalance 
        { 
            get { return this.LeaveName + " [" + this.LeaveBalanceInStr + "]"; } 
        }
        
        public List<EmployeeLeaveLedger> EmployeeLeaveLedgers { get; set; }
        #endregion

        #region Functions
     
        
        public EmployeeLeaveLedger Get(int nEmpLeaveLedgerID, long nUserID)
        {
            return EmployeeLeaveLedger.Service.Get(nEmpLeaveLedgerID, nUserID);
        }
        public static List<EmployeeLeaveLedger> Gets(int nEmpID, long nUserID)
        {
            return EmployeeLeaveLedger.Service.Gets(nEmpID, nUserID);
        }
        public static List<EmployeeLeaveLedger> GetsActiveLeaveLedger(int nEmpID, long nUserID)
        {
            return EmployeeLeaveLedger.Service.GetsActiveLeaveLedger(nEmpID, nUserID);
        }
        public static List<EmployeeLeaveLedger> Gets(string sSQL, long nUserID)
        {
            return EmployeeLeaveLedger.Service.Gets(sSQL, nUserID);
        }
        public EmployeeLeaveLedger IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLeaveLedger.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<EmployeeLeaveLedger> TransferLeave(int nELLIDFrom, int nELLIDTo, int nDays, string sNote, long nUserID)
        {
            return EmployeeLeaveLedger.Service.TransferLeave(nELLIDFrom, nELLIDTo, nDays,sNote, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeLeaveLedgerService Service
        {
            get { return (IEmployeeLeaveLedgerService)Services.Factory.CreateService(typeof(IEmployeeLeaveLedgerService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeLeaveLedger interface

    public interface IEmployeeLeaveLedgerService
    {
        EmployeeLeaveLedger Get(int nEmpLeaveLedgerID, Int64 nUserID);
        List<EmployeeLeaveLedger> GetsActiveLeaveLedger(int nEmpID, Int64 nUserID);
        List<EmployeeLeaveLedger> Gets(int nEmpID, Int64 nUserID);
        List<EmployeeLeaveLedger> Gets(string sSQL, Int64 nUserID);
        EmployeeLeaveLedger IUD(EmployeeLeaveLedger oEmployeeLeaveLedger,int nDBOperation, Int64 nUserID);
        List<EmployeeLeaveLedger> TransferLeave(int nELLIDFrom, int nELLIDTo, int nDays, string sNote, Int64 nUserID);
    }
    #endregion

}
