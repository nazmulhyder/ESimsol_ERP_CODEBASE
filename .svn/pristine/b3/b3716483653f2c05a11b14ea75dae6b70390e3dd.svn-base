using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
	public class FNRequisitionDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, FNRequisition oFNRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNRequisition]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%d,%n,%n, %n, %n,%n",
                                    oFNRequisition.FNRID, oFNRequisition.TreatmentID, oFNRequisition.ShiftID, oFNRequisition.FNExODetailID, oFNRequisition.Shade, oFNRequisition.WorkingUnitID, oFNRequisition.Remarks, oFNRequisition.RequestDate, oFNRequisition.WorkingUnitReceiveID, oFNRequisition.MachineID, oFNRequisition.IsRequisitionOpen, nUserID, (int)eEnumDBOperation);
		}
		public static void Delete(TransactionContext tc, FNRequisition oFNRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNRequisition]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%d,%n,%n, %n, %n,%n",
                                    oFNRequisition.FNRID, oFNRequisition.TreatmentID, oFNRequisition.ShiftID, oFNRequisition.FNExODetailID, oFNRequisition.Shade, oFNRequisition.WorkingUnitID, oFNRequisition.Remarks, oFNRequisition.RequestDate, oFNRequisition.WorkingUnitReceiveID, oFNRequisition.MachineID, oFNRequisition.IsRequisitionOpen, nUserID, (int)eEnumDBOperation);
        }
        public static void Approval(TransactionContext tc, int nFNRID, bool bIsApprove, Int64 nUserID)
        {
            if (bIsApprove)
            {
                tc.ExecuteNonQuery("Update FNRequisition SET ApproveBy = %n , ApproveDate  = %d WHERE FNRID = %n", nUserID, DateTime.Today, nFNRID);
            }
            else
            {
                tc.ExecuteNonQuery("Update FNRequisition SET ApproveBy = 0 , ApproveDate  = null WHERE FNRID = %n", nFNRID);
            }
        }
        public static void Received(TransactionContext tc, int nFNRID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update FNRequisition SET ReceiveBy = %n , ReceiveDate  = %d WHERE FNRID = %n", nUserID, DateTime.Today, nFNRID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FNRequisition WHERE FNRID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNRequisition");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
