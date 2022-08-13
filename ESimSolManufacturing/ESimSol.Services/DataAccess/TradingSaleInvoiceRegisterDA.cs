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
    public class TradingSaleInvoiceRegisterDA
    {
        public static IDataReader Gets(TransactionContext tc, string sSQL, int ReportLayout)
        {
            return tc.ExecuteReader("EXEC [SP_TradingSaleInvoiceRegister]" + " %s,%n", sSQL, ReportLayout);
        }
    }
}
