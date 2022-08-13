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
    public class CompanyRuleDescriptionDA
    {
        public CompanyRuleDescriptionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CompanyRuleDescription oCompanyRuleDescription, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CompanyRuleDescription] %n,%n,%u,%b,%n,%n",
                   oCompanyRuleDescription.CRDID,oCompanyRuleDescription.CRNID, oCompanyRuleDescription.Description, 
                   oCompanyRuleDescription.IsActive,
                   nUserID, nDBOperation);
        }

        public static IDataReader Activity(int nCRDID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE CompanyRuleDescription SET IsActive=~IsActive WHERE CRDID=%n;SELECT * FROM View_CompanyRuleDescription WHERE CRDID=%n", nCRDID, nCRDID);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCRDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompanyRuleDescription WHERE CRDID=%n", nCRDID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CompanyRuleDescription");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
