using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class PLineConfigureDA
    {
        public PLineConfigureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PLineConfigure oPLineConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_PLineConfigure]"
                                    + "%n, %n, %s,  %s,%n, %n, %n",
                                    oPLineConfigure.PLineConfigureID, oPLineConfigure.ProductionUnitID,  oPLineConfigure.LineNo, oPLineConfigure.Remarks, oPLineConfigure.MachineQty, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, PLineConfigure oPLineConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PLineConfigure]"
                                    + "%n, %n, %s,%s,%n, %n, %n",
                                    oPLineConfigure.PLineConfigureID, oPLineConfigure.ProductionUnitID, oPLineConfigure.LineNo, oPLineConfigure.Remarks, oPLineConfigure.MachineQty, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PLineConfigure WHERE PLineConfigureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PLineConfigure");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nPUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PLineConfigure WHERE ProductionUnitID=%n", nPUnitID);

        }
        
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}