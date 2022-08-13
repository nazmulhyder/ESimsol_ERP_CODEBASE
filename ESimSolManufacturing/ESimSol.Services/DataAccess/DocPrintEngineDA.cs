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
	public class DocPrintEngineDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, DocPrintEngine oDocPrintEngine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_DocPrintEngine]"
									+"%n,%n,%s,%s,%s, %s,%n,  %n,%n,%n",
                                    oDocPrintEngine.DocPrintEngineID, oDocPrintEngine.LetterTypeInt, oDocPrintEngine.PageSize, oDocPrintEngine.Margin, oDocPrintEngine.FontName, oDocPrintEngine.LetterName, oDocPrintEngine.ModuleID, oDocPrintEngine.BUID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, DocPrintEngine oDocPrintEngine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_DocPrintEngine]"
                                    + "%n,%n,%s,%s,%s, %s,%n,  %n,%n,%n",
                                    oDocPrintEngine.DocPrintEngineID, oDocPrintEngine.LetterTypeInt, oDocPrintEngine.PageSize, oDocPrintEngine.Margin, oDocPrintEngine.FontName, oDocPrintEngine.LetterName, oDocPrintEngine.ModuleID, oDocPrintEngine.BUID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM [View_DocPrintEngine] WHERE DocPrintEngineID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM [View_DocPrintEngine]");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion

        public static IDataReader Update(TransactionContext tc, int nDocPrintEngineID, long nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DocPrintEngine Set Activity=~Activity WHERE DocPrintEngineID=%n", nDocPrintEngineID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM [View_DocPrintEngine] WHERE DocPrintEngineID=%n", nDocPrintEngineID);
        }

        internal static IDataReader GetActiveByType(TransactionContext tc, int type)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM [View_DocPrintEngine] WHERE Activity=1 AND LetterType=%n", type);
        }
        internal static IDataReader GetActiveByTypenModule(TransactionContext tc, int type, int nModuleID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM [View_DocPrintEngine] WHERE Activity=1 AND LetterType=%n AND ModuleID=%n", type, nModuleID);
        }
    }

}
