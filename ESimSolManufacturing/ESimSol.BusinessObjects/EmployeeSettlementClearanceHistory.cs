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
    #region EmployeeSettlementClearanceHistory

    public class EmployeeSettlementClearanceHistory : BusinessObject
    {
        public EmployeeSettlementClearanceHistory()
        {
            ESCHID = 0;
            ESCID = 0;
            PreviousStatus = EnumESCrearance.None;
            CurrentStatus = EnumESCrearance.None;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ESCHID { get; set; }
        public int ESCID { get; set; }
        public EnumESCrearance PreviousStatus { get; set; }
        public EnumESCrearance CurrentStatus { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string PreviousStatusInString
        {
            get
            {
                return PreviousStatus.ToString();
            }
        }
        public string CurrentStatusInString
        {
            get
            {
                return CurrentStatus.ToString();
            }
        }

        #endregion

        #region Functions
        public static EmployeeSettlementClearanceHistory Get(int Id, long nUserID)
        {
            return EmployeeSettlementClearanceHistory.Service.Get(Id, nUserID);
        }
        public static EmployeeSettlementClearanceHistory Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceHistory.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSettlementClearanceHistory> Gets(long nUserID)
        {
            return EmployeeSettlementClearanceHistory.Service.Gets(nUserID);
        }
        public static List<EmployeeSettlementClearanceHistory> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceHistory.Service.Gets(sSQL, nUserID);
        }
        public EmployeeSettlementClearanceHistory IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSettlementClearanceHistory.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeSettlementClearanceHistoryService Service
        {
            get { return (IEmployeeSettlementClearanceHistoryService)Services.Factory.CreateService(typeof(IEmployeeSettlementClearanceHistoryService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSettlementClearanceHistory interface

    public interface IEmployeeSettlementClearanceHistoryService
    {
        EmployeeSettlementClearanceHistory Get(int id, Int64 nUserID);
        EmployeeSettlementClearanceHistory Get(string sSQL, Int64 nUserID);
        List<EmployeeSettlementClearanceHistory> Gets(Int64 nUserID);
        List<EmployeeSettlementClearanceHistory> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlementClearanceHistory IUD(EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
