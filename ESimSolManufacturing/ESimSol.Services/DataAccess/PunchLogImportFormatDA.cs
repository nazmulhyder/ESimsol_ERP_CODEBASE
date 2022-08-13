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
    public class PunchLogImportFormatDA
    {
        public PunchLogImportFormatDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PunchLogImportFormat oPunchLogImportFormat, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PunchLogImportFormat] %n,%n",
                   oPunchLogImportFormat.PLIFID, oPunchLogImportFormat.PunchFormat
                  );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PunchLogImportFormat");
        }

        #endregion
    }
}
