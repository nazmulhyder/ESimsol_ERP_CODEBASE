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
	public class FNProductionDA 
	{
		#region Insert Update Delete Function
        
        public static IDataReader InsertUpdate(TransactionContext tc, FNProduction oFNProduction, int eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNProduction]"
                                    + "%n,%n,%n,%D,%n,%n,%n, %s,%s",
									oFNProduction.FNProductionID,oFNProduction.FNMachineID,oFNProduction.FNTPID,oFNProduction.IssueDate,nUserID, eEnumDBOperation ,(int)oFNProduction.FNDyeingType,
                                    oFNProduction.StartDateTimeSt, oFNProduction.EndDateTimeSt);
		}

		public static void Delete(TransactionContext tc, FNProduction oFNProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNProduction]"
                                    + "%n,%n,%n,%D,%n,%n,%n, %s,%s",
                                    oFNProduction.FNProductionID, oFNProduction.FNMachineID, oFNProduction.FNTPID, oFNProduction.IssueDate, nUserID, eEnumDBOperation, (int)oFNProduction.FNDyeingType,
                                    oFNProduction.StartDateTimeSt, oFNProduction.EndDateTimeSt);
        }

       
        public static void Run(TransactionContext tc, FNProduction oFNProduction)
        {
            tc.ExecuteNonQuery("Update FNProduction SET StartDateTime=%D WHERE FNProductionID=%n",oFNProduction.StartDateTime, oFNProduction.FNProductionID);
        }
        public static void RunOut(TransactionContext tc, FNProduction oFNProduction,  Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNProduction]"
                                 + "%n,%n,%n,%D,%n,%n,%n, %s,%s",
                                 oFNProduction.FNProductionID, oFNProduction.FNMachineID, oFNProduction.FNTPID, oFNProduction.IssueDate, nUserID, (int)EnumDBOperation.Request, (int)oFNProduction.FNDyeingType,
                                 oFNProduction.StartDateTimeSt, oFNProduction.EndDateTimeSt);
            //tc.ExecuteNonQuery("Update FNProductionBatch SET EndDateTime = %D WHERE FNProductionID = %n AND EndDateTime=null", oFNProduction.EndDateTime, oFNProduction.FNProductionID);

            //tc.ExecuteNonQuery("Update FNProduction SET EndDateTime=%D WHERE FNProductionID=%n", oFNProduction.EndDateTime, oFNProduction.FNProductionID);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FNProduction WHERE FNProductionID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProduction");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
