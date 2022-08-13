using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region RequisitionSlip

    public class TransferRequisitionSlip : BusinessObject
    {
        public TransferRequisitionSlip()
        {
            TRSID = 0;
            TRSNO = "";
            RequisitionType = EnumRequisitionType.None;
            RequisitionTypeInt = 0;
            TransferStatus = EnumTransferStatus.Initialized;
            TransferStatusInt = 0;
            IssueBUID = 0;
            IssueWorkingUnitID = 0;
            ReceivedBUID = 0;
            ReceivedWorkingUnitID = 0;
            PreparedByID = 0;
            AuthorisedByID = 0;
            Remark = "";
            IssueDateTime = DateTime.Now;
            VehicleNo = "";
            DriverName = "";
            GatePassNo = "";
            DisburseByUserID = 0;
            ReceivedByUserID = 0;
            IssueStoreName = "";
            ReceivedStoreName = "";
            PreparedByName = "";
            AythorisedByName = "";
            IsWillVoucherEffect = true;
            TRSRefType = EnumTRSRefType.Open;
            TRSRefTypeInt = 1;
            TRSRefID = 0;
            ReceivedByName = "";
            IssueBUName = "";
            IssueBUShortName = "";
            ReceivedBUName = "";
            ReceivedBUShortName = "";
            TotalQty = 0;
            MUName = "";
            DisbursedByName = "";
            ChallanNo = "";
            FullTRSNO = "";
            BusinessUnit = new BusinessObjects.BusinessUnit();
            TransferRequisitionSlips = new List<TransferRequisitionSlip>();
            ErrorMessage = "";
        }

        #region Properties
        public int TRSID { get; set; }
        public string TRSNO { get; set; }
        public string FullTRSNO { get; set; }
        public EnumRequisitionType RequisitionType { get; set; }
        public int RequisitionTypeInt { get; set; }
        public EnumTransferStatus TransferStatus { get; set; }
        public int TransferStatusInt { get; set; } 
        public int IssueBUID { get; set; }
        public int IssueWorkingUnitID { get; set; }
        public int ReceivedBUID { get; set; }
        public int ReceivedWorkingUnitID { get; set; }
        public int PreparedByID { get; set; }
        public int AuthorisedByID { get; set; }
        public string Remark { get; set; }
        public DateTime IssueDateTime { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string GatePassNo { get; set; }
        public int DisburseByUserID { get; set; }
        public int ReceivedByUserID { get; set; }
        public string IssueStoreName { get; set; }
        public string ReceivedStoreName { get; set; }
        public string IssueStoreNameWitoutLocation { get; set; }
        public string ReceivedStoreNameWitoutLocation { get; set; }
        public string PreparedByName { get; set; }
        public string AythorisedByName { get; set; }
        public string DisbursedByName { get; set; }
        public string ReceivedByName { get; set; }
        public string IssueBUName { get; set; }
        public string IssueBUShortName { get; set; }
        public string ReceivedBUName { get; set; }
        public string ReceivedBUShortName { get; set; }
        public double TotalQty { get; set; }
        public string MUName { get; set; }
        public string ChallanNo { get; set; }
        public bool IsWillVoucherEffect { get; set; }
        public EnumTRSRefType TRSRefType { get; set; }
        public int TRSRefTypeInt { get; set; }
        public int TRSRefID { get; set; }
        public string Param { get; set; }
        public string ErrorMessage { get; set; }


        public int OPT { get; set; } //0 Means Issue & 1 Means Received

        #region Derive Property
        public int TRSDetailIDForAutoLot { get; set; }
        public List<TransferRequisitionSlipDetail> TransferRequisitionSlipDetails { get; set; }
        public Company Company { get; set; }
        public string RequisitionTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RequisitionType);
            }
        }
        public string TransferStatusSt
        {
            get
            {
                return EnumObject.jGet(this.TransferStatus);
            }
        }
        public string IssueDateTimeSt
        {
            get
            {
                return IssueDateTime.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #endregion
        #region Derived Property
        public BusinessUnit BusinessUnit { get; set; }
        public List<TransferRequisitionSlip> TransferRequisitionSlips { get; set; }
        public string IsWillVoucherEffectSt
        {
            get
            {
                if (this.IsWillVoucherEffect)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
        #endregion

        #region Functions
        public TransferRequisitionSlip Get(int nTRSID, long nUserID)
        {
            return TransferRequisitionSlip.Service.Get(nTRSID, nUserID);
        }
        public string Delete(int nTRSID, long nUserID)
        {
            return TransferRequisitionSlip.Service.Delete(nTRSID, nUserID);
        }
        public TransferRequisitionSlip Save(long nUserID)
        {
            return TransferRequisitionSlip.Service.Save(this, nUserID);
        }
        public TransferRequisitionSlip Approved(long nUserID)
        {
            return TransferRequisitionSlip.Service.Approved(this, nUserID);
        }

        public TransferRequisitionSlip UndoApproved(long nUserID)
        {
            return TransferRequisitionSlip.Service.UndoApproved(this, nUserID);
        }
        public TransferRequisitionSlip Disburse(long nUserID)
        {
            return TransferRequisitionSlip.Service.Disburse(this, nUserID);
        }
        public TransferRequisitionSlip UpdateVoucherEffect(long nUserID)
        {
            return TransferRequisitionSlip.Service.UpdateVoucherEffect(this, nUserID);
        }
        public TransferRequisitionSlip Received(long nUserID)
        {
            return TransferRequisitionSlip.Service.Received(this, nUserID);
        }
        public static List<TransferRequisitionSlip> Gets(string sSQL, long nUserID)
        {
            return TransferRequisitionSlip.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITransferRequisitionSlipService Service
        {
            get { return (ITransferRequisitionSlipService)Services.Factory.CreateService(typeof(ITransferRequisitionSlipService)); }
        }
        #endregion
    }
    #endregion

    #region IRequisitionSlip interface

    public interface ITransferRequisitionSlipService
    {
        TransferRequisitionSlip Get(int nTRSID, Int64 nUserId);
        string Delete(int nTRSID, Int64 nUserId);
        List<TransferRequisitionSlip> Gets(string sSQL, Int64 nUserID);
        TransferRequisitionSlip Save(TransferRequisitionSlip oRequisitionSlip, Int64 nUserID);
        TransferRequisitionSlip Approved(TransferRequisitionSlip oTransferRequisitionSlip, long nUserID);
        TransferRequisitionSlip UndoApproved(TransferRequisitionSlip oTransferRequisitionSlip, long nUserID);
        TransferRequisitionSlip Disburse(TransferRequisitionSlip oTransferRequisitionSlip, long nUserID);
        TransferRequisitionSlip Received(TransferRequisitionSlip oTransferRequisitionSlip, long nUserID);
        TransferRequisitionSlip UpdateVoucherEffect(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID);   
    }
    #endregion
}
