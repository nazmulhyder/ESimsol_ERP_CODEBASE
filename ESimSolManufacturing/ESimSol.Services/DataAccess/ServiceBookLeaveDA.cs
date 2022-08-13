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
    public class ServiceBookLeaveDA
    {
        public ServiceBookLeaveDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(int nEmployeeID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_ServiceBookLeave] %n", nEmployeeID);
        }
        #endregion
    }
}
