﻿using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNQCResultSetupDA
    {
        public FNQCResultSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FNQCResultSetup oFNQCResultSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNQCResultSetup]"
                                    + "%n, %n, %n, %s, %s, %s, %s, %n, %n",
                                    oFNQCResultSetup.FNQCResultSetupID,     oFNQCResultSetup.FNQCParameterID,   oFNQCResultSetup.FNTPID,        oFNQCResultSetup.SubName,
                                    oFNQCResultSetup.Value,                 oFNQCResultSetup.Note,              oFNQCResultSetup.TestMethod,    (int)eEnumDBOperation,      nUserID);
        }

        public static void Delete(TransactionContext tc, FNQCResultSetup oFNQCResultSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNQCResultSetup]"
                                   + "%n, %n, %n, %s, %s, %s, %s, %n, %n",
                                    oFNQCResultSetup.FNQCResultSetupID,     oFNQCResultSetup.FNQCParameterID,   oFNQCResultSetup.FNTPID,         oFNQCResultSetup.SubName,
                                    oFNQCResultSetup.Value,                 oFNQCResultSetup.Note,              oFNQCResultSetup.TestMethod,     (int)eEnumDBOperation,     nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCResultSetup WHERE FNQCResultSetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNQCResultSetup Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}


