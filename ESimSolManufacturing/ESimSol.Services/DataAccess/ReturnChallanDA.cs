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
    public class ReturnChallanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ReturnChallan oReturnChallan, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ReturnChallan]"
                                   + "%n,%s,%n,%d,%n,%n,%n,%s,%n,%n,%n,%n",
                                   oReturnChallan.ReturnChallanID, oReturnChallan.ReturnChallanNo, oReturnChallan.ExportSCID, oReturnChallan.ReturnDate, oReturnChallan.ContractorID, oReturnChallan.BUID, oReturnChallan.ProductNatureInInt, oReturnChallan.Note, oReturnChallan.ApprovedBy, oReturnChallan.WorkingUnitID, nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, ReturnChallan oReturnChallan, short nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReturnChallan]"
                                   + "%n,%s,%n,%d,%n,%n,%n,%s,%n,%n,%n,%n",
                                   oReturnChallan.ReturnChallanID, oReturnChallan.ReturnChallanNo, oReturnChallan.ExportSCID, oReturnChallan.ReturnDate, oReturnChallan.ContractorID, oReturnChallan.BUID, oReturnChallan.ProductNatureInInt, oReturnChallan.Note, oReturnChallan.ApprovedBy, oReturnChallan.WorkingUnitID, nUserID, nDBOperation);
        }
        public static IDataReader Receive(TransactionContext tc, int nReturnChallanID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitReturnChallan]"
                                 + "%n,%n",nReturnChallanID, nUserID);

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nReturnChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReturnChallan WHERE ReturnChallanID=%n", nReturnChallanID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
