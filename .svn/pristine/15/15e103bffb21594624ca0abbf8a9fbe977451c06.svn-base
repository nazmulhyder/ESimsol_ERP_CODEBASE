using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DURequisitionDA
    {
        public DURequisitionDA() { }

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, DURequisition oDURequisition, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DURequisition]"
                                    + "%n,%D, %n,%n,%n, %n,%n, %n, %s, %b, %n,%n",
                                    oDURequisition.DURequisitionID,
                                    oDURequisition.ReqDate,
                                    oDURequisition.BUID_issue,
                                    oDURequisition.BUID_Receive,
                                    oDURequisition.RequisitionTypeInt,
                                    oDURequisition.WorkingUnitID_Issue,
                                    oDURequisition.WorkingUnitID_Receive,
                                    oDURequisition.OperationUnitTypeInt,
                                    oDURequisition.Note,
                                    oDURequisition.IsOpenOrder,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, DURequisition oDURequisition, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DURequisition]"
                                    + "%n,%D, %n,%n,%n, %n,%n, %n, %s, %b, %n,%n",
                                    oDURequisition.DURequisitionID,
                                    oDURequisition.ReqDate,
                                    oDURequisition.BUID_issue,
                                    oDURequisition.BUID_Receive,
                                    oDURequisition.RequisitionTypeInt,
                                    oDURequisition.WorkingUnitID_Issue,
                                    oDURequisition.WorkingUnitID_Receive,
                                    oDURequisition.OperationUnitTypeInt,
                                    oDURequisition.Note,
                                    oDURequisition.IsOpenOrder,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDURequisitionID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_DURequisition where DURequisitionID=%n", nDURequisitionID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DURequisition");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       public static IDataReader Approve(TransactionContext tc, DURequisition oDURequisition, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DURequisition]"
                                      + "%n,%D, %n,%n,%n, %n,%n, %n, %s, %b, %n,%n",
                                    oDURequisition.DURequisitionID,
                                    oDURequisition.ReqDate,
                                    oDURequisition.BUID_issue,
                                    oDURequisition.BUID_Receive,
                                    oDURequisition.RequisitionTypeInt,
                                    oDURequisition.WorkingUnitID_Issue,
                                    oDURequisition.WorkingUnitID_Receive,
                                    oDURequisition.OperationUnitTypeInt,
                                    oDURequisition.Note,
                                    oDURequisition.IsOpenOrder,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
       public static IDataReader UndoApprove(TransactionContext tc, DURequisition oDURequisition, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DURequisition]"
                                     + "%n,%D, %n,%n,%n, %n,%n, %n, %s, %b, %n,%n",
                                    oDURequisition.DURequisitionID,
                                    oDURequisition.ReqDate,
                                    oDURequisition.BUID_issue,
                                    oDURequisition.BUID_Receive,
                                    oDURequisition.RequisitionTypeInt,
                                    oDURequisition.WorkingUnitID_Issue,
                                    oDURequisition.WorkingUnitID_Receive,
                                    oDURequisition.OperationUnitTypeInt,
                                    oDURequisition.Note,
                                    oDURequisition.IsOpenOrder,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
       public static IDataReader Issue(TransactionContext tc, DURequisition oDURequisition, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DURequisition_Issue_Receive]"
                                    + "%n,%n,%n",
                                   oDURequisition.DURequisitionID,
                                   (int)EnumInOutType.Disburse,
                                   nUserId);
        }
       public static IDataReader UndoIssue(TransactionContext tc, DURequisition oDURequisition, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
       {
           return tc.ExecuteReader("EXEC [SP_IUD_DURequisition]"
                                      + "%n,%D, %n,%n,%n, %n,%n, %n, %s, %b, %n,%n",
                                    oDURequisition.DURequisitionID,
                                    oDURequisition.ReqDate,
                                    oDURequisition.BUID_issue,
                                    oDURequisition.BUID_Receive,
                                    oDURequisition.RequisitionTypeInt,
                                    oDURequisition.WorkingUnitID_Issue,
                                    oDURequisition.WorkingUnitID_Receive,
                                    oDURequisition.OperationUnitTypeInt,
                                    oDURequisition.Note,
                                    oDURequisition.IsOpenOrder,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
       }
       public static IDataReader Receive(TransactionContext tc, DURequisition oDURequisition, Int64 nUserId)
       {
           return tc.ExecuteReader("EXEC [SP_IUD_DURequisition_Issue_Receive]"
                                    + "%n,%n,%n",
                                   oDURequisition.DURequisitionID,
                                   (int)EnumInOutType.Receive,
                                   nUserId);
       }
         
        #endregion
    }

}
