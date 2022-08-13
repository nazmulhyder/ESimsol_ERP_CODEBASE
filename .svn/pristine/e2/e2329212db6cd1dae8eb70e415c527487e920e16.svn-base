using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TransferRequisitionSlipDA
    {
        public TransferRequisitionSlipDA() { }

        #region Insert Function   
        public static IDataReader InsertUpdate(TransactionContext tc, TransferRequisitionSlip oRS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferRequisitionSlip]" + "%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s, %d, %s, %s, %s,%s, %n, %n, %n, %n",
                                     oRS.TRSID, oRS.TRSNO, oRS.RequisitionTypeInt, oRS.TransferStatusInt, oRS.IssueBUID, oRS.IssueWorkingUnitID, oRS.ReceivedBUID, oRS.ReceivedWorkingUnitID, oRS.PreparedByID, oRS.AuthorisedByID, oRS.Remark, oRS.IssueDateTime, oRS.VehicleNo, oRS.DriverName, oRS.GatePassNo, oRS.ChallanNo, oRS.DisburseByUserID,  oRS.ReceivedByUserID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, TransferRequisitionSlip oRS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TransferRequisitionSlip]" + "%n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s, %d, %s, %s, %s,%s, %n, %n, %n, %n",
                                     oRS.TRSID, oRS.TRSNO, oRS.RequisitionTypeInt, oRS.TransferStatusInt, oRS.IssueBUID, oRS.IssueWorkingUnitID, oRS.ReceivedBUID, oRS.ReceivedWorkingUnitID, oRS.PreparedByID, oRS.AuthorisedByID, oRS.Remark, oRS.IssueDateTime, oRS.VehicleNo, oRS.DriverName, oRS.GatePassNo, oRS.ChallanNo, oRS.DisburseByUserID, oRS.ReceivedByUserID, nUserID, (int)eEnumDBOperation);
        }
        public static void Approved(TransactionContext tc, TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE TransferRequisitionSlip SET AuthorisedByID = %n, TransferStatus=%n WHERE TRSID=%n", nUserID, (int)EnumSubContractStatus.Approved, oTransferRequisitionSlip.TRSID);
        }
        public static void UndoApproved(TransactionContext tc, TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE TransferRequisitionSlip SET AuthorisedByID = 0, TransferStatus=%n WHERE TRSID=%n",  (int)EnumSubContractStatus.Initialized, oTransferRequisitionSlip.TRSID);
        }
        public static IDataReader Disburse(TransactionContext tc, TransferRequisitionSlip oRS, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_TransferRequsition_Disburse]" + "%n, %n",  oRS.TRSID, nUserID);
        }
        public static IDataReader Received(TransactionContext tc, TransferRequisitionSlip oRS, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_TransferRequsition_Received]" + "%n, %n", oRS.TRSID, nUserID);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, TransferRequisitionSlip oTransferRequisitionSlip)
        {
            tc.ExecuteNonQuery(" Update TransferRequisitionSlip SET IsWillVoucherEffect = %b WHERE TRSID  = %n", oTransferRequisitionSlip.IsWillVoucherEffect, oTransferRequisitionSlip.TRSID);
        }  
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, Int64 nTRSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferRequisitionSlip WHERE TRSID=%n", nTRSID);
        }
        public static IDataReader Gets(TransactionContext tc,string sSQL, Int64 nUserID)
        {
            return tc.ExecuteReader(sSQL);
        } 
        #endregion
    }
}
