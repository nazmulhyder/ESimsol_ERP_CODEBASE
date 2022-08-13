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

    public class SparePartsRequisitionDA
    {
        public SparePartsRequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SparePartsRequisition oSparePartsRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_SparePartsRequisition]" +
                "%n, %s, %n, %n, %n, %d, %n, %s, %n, %n",
                oSparePartsRequisition.SparePartsRequisitionID, oSparePartsRequisition.RequisitionNo, oSparePartsRequisition.BUID, oSparePartsRequisition.RequisitionBy, 
                oSparePartsRequisition.SPStatus, oSparePartsRequisition.IssueDate, oSparePartsRequisition.CRID,oSparePartsRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SparePartsRequisition oSparePartsRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_SparePartsRequisition]" +
                "%n, %s, %n, %n, %n, %d, %n, %s, %n, %n",
                oSparePartsRequisition.SparePartsRequisitionID, oSparePartsRequisition.RequisitionNo, oSparePartsRequisition.BUID, oSparePartsRequisition.RequisitionBy,
                oSparePartsRequisition.SPStatus, oSparePartsRequisition.IssueDate, oSparePartsRequisition.CRID, oSparePartsRequisition.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, SparePartsRequisition oSparePartsRequisition)
        {
            tc.ExecuteNonQuery(" Update SparePartsRequisition SET IsWillVoucherEffect = %b WHERE SparePartsRequisitionID  = %n", oSparePartsRequisition.SparePartsRequisitionID);
        }

        #region Accept SparePartsRequisition Revise
        public static IDataReader AcceptSparePartsRequisitionRevise(TransactionContext tc, SparePartsRequisition oSparePartsRequisition, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptSparePartsRequisitionRevise]" + 
                "%n, %s, %n, %s, %n, %n, %n, %d, %n, %n, %s,%n,%n,%n",
                oSparePartsRequisition.SparePartsRequisitionID, oSparePartsRequisition.RequisitionNo, oSparePartsRequisition.BUID, oSparePartsRequisition.RequisitionBy, 
                oSparePartsRequisition.SPStatus, oSparePartsRequisition.IssueDate, oSparePartsRequisition.CRID, oSparePartsRequisition.Remarks, nUserID);
        }


        #endregion

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsRequisition WHERE SparePartsRequisitionID=%n", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsRequisitionLog WHERE SparePartsRequisitionLogID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SparePartsRequisition");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }




}

