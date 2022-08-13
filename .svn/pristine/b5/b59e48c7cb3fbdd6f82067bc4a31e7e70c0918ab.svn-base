using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SignatureSetupDA
    {
        public SignatureSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SignatureSetup oSignatureSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SignatureSetup]" + "%n, %n, %s, %n, %s, %s, %n, %n",
                                    oSignatureSetup.SignatureSetupID, oSignatureSetup.ReportModuleInt, oSignatureSetup.SignatureCaption, oSignatureSetup.SignatureSequence, oSignatureSetup.DisplayDataColumn, oSignatureSetup.DisplayFixedName, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, SignatureSetup oSignatureSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SignatureSetup]" + "%n, %n, %s, %n, %s, %s, %n, %n",
                                    oSignatureSetup.SignatureSetupID, oSignatureSetup.ReportModuleInt, oSignatureSetup.SignatureCaption, oSignatureSetup.SignatureSequence, oSignatureSetup.DisplayDataColumn, oSignatureSetup.DisplayFixedName, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM SignatureSetup WHERE SignatureSetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM SignatureSetup ORDER BY ReportModule, SignatureSequence ASC");
        }
        public static IDataReader GetsByReportModule(TransactionContext tc, EnumReportModule eReportModule)
        {
            return tc.ExecuteReader("SELECT * FROM SignatureSetup WHERE ReportModule=%n ORDER BY ReportModule, SignatureSequence ASC", (int)eReportModule);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
