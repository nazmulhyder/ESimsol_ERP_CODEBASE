using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class POTandCClauseDA
    {
        public POTandCClauseDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, POTandCClause oPOTandCClause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPOTandCIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_POTandCClause]" + "%n, %n, %n, %s, %n, %n, %s", 
                oPOTandCClause.POTandCClauseID, oPOTandCClause.POID, oPOTandCClause.ClauseType, oPOTandCClause.TermsAndCondition, nUserID, (int)eEnumDBOperation, sPOTandCIDs);
        }
        public static void Delete(TransactionContext tc, POTandCClause oPOTandCClause, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPOTandCIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_POTandCClause]" + "%n, %n, %n, %s, %n, %n, %s",
                oPOTandCClause.POTandCClauseID, oPOTandCClause.POID, oPOTandCClause.ClauseType, oPOTandCClause.TermsAndCondition, nUserID, (int)eEnumDBOperation, sPOTandCIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCClause WHERE POTandCClauseID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCClause WHERE POID = %n Order by ClauseType", nPIID);
        }

        public static IDataReader GetsPILog(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_POTandCClauseLog WHERE ProformaInvoiceLogID = %n", id);
        }
        //GetsPILog

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}
