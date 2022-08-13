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
	public class KnitDyeingPTUDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingPTU oKnitDyeingPTU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingPTU]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
									oKnitDyeingPTU.KnitDyeingPTUID,oKnitDyeingPTU.KnitDyeingProgramDetailID,oKnitDyeingPTU.ColorID,oKnitDyeingPTU.GarmentsQty,oKnitDyeingPTU.GarmentsMUnitID,oKnitDyeingPTU.FabricTypeID,oKnitDyeingPTU.GSMID,oKnitDyeingPTU.CompositionID,oKnitDyeingPTU.PantoneNo,oKnitDyeingPTU.ShadeRecipe,oKnitDyeingPTU.ReqFabricQty,oKnitDyeingPTU.MUnitID,oKnitDyeingPTU.KnitYarnBookQty,oKnitDyeingPTU.KnitYarnIssueQty,oKnitDyeingPTU.KnitPipeLineQty,oKnitDyeingPTU.KnitProcessLossQty,oKnitDyeingPTU.KnitRejectQty,oKnitDyeingPTU.GrayFabricRcvQty,oKnitDyeingPTU.DyeingIssueQty,oKnitDyeingPTU.DyeingPipeLineQty,oKnitDyeingPTU.ReDyeingQty,oKnitDyeingPTU.DyeingGainLossQty,oKnitDyeingPTU.DyeingFinishQty,oKnitDyeingPTU.ReFinishingQty,oKnitDyeingPTU.FinishingGainLossQty,oKnitDyeingPTU.FinishingQty,oKnitDyeingPTU.ChallanQty,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingPTU oKnitDyeingPTU, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingPTU]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
									oKnitDyeingPTU.KnitDyeingPTUID,oKnitDyeingPTU.KnitDyeingProgramDetailID,oKnitDyeingPTU.ColorID,oKnitDyeingPTU.GarmentsQty,oKnitDyeingPTU.GarmentsMUnitID,oKnitDyeingPTU.FabricTypeID,oKnitDyeingPTU.GSMID,oKnitDyeingPTU.CompositionID,oKnitDyeingPTU.PantoneNo,oKnitDyeingPTU.ShadeRecipe,oKnitDyeingPTU.ReqFabricQty,oKnitDyeingPTU.MUnitID,oKnitDyeingPTU.KnitYarnBookQty,oKnitDyeingPTU.KnitYarnIssueQty,oKnitDyeingPTU.KnitPipeLineQty,oKnitDyeingPTU.KnitProcessLossQty,oKnitDyeingPTU.KnitRejectQty,oKnitDyeingPTU.GrayFabricRcvQty,oKnitDyeingPTU.DyeingIssueQty,oKnitDyeingPTU.DyeingPipeLineQty,oKnitDyeingPTU.ReDyeingQty,oKnitDyeingPTU.DyeingGainLossQty,oKnitDyeingPTU.DyeingFinishQty,oKnitDyeingPTU.ReFinishingQty,oKnitDyeingPTU.FinishingGainLossQty,oKnitDyeingPTU.FinishingQty,oKnitDyeingPTU.ChallanQty,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingPTU WHERE KnitDyeingPTUID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingPTU");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
