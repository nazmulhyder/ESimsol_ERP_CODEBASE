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
    public class RouteSheetCancelDA
    {
        public RouteSheetCancelDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RouteSheetCancel oRouteSheetCancel, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetCancel] %n,%b,%n,%s,%s,%n,%n,%n",
                   oRouteSheetCancel.RouteSheetID, oRouteSheetCancel.IsNewLot, oRouteSheetCancel.WorkingUnitID, oRouteSheetCancel.Remarks, oRouteSheetCancel.ApprovalRemarks, oRouteSheetCancel.ReDyeingStatus, nUserID, nDBOperation);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nRouteSheetID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetCancel WHERE RouteSheetID=%n", nRouteSheetID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
