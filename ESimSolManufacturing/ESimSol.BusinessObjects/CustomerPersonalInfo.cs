using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;


namespace ESimSol.BusinessObjects
{
    public class CustomerPersonalInfo:BusinessObject
    {
        public CustomerPersonalInfo()
        {
            CustomerPersonalInfoID = 0;
            CustomerID = 0;
            CustomerName = "";
            EmployeerName = "";
            Designation = "";
            Address = "";
            ContactNumber = "";
            EmailAddress = "";
            DateOfBirth = DateTime.Now;
            MarriedStatus = EnumMarriedStatus.None;
            SpouseName = "";
            SpouseDateOfBirth = DateTime.Now;
            AnniversaryDate = DateTime.Now;
            Remarks = "";
            ErrorMessage = "";
            ContractorName = "";
        }
        #region Properties
        public int CustomerPersonalInfoID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string EmployeerName { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public EnumMarriedStatus MarriedStatus { get; set; }
        public string SpouseName { get; set; }
        public DateTime SpouseDateOfBirth { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string ContractorName { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public string MarriedStatusSt
        {
            get{
                return EnumObject.jGet(this.MarriedStatus);
            }            
        }
        public string DateOfBirthSt
        {
            get
            {
                return DateOfBirth.ToString("dd MMM yyyy");
            }
        }
        public string SpouseDateOfBirthSt
        {
            get
            {
                return SpouseDateOfBirth.ToString("dd MMM yyyy");
            }
        }
        public string AnniversaryDateSt
        {
            get
            {
                return AnniversaryDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region Function
        public static List<CustomerPersonalInfo> Gets(long nUserID)
        {
            return CustomerPersonalInfo.Service.Gets(nUserID);
        }
        public static List<CustomerPersonalInfo> Gets(string sSQL, long nUserID)
        {
            return CustomerPersonalInfo.Service.Gets(sSQL, nUserID);
        }
        public CustomerPersonalInfo Get(int id, long nUserID)
        {
            return CustomerPersonalInfo.Service.Get(id, nUserID);
        }
        public CustomerPersonalInfo Save(long nUserID)
        {
            return CustomerPersonalInfo.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return CustomerPersonalInfo.Service.Delete(id, nUserID);
        }
         #endregion

        #region ServiceFactory
        internal static ICustomerPersonalInfoService Service
        {
            get { return (ICustomerPersonalInfoService)Services.Factory.CreateService(typeof(ICustomerPersonalInfoService)); }
        }
        #endregion
    }
    public interface ICustomerPersonalInfoService 
    {
        CustomerPersonalInfo Get(int nid, long nUserID);
        List<CustomerPersonalInfo> Gets(long nUserID);
        List<CustomerPersonalInfo> Gets(string sSql, long nUserID);
        string Delete(int id, long nUserID);
        CustomerPersonalInfo Save(CustomerPersonalInfo oCustomerPersonalInfo, long nUserID);

    }
}
