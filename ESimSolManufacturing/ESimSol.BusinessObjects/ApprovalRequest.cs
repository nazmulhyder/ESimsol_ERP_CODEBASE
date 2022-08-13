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

    #region ApprovalRequest
    
    public class ApprovalRequest : BusinessObject
    {
        public ApprovalRequest()
        {

            ApprovalRequestID =0;
            OperationType = EnumApprovalRequestOperationType.None;
            OperationObjectID =0;
            RequestBy =0;
            RequestTo = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ApprovalRequestID { get; set; }
         
        public EnumApprovalRequestOperationType OperationType { get; set; }
        
         
        public int OperationObjectID { get; set; }

         
        public int RequestBy { get; set; }
         
        public int RequestTo { get; set; }

         
        public string Note { get; set; }

         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int OperationTypeInInt { get; set; }

        public string OperationTypeInString
        {
            get
            {
                return OperationType.ToString();
            }
        }
        public List<User> UserList { get; set; }

        #endregion

        #region Functions

        public static List<ApprovalRequest> Gets(long nUserID)
        {
            return ApprovalRequest.Service.Gets( nUserID);
        }
        public static List<ApprovalRequest> Gets(string sSQL, long nUserID)
        {
            return ApprovalRequest.Service.Gets(sSQL, nUserID);
        }
        public ApprovalRequest Get(int id, long nUserID)
        {
            return ApprovalRequest.Service.Get(id, nUserID);
        }
        public ApprovalRequest Save(long nUserID)
        {
            return ApprovalRequest.Service.Save(this, nUserID);
        }
        public string ApprovalRequestSave(List<ApprovalRequest> oApprovalRequests, long nUserID)
        {
            return ApprovalRequest.Service.ApprovalRequestSave(oApprovalRequests, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ApprovalRequest.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IApprovalRequestService Service
        {
            get { return (IApprovalRequestService)Services.Factory.CreateService(typeof(IApprovalRequestService)); }
        }
        #endregion
    }
    #endregion

    #region IApprovalRequest interface
     
    public interface IApprovalRequestService
    {
         
        ApprovalRequest Get(int id, Int64 nUserID);
         
        List<ApprovalRequest> Gets(Int64 nUserID);
         
        List<ApprovalRequest> Gets(string sSQL, Int64 nUserID);
         
        ApprovalRequest Save(ApprovalRequest oApprovalRequest, Int64 nUserID);
         
        string ApprovalRequestSave(List<ApprovalRequest> oApprovalRequests, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion
    

}
