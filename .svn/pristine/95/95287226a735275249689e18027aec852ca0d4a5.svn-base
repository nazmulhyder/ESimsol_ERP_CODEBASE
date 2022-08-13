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
    #region ProcessManagementPermission

    public class ProcessManagementPermission : BusinessObject
    {
        public ProcessManagementPermission()
        {
            PMPID = 0;
            UserID = 0;
            CompanyID = 0;
            LocationID = 0;
            DepartmentID = 0;
            ProcessManagementType = EnumProcessManagementType.None;
            ProcessType = EnumProcessType.None;
            ProcessStatus = EnumProcessStatus.Initialize;
            IsActive = true;
            ErrorMessage = "";
        }


        #region Properties


        public int PMPID { get; set; }

        public int UserID { get; set; }

        public int CompanyID { get; set; }

        public int LocationID { get; set; }

        public int DepartmentID { get; set; }

        public EnumProcessManagementType ProcessManagementType { get; set; }

        public EnumProcessType ProcessType { get; set; }

        public EnumProcessStatus ProcessStatus { get; set; }

        public bool IsActive { get; set; }

        public string ErrorMessage { get; set; }

        public List<ProcessManagementPermission> ProcessManagementPermissions { get; set; }
        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }

        public string EmployeeNameCode { get; set; }

        public string LocationName { get; set; }

        public string DepartmentName { get; set; }

        public string UserName { get; set; }

        public int ProcessTypeInt { get; set; }
        public string ProcessTypeInString
        {
            get
            {
                return ProcessType.ToString();
            }
        }

        public int ProcessManagementTypeInt { get; set; }
        public string ProcessManagementTypeInString
        {
            get
            {
                return ProcessManagementType.ToString();
            }
        }

        public int ProcessStatusInt { get; set; }
        public string ProcessStatusInString
        {
            get
            {
                return ProcessStatus.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<ProcessManagementPermission> Gets(int nUID, long nUserID)
        {
            return ProcessManagementPermission.Service.Gets(nUID, nUserID);
        }
        public static List<ProcessManagementPermission> Gets(string sSQL, long nUserID)
        {
            return ProcessManagementPermission.Service.Gets(sSQL, nUserID);
        }
        public static ProcessManagementPermission Get(int id, long nUserID)
        {
            return ProcessManagementPermission.Service.Get(id, nUserID);
        }

        public ProcessManagementPermission IUD(int nDBOperation, long nUserID)
        {
            return ProcessManagementPermission.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ProcessManagementPermission Activite(int id, bool Active, long nUserID)
        {
            return ProcessManagementPermission.Service.Activite(id, Active, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProcessManagementPermissionService Service
        {
            get { return (IProcessManagementPermissionService)Services.Factory.CreateService(typeof(IProcessManagementPermissionService)); }
        }

        #endregion
    }
    #endregion

    #region IProcessManagementPermission interface

    public interface IProcessManagementPermissionService
    {

        List<ProcessManagementPermission> Gets(int nUID, Int64 nUserID);//nUID=UserID

        List<ProcessManagementPermission> Gets(string sSQL, Int64 nUserID);

        ProcessManagementPermission Get(int id, Int64 nUserID);

        ProcessManagementPermission IUD(ProcessManagementPermission oProcessManagementPermission, int nDBOperation, Int64 nUserID);

        ProcessManagementPermission Activite(int id, bool Active, Int64 nUserID);
    }
    #endregion
}
