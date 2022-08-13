using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{


    public class PurchaseRequisitionDA
    {
        public PurchaseRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseRequisition oPR, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseRequisition]"
                                    + "%n, %s, %d, %d, %s, %n, %n, %n, %n,%s, %n, %n",
                                        oPR.PRID, oPR.PRNo, oPR.PRDate,oPR.RequirementDate,  oPR.Note, oPR.ApproveBy, oPR.BUID, oPR.DepartmentID, oPR.PriortyLevelInt, oPR.IDNo,  nUserID, (int)eEnumDBOperation );
        }

        public static void Delete(TransactionContext tc, PurchaseRequisition oPR, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseRequisition]"
                                    + "%n, %s, %d, %d, %s, %n, %n, %n, %n,%s, %n, %n",
                                        oPR.PRID, oPR.PRNo, oPR.PRDate, oPR.RequirementDate, oPR.Note, oPR.ApproveBy, oPR.BUID, oPR.DepartmentID, oPR.PriortyLevelInt, oPR.IDNo, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader RequestRequisitionRevise(TransactionContext tc, PurchaseRequisition oPR, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_PurchaseRequisitionRevise]" + "%n,%n,%b,%n",
                                    oPR.PRID
                                    ,oPR.Status
                                    ,oPR.bIsReviseWithNo
                                    ,nUserID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader UpdateForRevise(TransactionContext tc, PurchaseRequisition oPR, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PurchaseRequisition SET Status=" + oPR.Status + " WHERE PRID=" + oPR.PRID);
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisition WHERE PRID=" + oPR.PRID);
        }
        public static IDataReader UndoRequestRevise(TransactionContext tc, PurchaseRequisition oPR, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PurchaseRequisition SET Status=" + oPR.Status + " WHERE PRID=" + oPR.PRID);
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisition WHERE PRID=" + oPR.PRID);
        }
        public static IDataReader Finish(TransactionContext tc, PurchaseRequisition oPR, Int64 nUserID)
        {
            return tc.ExecuteReader("UPDATE PurchaseRequisition SET FinishByID=" + nUserID + " WHERE PRID=" + oPR.PRID);
        }
        public static IDataReader Cancel(TransactionContext tc, PurchaseRequisition oPR, Int64 nUserID)
        {
            return tc.ExecuteReader("UPDATE PurchaseRequisition SET CancelByID=" + nUserID + " WHERE PRID=" + oPR.PRID);
        }

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisition WHERE PRID=%n", nID);
        }
        public static IDataReader GetsBy(TransactionContext tc , string nStatus)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisition where [Status] in (%q)", nStatus);
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nbuid)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseRequisition where BUID = %n", nbuid);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    
   
}
