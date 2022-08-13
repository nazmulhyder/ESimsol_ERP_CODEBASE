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
    public class TechnicalSheetSizeDA
    {
        public TechnicalSheetSizeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheetSize oTechnicalSheetSize, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheetSize]"
                                    + "%n, %n, %n, %s, %n,%n, %n, %n",
                                    oTechnicalSheetSize.TechnicalSheetSizeID, oTechnicalSheetSize.TechnicalSheetID, oTechnicalSheetSize.SizeCategoryID, oTechnicalSheetSize.Note, oTechnicalSheetSize.Sequence, oTechnicalSheetSize.QtyInPercent,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TechnicalSheetSize oTechnicalSheetSize, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheetSize]"
                                    + "%n, %n, %n, %s, %n,%n, %n, %n",
                                    oTechnicalSheetSize.TechnicalSheetSizeID, oTechnicalSheetSize.TechnicalSheetID, oTechnicalSheetSize.SizeCategoryID, oTechnicalSheetSize.Note, oTechnicalSheetSize.Sequence, oTechnicalSheetSize.QtyInPercent, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetSize WHERE TechnicalSheetSizeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetSize");
        }
        public static IDataReader Gets(TransactionContext tc, int nTSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetSize WHERE TechnicalSheetID=%n ORDER BY Sequence", nTSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
