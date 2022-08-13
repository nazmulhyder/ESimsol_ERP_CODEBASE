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
	public class KnittingYarnReturnDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnittingYarnReturn oKnittingYarnReturn, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnReturn]"
									+"%n,%n,%s,%d,%s,%s,%n,%n,%n",
									oKnittingYarnReturn.KnittingYarnReturnID,oKnittingYarnReturn.KnittingOrderID,oKnittingYarnReturn.ReturnNo,oKnittingYarnReturn.ReturnDate,oKnittingYarnReturn.PartyChallanNo,oKnittingYarnReturn.Remarks,oKnittingYarnReturn.ApprovedBy,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnittingYarnReturn oKnittingYarnReturn, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingYarnReturn]"
									+"%n,%n,%s,%d,%s,%s,%n,%n,%n",
									oKnittingYarnReturn.KnittingYarnReturnID,oKnittingYarnReturn.KnittingOrderID,oKnittingYarnReturn.ReturnNo,oKnittingYarnReturn.ReturnDate,oKnittingYarnReturn.PartyChallanNo,oKnittingYarnReturn.Remarks,oKnittingYarnReturn.ApprovedBy,nUserID, (int)eEnumDBOperation);
		}

        public static IDataReader Approve(TransactionContext tc, KnittingYarnReturn oKnittingYarnReturn, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnReturn]"
                                   + "%n,%n,%s,%d,%s,%s,%n,%n,%n",
                                   oKnittingYarnReturn.KnittingYarnReturnID, oKnittingYarnReturn.KnittingOrderID, oKnittingYarnReturn.ReturnNo, oKnittingYarnReturn.ReturnDate, oKnittingYarnReturn.PartyChallanNo, oKnittingYarnReturn.Remarks, oKnittingYarnReturn.ApprovedBy, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnittingYarnReturn WHERE KnittingYarnReturnID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnittingYarnReturn");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
