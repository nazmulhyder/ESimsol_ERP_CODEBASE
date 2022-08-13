using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BusinessLocationDA
    {
        public BusinessLocationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BusinessLocation oBusinessLocation, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBusinessLocationIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BusinessLocation]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oBusinessLocation.BusinessLocationID, oBusinessLocation.BusinessUnitID, oBusinessLocation.LocationID, (int)eEnumDBOperation, nUserID, sBusinessLocationIDs);
        }

        public static void Delete(TransactionContext tc, BusinessLocation oBusinessLocation, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBusinessLocationIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BusinessLocation]"
                                    + "%n, %n, %n, %n, %n, %s",
                                    oBusinessLocation.BusinessLocationID, oBusinessLocation.BusinessUnitID, oBusinessLocation.LocationID, (int)eEnumDBOperation, nUserID, sBusinessLocationIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessLocation WHERE BusinessLocationID=%n", nID);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessLocation");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM BusinessLocation where BusinessUnitID=%n ", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
