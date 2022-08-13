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
    #region PerformanceIncentive

    public class PerformanceIncentive : BusinessObject
    {
        public PerformanceIncentive()
        {
            PIID = 0;
            Code="";
            Name="";
            Description="";
            SalaryHeadID=0;
            ApproveBy=0;
            ApproveDate=DateTime.Now;
            InactiveBy = 0;
            InactiveDate = DateTime.Now;
            ErrorMessage = "";
            EncryptPIID = "";
            SalaryHeadName = "";
            InactiveByName = "";
            ApproveByName = "";
        }

        #region Properties
        public int PIID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SalaryHeadID { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int InactiveBy { get; set; }
        public DateTime InactiveDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PerformanceIncentive> PerformanceIncentives { get; set; }
        public List<PerformanceIncentiveMember> PerformanceIncentiveMembers { get; set; }
        public List<PerformanceIncentiveSlab> PerformanceIncentiveSlabs { get; set; }
        public string Activity { get { if (InactiveBy==0)return "Active"; else return "Inactive"; } }
        public string EncryptPIID { get; set; }
        public string SalaryHeadName { get; set; }
        public string InactiveByName { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDateInString
        {
            get
            {
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static PerformanceIncentive Get(int Id, long nUserID)
        {
            return PerformanceIncentive.Service.Get(Id, nUserID);
        }
        public static PerformanceIncentive Get(string sSQL, long nUserID)
        {
            return PerformanceIncentive.Service.Get(sSQL, nUserID);
        }
        public static List<PerformanceIncentive> Gets(long nUserID)
        {
            return PerformanceIncentive.Service.Gets(nUserID);
        }

        public static List<PerformanceIncentive> Gets(string sSQL, long nUserID)
        {
            return PerformanceIncentive.Service.Gets(sSQL, nUserID);
        }

        public PerformanceIncentive IUD(int nDBOperation, long nUserID)
        {
            return PerformanceIncentive.Service.IUD(this, nDBOperation, nUserID);
        }

        public static PerformanceIncentive InActive(int nId, long nUserID)
        {
            return PerformanceIncentive.Service.InActive(nId, nUserID);
        }

        public static PerformanceIncentive Approve(int nId, long nUserID)
        {
            return PerformanceIncentive.Service.Approve(nId, nUserID);
        }

        public static PerformanceIncentive PerformanceIncentive_Transfer(int PreviousPIID, int PresentPIID, long nUserID)
        {
            return PerformanceIncentive.Service.PerformanceIncentive_Transfer(PreviousPIID, PresentPIID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPerformanceIncentiveService Service
        {
            get { return (IPerformanceIncentiveService)Services.Factory.CreateService(typeof(IPerformanceIncentiveService)); }
        }

        #endregion
    }
    #endregion

    #region IPerformanceIncentive interface

    public interface IPerformanceIncentiveService
    {
        PerformanceIncentive Get(int id, Int64 nUserID);
        PerformanceIncentive Get(string sSQL, Int64 nUserID);
        List<PerformanceIncentive> Gets(Int64 nUserID);
        List<PerformanceIncentive> Gets(string sSQL, Int64 nUserID);
        PerformanceIncentive IUD(PerformanceIncentive oPerformanceIncentive, int nDBOperation, Int64 nUserID);
        PerformanceIncentive InActive(int nId, Int64 nUserID);
        PerformanceIncentive Approve(int nId, Int64 nUserID);
        PerformanceIncentive PerformanceIncentive_Transfer(int PreviousPIID, int PresentPIID, Int64 nUserID);
    }
    #endregion
}
