using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RackDA
    {
        public RackDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Rack oRack, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Rack]"
                                    + "%n, %s, %n,%s, %n, %n",
                                    oRack.RackID, oRack.RackNo, oRack.ShelfID, oRack.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, Rack oRack, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Rack]"
                                    + "%n, %s, %n,%s, %n, %n",
                                    oRack.RackID, oRack.RackNo, oRack.ShelfID, oRack.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Rack WHERE RackID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Rack");
        }
        public static IDataReader Gets(TransactionContext tc, int nShelfID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Rack WHERE ShelfID = %n", nShelfID);
        }
         public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Rack WHERE ShelfID IN (SELECT ShelfID FROM Shelf WHERE BUID = %n)", nBUID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
