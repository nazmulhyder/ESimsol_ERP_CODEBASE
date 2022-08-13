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
    public class GRNLandingCostDA
    {
        public GRNLandingCostDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GRNLandingCost oGRNLandingCost, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GRNLandingCost]");   //No need
            //return tc.ExecuteReader("EXEC [SP_IUD_GRNLandingCost]" + "%n, %s, %n, %s, %n, %n",
            //                        oGRNLandingCost.GRNLandingCostID, oGRNLandingCost.GRNLandingCostName, oGRNLandingCost.Sequence, oGRNLandingCost.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, GRNLandingCost oGRNLandingCost, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GRNLandingCost]");   //No need
            //tc.ExecuteNonQuery("EXEC [SP_IUD_GRNLandingCost]" + "%n, %s, %n, %s, %n, %n",
            //                        oGRNLandingCost.GRNLandingCostID, oGRNLandingCost.GRNLandingCostName, oGRNLandingCost.Sequence, oGRNLandingCost.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRNLandingCost WHERE GRNLandingCostID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRNLandingCost ORDER BY Sequence ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
