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
    #region EmployeeSettlementClearanceSetup

    public class EmployeeSettlementClearanceSetup : BusinessObject
    {
        public EmployeeSettlementClearanceSetup()
        {
            ESCSetupID = 0;
            ESCSID = 0;
            EmployeeID = 0;
            InActiveDate = DateTime.Now;
            ErrorMessage = "";
            SectionName = "";
            EmployeeName = "";
            EmployeeCode = "";
        }

        #region Properties
        public int ESCSetupID { get; set; }
        public int ESCSID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime InActiveDate { get; set; }
        public string ErrorMessage { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string SectionName { get; set; }

        #endregion

        #region Derived Property
        public string InActiveDateInString
        {
            get
            {
                if (InActiveDate.Year < 1900)
                    return "-";
                else
                    return InActiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static EmployeeSettlementClearanceSetup Get(int Id, long nUserID)
        {
            return EmployeeSettlementClearanceSetup.Service.Get(Id, nUserID);
        }
        public static EmployeeSettlementClearanceSetup Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceSetup.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSettlementClearanceSetup> Gets(long nUserID)
        {
            return EmployeeSettlementClearanceSetup.Service.Gets(nUserID);
        }
        public static List<EmployeeSettlementClearanceSetup> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementClearanceSetup.Service.Gets(sSQL, nUserID);
        }
        public EmployeeSettlementClearanceSetup IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSettlementClearanceSetup.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeSettlementClearanceSetupService Service
        {
            get { return (IEmployeeSettlementClearanceSetupService)Services.Factory.CreateService(typeof(IEmployeeSettlementClearanceSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeSettlementClearanceSetup interface

    public interface IEmployeeSettlementClearanceSetupService
    {
        EmployeeSettlementClearanceSetup Get(int id, Int64 nUserID);
        EmployeeSettlementClearanceSetup Get(string sSQL, Int64 nUserID);
        List<EmployeeSettlementClearanceSetup> Gets(Int64 nUserID);
        List<EmployeeSettlementClearanceSetup> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlementClearanceSetup IUD(EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
