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

    public class StyleDepartmentDA
    {
        public StyleDepartmentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, StyleDepartment oStyleDepartment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_StyleDepartment]"
                                    + "%n,  %s, %s, %n,%n",
                                    oStyleDepartment.StyleDepartmentID,  oStyleDepartment.Name, oStyleDepartment.Note, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, StyleDepartment oStyleDepartment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_StyleDepartment]"
                                    + "%n,  %s, %s, %n,%n",
                                    oStyleDepartment.StyleDepartmentID, oStyleDepartment.Name, oStyleDepartment.Note, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM StyleDepartment WHERE StyleDepartmentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM StyleDepartment");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
  
}
