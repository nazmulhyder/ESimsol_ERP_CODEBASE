using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region EmployeeActiveInactiveHistory
    public class EmployeeActiveInactiveHistory : BusinessObject
    {
        public EmployeeActiveInactiveHistory()
        {
            EAIHID = 0;
            EmployeeID = 0;
            ActiveDate = DateTime.Now;
            InactiveDate = DateTime.Now;
            ErrorMessage = "";
        }
        #region Properties
        public int EAIHID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ActiveDateInString
        {
            get
            {
                return this.ActiveDate.ToString("dd MMM yyyy"); ;
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return this.InactiveDate.ToString("dd MMM yyyy"); ;
            }
        }
        #endregion

        #region Functions
        public EmployeeActiveInactiveHistory Get(int id, int nUserID)
        {
            return EmployeeActiveInactiveHistory.Service.Get(id, nUserID);
        }
        public static List<EmployeeActiveInactiveHistory> Gets(int nUserID)
        {
            return EmployeeActiveInactiveHistory.Service.Gets(nUserID);
        }
        public static List<EmployeeActiveInactiveHistory> Gets(string sSQL, int nUserID)
        {
            return EmployeeActiveInactiveHistory.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeActiveInactiveHistoryService Service
        {
            get { return (IEmployeeActiveInactiveHistoryService)Services.Factory.CreateService(typeof(IEmployeeActiveInactiveHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IAccountsBookSetup interface
    public interface IEmployeeActiveInactiveHistoryService
    {
        EmployeeActiveInactiveHistory Get(int id, int nUserID);
        List<EmployeeActiveInactiveHistory> Gets(int nUserID);
        List<EmployeeActiveInactiveHistory> Gets(string sSQL, int nUserID);
    }
    #endregion
}

