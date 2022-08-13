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
    public class PFSchemeBenefitDA
    {
        public PFSchemeBenefitDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, PFSchemeBenefit oPFSB, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PFSchemeBenefit]"
                                    + "%n,%n,%n,%n,%n,%b,%b,%n,%n", oPFSB.PFSBID, oPFSB.PFSchemeID, oPFSB.MaturityYear, (int)oPFSB.MaturityYrCalculateAfter, oPFSB.ContributionPercentage, oPFSB.IsProfitShare, oPFSB.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nPFSBID)
        {
            return tc.ExecuteReader("SELECT * FROM PFSchemeBenefit WHERE PFSBID=%n", nPFSBID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
