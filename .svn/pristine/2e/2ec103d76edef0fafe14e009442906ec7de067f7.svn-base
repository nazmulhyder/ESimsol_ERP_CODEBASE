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
    public class UnitConversionDA
    {
        public UnitConversionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, UnitConversion oUnitConversion, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_UnitConversion]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n",
                                    oUnitConversion.UnitConversionID, oUnitConversion.ProductID,  oUnitConversion.MeasurementUnitID, oUnitConversion.MeasureUnitValue, oUnitConversion.ConvertedUnitID, oUnitConversion.ConvertedUnitValue, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, UnitConversion oUnitConversion, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_UnitConversion]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n",
                                    oUnitConversion.UnitConversionID, oUnitConversion.ProductID, oUnitConversion.MeasurementUnitID, oUnitConversion.MeasureUnitValue, oUnitConversion.ConvertedUnitID, oUnitConversion.ConvertedUnitValue, nUserId, (int)eEnumDBOperation);
        }       
        public static void CommitUnitConversion(TransactionContext tc,int nProductID, string sIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_CommitUnitConversion]"+ "%n,%s,%n",nProductID, sIDs, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_UnitConversion WHERE UnitConversionID=%n", nID);
        }
        
        public static IDataReader GetCount(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT ISNULL(count(*),0) as NumberofConversion FROM View_UnitConversion WHERE ProductID =%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_UnitConversion");
        }
        public static IDataReader Gets(TransactionContext tc, int nProductID)
        {
            return tc.ExecuteReader("SELECT * FROM View_UnitConversion WHERE ProductID=%n", nProductID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
