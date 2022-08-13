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
    public class RSInQCManageDA
    {
        public RSInQCManageDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RSInQCManage oRSInQCManage, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RSInQCManage] %n,%n,%n,%n",
                   oRSInQCManage.RSInQCDetailID, oRSInQCManage.WorkingUnitID, nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nRSInQCManageID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RSInQCManage WHERE RSInQCManageID=%n", nRSInQCManageID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
