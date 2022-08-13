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
	public class KnitDyeingYarnRequisitionDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingYarnRequisition oKnitDyeingYarnRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingYarnRequisition]"
									+"%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID, oKnitDyeingYarnRequisition.KnitDyeingProgramID, oKnitDyeingYarnRequisition.YarnCountID, oKnitDyeingYarnRequisition.UsagesParcent, oKnitDyeingYarnRequisition.FabricTypeID, oKnitDyeingYarnRequisition.FinishRequiredQty, oKnitDyeingYarnRequisition.MUnitID, oKnitDyeingYarnRequisition.Remarks, nUserID, (int)eEnumDBOperation, sIDs);
		}

        public static void Delete(TransactionContext tc, KnitDyeingYarnRequisition oKnitDyeingYarnRequisition, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingYarnRequisition]"
									+"%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID, oKnitDyeingYarnRequisition.KnitDyeingProgramID, oKnitDyeingYarnRequisition.YarnCountID, oKnitDyeingYarnRequisition.UsagesParcent, oKnitDyeingYarnRequisition.FabricTypeID, oKnitDyeingYarnRequisition.FinishRequiredQty, oKnitDyeingYarnRequisition.MUnitID, oKnitDyeingYarnRequisition.Remarks, nUserID, (int)eEnumDBOperation, sIDs);
		}
        public static void CommitGrace(TransactionContext tc, KnitDyeingYarnRequisition oKnitDyeingYarnRequisition)
        {
            tc.ExecuteNonQuery("UPDATE KnitDyeingYarnRequisition SET GracePercent = %n, RequiredQty = %n WHERE KnitDyeingYarnRequisitionID = %n", oKnitDyeingYarnRequisition.GracePercent, oKnitDyeingYarnRequisition.RequiredQty, oKnitDyeingYarnRequisition.KnitDyeingYarnRequisitionID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingYarnRequisition WHERE KnitDyeingYarnRequisitionID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingYarnRequisition");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingYarnRequisition WHERE KnitDyeingProgramID = %n Order By KnitDyeingYarnRequisitionID", id);
        }
        public static IDataReader GetsLog(TransactionContext tc, int Logid)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingYarnRequisitionLog WHERE KnitDyeingProgramLogID = %n Order By KnitDyeingYarnRequisitionLogID", Logid);
        } 
        

		#endregion
	}

}
