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
    #region SampleRequest
    public class SampleRequest : BusinessObject
    {
        public SampleRequest()
        {
            SampleRequestID = 0;
            RequestNo = "";
            RequestDate = DateTime.Now;
            RequestBy = 0;
            Remarks = "";
            RequestTo = 0;
            Note = "";
            ContractorID = 0;
            ContactPersonID = 0;
            WorkingUnitID = 0;
            ApprovedBy = 0;
            DisbursedBy = 0;
            ApprovedByName = "";
            DisbursedByName = "";
            WUName = "";
            WorkingUnits = new List<WorkingUnit>();
            ErrorMessage = "";
            //RequestType = EnumProductNature.Hanger;
            SampleRequestDetails = new List<SampleRequestDetail>();
      
            
        }

        #region Property
        public int SampleRequestID { get; set; }
        public int WorkingUnitID { get; set; }
        public int ApprovedBy { get; set; }
        public int DisbursedBy { get; set; }
 
        public string ApprovedByName { get; set; }
        public string DisbursedByName { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestBy { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string RequestNo { get; set; }
        public string Note { get; set; }
        public int RequestTo { get; set; }
        public string WUName { get; set; }
        public EnumProductNature RequestType { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<WorkingUnit> WorkingUnits { get; set; }
        public string RequestByName { get; set; }
        public string RequestToName { get; set; }
        public string ContractorName { get; set; }
        public string ContactPersonName { get; set; }
        public SampleRequestDetail SampleRequestDetail { get; set; }
    

        public string RequestDateInString
        {
            get
            {
                return RequestDate.ToString("dd MMM yyyy");
            }
        }
        public string RequestTypeInString
        {
            get
            {
                return EnumObject.jGet(this.RequestType);
            }
        }
        public int RequestTypeInt
        {
            get
            {
                return (int)RequestType;
            }
        }

        public List<SampleRequestDetail> SampleRequestDetails { get; set; }
        #endregion

        #region Functions
        public static List<SampleRequest> Gets(long nUserID)
        {
            return SampleRequest.Service.Gets(nUserID);
        }
        public static List<SampleRequest> Gets(string sSQL, long nUserID)
        {
            return SampleRequest.Service.Gets(sSQL, nUserID);
        }
        public SampleRequest Get(int id, long nUserID)
        {
            return SampleRequest.Service.Get(id, nUserID);
        }
        public SampleRequest Approved(int nUserID)
        {
            return SampleRequest.Service.Approved(this, nUserID);
        }

        public SampleRequest UndoApproved(int nUserID)
        {
            return SampleRequest.Service.UndoApproved(this, nUserID);
        }

        public SampleRequest Save(long nUserID)
        {
            return SampleRequest.Service.Save(this, nUserID);
        }
        public SampleRequest Commit(long nUserID)
        {
            return SampleRequest.Service.Commit(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return SampleRequest.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISampleRequestService Service
        {
            get { return (ISampleRequestService)Services.Factory.CreateService(typeof(ISampleRequestService)); }
        }
        #endregion
    }
    #endregion

    #region ISampleRequest interface
    public interface ISampleRequestService
    {
        SampleRequest Get(int id, Int64 nUserID);
        List<SampleRequest> Gets(Int64 nUserID);
        List<SampleRequest> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SampleRequest Approved(SampleRequest oSampleRequest, int nUserID);
        SampleRequest UndoApproved(SampleRequest oSampleRequest, int nUserID);
        SampleRequest Save(SampleRequest oSampleRequest, Int64 nUserID);
        SampleRequest Commit(SampleRequest oSampleRequest, Int64 nUserID);

    }
    #endregion
}
