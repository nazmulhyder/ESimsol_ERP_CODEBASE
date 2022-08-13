using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class KnittingYarnChallanDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingYarnChallanDetail oKnittingYarnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingYarnChallanDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnChallanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%s",
                                   oKnittingYarnChallanDetail.KnittingYarnChallanDetailID, oKnittingYarnChallanDetail.KnittingYarnChallanID, oKnittingYarnChallanDetail.KnittingOrderDetailID, oKnittingYarnChallanDetail.KnittingCompositionID, oKnittingYarnChallanDetail.IssueStoreID, oKnittingYarnChallanDetail.YarnID, oKnittingYarnChallanDetail.LotID, oKnittingYarnChallanDetail.MUnitID, oKnittingYarnChallanDetail.Qty, oKnittingYarnChallanDetail.Remarks, oKnittingYarnChallanDetail.BrandName, oKnittingYarnChallanDetail.ColorID, oKnittingYarnChallanDetail.BagQty, nUserID, (int)eEnumDBOperation, sKnittingYarnChallanDetailIDs);
        }

        public static void Delete(TransactionContext tc, KnittingYarnChallanDetail oKnittingYarnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnittingYarnChallanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingYarnChallanDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%s",
                                    oKnittingYarnChallanDetail.KnittingYarnChallanDetailID, oKnittingYarnChallanDetail.KnittingYarnChallanID, oKnittingYarnChallanDetail.KnittingOrderDetailID, oKnittingYarnChallanDetail.KnittingCompositionID, oKnittingYarnChallanDetail.IssueStoreID, oKnittingYarnChallanDetail.YarnID, oKnittingYarnChallanDetail.LotID, oKnittingYarnChallanDetail.MUnitID, oKnittingYarnChallanDetail.Qty, oKnittingYarnChallanDetail.Remarks, oKnittingYarnChallanDetail.BrandName, oKnittingYarnChallanDetail.ColorID, oKnittingYarnChallanDetail.BagQty, nUserID, (int)eEnumDBOperation, sKnittingYarnChallanDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingYarnChallanDetail WHERE KnittingYarnChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM KnittingYarnChallanDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
