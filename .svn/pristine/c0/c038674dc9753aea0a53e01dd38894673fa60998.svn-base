using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
	public class FAScheduleDA 
	{
		#region Insert Update Delete Function

		#endregion

		#region Get & Exist Function
		public static IDataReader Gets(TransactionContext tc, long nID, long nUserID)
		{
            return tc.ExecuteReader("EXEC [SP_FARegisterProcess]"
                                    +"%n, %n",
                                    nID, nUserID);
		}
        public static IDataReader GetsLogScheduleBy(TransactionContext tc, long nFARLogID, long nUserID)
		{
            return tc.ExecuteReader("EXEC [SP_FARegisterLogProcess]"
                                    +"%n, %n",
                                    nFARLogID, nUserID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FASchedule");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader Gets(TransactionContext tc, double DateYear, int nReportLayout)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_FAShcedule] %n,%n", DateYear, nReportLayout);
        }
        public static IDataReader SaveFASchedules(TransactionContext tc, long nID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FASchedule]"
                                    + "%n, %n",
                                    nID, nUserID);
        }
		#endregion
	}

}
