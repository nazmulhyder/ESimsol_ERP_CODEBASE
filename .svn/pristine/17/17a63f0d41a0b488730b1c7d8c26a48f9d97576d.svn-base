using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class ProductionUnitDA
    {
        public ProductionUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionUnit oProductionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProductionUnit]"
                                    + "%n,%s,%s ,%n,%n, %n, %n",
                                    oProductionUnit.ProductionUnitID, oProductionUnit.Name, oProductionUnit.ShortName, oProductionUnit.ProductionUnitTypeInt, oProductionUnit.RefID, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ProductionUnit oProductionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProductionUnit]"
                                 + "%n,%s,%s ,%n,%n, %n, %n",
                                    oProductionUnit.ProductionUnitID, oProductionUnit.Name, oProductionUnit.ShortName, oProductionUnit.ProductionUnitTypeInt, oProductionUnit.RefID, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionUnit WHERE ProductionUnitID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nProductionUnitType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionUnit WHERE ProductionUnitType=%n", nProductionUnitType);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionUnit");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionUnit");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
       
        #endregion
    }
}