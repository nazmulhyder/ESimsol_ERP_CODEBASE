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
    public class PFSchemeDA
    {
        public PFSchemeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, PFScheme oPFScheme, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PFScheme]"
                                    + "%n,%s,%s,%b,%n,%b,%n,%n", oPFScheme.PFSchemeID, oPFScheme.Name, oPFScheme.Description, oPFScheme.IsRecognize, oPFScheme.RecommandedSalaryHeadID, oPFScheme.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nPFSchemeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PFScheme WHERE PFSchemeID=%n", nPFSchemeID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
