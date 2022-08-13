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
    public class PRSpecDA
    {
        #region Insert Update Delete Function


        public static void Delete(TransactionContext tc, string sSQL)
        {
            tc.ExecuteNonQuery(sSQL);
        }
        public static IDataReader InsertUpdate(TransactionContext tc, PRSpec oPRSpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            //return tc.ExecuteReader("EXEC [SP_IUD_PRSpec]"
            //                         + "%n,%n,%n,%s,%n,%n,%n,%s",
            //                         oPRSpec.PRSpecID, oPRSpec.SpecHeadID, oPRSpec.PRDetailID, oPRSpec.PRDescription, oPRSpec.SL, nUserID, nDBOperation, sSpecIDs);

            return tc.ExecuteReader("EXEC [SP_IUD_PRSpec]"
                                     + "%n,%n,%n,%s,%n,%n,%s",
                                     oPRSpec.PRSpecID, oPRSpec.SpecHeadID, oPRSpec.PRDetailID, oPRSpec.PRDescription, nUserID, nDBOperation, sSpecIDs);

        }


        public static void Delete(TransactionContext tc, PRSpec oPRSpec, int nDBOperation, int nUserID, string sSpecIDs)
        {
            //tc.ExecuteNonQuery("EXEC [SP_IUD_PRSpec]"
            //                        + "%n,%n,%n,%s,%n,%n,%n,%s",
            //                        oPRSpec.PRSpecID, oPRSpec.SpecHeadID, oPRSpec.PRDetailID, oPRSpec.PRDescription, oPRSpec.SL, nUserID, nDBOperation, sSpecIDs);

            tc.ExecuteNonQuery("EXEC [SP_IUD_PRSpec]"
                                   + "%n,%n,%n,%s,%n,%n,%s",
                                   oPRSpec.PRSpecID, oPRSpec.SpecHeadID, oPRSpec.PRDetailID, oPRSpec.PRDescription, nUserID, nDBOperation, sSpecIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PRSpec WHERE PRSpecID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PRSpec");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void UpdateSequence(TransactionContext tc, PRSpec oPRSpec)
        {
            tc.ExecuteNonQuery("Update PRSpec SET SL = %n WHERE PRSpecID = %n", oPRSpec.SL, oPRSpec.PRSpecID);
        }

        #endregion
    }
}
