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
    public class SampleRequestDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, SampleRequestDetail oSampleRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSampleRequestDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleRequestDetail]"
                                   + "%n,%n,%n,%n,%n, %n,%n,%s,   %n,%n,%s",
                                   oSampleRequestDetail.SampleRequestDetailID, oSampleRequestDetail.SampleRequestID, oSampleRequestDetail.ProductID, oSampleRequestDetail.ColorCategoryID, oSampleRequestDetail.MUnitID,oSampleRequestDetail.LotID, oSampleRequestDetail.Quantity,oSampleRequestDetail.Remarks, nUserID, (int)eEnumDBOperation, sSampleRequestDetailIDs);
        }

        public static void Delete(TransactionContext tc, SampleRequestDetail oSampleRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSampleRequestDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleRequestDetail]"
                                   + "%n,%n,%n,%n,%n, %n,%n,%s,   %n,%n,%s",
                                   oSampleRequestDetail.SampleRequestDetailID, oSampleRequestDetail.SampleRequestID, oSampleRequestDetail.ProductID, oSampleRequestDetail.ColorCategoryID, oSampleRequestDetail.MUnitID,oSampleRequestDetail.LotID, oSampleRequestDetail.Quantity, oSampleRequestDetail.Remarks, nUserID, (int)eEnumDBOperation, sSampleRequestDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequestDetail WHERE SampleRequestDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSampleRequestID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequestDetail WHERE SampleRequestID =%n", nSampleRequestID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
