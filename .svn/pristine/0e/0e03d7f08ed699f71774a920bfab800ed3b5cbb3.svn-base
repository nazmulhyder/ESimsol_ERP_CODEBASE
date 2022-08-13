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
    public class RMRequisitionSheetDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, RMRequisitionSheet oRMRequisitionSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RMRequisitionSheet]"
                                   + "%n, %n, %n, %n,  %n, %n, %s",
                                   oRMRequisitionSheet.RMRequisitionSheetID, oRMRequisitionSheet.RMRequisitionID,  oRMRequisitionSheet.ProductionSheetID, oRMRequisitionSheet.PPQty,  (int)eEnumDBOperation, nUserID,  sDetailIDs);
        }

        public static void Delete(TransactionContext tc, RMRequisitionSheet oRMRequisitionSheet, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMRequisitionSheet]"
                                   + "%n, %n, %n, %n,  %n, %n, %s",
                                   oRMRequisitionSheet.RMRequisitionSheetID, oRMRequisitionSheet.RMRequisitionID, oRMRequisitionSheet.ProductionSheetID, oRMRequisitionSheet.PPQty, (int)eEnumDBOperation, nUserID, sDetailIDs);
        }

  
        #endregion

        #region Get & Exist Function
             
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisitionSheet WHERE RMRequisitionSheetID=%n", nID);
        }
        public static IDataReader Gets(int nRMRequisitionID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisitionSheet WHERE RMRequisitionID = %n ORDER BY ProductionSheetID", nRMRequisitionID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
  
}
