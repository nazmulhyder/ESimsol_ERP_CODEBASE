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
    public class CandidateEducationDA
    {
        public CandidateEducationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CandidateEducation oCandidateEducation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CandidateEducation] %n,%n,%u,%u,%u,%d,%u,%u,%u,%u,%d,%n",
                   oCandidateEducation.CEID, oCandidateEducation.CandidateID, oCandidateEducation.Degree,
                   oCandidateEducation.Major, oCandidateEducation.Session, oCandidateEducation.PassingYear, 
                   oCandidateEducation.BoardUniversity, oCandidateEducation.Institution, oCandidateEducation.Result, 
                   oCandidateEducation.Country, oCandidateEducation.LastUpdatedDate, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCEID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateEducation WHERE CEID=%n", nCEID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int nCID,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateEducation WHERE CandidateID =%n", nCID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
