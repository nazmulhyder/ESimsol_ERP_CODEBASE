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
    #region ApprovalHistory
    public class ApprovalHistory
    {
        public ApprovalHistory()
        {
            ApprovalHistoryID =0;
            ObjectRefID =0;
            ApprovalHeadID =0;
            SendToPersonID =0;
            Note ="";
            ApprovalHeadName = "";
            SendToPersonName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ApprovalHistoryID { get; set; }
        public int ObjectRefID { get; set; }
        public int ApprovalHeadID { get; set; }
        public int SendToPersonID { get; set; }
        public string Note { get; set; }
        public string ApprovalHeadName { get; set; }
        public string SendToPersonName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions


        public static List<ApprovalHistory> Gets(string sSQL, long nUserID)
        {
            return ApprovalHistory.Service.Gets(sSQL, nUserID);
        }
        public static ApprovalHistory Get(string sSQL, long nUserID)
        {
            return ApprovalHistory.Service.Get(sSQL, nUserID);
        }
        public ApprovalHistory IUD(int nDBOperation, long nUserID)
        {
            return ApprovalHistory.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IApprovalHistoryService Service
        {
            get { return (IApprovalHistoryService)Services.Factory.CreateService(typeof(IApprovalHistoryService)); }
        }
        #endregion


    }
    #endregion

    #region IApprovalHistory interface

    public interface IApprovalHistoryService
    {
        List<ApprovalHistory> Gets(string sSQL, Int64 nUserID);
        ApprovalHistory Get(string sSQL, Int64 nUserID);
        ApprovalHistory IUD(ApprovalHistory oApprovalHistory, int nDBOperation, Int64 nUserID);
       
      
    }
    #endregion
}


