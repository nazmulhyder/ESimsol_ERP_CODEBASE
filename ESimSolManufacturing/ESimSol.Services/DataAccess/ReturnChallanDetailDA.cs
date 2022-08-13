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
    public class ReturnChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ReturnChallanDetail oReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ReturnChallanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oReturnChallanDetail.ReturnChallanDetailID, oReturnChallanDetail.ReturnChallanID, oReturnChallanDetail.DeliveryChallanID,  oReturnChallanDetail.DeliveryChallanDetailID,    oReturnChallanDetail.ProductID, oReturnChallanDetail.MUnitID, oReturnChallanDetail.Qty,  oReturnChallanDetail.Note, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

        public static void Delete(TransactionContext tc, ReturnChallanDetail oReturnChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReturnChallanDetail]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                   oReturnChallanDetail.ReturnChallanDetailID, oReturnChallanDetail.ReturnChallanID, oReturnChallanDetail.DeliveryChallanID, oReturnChallanDetail.DeliveryChallanDetailID, oReturnChallanDetail.ProductID, oReturnChallanDetail.MUnitID, oReturnChallanDetail.Qty, oReturnChallanDetail.Note, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReturnChallanDetail WHERE ReturnChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(int nDOID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReturnChallanDetail WHERE ReturnChallanID = %n", nDOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
