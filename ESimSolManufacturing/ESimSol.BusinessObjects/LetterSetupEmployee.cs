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
    #region LetterSetupEmployee
    public class LetterSetupEmployee
    {
        public LetterSetupEmployee()
        {
            LSEID = 0;
            LSID = 0;
            EmployeeID = 0;
            Code = "";
            Body = "";
            Remark = "";
            ApproveBy = 0;
            ApproveByName = "";
            ApproveDate = DateTime.Now;
            LetterName = "";
            EmployeeName = "";
            EmployeeCode = "";
            ErrorMessage = "";
            BUName = "";
            BUAddress = "";
            IsEnglish = false;
        }

        #region Properties
        public int LSEID { get; set; }
        public bool IsEnglish { get; set; }
        public int LSID { get; set; }
        public int EmployeeID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Code { get; set; }
        public string BUName { get; set; }
        public string BUAddress { get; set; }
        public string LetterName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Body { get; set; }
        public string ApproveByName { get; set; }
        public string Remark { get; set; }
        public string ErrorMessage { get; set; }
        #endregion


        public string ApproveDateInString
        {
            get
            {
                return (ApproveDate==DateTime.MinValue ? "-" : ApproveDate.ToString("dd/MM/yyyy"));
            }
        }

        #region Functions


        public static List<LetterSetupEmployee> Gets(string sSQL, long nUserID)
        {
            return LetterSetupEmployee.Service.Gets(sSQL, nUserID);
        }
        public LetterSetupEmployee Get(int id, long nUserID)
        {
            return LetterSetupEmployee.Service.Get(id, nUserID);
        }
        public static LetterSetupEmployee Get(string sSQL, long nUserID)
        {
            return LetterSetupEmployee.Service.Get(sSQL, nUserID);
        }
        public LetterSetupEmployee IUD(int nDBOperation, long nUserID)
        {
            return LetterSetupEmployee.Service.IUD(this, nDBOperation, nUserID);
        }
        public string Delete(long nUserID)
        {
            return LetterSetupEmployee.Service.Delete(this, nUserID);
        }
        public string GetEmpLetter(long nUserID)
        {
            return LetterSetupEmployee.Service.GetEmpLetter(this, nUserID);
        }
        public LetterSetupEmployee Approve(long nUserID)
        {
            return LetterSetupEmployee.Service.Approve(this, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static ILetterSetupEmployeeService Service
        {
            get { return (ILetterSetupEmployeeService)Services.Factory.CreateService(typeof(ILetterSetupEmployeeService)); }
        }
        #endregion
    }
    #endregion

    #region ILetterSetupService interface

    public interface ILetterSetupEmployeeService
    {
        LetterSetupEmployee Get(int id, Int64 nUserID);
        List<LetterSetupEmployee> Gets(string sSQL, Int64 nUserID);
        LetterSetupEmployee Get(string sSQL, Int64 nUserID);
        string Delete(LetterSetupEmployee oLetterSetup, Int64 nUserID);
        string GetEmpLetter(LetterSetupEmployee oLetterSetup, Int64 nUserID);
        LetterSetupEmployee Approve(LetterSetupEmployee oLetterSetupEmployee, Int64 nUserID);
        LetterSetupEmployee IUD(LetterSetupEmployee oLetterSetup, int nDBOperation, Int64 nUserID);
      
    }
    #endregion
}



