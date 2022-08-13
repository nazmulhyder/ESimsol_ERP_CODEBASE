using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class NOARequisitionDA
    {
        public NOARequisitionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, NOARequisition oNOARequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sReqIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_NOARequisition]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oNOARequisition.NOARequisitionID, oNOARequisition.NOAID, oNOARequisition.PRID, (int)eEnumDBOperation, nUserID, sReqIDs);
        }
        public static void Delete(TransactionContext tc, NOARequisition oNOARequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sReqIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_NOARequisition]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oNOARequisition.NOARequisitionID, oNOARequisition.NOAID, oNOARequisition.PRID, (int)eEnumDBOperation, nUserID, sReqIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOARequisition WHERE NOARequisitionID=%n", nId);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOARequisition");
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, long nNOId)
        {
            return tc.ExecuteReader("SELECT * FROM View_NOARequisition WHERE NOAID=%n ", nNOId);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
    

}
