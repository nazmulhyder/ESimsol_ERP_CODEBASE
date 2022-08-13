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
    #region EmployeeSettlementClearance

    public class EmployeeSettlementClearance : BusinessObject
    {
        public EmployeeSettlementClearance()
        {
            ESCID = 0;
            EmployeeSettlementID = 0;
            ESCSetupID = 0;
            CurrentStatus = EnumESCrearance.None;
            ErrorMessage = "";
            EmployeeName = "";
            EmployeeCode = "";
            DepartmentName = "";
        }

        #region Properties
        public int ESCID { get; set; }
        public int EmployeeSettlementID { get; set; }
        public int ESCSetupID { get; set; }
        public EnumESCrearance CurrentStatus { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public List<EmployeeSettlementClearance> EmployeeSettlementClearances { get; set; }
        public List<EmployeeSettlementClearanceHistory> EmployeeSettlementClearanceHistorys { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        #endregion

        #region Functions
        public static EmployeeSettlementClearance Get(int Id, long nUserID)
        {
            return EmployeeSettlementClearance.Service.Get(Id, nUserID);
        }
        public static EmployeeSettlementClearance Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearance.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSettlementClearance> Gets(long nUserID)
        {
            return EmployeeSettlementClearance.Service.Gets(nUserID);
        }
        public static List<EmployeeSettlementClearance> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearance.Service.Gets(sSQL, nUserID);
        }
        public List<EmployeeSettlementClearance> IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSettlementClearance.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeSettlementClearanceService Service
        {
            get { return (IEmployeeSettlementClearanceService)Services.Factory.CreateService(typeof(IEmployeeSettlementClearanceService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSettlementClearance interface

    public interface IEmployeeSettlementClearanceService
    {
        EmployeeSettlementClearance Get(int id, Int64 nUserID);
        EmployeeSettlementClearance Get(string sSQL, Int64 nUserID);
        List<EmployeeSettlementClearance> Gets(Int64 nUserID);
        List<EmployeeSettlementClearance> Gets(string sSQL, Int64 nUserID);
        List<EmployeeSettlementClearance> IUD(EmployeeSettlementClearance oEmployeeSettlementClearance, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
