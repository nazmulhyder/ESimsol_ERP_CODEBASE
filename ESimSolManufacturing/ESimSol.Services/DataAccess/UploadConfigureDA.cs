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
	public class UploadConfigureDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, UploadConfigure oUploadConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_UploadConfigure]"
									+"%n,%n,%s,%s,%n,%n",
									oUploadConfigure.UploadConfigureID,oUploadConfigure.UserID,oUploadConfigure.FieldNames,oUploadConfigure.CaptionNames,(int)oUploadConfigure.UploadType, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, UploadConfigure oUploadConfigure, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_UploadConfigure]"
                                    + "%n,%n,%s,%s,%n,%n",
                                    oUploadConfigure.UploadConfigureID, oUploadConfigure.UserID, oUploadConfigure.FieldNames, oUploadConfigure.CaptionNames, (int)oUploadConfigure.UploadType, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nUploadType, long nUserID)
		{
            return tc.ExecuteReader("SELECT top 1 * FROM UploadConfigure WHERE UserID=%n  AND UploadType = %n", nUserID, nUploadType);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM UploadConfigure");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
