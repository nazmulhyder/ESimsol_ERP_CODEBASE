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
	public class LotParentDA
    {
        #region SP
        public static IDataReader InsertUpdate(TransactionContext tc, LotParent oLotParent, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LotParent]"
                                   + "%n,%n,%n,  %n,%n,%n,  %s,%n,%n, %n,%n,%n,%n",
                                    oLotParent.LotParentID, oLotParent.ParentType, oLotParent.LotID, 
                                    oLotParent.Qty,     oLotParent.UnitPrice, oLotParent.CurrencyID, 
                                    oLotParent.Note,    oLotParent.DyeingOrderID, oLotParent.ProductID, 
                                    oLotParent.Balance, oLotParent.DyeingOrderID_Out, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, LotParent oLotParent, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LotParent]"
                                    + "%n,%n,%n,  %n,%n,%n,  %s,%n,%n, %n,%n,%n,%n",
                                    oLotParent.LotParentID, oLotParent.ParentType, oLotParent.LotID,
                                    oLotParent.Qty, oLotParent.UnitPrice, oLotParent.CurrencyID,
                                    oLotParent.Note, oLotParent.DyeingOrderID, oLotParent.ProductID,
                                    oLotParent.Balance, oLotParent.DyeingOrderID_Out, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Lot_Adjustment(TransactionContext tc, LotParent oLotParent, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUProGuideLine_Adjustment]"
                                  + "%n,%n,%n,%n,%n,    %n,%n,%n,%n",
                                   oLotParent.LotParentID, oLotParent.DUProGuidelineDetailID, oLotParent.DUProGuidelineDetailID_Out, oLotParent.DyeingOrderID, oLotParent.LotID, oLotParent.Qty, oLotParent.WorkingUnitID, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LotParent WHERE LotParentID=%n", nID);
		}
        public static IDataReader GetsBy(TransactionContext tc, int nDUPScheduleID)
		{
            return tc.ExecuteReader("SELECT * FROM View_LotParent where DUPScheduleID=%n", nDUPScheduleID);
		}
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LotParent");
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
