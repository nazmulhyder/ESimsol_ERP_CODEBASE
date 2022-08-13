using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class POSizerBreakDownDA
    {
        public POSizerBreakDownDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, POSizerBreakDown oPOSizerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_POSizerBreakDown]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%s", 
                                    oPOSizerBreakDown.POSizerBreakDownID, oPOSizerBreakDown.ProductionOrderID, oPOSizerBreakDown.ProductID, oPOSizerBreakDown.ColorID, oPOSizerBreakDown.SizeID, oPOSizerBreakDown.Quantity, oPOSizerBreakDown.Model, oPOSizerBreakDown.StyleNo, oPOSizerBreakDown.PantonNo, oPOSizerBreakDown.Remarks, (int)eEnumDBOperation, nUserID, "");
        }

        public static void Delete(TransactionContext tc, POSizerBreakDown oPOSizerBreakDown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sPOSizerBreakDownIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_POSizerBreakDown]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%s", 
                                    oPOSizerBreakDown.POSizerBreakDownID, oPOSizerBreakDown.ProductionOrderID, oPOSizerBreakDown.ProductID, oPOSizerBreakDown.ColorID, oPOSizerBreakDown.SizeID, oPOSizerBreakDown.Quantity, oPOSizerBreakDown.Model, oPOSizerBreakDown.StyleNo, oPOSizerBreakDown.PantonNo, oPOSizerBreakDown.Remarks, (int)eEnumDBOperation, nUserID, sPOSizerBreakDownIDs);
        }

        #endregion

       

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_POSizerBreakDown WHERE POSizerBreakDownID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_POSizerBreakDown");
        }

        public static IDataReader Gets(TransactionContext tc, int nProductionOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_POSizerBreakDown WHERE ProductionOrderID =%n Order By ProductID,POSizerBreakDownID", nProductionOrderID);
        }
        public static IDataReader GetsByLog(TransactionContext tc, int nProductionOrderLogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_POSizerBreakDownLog WHERE ProductionOrderLogID =%n Order By ProductID,POSizerBreakDownLogID", nProductionOrderLogID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  

}
