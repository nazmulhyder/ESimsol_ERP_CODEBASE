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
    public class MaxOTCEmployeeTypeDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, MaxOTCEmployeeType oMaxOTCEmployeeType, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MaxOTCEmployeeType]"
                                   + "%n, %n, %n,%s,%n,%n,%s",
                                   oMaxOTCEmployeeType.MaxOTCEmployeeTypeID, oMaxOTCEmployeeType.MaxOTConfigurationID, oMaxOTCEmployeeType.EmployeeTypeID, oMaxOTCEmployeeType.Remarks, (int)eEnumDBOperation, nUserID, sIDs);
        }

        public static void Delete(TransactionContext tc, MaxOTCEmployeeType oMaxOTCEmployeeType, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MaxOTCEmployeeType]"
                                    + "%n, %n, %n,%s,%n,%n,%s",
                                     oMaxOTCEmployeeType.MaxOTCEmployeeTypeID, oMaxOTCEmployeeType.MaxOTConfigurationID, oMaxOTCEmployeeType.EmployeeTypeID, oMaxOTCEmployeeType.Remarks, (int)eEnumDBOperation, nUserID, sIDs);
        }

        #endregion

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MaxOTCEmployeeType WHERE MaxOTConfigurationID = %n Order By MaxOTCEmployeeTypeID", id);
        } 
    }
}
