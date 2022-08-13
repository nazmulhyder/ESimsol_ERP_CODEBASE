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
    public class CandidateExperienceDA
    {
        public CandidateExperienceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CandidateExperience oCandidateExperience, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CandidateExperience] %n,%n,%u,%u,%u,%u,%u,%d,%d,%u,%u,%d,%n",
                   oCandidateExperience.CExpID, oCandidateExperience.CandidateID, oCandidateExperience.Organization,
                   oCandidateExperience.OrganizationType, oCandidateExperience.Address, oCandidateExperience.Department,
                   oCandidateExperience.Designation, oCandidateExperience.StartDate, oCandidateExperience.EndDate,
                   oCandidateExperience.AreaOfExperience, oCandidateExperience.MajorResponsibility,
                   oCandidateExperience.LastUpdatedDate, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCExpID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateExperience WHERE CExpID=%n", nCExpID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int nCID,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateExperience WHERE CandidateID=%n", nCID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
