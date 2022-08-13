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
    public class ELEncashDA
    {
        public ELEncashDA() { }

        #region Insert Update Delete Function
        public static IDataReader RollBackEncashedEL(TransactionContext tc, string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_RollBackEncasedEL] %s,%d,%n", sELEncashIDs, dtDeclarationDate, nUserID);
        }
        public static IDataReader ApproveEncashedEL(TransactionContext tc, string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_ApproveEncasedEL] %s,%d,%n", sELEncashIDs, dtDeclarationDate, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
