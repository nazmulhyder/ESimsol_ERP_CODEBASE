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
    public class RSInQCDetailDA
    {
        public RSInQCDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RSInQCDetail oRSInQCDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RSInQCDetail] %n,%n,%n,%n,%s,%n,%n,%n",
                   oRSInQCDetail.RSInQCDetailID, oRSInQCDetail.RouteSheetID, oRSInQCDetail.RSInQCSetupID, oRSInQCDetail.Qty , oRSInQCDetail.Note, oRSInQCDetail.DyeingOrderDetailID, nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nRSInQCDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSInQCDetail WHERE RSInQCDetailID=%n", nRSInQCDetailID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
