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
	public class KnitDyeingProgramDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingProgram oKnitDyeingProgram, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingProgram]"
									+"%n,%n, %n,%n, %s,%d,%n,%n,%n,%n,%n,%n,%d,%n,%s,%s,%s,%D,%s,%D,%s,%D,%s,%n,%n",
                                    oKnitDyeingProgram.KnitDyeingProgramID, oKnitDyeingProgram.BUID, oKnitDyeingProgram.ProgramTypeInt, oKnitDyeingProgram.RefTypeInt, oKnitDyeingProgram.RefNo, oKnitDyeingProgram.IssueDate, oKnitDyeingProgram.TechnicalSheetID, oKnitDyeingProgram.FabricID, oKnitDyeingProgram.OrderQty, oKnitDyeingProgram.MUnitID, oKnitDyeingProgram.MerchandiserID, oKnitDyeingProgram.DyedType, oKnitDyeingProgram.ShipmentDate, oKnitDyeingProgram.GSMID, oKnitDyeingProgram.SrinkageTollarance, oKnitDyeingProgram.Remarks, oKnitDyeingProgram.TermsAndCondition, oKnitDyeingProgram.TimeOfArrivalYarn, oKnitDyeingProgram.TimeOfArrivalYarnNote, oKnitDyeingProgram.TimeOfCompletionKnitting, oKnitDyeingProgram.TimeOfCompletionKnittingNote, oKnitDyeingProgram.TimeOfCompletionDying, oKnitDyeingProgram.TimeOfCompletionDyingNote, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingProgram oKnitDyeingProgram, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingProgram]"
                                    + "%n,%n, %n,%n, %s,%d,%n,%n,%n,%n,%n,%n,%d,%n,%s,%s,%s,%D,%s,%D,%s,%D,%s,%n,%n",
                                    oKnitDyeingProgram.KnitDyeingProgramID, oKnitDyeingProgram.BUID, oKnitDyeingProgram.ProgramTypeInt, oKnitDyeingProgram.RefTypeInt, oKnitDyeingProgram.RefNo, oKnitDyeingProgram.IssueDate, oKnitDyeingProgram.TechnicalSheetID, oKnitDyeingProgram.FabricID, oKnitDyeingProgram.OrderQty, oKnitDyeingProgram.MUnitID, oKnitDyeingProgram.MerchandiserID, oKnitDyeingProgram.DyedType, oKnitDyeingProgram.ShipmentDate, oKnitDyeingProgram.GSMID, oKnitDyeingProgram.SrinkageTollarance, oKnitDyeingProgram.Remarks, oKnitDyeingProgram.TermsAndCondition, oKnitDyeingProgram.TimeOfArrivalYarn, oKnitDyeingProgram.TimeOfArrivalYarnNote, oKnitDyeingProgram.TimeOfCompletionKnitting, oKnitDyeingProgram.TimeOfCompletionKnittingNote, oKnitDyeingProgram.TimeOfCompletionDying, oKnitDyeingProgram.TimeOfCompletionDyingNote, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ProductionStart(TransactionContext tc, KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_KnitDyeingProgram_ProductionStart]"
                                   + "%n,%n",oKnitDyeingProgram.KnitDyeingProgramID, nUserID);
        }
        public static void SendToFactory(TransactionContext tc, KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update KnitDyeingProgram SET KnitDyeingProgramStatus = " + (int)EnumKnitDyeingProgramStatus.SendToFactory + " WHERE KnitDyeingProgramID =" + oKnitDyeingProgram.KnitDyeingProgramID);
        }
        //
        public static IDataReader AcceptRevise(TransactionContext tc, KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptReviseKnitDyeingProgram]"
                                   + "%n,%n, %n,%n, %s,%d,%n,%n,%n,%n,%n,%n,%d,%n,%s,%s,%s,%D,%s,%D,%s,%D,%s,%n",
                                   oKnitDyeingProgram.KnitDyeingProgramID, oKnitDyeingProgram.BUID, oKnitDyeingProgram.ProgramTypeInt, oKnitDyeingProgram.RefTypeInt, oKnitDyeingProgram.RefNo, oKnitDyeingProgram.IssueDate, oKnitDyeingProgram.TechnicalSheetID, oKnitDyeingProgram.FabricID, oKnitDyeingProgram.OrderQty, oKnitDyeingProgram.MUnitID, oKnitDyeingProgram.MerchandiserID, oKnitDyeingProgram.DyedType, oKnitDyeingProgram.ShipmentDate, oKnitDyeingProgram.GSMID, oKnitDyeingProgram.SrinkageTollarance, oKnitDyeingProgram.Remarks, oKnitDyeingProgram.TermsAndCondition, oKnitDyeingProgram.TimeOfArrivalYarn, oKnitDyeingProgram.TimeOfArrivalYarnNote, oKnitDyeingProgram.TimeOfCompletionKnitting, oKnitDyeingProgram.TimeOfCompletionKnittingNote, oKnitDyeingProgram.TimeOfCompletionDying, oKnitDyeingProgram.TimeOfCompletionDyingNote, nUserID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgram WHERE KnitDyeingProgramID=%n", nID);
		}

        public static IDataReader GetLog(TransactionContext tc, long nLogID)
		{
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgramLog WHERE KnitDyeingProgramLogID=%n", nLogID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingProgram");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}

