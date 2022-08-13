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
    public class GarmentsTypeDA
    {
        public GarmentsTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GarmentsType oGarmentsType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GarmentsType]"
                                    + "%n, %s, %s, %n, %n",
                                    oGarmentsType.GarmentsTypeID, oGarmentsType.TypeName, oGarmentsType.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, GarmentsType oGarmentsType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GarmentsType]"
                                    + "%n, %s, %s, %n, %n",
                                    oGarmentsType.GarmentsTypeID, oGarmentsType.TypeName, oGarmentsType.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsType WHERE GarmentsTypeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
