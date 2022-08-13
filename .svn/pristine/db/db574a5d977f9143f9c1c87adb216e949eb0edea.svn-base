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
    public class PFmemberDA
    {
        public PFmemberDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, PFmember oPFM, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PFmember]"
                                    + "%n,%n,%n,%s,%n,%n,%b,%d,%n,%n", oPFM.PFMID, oPFM.PFSchemeID, oPFM.EmployeeID, oPFM.Description, oPFM.PFBalance, oPFM.RequestTo, oPFM.IsActive, oPFM.PFMembershipDate, nUserID, nDBOperation);
        }

        public static IDataReader UploadXL(TransactionContext tc, PFmember oPFmember, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_PFMember] %s,%s,%n,%n",
                   oPFmember.EmployeeCode,
                   oPFmember.PFSchemeName,
                   oPFmember.PFBalance,
                   nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nPFMID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PFmember WHERE PFMID=%n", nPFMID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
