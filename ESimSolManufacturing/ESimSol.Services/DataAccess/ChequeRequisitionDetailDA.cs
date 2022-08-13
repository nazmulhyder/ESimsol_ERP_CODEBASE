using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeRequisitionDetailDA
    {
        public ChequeRequisitionDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChequeRequisitionDetail oChequeRequisitionDetail, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChequeRequisitionDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                         oChequeRequisitionDetail.ChequeRequisitionDetailID, oChequeRequisitionDetail.ChequeRequisitionID, oChequeRequisitionDetail.VoucherBillID, oChequeRequisitionDetail.Amount, oChequeRequisitionDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ChequeRequisitionDetail oChequeRequisitionDetail, EnumDBOperation eEnumDBOperation, int nUserID, string sChequeRequisitionDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChequeRequisitionDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                         oChequeRequisitionDetail.ChequeRequisitionDetailID, oChequeRequisitionDetail.ChequeRequisitionID, oChequeRequisitionDetail.VoucherBillID, oChequeRequisitionDetail.Amount, oChequeRequisitionDetail.Remarks, nUserID, (int)eEnumDBOperation, sChequeRequisitionDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisitionDetail WHERE ChequeRequisitionDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisitionDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nChequeRequisitionID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisitionDetail WHERE ChequeRequisitionID=%n", nChequeRequisitionID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
