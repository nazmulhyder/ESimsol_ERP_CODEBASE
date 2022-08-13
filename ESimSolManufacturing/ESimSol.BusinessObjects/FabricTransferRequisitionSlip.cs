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
    #region FabricTransferRequisitionSlip
    public class FabricTransferRequisitionSlip : BusinessObject
    {
        public FabricTransferRequisitionSlip()
        {
            FabricTRSID = 0;
            TRSNO = "";
            RequisitionType = 0;
            BUIDIssue = 0;
            WorkingUnitIDIssue = 0;
            BUIDReceive = 0;
            WorkingUnitIDReceive = 0;
            PrepareBy = 0;
            ApproveBy = 0;
            Remarks = "";
            IssueDateTime = DateTime.MinValue;
            VehicleNo = "";
            DriverName = "";
            GatePassNo = "";
            DisburseBy = 0;
            ReceivedBy = 0;
            LocationNameIssue = "";
            LocationShortNameIssue = "";
            OperationUnitNameIssue = "";
            LocationNameReceive = "";
            LocationShortNameReceive = "";
            OperationUnitNameReceive = "";
            FabricTransferRequisitionSlipDetails = new List<FabricTransferRequisitionSlipDetail>();
            ErrorMessage = "";
        }

        #region Property
        public int FabricTRSID { get; set; }
        public string TRSNO { get; set; }
        public int RequisitionType { get; set; }
        public int BUIDIssue { get; set; }
        public int WorkingUnitIDIssue { get; set; }
        public int BUIDReceive { get; set; }
        public int WorkingUnitIDReceive { get; set; }
        public int PrepareBy { get; set; }
        public int ApproveBy { get; set; }
        public string Remarks { get; set; }
        public DateTime IssueDateTime { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string GatePassNo { get; set; }
        public int DisburseBy { get; set; }
        public int ReceivedBy { get; set; }
        public string LocationNameIssue { get; set; }
        public string LocationShortNameIssue { get; set; }
        public string OperationUnitNameIssue { get; set; }
        public string LocationNameReceive { get; set; }
        public string LocationShortNameReceive { get; set; }
        public string OperationUnitNameReceive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<FabricTransferRequisitionSlipDetail> FabricTransferRequisitionSlipDetails { get; set; }
        public string IssueDateTimeInString
        {
            get
            {
                if (IssueDateTime == DateTime.MinValue) return "";
                return IssueDateTime.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricTransferRequisitionSlip> Gets(long nUserID)
        {
            return FabricTransferRequisitionSlip.Service.Gets(nUserID);
        }
        public static List<FabricTransferRequisitionSlip> Gets(string sSQL, long nUserID)
        {
            return FabricTransferRequisitionSlip.Service.Gets(sSQL, nUserID);
        }
        public FabricTransferRequisitionSlip Get(int id, long nUserID)
        {
            return FabricTransferRequisitionSlip.Service.Get(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricTransferRequisitionSlipService Service
        {
            get { return (IFabricTransferRequisitionSlipService)Services.Factory.CreateService(typeof(IFabricTransferRequisitionSlipService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricTransferRequisitionSlip interface
    public interface IFabricTransferRequisitionSlipService
    {
        FabricTransferRequisitionSlip Get(int id, Int64 nUserID);
        List<FabricTransferRequisitionSlip> Gets(Int64 nUserID);
        List<FabricTransferRequisitionSlip> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
