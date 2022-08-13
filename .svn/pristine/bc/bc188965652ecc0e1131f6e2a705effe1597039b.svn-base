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
    public class SuperAdminDA
    {
        public SuperAdminDA() { }


        public static IDataReader MakeDayoffHoliday(TransactionContext tc, string sDate, string eDate, bool isComp, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_SuperAdmin_MakeDayOffHoliday]" + "%d, %d, %b, %n"
                , sDate
                , eDate
                , isComp
                , nUserID
            );
        }
    }
}


