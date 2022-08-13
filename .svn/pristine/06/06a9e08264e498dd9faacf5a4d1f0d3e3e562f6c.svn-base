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
    public class CandidateReferenceDA
    {
        public CandidateReferenceDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CandidateReference oCandidateReference, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CandidateReference] %n,%n,%u,%u,%u,%u,%u,%u,%u,%u,%d,%n",
                   oCandidateReference.CRefID, oCandidateReference.CandidateID, oCandidateReference.Name,
                   oCandidateReference.Organization, oCandidateReference.Department, oCandidateReference.Designation,
                   oCandidateReference.ContactNo, oCandidateReference.Email, oCandidateReference.Address,
                   oCandidateReference.Relation,oCandidateReference.LastUpdatedDate, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCRefID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateReference WHERE CRefID=%n", nCRefID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(int nCID,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CandidateReference WHERE CandidateID=%n", nCID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
