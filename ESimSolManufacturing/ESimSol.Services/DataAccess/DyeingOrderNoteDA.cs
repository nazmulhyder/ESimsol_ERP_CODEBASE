using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DyeingOrderNoteDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DyeingOrderNote oDyeingOrderNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderNote]"
                                    + "%n, %n, %s, %n,%n",
                                    oDyeingOrderNote.DyeingOrderNoteID, oDyeingOrderNote.DyeingOrderID, oDyeingOrderNote.OrderNote, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DyeingOrderNote oDyeingOrderNote, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingOrderNote]"
                                    + "%n, %n, %s, %n,%n",
                                    oDyeingOrderNote.DyeingOrderNoteID, oDyeingOrderNote.DyeingOrderID, oDyeingOrderNote.OrderNote, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingOrderNote WHERE DyeingOrderID=0");
        }
        public static IDataReader Get(TransactionContext tc, int nDyeingOrderNoteID)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingOrderNote WHERE DyeingOrderNoteID=%n", nDyeingOrderNoteID);
        }
        public static IDataReader GetByOrderID(TransactionContext tc, int nDyeingOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingOrderNote WHERE DyeingOrderID=%n", nDyeingOrderID);
        }
        public static IDataReader GetByConID(TransactionContext tc, DyeingOrderNote oDyeingOrderNote)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingOrderNote WHERE DyeingOrderID=0 or DyeingOrderID in (Select Max(DyeingOrderID) from DyeingOrder where DyeingOrderID!=%n and ContractorID=%n) ",oDyeingOrderNote.DyeingOrderID, oDyeingOrderNote.ContractorID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}