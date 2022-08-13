using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Globalization;

namespace ESimSol.BusinessObjects
{
    #region ArchiveSalaryStruc

    public class ArchiveSalaryStruc : BusinessObject
    {
        public ArchiveSalaryStruc()
        {
            ArchiveSalaryStrucID = 0;
            ArchiveDataID = 0;
            SalaryMonthID = 0;
            SalaryYearID = 0;
            EmployeeID = 0;
            BUID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            EmployeeTypeID = 0;
            SSGradeID = 0;
            DateOfJoin = DateTime.Today;
            SalarySchemeID = 0;
            IsAllowBankAccount = false;
            IsAllowOverTime = false;
            IsAttendanceDependent = false;
            IsProductionBase = false;
            GrossAmount = 0;
            CompGrossAmount = 0;
            IsCashFixed = false;
            CashBankAmount = 0;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            EmpCode = "";
            EmpName = "";
            LocName = "";
            SchemeName = "";
            DeptName = "";
            DesigName = "";
            BUName = "";
            BUShortName = "";
            EntryUserName = "";
            EmployeeTypeName = "";
            ErrorMessage = "";
            BasicArchiveSalaryStrucDtls = new List<ArchiveSalaryStrucDtl>();
            AllowanceArchiveSalaryStrucDtls = new List<ArchiveSalaryStrucDtl>();
        }

        #region Properties
        public int ArchiveSalaryStrucID { get; set; }
        public int ArchiveDataID { get; set; }
        public int SalaryMonthID { get; set; }
        public int SalaryYearID { get; set; }
        public int EmployeeID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int EmployeeTypeID { get; set; }
        public int SSGradeID { get; set; }
        public DateTime DateOfJoin { get; set; }
        public int SalarySchemeID { get; set; }
        public bool IsAllowBankAccount { get; set; }
        public bool IsAllowOverTime { get; set; }
        public bool IsAttendanceDependent { get; set; }
        public bool IsProductionBase { get; set; }
        public double GrossAmount { get; set; }
        public double CompGrossAmount { get; set; }
        public bool IsCashFixed { get; set; }
        public double CashBankAmount { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string LocName { get; set; }
        public string SchemeName { get; set; }
        public string DeptName { get; set; }
        public string DesigName { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string EntryUserName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string ErrorMessage { get; set; }
        public int EmployeeBatchID { get; set; }
        public List<ArchiveSalaryStrucDtl> BasicArchiveSalaryStrucDtls { get; set; }
        public List<ArchiveSalaryStrucDtl> AllowanceArchiveSalaryStrucDtls { get; set; }
        #endregion

        #region Derived Property
        public string IsAllowBankAccountSt
        {
            get
            {
                if (this.IsAllowBankAccount)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsAllowOverTimeSt
        {
            get
            {
                if (this.IsAllowOverTime)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsAttendanceDependentSt
        {
            get
            {
                if (this.IsAttendanceDependent)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsProductionBaseSt
        {
            get
            {
                if (this.IsProductionBase)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string IsCashFixedSt
        {
            get
            {
                if (this.IsCashFixed)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        public string GrossAmountSt
        {
            get
            {
                return this.GrossAmount.ToString("#,##0.00");
            }
        }
        public string CompGrossAmountSt
        {
            get
            {
                return this.CompGrossAmount.ToString("#,##0.00");
            }
        }
        public string BankAmountSt
        {
            get
            {
                if (this.IsCashFixed)
                {
                    return "";
                }
                else
                {
                    return this.CashBankAmount.ToString("#,##0.00");
                }
            }
        }
        public string DBServerDateTimeSt
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string DateOfJoinSt
        {
            get
            {
                return this.DateOfJoin.ToString("dd MMM yyyy");
            }
        }
        public string SalaryMonth 
        {
            get
            {
                string sMonthName = "";
                if (this.SalaryMonthID > 0)
                {
                    sMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(this.SalaryMonthID);
                    sMonthName = sMonthName.Substring(0, 3);
                }
                return sMonthName + "/" + this.SalaryYearID.ToString();
            }
        }        
        #endregion

        #region Functions

        public static List<ArchiveSalaryStruc> Gets(long nUserID)
        {
            return ArchiveSalaryStruc.Service.Gets(nUserID);
        }
        public static List<ArchiveSalaryStruc> Gets(string sSQL, Int64 nUserID)
        {
            return ArchiveSalaryStruc.Service.Gets(sSQL, nUserID);
        }
        public ArchiveSalaryStruc Get(int nId, long nUserID)
        {
            return ArchiveSalaryStruc.Service.Get(nId, nUserID);
        }
        public ArchiveSalaryStruc Save(long nUserID)
        {
            return ArchiveSalaryStruc.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ArchiveSalaryStruc.Service.Delete(nId, nUserID);
        }
        public static List<ArchiveSalaryStruc> ArchiveSalaryChnage(int nArchiveDataID, List<EmployeeBatchDetail> oEmployeeBatchDetails, long nUserID)
        {
            return ArchiveSalaryStruc.Service.ArchiveSalaryChnage(nArchiveDataID, oEmployeeBatchDetails, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IArchiveSalaryStrucService Service
        {
            get { return (IArchiveSalaryStrucService)Services.Factory.CreateService(typeof(IArchiveSalaryStrucService)); }
        }
        #endregion
    }
    #endregion

    #region IArchiveSalaryStruc interface

    public interface IArchiveSalaryStrucService
    {
        ArchiveSalaryStruc Get(int id, long nUserID);
        List<ArchiveSalaryStruc> Gets(long nUserID);
        List<ArchiveSalaryStruc> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        ArchiveSalaryStruc Save(ArchiveSalaryStruc oArchiveSalaryStruc, long nUserID);
        List<ArchiveSalaryStruc> ArchiveSalaryChnage(int nArchiveDataID, List<EmployeeBatchDetail> oEmployeeBatchDetails, long nUserID);
    }
    #endregion
}