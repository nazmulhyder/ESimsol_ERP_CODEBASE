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
    public class DUReturnChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUReturnChallanDetail oDUReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUReturnChallanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oDUReturnChallanDetail.DUReturnChallanDetailID, oDUReturnChallanDetail.DUReturnChallanID, oDUReturnChallanDetail.DUDeliveryChallanDetailID, oDUReturnChallanDetail.LotID, oDUReturnChallanDetail.ProductID, oDUReturnChallanDetail.MUnitID, oDUReturnChallanDetail.PTUID, oDUReturnChallanDetail.Qty, oDUReturnChallanDetail.Note, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

        public static void Delete(TransactionContext tc, DUReturnChallanDetail oDUReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUReturnChallanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oDUReturnChallanDetail.DUReturnChallanDetailID, oDUReturnChallanDetail.DUReturnChallanID, oDUReturnChallanDetail.DUDeliveryChallanDetailID, oDUReturnChallanDetail.LotID, oDUReturnChallanDetail.ProductID, oDUReturnChallanDetail.MUnitID, oDUReturnChallanDetail.PTUID, oDUReturnChallanDetail.Qty, oDUReturnChallanDetail.Note, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(int nDOID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID = %n", nDOID);
        }
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
