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
    public class AttendanceAccessPointDA
    {
        public AttendanceAccessPointDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceAccessPoint oAAP, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceAccessPoint] %n, %s, %s, %s, %n, %s, %s, %s, %s, %b, %b,%s, %n, %n",
                                   oAAP.AAPID, oAAP.Name, oAAP.MachineSLNo, oAAP.Note, oAAP.DataProvider, oAAP.DataSource, oAAP.DBLoginID, oAAP.DBPassword, oAAP.DBName, oAAP.IsThisPC, oAAP.IsActive,oAAP.Query, nUserID, nDBOperation);

              
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nAAPID)
        {
            return tc.ExecuteReader("SELECT * FROM AttendanceAccessPoint WHERE AAPID=%n", nAAPID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
