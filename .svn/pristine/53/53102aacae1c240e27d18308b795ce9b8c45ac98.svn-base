using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FNReProRequest
    public class FNReProRequest : BusinessObject
    {
        public FNReProRequest()
        {
            FNReProRequestID = 0;
            ReqNo = "";
            RequestByID = 0;
            RequestDate = DateTime.Now;
            ApproveBy = 0;
            ApproveDate = DateTime.Now;
            Status = EnumFNReProRequestStatus.Initialize;
            Note = "";
            Note_Approve = "";
            RequestByName = "";
            ApproveByName = "";
            ErrorMessage = "";
            FNReProRequestDetails = new List<FNReProRequestDetail>();
        }

        #region Property
        public int FNReProRequestID { get; set; }
        public string ReqNo { get; set; }
        public int RequestByID { get; set; }
        public DateTime RequestDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public EnumFNReProRequestStatus Status { get; set; }
        public string Note { get; set; }
        public string Note_Approve { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string RequestByName { get; set; }
        public string ApproveByName { get; set; }
        public string RequestDateInString
        {
            get
            {
                if (RequestDate == DateTime.MinValue) return "";
                return RequestDate.ToString("dd MMM yyyy");
            }
        }
        public string ApproveDateInString
        {
            get
            {
                if (ApproveDate == DateTime.MinValue) return "";
                return ApproveDate.ToString("dd MMM yyyy");
            }
        }
        public string StatusInString
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }

        public List<FNReProRequestDetail> FNReProRequestDetails { get; set; }
        #endregion

        #region Functions
        public static List<FNReProRequest> Gets(long nUserID)
        {
            return FNReProRequest.Service.Gets(nUserID);
        }
        public static List<FNReProRequest> Gets(string sSQL, long nUserID)
        {
            return FNReProRequest.Service.Gets(sSQL, nUserID);
        }
        public FNReProRequest Get(int id, long nUserID)
        {
            return FNReProRequest.Service.Get(id, nUserID);
        }
        public FNReProRequest Save(long nUserID)
        {
            return FNReProRequest.Service.Save(this, nUserID);
        }
        public FNReProRequest Approve(long nUserID)
        {
            return FNReProRequest.Service.Approve(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FNReProRequest.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNReProRequestService Service
        {
            get { return (IFNReProRequestService)Services.Factory.CreateService(typeof(IFNReProRequestService)); }
        }
        #endregion
    }
    #endregion

    #region IFNReProRequest interface
    public interface IFNReProRequestService
    {
        FNReProRequest Get(int id, Int64 nUserID);
        List<FNReProRequest> Gets(Int64 nUserID);
        List<FNReProRequest> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FNReProRequest Save(FNReProRequest oFNReProRequest, Int64 nUserID);
        FNReProRequest Approve(FNReProRequest oFNReProRequest, Int64 nUserID);

    }
    #endregion
}
