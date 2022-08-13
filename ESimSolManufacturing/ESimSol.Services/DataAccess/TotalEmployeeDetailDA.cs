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
    public class TotalEmployeeDetailDA
    {
        public TotalEmployeeDetailDA() { }

        #region Get

        public static IDataReader Gets(DateTime StartDate,DateTime EndDate,TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Get_TotalEmployeeDetail] %d,%d"
                                    ,StartDate, EndDate);
        }

        #endregion
    }
}
