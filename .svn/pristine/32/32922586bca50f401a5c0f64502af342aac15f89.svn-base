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
    public class DevelopmentRecapDetailDA
    {
        public DevelopmentRecapDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DevelopmentRecapDetail oDevelopmentRecapDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DevelopmentRecapDetail]"
                                    + "%n, %n,  %n, %n,%d, %n, %n, %n,%b, %s, %n, %n, %s",
                                    oDevelopmentRecapDetail.DevelopmentRecapDetailID, oDevelopmentRecapDetail.DevelopmentRecapID, oDevelopmentRecapDetail.FactoryID,oDevelopmentRecapDetail.FactoryPersonID,oDevelopmentRecapDetail.SeekingDate,oDevelopmentRecapDetail.ReceivedBy,oDevelopmentRecapDetail.UnitID,oDevelopmentRecapDetail.SampleQty, (bool)oDevelopmentRecapDetail.IsRawmaterialProvide, oDevelopmentRecapDetail.Note, nUserID, (int)eEnumDBOperation, "");
        }


        public static void Delete(TransactionContext tc, DevelopmentRecapDetail oDevelopmentRecapDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDevelopmentRecapDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DevelopmentRecapDetail]"
                                    + "%n, %n,  %n, %n,%d, %n, %n, %n,%b, %s, %n, %n, %s",
                                    oDevelopmentRecapDetail.DevelopmentRecapDetailID, oDevelopmentRecapDetail.DevelopmentRecapID, oDevelopmentRecapDetail.FactoryID, oDevelopmentRecapDetail.FactoryPersonID, oDevelopmentRecapDetail.SeekingDate, oDevelopmentRecapDetail.ReceivedBy, oDevelopmentRecapDetail.UnitID, oDevelopmentRecapDetail.SampleQty, (bool)oDevelopmentRecapDetail.IsRawmaterialProvide, oDevelopmentRecapDetail.Note, nUserID, (int)eEnumDBOperation, sDevelopmentRecapDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapDetail WHERE DevelopmentRecapID=%n", id);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapDetail WHERE DevelopmentRecapDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nDRID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DevelopmentRecapDetail WHERE DevelopmentRecapID=%n", nDRID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
