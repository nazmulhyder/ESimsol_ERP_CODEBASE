using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeRequisitionDA
    {
        public ChequeRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ChequeRequisition oChequeRequisition, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ChequeRequisition]" + "%n, %s, %n, %n, %d, %n, %n, %d, %n, %n, %n, %n, %n, %n, %s, %n, %n",
                                         oChequeRequisition.ChequeRequisitionID, oChequeRequisition.RequisitionNo, oChequeRequisition.BUID, oChequeRequisition.RequisitionStatusInt, oChequeRequisition.RequisitionDate, oChequeRequisition.SubledgerID, oChequeRequisition.PayTo, oChequeRequisition.ChequeDate, oChequeRequisition.ChequeTypeInt, oChequeRequisition.BankAccountID, oChequeRequisition.BankBookID, oChequeRequisition.ChequeID, oChequeRequisition.ChequeAmount, oChequeRequisition.ApprovedBy, oChequeRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ChequeRequisition oChequeRequisition, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ChequeRequisition]" + "%n, %s, %n, %n, %d, %n, %n, %d, %n, %n, %n, %n, %n, %n, %s, %n, %n",
                                         oChequeRequisition.ChequeRequisitionID, oChequeRequisition.RequisitionNo, oChequeRequisition.BUID, oChequeRequisition.RequisitionStatusInt, oChequeRequisition.RequisitionDate, oChequeRequisition.SubledgerID, oChequeRequisition.PayTo, oChequeRequisition.ChequeDate, oChequeRequisition.ChequeTypeInt, oChequeRequisition.BankAccountID, oChequeRequisition.BankBookID, oChequeRequisition.ChequeID, oChequeRequisition.ChequeAmount, oChequeRequisition.ApprovedBy, oChequeRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Approved(TransactionContext tc, ChequeRequisition oChequeRequisition, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_ApprovedChequeRequisition] %n, %n", oChequeRequisition.ChequeRequisitionID, nUserID);
        }
        public static void UpdateVoucherEffect(TransactionContext tc, ChequeRequisition oChequeRequisition)
        {
            tc.ExecuteNonQuery(" Update ChequeRequisition SET IsWillVoucherEffect = %b WHERE ChequeRequisitionID  = %n", oChequeRequisition.IsWillVoucherEffect, oChequeRequisition.ChequeRequisitionID);
        }  
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisition WHERE ChequeRequisitionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisition");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsInitialChequeRequisitions(TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeRequisition WHERE ISNULL(ApprovedBy,0)=0  ORDER BY ChequeRequisitionID ASC");
        }
        #endregion
    }
}
