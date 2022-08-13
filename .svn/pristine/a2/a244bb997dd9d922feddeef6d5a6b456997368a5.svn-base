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
    public class SampleRequestDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, SampleRequest oSampleRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleRequest]"
                                   + "%n,%n,%d,%s,%n,%n,%n ,%n,%n,%n, %s, %n,%n",
                                   oSampleRequest.SampleRequestID, oSampleRequest.BUID, oSampleRequest.RequestDate, oSampleRequest.RequestNo, oSampleRequest.RequestBy, oSampleRequest.RequestTo, oSampleRequest.RequestTypeInt, oSampleRequest.ContractorID, oSampleRequest.ContactPersonID, oSampleRequest.WorkingUnitID, oSampleRequest.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, SampleRequest oSampleRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleRequest]"
                                   + "%n,%n,%d,%s,%n,%n,%n ,%n,%n,%n, %s, %n,%n",
                                   oSampleRequest.SampleRequestID, oSampleRequest.BUID, oSampleRequest.RequestDate, oSampleRequest.RequestNo, oSampleRequest.RequestBy, oSampleRequest.RequestTo, oSampleRequest.RequestTypeInt, oSampleRequest.ContractorID, oSampleRequest.ContactPersonID, oSampleRequest.WorkingUnitID, oSampleRequest.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static void Approved(TransactionContext tc, SampleRequest oSampleRequest, int nUserID)
        {
            tc.ExecuteNonQuery("UPDATE SampleRequest SET ApprovedBy =%n WHERE SampleRequestID =%n", nUserID, oSampleRequest.SampleRequestID);
        }
        public static void UndoApproved(TransactionContext tc, SampleRequest oSampleRequest, int nUserID)
        {
            tc.ExecuteNonQuery("UPDATE SampleRequest SET ApprovedBy =0 WHERE SampleRequestID =%n", oSampleRequest.SampleRequestID);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequest WHERE SampleRequestID=%n", nID);
        }
        public static IDataReader Gets( TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleRequest " );
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
