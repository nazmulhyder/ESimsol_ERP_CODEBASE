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
    public class ITaxAssessmentYearDA
    {
        public ITaxAssessmentYearDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxAssessmentYear oITaxAssessmentYear, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxAssessmentYear] %n,%u,%d,%d,%n,%n",
                   oITaxAssessmentYear.ITaxAssessmentYearID, oITaxAssessmentYear.Description,
                   oITaxAssessmentYear.StartDate, oITaxAssessmentYear.EndDate, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxAssessmentYearID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID=%n", nITaxAssessmentYearID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxAssessmentYear");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
