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
    public class FabricRequisitionDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricRequisition oFabricRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricRequisition]"
                                   + "%n,%n,%s,%D,    %n,%n,%n,%s,      %n,%n",
                                   oFabricRequisition.FabricRequisitionID, oFabricRequisition.RequisitionType, oFabricRequisition.ReqNo, oFabricRequisition.ReqDate, oFabricRequisition.BUID, oFabricRequisition.IssueStoreID, oFabricRequisition.ReceiveStoreID, oFabricRequisition.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricRequisition oFabricRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricRequisition]"
                                   + "%n,%n,%s,%D,    %n,%n,%n,%s,      %n,%n",
                                   oFabricRequisition.FabricRequisitionID, oFabricRequisition.RequisitionType, oFabricRequisition.ReqNo, oFabricRequisition.ReqDate, oFabricRequisition.BUID, oFabricRequisition.IssueStoreID, oFabricRequisition.ReceiveStoreID, oFabricRequisition.Note, nUserID, (int)eEnumDBOperation);
        }
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRequisition WHERE FabricRequisitionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricRequisition]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

        public static IDataReader ChangeStatus(TransactionContext tc, FabricRequisition oFabricRequisition)
        {
            if (oFabricRequisition.ErrorMessage == "Approve")
            {
                string sSQL1 = SQLParser.MakeSQL("Update FabricRequisition Set ApprovedBy=%n, ApproveDate=%D WHERE FabricRequisitionID=%n", oFabricRequisition.ApprovedBy, oFabricRequisition.ApproveDate, oFabricRequisition.FabricRequisitionID);
                tc.ExecuteNonQuery(sSQL1);
                return tc.ExecuteReader("SELECT * FROM View_FabricRequisition WHERE FabricRequisitionID=%n", oFabricRequisition.FabricRequisitionID);
            }
            else if (oFabricRequisition.ErrorMessage == "Unapprove")
            {
                string sSQL1 = SQLParser.MakeSQL("Update FabricRequisition Set ApprovedBy=%n, ApproveDate=%D WHERE FabricRequisitionID=%n", 0, null, oFabricRequisition.FabricRequisitionID);
                tc.ExecuteNonQuery(sSQL1);
                return tc.ExecuteReader("SELECT * FROM View_FabricRequisition WHERE FabricRequisitionID=%n", oFabricRequisition.FabricRequisitionID);
            }
            else               //Disburse
            {
                string sSQL1 = SQLParser.MakeSQL("Update FabricRequisition Set DisburseBy=%n, DisburseDate=%D WHERE FabricRequisitionID=%n", oFabricRequisition.DisburseBy, oFabricRequisition.DisburseDate, oFabricRequisition.FabricRequisitionID);
                tc.ExecuteNonQuery(sSQL1);
                return tc.ExecuteReader("SELECT * FROM View_FabricRequisition WHERE FabricRequisitionID=%n", oFabricRequisition.FabricRequisitionID);
            }

        }

        #endregion
    }

}
