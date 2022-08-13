using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region EmployeeAdvanceSalaryProcess
    public class EmployeeAdvanceSalaryProcess : BusinessObject
    {
        public EmployeeAdvanceSalaryProcess()
        {
            EASPID = 0;
            BUID = 0;
            BUName = "";
            LocationName = "";
            LocationID = 0;
            Description = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            SalaryHeadID = 0;
            NYear = 0;
            NMonth = 0;
            ApproveBy = 0;
            ApproveByName = "";
            ApproveDate = DateTime.Now;
            DeclarationDate = DateTime.Now;
            ErrorMessage = "";
        }
        #region Properties
        public int EASPID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SalaryHeadID { get; set; }
        public int NYear { get; set; }
        public int NMonth { get; set; }
        public int ApproveBy { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string ApproveByName { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime DeclarationDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string StartDateInString
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }

        public string EndDateInString
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                return (ApproveBy == 0) ? "-" : this.ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string DeclarationDateInString
        {
            get
            {
                return this.DeclarationDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public EmployeeAdvanceSalaryProcess Save(int nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Delete(id, nUserID);
        }
        public static List<EmployeeAdvanceSalaryProcess> Gets(int nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Gets(nUserID);
        }
        public static List<EmployeeAdvanceSalaryProcess> Gets(string sSQL, int nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Gets(sSQL, nUserID);
        }
        public EmployeeAdvanceSalaryProcess Approve(long nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Approve(this, nUserID);
        }
        public EmployeeAdvanceSalaryProcess UndoApprove(long nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.UndoApprove(this, nUserID);
        }
        public static EmployeeAdvanceSalaryProcess Get(string sSQL, long nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Get(sSQL, nUserID);
        }
        public EmployeeAdvanceSalaryProcess Get(int nId, long nUserID)
        {
            return EmployeeAdvanceSalaryProcess.Service.Get(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeAdvanceSalaryProcessService Service
        {
            get { return (IEmployeeAdvanceSalaryProcessService)Services.Factory.CreateService(typeof(IEmployeeAdvanceSalaryProcessService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeAdvanceSalaryProcess interface
    public interface IEmployeeAdvanceSalaryProcessService
    {
        EmployeeAdvanceSalaryProcess UndoApprove(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, Int64 nUserID);
        EmployeeAdvanceSalaryProcess Save(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, int nUserID);
        List<EmployeeAdvanceSalaryProcess> Gets(int nUserID);
        List<EmployeeAdvanceSalaryProcess> Gets(string sSQL, int nUserID);
        string Delete(int id, int nUserID);
        EmployeeAdvanceSalaryProcess Get(int id, long nUserID);
        EmployeeAdvanceSalaryProcess Approve(EmployeeAdvanceSalaryProcess oEmployeeAdvanceSalaryProcess, Int64 nUserID);
        EmployeeAdvanceSalaryProcess Get(string sSQL, Int64 nUserID);
    }
    #endregion
}

