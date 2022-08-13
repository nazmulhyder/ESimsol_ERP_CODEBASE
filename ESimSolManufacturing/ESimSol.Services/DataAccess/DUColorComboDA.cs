using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUColorComboDA
    {
        public DUColorComboDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUColorCombo oDUColorCombo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUColorCombo]"
                                    + "%n,%n,%n,%n ,%s,%n,%n",
                                    oDUColorCombo.DUColorComboID,oDUColorCombo.DyeingOrderID, oDUColorCombo.DyeingOrderDetailID, (int)oDUColorCombo.ComboID,
                                    oDUColorCombo.SLNo,nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUColorCombo oDUColorCombo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUColorCombo]"
                                    + "%n,%n,%n,%n ,%s,%n,%n",
                                    oDUColorCombo.DUColorComboID, oDUColorCombo.DyeingOrderID, oDUColorCombo.DyeingOrderDetailID, (int)oDUColorCombo.ComboID,
                                    oDUColorCombo.SLNo, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUColorCombo WHERE DUColorComboID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDODID, int nComboID, long nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUColorCombo WHERE DyeingOrderID=%n and ComboID=%n",  nDODID,  nComboID);
        }
          public static IDataReader Gets(TransactionContext tc, int nDODID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUColorCombo WHERE DyeingOrderID=%n order by ComboID", nDODID);
        }
        #endregion
    }
}
