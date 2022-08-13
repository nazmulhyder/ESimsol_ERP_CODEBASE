using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeGroupDA
    {
        public EmployeeGroupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EmployeeGroup oEmployeeGroup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeGroup]"
                                    + "%n, %n, %n, %n, %n, %b,%s", oEmployeeGroup.EGID, oEmployeeGroup.EmployeeID, oEmployeeGroup.EmployeeTypeID, nUserID, (int)eEnumDBOperation, oEmployeeGroup.IsBlock,"");
        }

        public static void Delete(TransactionContext tc, EmployeeGroup oEmployeeGroup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeGroup]"
                                    + "%n, %n, %n, %n, %n, %b,%s", oEmployeeGroup.EGID, oEmployeeGroup.EmployeeID, oEmployeeGroup.EmployeeTypeID, nUserID, (int)eEnumDBOperation, oEmployeeGroup.IsBlock,"");
        }

        public static void Upload(TransactionContext tc, EmployeeGroup oEmployeeGroup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeGroup]"
                                    + "%n, %n, %n, %n, %n, %b,%s", oEmployeeGroup.EGID, oEmployeeGroup.EmployeeID, oEmployeeGroup.EmployeeTypeID, nUserID, (int)eEnumDBOperation, oEmployeeGroup.IsBlock,oEmployeeGroup.Name);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeGroup WHERE EmployeeTypeID=%n", nID);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeGroup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

