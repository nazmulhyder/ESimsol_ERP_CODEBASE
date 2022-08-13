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
    public class MailSetUpDA
    {
        public MailSetUpDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, MailSetUp oMailSetUp, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MailSetUp]"
                                    + "%n,%n,%s,%n,%D,%D,%b,%b,%n,%n,%n", oMailSetUp.MSID, oMailSetUp.ReportID, oMailSetUp.Subject, (int)oMailSetUp.MailType, oMailSetUp.MailTime, oMailSetUp.LastMailTime, oMailSetUp.IsActive, oMailSetUp.IsMailSend, oMailSetUp.ModuleTypeInt, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nMSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MailSetUp WHERE MSID=%n", nMSID);
        }
        public static IDataReader GetByModule(TransactionContext tc, long nModuleID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MailSetUp WHERE ModuleType=%n", nModuleID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
