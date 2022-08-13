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
    public class CompanyRuleNameDA
    {
        public CompanyRuleNameDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CompanyRuleName oCompanyRuleName, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CompanyRuleName] %n,%u,%b,%n,%n",
                   oCompanyRuleName.CRNID, oCompanyRuleName.Description, oCompanyRuleName.IsActive,
                   nUserID, nDBOperation);


        }

        public static IDataReader Activity(int nCRNID, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE CompanyRuleName SET IsActive=~IsActive WHERE CRNID=%n;SELECT * FROM CompanyRuleName WHERE CRNID=%n", nCRNID, nCRNID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCRNID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyRuleName WHERE CRNID=%n", nCRNID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CompanyRuleName");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
