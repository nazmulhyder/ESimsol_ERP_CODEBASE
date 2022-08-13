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
    public class GarmentsClassDA
    {
        public GarmentsClassDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GarmentsClass oGarmentsClass, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GarmentsClass]"
                                    + "%n, %s, %n, %s, %n, %n",
                                    oGarmentsClass.GarmentsClassID, oGarmentsClass.ClassName, oGarmentsClass.ParentClassID, oGarmentsClass.Note,    nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, GarmentsClass oGarmentsClass, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GarmentsClass]"
                                    + "%n, %s, %n, %s, %n, %n",
                                    oGarmentsClass.GarmentsClassID, oGarmentsClass.ClassName, oGarmentsClass.ParentClassID, oGarmentsClass.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsClass WHERE GarmentsClassID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsClass");
        }
        public static IDataReader GetsGarmentsClass(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsClass WHERE ParentClassID=1");
        }
        public static IDataReader GetsGarmentsSubClass(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM GarmentsClass WHERE ParentClassID NOT IN(0,1)");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
