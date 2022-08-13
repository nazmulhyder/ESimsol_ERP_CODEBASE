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
	public class KnitDyeingProgramDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingProgramDetail oKDPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID,string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingProgramDetail]"
									+"%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%s,%s,%n,%n,%s,%n, %n,%n,%s",
                                    oKDPDetail.KnitDyeingProgramDetailID, oKDPDetail.KnitDyeingProgramID, oKDPDetail.RefObjectID, oKDPDetail.RefProgramNo,  oKDPDetail.ColorID, oKDPDetail.GarmentsQty,oKDPDetail.GarmentsMUnitID, oKDPDetail.FabricTypeID, oKDPDetail.GSMID, oKDPDetail.CompositionID, oKDPDetail.ConsumptionPerDzn, oKDPDetail.FinishDiaID, oKDPDetail.PantoneNo, oKDPDetail.ApprovedShade, oKDPDetail.ShadeRecipe, oKDPDetail.ShadeRemarks, oKDPDetail.ReqFinishFabricQty, oKDPDetail.MUnitID,oKDPDetail.Remarks, oKDPDetail.ConsumPtionMUnitID,  nUserID, (int)eEnumDBOperation, sIDs);
		}

        public static void Delete(TransactionContext tc, KnitDyeingProgramDetail oKDPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingProgramDetail]"
                                    + "%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%s,%s,%n,%n,%s,%n, %n,%n,%s",
                                    oKDPDetail.KnitDyeingProgramDetailID, oKDPDetail.KnitDyeingProgramID, oKDPDetail.RefObjectID, oKDPDetail.RefProgramNo, oKDPDetail.ColorID, oKDPDetail.GarmentsQty, oKDPDetail.GarmentsMUnitID, oKDPDetail.FabricTypeID, oKDPDetail.GSMID, oKDPDetail.CompositionID, oKDPDetail.ConsumptionPerDzn, oKDPDetail.FinishDiaID, oKDPDetail.PantoneNo, oKDPDetail.ApprovedShade, oKDPDetail.ShadeRecipe, oKDPDetail.ShadeRemarks, oKDPDetail.ReqFinishFabricQty, oKDPDetail.MUnitID, oKDPDetail.Remarks, oKDPDetail.ConsumPtionMUnitID, nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static void CommitGrace(TransactionContext tc, KnitDyeingProgramDetail oKDPDetail)
        {
            tc.ExecuteNonQuery("UPDATE KnitDyeingProgramDetail SET GracePercent = %n, ReqFabricQty = %n WHERE KnitDyeingProgramDetailID = %n", oKDPDetail.GracePercent, oKDPDetail.ReqFabricQty, oKDPDetail.KnitDyeingProgramDetailID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgramDetail WHERE KnitDyeingProgramDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingProgramDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgramDetail WHERE KnitDyeingProgramID = %n Order By KnitDyeingProgramDetailID", id);
        }

        public static IDataReader GetsLog(TransactionContext tc, int LogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingProgramDetailLog WHERE KnitDyeingProgramLogID = %n Order By KnitDyeingProgramDetailLogID", LogID);
        } 
		#endregion
	}

}
