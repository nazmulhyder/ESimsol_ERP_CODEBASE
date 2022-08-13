using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region ApprovalHead
    public class EmployeeBonusProcess
    {
        public EmployeeBonusProcess()
        {
            EBPID=0;
            BUID=0;
            LocationID=0;
            SalaryheadID=0;
            IsEmployeeWise=false;
            ProcessDate=DateTime.Now;
            BonusDeclarationDate=DateTime.Now;
            Note="";
            Year = 0;
            MonthID=EnumMonth.None;
            ApproveBy=0;
            ApproveDate = DateTime.Now;
            SalaryHeads = new List<SalaryHead>();
            SalarySchemes = new List<SalaryScheme>();
            EmployeeGroups = new List<EmployeeGroup>();
            DepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            EmployeeIDs = "";
        }

        #region Properties
        public int EBPID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int SalaryheadID { get; set; }
        public bool IsEmployeeWise { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime BonusDeclarationDate { get; set; }
        public string Note { get; set; }
        public string EmployeeIDs { get; set; }
        public int Year { get; set; }
        public EnumMonth MonthID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ErrorMessage { get; set; }
        public string ApproveByName { get; set; }
        public List<Location> Locations { get; set; }
        public List<DepartmentRequirementPolicy> DepartmentRequirementPolicys { get; set; }
        public List<SalaryScheme> SalarySchemes { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
        public List<EmployeeGroup> EmployeeGroups { get; set; }

        public int MonthIDInt { get; set; }

        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string ProcessFor
        {
            get
            {
                return (this.IsEmployeeWise) ? "Employee Wise" : "BU,Loc Wise";
            }
        }
        public string ProcessDateInString
        {
            get
            {
                return (this.ProcessDate == DateTime.MinValue ? "-" : this.ProcessDate.ToString("dd MMM yyyy"));
            }
        }
        public string BonusDeclarationDateInString
        {
            get
            {
                return (this.BonusDeclarationDate == DateTime.MinValue ? "-" : this.BonusDeclarationDate.ToString("dd MMM yyyy"));
            }
        }
        #endregion

        #region Functions


        public static EmployeeBonusProcess Get(int id, long nUserID)
        {
            return EmployeeBonusProcess.Service.Get(id, nUserID);
        }
        public static List<EmployeeBonusProcess> Gets(string sSQL, long nUserID)
        {
            return EmployeeBonusProcess.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeBonusProcess Get(string sSQL, long nUserID)
        {
            return EmployeeBonusProcess.Service.Get(sSQL, nUserID);
        }
        public EmployeeBonusProcess IUD(int nDBOperation, long nUserID)
        {
            return EmployeeBonusProcess.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<EmployeeBonusProcess> Gets(long nUserID)
        {
            return EmployeeBonusProcess.Service.Gets(nUserID);
        }
        public EmployeeBonusProcess Approved(long nUserID)
        {
            return EmployeeBonusProcess.Service.Approved(this, nUserID);
        }
        public EmployeeBonusProcess UndoApproved(long nUserID)
        {
            return EmployeeBonusProcess.Service.UndoApproved(this, nUserID);
        }

        public EmployeeBonusProcess Process(long nUserID)
        {
            return EmployeeBonusProcess.Service.Process(this, nUserID);
        }
        public int ProcessEmpBonus(int nIndex, int nEBPID, string sEmployeeIDs, long nUserID)
        {
            return EmployeeBonusProcess.Service.ProcessEmpBonus(nIndex, nEBPID, sEmployeeIDs, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeBonousProcessService Service
        {
            get { return (IEmployeeBonousProcessService)Services.Factory.CreateService(typeof(IEmployeeBonousProcessService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IEmployeeBonousProcessService
    {
        EmployeeBonusProcess Get(int id, Int64 nUserID);
        List<EmployeeBonusProcess> Gets(string sSQL, Int64 nUserID);
        EmployeeBonusProcess Get(string sSQL, Int64 nUserID);
        List<EmployeeBonusProcess> Gets(long nUserID);
        EmployeeBonusProcess IUD(EmployeeBonusProcess oEmployeeBonousProcess, int nDBOperation, Int64 nUserID);
        EmployeeBonusProcess Process(EmployeeBonusProcess oEmployeeBonousProcess, Int64 nUserID);
        EmployeeBonusProcess Approved(EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID);
        EmployeeBonusProcess UndoApproved(EmployeeBonusProcess oEmployeeBonusProcess, Int64 nUserID);
        int ProcessEmpBonus(int nIndex, int nEBPID, string sEmployeeIDs, long nUserID);
      
    }
    #endregion
}


