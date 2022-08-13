using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ShelfDA
    {
        public ShelfDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Shelf oShelf, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Shelf]"
                                    + "%n, %s, %s, %n,%s, %n, %n",
                                    oShelf.ShelfID, oShelf.ShelfNo, oShelf.ShelfName,oShelf.BUID, oShelf.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, Shelf oShelf, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Shelf]"
                                    + "%n, %s, %s, %n,%s, %n, %n",
                                    oShelf.ShelfID, oShelf.ShelfNo, oShelf.ShelfName, oShelf.BUID, oShelf.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Shelf WHERE ShelfID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Shelf");
        }
        public static IDataReader GetsByNoOrName(TransactionContext tc, Shelf oShelf)
        {
            if (oShelf.ShelfNo != "")
            {
                return tc.ExecuteReader("SELECT * FROM Shelf WHERE LIKE '%" + oShelf.ShelfNo + "%' ORDER BY Name");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM Shelf WHERE ShelfName LIKE '%" + oShelf.ShelfName + "%' ORDER BY Name");
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
