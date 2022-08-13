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
    public class MachineLiquorDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MachineLiquor oMachineLiquor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MachineLiquor]"
                                   + "%n,%n,%n,%n,%n,%n",
                                   oMachineLiquor.MachineLiquorID,
                                   oMachineLiquor.MachineID,
                                   oMachineLiquor.Label,
                                   oMachineLiquor.Liquor,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MachineLiquor oMachineLiquor, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MachineLiquor]"
                                   + "%n,%n,%n,%n,%n,%n",
                                   oMachineLiquor.MachineLiquorID,
                                   oMachineLiquor.MachineID,
                                   oMachineLiquor.Label,
                                   oMachineLiquor.Liquor,
                                   nUserID,
                                   (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MachineLiquor WHERE MachineLiquorID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MachineLiquor");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
