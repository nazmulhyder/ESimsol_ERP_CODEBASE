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
    public class DyeingSolutionDA
    {
        public DyeingSolutionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, DyeingSolution oDyeingSolution, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingSolution] %n,%s,%n,%s,%s,%s,%n,%n",
                   oDyeingSolution.DyeingSolutionID, oDyeingSolution.Code,
                   oDyeingSolution.DyeingSolutionType, oDyeingSolution.Name,
                   oDyeingSolution.Description,oDyeingSolution.AdviseBy, nUserID, nDBOperation); 
            
        }

        public static IDataReader Copy(TransactionContext tc, int nDyeingSolutionID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingSolutionTemplet_Copy] %n,%n", nDyeingSolutionID, nUserID);
        }

        
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDyeingSolutionID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingSolution WHERE DyeingSolutionID=%n", nDyeingSolutionID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DyeingSolution");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
