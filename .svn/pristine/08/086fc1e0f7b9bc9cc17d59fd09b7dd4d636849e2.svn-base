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

    public class ConsumptionRequisitionDA
    {
        public ConsumptionRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ConsumptionRequisition oConsumptionRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ConsumptionRequisition]" + "%n, %s, %n, %s, %n, %n, %n, %d, %n, %n, %s,%n,%n, %n, %n, %n, %n",
                                    oConsumptionRequisition.ConsumptionRequisitionID, oConsumptionRequisition.RequisitionNo, oConsumptionRequisition.BUID, oConsumptionRequisition.RefNo, oConsumptionRequisition.CRTypeInt, oConsumptionRequisition.RequisitionBy, oConsumptionRequisition.CRStatusInt, oConsumptionRequisition.IssueDate, oConsumptionRequisition.RequisitionFor, oConsumptionRequisition.StoreID, oConsumptionRequisition.Remarks, oConsumptionRequisition.ShiftInInt, oConsumptionRequisition.SubLedgerID, oConsumptionRequisition.RefTypeInt, oConsumptionRequisition.RefObjID,   nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ConsumptionRequisition oConsumptionRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ConsumptionRequisition]" + "%n, %s, %n, %s, %n, %n, %n, %d, %n, %n, %s,%n,%n, %n, %n, %n, %n",
                                    oConsumptionRequisition.ConsumptionRequisitionID, oConsumptionRequisition.RequisitionNo, oConsumptionRequisition.BUID, oConsumptionRequisition.RefNo, oConsumptionRequisition.CRTypeInt, oConsumptionRequisition.RequisitionBy, oConsumptionRequisition.CRStatusInt, oConsumptionRequisition.IssueDate, oConsumptionRequisition.RequisitionFor, oConsumptionRequisition.StoreID, oConsumptionRequisition.Remarks, oConsumptionRequisition.ShiftInInt, oConsumptionRequisition.SubLedgerID, oConsumptionRequisition.RefTypeInt, oConsumptionRequisition.RefObjID, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, ConsumptionRequisition oConsumptionRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_ConsumptionRequisitionOperation]" + " %n, %n, %s, %n, %n, %n",
                                    oConsumptionRequisition.ConsumptionRequisitionID, oConsumptionRequisition.CRStatusInt, oConsumptionRequisition.Remarks, oConsumptionRequisition.CRActionTypeInt, nUserID, (int)eEnumDBOperation);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, ConsumptionRequisition oConsumptionRequisition)
        {
            tc.ExecuteNonQuery(" Update ConsumptionRequisition SET IsWillVoucherEffect = %b WHERE ConsumptionRequisitionID  = %n", oConsumptionRequisition.IsWillVoucherEffect, oConsumptionRequisition.ConsumptionRequisitionID);
        }  

        #region Accept ConsumptionRequisition Revise
        public static IDataReader AcceptConsumptionRequisitionRevise(TransactionContext tc, ConsumptionRequisition oConsumptionRequisition, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptConsumptionRequisitionRevise]" + "%n, %s, %n, %s, %n, %n, %n, %d, %n, %n, %s,%n,%n,%n",
                                    oConsumptionRequisition.ConsumptionRequisitionID, oConsumptionRequisition.RequisitionNo, oConsumptionRequisition.BUID, oConsumptionRequisition.RefNo, oConsumptionRequisition.CRTypeInt, oConsumptionRequisition.RequisitionBy, oConsumptionRequisition.CRStatusInt, oConsumptionRequisition.IssueDate, oConsumptionRequisition.RequisitionFor, oConsumptionRequisition.StoreID, oConsumptionRequisition.Remarks, oConsumptionRequisition.ShiftInInt, oConsumptionRequisition.SubLedgerID,  nUserID);
        }


        #endregion

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ConsumptionRequisition WHERE ConsumptionRequisitionID=%n", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ConsumptionRequisitionLog WHERE ConsumptionRequisitionLogID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ConsumptionRequisition");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  

  


}
