using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class EmailConfigDA
    {
         public EmailConfigDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EmailConfig oEmailConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmailConfig]" + "%n, %n, %s, %s, %s, %s, %s, %s, %b, %n, %n",
                                    oEmailConfig.EmailConfigID, oEmailConfig.BUID, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.PortNumber, oEmailConfig.HostName, oEmailConfig.Remarks, oEmailConfig.EmailDisplayName, oEmailConfig.SSLRequired, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EmailConfig oEmailConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EmailConfig]" + "%n, %n, %s, %s, %s, %s, %s, %s, %b, %n, %n",
                                    oEmailConfig.EmailConfigID, oEmailConfig.BUID, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.PortNumber, oEmailConfig.HostName, oEmailConfig.Remarks, oEmailConfig.EmailDisplayName, oEmailConfig.SSLRequired, nUserID, (int)eEnumDBOperation);
        }

       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmailConfig WHERE EmailConfigID=%n", nID);
        }
        public static IDataReader GetByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmailConfig WHERE BUID=%n", nBUID);
        }      
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmailConfig ORDER BY BUName ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

       #endregion
    }
}
