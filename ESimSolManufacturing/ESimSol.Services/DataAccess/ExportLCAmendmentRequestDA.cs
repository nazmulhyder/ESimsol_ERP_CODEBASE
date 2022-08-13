using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportLCAmendmentRequestDA
    {
        public ExportLCAmendmentRequestDA() { }

        #region Insert Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportLCAmendmentRequest oExportLCAmendmentRequest, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportLCAmendmentRequest]"
                                    + " %n,%n,%D,%n,%n,%n",
                                    oExportLCAmendmentRequest.ExportLCAmendmentRequestID,
                                    oExportLCAmendmentRequest.ExportLCID,
                                    oExportLCAmendmentRequest.DateOfRequest,
                                    nUserId,
                                    nUserId,
                                    (int)eEnumDBPurchaseLC);

        }
        #endregion

      

        #region Delete Function
        public static void Delete(TransactionContext tc, ExportLCAmendmentRequest oExportLCAmendmentRequest, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportLCAmendmentRequest]"
                                    + " %n,%n,%D,%n,%n,%n",
                                    oExportLCAmendmentRequest.ExportLCAmendmentRequestID,
                                    oExportLCAmendmentRequest.ExportLCID,
                                    oExportLCAmendmentRequest.DateOfRequest,
                                    nUserId,
                                    nUserId,
                                    (int)eEnumDBPurchaseLC);
        }
     
        #endregion

        #region Generation Function
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {

            return tc.ExecuteReader("SELECT * FROM ExportLCAmendmentRequest WHERE ExportLCAmendmentRequestID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPLCID)
        {
            return tc.ExecuteReader("SELECT * FROM ExportLCAmendmentRequest WHERE ExportLCID=%n", nPLCID);
        }
        public static int GetSequenceNo(TransactionContext tc, int nPLCID)
        {
            object MaxSNo = tc.ExecuteScalar("SELECT isnull(MAX(Sequence),0)+1 FROM ExportLCAmendmentRequest Where ExportLCID=%n", nPLCID); 
            //if(MaxRSNo){return 1}//no need to execute for only once in the life time of this table-when there will no data
            int Temp = Convert.ToInt32(MaxSNo);
            return Temp;
        }
        #endregion
    }


}
