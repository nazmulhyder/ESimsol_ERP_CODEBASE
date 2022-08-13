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
    public class CandidateTrainingDA
    {
        public CandidateTrainingDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CandidateTraining oCandidateTraining, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CandidateTraining] %n,%n,%u,%u,%d,%d,%n,%d,%u,%u,%u,%u,%d,%n",
                   oCandidateTraining.CTID, oCandidateTraining.CandidateID, oCandidateTraining.CourseName,
                   oCandidateTraining.Specification, oCandidateTraining.StartDate, oCandidateTraining.EndDate,
                   oCandidateTraining.Duration, oCandidateTraining.PassingDate, oCandidateTraining.Result,
                   oCandidateTraining.Institution, oCandidateTraining.CertifyBodyVendor, oCandidateTraining.Country, 
                   oCandidateTraining.LastUpdatedDate, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCTID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateTraining WHERE CTID=%n", nCTID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int nCID,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateTraining WHERE CandidateID=%n", nCID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
