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
	public class DocPrintEngineDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, DocPrintEngineDetail oDocPrintEngineDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_DocPrintEngineDetail]"
									+"%n,%n,%s,%s,%s,%s,%s,%n,%s,%n,%n",
									oDocPrintEngineDetail.DocPrintEngineDetailID,oDocPrintEngineDetail.DocPrintEngineID,oDocPrintEngineDetail.SLNo,oDocPrintEngineDetail.SetWidths,oDocPrintEngineDetail.SetAligns,oDocPrintEngineDetail.SetFields,oDocPrintEngineDetail.FontSize,oDocPrintEngineDetail.RowHeight,oDocPrintEngineDetail.TableName,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, DocPrintEngineDetail oDocPrintEngineDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_DocPrintEngineDetail]"
									+"%n,%n,%s,%s,%s,%s,%s,%n,%s,%n,%n",
									oDocPrintEngineDetail.DocPrintEngineDetailID,oDocPrintEngineDetail.DocPrintEngineID,oDocPrintEngineDetail.SLNo,oDocPrintEngineDetail.SetWidths,oDocPrintEngineDetail.SetAligns,oDocPrintEngineDetail.SetFields,oDocPrintEngineDetail.FontSize,oDocPrintEngineDetail.RowHeight,oDocPrintEngineDetail.TableName,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM DocPrintEngineDetail WHERE DocPrintEngineDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM DocPrintEngineDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
