using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class AdjustmentRequisitionSlipDetailDA
    {
        public AdjustmentRequisitionSlipDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AdjustmentRequisitionSlipDetail oARSD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sAdjustmentRequisitionSlipDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AdjustmentRequisitionSlipDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                       oARSD.AdjustmentRequisitionSlipDetailID, oARSD.AdjustmentRequisitionSlipID, oARSD.Qty, oARSD.LotID, oARSD.Note, nUserID, (int)eEnumDBOperation, sAdjustmentRequisitionSlipDetaillIDs);
        }

        public static void Delete(TransactionContext tc, AdjustmentRequisitionSlipDetail oARSD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sAdjustmentRequisitionSlipDetaillIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AdjustmentRequisitionSlipDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                       oARSD.AdjustmentRequisitionSlipDetailID, oARSD.AdjustmentRequisitionSlipID, oARSD.Qty, oARSD.LotID, oARSD.Note, nUserID, (int)eEnumDBOperation, sAdjustmentRequisitionSlipDetaillIDs);
        }
        #endregion

        #region Get & Exist Function      
        public static IDataReader Get( TransactionContext tc,int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AdjustmentRequisitionSlipDetail WHERE AdjustmentRequisitionSlipDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nAdjustmentRequisitionSlipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AdjustmentRequisitionSlipDetail WHERE AdjustmentRequisitionSlipID=%n", nAdjustmentRequisitionSlipID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
