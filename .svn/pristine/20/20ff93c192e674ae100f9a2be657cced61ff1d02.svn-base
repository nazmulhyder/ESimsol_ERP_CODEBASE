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
    public class MeasurementUnitBUDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, MeasurementUnitBU oMeasurementUnitBU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeasurementUnitBU]"
                                   + "%n,%n,%n",
                                   oMeasurementUnitBU.MeasurementUnitConID, oMeasurementUnitBU.BUID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MeasurementUnitBU oMeasurementUnitBU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MeasurementUnitBU]"
                                    + "%n,%n,%n",
                                    oMeasurementUnitBU.MeasurementUnitConID, oMeasurementUnitBU.BUID, (int)eEnumDBOperation);
        }



        #endregion

        #region Get & Exist Function
       
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
