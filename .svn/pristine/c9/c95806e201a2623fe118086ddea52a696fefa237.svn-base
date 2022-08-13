using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;

namespace ESimSol.BusinessObjects
{
    public class EmployeeSalaryDetailV2 : BusinessObject
    {
        public EmployeeSalaryDetailV2()
        {
            ESDID = 0;
            EmployeeSalaryID = 0;
            SalaryHeadID = 0;
            Amount = 0;
            SubTotalAmount = 0;
            GrandTotalAmount = 0;//Department Wise
            TGrandTotalAmount = 0;
            SalaryHeadName = "";
            SalaryHeadType = EnumSalaryHeadType.None;
            ErrorMessage = "";
            CompAmount = 0;
            EmployeeID = 0;
            SalaryHeadSequence = 0;
            SalaryHeadNameInBangla = "";
        }

        #region Properties
        public int ESDID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int SalaryHeadID { get; set; }
        public int EmployeeID { get; set; }
        public int SalaryHeadSequence { get; set; }
        public double Amount { get; set; }
        public double SubTotalAmount { get; set; }
        public double GrandTotalAmount { get; set; }//Department Wise
        public double TGrandTotalAmount { get; set; }
        public double CompAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string SalaryHeadName { get; set; }
        public string SalaryHeadNameInBangla { get; set; }
        public EnumSalaryHeadType SalaryHeadType { get; set; }
        #endregion
        #region Functions
        public static List<EmployeeSalaryDetailV2> Gets(string sSQL, Int64 nUserID)
        {
            return EmployeeSalaryDetailV2.Service.Gets(sSQL, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IEmployeeSalaryDetailV2Service Service
        {
            get { return (IEmployeeSalaryDetailV2Service)Services.Factory.CreateService(typeof(IEmployeeSalaryDetailV2Service)); }
        }
        #endregion
    }

    #region IEmployeeSalaryDetailV2interface

    public interface IEmployeeSalaryDetailV2Service
    {
        List<EmployeeSalaryDetailV2> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
