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
    #region EmployeeSettlementSalary
    public class EmployeeSettlementSalaryDetail
    {
        public EmployeeSettlementSalaryDetail()
        {
            ESDSalarylID = 0;
            EmployeeSalaryID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            CompAmount = 0.0;
            SalaryHeadName = "";
            SalaryHeadType = 0;
        }
        #endregion

        public int ESDSalarylID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int SalaryHeadID { get; set; }
        public double Amount { get; set; }
        public double CompAmount { get; set; }
        public string SalaryHeadName { get; set; }
        public int SalaryHeadType { get; set; }
        #region derivedproperties



        #endregion

        #region Functions
        public static List<EmployeeSettlementSalaryDetail> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlementSalaryDetail.Service.Gets(sSQL, nUserID);
        }
        public static EmployeeSettlementSalaryDetail Get(string sSQL, long nUserID)
        {
            return EmployeeSettlementSalaryDetail.Service.Get(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IEmployeeSettlementSalaryDetailService Service
        {
            get { return (IEmployeeSettlementSalaryDetailService)Services.Factory.CreateService(typeof(IEmployeeSettlementSalaryDetailService)); }
        }
        #endregion
    }

    #region IEmployeeSettlementSalary interface

    public interface IEmployeeSettlementSalaryDetailService
    {
        List<EmployeeSettlementSalaryDetail> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlementSalaryDetail Get(string sSQL, Int64 nUserID);
    }
    #endregion
}



