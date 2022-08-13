using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class PISizerBreakDownDA
    {
        public PISizerBreakDownDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PISizerBreakDown oPISizerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PISizerBreakDown]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%s",
                                    oPISizerBreakDown.PISizerBreakDownID, oPISizerBreakDown.ExportPIID, oPISizerBreakDown.ProductID, oPISizerBreakDown.ColorID, oPISizerBreakDown.SizeID,  oPISizerBreakDown.Quantity,oPISizerBreakDown.Model, oPISizerBreakDown.StyleNo, oPISizerBreakDown.PantonNo, oPISizerBreakDown.Remarks,   (int)eEnumDBOperation, nUserID, "");
        }

        public static void Delete(TransactionContext tc, PISizerBreakDown oPISizerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPISizerBreakDownIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PISizerBreakDown]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%s",
                                    oPISizerBreakDown.PISizerBreakDownID, oPISizerBreakDown.ExportPIID, oPISizerBreakDown.ProductID, oPISizerBreakDown.ColorID, oPISizerBreakDown.SizeID, oPISizerBreakDown.Quantity, oPISizerBreakDown.Model, oPISizerBreakDown.StyleNo, oPISizerBreakDown.PantonNo, oPISizerBreakDown.Remarks, (int)eEnumDBOperation, nUserID, sPISizerBreakDownIDs);
        }


        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PISizerBreakDown WHERE PISizerBreakDownID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PISizerBreakDown");
        }

        public static IDataReader Gets(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PISizerBreakDown WHERE ExportPIID =%n Order By ProductID, PISizerBreakDownID", nExportPIID);
        }

        public static IDataReader GetsByLog(TransactionContext tc, int nExportPILogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PISizerBreakDownLog WHERE ExportPILogID =%n Order By ProductID, PISizerBreakDownLogID", nExportPILogID);
        }

        //
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }





        #endregion
    }  

}
