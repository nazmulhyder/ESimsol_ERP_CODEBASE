using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class WYRequisitionDA
    {
        public WYRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, WYRequisition oWYRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_WYRequisition]"
                                    + "%n, %s,%n, %D, %n, %n,%s, %n, %n, %n, %n, %n",
                                    oWYRequisition.WYRequisitionID, oWYRequisition.RequisitionNo, oWYRequisition.BUID, oWYRequisition.IssueDate, oWYRequisition.IssueStoreID, oWYRequisition.ReceiveStoreID, oWYRequisition.Remarks, oWYRequisition.WYarnTypeInt, oWYRequisition.RequisitionTypeInt, (int)oWYRequisition.WarpWeftType, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, WYRequisition oWYRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_WYRequisition]"
                                    + "%n, %s,%n, %d, %n, %n,%s, %n, %n, %n, %n, %n",
                                    oWYRequisition.WYRequisitionID, oWYRequisition.RequisitionNo, oWYRequisition.BUID, oWYRequisition.IssueDate, oWYRequisition.IssueStoreID, oWYRequisition.ReceiveStoreID, oWYRequisition.Remarks, oWYRequisition.WYarnTypeInt, oWYRequisition.RequisitionTypeInt, (int)oWYRequisition.WarpWeftType, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYRequisition WHERE WYRequisitionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYRequisition");
        }
        public static IDataReader BUWiseGets(int buid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYRequisition WHERE BUID = %n",buid);
        }
        
        public static IDataReader GetsByName(TransactionContext tc, string sName)
        {

            return tc.ExecuteReader("SELECT * FROM View_WYRequisition WHERE RequisitionNo LIKE ('%" + sName + "%')   Order by [RequisitionNo]");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
