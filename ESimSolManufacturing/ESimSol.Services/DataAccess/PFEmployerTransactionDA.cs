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
    public class PFEmployerTransactionDA
    {
        public PFEmployerTransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, PFEmployerTransaction oPFET, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PFEmployerTransaction]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n", oPFET.PETID, oPFET.Year, oPFET.Month, oPFET.PETType, oPFET.Amount,oPFET.Note, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Distribute(TransactionContext tc, int EnumDBOp, int nPFID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_PFDistribute]"
                                    + "%n,%n, %n", EnumDBOp, nPFID, nUserID);
        }

        public static IDataReader Get(TransactionContext tc, long nPETID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PFEmployerTransaction WHERE PETID=%n", nPETID);
        }

        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
