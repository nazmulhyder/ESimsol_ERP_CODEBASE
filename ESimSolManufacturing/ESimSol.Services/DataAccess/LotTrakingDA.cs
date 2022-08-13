using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;


namespace ESimSol.Services.DataAccess
{
    public class LotTrakingDA
    {
        public LotTrakingDA() { }


        #region Update Function

        public static IDataReader Gets_WU(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nTParentType)
        {
            return tc.ExecuteReader("EXEC [sp_LotTraking_WU ]" + "%D, %D, %n,%n", dStartDate, dEndDate, nBUID, nTParentType);
        }
        public static IDataReader Gets_Product(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID,int nTParentType)
        {
            return tc.ExecuteReader("EXEC [sp_LotTraking_Product]" + "%D, %D, %n,%n,%n", dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType);
        }
        public static IDataReader Gets_Lot(TransactionContext tc, int nBUID,string sLotIDs)
        {
            return tc.ExecuteReader("EXEC [sp_LotTracking]" + "%n,%s", nBUID,sLotIDs);
        }
      

        #endregion

        
 
    }
}