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
    #region EmployeeOfficial
    [DataContract]
    public class EmployeeOfficial : BusinessObject
    {
        public EmployeeOfficial()
        {
            EmployeeOfficialID = 0;
            EmployeeID = 0;
            AttendanceSchemeID = 0;
            DRPID = 0;
            DesignationID = 0;
            CurrentShiftID = 0;
            WorkingStatus = EnumEmployeeWorkigStatus.None;
            DateOfJoin = DateTime.Now;
            DateOfConfirmation = DateTime.Now;
            IsActive = true;
            EmployeeTypeID = 0;
            ErrorMessage = "";
            EmployeeOfficials=new List<EmployeeOfficial>();
            DRPName = "";
            EmployeeName = "";
            RowNum = 0;
            Params = "";
            IsUser = false;
            EmployeeConfirmation = new EmployeeConfirmation();
        }

        #region Properties
        
        public int EmployeeOfficialID { get; set; }
        
        public string EmployeeName { get; set; }
        
        public int EmployeeID { get; set; }
        
        public int AttendanceSchemeID { get; set; }
        
        public int DRPID { get; set; }
        
        public int DesignationID { get; set; }
        
        public int CurrentShiftID { get; set; }
        
        public EnumEmployeeWorkigStatus WorkingStatus { get; set; }
        
        public DateTime DateOfJoin { get; set; }
        
        public DateTime DateOfConfirmation { get; set; }
        
        public bool IsActive { get; set; }

        public int EmployeeTypeID { get; set; }
        
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public bool IsUser { get; set; }
        #endregion
        
        #region Derived Property
        public int RowNum { get; set; }        
        public string Code { get; set; }
        
        public string EmployeeTypeName { get; set; }
        
        public string CurrentShiftName { get; set; }
        
        public string DesignationName { get; set; }
        
        public List<EmployeeOfficial> EmployeeOfficials { get; set; }
       
        public string AttendanceSchemeName { get; set; }
        
        public int LocationID { get; set; }
        
        public string LocationName { get; set; }
        
        public string DepartmentName { get; set; }
        
        public int DepartmentID { get; set; }
        
        public int RosterPlanID { get; set; }
        
        public string RosterPlanDescription { get; set; }
        
        public string DRPName { get; set; }
        
        public AttendanceScheme AttendanceScheme { get; set; }
        public bool Selected { get; set; }

        public string DateOfJoinInString { get { return this.DateOfJoin.ToString("dd MMM yyyy"); } }
        public string DateOfConfirmationInString { get { return this.DateOfConfirmation.ToString("dd MMM yyyy"); } }
        public System.Drawing.Image EmployeePhoto { get; set; }
        public string WorkingStatusInString { get { return this.WorkingStatus.ToString(); } }
        public EmployeeConfirmation EmployeeConfirmation { get; set; }
        
        #endregion

        #region Functions

        public static List<EmployeeOfficial> Gets(int nEmployeeID, long nUserID)
        {
            return EmployeeOfficial.Service.Gets(nEmployeeID, nUserID);
        }        
        public static List<EmployeeOfficial> Gets(string sSql, long nUserID)
        {
            return EmployeeOfficial.Service.Gets(sSql, nUserID);
        }
        public static int TotalRecordCount(string sSql, long nUserID)
        {
            return EmployeeOfficial.Service.TotalRecordCount(sSql, nUserID);
        }
        public static EmployeeOfficial Get(string sSql, long nUserID)
        {
            return EmployeeOfficial.Service.Get(sSql, nUserID);
        }
        public EmployeeOfficial Get(int id, long nUserID) //EmployeeOfficialID
        {
            return EmployeeOfficial.Service.Get(id, nUserID);
        }
        public EmployeeOfficial GetByEmployee(int nEmployeeID, long nUserID) //EmployeeID
        {
            return EmployeeOfficial.Service.GetByEmployee(nEmployeeID, nUserID);
        }
        public EmployeeOfficial IUD(int nDBOperation, long nUserID)
        {
            return EmployeeOfficial.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeOfficialService Service
        {
            get { return (IEmployeeOfficialService)Services.Factory.CreateService(typeof(IEmployeeOfficialService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeOfficial interface
    
    public interface IEmployeeOfficialService
    {        
        EmployeeOfficial Get(int id, Int64 nUserID);//EmployeeOfficialID        
        EmployeeOfficial GetByEmployee(int nEmployeeID, long nUserID); //EmployeeID
        List<EmployeeOfficial> Gets(int nEmployeeID, Int64 nUserID);        
        List<EmployeeOfficial> Gets(string sSql, Int64 nUserID);
        int TotalRecordCount(string sSql, long nUserID);
        EmployeeOfficial Get(string sSql, Int64 nUserID);        
        EmployeeOfficial IUD(EmployeeOfficial oEmployeeOfficial, int nDBOperation, Int64 nUserID);

    }
    #endregion
}