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
   public  class CharitySlabDA
    {
        #region Insert Update Delete Function

       public static IDataReader InsertUpdate(TransactionContext tc, CharitySlab oCharitySlab, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CharitySlab]"
                                   + "%n,%n,%n,%n,%n,%n,%n",
                                   oCharitySlab.CharitySlabID, oCharitySlab.SalaryHeadID, oCharitySlab.StartSalaryRange, oCharitySlab.EndSalaryRange, oCharitySlab.CharityAmount, nUserID, (int)eEnumDBOperation);
        }

       public static void Delete(TransactionContext tc, CharitySlab oCharitySlab, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CharitySlab]"
                                  + "%n,%n,%n,%n,%n,%n,%n",
                                   oCharitySlab.CharitySlabID, oCharitySlab.SalaryHeadID, oCharitySlab.StartSalaryRange, oCharitySlab.EndSalaryRange, oCharitySlab.CharityAmount, nUserID, (int)eEnumDBOperation);
        }


        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CharitySlab WHERE CharitySlabID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
