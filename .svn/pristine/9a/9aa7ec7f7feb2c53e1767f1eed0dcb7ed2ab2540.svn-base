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
    #region EmployeeCardHistory

    public class EmployeeCardHistory : BusinessObject
    {
        public EmployeeCardHistory()
        {
            ECHID = 0;
            EmployeeCardID = 0;
            PreviousStatus = EnumEmployeeCardStatus.None;
            CurrentStatus = EnumEmployeeCardStatus.None;
            StatusChangeDate = DateTime.Now;
            StatusChangeBy = "";
            ErrorMessage = "";

        }

        #region Properties
        public int ECHID { get; set; }
        public int EmployeeCardID { get; set; }
        public EnumEmployeeCardStatus PreviousStatus { get; set; }
        public EnumEmployeeCardStatus CurrentStatus { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public string StatusChangeBy { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region Derived Property
        public Company Company { get; set; }
        public List<Employee> Employees { get; set; }
        public string StatusChangeDateInString
        {
            get
            {
                return StatusChangeDate.ToString("dd MMM yyyy");

            }

        }

        public int PreviousStatusInt;
        public string PreviousStatusInString
        {
            get
            {
                return PreviousStatus.ToString();
            }
        }

        public int CurrentStatusInt;
        public string CurrentStatusInString
        {
            get
            {
                return CurrentStatus.ToString();
            }
        }

        #endregion

        #region Functions
        public static EmployeeCardHistory Get(int id, long nUserID)
        {
            return EmployeeCardHistory.Service.Get(id, nUserID);
        }
        public static EmployeeCardHistory Get(string sSQL, long nUserID)
        {
            return EmployeeCardHistory.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeCardHistory> Gets(long nUserID)
        {
            return EmployeeCardHistory.Service.Gets(nUserID);
        }
        public static List<EmployeeCardHistory> Gets(string sSQL, long nUserID)
        {
            return EmployeeCardHistory.Service.Gets(sSQL, nUserID);
        }
        public EmployeeCardHistory IUD(long nUserID)
        {
            return EmployeeCardHistory.Service.IUD(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeCardHistoryService Service
        {
            get { return (IEmployeeCardHistoryService)Services.Factory.CreateService(typeof(IEmployeeCardHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeCardHistory interface
    public interface IEmployeeCardHistoryService
    {
        EmployeeCardHistory Get(int id, Int64 nUserID);
        EmployeeCardHistory Get(string sSQL, Int64 nUserID);
        List<EmployeeCardHistory> Gets(Int64 nUserID);
        List<EmployeeCardHistory> Gets(string sSQL, Int64 nUserID);
        EmployeeCardHistory IUD(EmployeeCardHistory oEmployeeCardHistorySheet, Int64 nUserID);


    }
    #endregion
}
