using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class GRNDA
    {
        public GRNDA() { }
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GRN oGRN, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GRN]" + "%n, %s, %d, %d, %n, %n, %n, %n, %n, %s, %n, %n, %n, %d, %s, %d, %n,%s,%s,%s, %s,%d, %n, %n",
                                    oGRN.GRNID, oGRN.GRNNo, oGRN.GRNDate, oGRN.GLDate, oGRN.GRNTypeInt, oGRN.GRNStatusInt, oGRN.BUID, oGRN.ContractorID, oGRN.RefObjectID, oGRN.Remarks, oGRN.CurrencyID, oGRN.ConversionRate, oGRN.ApproveBy, oGRN.ApproveDate, oGRN.ChallanNo, oGRN.ChallanDate, oGRN.StoreID, oGRN.GatePassNo, oGRN.VehicleNo, oGRN.MRFNo,  oGRN.MRIRNo, oGRN.MRIRDate,  (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, GRN oGRN, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GRN]" + "%n, %s, %d, %d, %n, %n, %n, %n, %n, %s, %n, %n, %n, %d, %s, %d, %n,%s,%s,%s, %s,%d, %n, %n",
                                    oGRN.GRNID, oGRN.GRNNo, oGRN.GRNDate, oGRN.GLDate, oGRN.GRNTypeInt, oGRN.GRNStatusInt, oGRN.BUID, oGRN.ContractorID, oGRN.RefObjectID, oGRN.Remarks, oGRN.CurrencyID, oGRN.ConversionRate, oGRN.ApproveBy, oGRN.ApproveDate, oGRN.ChallanNo, oGRN.ChallanDate, oGRN.StoreID, oGRN.GatePassNo, oGRN.VehicleNo, oGRN.MRFNo, oGRN.MRIRNo, oGRN.MRIRDate, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Update and Status Change
        public static IDataReader Approve(TransactionContext tc, GRN oGRN, Int64 nUserID)
        {
            return tc.ExecuteReader("UPDATE GRN SET ApproveBy=" + nUserID + ", ApproveDate=GETDATE(), GRNStatus=" + (int)EnumGRNStatus.Approved + ", LastUpdateBy="+nUserID+", LastUpdateDate=GETDATE() WHERE GRNID=" + oGRN.GRNID
                                        + "SELECT * FROM View_GRN WHERE GRNID=" + oGRN.GRNID);
        }

        public static IDataReader UndoApprove(TransactionContext tc, GRN oGRN, Int64 nUserID)
        {
            return tc.ExecuteReader("UPDATE GRN SET ApproveBy=0,  GRNStatus=" + (int)EnumGRNStatus.Initialize + ", LastUpdateBy=" + nUserID + ", LastUpdateDate=GETDATE() WHERE GRNID=" + oGRN.GRNID
                                        + "SELECT * FROM View_GRN WHERE GRNID=" + oGRN.GRNID);
        }
        public static IDataReader Receive(TransactionContext tc, GRN oGRN, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitGRNReceived] %n, %n", oGRN.GRNID, nUserID);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, GRN oGRN)
        {
            tc.ExecuteNonQuery(" Update GRN SET IsWillVoucherEffect = %b WHERE GRNID  = %n", oGRN.IsWillVoucherEffect, oGRN.GRNID);
        }
        public static void UpdateMRIRInfo(TransactionContext tc, GRN oGRN)
        {
            tc.ExecuteNonQuery(" Update GRN SET MRIRNo = %s, MRIRDate = %d WHERE GRNID  = %n", oGRN.MRIRNo,oGRN.MRIRDateSt,  oGRN.GRNID);
        }  

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRN WHERE GRNID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRN");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
