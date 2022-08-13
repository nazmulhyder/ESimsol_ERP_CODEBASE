using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class QCRegisterDA
    {
        #region Get & Exist Function       
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByQCFollowUp(TransactionContext tc, QCRegister oQCRegister)
        {
            return tc.ExecuteReader("EXEC[SP_QCRegister]" + "%n, %d, %d, %s", oQCRegister.BUID, oQCRegister.ConsumptionStartDate, oQCRegister.ConsumptionEndDate, oQCRegister.SheetNo);
        }
        
        #endregion
    }
}
