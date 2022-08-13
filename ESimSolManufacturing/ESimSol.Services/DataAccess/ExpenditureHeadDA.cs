using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExpenditureHeadDA
    {
        public ExpenditureHeadDA() { }

        #region 

        #region Insert, Update, Delete
        public static IDataReader InsertUpdate(TransactionContext tc, ExpenditureHead oExpenditureHead, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExpenditureHead]" + "%n, %n, %s, %b, %n,%n, %n",
                                    oExpenditureHead.ExpenditureHeadID,     oExpenditureHead.AccountHeadID,    oExpenditureHead.Name,  oExpenditureHead.Activity,  (int)oExpenditureHead.ExpenditureHeadType,   nUserId,    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, ExpenditureHead oExpenditureHead, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExpenditureHead]" + "%n, %n, %s, %b, %n,%n, %n",
                                    oExpenditureHead.ExpenditureHeadID, oExpenditureHead.AccountHeadID, oExpenditureHead.Name, oExpenditureHead.Activity, (int)oExpenditureHead.ExpenditureHeadType, nUserId, (int)eENumDBPurchaseInvoice);
        }
        #endregion



        #region Get & Exist Function


        public static IDataReader Get(int nExpenditureHeadID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ExpenditureHead where ExpenditureHeadID=%n", nExpenditureHeadID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExpenditureHead");
        }
        public static IDataReader Gets(TransactionContext tc, int nOperationType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExpenditureHead where ExpenditureHeadID in (Select ExpenditureHeadID from ExpenditureHeadMapping where OperationType=%n)", nOperationType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
        #endregion
    }

}
