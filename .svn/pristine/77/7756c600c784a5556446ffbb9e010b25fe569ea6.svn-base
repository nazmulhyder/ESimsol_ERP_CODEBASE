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
    public class RSInQCSetupDA
    {
        public RSInQCSetupDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RSInQCSetup oRSInQCSetup, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RSInQCSetup] %n,%s,%n,%n,%n,%b,%n,%n",
                   oRSInQCSetup.RSInQCSetupID, oRSInQCSetup.Name, oRSInQCSetup.YarnType, oRSInQCSetup.LocationID, oRSInQCSetup.WorkingUnitID, oRSInQCSetup.Activity, nUserID, nDBOperation);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nRSInQCSetupID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSInQCSetup WHERE RSInQCSetupID=%n", nRSInQCSetupID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsBy(int nYarnType, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSInQCSetup WHERE Activity=1 and YarnType=%n", nYarnType);
        }

        #endregion
    }
}
