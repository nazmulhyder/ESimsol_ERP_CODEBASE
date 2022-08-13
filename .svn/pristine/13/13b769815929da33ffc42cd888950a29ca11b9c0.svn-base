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
    public class EmployeeProductionReceiveDetailDA
    {
        public EmployeeProductionReceiveDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeProductionReceiveDetail] %n,%n,%n,%d,%n,%n",
                   oEmployeeProductionReceiveDetail.EPSRDID, oEmployeeProductionReceiveDetail.EPSID,
                   oEmployeeProductionReceiveDetail.RcvQty, oEmployeeProductionReceiveDetail.RcvByDate,
                   nUserID, nDBOperation);
        }


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nEPSRDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProductionReceiveDetail WHERE EPSRDID=%n", nEPSRDID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeProductionReceiveDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader AdvanceEdit(EmployeeProductionReceiveDetail EPRD, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE EmployeeProductionReceiveDetail SET RcvQty = %n, RcvByDate = %d WHERE EPSRDID= %n", EPRD.RcvQty, EPRD.RcvByDate, EPRD.EPSRDID);
        }

        #endregion
    }
}
