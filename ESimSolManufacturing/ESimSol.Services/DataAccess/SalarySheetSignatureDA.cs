using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;


namespace ESimSol.Services.DataAccess
{
    public class SalarySheetSignatureDA
    {
        public SalarySheetSignatureDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, SalarySheetSignature oSalarySheetSignature, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalarySheetSignature] %n,%s,%n",oSalarySheetSignature.SignatureID, oSalarySheetSignature.SignatureName, nDBOperation);

        }
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc,int nId)
        {
            return tc.ExecuteReader("SELECT * FROM SalarySheetSignature WHERE SignatureID=%n", nId);
        }
        public static IDataReader Gets(TransactionContext tc,string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
