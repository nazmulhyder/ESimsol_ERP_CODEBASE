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
    #region EmployeeCard

    public class EmployeeCard : BusinessObject
    {
        public EmployeeCard()
        {
            EmployeeCardID = 0;
            EmployeeID = 0;
            EmployeeCardStatus = EnumEmployeeCardStatus.None;
            IssueDate = DateTime.Now;
            ExpireDate = DateTime.Now;
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties

        public int EmployeeCardID { get; set; }
        public int EmployeeID { get; set; }
        public EnumEmployeeCardStatus EmployeeCardStatus { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        #region Derived Property
        public Company Company { get; set; }
        public List<Employee> Employees { get; set; }
        public string EmployeeNameCode { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }

        public string ExpireDateInString
        {
            get
            {
                return ExpireDate.ToString("dd MMM yyyy");
            }
        }

        public int EmployeeCardStatusInt;
        public string EmployeeCardStatusInString
        {
            get
            {
                return EmployeeCardStatus.ToString();
            }
        }

        #endregion

        #region Functions
        public static EmployeeCard Get(int id, long nUserID)
        {
            return EmployeeCard.Service.Get(id, nUserID);
        }
        public static EmployeeCard Get(string sSQL, long nUserID)
        {
            return EmployeeCard.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeCard> Gets(long nUserID)
        {
            return EmployeeCard.Service.Gets(nUserID);
        }
        public static List<EmployeeCard> Gets(string sSQL, long nUserID)
        {
            return EmployeeCard.Service.Gets(sSQL, nUserID);
        }
        public EmployeeCard IUD(long nUserID)
        {
            return EmployeeCard.Service.IUD(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeCardService Service
        {
            get { return (IEmployeeCardService)Services.Factory.CreateService(typeof(IEmployeeCardService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeCard interface

    public interface IEmployeeCardService
    {

        EmployeeCard Get(int id, Int64 nUserID);
        EmployeeCard Get(string sSQL, Int64 nUserID);
        List<EmployeeCard> Gets(Int64 nUserID);
        List<EmployeeCard> Gets(string sSQL, Int64 nUserID);
        EmployeeCard IUD(EmployeeCard oEmployeeCardSheet, Int64 nUserID);

    }
    #endregion
}
