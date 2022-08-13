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
    public class MailReportingDA
    {
        public MailReportingDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, MailReporting oMailReporting, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MailReporting]"
                                    + "%n, %s, %s, %s, %b, %b, %n, %n", oMailReporting.ReportID, oMailReporting.Name, oMailReporting.ControllerName, oMailReporting.FunctionName, oMailReporting.IsActive, oMailReporting.IsMail, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nReportID)
        {
            return tc.ExecuteReader("SELECT * FROM MailReporting WHERE ReportID=%n", nReportID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
