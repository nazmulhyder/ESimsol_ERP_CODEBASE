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
    public class AdjustmentRequisitionSlipDA
    {
        public AdjustmentRequisitionSlipDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AdjustmentRequisitionSlip oARS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AdjustmentRequisitionSlip]" + "%n, %s, %D, %n, %n, %D, %s, %n, %n, %n, %n",
                                    oARS.AdjustmentRequisitionSlipID, oARS.ARSlipNo, oARS.Date, oARS.BUID,  oARS.RequestedByID, oARS.RequestedTime, oARS.Note, oARS.AdjustmentTypeInt, oARS.InoutTypeInInt, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, AdjustmentRequisitionSlip oARS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AdjustmentRequisitionSlip]" + "%n, %s, %D, %n, %n, %D, %s, %n, %n, %n, %n",
                                    oARS.AdjustmentRequisitionSlipID, oARS.ARSlipNo, oARS.Date, oARS.BUID, oARS.RequestedByID, oARS.RequestedTime, oARS.Note, oARS.AdjustmentTypeInt, oARS.InoutTypeInInt, nUserID, (int)eEnumDBOperation);
        }

        public static void UpdateVoucherEffect(TransactionContext tc, AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            tc.ExecuteNonQuery(" Update AdjustmentRequisitionSlip SET IsWillVoucherEffect = %b WHERE AdjustmentRequisitionSlipID  = %n", oAdjustmentRequisitionSlip.IsWillVoucherEffect, oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID);
        }  
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AdjustmentRequisitionSlip WHERE AdjustmentRequisitionSlipID=%n", nID);
        }       
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
