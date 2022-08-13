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

    #region ReviseRequest
    
    public class ReviseRequest : BusinessObject
    {
        public ReviseRequest()
        {

            ReviseRequestID =0;
            OperationType = EnumReviseRequestOperationType.None;
            OperationObjectID =0;
            RequestBy = 0;
            RequestTo = 0;
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ReviseRequestID { get; set; }
         
        public EnumReviseRequestOperationType OperationType { get; set; }
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

        public static List<ReviseRequest> Gets(long nUserID)
        {
            return ReviseRequest.Service.Gets( nUserID);
        }
        public static List<ReviseRequest> Gets(string sSQL, long nUserID)
        {
            return ReviseRequest.Service.Gets(sSQL, nUserID);
        }
        public ReviseRequest Get(int id, long nUserID)
        {
            return ReviseRequest.Service.Get(id, nUserID);
        }
        public ReviseRequest Save(long nUserID)
        {
            return ReviseRequest.Service.Save(this, nUserID);
        }
        public string ReviseRequestSave(List<ReviseRequest> oReviseRequests, long nUserID)
        {
            return ReviseRequest.Service.ReviseRequestSave(oReviseRequests, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ReviseRequest.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IReviseRequestService Service
        {
            get { return (IReviseRequestService)Services.Factory.CreateService(typeof(IReviseRequestService)); }
        }
        #endregion
    }
    #endregion

    #region IReviseRequest interface
     
    public interface IReviseRequestService
    {
         
        ReviseRequest Get(int id, Int64 nUserID);
         
        List<ReviseRequest> Gets(Int64 nUserID);
         
        List<ReviseRequest> Gets(string sSQL, Int64 nUserID);
         
        ReviseRequest Save(ReviseRequest oReviseRequest, Int64 nUserID);
         
        string ReviseRequestSave(List<ReviseRequest> oReviseRequests, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion
    

}
