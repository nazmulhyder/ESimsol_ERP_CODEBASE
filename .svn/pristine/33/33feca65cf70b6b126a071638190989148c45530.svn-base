using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BDYEACDetailDA
    {
        public BDYEACDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BDYEACDetail oBDYEACDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BDYEACDetail]"
                                    + "%n, %n, %s, %n, %n, %n, %s",
                                    oBDYEACDetail.BDYEACDetailID, oBDYEACDetail.BDYEACID, oBDYEACDetail.ProductName, oBDYEACDetail.Qty, (int)eEnumDBOperation, nUserID, "");
        }

        public static void Delete(TransactionContext tc, BDYEACDetail oBDYEACDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBDYEACDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BDYEACDetail]"
                                    + "%n, %n, %s, %n, %n, %n, %s",
                                    oBDYEACDetail.BDYEACDetailID, oBDYEACDetail.BDYEACID, oBDYEACDetail.ProductName, oBDYEACDetail.Qty, (int)eEnumDBOperation, nUserID, sBDYEACDetailIDs);
        }

        #endregion

      
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BDYEACDetail WHERE BDYEACDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BDYEACDetail");
        }

        public static IDataReader Gets(TransactionContext tc, int nGRNID)
        {
            return tc.ExecuteReader("SELECT * FROM BDYEACDetail WHERE BDYEACID =%n", nGRNID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
