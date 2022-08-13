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
    #region EmployeeLoanHistory

    public class EmployeeLoanHistory : BusinessObject
    {
        public EmployeeLoanHistory()
        {


            ELHID = 0;
            EmployeeLoanID = 0;
            Status = EnumEmployeeLoanStatus.None;
            PreviousStatus = EnumEmployeeLoanStatus.None;
            Note = "";
            ErrorMessage = "";


        }

        #region Properties

        public int ELHID { get; set; }
        public int EmployeeLoanID { get; set; }
        public EnumEmployeeLoanStatus Status { get; set; }
        public EnumEmployeeLoanStatus PreviousStatus { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int StatusInt { get; set; }
        public string StatusInString { get { return this.Status.ToString(); } }
        public int PreviousStatusInt { get; set; }
        public string PreviousStatusInString { get { return this.PreviousStatus.ToString(); } }

        #endregion

        #region Functions
        public static EmployeeLoanHistory Get(int Id, long nUserID)
        {
            return EmployeeLoanHistory.Service.Get(Id, nUserID);
        }
        public static EmployeeLoanHistory Get(string sSQL, long nUserID)
        {
            return EmployeeLoanHistory.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeLoanHistory> Gets(long nUserID)
        {
            return EmployeeLoanHistory.Service.Gets(nUserID);
        }

        public static List<EmployeeLoanHistory> Gets(string sSQL, long nUserID)
        {
            return EmployeeLoanHistory.Service.Gets(sSQL, nUserID);
        }

        public EmployeeLoanHistory IUD(int nDBOperation, long nUserID)
        {
            return EmployeeLoanHistory.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeLoanHistoryService Service
        {
            get { return (IEmployeeLoanHistoryService)Services.Factory.CreateService(typeof(IEmployeeLoanHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeLoanHistory interface

    public interface IEmployeeLoanHistoryService
    {
        EmployeeLoanHistory Get(int id, Int64 nUserID);
        EmployeeLoanHistory Get(string sSQL, Int64 nUserID);
        List<EmployeeLoanHistory> Gets(Int64 nUserID);
        List<EmployeeLoanHistory> Gets(string sSQL, Int64 nUserID);
        EmployeeLoanHistory IUD(EmployeeLoanHistory oEmployeeLoanHistory, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
