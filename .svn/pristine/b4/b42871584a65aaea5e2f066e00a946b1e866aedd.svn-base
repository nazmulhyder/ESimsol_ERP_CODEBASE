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
    #region SalarySummery
    [DataContract]
    public class EmpSalarySummery : BusinessObject
    {
        public EmpSalarySummery()
        {
            DepartmentID = 0;
            IsProductionBase = true;
            DepartmentName = "";
            NoOfEmp = 0;
            ProductionAmount = 0;
            ProductionBonus = 0;
            AttBonus = 0;
            OTAmount = 0;
            LeaveAllw = 0;
            ShortFall = 0;
            TotalNoWorkDayAllowance = 0;
            AdvPayment = 0;
            RStamp = 0;
            ErrorMessage = "";
          
        }

        #region Properties
        public int DepartmentID { get; set; }
        public bool IsProductionBase { get; set; }
        public string DepartmentName { get; set; }
        public int NoOfEmp { get; set; }
        public double ProductionAmount { get; set; }
        public double ProductionBonus { get; set; }
        public double AttBonus { get; set; }
        public double OTAmount { get; set; }
        public double LeaveAllw { get; set; }
        public double ShortFall { get; set; }
        public double TotalNoWorkDayAllowance { get; set; }
        public double AdvPayment { get; set; }
        public double RStamp { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        public Company Company { get; set; }
        
        public List<EmpSalarySummery> SalarySummerys { get; set; }
        public string NoOfEmpST
        {
            get
            {
                if (this.NoOfEmp > 0) return Global.MillionFormat_Round(this.NoOfEmp);
                else return "--";
            }
        }
        public string ProductionAmountST
        {
            get
            {
                if (this.ProductionAmount > 0) return Global.MillionFormat(this.ProductionAmount);
                else return "--";
            }
        }
        public string ProductionBonusST
        {
            get
            {
                if (this.ProductionBonus > 0) return Global.MillionFormat(this.ProductionBonus);
                else return "--";
            }
        }
        public string AttBonusST
        {
            get
            {
                if (this.AttBonus > 0) return Global.MillionFormat(this.AttBonus);
                else return "--";
            }
        }
        public string OTAmountST
        {
            get
            {
                if (this.OTAmount > 0) return Global.MillionFormat(this.OTAmount);
                else return "--";
            }
        }
        public string LeaveAllwST
        {
            get
            {
                if (this.LeaveAllw > 0) return Global.MillionFormat(this.LeaveAllw);
                else return "--";
            }
        }
        public string ShortFallST
        {
            get
            {
                if (this.ShortFall > 0) return Global.MillionFormat(this.ShortFall);
                else return "--";
            }
        }
        public string TotalNoWorkDayAllowanceST
        {
            get
            {
                if (this.TotalNoWorkDayAllowance > 0) return Global.MillionFormat(this.TotalNoWorkDayAllowance);
                else return "--";
            }
        }
        public string AdvPaymentST
        {
            get
            {
                if (this.AdvPayment > 0) return Global.MillionFormat(this.AdvPayment);
                else return "--";
            }
        }
        public string RStampST
        {
            get
            {
                if (this.RStamp > 0) return Global.MillionFormat(this.RStamp);
                else return "--";
            }
        }
        #endregion

        #region Functions
        public static List<EmpSalarySummery> Gets(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID)
        {
            return EmpSalarySummery.Service.Gets(sEmpIDs, StartDate, EndDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, nUserID);
        }

        public static List<EmpSalarySummery> Gets_Compliance(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID)
        {
            return EmpSalarySummery.Service.Gets(sEmpIDs, StartDate, EndDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmpSalarySummeryService Service
        {
            get { return (IEmpSalarySummeryService)Services.Factory.CreateService(typeof(IEmpSalarySummeryService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySummery interface

    public interface IEmpSalarySummeryService
    {
        List<EmpSalarySummery> Gets(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID);

        List<EmpSalarySummery> Gets_Compliance(string sEmpIDs, DateTime StartDate, DateTime EndDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, Int64 nUserID);
    }
    #endregion
}
