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
	public class CommercialBSDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CommercialBS oCommercialBS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            return tc.ExecuteReader("EXEC [SP_IUD_CommercialBS]"
                                   + "%n,%n,%s,%d, %n,%n,%n,%n,%n,%s,%n,%n,%n",
                                   oCommercialBS.CommercialBSID, oCommercialBS.MasterLCID, oCommercialBS.RefNo, oCommercialBS.IssueDate, oCommercialBS.BUID, oCommercialBS.BuyerID, oCommercialBS.BSStatus, oCommercialBS.BankID, oCommercialBS.BSAmount, oCommercialBS.Remarks, oCommercialBS.BankBranchID, nUserID, (int)eEnumDBOperation);
        }

		public static void Delete(TransactionContext tc, CommercialBS oCommercialBS, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialBS]"
                                    + "%n,%n,%s,%d, %n,%n,%n,%n,%n,%s,%n,%n,%n",
                                    oCommercialBS.CommercialBSID, oCommercialBS.MasterLCID, oCommercialBS.RefNo, oCommercialBS.IssueDate,  oCommercialBS.BUID, oCommercialBS.BuyerID, oCommercialBS.BSStatus, oCommercialBS.BankID, oCommercialBS.BSAmount, oCommercialBS.Remarks, oCommercialBS.BankBranchID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader ChangeStatus(TransactionContext tc, CommercialBS oCommercialBS, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommercialBSOperation]"
                                   + "%n,%n,%s,%d,%d,%s,%s, %n,%n,%n,%n",
                                   oCommercialBS.CommercialBSID, (int)oCommercialBS.BSStatus, oCommercialBS.Remarks, oCommercialBS.DynamicDate, oCommercialBS.MaturityRcvDate, oCommercialBS.FDBPNo, oCommercialBS.BankRefNo, oCommercialBS.CRate, oCommercialBS.BSAmountBC, oCommercialBS.ActionType,nUserID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CommercialBS WHERE CommercialBSID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM CommercialBS");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
