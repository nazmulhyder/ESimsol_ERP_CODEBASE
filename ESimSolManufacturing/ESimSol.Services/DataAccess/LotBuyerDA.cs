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
    public class LotBuyerDA
    {
        public LotBuyerDA() { }

        public static IDataReader Gets(string sSQL, int ReportType, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_LotBuyer] " + " %s , %n " ,sSQL,ReportType);
        }
    }
}
