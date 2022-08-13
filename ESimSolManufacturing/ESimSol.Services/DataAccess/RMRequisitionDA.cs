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
    public class RMRequisitionDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RMRequisition oRMRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RMRequisition]"
                                   + "%n, %s, %d,%n, %s, %n, %n",
                                    oRMRequisition.RMRequisitionID, oRMRequisition.RefNo, oRMRequisition.RequisitionDate,  oRMRequisition.BUID,  oRMRequisition.Remarks,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, RMRequisition oRMRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMRequisition]"
                                   + "%n, %s, %d,%n, %s, %n, %n",
                                    oRMRequisition.RMRequisitionID, oRMRequisition.RefNo, oRMRequisition.RequisitionDate, oRMRequisition.BUID, oRMRequisition.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
     
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisition WHERE RMRequisitionID=%n", nID);
        }
        public static IDataReader BUWiseGets(int nBUID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RMRequisition WHERE BUID = %n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
  
}
