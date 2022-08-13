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
	public class MarketingAccount_BUDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, MarketingAccount_BU oMarketingAccount_BU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_MarketingAccount_BU]"
									+"%n,%n,%n,%n,%n",
									oMarketingAccount_BU.MarketingAccount_BUID,oMarketingAccount_BU.BUID,oMarketingAccount_BU.MarketingAccountID,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, MarketingAccount_BU oMarketingAccount_BU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_MarketingAccount_BU]"
                                    + "%n,%n,%n,%n,%n",
                                    oMarketingAccount_BU.MarketingAccount_BUID, oMarketingAccount_BU.BUID, oMarketingAccount_BU.MarketingAccountID, nUserID, (int)eEnumDBOperation);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_MarketingAccount_BU WHERE MarketingAccount_BUID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_MarketingAccount_BU WHERE MarketingAccountID=%n", nID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
