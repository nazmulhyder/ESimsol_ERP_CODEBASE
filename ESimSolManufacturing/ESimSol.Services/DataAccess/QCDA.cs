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

    public class QCDA
    {
        public QCDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, QC oQC, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_Commit_QC]"
                                    + "%n, %n, %n, %n, %n, %n,%n,%n,%n, %n",
                                    oQC.QCID, oQC.ProductionSheetID, oQC.PassQuantity, oQC.RejectQuantity, oQC.ProductID, oQC.WorkingUnitID,   oQC.CartonQty, oQC.PerCartonFGQty, oQC.LotID,  nUserId);
        }

        public static void Delete(TransactionContext tc, QC oQC, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_Commit_QC]"
                                    + "%n, %n, %n, %n, %n, %n,%n,%n,%n, %n",
                                    oQC.QCID, oQC.ProductionSheetID, oQC.PassQuantity, oQC.RejectQuantity, oQC.ProductID, oQC.WorkingUnitID, oQC.CartonQty, oQC.PerCartonFGQty, oQC.LotID, nUserId);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_QC WHERE QCID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_QC ORDER BY QCID");
        }
        public static IDataReader Gets(TransactionContext tc, int nPSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_QC WHERE ProductionSheetID=%n ORDER BY QCID", nPSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
