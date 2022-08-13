using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region EmployeeProcessMailHistory
    
    public class EmployeeProcessMailHistory : BusinessObject
    {
        public EmployeeProcessMailHistory()
        {
            EPMHID = 0;
            PPMID = 0;
            EmployeeID = 0;
            IsStatus = false;
            FeedBackMessage = "";
            SendingTime = DateTime.MinValue;
            OperatedBy = 0;
            EmployeeName = "";
            EmployeeCode = "";
            Email = "";
            OperatedByName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int EPMHID { get; set; }
        public int PPMID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsStatus { get; set; }
        public string FeedBackMessage { get; set; }
        public DateTime SendingTime { get; set; }
        public int OperatedBy { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Email { get; set; }
        public string OperatedByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SendingTimeSt
        {
            get
            {
                return SendingTime.ToString("dd MMM yyyy (hh:mm tt)");
            }
        }
     
        #endregion

        #region Functions

        public static List<EmployeeProcessMailHistory> Gets(long nUserID)
        {
            return EmployeeProcessMailHistory.Service.Gets(nUserID);
        }
        public static List<EmployeeProcessMailHistory> Gets(string sSQL, Int64 nUserID)
        {
            return EmployeeProcessMailHistory.Service.Gets(sSQL, nUserID);
        }
        public EmployeeProcessMailHistory Get(int nId, long nUserID)
        {
            return EmployeeProcessMailHistory.Service.Get(nId,nUserID);
        }
        public EmployeeProcessMailHistory Save(long nUserID)
        {
            return EmployeeProcessMailHistory.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return EmployeeProcessMailHistory.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeProcessMailHistoryService Service
        {
            get { return (IEmployeeProcessMailHistoryService)Services.Factory.CreateService(typeof(IEmployeeProcessMailHistoryService)); }
        }
        #endregion
    }
    #endregion

    #region IEmployeeProcessMailHistory interface
    
    public interface IEmployeeProcessMailHistoryService
    {
        EmployeeProcessMailHistory Get(int id, long nUserID);
        List<EmployeeProcessMailHistory> Gets(long nUserID);
        List<EmployeeProcessMailHistory> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        EmployeeProcessMailHistory Save(EmployeeProcessMailHistory oEmployeeProcessMailHistory, long nUserID);
    }
    #endregion
}

