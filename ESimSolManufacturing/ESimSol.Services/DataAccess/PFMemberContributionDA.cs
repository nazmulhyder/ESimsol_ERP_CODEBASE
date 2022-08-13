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
    public class PFMemberContributionDA
    {
        public PFMemberContributionDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, PFMemberContribution oPFMC, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PFMemberContribution]"
                                    + "%n,%n,%n,%n,%b,%n,%n,%b,%n,%n", oPFMC.PFMCID, oPFMC.PFSchemeID, oPFMC.MinAmount, oPFMC.MaxAmount, oPFMC.IsPercent, oPFMC.Value, (int)oPFMC.CalculationOn, oPFMC.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nPFMCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PFMemberContribution WHERE PFMCID=%n", nPFMCID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
