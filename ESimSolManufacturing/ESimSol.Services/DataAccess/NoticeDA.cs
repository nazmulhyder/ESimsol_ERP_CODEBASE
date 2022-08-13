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
    public class NoticeDA
    {
        public NoticeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, Notice oNotice, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Notice] %n, %s, %s, %d, %d, %b, %n, %n",
                                   oNotice.NoticeID, oNotice.Title, oNotice.Description, oNotice.IssueDate, oNotice.ExpireDate, oNotice.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nNoticeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Notice WHERE NoticeID=%n", nNoticeID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

      
        #endregion
    }
}
