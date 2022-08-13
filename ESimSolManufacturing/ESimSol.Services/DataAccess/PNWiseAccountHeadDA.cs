using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PNWiseAccountHeadDA
    {
        public PNWiseAccountHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PNWiseAccountHead oPNWiseAccountHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PNWiseAccountHead]"
                                    + "%n,%n, %n, %n,%n,%n",
                                    oPNWiseAccountHead.PNWiseAccountHeadID, oPNWiseAccountHead.AccountHeadNature, oPNWiseAccountHead.ProductNature, oPNWiseAccountHead.AccountHeadID, nUserID, (int)eEnumDBOperation);
                                 
        }

        public static void Delete(TransactionContext tc, PNWiseAccountHead oPNWiseAccountHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PNWiseAccountHead]"
                                    + "%n,%n, %n, %n,%n,%n",
                                    oPNWiseAccountHead.PNWiseAccountHeadID, oPNWiseAccountHead.AccountHeadNature, oPNWiseAccountHead.ProductNature, oPNWiseAccountHead.AccountHeadID, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PNWiseAccountHead WHERE PNWiseAccountHeadID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PNWiseAccountHead");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_PNWiseAccountHead
        }

        public static IDataReader GetByCategory(TransactionContext tc, bool bCategory)
        {
            return tc.ExecuteReader("SELECT * FROM View_PNWiseAccountHead", bCategory);
        }

        public static IDataReader GetByNegoPNWiseAccountHead(TransactionContext tc, int nPNWiseAccountHeadID)
        {
            return tc.ExecuteReader("select * from View_PNWiseAccountHead where PNWiseAccountHeadID in (select DISTINCT(NegotiationPNWiseAccountHeadID) from EXPORTLC where NegotiationPNWiseAccountHeadID>0 ) ", nPNWiseAccountHeadID);
        }
        #endregion
    }
}
